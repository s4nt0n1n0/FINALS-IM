Imports System.Windows.Forms.DataVisualization.Charting
Imports MySqlConnector

Public Class FormSales

    Private currentYear As Integer = DateTime.Now.Year
    Private currentMonth As Integer = DateTime.Now.Month
    Private salesData As New Dictionary(Of String, (Revenue As Decimal, Expenses As Decimal, Profit As Decimal))
    Private currentPeriod As String = "Daily"
    Private isInitializing As Boolean = True


    ' =======================================================================
    ' FORM LOAD
    ' =======================================================================
    Private Sub FormSales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If Panel1 IsNot Nothing Then
                Panel1.Visible = False
                Panel1.SendToBack()
            End If

            

            RoundedPane21.BringToFront()
            RoundedPane22.BringToFront()
            RoundedPane23.BringToFront()
            RoundedPane24.BringToFront()

            ' Synchronize with Reports global filters
            currentPeriod = Reports.SelectedPeriod
            currentYear = Reports.SelectedYear
            currentMonth = Reports.SelectedMonth

        ' ConfigureDateFilter()

            ConfigureChart()
            LoadAndDisplaySalesData()
            UpdateSummaryCards()

            ' Update label to show current period
            UpdateHeaderLabel()

            isInitializing = False

        Catch ex As Exception
            MessageBox.Show($"Form Load Error: {ex.Message}{vbCrLf}{ex.StackTrace}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Function GetLatestYearWithData() As Integer
        Try
            ' Try to open connection if not already open
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then
                Try
                    openConn()
                Catch
                    ' If connection fails, default to current year
                    Return DateTime.Now.Year
                End Try
            End If

            ' Check for most recent year in orders table
            Dim sql = "SELECT MAX(YEAR(OrderDate)) FROM orders WHERE OrderDate IS NOT NULL"
            Using cmd As New MySqlCommand(sql, conn)
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    Dim latestYear = Convert.ToInt32(result)
                    ' If latest year is valid, use it
                    If latestYear > 2000 Then
                        Return latestYear
                    End If
                End If
            End Using

            ' Also check payments if orders is empty
            If TableExists("payments") Then
                sql = "SELECT MAX(YEAR(PaymentDate)) FROM payments WHERE PaymentDate IS NOT NULL AND ReservationID IS NOT NULL"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Dim latestYear = Convert.ToInt32(result)
                        If latestYear > 2000 Then
                            Return latestYear
                        End If
                    End If
                End Using
            End If

        Catch ex As Exception
            ' If error occurs, just use current year
            MessageBox.Show($"Note: Using current year. Unable to detect data year: {ex.Message}",
                          "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

        ' Default to current year if no data found
        Return DateTime.Now.Year
    End Function

    ' =======================================================================
    ' CHECK IF YEAR HAS DATA
    ' =======================================================================
    Private Function HasDataForYear(year As Integer) As Boolean
        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then
                Return False
            End If

            ' Check orders table
            Dim sql = "SELECT COUNT(*) FROM orders WHERE YEAR(OrderDate) = @Year"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Year", year)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    Return True
                End If
            End Using

            ' Check payments if available
            If TableExists("payments") Then
                sql = "SELECT COUNT(*) FROM payments WHERE YEAR(PaymentDate) = @Year AND ReservationID IS NOT NULL"
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Year", year)
                    If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                        Return True
                    End If
                End Using
            End If

            Return False

        Catch
            Return False
        End Try
    End Function

    ' =======================================================================
    ' UPDATE HEADER LABEL
    ' =======================================================================
    Private Sub UpdateHeaderLabel()
        Try
            If Label1 IsNot Nothing Then
                Dim monthPart As String = ""
                If currentPeriod = "Daily" AndAlso Reports.SelectedMonth > 0 Then
                    monthPart = " - " & System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Reports.SelectedMonth)
                End If
                Label1.Text = $"Financial Overview - {currentPeriod}{monthPart} ({currentYear})"
            End If
        Catch ex As Exception
            ' Silently handle if Label1 doesn't exist
        End Try
    End Sub

    ' =======================================================================
    ' CHART CONFIG
    ' =======================================================================
    Private Sub ConfigureChart()
        Try
            With Chart1
                .ChartAreas(0).AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                .ChartAreas(0).AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot
                .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Segoe UI", 9)
                .ChartAreas(0).AxisX.Interval = 1

                ' Angle labels for better readability based on period
                If currentPeriod = "Daily" Then
                    .ChartAreas(0).AxisX.LabelStyle.Angle = -45
                Else
                    .ChartAreas(0).AxisX.LabelStyle.Angle = 0
                End If

                .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                .ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot
                .ChartAreas(0).AxisY.LabelStyle.Format = "₱{0:N0}"
                .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Segoe UI", 9)

                .Series("Revenue").Color = Color.FromArgb(99, 102, 241)
                .Series("Expenses").Color = Color.FromArgb(239, 68, 68)
                .Series("NetProfit").Color = Color.FromArgb(34, 197, 94)

                For Each series As Series In .Series
                    series.ChartType = SeriesChartType.Column
                    series.BorderWidth = 0
                    series("PointWidth") = "0.6"
                    series.ToolTip = "#VALX: ₱#VALY{N2}"
                Next

                .Legends(0).Font = New Font("Segoe UI", 9)
                .Legends(0).Docking = Docking.Bottom
            End With

        Catch ex As Exception
            MessageBox.Show($"Chart Config Error: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================================
    ' MAIN LOAD FUNCTION
    ' =======================================================================
    Private Sub LoadAndDisplaySalesData()
        Try
            If conn Is Nothing Then
                MessageBox.Show("Database connection is missing.", "Connection Error")
                LoadSampleData()
                Return
            End If

            If conn.State <> ConnectionState.Open Then
                Try
                    openConn()
                Catch
                    MessageBox.Show("Unable to open DB connection.")
                    LoadSampleData()
                    Return
                End Try
            End If

            If Not TablesExist() Then
                MessageBox.Show("Required tables not found. Showing sample data.")
                LoadSampleData()
                Return
            End If

            Dim sql As String = BuildSalesQuery()

            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Year", currentYear)
                cmd.Parameters.AddWithValue("@Month", currentMonth)

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    InitializeChartData()

                    Dim hasRows As Boolean = False

                    While reader.Read()
                        hasRows = True

                        Dim periodValue As Object = reader("PeriodGroup")
                        Dim periodLabel As String = GetPeriodLabel(periodValue)
                        
                        ' For multi-year, the label might be repeated in chart logic if not careful
                        ' but we use periodLabel as the key in salesData
                        
                        Dim revenue As Decimal = If(IsDBNull(reader("TotalRevenue")), 0D, Convert.ToDecimal(reader("TotalRevenue")))
                        Dim expenses As Decimal = If(IsDBNull(reader("TotalExpenses")), 0D, Convert.ToDecimal(reader("TotalExpenses")))
                        Dim profit As Decimal = revenue - expenses

                        salesData(periodLabel) = (revenue, expenses, profit)

                        ' Add data points to chart
                        Chart1.Series("Revenue").Points.AddXY(periodLabel, revenue)
                        Chart1.Series("Expenses").Points.AddXY(periodLabel, expenses)
                        Chart1.Series("NetProfit").Points.AddXY(periodLabel, profit)
                    End While

                    If Not hasRows Then
                        MessageBox.Show($"No sales data found for {currentPeriod} period in {currentYear}. Showing sample data.",
                                      "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadSampleData()
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading sales data: " & ex.Message & vbCrLf & ex.StackTrace)
            LoadSampleData()
        End Try
    End Sub

    ' =======================================================================
    ' GET PERIOD LABEL FOR DISPLAY
    ' =======================================================================
    Private Function GetPeriodLabel(periodValue As Object) As String
        If IsDBNull(periodValue) OrElse periodValue Is Nothing Then Return "N/A"

        Try
            Select Case currentPeriod
                Case "Daily"
                    ' Format: "Jan 15"
                    Return Convert.ToDateTime(periodValue).ToString("MMM dd")

                Case "Weekly"
                    ' Format: "Week 52"
                    Return $"Week {periodValue}"

                Case "Monthly"
                    ' periodValue will be month number (1-12)
                    Dim monthNum As Integer = Convert.ToInt32(periodValue)
                    Return New DateTime(currentYear, monthNum, 1).ToString("MMM")

                Case "Yearly"
                    ' periodValue will be the year number
                    Return periodValue.ToString()

                Case Else
                    Return periodValue.ToString()
            End Select
        Catch ex As Exception
            Return "Invalid"
        End Try
    End Function

    ' =======================================================================
    ' TABLE CHECKER
    ' =======================================================================
    Private Function TablesExist() As Boolean
        ' We primarily rely on orders and payments now, which should exist.
        Return TableExists("orders") OrElse
               TableExists("payments")
    End Function

    Private Function TableExists(tableName As String) As Boolean
        Try
            Dim sql = "
                SELECT COUNT(*) FROM information_schema.tables
                WHERE table_schema = DATABASE()
                AND LOWER(table_name) = LOWER(@TableName)
            "

            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@TableName", tableName)
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using

        Catch
            Return False
        End Try
    End Function

    ' =======================================================================
    ' SALES QUERY BUILDER - WITH PERIOD FILTER
    ' =======================================================================
    Private Function BuildSalesQuery() As String
        Dim q As New List(Of String)

        ' ORDERS TABLE - Revenue (Matches Dashboard Logic)
        Dim orderDateGrouping As String = GetDateGroupingForColumn("OrderDate")
        Dim orderMonthFilter As String = ""
        If currentPeriod = "Daily" Then
            orderMonthFilter = " AND MONTH(OrderDate) = @Month "
        End If

        q.Add($"
            SELECT {orderDateGrouping} AS PeriodGroup, TotalAmount AS Amount, 'Revenue' AS Type
            FROM orders
            WHERE OrderStatus = 'Completed'
            {GetYearFilter("OrderDate")}
            {orderMonthFilter}
        ")

        ' PAYMENTS TABLE - Revenue (Only for Reservations/Catering to avoid double-counting POS)
        If TableExists("payments") Then
            Dim resDateGrouping As String = GetDateGroupingForColumn("PaymentDate")
            Dim resMonthFilter As String = ""
            If currentPeriod = "Daily" Then
                resMonthFilter = " AND MONTH(PaymentDate) = @Month "
            End If

            q.Add($"
                SELECT {resDateGrouping} AS PeriodGroup, AmountPaid AS Amount, 'Revenue' AS Type
                FROM payments
                WHERE ReservationID IS NOT NULL 
                AND PaymentStatus = 'Completed'
                {GetYearFilter("PaymentDate")}
                {resMonthFilter}
            ")
        End If

        ' INVENTORY_BATCHES TABLE - Expenses
        If TableExists("inventory_batches") Then
            Dim purchaseDateGrouping As String = GetDateGroupingForColumn("PurchaseDate")
            Dim purchaseMonthFilter As String = ""
            If currentPeriod = "Daily" Then
                purchaseMonthFilter = " AND MONTH(PurchaseDate) = @Month "
            End If

            q.Add($"
                SELECT {purchaseDateGrouping} AS PeriodGroup, TotalCost AS Amount, 'Expenses' AS Type
                FROM inventory_batches
                WHERE BatchStatus = 'Active'
                {GetYearFilter("PurchaseDate")}
                {purchaseMonthFilter}
            ")
        End If

        If q.Count = 0 Then Throw New Exception("No valid tables found.")

        Return $"
            SELECT 
                PeriodGroup,
                SUM(CASE WHEN Type='Revenue' THEN Amount ELSE 0 END) AS TotalRevenue,
                SUM(CASE WHEN Type='Expenses' THEN Amount ELSE 0 END) AS TotalExpenses
            FROM ({String.Join(" UNION ALL ", q)}) AS c
            WHERE PeriodGroup IS NOT NULL
            GROUP BY PeriodGroup 
            ORDER BY PeriodGroup
        "
    End Function

    ' =======================================================================
    ' GET DATE GROUPING FOR SQL COLUMN
    ' =======================================================================

    ' Helper to handle multi-year filter
    Private Function GetYearFilter(columnName As String) As String
        If currentPeriod = "Yearly" Then
            Return $" AND YEAR({columnName}) <= @Year AND YEAR({columnName}) >= @Year - 4 "
        Else
            Return $" AND YEAR({columnName}) = @Year "
        End If
    End Function

    ' =======================================================================
    ' INITIAL EMPTY CHART
    ' =======================================================================
    Private Sub InitializeChartData()
        Chart1.Series("Revenue").Points.Clear()
        Chart1.Series("Expenses").Points.Clear()
        Chart1.Series("NetProfit").Points.Clear()
        salesData.Clear()
    End Sub

    ' =======================================================================
    ' SAMPLE DATA (if DB fails)
    ' =======================================================================
    Private Sub LoadSampleData()
        InitializeChartData()

        Select Case currentPeriod
            Case "Monthly"
                ' Monthly sample data
                Dim sample = New Dictionary(Of Integer, (Decimal, Decimal)) From {
                    {1, (2250000, 1600000)},
                    {2, (2600000, 1750000)},
                    {3, (2400000, 1650000)},
                    {4, (3050000, 1900000)},
                    {5, (2750000, 1800000)},
                    {6, (3350000, 2050000)}
                }

                For Each kv In sample
                    Dim name As String = New DateTime(currentYear, kv.Key, 1).ToString("MMM")
                    Dim revenue = kv.Value.Item1
                    Dim expenses = kv.Value.Item2
                    Dim profit = revenue - expenses

                    salesData(name) = (revenue, expenses, profit)
                    Chart1.Series("Revenue").Points.AddXY(name, revenue)
                    Chart1.Series("Expenses").Points.AddXY(name, expenses)
                    Chart1.Series("NetProfit").Points.AddXY(name, profit)
                Next

            Case "Daily"
                ' Show last 7 days sample
                For i As Integer = 6 To 0 Step -1
                    Dim dt As DateTime = DateTime.Now.AddDays(-i)
                    Dim name As String = dt.ToString("MMM dd")
                    Dim revenue As Decimal = 50000 + (i * 5000)
                    Dim expenses As Decimal = 35000 + (i * 3000)
                    Dim profit As Decimal = revenue - expenses

                    salesData(name) = (revenue, expenses, profit)
                    Chart1.Series("Revenue").Points.AddXY(name, revenue)
                    Chart1.Series("Expenses").Points.AddXY(name, expenses)
                    Chart1.Series("NetProfit").Points.AddXY(name, profit)
                Next

            Case "Weekly"
                ' Show last 8 weeks sample
                For i As Integer = 1 To 8
                    Dim name As String = $"Week {i}"
                    Dim revenue As Decimal = 300000 + (i * 20000)
                    Dim expenses As Decimal = 200000 + (i * 15000)
                    Dim profit As Decimal = revenue - expenses

                    salesData(name) = (revenue, expenses, profit)
                    Chart1.Series("Revenue").Points.AddXY(name, revenue)
                    Chart1.Series("Expenses").Points.AddXY(name, expenses)
                    Chart1.Series("NetProfit").Points.AddXY(name, profit)
                Next

            Case "Yearly"
                ' Show last 5 years sample
                For i As Integer = 4 To 0 Step -1
                    Dim year As Integer = currentYear - i
                    Dim name As String = year.ToString()
                    Dim revenue As Decimal = 20000000 + (i * 2000000)
                    Dim expenses As Decimal = 15000000 + (i * 1500000)
                    Dim profit As Decimal = revenue - expenses

                    salesData(name) = (revenue, expenses, profit)
                    Chart1.Series("Revenue").Points.AddXY(name, revenue)
                    Chart1.Series("Expenses").Points.AddXY(name, expenses)
                    Chart1.Series("NetProfit").Points.AddXY(name, profit)
                Next
        End Select
    End Sub

    ' =======================================================================
    ' SUMMARY CARDS - WITH PERIOD FILTER APPLIED TO ALL DATA
    ' =======================================================================
    Private Sub UpdateSummaryCards()
        Try
            ' Get filtered revenue and expenses from database
            Dim filteredRevenue As Decimal = GetFilteredRevenue()
            Dim filteredExpenses As Decimal = GetFilteredExpenses()
            Dim filteredProfit As Decimal = filteredRevenue - filteredExpenses

            ' Update labels if they exist
            If lblTotalRevenue IsNot Nothing Then
                lblTotalRevenue.Text = $"₱{filteredRevenue:N2}"
            End If

            If Label11 IsNot Nothing Then
                Label11.Text = $"₱{filteredExpenses:N2}"
            End If

            If Label14 IsNot Nothing Then
                Label14.Text = $"₱{filteredProfit:N2}"
            End If

            ' If Yearly, show YoY change in Revenue card
            If currentPeriod = "Yearly" AndAlso lblTotalRevenue IsNot Nothing Then
                Dim prevYearRevenue As Decimal = GetSingleYearRevenue(currentYear - 1)
                Dim currYearRevenue As Decimal = GetSingleYearRevenue(currentYear)
                
                If prevYearRevenue > 0 Then
                    Dim diff As Decimal = currYearRevenue - prevYearRevenue
                    Dim percent As Decimal = (diff / prevYearRevenue) * 100
                    Dim sign As String = If(diff >= 0, "+", "")
                    lblTotalRevenue.Text &= $" ({sign}{percent:N1}%)"
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error updating summary cards: " & ex.Message,
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


    ' =======================================================================
    ' GET FILTERED REVENUE - ALL SOURCES WITH PERIOD FILTER
    ' =======================================================================
    Private Function GetFilteredRevenue() As Decimal
        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then
                Return 0D
            End If

            Dim q As New List(Of String)

            ' ORDERS TABLE - Revenue (Matches Dashboard)
            Dim whereClauseOrders As String = GetPeriodWhereClause("OrderDate")
            q.Add($"
                SELECT COALESCE(SUM(TotalAmount), 0) AS Amount
                FROM orders
                WHERE OrderStatus = 'Completed'
                {GetYearFilter("OrderDate")} 
                {If(currentPeriod = "Daily", " AND MONTH(OrderDate) = @Month ", "")}
            ")

            ' PAYMENTS TABLE - Revenue
            If TableExists("payments") Then
                q.Add($"
                    SELECT COALESCE(SUM(AmountPaid), 0) AS Amount
                    FROM payments
                    WHERE ReservationID IS NOT NULL 
                    AND PaymentStatus = 'Completed'
                    {GetYearFilter("PaymentDate")}
                    {If(currentPeriod = "Daily", " AND MONTH(PaymentDate) = @Month ", "")}
                ")
            End If

            If q.Count = 0 Then Return 0D

            Dim unionQuery As String = String.Join(" UNION ALL ", q)
            Dim finalQuery As String = $"SELECT SUM(Amount) AS TotalRevenue FROM ({unionQuery}) AS revenue_sources"

            Using cmd As New MySqlCommand(finalQuery, conn)
                AddPeriodParameters(cmd)
                Dim result = cmd.ExecuteScalar()
                Return If(IsDBNull(result) OrElse result Is Nothing, 0D, Convert.ToDecimal(result))
            End Using

        Catch ex As Exception
            MessageBox.Show("Error calculating filtered revenue: " & ex.Message,
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return 0D
        End Try
    End Function

    ' =======================================================================
    ' GET FILTERED EXPENSES - ALL SOURCES WITH PERIOD FILTER
    ' =======================================================================
    Private Function GetFilteredExpenses() As Decimal
        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then
                Return 0D
            End If

            Dim q As New List(Of String)

            ' INVENTORY_BATCHES TABLE - Expenses
            If TableExists("inventory_batches") Then
                q.Add($"
                    SELECT COALESCE(SUM(TotalCost), 0) AS Amount
                    FROM inventory_batches
                    WHERE BatchStatus = 'Active'
                    {GetYearFilter("PurchaseDate")}
                    {If(currentPeriod = "Daily", " AND MONTH(PurchaseDate) = @Month ", "")}
                ")
            End If

            If q.Count = 0 Then Return 0D

            Dim unionQuery As String = String.Join(" UNION ALL ", q)
            Dim finalQuery As String = $"SELECT SUM(Amount) AS TotalExpenses FROM ({unionQuery}) AS expense_sources"

            Using cmd As New MySqlCommand(finalQuery, conn)
                AddPeriodParameters(cmd)
                Dim result = cmd.ExecuteScalar()
                Return If(IsDBNull(result) OrElse result Is Nothing, 0D, Convert.ToDecimal(result))
            End Using

        Catch ex As Exception
            MessageBox.Show("Error calculating filtered expenses: " & ex.Message,
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return 0D
        End Try
    End Function

    ' Helper to get single year revenue for YoY calculation
    Private Function GetSingleYearRevenue(year As Integer) As Decimal
        Try
            Dim q As New List(Of String)
            q.Add($"SELECT COALESCE(SUM(TotalAmount), 0) FROM orders WHERE OrderStatus = 'Completed' AND YEAR(OrderDate) = {year}")
            If TableExists("payments") Then
                q.Add($"SELECT COALESCE(SUM(AmountPaid), 0) FROM payments WHERE ReservationID IS NOT NULL AND PaymentStatus = 'Completed' AND YEAR(PaymentDate) = {year}")
            End If
            Dim sql As String = $"SELECT SUM(val) FROM ({String.Join(" UNION ALL ", q.Select(Function(s) $"SELECT ({s}) as val"))}) as t"
            Using cmd As New MySqlCommand(sql, conn)
                Dim res = cmd.ExecuteScalar()
                Return If(IsDBNull(res) OrElse res Is Nothing, 0D, Convert.ToDecimal(res))
            End Using
        Catch
            Return 0D
        End Try
    End Function

    ' =======================================================================
    ' GET DATE GROUPING FOR SQL COLUMN
    ' =======================================================================
    Private Function GetDateGroupingForColumn(columnName As String) As String
        Select Case currentPeriod
            Case "Daily"
                Return $"DATE({columnName})"

            Case "Weekly"
                Return $"YEARWEEK({columnName}, 1)"

            Case "Monthly"
                Return $"MONTH({columnName})"

            Case "Yearly"
                Return $"YEAR({columnName})"

            Case Else
                Return $"DATE({columnName})"
        End Select
    End Function

    ' =======================================================================
    ' GET PERIOD WHERE CLAUSE FOR FILTERING
    ' =======================================================================
    Private Function GetPeriodWhereClause(dateColumn As String) As String
        Select Case currentPeriod
            Case "Daily"
                ' Show breakdown of days in the selected month
                If Reports.SelectedMonth > 0 Then
                    Return $"AND YEAR({dateColumn}) = @Year AND MONTH({dateColumn}) = @Month"
                Else
                    Return $"AND YEAR({dateColumn}) = @Year"
                End If

            Case "Weekly", "Monthly"
                ' Show breakdown of weeks/months in the selected year
                Return $"AND YEAR({dateColumn}) = @Year"

            Case "Yearly"
                ' Multi-year range handled by GetYearFilter
                Return ""

            Case Else
                Return $"AND YEAR({dateColumn}) = @Year"
        End Select
    End Function

    ' =======================================================================
    ' FILTER CHANGE EVENT
    ' =======================================================================
    Private Sub RoundCorners(control As Control, radius As Integer)
        ' Helper for controls
    End Sub


    ' =======================================================================
    ' ADD PERIOD PARAMETERS TO COMMAND
    ' =======================================================================
    Private Sub AddPeriodParameters(cmd As MySqlCommand)
        If Not cmd.Parameters.Contains("@Year") Then
            cmd.Parameters.AddWithValue("@Year", currentYear)
        End If
        If Not cmd.Parameters.Contains("@Month") Then
            cmd.Parameters.AddWithValue("@Month", currentMonth)
        End If
    End Sub


    ' =======================================================================
    ' REFRESH DATA
    ' =======================================================================
    Public Sub RefreshData()
        currentPeriod = Reports.SelectedPeriod
        
        ' Use the global selected Year and Month
        currentYear = Reports.SelectedYear
        currentMonth = Reports.SelectedMonth
        
        ' ConfigureDateFilter()
        
        ' Update header
        UpdateHeaderLabel()
        
        ConfigureChart()
        LoadAndDisplaySalesData()
        UpdateSummaryCards()
    End Sub


    ' =======================================================================
    ' CHANGE YEAR
    ' =======================================================================
    Public Sub SetYear(year As Integer)
        currentYear = year
        UpdateHeaderLabel()
        RefreshData()
    End Sub

    ' =======================================================================
    ' FORM CLOSING
    ' =======================================================================
    Private Sub FormSales_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        Catch
        End Try
    End Sub

    Private Sub RoundedPane24_Paint(sender As Object, e As PaintEventArgs) Handles RoundedPane24.Paint

    End Sub
End Class