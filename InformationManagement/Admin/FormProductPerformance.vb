Imports System.Globalization
Imports System.Windows.Forms.DataVisualization.Charting
Imports MySqlConnector

Public Class FormProductPerformance

    Public Property DefaultView As String = "Product"

    Private ReadOnly currencyCulture As CultureInfo = CultureInfo.GetCultureInfo("en-PH")
    Private summaryTiles As List(Of SummaryTile)
    Private topProductsLimit As Integer = 10
    Private selectedCategory As String = "All Categories"
    Private viewType As String = "Product" ' Options: "Product", "Category"

    Private Class SummaryTile
        Public Property NameLabel As Label
        Public Property DetailLabel As Label
    End Class

    ' =======================================================================
    ' REFRESH DATA
    ' =======================================================================
    Public Sub RefreshData()
        ' Update any local state if needed
        LoadProductPerformance()
    End Sub

    Private Sub FormProductPerformance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        viewType = DefaultView
        InitializeSummaryTiles()
        InitializeControls()

        ' Update combo boxes to match viewType
        Dim comboViewType = TryCast(RoundedPane21.Controls("ComboBoxViewType"), ComboBox)
        If comboViewType IsNot Nothing Then
            comboViewType.SelectedIndex = If(viewType = "Category", 1, 0)
        End If

        ConfigureChart()
        LoadProductPerformance()
    End Sub

    Private Sub InitializeSummaryTiles()


        summaryTiles = New List(Of SummaryTile) From {
            New SummaryTile With {.NameLabel = Label2, .DetailLabel = Label3},
            New SummaryTile With {.NameLabel = Label5, .DetailLabel = Label4},
            New SummaryTile With {.NameLabel = Label7, .DetailLabel = Label6},
            New SummaryTile With {.NameLabel = Label9, .DetailLabel = Label8},
            New SummaryTile With {.NameLabel = Label13, .DetailLabel = Label12},
            New SummaryTile With {.NameLabel = Label11, .DetailLabel = Label10}
        }
    End Sub

    Private Sub InitializeControls()
        ' Programmatically add ComboBoxViewType and ComboBoxCategory if they don't exist
        Dim comboViewType = TryCast(RoundedPane21.Controls("ComboBoxViewType"), ComboBox)
        If comboViewType Is Nothing Then
            comboViewType = New ComboBox()
            With comboViewType
                .Name = "ComboBoxViewType"
                .Location = New Point(Label1.Right + 20, Label1.Top + 5)
                .Size = New Size(150, 30)
                .DropDownStyle = ComboBoxStyle.DropDownList
                .Items.AddRange(New String() {"View by Product", "View by Category"})
                .SelectedIndex = 0
                .Font = New Font("Segoe UI", 9.0F)
            End With
            RoundedPane21.Controls.Add(comboViewType)
            AddHandler comboViewType.SelectedIndexChanged, AddressOf ComboBoxViewType_SelectedIndexChanged
        End If

        Dim comboCategory = TryCast(RoundedPane21.Controls("ComboBoxCategory"), ComboBox)
        If comboCategory Is Nothing Then
            comboCategory = New ComboBox()
            With comboCategory
                .Name = "ComboBoxCategory"
                .Location = New Point(comboViewType.Right + 10, comboViewType.Top)
                .Size = New Size(150, 30)
                .DropDownStyle = ComboBoxStyle.DropDownList
                .Font = New Font("Segoe UI", 9.0F)
            End With
            RoundedPane21.Controls.Add(comboCategory)

            ' Load categories dynamically from database
            LoadCategories(comboCategory)

            AddHandler comboCategory.SelectedIndexChanged, AddressOf ComboBoxCategory_SelectedIndexChanged
        End If
    End Sub

    Private Sub LoadCategories(comboBox As ComboBox)
        Try
            comboBox.Items.Clear()
            comboBox.Items.Add("All Categories")

            ' Fetch distinct categories from database
            Dim query As String = "SELECT DISTINCT Category FROM products WHERE Category IS NOT NULL AND Category <> '' ORDER BY Category"

            Using connection As New MySqlConnection(strConnection)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Using reader = command.ExecuteReader()
                        While reader.Read()
                            Dim category = reader("Category").ToString().Trim()
                            If Not String.IsNullOrWhiteSpace(category) Then
                                comboBox.Items.Add(category)
                            End If
                        End While
                    End Using
                End Using
            End Using

            comboBox.SelectedIndex = 0
        Catch ex As Exception
            ' Fallback to hardcoded categories if database query fails
            comboBox.Items.AddRange(New String() {
                "All Categories",
                "Platter",
                "Rice Meal",
                "Spaghetti Meals",
                "Snacks",
                "Dessert",
                "Drinks"
            })
            comboBox.SelectedIndex = 0

            MessageBox.Show($"Could not load categories from database. Using default categories.{Environment.NewLine}{ex.Message}",
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub ComboBoxViewType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim combo = DirectCast(sender, ComboBox)
        viewType = If(combo.SelectedIndex = 1, "Category", "Product")

        ' Show/Hide category filter based on view type
        Dim comboCategory = RoundedPane21.Controls("ComboBoxCategory")
        If comboCategory IsNot Nothing Then
            comboCategory.Visible = (viewType = "Product")
        End If

        LoadProductPerformance()
    End Sub

    Private Sub ComboBoxCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim combo = DirectCast(sender, ComboBox)
        selectedCategory = combo.SelectedItem.ToString()
        LoadProductPerformance()
    End Sub



    Private Sub ConfigureChart()
        Chart1.Series.Clear()
        Chart1.Titles.Clear()
        Chart1.Legends.Clear()

        If Chart1.ChartAreas.Count = 0 Then
            Chart1.ChartAreas.Add(New ChartArea("ChartArea1"))
        End If

        Dim chartArea = Chart1.ChartAreas(0)
        With chartArea
            .AxisX.Interval = 1
            .AxisX.MajorGrid.Enabled = False
            .AxisX.LabelStyle.Font = New Font("Segoe UI", 9.0F)
            .AxisX.LabelStyle.Angle = -45
            .AxisX.IsLabelAutoFit = True

            .AxisX.ScaleView.Zoomable = True
            .AxisX.ScrollBar.Enabled = True
            .AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
            .AxisX.ScrollBar.Size = 15
            .AxisX.ScaleView.Size = 15

            .AxisY.LabelStyle.Format = "₱#,##0"
            .AxisY.LabelStyle.Font = New Font("Segoe UI", 9.0F)
            .AxisY.MajorGrid.LineColor = Color.LightGray
            .AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot
            .BackColor = Color.White
        End With

        Dim series = Chart1.Series.Add("Revenue")
        With series
            .ChartType = SeriesChartType.Column
            .Color = Color.MediumSlateBlue
            .BorderWidth = 0
            .IsValueShownAsLabel = True
            .LabelFormat = "₱#,##0"
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
            .LabelAngle = -90
        End With

        Chart1.Titles.Add(New Title With {
            .Text = "Revenue Performance",
            .Alignment = ContentAlignment.TopLeft,
            .Font = New Font("Segoe UI Semibold", 11.25F, FontStyle.Bold)
        })
    End Sub

    Private Async Sub LoadProductPerformance()
        Try
            ' Disable refresh while loading
            Dim comboViewType = TryCast(RoundedPane21.Controls("ComboBoxViewType"), ComboBox)
            If comboViewType IsNot Nothing Then comboViewType.Enabled = False

            Dim periodText = $"({Reports.SelectedPeriod})"
            Dim filterText = If(viewType = "Product" AndAlso selectedCategory <> "All Categories", $" - {selectedCategory}", "")
            Dim viewTypeText = If(viewType = "Category", "by Category", "by Product")

            Chart1.Titles(0).Text = $"Loading {viewTypeText} {periodText}..."

            ' Fetch data asynchronously
            Dim performanceData = Await Task.Run(Function() FetchProductPerformanceData())

            UpdateChart(performanceData)
            UpdateSummaryTiles(performanceData)

            If comboViewType IsNot Nothing Then comboViewType.Enabled = True
        Catch ex As Exception
            MessageBox.Show($"Unable to load product performance.{Environment.NewLine}{ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Chart1.Titles(0).Text = "Error Loading Data"
        End Try
    End Sub

    Private Function FetchProductPerformanceData() As DataTable
        ' Use shared period from Reports form
        Dim periodFilter As String = Reports.SelectedPeriod
        Dim dateColumnReservations As String = "r.ReservationDate"
        Dim dateColumnOrders As String = "o.OrderDate"

        Dim whereClauseReservations As String = ""
        Dim whereClauseOrders As String = ""

        Dim selectedYear As Integer = Reports.SelectedYear
        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case periodFilter
            Case "Daily"
                whereClauseReservations = $" AND DATE({dateColumnReservations}) = '{Reports.GlobalFilterDate:yyyy-MM-dd}'"
                whereClauseOrders = $" AND DATE({dateColumnOrders}) = '{Reports.GlobalFilterDate:yyyy-MM-dd}'"
            Case "Weekly"
                whereClauseReservations = $" AND YEARWEEK({dateColumnReservations}, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1)"
                whereClauseOrders = $" AND YEARWEEK({dateColumnOrders}, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1)"
            Case "Monthly"
                If selectedMonth = 0 Then
                    whereClauseReservations = $" AND YEAR({dateColumnReservations}) = {selectedYear}"
                    whereClauseOrders = $" AND YEAR({dateColumnOrders}) = {selectedYear}"
                Else
                    whereClauseReservations = $" AND YEAR({dateColumnReservations}) = {selectedYear} AND MONTH({dateColumnReservations}) = {selectedMonth}"
                    whereClauseOrders = $" AND YEAR({dateColumnOrders}) = {selectedYear} AND MONTH({dateColumnOrders}) = {selectedMonth}"
                End If
            Case "Yearly"
                whereClauseReservations = $" AND YEAR({dateColumnReservations}) = {selectedYear}"
                whereClauseOrders = $" AND YEAR({dateColumnOrders}) = {selectedYear}"
        End Select

        Dim groupByColumn As String = If(viewType = "Category", "p.Category", "p.ProductName")
        Dim selectItems As String = If(viewType = "Category", "p.Category AS DisplayName", "p.ProductName AS DisplayName")

        ' Fix: Use TRIM to remove any extra spaces and make category comparison case-insensitive
        Dim categoryFilterLine As String = ""
        If viewType = "Product" AndAlso selectedCategory <> "All Categories" Then
            categoryFilterLine = " AND TRIM(p.Category) = @Category"
        End If

        Dim query As String =
$"SELECT DisplayName,
        SUM(TotalOrders) AS TotalOrders,
        SUM(Revenue) AS Revenue
 FROM (
        -- Reservation items
        SELECT {selectItems},
               SUM(ri.Quantity) AS TotalOrders,
               SUM(ri.TotalPrice) AS Revenue
        FROM reservation_items ri
        INNER JOIN reservations r ON ri.ReservationID = r.ReservationID
        INNER JOIN products p ON TRIM(ri.ProductName) = TRIM(p.ProductName)
        WHERE r.ReservationStatus IN ('Confirmed', 'Served')
        {whereClauseReservations}
        {categoryFilterLine}
        GROUP BY {groupByColumn}
        
        UNION ALL
        
        -- Order items
        SELECT {selectItems},
               SUM(oi.Quantity) AS TotalOrders,
               SUM(oi.Quantity * (CASE WHEN oi.UnitPrice > 0 THEN oi.UnitPrice ELSE p.Price END)) AS Revenue
        FROM order_items oi
        INNER JOIN orders o ON oi.OrderID = o.OrderID
        INNER JOIN products p ON TRIM(oi.ProductName) = TRIM(p.ProductName)
        WHERE o.OrderStatus IN ('Served', 'Completed')
        {whereClauseOrders}
        {categoryFilterLine}
        GROUP BY {groupByColumn}
      ) AS combined
 GROUP BY DisplayName
 ORDER BY Revenue DESC"

        If viewType = "Product" AndAlso topProductsLimit > 0 Then
            query &= " LIMIT " & topProductsLimit
        End If

        query &= ";"

        Dim dt As New DataTable()

        Using connection As New MySqlConnection(strConnection)
            connection.Open()
            Using command As New MySqlCommand(query, connection)
                If viewType = "Product" AndAlso selectedCategory <> "All Categories" Then
                    command.Parameters.AddWithValue("@Category", selectedCategory.Trim())
                End If
                Using reader = command.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Private Sub UpdateChart(data As DataTable)
        Dim series = Chart1.Series("Revenue")
        series.Points.Clear()

        If data.Rows.Count = 0 Then
            series.Points.AddXY("No data", 0)
            Return
        End If

        For Each row As DataRow In data.Rows
            Dim displayName = row("DisplayName").ToString()
            Dim revenue = If(IsDBNull(row("Revenue")), 0D, Convert.ToDecimal(row("Revenue")))

            Dim pointIndex = series.Points.AddXY(displayName, revenue)
            ' Add some visual polish to the points
            Dim point = series.Points(pointIndex)
            point.ToolTip = $"{displayName}: {String.Format(currencyCulture, "{0:C0}", revenue)}"
        Next

        ' Smart label density
        If data.Rows.Count <= 12 Then
            series.IsValueShownAsLabel = True
            series.LabelFormat = "₱#,##0"
            series.LabelForeColor = Color.FromArgb(71, 85, 105)
        Else
            series.IsValueShownAsLabel = False
        End If

        ' Axis and Scrolling
        Dim chartArea = Chart1.ChartAreas(0)
        chartArea.AxisX.Interval = 1
        chartArea.AxisX.LabelStyle.Font = New Font("Segoe UI", 9.0!)
        
        If data.Rows.Count > 15 Then
            chartArea.AxisX.ScrollBar.Enabled = True
            chartArea.AxisX.ScaleView.Zoomable = True
            chartArea.AxisX.ScaleView.Size = 15
            chartArea.AxisX.ScaleView.Position = 1
            chartArea.AxisX.LabelStyle.Angle = -45
        Else
            chartArea.AxisX.ScrollBar.Enabled = False
            chartArea.AxisX.ScaleView.ZoomReset()
            chartArea.AxisX.LabelStyle.Angle = If(data.Rows.Count > 8, -45, 0)
        End If

        Dim periodPart As String = ""
        If Reports.SelectedPeriod = "Daily" Then
            periodPart = $" ({Reports.GlobalFilterDate:MMM dd, yyyy})"
        ElseIf Reports.SelectedPeriod = "Monthly" AndAlso Reports.SelectedMonth > 0 Then
            periodPart = $" ({System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Reports.SelectedMonth)} {Reports.SelectedYear})"
        Else
            periodPart = $" ({Reports.SelectedPeriod} {Reports.SelectedYear})"
        End If
        
        Dim filterText = If(viewType = "Product" AndAlso selectedCategory <> "All Categories", $" - {selectedCategory}", "")
        Dim viewTypeText = If(viewType = "Category", "by Category", "by Product")
        
        Chart1.Titles(0).Text = $"Revenue {viewTypeText}{periodPart}{filterText}"
    End Sub

    Private Sub UpdateSummaryTiles(data As DataTable)
        If summaryTiles Is Nothing OrElse summaryTiles.Count = 0 Then Return

        For i As Integer = 0 To summaryTiles.Count - 1
            Dim tile = summaryTiles(i)

            If i < data.Rows.Count Then
                Dim row = data.Rows(i)
                Dim displayName = row("DisplayName").ToString()
                Dim totalOrders = If(IsDBNull(row("TotalOrders")), 0, Convert.ToInt64(row("TotalOrders")))
                Dim revenue = If(IsDBNull(row("Revenue")), 0D, Convert.ToDecimal(row("Revenue")))
                Dim revenueText = String.Format(currencyCulture, "{0:C0}", revenue)

                tile.NameLabel.Text = displayName
                tile.DetailLabel.Text = $"{totalOrders} orders | {revenueText}"
            Else
                tile.NameLabel.Text = "N/A"
                tile.DetailLabel.Text = "No data available"
            End If
        Next
    End Sub


End Class