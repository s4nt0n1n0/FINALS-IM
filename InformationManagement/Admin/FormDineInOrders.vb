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
    Private _totalPage As Integer = 0 ' Fixed variable name typo from previous versions if any

    Private originalData As DataTable
    Private isInitialLoad As Boolean = True
    Private _lastSearchText As String = ""
    Private _totalPageCount As Integer = 0 ' Using a clearer name for total pages

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
            .DefaultCellStyle.SelectionForeColor = Color.Black
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
            _totalPageCount = Math.Max(1, CInt(Math.Ceiling(CDbl(_totalRecords) / _pageSize)))

            If _currentPage > _totalPageCount Then _currentPage = _totalPageCount
            If _currentPage < 1 Then _currentPage = 1

            Dim offset As Integer = (_currentPage - 1) * _pageSize
            Dim table As DataTable = Await Task.Run(Function() FetchDineInOrdersTable(searchText, offset, _pageSize))

            If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

            _dataCache = table
            _lastRefresh = DateTime.Now

            DataGridView1.DataSource = table
            ConfigureGrid()
            ApplyStatusColors()

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
        Dim periodFilter As String = ""
        Dim selectedYear As Integer = Reports.SelectedYear
        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                periodFilter = " AND DATE(OrderDate) = @filterDate "
            Case "Weekly"
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                End If
            Case "Monthly"
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                End If
            Case "Yearly"
                periodFilter = $" AND YEAR(OrderDate) <= {selectedYear} AND YEAR(OrderDate) >= {selectedYear - 4} "
        End Select

        Dim query As String = "SELECT COUNT(*) FROM orders WHERE OrderType = 'Dine-in' " & periodFilter & " AND (OrderID LIKE @search OR OrderStatus LIKE @search)"
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                cmd.Parameters.AddWithValue("@filterDate", Reports.GlobalFilterDate.ToString("yyyy-MM-dd"))
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Function FetchDineInOrdersTable(searchText As String, offset As Integer, limit As Integer) As DataTable
        Dim periodFilter As String = ""
        Dim selectedYear As Integer = Reports.SelectedYear
        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                periodFilter = " AND DATE(o.OrderDate) = @filterDate "
            Case "Weekly"
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} AND MONTH(o.OrderDate) = {selectedMonth} "
                End If
            Case "Monthly"
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} AND MONTH(o.OrderDate) = {selectedMonth} "
                End If
            Case "Yearly"
                periodFilter = $" AND YEAR(o.OrderDate) <= {selectedYear} AND YEAR(o.OrderDate) >= {selectedYear - 4} "
        End Select

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
                cmd.Parameters.AddWithValue("@filterDate", Reports.GlobalFilterDate.ToString("yyyy-MM-dd"))
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
        Dim prevYearRevenue As Decimal = 0

        Try
            Await Task.Run(Sub()
                               Dim periodFilter As String = ""
                               Dim selectedYear As Integer = Reports.SelectedYear
                               Dim selectedMonth As Integer = Reports.SelectedMonth

                                Select Case Reports.SelectedPeriod
                                   Case "Daily"
                                       periodFilter = " AND DATE(OrderDate) = @filterDate "
                                   Case "Weekly"
                                       If selectedMonth = 0 Then
                                           periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                                       Else
                                           periodFilter = $" AND YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                                       End If
                                   Case "Monthly"
                                       If selectedMonth = 0 Then
                                           periodFilter = $" AND YEAR(OrderDate) = {selectedYear} "
                                       Else
                                           periodFilter = $" AND YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                                       End If
                                   Case "Yearly"
                                       periodFilter = $" AND YEAR(OrderDate) <= {selectedYear} AND YEAR(OrderDate) >= {selectedYear - 4} "
                               End Select

                               Using conn As New MySqlConnection(connectionString)
                                   conn.Open()
                                   Dim sql = "SELECT COUNT(*), COALESCE(SUM(TotalAmount), 0) FROM orders WHERE OrderType = 'Dine-in' " & periodFilter & " AND (OrderID LIKE @search OR OrderStatus LIKE @search)"
                                   Using cmd As New MySqlCommand(sql, conn)
                                       cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                                       cmd.Parameters.AddWithValue("@filterDate", Reports.GlobalFilterDate.ToString("yyyy-MM-dd"))
                                       Using reader = cmd.ExecuteReader()
                                           If reader.Read() Then
                                               totalCount = reader.GetInt32(0)
                                               totalRevenue = reader.GetDecimal(1)
                                           End If
                                       End Using
                                   End Using

                                   If Reports.SelectedPeriod = "Yearly" Then
                                       Dim sqlCompare = $"SELECT COALESCE(SUM(TotalAmount), 0) FROM orders WHERE OrderType = 'Dine-in' AND YEAR(OrderDate) = {selectedYear - 1}"
                                       Using cmdComp As New MySqlCommand(sqlCompare, conn)
                                           prevYearRevenue = Convert.ToDecimal(cmdComp.ExecuteScalar())
                                       End Using
                                   End If
                               End Using
                           End Sub)

            If totalCount > 0 Then avgValue = totalRevenue / totalCount

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
        End Try
    End Function

    Private Async Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitialLoad Then Return
        Dim currentSearch = TextBoxSearch.Text.Trim()
        If currentSearch = "Search orders..." Then currentSearch = ""
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

    Private Sub ApplyStatusColors()
        Try
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow AndAlso row.Cells("Status").Value IsNot Nothing Then
                    Dim status As String = row.Cells("Status").Value.ToString().Trim().ToLower()
                    row.Cells("Status").Style.ForeColor = Color.FromArgb(44, 62, 80)
                    row.Cells("Status").Style.Font = New Font("Segoe UI", 9, FontStyle.Regular)
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
            DataGridView1.InvalidateColumn(DataGridView1.Columns("Status").Index)
        Catch ex As Exception
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
        Catch
        End Try
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            Me.UseWaitCursor = isLoading
            If btnPrev IsNot Nothing Then btnPrev.Enabled = Not isLoading AndAlso _currentPage > 1
            If btnNext IsNot Nothing Then btnNext.Enabled = Not isLoading AndAlso _currentPage < _totalPageCount
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
        If _currentPage < _totalPageCount Then
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
        _currentPage = 1
        Await BeginLoadDineInOrders()
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
                                                                      query = "SELECT DATE_FORMAT(OrderDate, '%Y-%m-%d %H:00') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Dine-in' AND DATE(OrderDate) = @filterDate GROUP BY HOUR(OrderTime) ORDER BY Period DESC"
                                                                      params.Add(New MySqlParameter("@filterDate", Reports.GlobalFilterDate.ToString("yyyy-MM-dd")))
                                                                  Case "Weekly"
                                                                      ' Week-by-week analysis for the selected month/year
                                                                      If selectedMonth = 0 Then
                                                                          query = "SELECT CONCAT('Week ', WEEK(OrderDate, 1)) AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Dine-in' AND YEAR(OrderDate) = @Year GROUP BY YEARWEEK(OrderDate, 1) ORDER BY YEARWEEK(OrderDate, 1) DESC"
                                                                          params.Add(New MySqlParameter("@Year", selectedYear))
                                                                      Else
                                                                          query = "SELECT CONCAT('Week ', FLOOR((DAY(OrderDate) - 1) / 7) + 1) AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Dine-in' AND YEAR(OrderDate) = @Year AND MONTH(OrderDate) = @Month GROUP BY Period ORDER BY Period DESC"
                                                                          params.Add(New MySqlParameter("@Year", selectedYear))
                                                                          params.Add(New MySqlParameter("@Month", selectedMonth))
                                                                      End If
                                                                  Case "Monthly"
                                                                      If selectedMonth = 0 Then
                                                                          query = "SELECT DATE_FORMAT(OrderDate, '%Y-%m') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Dine-in' AND YEAR(OrderDate) = @Year GROUP BY MONTH(OrderDate) ORDER BY Period DESC"
                                                                          params.Add(New MySqlParameter("@Year", selectedYear))
                                                                      Else
                                                                          query = "SELECT DATE_FORMAT(OrderDate, '%Y-%m-%d') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Dine-in' AND YEAR(OrderDate) = @Year AND MONTH(OrderDate) = @Month GROUP BY DATE(OrderDate) ORDER BY Period DESC"
                                                                          params.Add(New MySqlParameter("@Year", selectedYear))
                                                                          params.Add(New MySqlParameter("@Month", selectedMonth))
                                                                      End If
                                                                  Case "Yearly"
                                                                      query = "SELECT DATE_FORMAT(OrderDate, '%Y-%m') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE OrderType = 'Dine-in' AND YEAR(OrderDate) = @Year GROUP BY MONTH(OrderDate) ORDER BY Period DESC"
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