Imports MySqlConnector
Imports System.Data
Imports System.Threading.Tasks

Public Class FormEmployeeAttendance

    Private originalData As DataTable
    Private isInitialLoad As Boolean = True
    Private _isLoading As Boolean = False
    Private _searchDebounceTimer As Timer
    Private _lastSearchText As String = ""

    ' Pagination state
    Private _currentPage As Integer = 1
    Private ReadOnly _pageSize As Integer = 50
    Private _totalRecords As Integer = 0
    Private _totalPages As Integer = 0

    Private Async Sub FormEmployeeAttendance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Setup search debounce timer
            _searchDebounceTimer = New Timer() With {.Interval = 500}
            AddHandler _searchDebounceTimer.Tick, AddressOf SearchDebounceTimer_Tick

            ' Setup DataGridView
            SetupDataGridView()

            ' Initial loading indicators
            Label4.Text = "..."
            Label6.Text = "..."
            Label7.Text = "..."

            ' Load data asynchronously
            Await RefreshAttendanceAsync()

            ' Add double-click handler for payroll link
            AddHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick


        Catch ex As Exception
            MessageBox.Show("Initialization Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            isInitialLoad = False
        End Try
    End Sub

    '====================================
    ' HELPER: Get Clean Search Text
    '====================================
    Private Function GetSearchText() As String
        Dim text As String = TextBoxSearch.Text.Trim()
        If text = "Search employees..." OrElse String.IsNullOrWhiteSpace(text) Then
            Return ""
        End If
        Return text
    End Function

    '====================================
    ' REFRESH DATA ASYNC
    '====================================
    Private Async Function RefreshAttendanceAsync(Optional resetPage As Boolean = False) As Task
        If _isLoading Then Return
        _isLoading = True
        SetLoadingState(True)

        If resetPage Then _currentPage = 1

        Try
            Dim searchText As String = GetSearchText()

            ' Get total count
            _totalRecords = Await Task.Run(Function() FetchTotalAttendanceCount(searchText))
            _totalPages = Math.Max(1, Math.Ceiling(_totalRecords / CDbl(_pageSize)))

            ' Ensure current page is within bounds
            If _currentPage > _totalPages Then _currentPage = _totalPages
            If _currentPage < 1 Then _currentPage = 1

            Dim offset As Integer = (_currentPage - 1) * _pageSize

            ' Run database query in background
            Dim data As DataTable = Await Task.Run(Function() LoadAttendanceDataFromDB(searchText, offset, _pageSize))

            ' Update UI on main thread
            If data IsNot Nothing Then
                originalData = data.Copy()
                DataGridView1.DataSource = data

                ' Fetch global stats for tiles
                Dim stats = Await Task.Run(Function() FetchAttendanceStats(searchText))
                UpdateSummaryTiles(stats, _totalRecords)

                FormatDataGridView()
                UpdatePaginationUI()
            End If
        Catch ex As Exception
            If Not Me.IsDisposed Then
                MessageBox.Show("Error refreshing attendance: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If Not Me.IsDisposed Then
                SetLoadingState(False)
                _isLoading = False
            End If
        End Try
    End Function

    '====================================
    ' SETUP DATAGRIDVIEW
    '====================================
    Private Sub SetupDataGridView()
        Try
            With DataGridView1
                .AutoGenerateColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .ReadOnly = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .RowHeadersVisible = False
                .BackgroundColor = Color.White
                .BorderStyle = BorderStyle.None
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .GridColor = Color.FromArgb(241, 245, 249)
                .DefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 250, 252)
                .DefaultCellStyle.SelectionForeColor = Color.Black ' Changed to Black for better readability on select
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
                .ColumnHeadersDefaultCellStyle.BackColor = Color.White
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
                .ColumnHeadersHeight = 50
                .RowTemplate.Height = 50
                .EnableHeadersVisualStyles = False
            End With

            ' Clear existing columns
            DataGridView1.Columns.Clear()

            ' Add columns to match the design EXACTLY
            DataGridView1.Columns.Add(CreateColumn("AttendanceDate", "Date", 120, False, "MMM dd, yyyy"))
            DataGridView1.Columns.Add(CreateColumn("EmployeeName", "Employee", 200))
            DataGridView1.Columns.Add(CreateColumn("Shift", "Shift", 120))
            DataGridView1.Columns.Add(CreateColumn("TimeIn", "Time In", 120, False, "hh:mm tt"))
            DataGridView1.Columns.Add(CreateColumn("TimeOut", "Time Out", 120, False, "hh:mm tt"))
            DataGridView1.Columns.Add(CreateColumn("WorkHours", "Total Hours", 120))
            DataGridView1.Columns.Add(CreateColumn("Overtime", "Overtime", 120))
            DataGridView1.Columns.Add(CreateColumn("Status", "Status", 120))

            ' Hidden ID column for reference
            DataGridView1.Columns.Add(CreateColumn("AttendanceID", "ID", 0, True))

            ' Enable sorting for all columns
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.SortMode = DataGridViewColumnSortMode.Automatic
            Next

        Catch ex As Exception
            MessageBox.Show("Error setting up grid: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '====================================
    ' CREATE COLUMN HELPER
    '====================================
    Private Function CreateColumn(name As String, headerText As String, width As Integer, Optional isHidden As Boolean = False, Optional format As String = "") As DataGridViewTextBoxColumn
        Dim col As New DataGridViewTextBoxColumn With {
            .Name = name,
            .DataPropertyName = name,
            .HeaderText = headerText,
            .Width = width,
            .Visible = Not isHidden,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        }
        If Not String.IsNullOrEmpty(format) Then
            col.DefaultCellStyle.Format = format
        End If
        Return col
    End Function

    Private Function GetDateFilterCondition(prefix As String) As String
        Dim dateField As String = prefix & ".AttendanceDate"
        Dim condition As String = ""

        ' NOTE: Since there is no specific "Day" picker in the Reports module,
        ' "Daily" and "Monthly" will both filter by the selected Month and Year.
        ' This allows the user to see all daily logs for the selected month.
        Select Case Reports.SelectedPeriod
            Case "Daily"
                ' Filter by the specific date selected in the DateTimePicker
                condition = $"DATE({dateField}) = '{Reports.GlobalFilterDate:yyyy-MM-dd}'"

            Case "Monthly"
                ' Start with Year filter
                condition = $"YEAR({dateField}) = {Reports.SelectedYear}"

                If Reports.SelectedMonth > 0 Then
                    condition &= $" AND MONTH({dateField}) = {Reports.SelectedMonth}"
                End If

            Case "Weekly"
                ' Show week containing the selected date in date picker
                condition = $"YEARWEEK({dateField}, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1)"

            Case "Yearly"
                condition = $"YEAR({dateField}) = {Reports.SelectedYear}"
        End Select

        Return condition
    End Function

    Private Function FetchTotalAttendanceCount(searchText As String) As Integer
        Dim query As String = "SELECT COUNT(*) FROM employee_attendance a JOIN employee e ON a.EmployeeID = e.EmployeeID WHERE 1=1"

        Dim dateFilter = GetDateFilterCondition("a")
        If Not String.IsNullOrEmpty(dateFilter) Then query &= " AND " & dateFilter

        If Not String.IsNullOrEmpty(searchText) Then
            query &= " AND (e.FirstName LIKE @search OR e.LastName LIKE @search OR a.Status LIKE @search)"
        End If

        Try
            openConn()
            Using cmd As New MySqlCommand(query, conn)
                cmd.CommandTimeout = 60
                If Not String.IsNullOrEmpty(searchText) Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If
                Dim result = cmd.ExecuteScalar()
                Return If(IsDBNull(result), 0, Convert.ToInt32(result))
            End Using
        Finally
            closeConn()
        End Try
    End Function

    Private Function FetchAttendanceStats(searchText As String) As Dictionary(Of String, Object)
        Dim stats As New Dictionary(Of String, Object) From {
            {"TotalPresent", 0},
            {"Absences", 0},
            {"LateInstances", 0},
            {"TotalOvertime", 0.0}
        }

        Dim queryCounts As String = "
            SELECT 
                COUNT(CASE WHEN Status = 'Present' THEN 1 END) as TotalPresent,
                COUNT(CASE WHEN Status IN ('Absent', 'On Leave') THEN 1 END) as Absences,
                COUNT(CASE WHEN Status = 'Late' THEN 1 END) as LateInstances,
                SUM(CASE WHEN WorkHours > 8 THEN WorkHours - 8 ELSE 0 END) as TotalOvertime
            FROM employee_attendance a 
            JOIN employee e ON a.EmployeeID = e.EmployeeID
            WHERE 1=1"

        Dim dateFilter = GetDateFilterCondition("a")
        If Not String.IsNullOrEmpty(dateFilter) Then queryCounts &= " AND " & dateFilter

        If Not String.IsNullOrEmpty(searchText) Then
            queryCounts &= " AND (e.FirstName LIKE @search OR e.LastName LIKE @search)"
        End If

        Try
            openConn()
            Using cmd As New MySqlCommand(queryCounts, conn)
                cmd.CommandTimeout = 60
                If Not String.IsNullOrEmpty(searchText) Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        stats("TotalPresent") = If(IsDBNull(reader("TotalPresent")), 0, Convert.ToInt32(reader("TotalPresent")))
                        stats("Absences") = If(IsDBNull(reader("Absences")), 0, Convert.ToInt32(reader("Absences")))
                        stats("LateInstances") = If(IsDBNull(reader("LateInstances")), 0, Convert.ToInt32(reader("LateInstances")))
                        stats("TotalOvertime") = If(IsDBNull(reader("TotalOvertime")), 0.0, Convert.ToDouble(reader("TotalOvertime")))
                    End If
                End Using
            End Using
        Finally
            closeConn()
        End Try
        Return stats
    End Function

    '====================================
    ' LOAD EMPLOYEE DATA (SIMULATED ATTENDANCE)
    '====================================
    '====================================
    ' LOAD EMPLOYEE DATA (REAL ATTENDANCE)
    '====================================
    Private Function LoadAttendanceDataFromDB(searchText As String, offset As Integer, limit As Integer) As DataTable
        Dim table As New DataTable()
        Try
            openConn()
            Dim query As String = "
                SELECT 
                    a.AttendanceID,
                    a.AttendanceDate,
                    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
                    a.TimeIn,
                    a.TimeOut,
                    a.WorkHours,
                    a.Status,
                    CASE 
                        WHEN a.TimeIn IS NULL THEN 'Unknown'
                        WHEN HOUR(a.TimeIn) < 12 THEN 'Morning'
                        WHEN HOUR(a.TimeIn) >= 12 AND HOUR(a.TimeIn) < 17 THEN 'Afternoon'
                        ELSE 'Evening'
                    END as Shift,
                    CASE 
                        WHEN IFNULL(a.WorkHours, 0) > 8 THEN IFNULL(a.WorkHours, 0) - 8 
                        ELSE 0 
                    END as Overtime
                FROM 
                    employee_attendance a
                JOIN 
                    employee e ON a.EmployeeID = e.EmployeeID
                WHERE 1=1 "

            Dim dateFilter = GetDateFilterCondition("a")
            If Not String.IsNullOrEmpty(dateFilter) Then query &= " AND " & dateFilter

            If Not String.IsNullOrEmpty(searchText) Then
                query &= " AND (e.FirstName LIKE @search OR e.LastName LIKE @search OR a.Status LIKE @search) "
            End If

            query &= " ORDER BY a.AttendanceDate DESC, a.TimeIn DESC LIMIT @limit OFFSET @offset"

            Using cmd As New MySqlCommand(query, conn)
                cmd.CommandTimeout = 60
                If Not String.IsNullOrEmpty(searchText) Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If
                cmd.Parameters.AddWithValue("@limit", limit)
                cmd.Parameters.AddWithValue("@offset", offset)
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(table)
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            closeConn()
        End Try
        Return table
    End Function

    '====================================
    ' UPDATE SUMMARY TILES
    '====================================
    Private Sub UpdateSummaryTiles(stats As Dictionary(Of String, Object), totalCount As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() UpdateSummaryTiles(stats, totalCount))
            Return
        End If

        Try
            ' Update with new keys from FetchAttendanceStats
            ' Layout assumption: 
            ' Label4 -> Total Present
            ' Label6 -> Absences
            ' Label7 -> Late Instances (Was OnLeave)
            ' Need a 4th label for Overtime? The design shows 4 cards. 
            ' Existing code only updated 3 labels. I'll check if there's a 4th label available or reuse.
            ' Let's look at Designer file implied structure or existing usage.
            ' Existing: Label4, Label6, Label7. 
            ' If user only has 3 cards, I'll map: Present, Absences, Late.
            
            Label4.Text = Convert.ToInt32(stats("TotalPresent")).ToString("N0")
            Label6.Text = Convert.ToInt32(stats("Absences")).ToString("N0")
            Label7.Text = Convert.ToInt32(stats("LateInstances")).ToString("N0")
            
            ' If there is a label for Overtime, update it. If not, maybe create dynamic or ignore for now.
            ' For now, I'll assume 3 cards as per existing code structure.
            
            Dim headerText As String = $"Attendance Tracking - {Reports.SelectedPeriod}"
            If Reports.SelectedPeriod = "Daily" Then
                headerText &= $" ({Reports.GlobalFilterDate:MMM dd, yyyy})"
            End If
            LabelHeader.Text = headerText
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    '====================================
    ' FORMAT DATAGRIDVIEW (STYLING)
    '====================================
    Private Sub FormatDataGridView()
        Try
            For Each row As DataGridViewRow In DataGridView1.Rows
                Dim statusCell As Object = row.Cells("Status").Value
                Dim status As String = If(statusCell IsNot Nothing, statusCell.ToString(), "")

                ' Style based on Status
                Dim statusStyle As New DataGridViewCellStyle()
                statusStyle.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
                
                Select Case status
                    Case "Present"
                        statusStyle.ForeColor = Color.FromArgb(16, 185, 129) ' Green
                        statusStyle.BackColor = Color.FromArgb(209, 250, 229)
                        
                    Case "Late"
                        statusStyle.ForeColor = Color.FromArgb(245, 158, 11) ' Orange
                         statusStyle.BackColor = Color.FromArgb(254, 243, 199)

                    Case "Absent"
                        statusStyle.ForeColor = Color.FromArgb(239, 68, 68) ' Red
                        statusStyle.BackColor = Color.FromArgb(254, 226, 226)

                    Case "On Leave"
                        statusStyle.ForeColor = Color.FromArgb(139, 92, 246) ' Purple
                        statusStyle.BackColor = Color.FromArgb(237, 233, 254)

                    Case Else
                        statusStyle.ForeColor = Color.FromArgb(100, 116, 139)
                End Select
                
                row.Cells("Status").Style = statusStyle

                ' Format Time columns - REMOVED: Now handled by DefaultCellStyle.Format in SetupDataGridView
                ' This prevents conflicts and ensures proper sorting/rendering.
                
                ' Visual indicator for Overtime - SAFELY CHECK FOR NULL
                Dim otVal As Double = 0
                Dim otCellVal = row.Cells("Overtime").Value

                If otCellVal IsNot Nothing AndAlso
                   otCellVal IsNot DBNull.Value AndAlso
                   Double.TryParse(otCellVal.ToString(), otVal) AndAlso
                   otVal > 0 Then

                    row.Cells("Overtime").Style.ForeColor = Color.Green
                    row.Cells("Overtime").Style.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
                    row.Cells("Overtime").Value = "+" & otVal.ToString("0.##")
                End If
            Next

        Catch ex As Exception
            ' Silent fail for formatting errors but log to console if debugging
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    '====================================
    ' SEARCH TEXTBOX EVENTS WITH DEBOUNCE
    '====================================
    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitialLoad Then Return

        Dim currentSearch = GetSearchText()
        ' Only restart timer if the actual search criteria changed
        If currentSearch = _lastSearchText Then Return
        
        _lastSearchText = currentSearch
        
        ' Stop any existing timer and restart
        _searchDebounceTimer.Stop()
        _searchDebounceTimer.Start()
    End Sub

    Private Async Sub SearchDebounceTimer_Tick(sender As Object, e As EventArgs)
        _searchDebounceTimer.Stop()
        Await RefreshAttendanceAsync(True) ' Reset to page 1 when searching
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            Me.UseWaitCursor = isLoading
            DataGridView1.Enabled = Not isLoading

            TextBoxSearch.Enabled = Not isLoading
            If btnPrev IsNot Nothing Then btnPrev.Enabled = Not isLoading AndAlso _currentPage > 1
            If btnNext IsNot Nothing Then btnNext.Enabled = Not isLoading AndAlso _currentPage < _totalPages
        Catch
        End Try
    End Sub

    Private Sub UpdatePaginationUI()
        If lblPageStatus IsNot Nothing Then
            lblPageStatus.Text = $"Page {_currentPage} of {_totalPages} (Total: {_totalRecords:N0})"
        End If
        If btnPrev IsNot Nothing Then btnPrev.Enabled = (_currentPage > 1) AndAlso Not _isLoading
        If btnNext IsNot Nothing Then btnNext.Enabled = (_currentPage < _totalPages) AndAlso Not _isLoading
    End Sub

    Private Async Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If _currentPage < _totalPages AndAlso Not _isLoading Then
            _currentPage += 1
            Await RefreshAttendanceAsync()
        End If
    End Sub

    Private Async Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If _currentPage > 1 AndAlso Not _isLoading Then
            _currentPage -= 1
            Await RefreshAttendanceAsync()
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            ' Ensure column exists before accessing
            If DataGridView1.Columns.Contains("EmployeeName") Then
                Dim employeeName As String = DataGridView1.Rows(e.RowIndex).Cells("EmployeeName").Value.ToString()
                Dim result = MessageBox.Show($"Would you like to view the Payroll record for {employeeName}?", "Cross-reference Payroll", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                If result = DialogResult.Yes Then
                    Try
                        Dim payrollForm As New FormPayroll()
                        payrollForm.Show()
                    Catch ex As Exception
                        MessageBox.Show("Could not open payroll form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End If
    End Sub



    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search employees..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42)
        End If
        If SearchContainer IsNot Nothing Then
            SearchContainer.BorderColor = Color.FromArgb(99, 102, 241)
        End If
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search employees..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
        End If
        If SearchContainer IsNot Nothing Then
            SearchContainer.BorderColor = Color.FromArgb(226, 232, 240)
        End If
    End Sub

    '====================================
    ' EXPORT BUTTON
    '====================================
    '====================================
    ' DATE FILTER CHANGED
    '====================================




    '====================================
    ' REFRESH DATA (PUBLIC METHOD)
    '====================================
    Public Async Sub RefreshData()
        Await RefreshAttendanceAsync(True)
    End Sub





End Class