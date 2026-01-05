Imports System.Windows.Forms.DataVisualization.Charting
Imports MySqlConnector

Public Class FormReservationStatus

    Private currentYear As Integer = DateTime.Now.Year
    Private currentMonth As Integer = DateTime.Now.Month
    Private filterPeriod As String = "Monthly" ' Daily, Weekly, Monthly, Yearly
    Private reservationData As New Dictionary(Of String, Integer)
    Private isInitializing As Boolean = True

    ' UI Controls created programmatically
    Private WithEvents cmbSource As ComboBox



    ' =======================================================================
    ' FORM LOAD
    ' =======================================================================
    Private Sub FormReservationStatus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            InitializeForm()
            ConfigureChart()
            'ConfigureDateFilter()
            LoadReservationData()
            isInitializing = False

        Catch ex As Exception
            MessageBox.Show($"Form Load Error: {ex.Message}{vbCrLf}{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================================
    ' INITIALIZE FORM CONTROLS
    ' =======================================================================
    Private Sub InitializeForm()
        Try
            filterPeriod = Reports.SelectedPeriod

            ' === CREATE SOURCE FILTER (Programmatically) ===
            If cmbSource Is Nothing Then
                cmbSource = New ComboBox()
                cmbSource.Name = "cmbSource"
                cmbSource.DropDownStyle = ComboBoxStyle.DropDownList
                cmbSource.Font = New Font("Segoe UI", 10)
                cmbSource.Size = New Size(150, 30)
                ' Positioning: Top Right, aligned with existing layout
                cmbSource.Location = New Point(Me.Width - 180, 40)
                cmbSource.Anchor = AnchorStyles.Top Or AnchorStyles.Right

                cmbSource.Items.Add("All Sources")
                cmbSource.Items.Add("Website (Online)")
                cmbSource.Items.Add("POS (Walk-in)")
                cmbSource.SelectedIndex = 0

                Me.Controls.Add(cmbSource)
                cmbSource.BringToFront()
            End If

            ' Configure chart colors
            Chart1.BackColor = Color.White
            If Chart1.ChartAreas.Count > 0 Then
                Chart1.ChartAreas(0).BackColor = Color.White
            End If

            ' Set label colors to White for better contrast on colored cards
            lblPending.ForeColor = Color.White
            lblConfirmed.ForeColor = Color.White
            lblCancelled.ForeColor = Color.White

            ' Set initial values to prevent blank display
            lblTotalReservations.Text = "0"
            lblPending.Text = "0"
            lblConfirmed.Text = "0"
            lblCancelled.Text = "0"

        Catch ex As Exception
            MessageBox.Show($"Initialize Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Handle Source Change
    Private Sub cmbSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSource.SelectedIndexChanged
        If Not isInitializing Then
            LoadReservationData()
        End If
    End Sub

    ' =======================================================================
    ' CONFIGURE PIE CHART
    ' =======================================================================
    Private Sub ConfigureChart()
        With Chart1
            .ChartAreas(0).BackColor = Color.Transparent
            .BackColor = Color.Transparent

            ' Configure series
            .Series("ReservationStatus").ChartType = SeriesChartType.Pie
            .Series("ReservationStatus").IsValueShownAsLabel = True
            .Series("ReservationStatus")("PieLabelStyle") = "Inside"
            .Series("ReservationStatus")("PieLineColor") = "Gray"
            .Series("ReservationStatus").Font = New Font("Segoe UI", 10, FontStyle.Bold)

            ' Enable 3D effect
            .ChartAreas(0).Area3DStyle.Enable3D = True
            .ChartAreas(0).Area3DStyle.Inclination = 15
            .ChartAreas(0).Area3DStyle.Rotation = 10

            ' Add legend
            .Legends(0).Enabled = True
            .Legends(0).Docking = Docking.Right
            .Legends(0).Font = New Font("Segoe UI", 10)
            .Legends(0).BackColor = Color.Transparent
        End With
    End Sub

    ' =======================================================================
    ' LOAD RESERVATION DATA FROM DATABASE
    ' =======================================================================
    Private Sub LoadReservationData()
        Try
            ' Check if connection exists
            If conn Is Nothing Then
                MessageBox.Show("Database connection not initialized. Please check your connection settings.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ' Set default values
                SetDefaultValues()
                Return
            End If

            If conn.State <> ConnectionState.Open Then
                openConn()
            End If

            filterPeriod = Reports.SelectedPeriod
            Dim selectedYear As Integer = Reports.SelectedYear
            If selectedYear = 0 Then selectedYear = DateTime.Now.Year

            Dim dateFilter As String = GetDateFilter()

            ' Get reservation counts by status
            ' Apply Source Filter
            Dim sourceFilter As String = ""
            If cmbSource IsNot Nothing AndAlso cmbSource.SelectedIndex > 0 Then
                If cmbSource.SelectedIndex = 1 Then
                    ' Website (Online)
                    sourceFilter = " AND ReservationType = 'Online'"
                ElseIf cmbSource.SelectedIndex = 2 Then
                    ' POS (Walk-in)
                    sourceFilter = " AND ReservationType = 'Walk-in'"
                End If
            End If

            ' Get reservation counts
            Dim sql As String = ""
            If filterPeriod = "Yearly" Then
                ' Multi-year trend: Count by Year
                sql = $"
                    SELECT 
                        YEAR(EventDate) AS Period,
                        COUNT(*) AS StatusCount
                    FROM reservations
                    WHERE YEAR(EventDate) <= {selectedYear} AND YEAR(EventDate) >= {selectedYear - 4}
                    {sourceFilter}
                    GROUP BY YEAR(EventDate)
                    ORDER BY YEAR(EventDate) ASC
                "
            Else
                ' Breakdown by Status for other periods
                sql = $"
                    SELECT 
                        ReservationStatus AS Period,
                        COUNT(*) AS StatusCount
                    FROM reservations
                    WHERE {dateFilter} {sourceFilter}
                    GROUP BY ReservationStatus
                "
            End If

            Using cmd As New MySqlCommand(sql, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Initialize defaults
                    reservationData.Clear()
                    If filterPeriod <> "Yearly" Then
                        reservationData("Pending") = 0
                        reservationData("Confirmed") = 0
                        reservationData("Cancelled") = 0
                        reservationData("Completed") = 0
                    End If

                    ' Load actual data
                    While reader.Read()
                        Dim key As String = reader("Period").ToString()
                        Dim count As Integer = Convert.ToInt32(reader("StatusCount"))
                        reservationData(key) = count
                    End While
                End Using
            End Using

            ' Update UI with data
            If filterPeriod = "Yearly" Then
                UpdateYearlyStatistics(selectedYear)
            Else
                UpdateStatisticsCards()
            End If
            UpdateChart()

            ' Update Title with context
            Dim periodText As String = $"{filterPeriod} Report"
            If filterPeriod = "Daily" Then
                periodText = $"Report for {Reports.GlobalFilterDate:MMM dd, yyyy}"
            ElseIf filterPeriod = "Monthly" Then
                If Reports.SelectedMonth > 0 Then
                    periodText = $"Report for {System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Reports.SelectedMonth)} {selectedYear}"
                Else
                    periodText = $"Report for Year {selectedYear}"
                End If
            ElseIf filterPeriod = "Yearly" Then
                periodText = $"Summary Throughout the Years ({selectedYear - 4}-{selectedYear})"
            End If

            Label4.Text = $"Reservation Status Breakdown - {periodText}"

        Catch ex As MySqlException
            MessageBox.Show($"Database Error: {ex.Message}{vbCrLf}Make sure the 'reservations' table exists with 'ReservationStatus' and 'ReservationDate' columns.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            SetDefaultValues()
        Catch ex As Exception
            MessageBox.Show($"Error loading reservation data: {ex.Message}{vbCrLf}{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            SetDefaultValues()
        End Try
    End Sub

    ' =======================================================================
    ' SET DEFAULT VALUES WHEN NO DATA AVAILABLE
    ' =======================================================================
    Private Sub SetDefaultValues()
        reservationData.Clear()
        reservationData("Pending") = 0
        reservationData("Confirmed") = 0
        reservationData("Cancelled") = 0
        reservationData("Completed") = 0

        UpdateStatisticsCards()
        UpdateChart()
    End Sub

    ' =======================================================================
    ' GET DATE FILTER BASED ON SELECTED PERIOD
    ' =======================================================================
    Private Function GetDateFilter() As String
        Dim filter As String = ""

        Dim selectedYear As Integer = Reports.SelectedYear
        If selectedYear = 0 Then selectedYear = DateTime.Now.Year

        Dim selectedMonth As Integer = Reports.SelectedMonth

        Select Case filterPeriod
            Case "Daily", "Weekly", "Monthly"
                ' All three now use the Month Scope logic
                If selectedMonth = 0 Then
                    filter = $"YEAR(EventDate) = {selectedYear}"
                Else
                    filter = $"YEAR(EventDate) = {selectedYear} AND MONTH(EventDate) = {selectedMonth}"
                End If

            Case "Yearly"
                filter = $"YEAR(EventDate) = {selectedYear}"

            Case Else
                filter = $"YEAR(EventDate) = {selectedYear}"
        End Select

        Return filter
    End Function

    ' Special update for Yearly view (multi-year)
    Private Sub UpdateYearlyStatistics(selectedYear As Integer)
        Try
            ' Get current year stats
            Dim currYearCount As Integer = 0
            Dim prevYearCount As Integer = 0
            
            ' Query current year
            Dim sqlCurr = $"SELECT COUNT(*) FROM reservations WHERE YEAR(EventDate) = {selectedYear}"
            Using cmd As New MySqlCommand(sqlCurr, conn)
                currYearCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            ' Query previous year
            Dim sqlPrev = $"SELECT COUNT(*) FROM reservations WHERE YEAR(EventDate) = {selectedYear - 1}"
            Using cmd As New MySqlCommand(sqlPrev, conn)
                prevYearCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            lblTotalReservations.Text = currYearCount.ToString()
            
            If prevYearCount > 0 Then
                Dim diff As Decimal = currYearCount - prevYearCount
                Dim percent As Decimal = (diff / prevYearCount) * 100
                Dim sign As String = If(diff >= 0, "+", "")
                lblTotalReservations.Text &= $" ({sign}{percent:N1}%)"
            End If

            ' Hide other labels or set to N/A for yearly multi-summary 
            ' or we can fetch their breakdowns too but let's keep it clean
            lblPending.Text = "-"
            lblConfirmed.Text = "-"
            lblCancelled.Text = "-"
            Label3.Text = "Awaiting Confirmation"
            Label5.Text = "Confirmed & Completed"
            Label7.Text = "Cancellations"

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    ' =======================================================================
    ' UPDATE STATISTICS CARDS
    ' =======================================================================
    Private Sub UpdateStatisticsCards()
        Try
            Dim pending As Integer = reservationData("Pending")
            Dim confirmed As Integer = reservationData("Confirmed")
            Dim completed As Integer = If(reservationData.ContainsKey("Completed"), reservationData("Completed"), 0)
            Dim cancelled As Integer = reservationData("Cancelled")

            ' Total includes completed
            Dim total As Integer = pending + confirmed + completed + cancelled

            ' Update labels
            lblTotalReservations.Text = total.ToString()
            lblPending.Text = pending.ToString()

            ' Confirmed Card now shows Active Confirmed (Ready to serve)
            ' But maybe user wants "Successful" = Confirmed + Completed?
            ' Let's show Confirmed + Completed in the Green Card to show total success
            ' Or keep it strictly Confirmed. 
            ' Given the label "Ready to serve", it usually implies future/active.
            ' But "Completed" implies served.
            ' Let's update the Green card to show BOTH if space allows, or SUM.
            ' Decision: Display SUM of Confirmed + Completed, as both are "Good" stats.
            lblConfirmed.Text = (confirmed + completed).ToString()

            lblCancelled.Text = cancelled.ToString()

            ' Calculate and show percentages
            If total > 0 Then
                Dim pendingPercent As Decimal = (pending / total) * 100
                Dim successPercent As Decimal = ((confirmed + completed) / total) * 100
                Dim cancelledPercent As Decimal = (cancelled / total) * 100

                Label3.Text = $"Awaiting Confirmation ({pendingPercent:N1}%)"
                ' Update label text to reflect inclusion of completed
                Label5.Text = $"Confirmed & Completed ({successPercent:N1}%)"
                Label7.Text = $"Cancellations ({cancelledPercent:N1}%)"
            Else
                Label3.Text = "Awaiting Confirmation (0%)"
                Label5.Text = "Confirmed & Completed (0%)"
                Label7.Text = "Cancellations (0%)"
            End If

        Catch ex As Exception
            MessageBox.Show($"Error updating statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================================
    ' UPDATE PIE CHART
    ' =======================================================================
    Private Sub UpdateChart()
        Try
            Chart1.Series("ReservationStatus").Points.Clear()

            If filterPeriod = "Yearly" Then
                ' Change to Column chart for trend
                Chart1.Series("ReservationStatus").ChartType = SeriesChartType.Column
                Chart1.Series("ReservationStatus")("PieLabelStyle") = Nothing
                Chart1.ChartAreas(0).Area3DStyle.Enable3D = False
                
                For Each kvp In reservationData
                    Dim point = Chart1.Series("ReservationStatus").Points.AddXY(kvp.Key, kvp.Value)
                    Chart1.Series("ReservationStatus").Points(point).Color = Color.FromArgb(59, 130, 246) ' Blue
                Next
                Return
            End If

            ' Restore Pie chart for non-yearly
            Chart1.Series("ReservationStatus").ChartType = SeriesChartType.Pie
            Chart1.Series("ReservationStatus")("PieLabelStyle") = "Inside"
            Chart1.ChartAreas(0).Area3DStyle.Enable3D = True
            
            Dim pending As Integer = If(reservationData.ContainsKey("Pending"), reservationData("Pending"), 0)
            Dim confirmed As Integer = reservationData("Confirmed")
            Dim completed As Integer = If(reservationData.ContainsKey("Completed"), reservationData("Completed"), 0)
            Dim cancelled As Integer = reservationData("Cancelled")

            ' Only add points if there's data
            If pending > 0 Then
                Dim point1 As New DataPoint(0, pending)
                point1.AxisLabel = "Pending"
                point1.Label = $"Pending ({pending})"
                point1.LegendText = $"Pending ({pending})"
                point1.Color = Color.FromArgb(245, 158, 11) ' Orange
                point1.LabelForeColor = Color.Black
                Chart1.Series("ReservationStatus").Points.Add(point1)
            End If

            If confirmed > 0 Then
                Dim point2 As New DataPoint(0, confirmed)
                point2.AxisLabel = "Confirmed"
                point2.Label = $"Confirmed ({confirmed})"
                point2.LegendText = $"Confirmed ({confirmed})"
                point2.Color = Color.FromArgb(34, 197, 94) ' Green
                point2.LabelForeColor = Color.Black
                Chart1.Series("ReservationStatus").Points.Add(point2)
            End If

            If completed > 0 Then
                Dim point4 As New DataPoint(0, completed)
                point4.AxisLabel = "Completed"
                point4.Label = $"Completed ({completed})"
                point4.LegendText = $"Completed ({completed})"
                point4.Color = Color.FromArgb(59, 130, 246) ' Blue
                point4.LabelForeColor = Color.Black
                Chart1.Series("ReservationStatus").Points.Add(point4)
            End If

            If cancelled > 0 Then
                Dim point3 As New DataPoint(0, cancelled)
                point3.AxisLabel = "Cancelled"
                point3.Label = $"Cancelled ({cancelled})"
                point3.LegendText = $"Cancelled ({cancelled})"
                point3.Color = Color.FromArgb(239, 68, 68) ' Red
                point3.LabelForeColor = Color.Black
                Chart1.Series("ReservationStatus").Points.Add(point3)
            End If

            ' Show message if no data
            If pending = 0 AndAlso confirmed = 0 AndAlso cancelled = 0 AndAlso completed = 0 Then
                Dim emptyPoint As New DataPoint(0, 1)
                emptyPoint.AxisLabel = "No Data"
                emptyPoint.Label = "No Reservations"
                emptyPoint.Color = Color.LightGray
                Chart1.Series("ReservationStatus").Points.Add(emptyPoint)
            End If

        Catch ex As Exception
            MessageBox.Show($"Error updating chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ' =======================================================================
    ' GET DETAILED RESERVATION STATISTICS
    ' =======================================================================
    Public Function GetDetailedStatistics() As Dictionary(Of String, Object)
        Dim stats As New Dictionary(Of String, Object)

        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then
                openConn()
            End If

            Dim dateFilter As String = GetDateFilter()

            ' Get reservation statistics
            Dim sql As String = $"
                SELECT 
                    COUNT(*) AS TotalReservations,
                    COUNT(CASE WHEN ReservationStatus = 'Pending' THEN 1 END) AS Pending,
                    COUNT(CASE WHEN ReservationStatus = 'Confirmed' THEN 1 END) AS Confirmed,
                    COUNT(CASE WHEN ReservationStatus = 'Cancelled' THEN 1 END) AS Cancelled,
                    COUNT(CASE WHEN ReservationStatus = 'Completed' THEN 1 END) AS Completed,
                    MIN(EventDate) AS FirstReservation,
                    MAX(EventDate) AS LastReservation
                FROM reservations
                WHERE {dateFilter}

            "

            Using cmd As New MySqlCommand(sql, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        stats("Total") = Convert.ToInt32(reader("TotalReservations"))
                        stats("Pending") = Convert.ToInt32(reader("Pending"))
                        stats("Confirmed") = Convert.ToInt32(reader("Confirmed"))
                        stats("Cancelled") = Convert.ToInt32(reader("Cancelled"))
                        stats("Completed") = If(reader("Completed") IsNot DBNull.Value, Convert.ToInt32(reader("Completed")), 0)
                        stats("FirstDate") = If(reader("FirstReservation") IsNot DBNull.Value, Convert.ToDateTime(reader("FirstReservation")), DateTime.MinValue)
                        stats("LastDate") = If(reader("LastReservation") IsNot DBNull.Value, Convert.ToDateTime(reader("LastReservation")), DateTime.MinValue)
                    End If
                End Using
            End Using

            ' Get most popular reservation times
            Dim sqlTimes As String = $"
                SELECT 
                    HOUR(EventTime) AS ReservationHour,
                    COUNT(*) AS HourCount
                FROM reservations
                WHERE {dateFilter}
                GROUP BY HOUR(EventTime)
                ORDER BY HourCount DESC
                LIMIT 3
            "


            Dim popularTimes As New List(Of (Hour As Integer, Count As Integer))
            Using cmd As New MySqlCommand(sqlTimes, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        popularTimes.Add((Convert.ToInt32(reader("ReservationHour")), Convert.ToInt32(reader("HourCount"))))
                    End While
                End Using
            End Using
            stats("PopularTimes") = popularTimes

            ' Calculate conversion rate (Confirmed / Total)
            Dim total As Integer = Convert.ToInt32(stats("Total"))
            Dim confirmed As Integer = Convert.ToInt32(stats("Confirmed"))
            stats("ConversionRate") = If(total > 0, (confirmed / total) * 100, 0)

            ' Calculate cancellation rate
            Dim cancelled As Integer = Convert.ToInt32(stats("Cancelled"))
            stats("CancellationRate") = If(total > 0, (cancelled / total) * 100, 0)

        Catch ex As Exception
            MessageBox.Show($"Error getting detailed statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return stats
    End Function

    ' =======================================================================
    ' GENERATE DETAILED REPORT
    ' =======================================================================
    Public Function GenerateReport() As String
        Dim report As New Text.StringBuilder()
        Dim stats = GetDetailedStatistics()

        report.AppendLine("═══════════════════════════════════════════════════════")
        report.AppendLine($"       RESERVATION STATUS REPORT - {filterPeriod}")
        report.AppendLine("═══════════════════════════════════════════════════════")
        report.AppendLine()

        ' Summary
        report.AppendLine("SUMMARY:")
        report.AppendLine($"  Period:            {filterPeriod}")
        report.AppendLine($"  Total Reservations: {stats("Total")}")
        report.AppendLine($"  Conversion Rate:    {stats("ConversionRate"):N2}%")
        report.AppendLine($"  Cancellation Rate:  {stats("CancellationRate"):N2}%")
        report.AppendLine()

        ' Status Breakdown
        report.AppendLine("STATUS BREAKDOWN:")
        report.AppendLine($"  Pending:    {stats("Pending"),5} ({If(stats("Total") > 0, (stats("Pending") / stats("Total")) * 100, 0):N1}%)")
        report.AppendLine($"  Confirmed:  {stats("Confirmed"),5} ({If(stats("Total") > 0, (stats("Confirmed") / stats("Total")) * 100, 0):N1}%)")
        report.AppendLine($"  Cancelled:  {stats("Cancelled"),5} ({If(stats("Total") > 0, (stats("Cancelled") / stats("Total")) * 100, 0):N1}%)")
        If stats.ContainsKey("Completed") Then
            report.AppendLine($"  Completed:  {stats("Completed"),5} ({If(stats("Total") > 0, (stats("Completed") / stats("Total")) * 100, 0):N1}%)")
        End If
        report.AppendLine()

        ' Popular Times
        If stats.ContainsKey("PopularTimes") Then
            Dim times = DirectCast(stats("PopularTimes"), List(Of (Hour As Integer, Count As Integer)))
            If times.Count > 0 Then
                report.AppendLine("MOST POPULAR RESERVATION TIMES:")
                For i As Integer = 0 To Math.Min(2, times.Count - 1)
                    Dim timeStr As String = $"{times(i).Hour:D2}:00 - {times(i).Hour + 1:D2}:00"
                    report.AppendLine($"  {i + 1}. {timeStr,-15} {times(i).Count} reservations")
                Next
                report.AppendLine()
            End If
        End If

        ' Date Range
        If stats("FirstDate") IsNot Nothing AndAlso stats("FirstDate") <> DateTime.MinValue Then
            report.AppendLine("DATE RANGE:")
            report.AppendLine($"  First Reservation: {stats("FirstDate"):yyyy-MM-dd}")
            report.AppendLine($"  Last Reservation:  {stats("LastDate"):yyyy-MM-dd}")
        End If

        report.AppendLine("═══════════════════════════════════════════════════════")

        Return report.ToString()
    End Function

    ' =======================================================================
    ' REFRESH DATA (Called by Reports form)
    ' =======================================================================
    Public Sub RefreshData()
        filterPeriod = Reports.SelectedPeriod
        currentYear = Reports.SelectedYear
        currentMonth = Reports.SelectedMonth
        
        'ConfigureDateFilter()
        LoadReservationData()
    End Sub

    Private Sub ConfigureDateFilter()
        ' Logic handled in Reports.vb
    End Sub




    ' =======================================================================
    ' SET CUSTOM DATE RANGE
    ' =======================================================================
    Public Sub SetDateRange(startDate As DateTime, endDate As DateTime)
        ' This can be enhanced to support custom date ranges
        currentYear = startDate.Year
        currentMonth = startDate.Month
        LoadReservationData()
    End Sub

    ' =======================================================================
    ' CLEANUP
    ' =======================================================================
    Private Sub FormReservationStatus_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
            conn.Close()
        End If
    End Sub

    Private Sub RoundedPane25_Paint(sender As Object, e As PaintEventArgs) Handles RoundedPane25.Paint

    End Sub
End Class