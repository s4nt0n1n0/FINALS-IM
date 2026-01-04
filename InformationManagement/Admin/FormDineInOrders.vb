Imports MySqlConnector
Imports System.Data
Imports System.Threading.Tasks
Imports System.Drawing.Drawing2D

Public Class FormDineInOrders
    Private ReadOnly connectionString As String = modDB.strConnection
    Private _isLoading As Boolean = False
    Private _baseTitle As String = ""
    Private _dataCache As DataTable = Nothing
    Private _lastRefresh As DateTime = DateTime.MinValue
    Private ReadOnly _cacheTimeout As TimeSpan = TimeSpan.FromSeconds(30)

    ' Pagination state
    Private _currentPage As Integer = 1
    Private ReadOnly _pageSize As Integer = 50
    Private _totalRecords As Integer = 0
    Private _totalPages As Integer = 0

    Private originalData As DataTable
    Private isInitialLoad As Boolean = True
    Private _lastSearchText As String = ""

    Private Async Sub FormDineInOrders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set initial loading state
        Label4.Text = "..."
        Label6.Text = "..."
        Label7.Text = "..."

        _baseTitle = LabelHeader.Text
        _currentPage = 1
        Await BeginLoadDineInOrders()
        isInitialLoad = False
        ' ConfigureDateFilter()
    End Sub





    Private Sub InitializeModernUI()
        ' Enhanced form appearance
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        ' modern DataGridView styling
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


        ' Style the label
        Label2.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        Label2.ForeColor = Color.FromArgb(44, 62, 80)
    End Sub

    Private Sub StyleButton(btn As Button)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = Color.FromArgb(46, 204, 113)
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        btn.Cursor = Cursors.Hand
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96)
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(34, 153, 84)
    End Sub

    Private Async Function BeginLoadDineInOrders() As Task
        If _isLoading Then Return

        _isLoading = True
        SetLoadingState(True)

        Try
            Dim searchText As String = TextBoxSearch.Text.Trim()
            If searchText = "Search orders..." Then searchText = ""

            ' Get total count with filter
            _totalRecords = Await Task.Run(Function() FetchTotalDineInCount(searchText))
            _totalPages = Math.Max(1, CInt(Math.Ceiling(CDbl(_totalRecords) / _pageSize)))
            
            If _currentPage > _totalPages Then _currentPage = _totalPages
            If _currentPage < 1 Then _currentPage = 1

            Dim offset As Integer = (_currentPage - 1) * _pageSize
            Dim table As DataTable = Await Task.Run(Function() FetchDineInOrdersTable(searchText, offset, _pageSize))

            If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

            _dataCache = table
            _lastRefresh = DateTime.Now

            DataGridView1.DataSource = table
            ConfigureGrid()
            ApplyStatusColors()
            ' UpdatePaginationUI() ' This function is not yet defined, will be added later
            ' UpdateSummaryTiles(table) ' Replaced with UpdateTotalSummaryAsync
            
            ' Update summary with total stats (non-paginated)
            Await UpdateTotalSummaryAsync(searchText)
        Catch ex As Exception
            If Not Me.IsDisposed Then
                MessageBox.Show("Error refreshing dine-in orders: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If Not Me.IsDisposed Then SetLoadingState(False)
            _isLoading = False
        End Try
    End Function

    Private Function FetchTotalDineInCount(searchText As String) As Integer
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim selectedYear As Integer = Reports.SelectedYear
        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                If selectedYear = DateTime.Now.Year Then
                    periodFilter = $" AND DATE(OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "
                Else
                    periodFilter = $" AND DATE(OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' " ' Use picker date for historic daily too if desired, or keep as is.
                    ' Actually, if Daily is selected, we usually want specific day regardless of year logic if we have a picker.
                    ' But to be safe and consistent with other forms:
                    periodFilter = $" AND DATE(OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "
                End If

            Case "Weekly"
                periodFilter = $" AND YEARWEEK(OrderDate, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1) "

            Case "Monthly"
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                End If

            Case "Yearly"
                periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
        End Select

        Dim query As String = "SELECT COUNT(*) FROM orders WHERE OrderType = 'Dine-in' " & periodFilter & " AND (OrderID LIKE @search OR OrderStatus LIKE @search)"
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Function FetchDineInOrdersTable(searchText As String, offset As Integer, limit As Integer) As DataTable
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim selectedYear As Integer = Reports.SelectedYear
        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                periodFilter = $" AND DATE(o.OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "

            Case "Weekly"
                periodFilter = $" AND YEARWEEK(o.OrderDate, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1) "

            Case "Monthly"
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} AND MONTH(o.OrderDate) = {selectedMonth} "
                End If

            Case "Yearly"
                periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} "
        End Select

        ' Build query with LIMIT, OFFSET and search
        Dim query As String =
            "SELECT " &
            "o.OrderID, " &
            "CONCAT('#', o.OrderID) AS OrderNumber, " &
            "(SELECT GROUP_CONCAT(CONCAT(oi2.Quantity, 'x ', oi2.ProductName) SEPARATOR ', ') " &
            "   FROM order_items oi2 " &
            "   WHERE oi2.OrderID = o.OrderID " &
            "   LIMIT 10) AS ItemsOrdered, " &
            "o.TotalAmount, " &
            "o.OrderStatus AS Status, " &
            "DATE_FORMAT(CONCAT(o.OrderDate, ' ', o.OrderTime), '%Y-%m-%d %H:%i') AS OrderDateTime " &
            "FROM orders o " &
            "WHERE o.OrderType = 'Dine-in' " & periodFilter & " AND (o.OrderID LIKE @search OR o.OrderStatus LIKE @search) " &
            "ORDER BY o.OrderDate DESC, o.OrderTime DESC, o.OrderID DESC " &
            "LIMIT @limit OFFSET @offset"

        Dim dt As New DataTable()
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                cmd.Parameters.AddWithValue("@limit", limit)
                cmd.Parameters.AddWithValue("@offset", offset)
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Private Async Function UpdateTotalSummaryAsync(searchText As String) As Task
        Dim totalCount As Integer = 0
        Dim totalRevenue As Decimal = 0
        Dim avgValue As Decimal = 0

        Try
            Await Task.Run(Sub()
                               ' Get period filter from Reports form
                               Dim periodFilter As String = ""
                               Dim selectedYear As Integer = Reports.SelectedYear
                               Dim selectedMonth As Integer = Reports.SelectedMonth

                               Select Case Reports.SelectedPeriod
                                   Case "Daily"
                                       periodFilter = $" AND DATE(OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "

                                   Case "Weekly"
                                       periodFilter = $" AND YEARWEEK(OrderDate, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1) "

                                   Case "Monthly"
                                       If selectedMonth = 0 Then
                                           periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                                       Else
                                           periodFilter = $" AND YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                                       End If

                                   Case "Yearly"
                                       periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                               End Select

                               Using conn As New MySqlConnection(connectionString)
                                   conn.Open()
                                   Dim sql = "SELECT COUNT(*), COALESCE(SUM(TotalAmount), 0) FROM orders WHERE OrderType = 'Dine-in' " & periodFilter & " AND (OrderID LIKE @search OR OrderStatus LIKE @search)"
                                   Using cmd As New MySqlCommand(sql, conn)
                                       cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                                       Using reader = cmd.ExecuteReader()
                                           If reader.Read() Then
                                               totalCount = reader.GetInt32(0)
                                               totalRevenue = reader.GetDecimal(1)
                                           End If
                                       End Using
                                   End Using
                               End Using
                           End Sub)

            If totalCount > 0 Then avgValue = totalRevenue / totalCount

            ' Update UI labels
            Me.Invoke(Sub()
                          Label4.Text = totalCount.ToString("N0")
                          Label6.Text = "₱" & totalRevenue.ToString("N2")
                          Label7.Text = "₱" & avgValue.ToString("N2")
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
                If Not IsDBNull(row("TotalAmount")) Then
                    totalRevenue += Convert.ToDecimal(row("TotalAmount"))
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
        _currentPage = 1
        Await BeginLoadDineInOrders()
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



    ' FIXED: Improved status color application
    Private Sub ApplyStatusColors()
        Try
            ' Add visual indicators for order status
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow AndAlso row.Cells("Status").Value IsNot Nothing Then
                    Dim status As String = row.Cells("Status").Value.ToString().Trim().ToLower()

                    ' Reset to default first
                    row.Cells("Status").Style.ForeColor = Color.FromArgb(44, 62, 80)
                    row.Cells("Status").Style.Font = New Font("Segoe UI", 9, FontStyle.Regular)

                    ' Apply status-specific colors
                    Select Case status
                        Case "completed", "paid"
                            row.Cells("Status").Style.ForeColor = Color.FromArgb(16, 185, 129)
                        Case "pending", "preparing"
                            row.Cells("Status").Style.ForeColor = Color.FromArgb(245, 158, 11)
                        Case "cancelled", "canceled"
                            row.Cells("Status").Style.ForeColor = Color.FromArgb(239, 68, 68)
                    End Select
                End If
            Next

            ' Force redraw
            DataGridView1.InvalidateColumn(DataGridView1.Columns("Status").Index)
        Catch ex As Exception
            ' Silently handle errors in color application
            Debug.WriteLine("Error applying status colors: " & ex.Message)
        End Try
    End Sub



    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        Try
            If DataGridView1.Columns(e.ColumnIndex).Name = "Status" AndAlso e.Value IsNot Nothing Then
                Dim status As String = e.Value.ToString().Trim().ToLower()

                Select Case status
                    Case "completed", "paid"
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96)
                        e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    Case "pending", "preparing"
                        e.CellStyle.ForeColor = Color.FromArgb(241, 196, 15)
                        e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    Case "cancelled", "canceled"
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60)
                        e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End Select
            End If
        Catch ex As Exception
            ' Silently handle formatting errors
        End Try
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            Me.UseWaitCursor = isLoading

            If btnPrev IsNot Nothing Then btnPrev.Enabled = Not isLoading AndAlso _currentPage > 1
            If btnNext IsNot Nothing Then btnNext.Enabled = Not isLoading AndAlso _currentPage < _totalPages
            LabelHeader.Text = If(isLoading, _baseTitle & " (Updating...)", _baseTitle)


        Catch
        End Try
    End Sub

    Private Sub ConfigureGrid()
        With DataGridView1
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToOrderColumns = False
            .AllowUserToAddRows = False
        End With

        ' Optimized column configuration
        If DataGridView1.Columns.Contains("OrderID") Then
            With DataGridView1.Columns("OrderID")
                .HeaderText = "Order #"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            End With
        End If

        If DataGridView1.Columns.Contains("ItemsOrdered") Then
            With DataGridView1.Columns("ItemsOrdered")
                .HeaderText = "Items Ordered"
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .MinimumWidth = 300
                .DefaultCellStyle.WrapMode = DataGridViewTriState.False
            End With
        End If

        If DataGridView1.Columns.Contains("TotalAmount") Then
            With DataGridView1.Columns("TotalAmount")
                .HeaderText = "Amount"
                .Width = 130
                .DefaultCellStyle.Format = "₱#,##0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94)
            End With
        End If

        If DataGridView1.Columns.Contains("Status") Then
            With DataGridView1.Columns("Status")
                .HeaderText = "Status"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End If

        If DataGridView1.Columns.Contains("OrderDateTime") Then
            With DataGridView1.Columns("OrderDateTime")
                .HeaderText = "Date & Time"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End If
    End Sub

    Private Async Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If _currentPage > 1 Then
            _currentPage -= 1
            Await BeginLoadDineInOrders()
        End If
    End Sub

    Private Async Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If _currentPage < _totalPages Then
            _currentPage += 1
            Await BeginLoadDineInOrders()
        End If
    End Sub


    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        _dataCache?.Dispose()
        MyBase.OnFormClosing(e)
    End Sub
    ' =======================================================================
    ' REFRESH DATA
    ' =======================================================================
    Public Async Sub RefreshData()
        ' ConfigureDateFilter()
        _currentPage = 1
        Await BeginLoadDineInOrders()
    End Sub


End Class