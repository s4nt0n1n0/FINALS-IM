Imports System.Windows.Forms.DataVisualization.Charting
Imports MySqlConnector
Imports Org.BouncyCastle.Tls

Public Class Dashboard
    Private WithEvents refreshTimer As New Timer()

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = ColorTranslator.FromHtml("#F7F8FA")

        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or
            ControlStyles.AllPaintingInWmPaint Or
            ControlStyles.UserPaint, True)
        Me.UpdateStyles()

        ' Initialize the Filters ComboBox
        InitializeFiltersComboBox()
        LoadInventoryAlerts()
        ' Load all dashboard data
        LoadDashboardData()

        ' Setup auto-refresh timer (refresh every 30 seconds)
        refreshTimer.Interval = 30000 ' 30 seconds
        refreshTimer.Start()
    End Sub
    Private Sub InitializeFiltersComboBox()
        Try
            ' Check if Filters control exists
            If Filters Is Nothing Then
                MessageBox.Show("Filters ComboBox control not found. Please check the form designer.",
                          "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Clear any existing items
            Filters.Items.Clear()

            ' Add filter options
            Filters.Items.Add("Daily")
            Filters.Items.Add("Weekly")
            Filters.Items.Add("Monthly")
            Filters.Items.Add("Yearly")

            ' Set default to "Daily"
            Filters.SelectedIndex = 0 ' This selects "Daily"

            ' Configure ComboBox appearance
            Filters.DropDownStyle = ComboBoxStyle.DropDownList
            Filters.FlatStyle = FlatStyle.Flat

        Catch ex As Exception
            MessageBox.Show("Error initializing filters: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub refreshTimer_Tick(sender As Object, e As EventArgs) Handles refreshTimer.Tick
        LoadDashboardData()

        LoadTotalOrders()
        LoadInventoryAlerts()
        LoadOrdersOverviewChart()
    End Sub

    Private Sub LoadDashboardData()
        Try
            ' Load metrics
            LoadTotalRevenue()
            LoadTotalOrders()
            LoadActiveReservations()
            LoadAverageOrder()

            ' Load charts and lists
            LoadSalesByChannel()
            LoadTopProductsChart()

            LoadOrdersOverviewChart()

            ConfigureChartTopProducts()
            ConfigureChart2OrderTrends()

        Catch ex As Exception
            MessageBox.Show("Error loading dashboard: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ============================================
    ' FILTERS COMBO BOX EVENT HANDLER
    ' ============================================
    Private Sub Filters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Filters.SelectedIndexChanged
        Try
            ' Reload all dashboard data based on selected filter
            LoadDashboardData()
        Catch ex As Exception
            MessageBox.Show("Error applying filter: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ============================================
    ' GET DATE FILTER SQL CONDITION
    ' ============================================
    Private Function GetDateFilterCondition(dateColumn As String) As String
        ' Get selected filter, default to "Daily" if nothing is selected
        Dim selectedFilter As String = "Daily" ' Set default first

        ' Safely check if Filters exists and has a selected item
        If Filters IsNot Nothing AndAlso Filters.SelectedItem IsNot Nothing Then
            selectedFilter = Filters.SelectedItem.ToString()
        End If

        Select Case selectedFilter
            Case "Daily"
                ' Today only
                Return $"DATE({dateColumn}) = CURDATE()"

            Case "Weekly"
                ' Current Week (ISO)
                Return $"YEARWEEK({dateColumn}, 1) = YEARWEEK(CURDATE(), 1)"

            Case "Monthly"
                ' Current calendar month
                Return $"YEAR({dateColumn}) = YEAR(CURDATE()) AND MONTH({dateColumn}) = MONTH(CURDATE())"

            Case "Yearly"
                ' Current year
                Return $"YEAR({dateColumn}) = YEAR(CURDATE())"

            Case Else
                ' Default to today
                Return $"DATE({dateColumn}) = CURDATE()"
        End Select
    End Function
    Private Sub Filters_DrawItem(sender As Object, e As DrawItemEventArgs) Handles Filters.DrawItem
        If e.Index < 0 Then Return
        Dim cmb As ComboBox = DirectCast(sender, ComboBox)
        e.DrawBackground()
        e.Graphics.DrawString(cmb.Items(e.Index).ToString(), cmb.Font, Brushes.Black, e.Bounds)
        e.DrawFocusRectangle()
    End Sub


    ' ============================================
    ' CLICKABLE CONTAINER HELPER
    ' ============================================
    Private Sub SetupClickableControl(ctrl As Control, handler As EventHandler)
        If ctrl Is Nothing Then Return

        ' Fix hit-testing for the main container: Transparent panels often pass clicks to the parent.
        ' Setting a non-transparent color ensures it catches mouse events.
        If ctrl.BackColor = Color.Transparent Then
            ctrl.BackColor = Me.BackColor
        End If

        ' Recursively attach handlers and set cursors
        AttachHandlersRecursive(ctrl, handler)
    End Sub

    Private Sub AttachHandlersRecursive(ctrl As Control, handler As EventHandler)
        If ctrl Is Nothing Then Return

        ctrl.Cursor = Cursors.Hand
        RemoveHandler ctrl.Click, handler
        AddHandler ctrl.Click, handler

        For Each child As Control In ctrl.Controls
            AttachHandlersRecursive(child, handler)
        Next
    End Sub

    ' ============================================
    ' TOP METRICS
    ' ============================================

    Private Sub LoadTotalRevenue()
        Try
            openConn()
            Dim dateFilter As String = GetDateFilterCondition("OrderDate")
            Dim dateFilterPayment As String = GetDateFilterCondition("PaymentDate")

            ' Calculate from both Orders and Payments (targeting reservations)
            ' Include 'Served' for orders as it represents realized sales in Dine-in
            cmd = New MySqlCommand($"
                SELECT COALESCE(
                    (SELECT SUM(TotalAmount) FROM orders WHERE OrderStatus IN ('Completed', 'Served') AND {dateFilter}),
                    0
                ) + COALESCE(
                    (SELECT SUM(AmountPaid) FROM payments WHERE ReservationID IS NOT NULL AND PaymentStatus IN ('Paid', 'Completed') AND {dateFilterPayment}),
                    0
                ) as TotalRevenue", conn)

            Dim revenue As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())
            lblTotalRevenue.Text = "₱" & revenue.ToString("N2")
            closeConn()

            ' Apply recursive click handling to the entire metric card
            SetupClickableControl(RoundedPane215, AddressOf Revenue_Click)
        Catch ex As Exception
            lblTotalRevenue.Text = "₱0.00"
            closeConn()
        End Try
    End Sub

    Private Sub Revenue_Click(sender As Object, e As EventArgs)
        Try
            Dim adminDashboard = TryCast(Me.ParentForm, AdminDashboard)
            If adminDashboard Is Nothing Then
                adminDashboard = Application.OpenForms.OfType(Of AdminDashboard)().FirstOrDefault()
            End If

            If adminDashboard IsNot Nothing Then
                adminDashboard.btnReports.PerformClick()

                ' Wait a bit for Reports to load
                Application.DoEvents()

                ' Find Reports form instance and switch to Sales
                If Reports.Instance IsNot Nothing Then
                    Reports.Instance.LoadSalesReport()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error navigating to Revenue report: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadTotalOrders()
        Try
            openConn()
            Dim dateFilterOrder As String = GetDateFilterCondition("OrderDate")
            Dim dateFilterPayment As String = GetDateFilterCondition("PaymentDate")

            ' Count both POS/Website orders and successfully paid/completed reservations
            cmd = New MySqlCommand($"
                SELECT (
                    SELECT COUNT(*) FROM orders 
                    WHERE OrderStatus IN ('Completed', 'Served') 
                    AND {dateFilterOrder}
                ) + (
                    SELECT COUNT(DISTINCT ReservationID) FROM payments 
                    WHERE ReservationID IS NOT NULL 
                    AND PaymentStatus IN ('Paid', 'Completed') 
                    AND {dateFilterPayment}
                ) as TotalOrders", conn)

            Dim totalOrders As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            lblTotalOrder.Text = totalOrders.ToString("#,##0")
            closeConn()

            ' Apply recursive click handling to the entire metric card
            SetupClickableControl(RoundedPane21, AddressOf Orders_Click)
        Catch ex As Exception
            lblTotalOrder.Text = "0"
            closeConn()
        End Try
    End Sub

    Private Sub Orders_Click(sender As Object, e As EventArgs)
        Try
            Dim adminDashboard = TryCast(Me.ParentForm, AdminDashboard)
            If adminDashboard Is Nothing Then
                adminDashboard = Application.OpenForms.OfType(Of AdminDashboard)().FirstOrDefault()
            End If

            If adminDashboard IsNot Nothing Then
                adminDashboard.btnOrders.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Error navigating to Orders screen: " & ex.Message)
        End Try
    End Sub
    Private Sub LoadActiveReservations()
        Try
            openConn()
            Dim dateFilter As String = GetDateFilterCondition("EventDate")

            ' Count reservations that are Pending or Confirmed within the filter period
            cmd = New MySqlCommand($"
                SELECT COUNT(*) FROM reservations 
                WHERE ReservationStatus IN ('Pending', 'Confirmed')
                AND EventDate >= CURDATE()
                AND {dateFilter}", conn)

            Dim activeReservations As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            lblActiveReservations.Text = activeReservations.ToString()
            closeConn()

            ' Apply recursive click handling to the entire metric card
            SetupClickableControl(RoundedPane24, AddressOf Reservations_Click)
        Catch ex As Exception
            lblActiveReservations.Text = "0"
            closeConn()
        End Try
    End Sub

    Private Sub Reservations_Click(sender As Object, e As EventArgs)
        Try
            Dim adminDashboard = TryCast(Me.ParentForm, AdminDashboard)
            If adminDashboard Is Nothing Then
                adminDashboard = Application.OpenForms.OfType(Of AdminDashboard)().FirstOrDefault()
            End If

            If adminDashboard IsNot Nothing Then
                adminDashboard.btnReservations.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Error navigating to Reservations screen: " & ex.Message)
        End Try
    End Sub



    Private Sub LoadTopProductsChart()
        Try
            ' Configure chart first
            ConfigureTopProductsChart()

            ' Fetch and display data
            Dim performanceData = FetchTopProductsData()
            UpdateTopProductsChart(performanceData)
        Catch ex As Exception
            MessageBox.Show($"Unable to load top products chart.{Environment.NewLine}{ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ============================================
    ' FETCH TOP PRODUCTS DATA WITH FILTER
    ' ============================================
    Private Function FetchTopProductsData() As DataTable
        ' Get the appropriate date filters for both orders and reservations
        Dim orderDateFilter As String = GetDateFilterCondition("o.OrderDate")
        Dim reservationDateFilter As String = GetDateFilterCondition("r.ReservationDate")

        Dim query As String = $"
SELECT ProductName,
       SUM(Quantity) AS TotalQuantity,
       SUM(TotalPrice) AS Revenue
FROM (
       -- Reservation items with Confirmed or Served status
       SELECT ri.ProductName,
              ri.Quantity,
              ri.TotalPrice,
              r.ReservationDate AS OrderDate
       FROM reservation_items ri
       INNER JOIN reservations r ON ri.ReservationID = r.ReservationID
       WHERE r.ReservationStatus IN ('Confirmed', 'Served')
       AND {reservationDateFilter}
       
       UNION ALL
       
       -- Order items with Served or Completed status
       SELECT oi.ProductName,
              oi.Quantity,
              (oi.Quantity * oi.UnitPrice) AS TotalPrice,
              o.OrderDate
       FROM order_items oi
       INNER JOIN orders o ON oi.OrderID = o.OrderID
       WHERE o.OrderStatus IN ('Served', 'Completed')
       AND {orderDateFilter}
     ) AS combined
GROUP BY ProductName
ORDER BY TotalQuantity DESC
LIMIT 8;"

        Dim dt As New DataTable()

        Using connection As New MySqlConnection(strConnection)
            connection.Open()
            Using command As New MySqlCommand(query, connection)
                Using reader = command.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        End Using

        Return dt
    End Function
    ' ============================================
    ' UPDATE TOP PRODUCTS CHART
    ' ============================================
    Private Sub ConfigureTopProductsChart()
        ' Configure the FlowLayoutPanel
        ChartTopProducts.AutoScroll = True
        ChartTopProducts.FlowDirection = FlowDirection.TopDown
        ChartTopProducts.WrapContents = False
        ChartTopProducts.BackColor = Color.White

        ' Apply recursive click handling to the entire container
        SetupClickableControl(RoundedPane223, AddressOf TopProducts_Click)
    End Sub

    ' ============================================
    ' UPDATE TOP PRODUCTS CHART
    Private Sub UpdateTopProductsChart(data As DataTable)
        ' Clear the FlowLayoutPanel
        ChartTopProducts.Controls.Clear()

        If data.Rows.Count = 0 Then
            ' Show "No data" message
            Dim noDataLabel As New Label()
            noDataLabel.Text = "No Product Data Available"
            noDataLabel.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            noDataLabel.ForeColor = Color.Gray
            noDataLabel.Margin = New Padding(20, 20, 20, 20)
            noDataLabel.AutoSize = True
            noDataLabel.Cursor = Cursors.Hand ' Make it look clickable
            AddHandler noDataLabel.Click, AddressOf TopProducts_Click ' Add click handler
            ChartTopProducts.Controls.Add(noDataLabel)
            Return
        End If

        ' Display top products in list format
        Dim rankColors As Color() = {
            Color.FromArgb(255, 152, 0),  ' Orange for #1
            Color.FromArgb(158, 158, 158), ' Gray for #2
            Color.FromArgb(205, 127, 50),  ' Bronze for #3
            Color.FromArgb(100, 100, 100), ' Dark gray for others
            Color.FromArgb(100, 100, 100),
            Color.FromArgb(100, 100, 100),
            Color.FromArgb(100, 100, 100),
            Color.FromArgb(100, 100, 100)
        }

        For i As Integer = 0 To Math.Min(data.Rows.Count - 1, 7) ' Top 8 products
            Dim row = data.Rows(i)
            Dim productName As String = row("ProductName").ToString()
            Dim quantity As Integer = If(IsDBNull(row("TotalQuantity")), 0, Convert.ToInt32(row("TotalQuantity")))
            Dim revenue As Decimal = If(IsDBNull(row("Revenue")), 0D, Convert.ToDecimal(row("Revenue")))

            ' Create product item panel
            Dim itemPanel As New Panel()
            itemPanel.Size = New Size(ChartTopProducts.Width - 23, 60)
            itemPanel.BackColor = Color.FromArgb(255, 250, 240)
            itemPanel.Margin = New Padding(10, 5, 10, 5)

            ' Rank badge (circular)
            Dim lblRank As New Label()
            lblRank.Text = (i + 1).ToString()
            lblRank.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblRank.ForeColor = Color.White
            lblRank.BackColor = rankColors(i)
            lblRank.Size = New Size(35, 35)
            lblRank.Location = New Point(10, 12)
            lblRank.TextAlign = ContentAlignment.MiddleCenter
            ' Make it circular
            Dim rankPath As New System.Drawing.Drawing2D.GraphicsPath()
            rankPath.AddEllipse(0, 0, lblRank.Width, lblRank.Height)
            lblRank.Region = New Region(rankPath)
            itemPanel.Controls.Add(lblRank)

            ' Product name
            Dim lblProduct As New Label()
            lblProduct.Text = productName
            lblProduct.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            lblProduct.ForeColor = Color.FromArgb(45, 45, 45)
            lblProduct.Location = New Point(55, 8)
            lblProduct.AutoSize = True
            itemPanel.Controls.Add(lblProduct)

            ' Quantity sold
            Dim lblQuantity As New Label()
            lblQuantity.Text = quantity.ToString() & " sold"
            lblQuantity.Font = New Font("Segoe UI", 9)
            lblQuantity.ForeColor = Color.Gray
            lblQuantity.Location = New Point(55, 30)
            lblQuantity.AutoSize = True
            itemPanel.Controls.Add(lblQuantity)

            ' Revenue (right aligned)
            Dim lblRevenue As New Label()
            lblRevenue.Text = "₱" & revenue.ToString("N0")
            lblRevenue.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblRevenue.ForeColor = Color.FromArgb(34, 197, 94)
            lblRevenue.Location = New Point(itemPanel.Width - 120, 18)
            lblRevenue.Size = New Size(110, 25)
            lblRevenue.TextAlign = ContentAlignment.MiddleRight
            itemPanel.Controls.Add(lblRevenue)

            ' Apply recursive click handling to the item panel and its labels/badges
            SetupClickableControl(itemPanel, AddressOf TopProducts_Click)

            ' Add separator line (except for last item)

            ' Add separator line (except for last item)
            If i < Math.Min(data.Rows.Count - 1, 7) Then
                Dim separator As New Panel()
                separator.Height = 1
                separator.Width = itemPanel.Width - 20
                separator.BackColor = Color.FromArgb(230, 230, 230)
                separator.Location = New Point(10, 59)
                itemPanel.Controls.Add(separator)
            End If

            ChartTopProducts.Controls.Add(itemPanel)
        Next
    End Sub

    Private Sub TopProducts_Click(sender As Object, e As EventArgs)
        Try
            Dim adminDashboard = TryCast(Me.ParentForm, AdminDashboard)
            If adminDashboard Is Nothing Then
                adminDashboard = Application.OpenForms.OfType(Of AdminDashboard)().FirstOrDefault()
            End If

            If adminDashboard IsNot Nothing Then
                adminDashboard.btnReports.PerformClick()
                Application.DoEvents()

                If Reports.Instance IsNot Nothing Then
                    Reports.Instance.LoadProductPerformanceReport()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error navigating to Product Performance report: " & ex.Message)
        End Try
    End Sub

    ' ============================================
    ' GET OR CREATE PRODUCTS PANEL
    ' ============================================
    Private Function GetOrCreateProductsPanel() As Panel
        ' Check if products panel already exists
        Dim existingPanel As Panel = TryCast(Me.Controls.Find("pnlTopProducts", True).FirstOrDefault(), Panel)

        If existingPanel IsNot Nothing Then
            Return existingPanel
        End If

        ' Create new panel with same location and size as ChartTopProducts
        Dim newPanel As New Panel()
        newPanel.Name = "pnlTopProducts"
        newPanel.Location = ChartTopProducts.Location
        newPanel.Size = ChartTopProducts.Size
        newPanel.BackColor = Color.White

        ' Add to the same parent as ChartTopProducts
        ChartTopProducts.Parent.Controls.Add(newPanel)
        newPanel.BringToFront()

        Return newPanel
    End Function
    Private Sub LoadAverageOrder()

        openConn()
            Dim dateFilterOrder As String = GetDateFilterCondition("OrderDate")
            Dim dateFilterPayment As String = GetDateFilterCondition("PaymentDate")

            ' Calculate Average Order Value as (Total Revenue) / (Total Orders)
            ' This ensures it remains perfectly consistent with the Revenue and Order labels
            cmd = New MySqlCommand($"
                SELECT 
                    CASE WHEN counts.TotalCount = 0 THEN 0 
                    ELSE revs.TotalRev / counts.TotalCount END as AOV
                FROM (
                    SELECT 
                        (SELECT COALESCE(SUM(TotalAmount), 0) FROM orders WHERE OrderStatus IN ('Completed', 'Served') AND {dateFilterOrder}) + 
                        (SELECT COALESCE(SUM(AmountPaid), 0) FROM payments WHERE ReservationID IS NOT NULL AND PaymentStatus IN ('Paid', 'Completed') AND {dateFilterPayment}) as TotalRev
                ) as revs,
                (
                    SELECT 
                        (SELECT COUNT(*) FROM orders WHERE OrderStatus IN ('Completed', 'Served') AND {dateFilterOrder}) + 
                        (SELECT COUNT(DISTINCT ReservationID) FROM payments WHERE ReservationID IS NOT NULL AND PaymentStatus IN ('Paid', 'Completed') AND {dateFilterPayment}) as TotalCount
                ) as counts", conn)

            Dim avgOrder As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())
            closeConn()


    End Sub

    Private Sub AverageOrder_Click(sender As Object, e As EventArgs)
        Try
            Dim adminDashboard = TryCast(Me.ParentForm, AdminDashboard)
            If adminDashboard Is Nothing Then
                adminDashboard = Application.OpenForms.OfType(Of AdminDashboard)().FirstOrDefault()
            End If

            If adminDashboard IsNot Nothing Then
                adminDashboard.btnReports.PerformClick()
                Application.DoEvents()

                If Reports.Instance IsNot Nothing Then
                    Reports.Instance.LoadOrderTrends()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error navigating to Average Order report: " & ex.Message)
        End Try
    End Sub


    ' ============================================
    ' FIXED: SALES BY CHANNEL - Now captures actual prices from orders
    ' ============================================

    Private Sub LoadSalesByChannel()
        Try
            openConn()

            ' Get date filters for orders and payments
            Dim orderDateFilter As String = GetDateFilterCondition("o.OrderDate")
            Dim paymentDateFilter As String = GetDateFilterCondition("rp.PaymentDate")


            ' Get sales from completed orders grouped by type with filter
            cmd = New MySqlCommand($"
        SELECT 
            o.OrderType,
            COALESCE(SUM(o.TotalAmount), 0) as TotalSales
        FROM orders o
        WHERE o.OrderStatus IN ('Completed', 'Served')
        AND {orderDateFilter}
        GROUP BY o.OrderType", conn)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim dineInSales As Decimal = 0
            Dim takeoutSales As Decimal = 0
            Dim onlineSales As Decimal = 0

            While reader.Read()
                Dim orderType As String = reader("OrderType").ToString()
                Dim sales As Decimal = Convert.ToDecimal(reader("TotalSales"))

                Select Case orderType
                    Case "Dine-in"
                        dineInSales = sales
                    Case "Takeout"
                        takeoutSales = sales
                    Case "Online"
                        onlineSales = sales
                End Select
            End While
            reader.Close()

            ' Get Catering/Reservation revenue from completed payments with filter
            cmd = New MySqlCommand($"
        SELECT COALESCE(SUM(rp.AmountPaid), 0) as CateringRevenue
        FROM payments rp
        WHERE rp.ReservationID IS NOT NULL 
        AND rp.PaymentStatus IN ('Completed', 'Paid')
        AND {paymentDateFilter}", conn)

            Dim cateringRevenue As Decimal = Convert.ToDecimal(cmd.ExecuteScalar())

            closeConn()

            ' Calculate total and percentages
            Dim totalSales As Decimal = dineInSales + takeoutSales + onlineSales + cateringRevenue

            ' Clear chart and annotations first - ADD THIS BEFORE THE IF STATEMENT
            Chart2.Series(0).Points.Clear()
            Chart2.Annotations.Clear()

            ' Apply recursive click handling to the entire card
            SetupClickableControl(RoundedPane28, AddressOf Revenue_Click)

            ' Chart control specifically needs its own handler as it can sometimes consume events
            SetupClickableControl(Chart2, AddressOf Revenue_Click)

            If totalSales > 0 Then
                Dim dineInPercent As Decimal = (dineInSales / totalSales) * 100
                Dim takeoutPercent As Decimal = ((takeoutSales + onlineSales) / totalSales) * 100
                Dim cateringPercent As Decimal = (cateringRevenue / totalSales) * 100

                ' Add data points
                Dim point1 As New DataVisualization.Charting.DataPoint()
                point1.SetValueXY("Dine-in", dineInSales)
                point1.Color = Color.FromArgb(165, 149, 233)
                point1.BorderColor = Color.White
                point1.BorderWidth = 2
                point1.Label = Math.Round(dineInPercent, 0).ToString() & "%"
                point1.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                point1.LabelForeColor = Color.White
                Chart2.Series(0).Points.Add(point1)

                Dim point2 As New DataVisualization.Charting.DataPoint()
                point2.SetValueXY("Takeout", takeoutSales + onlineSales)
                point2.Color = Color.FromArgb(144, 213, 169)
                point2.BorderColor = Color.White
                point2.BorderWidth = 2
                point2.Label = Math.Round(takeoutPercent, 0).ToString() & "%"
                point2.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                point2.LabelForeColor = Color.White
                Chart2.Series(0).Points.Add(point2)

                Dim point3 As New DataVisualization.Charting.DataPoint()
                point3.SetValueXY("Catering", cateringRevenue)
                point3.Color = Color.FromArgb(251, 203, 119)
                point3.BorderColor = Color.White
                point3.BorderWidth = 2
                point3.Label = Math.Round(cateringPercent, 0).ToString() & "%"
                point3.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                point3.LabelForeColor = Color.White
                Chart2.Series(0).Points.Add(point3)

                ' Update legend labels
                lblPercentDineIn.Text = Math.Round(dineInPercent, 0).ToString() & "%"
                lblValueDinein.Text = "₱" & dineInSales.ToString("N2")

                ' Combine takeout and online
                lblPercentTakeout.Text = Math.Round(takeoutPercent, 0).ToString() & "%"
                lblValueTakeout.Text = "₱" & (takeoutSales + onlineSales).ToString("N2")

                lblPercentCatering.Text = Math.Round(cateringPercent, 0).ToString() & "%"
                lblValueCatering.Text = "₱" & cateringRevenue.ToString("N2")
            Else
                ' No data yet - show zeros and add "No data" label
                Dim noDataAnnotation As New TextAnnotation()
                noDataAnnotation.Text = "No Sales Data Available"
                noDataAnnotation.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                noDataAnnotation.ForeColor = Color.Gray
                noDataAnnotation.X = 50
                noDataAnnotation.Y = 50
                noDataAnnotation.Alignment = ContentAlignment.MiddleCenter
                Chart2.Annotations.Add(noDataAnnotation)

                lblPercentDineIn.Text = "0%"
                lblValueDinein.Text = "₱0.00"

                lblPercentTakeout.Text = "0%"
                lblValueTakeout.Text = "₱0.00"

                lblPercentCatering.Text = "0%"
                lblValueCatering.Text = "₱0.00"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading sales by channel: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            closeConn()
        End Try
    End Sub

    Private Sub LoadInventoryAlerts()
        Try
            If Me.Controls.Contains(InventoryAlerts) Then
                InventoryAlerts.BringToFront()
                InventoryAlerts.Visible = True
                InventoryAlerts.AutoScroll = True ' Allow scrolling if items exceed panel height
            End If
            openConn()

            ' Query to get critical and low stock items
            Dim sql As String = "
            SELECT 
                i.IngredientName AS 'Item Name',
                COALESCE(SUM(ib.StockQuantity), 0) AS 'Stock',
                i.UnitType AS 'Unit',
                i.MinStockLevel,
                CASE 
                    WHEN COALESCE(SUM(ib.StockQuantity), 0) = 0 THEN 'Critical'
                    WHEN COALESCE(SUM(ib.StockQuantity), 0) < i.MinStockLevel THEN 'Low'
                    ELSE 'OK'
                END AS 'Alert Level'
            FROM ingredients i
            LEFT JOIN inventory_batches ib ON i.IngredientID = ib.IngredientID 
                AND ib.BatchStatus = 'Active'
            WHERE i.IsActive = 1
            GROUP BY i.IngredientID, i.IngredientName, i.UnitType, i.MinStockLevel
            HAVING COALESCE(SUM(ib.StockQuantity), 0) < i.MinStockLevel
            ORDER BY 
                CASE 
                    WHEN COALESCE(SUM(ib.StockQuantity), 0) = 0 THEN 1
                    ELSE 2
                END,
                COALESCE(SUM(ib.StockQuantity), 0) ASC
            LIMIT 5
        "

            Dim cmd As New MySqlCommand(sql, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Clear existing alert items


            ' Warning icon label

            ' View All button
            Dim label11 As New Button()
            label11.Text = "View All"
            label11.FlatStyle = FlatStyle.Flat
            label11.FlatAppearance.BorderSize = 0
            label11.BackColor = Color.Transparent
            label11.Location = New Point(390, 17)
            label11.ForeColor = Color.FromArgb(111, 66, 193)
            label11.Font = New Font("Segoe UI", 8, FontStyle.Regular)
            label11.Size = New Size(60, 25)
            label11.Cursor = Cursors.Hand
            label11.TextAlign = ContentAlignment.MiddleRight



            ' Add click event handler
            AddHandler label11.Click, AddressOf ViewAllInventory_Click

            ' Add hover effects
            AddHandler label11.MouseEnter, Sub(s, ev)
                                               label11.ForeColor = Color.FromArgb(90, 50, 160)
                                               label11.Font = New Font("Segoe UI", 8, FontStyle.Underline)
                                           End Sub
            AddHandler label11.MouseLeave, Sub(s, ev)
                                               label11.ForeColor = Color.FromArgb(111, 66, 193)
                                               label11.Font = New Font("Segoe UI", 8, FontStyle.Regular)
                                           End Sub
            Panel.Controls.Add(label11)
            InventoryAlerts.AutoScroll = True
            InventoryAlerts.WrapContents = False
            InventoryAlerts.FlowDirection = FlowDirection.TopDown

            Dim yPosition As Integer = 40
            Dim alertCount As Integer = 0

            While reader.Read() AndAlso alertCount < 5
                Dim itemName As String = reader("Item Name").ToString()
                Dim stock As Decimal = Convert.ToDecimal(reader("Stock"))
                Dim unit As String = reader("Unit").ToString()
                Dim alertLevel As String = reader("Alert Level").ToString()

                ' Create alert item panel
                Dim alertItem As New Panel()
                alertItem.Size = New Size(InventoryAlerts.Width - 6, 50)
                alertItem.Location = New Point(3, yPosition)
                alertItem.BackColor = Color.White
                alertItem.BorderStyle = BorderStyle.None

                ' Item name label
                Dim lblItem As New Label()
                lblItem.Text = itemName
                lblItem.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                lblItem.ForeColor = Color.FromArgb(45, 45, 45)
                lblItem.Location = New Point(10, 8)
                lblItem.AutoSize = True
                alertItem.Controls.Add(lblItem)

                ' Stock quantity label
                Dim lblStock As New Label()
                lblStock.Text = stock.ToString("N2") & " " & unit & " left"
                lblStock.Font = New Font("Segoe UI", 8)
                lblStock.ForeColor = Color.Gray
                lblStock.Location = New Point(10, 28)
                lblStock.AutoSize = True
                alertItem.Controls.Add(lblStock)

                ' Alert badge
                Dim lblAlert As New Label()
                lblAlert.Text = alertLevel
                lblAlert.Font = New Font("Segoe UI", 8, FontStyle.Bold)
                lblAlert.AutoSize = False
                lblAlert.Size = New Size(60, 22)
                lblAlert.TextAlign = ContentAlignment.MiddleCenter
                lblAlert.Location = New Point(alertItem.Width - 75, 14)

                ' Color code based on alert level
                Select Case alertLevel
                    Case "Critical"
                        lblAlert.BackColor = Color.FromArgb(220, 53, 69)
                        lblAlert.ForeColor = Color.White
                    Case "Low"
                        lblAlert.BackColor = Color.FromArgb(255, 193, 7)
                        lblAlert.ForeColor = Color.Black
                End Select

                alertItem.Controls.Add(lblAlert)

                ' Add separator line
                If alertCount < 4 Then
                    Dim separator As New Panel()
                    separator.Height = 1
                    separator.Width = alertItem.Width - 6
                    separator.BackColor = Color.FromArgb(230, 230, 230)
                    separator.Location = New Point(3, 49)
                    alertItem.Controls.Add(separator)
                End If

                InventoryAlerts.Controls.Add(alertItem)

                yPosition += 55
                alertCount += 1
            End While

            reader.Close()

            ' Show message if no alerts
            If alertCount = 0 Then
                Dim noAlerts As New Label()
                noAlerts.Text = "✓ All inventory levels are healthy"
                noAlerts.Font = New Font("Segoe UI", 9, FontStyle.Italic)
                noAlerts.ForeColor = Color.FromArgb(40, 167, 69)
                noAlerts.Location = New Point(20, 60)
                noAlerts.AutoSize = True
                InventoryAlerts.Controls.Add(noAlerts)
            End If

            closeConn()

        Catch ex As Exception
            MessageBox.Show("Error loading inventory alerts: " & ex.Message,
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            closeConn()
        End Try
    End Sub

    ' Add a click event to navigate to full inventory
    Private Sub ViewAllInventory_Click(sender As Object, e As EventArgs)
        Try
            Dim adminDashboard As AdminDashboard = TryCast(Me.ParentForm, AdminDashboard)
            If adminDashboard IsNot Nothing Then
                adminDashboard.Inventory.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Error navigating to inventory: " & ex.Message,
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadOrdersOverviewChart()
        Try
            ' Configure chart appearance
            ConfigureOrdersOverviewChart()

            ' Load and display data
            LoadOrdersOverviewData()

        Catch ex As Exception
            MessageBox.Show("Error loading orders overview chart: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ConfigureOrdersOverviewChart()
        ' Clear existing configuration
        OrdersOverviewChart.Series.Clear()
        OrdersOverviewChart.Titles.Clear()
        OrdersOverviewChart.Legends.Clear()

        ' Ensure chart area exists
        If OrdersOverviewChart.ChartAreas.Count = 0 Then
            OrdersOverviewChart.ChartAreas.Add(New ChartArea("MainArea"))
        End If

        Dim chartArea = OrdersOverviewChart.ChartAreas(0)
        With chartArea
            ' X-Axis configuration
            .AxisX.MajorGrid.Enabled = False
            .AxisX.LabelStyle.Font = New Font("Segoe UI", 9.0F)
            .AxisX.LineColor = Color.LightGray
            .AxisX.Interval = 1

            ' Y-Axis configuration with default values
            .AxisY.MajorGrid.LineColor = Color.LightGray
            .AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot
            .AxisY.LabelStyle.Font = New Font("Segoe UI", 9.0F)
            .AxisY.LabelStyle.Format = "#,##0"
            .AxisY.Minimum = 0
            .AxisY.Maximum = 50  ' Set default maximum
            .AxisY.Interval = 10  ' Set default interval

            .BackColor = Color.White
        End With

        ' Create Completed Orders Series (Black bars)
        Dim completedSeries = OrdersOverviewChart.Series.Add("Completed")
        With completedSeries
            .ChartType = SeriesChartType.Column
            .Color = Color.FromArgb(34, 197, 94) ' Black
            .BorderWidth = 0
            .IsValueShownAsLabel = False
        End With

        ' Create Cancelled Orders Series (Gray bars)
        Dim cancelledSeries = OrdersOverviewChart.Series.Add("Cancelled")
        With cancelledSeries
            .ChartType = SeriesChartType.Column
            .Color = Color.FromArgb(239, 68, 68) ' Light gray
            .BorderWidth = 0
            .IsValueShownAsLabel = False
        End With

        ' Add title
        OrdersOverviewChart.Titles.Add(New Title With {
        .Text = "Orders Overview",
        .Alignment = ContentAlignment.TopLeft,
        .Font = New Font("Segoe UI Semibold", 11.0F, FontStyle.Bold),
        .ForeColor = Color.FromArgb(51, 51, 51)
    })

        ' Add subtitle
        OrdersOverviewChart.Titles.Add(New Title With {
        .Text = "Weekly order statistics",
        .Alignment = ContentAlignment.TopLeft,
        .Font = New Font("Segoe UI", 8.5F, FontStyle.Regular),
        .ForeColor = Color.Gray,
        .Docking = Docking.Top
    })

        ' Add Legend
        Dim legend As New Legend("StatusLegend")
        With legend
            .Docking = Docking.Bottom
            .Alignment = StringAlignment.Center
            .Font = New Font("Segoe UI", 8.5F)
            .LegendStyle = LegendStyle.Row
        End With
        OrdersOverviewChart.Legends.Add(legend)
    End Sub
    Private Sub LoadOrdersOverviewData()
        Try
            openConn()

            Dim selectedFilter As String = If(Filters.SelectedItem?.ToString(), "Yearly")
            Dim labels As New List(Of String)
            Dim completedData As New Dictionary(Of String, Integer)
            Dim cancelledData As New Dictionary(Of String, Integer)
            Dim sql As String = ""

            Select Case selectedFilter
                Case "Daily"
                    ' Show hourly data for today
                    labels.AddRange({"12AM", "3AM", "6AM", "9AM", "12PM", "3PM", "6PM", "9PM"})

                    sql = "
                SELECT 
                    HOUR(OrderTime) AS Period,
                    OrderStatus,
                    COUNT(*) AS OrderCount
                FROM orders
                WHERE DATE(OrderDate) = CURDATE()
                AND OrderStatus IN ('Completed', 'Cancelled', 'Served')
                GROUP BY HOUR(OrderTime), OrderStatus"

                    ' Initialize data
                    For Each label In labels
                        completedData(label) = 0
                        cancelledData(label) = 0
                    Next

                    Using cmd As New MySqlCommand(sql, conn)
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim hour As Integer = Convert.ToInt32(reader("Period"))
                                Dim status As String = reader("OrderStatus").ToString()
                                Dim count As Integer = Convert.ToInt32(reader("OrderCount"))

                                ' Map hour to label (group into 3-hour blocks)
                                Dim labelIndex As Integer = hour \ 3
                                If labelIndex >= 0 AndAlso labelIndex < labels.Count Then
                                    If status = "Completed" OrElse status = "Served" Then
                                        completedData(labels(labelIndex)) += count
                                    ElseIf status = "Cancelled" Then
                                        cancelledData(labels(labelIndex)) += count
                                    End If
                                End If
                            End While
                        End Using
                    End Using

                Case "Weekly"
                    ' Show daily data for last 7 days
                    For i As Integer = 6 To 0 Step -1
                        labels.Add(Format(Date.Today.AddDays(-i), "ddd"))
                    Next

                    sql = "
                SELECT 
                    DATE(OrderDate) AS Period,
                    OrderStatus,
                    COUNT(*) AS OrderCount
                FROM orders
                WHERE OrderDate >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)
                AND OrderStatus IN ('Completed', 'Cancelled', 'Served')
                GROUP BY DATE(OrderDate), OrderStatus
                ORDER BY DATE(OrderDate)"

                    ' Initialize data
                    For Each label In labels
                        completedData(label) = 0
                        cancelledData(label) = 0
                    Next

                    Using cmd As New MySqlCommand(sql, conn)
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim orderDate As Date = Convert.ToDateTime(reader("Period"))
                                Dim status As String = reader("OrderStatus").ToString()
                                Dim count As Integer = Convert.ToInt32(reader("OrderCount"))
                                Dim dayLabel As String = Format(orderDate, "ddd")

                                If labels.Contains(dayLabel) Then
                                    If status = "Completed" OrElse status = "Served" Then
                                        completedData(dayLabel) += count
                                    ElseIf status = "Cancelled" Then
                                        cancelledData(dayLabel) += count
                                    End If
                                End If
                            End While
                        End Using
                    End Using

                Case "Monthly"
                    ' Show weekly data for last 30 days (4 weeks)
                    labels.AddRange({"Week 1", "Week 2", "Week 3", "Week 4"})

                    sql = "
                SELECT 
                    FLOOR(DATEDIFF(CURDATE(), OrderDate) / 7) AS WeekNum,
                    OrderStatus,
                    COUNT(*) AS OrderCount
                FROM orders
                WHERE OrderDate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                AND OrderStatus IN ('Completed', 'Cancelled', 'Served')
                GROUP BY FLOOR(DATEDIFF(CURDATE(), OrderDate) / 7), OrderStatus"

                    ' Initialize data
                    For Each label In labels
                        completedData(label) = 0
                        cancelledData(label) = 0
                    Next

                    Using cmd As New MySqlCommand(sql, conn)
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim weekNum As Integer = Convert.ToInt32(reader("WeekNum"))
                                Dim status As String = reader("OrderStatus").ToString()
                                Dim count As Integer = Convert.ToInt32(reader("OrderCount"))

                                ' Reverse week numbering (0 = current week = Week 4)
                                Dim labelIndex As Integer = 3 - weekNum
                                If labelIndex >= 0 AndAlso labelIndex < labels.Count Then
                                    If status = "Completed" OrElse status = "Served" Then
                                        completedData(labels(labelIndex)) += count
                                    ElseIf status = "Cancelled" Then
                                        cancelledData(labels(labelIndex)) += count
                                    End If
                                End If
                            End While
                        End Using
                    End Using

                Case "Yearly"
                    ' Show monthly data for current year
                    For i As Integer = 1 To 12
                        labels.Add(New DateTime(Date.Today.Year, i, 1).ToString("MMM"))
                    Next

                    sql = "
                SELECT 
                    MONTH(OrderDate) AS MonthNum,
                    OrderStatus,
                    COUNT(*) AS OrderCount
                FROM orders
                WHERE YEAR(OrderDate) = YEAR(CURDATE())
                AND OrderStatus IN ('Completed', 'Cancelled', 'Served')
                GROUP BY MONTH(OrderDate), OrderStatus
                ORDER BY MONTH(OrderDate)"

                    ' Initialize data
                    For Each label In labels
                        completedData(label) = 0
                        cancelledData(label) = 0
                    Next

                    Using cmd As New MySqlCommand(sql, conn)
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim monthNum As Integer = Convert.ToInt32(reader("MonthNum"))
                                Dim status As String = reader("OrderStatus").ToString()
                                Dim count As Integer = Convert.ToInt32(reader("OrderCount"))

                                If monthNum >= 1 AndAlso monthNum <= 12 Then
                                    Dim label As String = labels(monthNum - 1)
                                    If status = "Completed" OrElse status = "Served" Then
                                        completedData(label) += count
                                    ElseIf status = "Cancelled" Then
                                        cancelledData(label) += count
                                    End If
                                End If
                            End While
                        End Using
                    End Using
            End Select

            closeConn()

            ' Update chart with data
            UpdateOrdersOverviewChart(labels, completedData, cancelledData)

        Catch ex As Exception
            MessageBox.Show("Error loading orders overview data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            closeConn()
        End Try
    End Sub

    Private Sub UpdateOrdersOverviewChart(labels As List(Of String),
                                      completedData As Dictionary(Of String, Integer),
                                      cancelledData As Dictionary(Of String, Integer))

        Dim completedSeries As Series = OrdersOverviewChart.Series("Completed")
        Dim cancelledSeries As Series = OrdersOverviewChart.Series("Cancelled")

        completedSeries.Points.Clear()
        cancelledSeries.Points.Clear()

        If labels.Count = 0 Then
            ' Set default axis values for empty data
            OrdersOverviewChart.ChartAreas(0).AxisY.Minimum = 0
            OrdersOverviewChart.ChartAreas(0).AxisY.Maximum = 50
            OrdersOverviewChart.ChartAreas(0).AxisY.Interval = 10

            completedSeries.Points.AddXY("No data", 0)
            cancelledSeries.Points.AddXY("No data", 0)
            Return
        End If

        ' Find max value for Y-axis scaling
        Dim maxValue As Integer = 0
        For Each label In labels
            Dim completedCount As Integer = If(completedData.ContainsKey(label), completedData(label), 0)
            Dim cancelledCount As Integer = If(cancelledData.ContainsKey(label), cancelledData(label), 0)
            Dim total As Integer = completedCount + cancelledCount
            If total > maxValue Then maxValue = total
        Next

        ' Set Y-axis with proper values (avoid zero maximum)
        With OrdersOverviewChart.ChartAreas(0).AxisY
            .Minimum = 0

            If maxValue = 0 Then
                ' No data, set reasonable default
                .Maximum = 50
                .Interval = 10
            Else
                ' Calculate nice maximum and interval
                Dim calculatedMax As Double = Math.Ceiling(maxValue * 1.3) ' 30% padding

                ' Round up to nearest nice number
                If calculatedMax <= 10 Then
                    .Maximum = 10
                    .Interval = 2
                ElseIf calculatedMax <= 50 Then
                    .Maximum = Math.Ceiling(calculatedMax / 10) * 10
                    .Interval = Math.Max(5, Math.Ceiling(.Maximum / 5))
                ElseIf calculatedMax <= 100 Then
                    .Maximum = Math.Ceiling(calculatedMax / 20) * 20
                    .Interval = Math.Max(10, Math.Ceiling(.Maximum / 5))
                Else
                    .Maximum = Math.Ceiling(calculatedMax / 50) * 50
                    .Interval = Math.Max(25, Math.Ceiling(.Maximum / 4))
                End If
            End If
        End With

        ' Add data points
        For Each label In labels
            Dim completedCount As Integer = If(completedData.ContainsKey(label), completedData(label), 0)
            Dim cancelledCount As Integer = If(cancelledData.ContainsKey(label), cancelledData(label), 0)

            completedSeries.Points.AddXY(label, completedCount)
            cancelledSeries.Points.AddXY(label, cancelledCount)
        Next
    End Sub
    ' ============================================


    ' ============================================
    ' SIMPLIFIED: Call FormReservationStatus chart rendering directly
    ' Add this to Dashboard.vb, replacing LoadReservationChart method
    ' ============================================

    ' ============================================
    ' SHARED: Get Reservation Status Data
    ' This returns the same data structure used by FormReservationStatus
    ' ============================================
    Private Function GetReservationStatusData(period As String) As Dictionary(Of String, Integer)
        Dim data As New Dictionary(Of String, Integer) From {
            {"Pending", 0},
            {"Confirmed", 0},
            {"Cancelled", 0},
            {"Completed", 0}
        }

        Try
            openConn()

            ' Get date filter based on selected period
            Dim dateFilter As String = GetDateFilterCondition("ReservationDate")

            cmd = New MySqlCommand($"
            SELECT 
                ReservationStatus,
                COUNT(*) AS StatusCount
            FROM reservations
            WHERE {dateFilter}
            GROUP BY ReservationStatus", conn)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                Dim status As String = reader("ReservationStatus").ToString()
                Dim count As Integer = Convert.ToInt32(reader("StatusCount"))
                If data.ContainsKey(status) Then
                    data(status) = count
                End If
            End While

            reader.Close()
            closeConn()

        Catch ex As Exception
            closeConn()
            Throw
        End Try

        Return data
    End Function
    ' ============================================
    ' SHARED: Render Reservation Chart
    ' This uses the EXACT same rendering logic as FormReservationStatus
    ' ============================================
    Private Sub RenderReservationChart(chart As DataVisualization.Charting.Chart, data As Dictionary(Of String, Integer))
        Try
            ' Clear chart
            chart.Series("ReservationStatus").Points.Clear()
            chart.Annotations.Clear()

            Dim totalCount As Integer = data.Values.Sum()

            If totalCount > 0 Then
                ' Add Pending (Orange) - Same as FormReservationStatus
                If data("Pending") > 0 Then
                    Dim point As New DataVisualization.Charting.DataPoint()
                    point.SetValueXY("Pending", data("Pending"))
                    point.Color = Color.FromArgb(255, 165, 0)
                    point.BorderColor = Color.White
                    point.BorderWidth = 2
                    point.Label = data("Pending").ToString()
                    point.LegendText = $"Pending ({data("Pending")})"
                    point.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                    point.LabelForeColor = Color.White
                    chart.Series("ReservationStatus").Points.Add(point)
                End If

                ' Add Confirmed (Green)
                If data("Confirmed") > 0 Then
                    Dim point As New DataVisualization.Charting.DataPoint()
                    point.SetValueXY("Confirmed", data("Confirmed"))
                    point.Color = Color.FromArgb(34, 197, 94)
                    point.BorderColor = Color.White
                    point.BorderWidth = 2
                    point.Label = data("Confirmed").ToString()
                    point.LegendText = $"Confirmed ({data("Confirmed")})"
                    point.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                    point.LabelForeColor = Color.White
                    chart.Series("ReservationStatus").Points.Add(point)
                End If

                ' Add Cancelled (Red)
                If data("Cancelled") > 0 Then
                    Dim point As New DataVisualization.Charting.DataPoint()
                    point.SetValueXY("Cancelled", data("Cancelled"))
                    point.Color = Color.FromArgb(239, 68, 68)
                    point.BorderColor = Color.White
                    point.BorderWidth = 2
                    point.Label = data("Cancelled").ToString()
                    point.LegendText = $"Cancelled ({data("Cancelled")})"
                    point.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                    point.LabelForeColor = Color.White
                    chart.Series("ReservationStatus").Points.Add(point)
                End If

                ' Add Completed (Blue)
                If data("Completed") > 0 Then
                    Dim point As New DataVisualization.Charting.DataPoint()
                    point.SetValueXY("Completed", data("Completed"))
                    point.Color = Color.FromArgb(59, 130, 246)
                    point.BorderColor = Color.White
                    point.BorderWidth = 2
                    point.Label = data("Completed").ToString()
                    point.LegendText = $"Completed ({data("Completed")})"
                    point.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                    point.LabelForeColor = Color.White
                    chart.Series("ReservationStatus").Points.Add(point)
                End If

                ' Configure legend
                chart.Legends(0).Enabled = True
                chart.Legends(0).Docking = Docking.Right
                chart.Legends(0).Font = New Font("Segoe UI", 9)
                chart.Legends(0).BackColor = Color.Transparent
            Else
                ' No data message
                Dim noDataAnnotation As New TextAnnotation()
                noDataAnnotation.Text = "No Reservation Data"
                noDataAnnotation.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                noDataAnnotation.ForeColor = Color.Gray
                noDataAnnotation.X = 50
                noDataAnnotation.Y = 50
                noDataAnnotation.Alignment = ContentAlignment.MiddleCenter
                chart.Annotations.Add(noDataAnnotation)
                chart.Legends(0).Enabled = False
            End If

            ' Configure 3D effect
            chart.ChartAreas(0).Area3DStyle.Enable3D = True
            chart.ChartAreas(0).Area3DStyle.Inclination = 15
            chart.ChartAreas(0).Area3DStyle.Rotation = 10
            chart.Series("ReservationStatus")("PieLabelStyle") = "Inside"
            chart.Series("ReservationStatus").IsValueShownAsLabel = True

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Private Sub ConfigureChartTopProducts()
        ' Make the chart cursor indicate it's clickable
        ChartTopProducts.Cursor = Cursors.Hand
        ' Add tooltip to indicate it's clickable
        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(Chart2, "Click to view detailed Catering Reservations report")
    End Sub
    Private Sub ConfigureChart2OrderTrends()
        ' Make the chart cursor indicate it's clickable
        OrdersOverviewChart.Cursor = Cursors.Hand
        ' Add tooltip to indicate it's clickable
        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(Chart2, "Click to view detailed Catering Reservations report")
    End Sub





    Private Sub MonthlyChartOrder_Click(sender As Object, e As EventArgs) Handles OrdersOverviewChart.Click
        Try
            ' Get reference to AdminDashboard (parent form)
            Dim adminDashboard As AdminDashboard = TryCast(Me.ParentForm, AdminDashboard)

            If adminDashboard IsNot Nothing Then
                ' First, load the Reports form in AdminDashboard
                adminDashboard.btnReports.PerformClick()

                ' Give UI time to load
                Application.DoEvents()

                ' Then load the Catering Reservations report
                If Reports IsNot Nothing Then
                    Reports.LoadOrderTrends()
                End If
            Else
                MessageBox.Show("Unable to navigate to Reports section.", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error navigating to orders: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ChartTopProducts_Click(sender As Object, e As EventArgs)
        Try
            ' Get reference to AdminDashboard (parent form)
            Dim adminDashboard As AdminDashboard = TryCast(Me.ParentForm, AdminDashboard)

            If adminDashboard IsNot Nothing Then
                ' First, load the Reports form in AdminDashboard
                adminDashboard.btnReports.PerformClick()

                ' Give UI time to load
                Application.DoEvents()

                ' Then load the Catering Reservations report
                If Reports IsNot Nothing Then
                    Reports.LoadProductPerformanceReport()
                End If
            Else
                MessageBox.Show("Unable to navigate to Reports section.", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error navigating to orders: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ChartTopProducts_MouseEnter(sender As Object, e As EventArgs)
        Chart2.Cursor = Cursors.Hand
        RoundedPane28.BackColor = Color.FromArgb(248, 248, 250)
    End Sub

    Private Sub ChartTopProducts_MouseLeave(sender As Object, e As EventArgs)
        Chart2.Cursor = Cursors.Default
        RoundedPane28.BackColor = Color.White
    End Sub

    Private Sub OrdersOverviewChart_MouseEnter(sender As Object, e As EventArgs) Handles OrdersOverviewChart.MouseEnter
        Chart2.Cursor = Cursors.Hand
        RoundedPane28.BackColor = Color.FromArgb(248, 248, 250)
    End Sub

    Private Sub OrdersOverviewChart_MouseLeave(sender As Object, e As EventArgs) Handles OrdersOverviewChart.MouseLeave
        Chart2.Cursor = Cursors.Default
        RoundedPane28.BackColor = Color.White
    End Sub

    ' Add visual feedback when hovering over the chart


    ' Add visual feedback when hovering over the chart

    Private Sub RoundedPane21_Paint(sender As Object, e As PaintEventArgs)


    End Sub
    ' ============================================
    ' GET SALES DATA - Same logic as FormSales
    ' ============================================
    Private Function GetSalesData() As Dictionary(Of String, (Revenue As Decimal, Expenses As Decimal, Profit As Decimal))
        Dim salesData As New Dictionary(Of String, (Revenue As Decimal, Expenses As Decimal, Profit As Decimal))
        Dim currentYear As Integer = DateTime.Now.Year

        ' Initialize all 12 months with zero values
        For month As Integer = 1 To 12
            Dim monthName As String = New DateTime(currentYear, month, 1).ToString("MMM")
            salesData(monthName) = (0D, 0D, 0D)
        Next

        Try
            openConn()

            ' Build the sales query using the same logic as FormSales
            Dim sql As String = BuildSalesQueryForDashboard(currentYear)

            cmd = New MySqlCommand(sql, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                Dim monthNum As Integer = Convert.ToInt32(reader("MonthNum"))
                Dim monthName As String = New DateTime(currentYear, monthNum, 1).ToString("MMM")

                Dim revenue As Decimal = If(IsDBNull(reader("TotalRevenue")), 0D, Convert.ToDecimal(reader("TotalRevenue")))
                Dim expenses As Decimal = If(IsDBNull(reader("TotalExpenses")), 0D, Convert.ToDecimal(reader("TotalExpenses")))
                Dim profit As Decimal = revenue - expenses

                salesData(monthName) = (revenue, expenses, profit)
            End While

            reader.Close()
            closeConn()

        Catch ex As Exception
            closeConn()
            Throw New Exception($"Error fetching sales data: {ex.Message}", ex)
        End Try

        Return salesData
    End Function


    ' ============================================
    ' BUILD SALES QUERY - Exact same as FormSales
    ' ============================================
    Private Function BuildSalesQueryForDashboard(currentYear As Integer) As String
        Dim queries As New List(Of String)

        ' Check and add payments table
        If TableExists("payments") Then
            queries.Add($"
            SELECT MONTH(PaymentDate) AS MonthNum, AmountPaid AS Amount, 'Revenue' AS Type
            FROM payments
            WHERE PaymentStatus IN ('Paid','Completed')
            AND YEAR(PaymentDate) = {currentYear}
        ")
        End If

        ' We now use the unified 'payments' table which is already handled above.
        ' We primarily rely on orders and payments now, which should exist.
        If TableExists("orders") OrElse TableExists("payments") Then
            queries.Add($"
            SELECT MONTH(sales_date) AS MonthNum, revenue AS Amount, 'Revenue' AS Type
            FROM sales
            WHERE YEAR(sales_date) = {currentYear}
        ")

            queries.Add($"
            SELECT MONTH(sales_date) AS MonthNum, expenses AS Amount, 'Expenses' AS Type
            FROM sales
            WHERE YEAR(sales_date) = {currentYear}
        ")
        End If

        ' Add inventory batches as expenses
        If TableExists("inventory_batches") Then
            queries.Add($"
            SELECT MONTH(PurchaseDate) AS MonthNum, TotalCost AS Amount, 'Expenses' AS Type
            FROM inventory_batches
            WHERE BatchStatus = 'Active'
            AND YEAR(PurchaseDate) = {currentYear}
        ")
        End If

        If queries.Count = 0 Then
            Throw New Exception("No valid sales tables found.")
        End If

        Return $"
        SELECT 
            MonthNum,
            SUM(CASE WHEN Type='Revenue' THEN Amount ELSE 0 END) AS TotalRevenue,
            SUM(CASE WHEN Type='Expenses' THEN Amount ELSE 0 END) AS TotalExpenses
        FROM ({String.Join(" UNION ALL ", queries)}) AS combined
        GROUP BY MonthNum 
        ORDER BY MonthNum
    "
    End Function

    ' ============================================
    ' RENDER SALES CHART - Same styling as FormSales
    ' ============================================
    Private Sub RenderSalesChart(chart As DataVisualization.Charting.Chart, salesData As Dictionary(Of String, (Revenue As Decimal, Expenses As Decimal, Profit As Decimal)))
        Try
            ' Clear existing series if they exist
            chart.Series.Clear()

            ' Create the three series
            Dim revenueSeries As New DataVisualization.Charting.Series("Revenue")
            Dim expensesSeries As New DataVisualization.Charting.Series("Expenses")
            Dim profitSeries As New DataVisualization.Charting.Series("NetProfit")

            ' Configure series appearance - Same as FormSales
            revenueSeries.ChartType = SeriesChartType.Column
            revenueSeries.Color = Color.FromArgb(99, 102, 241)  ' Indigo
            revenueSeries.BorderWidth = 0
            revenueSeries("PointWidth") = "0.6"

            expensesSeries.ChartType = SeriesChartType.Column
            expensesSeries.Color = Color.FromArgb(239, 68, 68)  ' Red
            expensesSeries.BorderWidth = 0
            expensesSeries("PointWidth") = "0.6"

            profitSeries.ChartType = SeriesChartType.Column
            profitSeries.Color = Color.FromArgb(34, 197, 94)  ' Green
            profitSeries.BorderWidth = 0
            profitSeries("PointWidth") = "0.6"

            ' Add data points for all 12 months
            Dim currentYear As Integer = DateTime.Now.Year
            For month As Integer = 1 To 12
                Dim monthName As String = New DateTime(currentYear, month, 1).ToString("MMM")

                If salesData.ContainsKey(monthName) Then
                    Dim data = salesData(monthName)

                    revenueSeries.Points.AddXY(monthName, data.Revenue)
                    expensesSeries.Points.AddXY(monthName, data.Expenses)
                    profitSeries.Points.AddXY(monthName, data.Profit)
                Else
                    revenueSeries.Points.AddXY(monthName, 0)
                    expensesSeries.Points.AddXY(monthName, 0)
                    profitSeries.Points.AddXY(monthName, 0)
                End If
            Next

            ' Add series to chart
            chart.Series.Add(revenueSeries)
            chart.Series.Add(expensesSeries)
            chart.Series.Add(profitSeries)

            ' Configure chart area styling - Same as FormSales
            If chart.ChartAreas.Count > 0 Then
                With chart.ChartAreas(0)
                    ' X-axis styling
                    .AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                    .AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot
                    .AxisX.LabelStyle.Font = New Font("Segoe UI", 9)

                    ' Y-axis styling
                    .AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                    .AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot
                    .AxisY.LabelStyle.Format = "₱{0:N0}"
                    .AxisY.LabelStyle.Font = New Font("Segoe UI", 9)
                End With
            End If

            ' Configure tooltips
            For Each series As DataVisualization.Charting.Series In chart.Series
                series.ToolTip = "#VALX: ₱#VALY{N2}"
            Next

            ' Configure legend
            If chart.Legends.Count > 0 Then
                chart.Legends(0).Font = New Font("Segoe UI", 9)
                chart.Legends(0).Docking = Docking.Bottom
                chart.Legends(0).BackColor = Color.Transparent
            End If

            ' Check if there's any data
            Dim hasData As Boolean = salesData.Values.Any(Function(d) d.Revenue > 0 OrElse d.Expenses > 0)

            If Not hasData Then
                ' Show "No Data" message
                chart.Annotations.Clear()
                Dim noDataAnnotation As New TextAnnotation()
                noDataAnnotation.Text = "No Sales Data Available"
                noDataAnnotation.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                noDataAnnotation.ForeColor = Color.Gray
                noDataAnnotation.X = 50
                noDataAnnotation.Y = 50
                noDataAnnotation.Alignment = ContentAlignment.MiddleCenter
                chart.Annotations.Add(noDataAnnotation)
            End If

        Catch ex As Exception
            Throw New Exception($"Error rendering sales chart: {ex.Message}", ex)
        End Try
    End Sub


    Private Function TableExists(tableName As String) As Boolean
        Try
            If conn Is Nothing Then Return False

            If conn.State <> ConnectionState.Open Then
                openConn()
            End If

            Dim sql As String = "
            SELECT COUNT(*) 
            FROM information_schema.tables
            WHERE table_schema = DATABASE()
            AND LOWER(table_name) = LOWER(@TableName)
        "

            Dim checkCmd As New MySqlCommand(sql, conn)
            checkCmd.Parameters.AddWithValue("@TableName", tableName)
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            Return count > 0

        Catch ex As Exception
            Return False
        End Try
    End Function

    ' 
    ' Add this method to your Reports.vb file
    Private Sub ProductPerformance_Click(sender As Object, e As EventArgs)
        Try
            ' Get reference to AdminDashboard (parent form)
            Dim adminDashboard As AdminDashboard = TryCast(Me.ParentForm, AdminDashboard)

            If adminDashboard IsNot Nothing Then
                ' First, load the Reports form in AdminDashboard
                adminDashboard.btnReports.PerformClick()

                ' Give UI time to load
                Application.DoEvents()

                ' Then load the Catering Reservations report
                If Reports IsNot Nothing Then
                    Reports.LoadProductPerformanceReport()
                End If
            Else
                MessageBox.Show("Unable to navigate to Reports section.", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error navigating to catering reservations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub RoundedPane24_Paint(sender As Object, e As PaintEventArgs) Handles RoundedPane24.Paint

    End Sub




    ' ============================================
    ' TOP PRODUCTS CHART
    ' ============================================

End Class