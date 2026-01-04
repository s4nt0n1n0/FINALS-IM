Imports System.Windows.Forms.DataVisualization.Charting
Imports MySqlConnector

Public Class FormReservationStatus

    Private currentYear As Integer = DateTime.Now.Year
    Private currentMonth As Integer = DateTime.Now.Month
    Private filterPeriod As String = "Monthly" ' Daily, Weekly, Monthly, Yearly
    Private reservationData As New Dictionary(Of String, Integer)
    Private isInitializing As Boolean = True


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

            ' Configure chart colors
            Chart1.BackColor = Color.White
            If Chart1.ChartAreas.Count > 0 Then
                Chart1.ChartAreas(0).BackColor = Color.White
            End If

            ' Set label colors
            lblPending.ForeColor = Color.FromArgb(255, 165, 0) ' Orange
            lblConfirmed.ForeColor = Color.FromArgb(34, 197, 94) ' Green
            lblCancelled.ForeColor = Color.FromArgb(239, 68, 68) ' Red

            ' Set initial values to prevent blank display
            lblTotalReservations.Text = "0"
            lblPending.Text = "0"
            lblConfirmed.Text = "0"
            lblCancelled.Text = "0"

        Catch ex As Exception
            MessageBox.Show($"Initialize Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

            Dim dateFilter As String = GetDateFilter()

            ' Get reservation counts by status
            Dim sql As String = $"
                SELECT 
                    ReservationStatus,
                    COUNT(*) AS StatusCount
                FROM reservations
                WHERE {dateFilter}
                GROUP BY ReservationStatus
            "

            Using cmd As New MySqlCommand(sql, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear existing data
                    reservationData.Clear()

                    ' Initialize with zeros
                    reservationData("Pending") = 0
                    reservationData("Confirmed") = 0
                    reservationData("Cancelled") = 0

                    ' Load actual data
                    While reader.Read()
                        Dim status As String = If(IsDBNull(reader("ReservationStatus")), "Unknown", reader("ReservationStatus").ToString())
                        Dim count As Integer = Convert.ToInt32(reader("StatusCount"))

                        If reservationData.ContainsKey(status) Then
                            reservationData(status) = count
                        End If
                    End While
                End Using
            End Using

            ' Update UI with data
            UpdateStatisticsCards()
            UpdateChart()

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
            Case "Daily"
                filter = $"DATE(EventDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}'"

            Case "Weekly"
                filter = $"YEARWEEK(EventDate, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1)"


            Case "Monthly"
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

    ' =======================================================================
    ' UPDATE STATISTICS CARDS
    ' =======================================================================
    Private Sub UpdateStatisticsCards()
        Try
            Dim total As Integer = reservationData.Values.Sum()
            Dim pending As Integer = reservationData("Pending")
            Dim confirmed As Integer = reservationData("Confirmed")
            Dim cancelled As Integer = reservationData("Cancelled")

            ' Update labels
            lblTotalReservations.Text = total.ToString()
            lblPending.Text = pending.ToString()
            lblConfirmed.Text = confirmed.ToString()
            lblCancelled.Text = cancelled.ToString()

            ' Calculate and show percentages
            If total > 0 Then
                Dim pendingPercent As Decimal = (pending / total) * 100
                Dim confirmedPercent As Decimal = (confirmed / total) * 100
                Dim cancelledPercent As Decimal = (cancelled / total) * 100

                Label3.Text = $"Awaiting Confirmation ({pendingPercent:N1}%)"
                Label5.Text = $"Ready to serve ({confirmedPercent:N1}%)"
                Label7.Text = $"Cancellations ({cancelledPercent:N1}%)"
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

            Dim pending As Integer = reservationData("Pending")
            Dim confirmed As Integer = reservationData("Confirmed")
            Dim cancelled As Integer = reservationData("Cancelled")

            ' Only add points if there's data
            If pending > 0 Then
                Dim point1 As New DataPoint(0, pending)
                point1.AxisLabel = "Pending"
                point1.Label = $"Pending ({pending})"
                point1.LegendText = $"Pending ({pending})"
                point1.Color = Color.FromArgb(245, 158, 11) ' Orange
                point1.LabelForeColor = Color.White
                Chart1.Series("ReservationStatus").Points.Add(point1)
            End If

            If confirmed > 0 Then
                Dim point2 As New DataPoint(0, confirmed)
                point2.AxisLabel = "Confirmed"
                point2.Label = $"Confirmed ({confirmed})"
                point2.LegendText = $"Confirmed ({confirmed})"
                point2.Color = Color.FromArgb(34, 197, 94) ' Green
                point2.LabelForeColor = Color.White
                Chart1.Series("ReservationStatus").Points.Add(point2)
            End If

            If cancelled > 0 Then
                Dim point3 As New DataPoint(0, cancelled)
                point3.AxisLabel = "Cancelled"
                point3.Label = $"Cancelled ({cancelled})"
                point3.LegendText = $"Cancelled ({cancelled})"
                point3.Color = Color.FromArgb(239, 68, 68) ' Red
                point3.LabelForeColor = Color.White
                Chart1.Series("ReservationStatus").Points.Add(point3)
            End If

            ' Show message if no data
            If pending = 0 AndAlso confirmed = 0 AndAlso cancelled = 0 Then
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