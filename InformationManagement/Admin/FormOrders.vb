Imports MySqlConnector
Imports System.Data
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing.Drawing2D

Public Class FormOrders
    Private ordersData As New DataTable()
    Private currentFilter As String = "All"
    Private searchText As String = ""
    Private isInitializing As Boolean = True


    ' =======================================================================
    ' FORM LOAD
    ' =======================================================================
    Private Sub FormOrders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            InitializeDataGridView()
            InitializeFilters()

            LoadOrdersData() ' Ensure Grid structure is initialized first
            InitializeCharts()
            RefreshData()
            
            isInitializing = False

            isInitializing = False

        Catch ex As Exception
            MessageBox.Show($"Form Load Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================================
    ' UPDATE STATISTICS FROM DATABASE
    ' =======================================================================
    Private Sub UpdateStatisticsFromDatabase()
        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then
                openConn()
            End If

            ' Get period filter from Reports form
            Dim periodFilter As String = ""
            Dim selectedYear As Integer = Reports.SelectedYear
            Dim selectedMonth As Integer = Reports.SelectedMonth

            Select Case Reports.SelectedPeriod
                Case "Daily"
                    If selectedMonth = 0 Then
                         periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                         periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Weekly"
                    ' Weekly now implies "View Weekly Breakdown for selected Month"
                    If selectedMonth = 0 Then
                         periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                         periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Monthly"
                    If selectedMonth = 0 Then
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If

                Case "Yearly"
                    ' Historical 5-year summary for cards
                    periodFilter = $" WHERE YEAR(OrderDate) <= {selectedYear} AND YEAR(OrderDate) >= {selectedYear - 4} "
                Case Else
                    periodFilter = "" ' All time
            End Select

            ' If Yearly, also get previous year data for comparison
            Dim prevYearRevenue As Decimal = 0
            If Reports.SelectedPeriod = "Yearly" Then
                Dim compareQuery As String = $"SELECT COALESCE(SUM(TotalAmount), 0) FROM orders WHERE YEAR(OrderDate) = {selectedYear - 1}"
                Using cmdComp As New MySqlCommand(compareQuery, conn)
                    prevYearRevenue = Convert.ToDecimal(cmdComp.ExecuteScalar())
                End Using
            End If

            ' Query to get statistics
            Dim statsQuery As String = $"
                SELECT 
                    COUNT(*) AS TotalOrders,
                    COALESCE(SUM(TotalAmount), 0) AS TotalRevenue,
                    COALESCE(AVG(TotalAmount), 0) AS AvgOrderValue,
                    COUNT(CASE WHEN OrderStatus = 'Pending' THEN 1 END) AS PendingCount,
                    COUNT(CASE WHEN OrderStatus = 'Confirmed' THEN 1 END) AS ConfirmedCount,
                    COUNT(CASE WHEN OrderStatus = 'Completed' THEN 1 END) AS CompletedCount,
                    COUNT(CASE WHEN OrderStatus = 'Cancelled' THEN 1 END) AS CancelledCount
                FROM orders
                {periodFilter}
            "

            Using cmd As New MySqlCommand(statsQuery, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Update Total Orders Card (Label4)
                        Dim totalOrders As Integer = Convert.ToInt32(reader("TotalOrders"))
                        Label4.Text = totalOrders.ToString("N0")

                        ' Update Total Revenue Card (Label6)
                        Dim totalRevenue As Decimal = Convert.ToDecimal(reader("TotalRevenue"))
                        Label6.Text = ChrW(&H20B1) & totalRevenue.ToString("#,##0.00")

                        ' Update Average Order Value Card (Label7)
                        Dim avgOrderValue As Decimal = Convert.ToDecimal(reader("AvgOrderValue"))
                        Label7.Text = ChrW(&H20B1) & avgOrderValue.ToString("#,##0.00")

                        ' If Yearly, show 5-yr summary note
                        If Reports.SelectedPeriod = "Yearly" Then
                            Label6.Text &= " (5-yr Total)"
                            If prevYearRevenue > 0 Then
                                Dim diff As Decimal = totalRevenue - prevYearRevenue
                                Dim percent As Decimal = (diff / prevYearRevenue) * 100
                                Dim sign As String = If(diff >= 0, "+", "")
                                Label6.Text &= $" ({sign}{percent:N1}% YoY)"
                            End If
                        End If

                        ' Optional: Store counts for future use
                        Dim pendingCount As Integer = Convert.ToInt32(reader("PendingCount"))
                        Dim confirmedCount As Integer = Convert.ToInt32(reader("ConfirmedCount"))
                        Dim completedCount As Integer = Convert.ToInt32(reader("CompletedCount"))
                        Dim cancelledCount As Integer = Convert.ToInt32(reader("CancelledCount"))
                    End If
                End Using
            End Using

        Catch ex As Exception
            ' If database fails, show default values
            Label4.Text = "0"
            Label6.Text = ChrW(&H20B1) & "0.00"
            Label7.Text = ChrW(&H20B1) & "0.00"

            MessageBox.Show($"Error loading statistics: {ex.Message}", "Warning",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ' =======================================================================
    ' LOAD ORDERS DATA FROM DATABASE
    ' =======================================================================
    Private Sub LoadOrdersData(Optional filterStatus As String = "All", Optional search As String = "")
        Try
            If conn Is Nothing Then
                MessageBox.Show("Database connection not initialized.", "Connection Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)

                Return
            End If

            If conn.State <> ConnectionState.Open Then
                openConn()
            End If

            ' Check if orders table exists
            If Not TableExists("orders") Then
                MessageBox.Show("Orders table not found. Loading sample data.", "Table Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)

                Return
            End If

            ' Build query with filters
            Dim sql As String = BuildOrdersQuery(filterStatus, search)

            Using cmd As New MySqlCommand(sql, conn)
                Using adapter As New MySqlDataAdapter(cmd)
                    ordersData.Clear()
                    adapter.Fill(ordersData)
                End Using
            End Using

            ' Update DataGridView if it exists
            If DataGridView1 IsNot Nothing Then
                ' Reset DataSource and Columns to prevent duplicates
                DataGridView1.DataSource = Nothing
                DataGridView1.Columns.Clear()
                DataGridView1.AutoGenerateColumns = True

                DataGridView1.DataSource = ordersData
                FormatOrdersDataGridView()
            End If

        Catch ex As MySqlException
            MessageBox.Show($"Database Error: {ex.Message}{vbCrLf}Loading sample data instead.",
                          "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    ' =======================================================================
    ' FORMAT ORDERS DATAGRIDVIEW
    ' =======================================================================
    Private Sub FormatOrdersDataGridView()
        Try
            With DataGridView1
                .ReadOnly = True
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .RowHeadersVisible = False
                .RowTemplate.Height = 45
                .ColumnHeadersHeight = 45
                .EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250)
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
                .DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 241, 255)
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .BackgroundColor = Color.White
                .BorderStyle = BorderStyle.None
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .GridColor = Color.FromArgb(233, 236, 239)

                ' Format Columns
                If .Columns.Contains("OrderID") Then
                    .Columns("OrderID").HeaderText = "Order ID"
                    .Columns("OrderID").Width = 100
                    .Columns("OrderID").DefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
                End If

                If .Columns.Contains("OrderDateTime") Then
                    .Columns("OrderDateTime").HeaderText = "Date & Time"
                    .Columns("OrderDateTime").Width = 160
                End If

                If .Columns.Contains("OrderType") Then
                    .Columns("OrderType").HeaderText = "Type"
                    .Columns("OrderType").Width = 100
                    .Columns("OrderType").DefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
                End If

                If .Columns.Contains("Items") Then
                    .Columns("Items").FillWeight = 200
                End If

                If .Columns.Contains("TotalAmount") Then
                    .Columns("TotalAmount").HeaderText = "Total"
                    .Columns("TotalAmount").DefaultCellStyle.Format = ChrW(&H20B1) & "#,##0.00"
                    .Columns("TotalAmount").DefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
                    .Columns("TotalAmount").Width = 100
                End If

                If .Columns.Contains("PaymentMethod") Then
                    .Columns("PaymentMethod").HeaderText = "Payment"
                    .Columns("PaymentMethod").Width = 110
                End If

                If .Columns.Contains("OrderStatus") Then
                    .Columns("OrderStatus").HeaderText = "Status"
                    .Columns("OrderStatus").Width = 120
                End If
            End With
        Catch ex As Exception
            ' Handle formatting errors silently
        End Try
    End Sub

    ' =======================================================================
    ' DATAGRIDVIEW CELL FORMATTING - FOR BADGES AND COLORS
    ' =======================================================================
    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Try
            If e.Value Is Nothing Then Return

            ' Status Badges
            If DataGridView1.Columns(e.ColumnIndex).Name = "OrderStatus" Then
                Dim status As String = e.Value.ToString()
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)

                Select Case status
                    Case "Completed"
                        e.CellStyle.BackColor = Color.FromArgb(0, 200, 83) ' Bright Green
                        e.CellStyle.ForeColor = Color.White
                    Case "Cancelled"
                        e.CellStyle.BackColor = Color.FromArgb(255, 140, 0) ' Orange
                        e.CellStyle.ForeColor = Color.White
                    Case "Refunded"
                        e.CellStyle.BackColor = Color.FromArgb(255, 193, 7) ' Yellow/Gold
                        e.CellStyle.ForeColor = Color.White
                    Case "Pending"
                        e.CellStyle.BackColor = Color.FromArgb(108, 117, 125) ' Grey
                        e.CellStyle.ForeColor = Color.White
                    Case Else
                        e.CellStyle.BackColor = Color.FromArgb(23, 162, 184) ' Cyan
                        e.CellStyle.ForeColor = Color.White
                End Select
            End If

            ' Payment Method Badges
            If DataGridView1.Columns(e.ColumnIndex).Name = "PaymentMethod" Then
                Dim payment As String = e.Value.ToString()
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)

                Select Case payment
                    Case "Cash"
                        e.CellStyle.BackColor = Color.FromArgb(220, 255, 230) ' Light Green
                        e.CellStyle.ForeColor = Color.FromArgb(40, 167, 69)
                    Case "Card", "Credit Card"
                        e.CellStyle.BackColor = Color.FromArgb(255, 235, 238) ' Light Red
                        e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69)
                    Case "GCash", "E-Wallet"
                        e.CellStyle.BackColor = Color.FromArgb(232, 240, 254) ' Light Blue
                        e.CellStyle.ForeColor = Color.FromArgb(26, 115, 232)
                End Select
            End If

            ' Order ID Color
            If DataGridView1.Columns(e.ColumnIndex).Name = "OrderID" Then
                e.CellStyle.ForeColor = Color.FromArgb(26, 115, 232) ' Blue

                ' Add Hash prefix if missing
                If Not e.Value.ToString().StartsWith("#") Then
                    e.Value = "#" & e.Value.ToString()
                    e.FormattingApplied = True
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub

    ' =======================================================================
    ' BUILD ORDERS QUERY
    ' =======================================================================
    Private Function BuildOrdersQuery(filterStatus As String, search As String) As String
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim selectedYear As Integer = Reports.SelectedYear
        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                ' Daily now implies "View Daily Breakdown for selected Month"
                ' So we filter by Year AND Month, just like Monthly
                If selectedMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {selectedYear} AND MONTH(o.OrderDate) = {selectedMonth} "
                End If
            Case "Weekly"
                ' Weekly now implies "View Weekly Breakdown for selected Month"
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
            Case "All Time"
                periodFilter = ""
        End Select


        ' Simplified query - match the design from the image
        Dim sql As String = "
            SELECT 
                o.OrderID,
                CONCAT(DATE_FORMAT(o.OrderDate, '%Y-%m-%d'), ' ', TIME_FORMAT(o.OrderTime, '%h:%i %p')) AS OrderDateTime,
                o.OrderType,
                (
                    SELECT GROUP_CONCAT(ProductName SEPARATOR ', ')
                    FROM order_items
                    WHERE OrderID = o.OrderID
                ) AS Items,
                o.TotalAmount,
                (SELECT COALESCE(PaymentMethod, 'Cash') FROM payments WHERE OrderID = o.OrderID LIMIT 1) AS PaymentMethod,
                o.OrderStatus
            FROM orders o
            WHERE 1=1
        "

        sql &= periodFilter

        If filterStatus <> "All" AndAlso Not String.IsNullOrEmpty(filterStatus) Then
            sql &= $" AND o.OrderStatus = '{filterStatus}'"
        End If

        If Not String.IsNullOrEmpty(search) Then
            sql &= $" AND (o.OrderID LIKE '%{search}%' OR o.OrderStatus LIKE '%{search}%' OR o.OrderType LIKE '%{search}%')"
        End If

        sql &= " ORDER BY o.OrderDate DESC, o.OrderTime DESC LIMIT 100"

        Return sql
    End Function

    ' =======================================================================
    ' REFRESH DATA - Public method to refresh all data
    ' =======================================================================
    Public Sub RefreshData()
        Try
            ' Reload statistics
            UpdateStatisticsFromDatabase()

            ' Reload orders data
            LoadOrdersData(currentFilter, searchText)

            ' Load breakdown data
            LoadOrderBreakdownAsync()

            ' Refresh charts
            LoadOrdersTrendChart()
            LoadCategoriesChart()

        Catch ex As Exception
            MessageBox.Show($"Error refreshing data: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================================
    ' CHECK IF TABLE EXISTS
    ' =======================================================================
    Private Function TableExists(tableName As String) As Boolean
        Try
            Dim sql As String = $"
                SELECT COUNT(*) 
                FROM information_schema.tables 
                WHERE table_schema = DATABASE()
                  AND table_name = '{tableName}'
            "
            Using cmd As New MySqlCommand(sql, conn)
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        Catch
            Return False
        End Try
    End Function

    ' =======================================================================
    ' INITIALIZE DATAGRIDVIEW
    ' =======================================================================
    Private Sub InitializeDataGridView()
        ' DataGridView is already defined in the designer
        ' Just set basic properties if needed
        If DataGridView1 IsNot Nothing Then
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.AllowUserToDeleteRows = False
            DataGridView1.ReadOnly = True
        End If
    End Sub

    ' =======================================================================
    ' INITIALIZE FILTERS
    ' =======================================================================
    Private Sub InitializeFilters()
        ' Add filter initialization if needed
    End Sub

    ' Private Sub ConfigureDateFilter()

    Private Sub RoundCorners(control As Control, radius As Integer)
        Dim path As New System.Drawing.Drawing2D.GraphicsPath()
        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(control.Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90)
        path.AddArc(0, control.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        control.Region = New Region(path)
    End Sub


    ' =======================================================================
    ' INITIALIZE CHARTS
    ' =======================================================================
    Private Sub InitializeCharts()
        Try
            ' Configure Monthly Chart (Trends)
            With MonthlyChartOrder
                .Series.Clear()
                .ChartAreas(0).AxisX.MajorGrid.Enabled = False
                .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 240)
                .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Segoe UI", 8)
                .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Segoe UI", 8)

                Dim seriesTrend As New Series("Orders")
                seriesTrend.ChartType = SeriesChartType.SplineArea
                seriesTrend.Color = Color.FromArgb(100, 78, 115, 223) ' Semi-transparent blue
                seriesTrend.BorderWidth = 3
                seriesTrend.BorderColor = Color.FromArgb(78, 115, 223)
                seriesTrend.YAxisType = AxisType.Primary

                Dim seriesRevenue As New Series("Revenue")
                seriesRevenue.ChartType = SeriesChartType.Spline
                seriesRevenue.Color = Color.FromArgb(249, 115, 22) ' Orange
                seriesRevenue.BorderWidth = 3
                seriesRevenue.YAxisType = AxisType.Secondary
                seriesRevenue.MarkerStyle = MarkerStyle.Circle

                .Series.Add(seriesTrend)
                .Series.Add(seriesRevenue)

                ' Configure Axis
                .ChartAreas(0).AxisY.Title = "Orders"
                .ChartAreas(0).AxisY2.Enabled = AxisEnabled.True
                .ChartAreas(0).AxisY2.Title = "Revenue"
                .ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                
                If .Legends.Count = 0 Then .Legends.Add(New Legend("Default"))
                .Legends(0).Docking = Docking.Top
            End With

            ' Configure Categories Chart (Orders By Type)
            With OrderCategoriesGraph
                .Series.Clear()
                .Titles.Clear()
                .Legends.Clear()

                .ChartAreas(0).BackColor = Color.White
                .ChartAreas(0).AxisX.MajorGrid.Enabled = False
                .ChartAreas(0).AxisY.MajorGrid.Enabled = True
                .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 240)
                .ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash

                .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Segoe UI", 9)
                .ChartAreas(0).AxisX.LabelStyle.ForeColor = Color.DimGray
                .ChartAreas(0).AxisX.LineWidth = 0

                .ChartAreas(0).AxisY.LabelStyle.Enabled = False
                .ChartAreas(0).AxisY.LineWidth = 0

                .Titles.Add("Orders by Type")
                .Titles(0).Font = New Font("Segoe UI", 12, FontStyle.Bold)
                .Titles(0).Alignment = ContentAlignment.TopLeft
                .Titles(0).ForeColor = Color.FromArgb(50, 50, 50)

                Dim seriesType As New Series("OrderTypes")
                seriesType.ChartType = SeriesChartType.Column
                seriesType("PointWidth") = "0.4"

                .Series.Add(seriesType)
            End With

        Catch ex As Exception
            ' Handle silently or show basic warning
        End Try
    End Sub

    ' =======================================================================
    ' LOAD ORDERS TREND CHART
    ' =======================================================================
    Private Sub LoadOrdersTrendChart()
        Try
            If conn.State <> ConnectionState.Open Then openConn()

            ' Use shared date filter logic
            Dim dateGrouping As String = ""
            Dim periodValue As String = ""

            Select Case Reports.SelectedPeriod
                Case "Daily"
                    dateGrouping = "DATE_FORMAT(OrderDate, '%m/%d')"
                Case "Weekly"
                    ' Done below in period filter section to include WHERE clause logic if needed, but here just format
                    dateGrouping = "CONCAT('Week ', FLOOR((DAY(OrderDate) - 1) / 7) + 1)"
                Case "Monthly"
                    dateGrouping = "DATE_FORMAT(OrderDate, '%b')"
                Case "Yearly"
                    dateGrouping = "YEAR(OrderDate)"
                Case Else
                    dateGrouping = "DATE_FORMAT(OrderDate, '%m/%d')"
            End Select

            ' Get period filter from Reports form
            Dim periodFilter As String = ""
            Dim selectedYear As Integer = Reports.SelectedYear
            Dim selectedMonth As Integer = Reports.SelectedMonth

            Select Case Reports.SelectedPeriod
                Case "Daily"
                    If selectedMonth = 0 Then
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Weekly"
                    ' Filter by Month for Weekly view
                    If selectedMonth = 0 Then
                         periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                         periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Monthly"
                    If selectedMonth = 0 Then
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Yearly"
                    ' Comparison: Last 5 years including selected year
                    periodFilter = $" WHERE YEAR(OrderDate) <= {selectedYear} AND YEAR(OrderDate) >= {selectedYear - 4} "
            End Select

            Dim sql As String = $"
                SELECT 
                    {dateGrouping} AS Period, 
                    COUNT(*) AS OrderCount,
                    COALESCE(SUM(TotalAmount), 0) AS Revenue
                FROM orders
                {periodFilter}
                GROUP BY {dateGrouping}
                ORDER BY MIN(OrderDate)
            "

            MonthlyChartOrder.Series("Orders").Points.Clear()
            MonthlyChartOrder.Series("Revenue").Points.Clear()

            Using cmd As New MySqlCommand(sql, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim period As String = reader("Period").ToString()
                        Dim count As Integer = Convert.ToInt32(reader("OrderCount"))
                        Dim rev As Decimal = Convert.ToDecimal(reader("Revenue"))

                        MonthlyChartOrder.Series("Orders").Points.AddXY(period, count)
                        MonthlyChartOrder.Series("Revenue").Points.AddXY(period, rev)
                    End While
                End Using
            End Using

        Catch ex As Exception
            ' Load sample points if db fails
            MonthlyChartOrder.Series("Orders").Points.AddXY("Mon", 10)
            MonthlyChartOrder.Series("Orders").Points.AddXY("Tue", 15)
            MonthlyChartOrder.Series("Orders").Points.AddXY("Wed", 12)
        End Try
    End Sub

    ' =======================================================================
    ' LOAD CATEGORIES CHART -> ORDERS BY TYPE (COLUMN CHART)
    ' =======================================================================
    Private Sub LoadCategoriesChart()
        Try
            If conn.State <> ConnectionState.Open Then openConn()

            ' Get period filter from Reports form
            Dim periodFilter As String = ""
            Dim selectedYear As Integer = Reports.SelectedYear
            Dim selectedMonth As Integer = Reports.SelectedMonth

            Select Case Reports.SelectedPeriod
                Case "Daily", "Weekly"
                    If selectedMonth = 0 Then
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Monthly"
                    If selectedMonth = 0 Then
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} "
                    Else
                        periodFilter = $" WHERE YEAR(OrderDate) = {selectedYear} AND MONTH(OrderDate) = {selectedMonth} "
                    End If
                Case "Yearly"
                    periodFilter = $" WHERE YEAR(OrderDate) <= {selectedYear} AND YEAR(OrderDate) >= {selectedYear - 4} "
                Case Else
                    periodFilter = "" ' All time - no filter
            End Select

            ' DEBUG: First, let's see what's actually in the database
            Dim debugSql As String = $"
            SELECT OrderType, OrderSource, COUNT(*) AS Count
            FROM orders
            {periodFilter}
            GROUP BY OrderType, OrderSource
        "

            Debug.WriteLine("=== DEBUG: Order Type Distribution ===")
            Using debugCmd As New MySqlCommand(debugSql, conn)
                Using debugReader As MySqlDataReader = debugCmd.ExecuteReader()
                    While debugReader.Read()
                        Debug.WriteLine($"OrderType: {debugReader("OrderType")}, OrderSource: {debugReader("OrderSource")}, Count: {debugReader("Count")}")
                    End While
                End Using
            End Using

            ' Main query - Group by Year and OrderType for comparison
            Dim sql As String = ""
            If Reports.SelectedPeriod = "Yearly" Then
                sql = $"
                    SELECT 
                        YEAR(OrderDate) AS StatYear,
                        TRIM(OrderType) AS OrderType, 
                        COUNT(*) AS Count
                    FROM orders
                    {periodFilter}
                    GROUP BY YEAR(OrderDate), TRIM(OrderType)
                    ORDER BY YEAR(OrderDate) ASC, OrderType ASC
                "
            Else
                sql = $"
                    SELECT 
                        TRIM(OrderType) AS OrderType, 
                        COUNT(*) AS Count
                    FROM orders
                    {periodFilter}
                    GROUP BY TRIM(OrderType)
                    ORDER BY OrderType ASC
                "
            End If

            ' Bind to Chart
            OrderCategoriesGraph.Series.Clear()

            If Reports.SelectedPeriod = "Yearly" Then
                ' Yearly Comparison View (5 years)
                Dim years As New List(Of Integer)
                For i As Integer = 0 To 4
                    years.Add(selectedYear - (4 - i))
                Next
                
                For Each yr In years
                    Dim seriesName As String = yr.ToString()
                    Dim series = New Series(seriesName)
                    series.ChartType = SeriesChartType.Column
                    series("PointWidth") = "0.4"
                    OrderCategoriesGraph.Series.Add(series)

                    ' Fill data for this year
                    Dim typeCounts As New Dictionary(Of String, Integer) From {
                        {"Dine-in", 0},
                        {"Takeout", 0},
                        {"Online", 0}
                    }

                    Using cmd As New MySqlCommand(sql, conn)
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                If Not IsDBNull(reader("StatYear")) AndAlso Convert.ToInt32(reader("StatYear")) = yr Then
                                    Dim orderType As String = reader("OrderType").ToString()
                                    Dim count As Integer = Convert.ToInt32(reader("Count"))

                                    If typeCounts.ContainsKey(orderType) Then
                                        typeCounts(orderType) = count
                                    Else
                                        For Each key In typeCounts.Keys.ToList()
                                            If String.Equals(key, orderType, StringComparison.OrdinalIgnoreCase) Then
                                                typeCounts(key) = count
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            End While
                        End Using
                    End Using

                    ' Add points to this series
                    For Each kvp In typeCounts.OrderBy(Function(x) x.Key)
                        Dim idx As Integer = series.Points.AddXY(kvp.Key, kvp.Value)
                        
                        Dim baseColor As Color
                        Select Case kvp.Key
                            Case "Dine-in" : baseColor = Color.FromArgb(88, 86, 214)
                            Case "Takeout" : baseColor = Color.FromArgb(149, 209, 36)
                            Case "Online" : baseColor = Color.FromArgb(255, 149, 0)
                            Case Else : baseColor = Color.FromArgb(128, 128, 128)
                        End Select

                        If yr <> Reports.SelectedYear Then
                            series.Points(idx).Color = Color.FromArgb(100 + (20 * years.IndexOf(yr)), baseColor.R, baseColor.G, baseColor.B)
                        Else
                            series.Points(idx).Color = baseColor
                        End If

                        series.Points(idx).Label = kvp.Value.ToString()
                        series.Points(idx).Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    Next
                Next

                If OrderCategoriesGraph.Legends.Count = 0 Then
                    Dim lgd As New Legend("Default")
                    lgd.Docking = Docking.Bottom
                    OrderCategoriesGraph.Legends.Add(lgd)
                End If
                OrderCategoriesGraph.Titles(0).Text = $"Order Type Distribution ({selectedYear - 4} - {selectedYear})"

            Else
                ' Standard View (Single series)
                Dim seriesType As New Series("OrderTypes")
                seriesType.ChartType = SeriesChartType.Column
                seriesType("PointWidth") = "0.4"
                OrderCategoriesGraph.Series.Add(seriesType)

                Dim typeCounts As New Dictionary(Of String, Integer) From {
                    {"Dine-in", 0},
                    {"Takeout", 0},
                    {"Online", 0}
                }

                Using cmd As New MySqlCommand(sql, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            If Not IsDBNull(reader("OrderType")) Then
                                Dim orderType As String = reader("OrderType").ToString()
                                Dim count As Integer = Convert.ToInt32(reader("Count"))

                                If typeCounts.ContainsKey(orderType) Then
                                    typeCounts(orderType) = count
                                Else
                                    For Each key In typeCounts.Keys.ToList()
                                        If String.Equals(key, orderType, StringComparison.OrdinalIgnoreCase) Then
                                            typeCounts(key) = count
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        End While
                    End Using
                End Using

                With seriesType
                    .Points.Clear()
                    For Each kvp In typeCounts.OrderBy(Function(x) x.Key)
                        Dim idx As Integer = .Points.AddXY(kvp.Key, kvp.Value)
                        Select Case kvp.Key
                            Case "Dine-in" : .Points(idx).Color = Color.FromArgb(88, 86, 214)
                            Case "Takeout" : .Points(idx).Color = Color.FromArgb(149, 209, 36)
                            Case "Online" : .Points(idx).Color = Color.FromArgb(255, 149, 0)
                            Case Else : .Points(idx).Color = Color.FromArgb(128, 128, 128)
                        End Select
                        .Points(idx).Label = kvp.Value.ToString()
                        .Points(idx).LabelForeColor = Color.FromArgb(50, 50, 50)
                        .Points(idx).Font = New Font("Segoe UI", 10, FontStyle.Bold)
                    Next
                End With
                
                If OrderCategoriesGraph.Legends.Count > 0 Then OrderCategoriesGraph.Legends.Clear()
                OrderCategoriesGraph.Titles(0).Text = "Orders by Type"
            End If

            ' Force chart refresh
            OrderCategoriesGraph.Invalidate()

        Catch ex As Exception
            Debug.WriteLine($"Chart Error: {ex.Message}")
            ' Safe Fallback
            If OrderCategoriesGraph.Series.IndexOf("OrderTypes") <> -1 Then
                With OrderCategoriesGraph.Series("OrderTypes")
                    .Points.Clear()
                    Dim idx1 As Integer = .Points.AddXY("Dine-in", 0)
                    .Points(idx1).Color = Color.FromArgb(88, 86, 214)
                    .Points(idx1).Label = "0"
                End With
            End If
        End Try
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
                                                          If conn.State <> ConnectionState.Open Then openConn()
                                                          Dim query As String = ""
                                                          Dim params As New List(Of MySqlParameter)

                                                          Select Case selectedPeriod
                                                              Case "Daily"
                                                                  query = "SELECT DATE_FORMAT(OrderDate, '%Y-%m-%d') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE YEAR(OrderDate) = @Year AND MONTH(OrderDate) = @Month GROUP BY DATE(OrderDate) ORDER BY Period DESC"
                                                                  params.Add(New MySqlParameter("@Year", selectedYear))
                                                                  params.Add(New MySqlParameter("@Month", selectedMonth))
                                                              Case "Weekly"
                                                                  ' Week of Month Logic
                                                                  query = "SELECT CONCAT('Week ', FLOOR((DAY(OrderDate) - 1) / 7) + 1) AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE YEAR(OrderDate) = @Year AND MONTH(OrderDate) = @Month GROUP BY Period ORDER BY Period DESC"
                                                                  params.Add(New MySqlParameter("@Year", selectedYear))
                                                                  params.Add(New MySqlParameter("@Month", selectedMonth))
                                                              Case "Monthly"
                                                                  query = "SELECT DATE_FORMAT(OrderDate, '%M') AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE YEAR(OrderDate) = @Year GROUP BY MONTH(OrderDate) ORDER BY MONTH(OrderDate) DESC"
                                                                  params.Add(New MySqlParameter("@Year", selectedYear))
                                                              Case "Yearly"
                                                                  query = "SELECT YEAR(OrderDate) AS Period, COUNT(*) AS OrderCount, SUM(TotalAmount) AS TotalRevenue, AVG(TotalAmount) AS AvgValue FROM orders WHERE YEAR(OrderDate) <= @Year AND YEAR(OrderDate) >= @Year - 4 GROUP BY YEAR(OrderDate) ORDER BY Period DESC"
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