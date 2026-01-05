Imports MySqlConnector
Imports System.Data
Imports System.Threading.Tasks

Public Class FormTakeOutOrders
    Private ReadOnly connectionString As String = modDB.strConnection
    Private originalData As DataTable
    Private isInitialLoad As Boolean = True
    Private _isLoading As Boolean = False
    Private _lastSearchText As String = ""

    ' Pagination state
    Private _currentPage As Integer = 1
    Private ReadOnly _pageSize As Integer = 50
    Private _totalRecords As Integer = 0
    Private _totalPages As Integer = 0

    Private Async Sub FormTakeOutOrders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set initial loading state
        Label4.Text = "..."
        Label6.Text = "..."
        Label7.Text = "..."

        ' Initialize pagination controls
        InitializePaginationControls()

        Await RefreshOrdersAsync()
        isInitialLoad = False
        Await RefreshOrdersAsync()
        'ConfigureDateFilter()
        isInitialLoad = False
        'ConfigureDateFilter()
    End Sub

    Private Sub InitializePaginationControls()
        ' Make sure pagination controls exist and are enabled
        If btnPrev IsNot Nothing Then
            btnPrev.Enabled = False
        End If
        If btnNext IsNot Nothing Then
            btnNext.Enabled = False
        End If
        If lblPageStatus IsNot Nothing Then
            lblPageStatus.Text = "Loading..."
        End If
    End Sub

    Private Async Function RefreshOrdersAsync(Optional resetPage As Boolean = False) As Task
        ' Prevent concurrent refresh operations
        If _isLoading Then Return

        _isLoading = True

        Try
            ' Update UI to show loading state
            If Me.InvokeRequired Then
                Me.Invoke(Sub() SetLoadingState(True))
            Else
                SetLoadingState(True)
            End If

            If resetPage Then _currentPage = 1

            Dim searchText As String = TextBoxSearch.Text.Trim()
            If searchText = "Search orders..." Then searchText = ""

            ' Get total count first
            _totalRecords = Await Task.Run(Function() FetchTotalTakeOutCount(searchText))
            _totalPages = Math.Max(1, CInt(Math.Ceiling(CDbl(_totalRecords) / _pageSize)))

            ' Validate current page
            If _currentPage > _totalPages Then _currentPage = _totalPages
            If _currentPage < 1 Then _currentPage = 1

            ' Calculate offset
            Dim offset As Integer = Math.Max(0, (_currentPage - 1) * _pageSize)

            ' Load data
            Dim dt As DataTable = Await Task.Run(Function() LoadOrdersDataFromDB(searchText, offset, _pageSize))
            originalData = dt

            ' Check if form is still valid
            If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

            ' Update UI on UI thread
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              DataGridView1.DataSource = dt
                              FormatGrid()
                              UpdatePaginationUI()
                              Label10.Text = "Recent Take-Out Orders"
                          End Sub)
            Else
                DataGridView1.DataSource = dt
                FormatGrid()
                UpdatePaginationUI()
                Label10.Text = "Recent Take-Out Orders"
            End If

            ' Update summary with total stats (non-paginated)
            Await UpdateTotalSummaryAsync(searchText)

        Catch ex As Exception
            If Not Me.IsDisposed Then
                MessageBox.Show("Error refreshing orders: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            ' Always reset loading state
            If Not Me.IsDisposed Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  SetLoadingState(False)
                                  _isLoading = False
                              End Sub)
                Else
                    SetLoadingState(False)
                    _isLoading = False
                End If
            End If
        End Try
    End Function

    Private Function FetchTotalTakeOutCount(searchText As String) As Integer
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim sYear As Integer = Reports.SelectedYear
        Dim sMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If

            Case "Weekly"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If

            Case "Monthly"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If

            Case "Yearly"
                ' 5-year historical summary
                periodFilter = $" AND YEAR(o.OrderDate) <= {sYear} AND YEAR(o.OrderDate) >= {sYear - 4} "
        End Select

        Dim query As String = "SELECT COUNT(*) FROM orders o WHERE o.OrderType = 'Takeout' " & periodFilter & " AND (o.OrderID LIKE @search OR o.OrderStatus LIKE @search)"

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            ' Return 0 if there's an error
            Return 0
        End Try
    End Function

    Private Function LoadOrdersDataFromDB(searchText As String, offset As Integer, limit As Integer) As DataTable
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim sYear As Integer = Reports.SelectedYear
        Dim sMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If

            Case "Weekly"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If

            Case "Monthly"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If

            Case "Yearly"
                ' 5-year historical summary
                periodFilter = $" AND YEAR(o.OrderDate) <= {sYear} AND YEAR(o.OrderDate) >= {sYear - 4} "
        End Select

        Dim dt As New DataTable()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String =
                    "SELECT " &
                    "OrderID, " &
                    "CONCAT('#', OrderID) AS OrderNumber, " &
                    "ItemsOrderedCount AS Items, " &
                    "TotalAmount AS Amount, " &
                    "OrderStatus AS Status, " &
                    "DATE_FORMAT(CONCAT(OrderDate, ' ', OrderTime), '%Y-%m-%d %H:%i') AS Time " &
                    "FROM orders o " &
                    "WHERE o.OrderType = 'Takeout' " & periodFilter & " AND (o.OrderID LIKE @search OR o.OrderStatus LIKE @search) " &
                    "ORDER BY o.OrderDate DESC, o.OrderTime DESC, o.OrderID DESC " &
                    "LIMIT @limit OFFSET @offset"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    cmd.Parameters.AddWithValue("@limit", limit)
                    cmd.Parameters.AddWithValue("@offset", offset)
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Private Async Function UpdateTotalSummaryAsync(searchText As String) As Task
        Dim totalCount As Integer = 0
        Dim totalRevenue As Decimal = 0
        Dim avgValue As Decimal = 0
        
        Dim prevYearRevenue As Decimal = 0

        Try
            Await Task.Run(Sub()
                               ' Get period filter from Reports form
                               Dim periodFilter As String = ""
                               Dim sYear As Integer = Reports.SelectedYear
                               Dim sMonth As Integer = Reports.SelectedMonth

                               Select Case Reports.SelectedPeriod
                                   Case "Daily"
                                        If sMonth = 0 Then
                                            periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                                        Else
                                            periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                                        End If

                                   Case "Weekly"
                                        If sMonth = 0 Then
                                            periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                                        Else
                                            periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                                        End If

                                   Case "Monthly"
                                       If sMonth = 0 Then
                                           periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                                       Else
                                           periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                                       End If

                                   Case "Yearly"
                                       ' 5-year historical summary
                                       periodFilter = $" AND YEAR(o.OrderDate) <= {sYear} AND YEAR(o.OrderDate) >= {sYear - 4} "
                               End Select

                               Using conn As New MySqlConnection(connectionString)
                                   conn.Open()
                                   Dim sql = "SELECT COUNT(*), COALESCE(SUM(TotalAmount), 0) FROM orders o WHERE o.OrderType = 'Takeout' " & periodFilter & " AND (o.OrderID LIKE @search OR o.OrderStatus LIKE @search)"
                                   Using cmd As New MySqlCommand(sql, conn)
                                       cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                                       Using reader = cmd.ExecuteReader()
                                           If reader.Read() Then
                                               totalCount = reader.GetInt32(0)
                                               totalRevenue = reader.GetDecimal(1)
                                           End If
                                       End Using
                                   End Using

                                   ' If Yearly, get comparison
                                   If Reports.SelectedPeriod = "Yearly" Then
                                       Dim sqlCompare = $"SELECT COALESCE(SUM(TotalAmount), 0) FROM orders WHERE OrderType = 'Takeout' AND YEAR(OrderDate) = {sYear - 1}"
                                       Using cmdComp As New MySqlCommand(sqlCompare, conn)
                                           prevYearRevenue = Convert.ToDecimal(cmdComp.ExecuteScalar())
                                       End Using
                                   End If
                               End Using
                           End Sub)

            If totalCount > 0 Then avgValue = totalRevenue / totalCount

            ' Update UI labels
            Me.Invoke(Sub()
                          Label4.Text = totalCount.ToString("N0")
                          Label6.Text = "₱" & totalRevenue.ToString("N2")
                          Label7.Text = "₱" & avgValue.ToString("N2")
                          
                          If Reports.SelectedPeriod = "Yearly" Then
                              Label6.Text &= " (5-yr Total)"
                              If prevYearRevenue > 0 Then
                                  Dim diff As Decimal = totalRevenue - prevYearRevenue
                                  Dim percent As Decimal = (diff / prevYearRevenue) * 100
                                  Dim sign As String = If(diff >= 0, "+", "")
                                  Label6.Text &= $" ({sign}{percent:N1}% YoY)"
                              End If
                          End If
                      End Sub)
        Catch
            ' Silent fail
        End Try
    End Function

    Private Sub UpdateSummaryTiles(dt As DataTable)
        Try
            Dim totalOrders As Integer = dt.Rows.Count
            Dim totalRevenue As Decimal = 0
            Dim avgOrderValue As Decimal = 0

            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("Amount")) Then
                    totalRevenue += Convert.ToDecimal(row("Amount"))
                End If
            Next

            If totalOrders > 0 Then
                avgOrderValue = totalRevenue / totalOrders
            End If

            ' Safe UI updates
            Label4.Text = totalOrders.ToString("N0")
            Label6.Text = "₱" & totalRevenue.ToString("N2")
            Label7.Text = "₱" & avgOrderValue.ToString("N2")

        Catch ex As Exception
            ' Silent fail for stats
        End Try
    End Sub

    ' =============================
    ' SEARCH FUNCTIONALITY
    ' =============================
    Private Async Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitialLoad Then Return
        
        Dim currentSearch = TextBoxSearch.Text.Trim()
        If currentSearch = "Search orders..." Then currentSearch = ""
        
        ' Only refresh if the actual search criteria changed
        If currentSearch = _lastSearchText Then Return
        
        _lastSearchText = currentSearch
        Await RefreshOrdersAsync(True) ' Reset to page 1 on search
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            Me.UseWaitCursor = isLoading
            DataGridView1.Enabled = Not isLoading

            ' Update pagination buttons based on loading state AND current page position
            If btnPrev IsNot Nothing Then
                btnPrev.Enabled = (Not isLoading) AndAlso (_currentPage > 1)
            End If

            If btnNext IsNot Nothing Then
                btnNext.Enabled = (Not isLoading) AndAlso (_currentPage < _totalPages)
            End If
        Catch ex As Exception
            ' Ignore errors in setting loading state
        End Try
    End Sub

    Private Sub UpdatePaginationUI()
        Try
            ' Update page status label
            If lblPageStatus IsNot Nothing Then
                lblPageStatus.Text = $"Page {_currentPage} of {_totalPages} (Total Records: {_totalRecords:N0})"
            End If

            ' Update button states
            If btnPrev IsNot Nothing Then
                btnPrev.Enabled = (_currentPage > 1) AndAlso (Not _isLoading)
            End If

            If btnNext IsNot Nothing Then
                btnNext.Enabled = (_currentPage < _totalPages) AndAlso (Not _isLoading)
            End If
        Catch ex As Exception
            ' Ignore errors in updating pagination UI
        End Try
    End Sub

    Private Async Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        ' Prevent multiple clicks while loading
        If _isLoading Then Return

        If _currentPage < _totalPages Then
            _currentPage += 1
            Await RefreshOrdersAsync()
        End If
    End Sub

    Private Async Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        ' Prevent multiple clicks while loading
        If _isLoading Then Return

        If _currentPage > 1 Then
            _currentPage -= 1
            Await RefreshOrdersAsync()
        End If
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search orders..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42)
            SearchContainer.BorderColor = Color.FromArgb(99, 102, 241)
        End If
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search orders..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
            SearchContainer.BorderColor = Color.FromArgb(226, 232, 240)
        End If
    End Sub

    ' =============================
    ' FORMAT GRID
    ' =============================
    Private Sub FormatGrid()
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
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
                .ColumnHeadersDefaultCellStyle.BackColor = Color.White
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
                .ColumnHeadersHeight = 50
                .RowTemplate.Height = 50
                .EnableHeadersVisualStyles = False

            If .Columns.Contains("OrderID") Then .Columns("OrderID").Visible = False

            If .Columns.Contains("OrderNumber") Then
                With .Columns("OrderNumber")
                    .HeaderText = "Order #"
                    .FillWeight = 80
                End With
            End If

            If .Columns.Contains("Items") Then
                With .Columns("Items")
                    .HeaderText = "Items"
                    .FillWeight = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End With
            End If

            If .Columns.Contains("Amount") Then
                With .Columns("Amount")
                    .HeaderText = "Total Amount"
                    .DefaultCellStyle.Format = "₱#,##0.00"
                    .DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42)
                    .DefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)
                End With
            End If

            If .Columns.Contains("Time") Then
                With .Columns("Time")
                    .HeaderText = "Date & Time"
                    .FillWeight = 120
                End With
            End If
        End With

        ' Apply status colors
        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim statusCell = row.Cells("Status")
            If statusCell.Value IsNot Nothing Then
                Dim status = statusCell.Value.ToString().ToLower()
                Select Case status
                    Case "paid", "completed"
                        statusCell.Style.ForeColor = Color.FromArgb(16, 185, 129)
                    Case "pending"
                        statusCell.Style.ForeColor = Color.FromArgb(245, 158, 11)
                    Case "cancelled"
                        statusCell.Style.ForeColor = Color.FromArgb(239, 68, 68)
                End Select
            End If
        Next
    End Sub

    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        Reports.Instance.ExportCurrentReport()
    End Sub

    ' =============================
    ' PREVENT ERROR POPUPS
    ' =============================
    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) _
        Handles DataGridView1.DataError
        e.ThrowException = False
    End Sub

    ' =======================================================================
    ' REFRESH DATA
    ' =======================================================================
    Public Async Sub RefreshData()
        Await RefreshOrdersAsync(True)
        LoadOrderBreakdownAsync()
    End Sub

    ' =======================================================================
    ' LOAD ORDER BREAKDOWN ASYNC
    ' =======================================================================
    Private Async Sub LoadOrderBreakdownAsync()
        Try
            Dim dt As New DataTable()
            Dim selectedPeriod As String = Reports.SelectedPeriod
            Dim selectedYear As Integer = Reports.SelectedYear
            Dim selectedMonth As Integer = Reports.SelectedMonth

            Await System.Threading.Tasks.Task.Run(Sub()
                                                      Try
                                                          Using conn As New MySqlConnection(connectionString)
                                                              conn.Open()
                                                              Dim query As String = ""
                                                              Dim params As New List(Of MySqlParameter)

                                                              Select Case selectedPeriod
                                                                  Case "Daily"
                                                                      query = "SELECT DATE_FORMAT(OrderDate, '%Y-%m-%d') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Takeout' AND YEAR(OrderDate) = @Year AND MONTH(OrderDate) = @Month GROUP BY DATE(OrderDate) ORDER BY Period DESC"
                                                                      params.Add(New MySqlParameter("@Year", selectedYear))
                                                                      params.Add(New MySqlParameter("@Month", selectedMonth))
                                                                  Case "Weekly"
                                                                      query = "SELECT CONCAT('Week ', WEEK(OrderDate, 1)) AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Takeout' AND YEAR(OrderDate) = @Year GROUP BY YEARWEEK(OrderDate, 1) ORDER BY YEARWEEK(OrderDate, 1) DESC"
                                                                      params.Add(New MySqlParameter("@Year", selectedYear))
                                                                  Case "Monthly"
                                                                      query = "SELECT DATE_FORMAT(OrderDate, '%M') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Takeout' AND YEAR(OrderDate) = @Year GROUP BY MONTH(OrderDate) ORDER BY MONTH(OrderDate) DESC"
                                                                      params.Add(New MySqlParameter("@Year", selectedYear))
                                                                  Case "Yearly"
                                                                      query = "SELECT YEAR(OrderDate) AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Takeout' AND YEAR(OrderDate) <= @Year AND YEAR(OrderDate) >= @Year - 4 GROUP BY YEAR(OrderDate) ORDER BY Period DESC"
                                                                      params.Add(New MySqlParameter("@Year", selectedYear))
                                                              End Select

                                                              If Not String.IsNullOrEmpty(query) Then
                                                                  Using cmd As New MySqlCommand(query, conn)
                                                                      cmd.Parameters.AddRange(params.ToArray())
                                                                      Using adapter As New MySqlDataAdapter(cmd)
                                                                          adapter.Fill(dt)
                                                                      End Using
                                                                  End Using
                                                              End If
                                                          End Using
                                                      Catch
                                                      End Try
                                                  End Sub)

            If DataGridViewBreakdown IsNot Nothing Then
                DataGridViewBreakdown.DataSource = dt
                FormatBreakdownDataGridView()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FormatBreakdownDataGridView()
        Try
            With DataGridViewBreakdown
                .ReadOnly = True
                .AllowUserToAddRows = False
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .RowHeadersVisible = False
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
                .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250)

                If .Columns.Contains("Period") Then .Columns("Period").HeaderText = "Time Period"
                If .Columns.Contains("OrderCount") Then .Columns("OrderCount").HeaderText = "Orders"
                If .Columns.Contains("TotalRevenue") Then
                    .Columns("TotalRevenue").HeaderText = "Revenue"
                    .Columns("TotalRevenue").DefaultCellStyle.Format = ChrW(&H20B1) & "#,##0.00"
                End If
                If .Columns.Contains("AvgValue") Then
                    .Columns("AvgValue").HeaderText = "Avg Value"
                    .Columns("AvgValue").DefaultCellStyle.Format = ChrW(&H20B1) & "#,##0.00"
                End If
            End With
        Catch
        End Try
    End Sub
End Class