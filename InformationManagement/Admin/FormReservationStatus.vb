Imports System.Windows.Forms.DataVisualization.Charting
Imports MySqlConnector

Public Class FormReservationStatus

    Private currentYear As Integer = DateTime.Now.Year
    Private currentMonth As Integer = DateTime.Now.Month
    Private filterPeriod As String = "Monthly" ' Daily, Weekly, Monthly, Yearly
    Private reservationData As New Dictionary(Of String, Integer)
    Private isInitializing As Boolean = True
    Private _lastSearchText As String = ""


    ' =======================================================================
    ' FORM LOAD
    ' =======================================================================
    Private Sub FormReservationStatus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            InitializeForm()
            InitializeDetailsGrid()
            ConfigureDateFilter()
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
            LoadDetailedReservations()

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
                filter = $"DATE(EventDate) = '{dtpFilter.Value:yyyy-MM-dd}'"

            Case "Weekly"
                filter = $"YEARWEEK(EventDate, 1) = YEARWEEK('{dtpFilter.Value:yyyy-MM-dd}', 1)"


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

    ' =======================================================================
    ' EXPORT PDF
    ' =======================================================================
    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        If Reports.Instance IsNot Nothing Then
            Reports.Instance.ExportCurrentReport()
        Else
            MessageBox.Show("Please open the Reports screen to export.", "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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

        ConfigureDateFilter()
        LoadReservationData()
    End Sub

    Private Sub ConfigureDateFilter()
        If dtpFilter Is Nothing Then Return

        Select Case filterPeriod
            Case "Daily", "Weekly"
                dtpFilter.Visible = True
                dtpFilter.CustomFormat = "MMMM dd, yyyy"
                dtpFilter.Format = DateTimePickerFormat.Custom
            Case Else
                dtpFilter.Visible = False
        End Select
    End Sub

    Private Sub dtpFilter_ValueChanged(sender As Object, e As EventArgs) Handles dtpFilter.ValueChanged
        If isInitializing Then Return
        LoadReservationData()
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

    ' =======================================================================
    ' DETAILS GRID LOGIC
    ' =======================================================================
    Private Sub InitializeDetailsGrid()
        With dgvDetails
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
    End Sub

    Private Sub LoadDetailedReservations(Optional searchText As String = "")
        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then openConn()

            Dim dateFilter = GetDateFilter()
            Dim searchFilter = ""
            If Not String.IsNullOrEmpty(searchText) Then
                searchFilter = $" AND (FullName LIKE @search OR ReservationID LIKE @search OR ContactNumber LIKE @search)"
            End If

            ' SQL to match the requested columns as closely as possible
            Dim sql = $"
                SELECT 
                    CONCAT('RES-', LPAD(ReservationID, 4, '0')) as ID,
                    FullName as Customer,
                    ContactNumber,
                    EventDate,
                    TIME_FORMAT(EventTime, '%h:%i %p') as `Time`,
                    NumberOfGuests as Guests,
                    'T-' as `Table`, 
                    ReservationStatus as Status,
                    ReservationDate as Created,
                    UpdatedDate as Confirmed,
                    NULL as Seated
                FROM reservations
                WHERE {dateFilter} {searchFilter}
                ORDER BY ReservationID DESC
            "

            Dim dt As New DataTable()
            Using cmd As New MySqlCommand(sql, conn)
                If Not String.IsNullOrEmpty(searchText) Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using
            End Using

            dgvDetails.DataSource = dt
            ConfigureDetailsColumns()

        Catch ex As Exception
            ' Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub ConfigureDetailsColumns()
        Try
            With dgvDetails
                ' Column Visibility & Formatting
                If .Columns.Contains("ContactNumber") Then .Columns("ContactNumber").Visible = False

                ' ID Column - Blue Text
                If .Columns.Contains("ID") Then
                    .Columns("ID").HeaderText = "ID"
                    .Columns("ID").Width = 80
                    .Columns("ID").DefaultCellStyle.ForeColor = Color.FromArgb(59, 130, 246)
                    .Columns("ID").DefaultCellStyle.Font = New Font("Segoe UI Semibold", 9.0!)
                End If

                ' Customer Column - Bold
                If .Columns.Contains("Customer") Then
                    .Columns("Customer").HeaderText = "Customer"
                    .Columns("Customer").Width = 180
                    .Columns("Customer").DefaultCellStyle.Font = New Font("Segoe UI Bold", 9.5!)
                End If

                ' Date/Time
                If .Columns.Contains("EventDate") Then
                    .Columns("EventDate").HeaderText = "Date/Time"
                    .Columns("EventDate").Width = 120
                    .Columns("EventDate").DefaultCellStyle.Format = "yyyy-MM-dd"
                End If

                If .Columns.Contains("Guests") Then
                    .Columns("Guests").Width = 80
                    .Columns("Guests").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If

                If .Columns.Contains("Table") Then
                    .Columns("Table").Width = 80
                End If

                If .Columns.Contains("Status") Then
                    .Columns("Status").Width = 100
                End If

                ' Created/Confirmed Dates
                If .Columns.Contains("Created") Then
                    .Columns("Created").DefaultCellStyle.Format = "MMM d, h:mm tt"
                    .Columns("Created").Width = 130
                End If

                If .Columns.Contains("Confirmed") Then
                    .Columns("Confirmed").DefaultCellStyle.Format = "MMM d, h:mm tt"
                    .Columns("Confirmed").Width = 130
                End If

                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End With
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitializing Then Return

        Dim currentSearch = TextBoxSearch.Text.Trim()
        If currentSearch = "Search reservation..." Then currentSearch = ""

        ' Only refresh if the actual search criteria changed
        If currentSearch = _lastSearchText Then Return

        _lastSearchText = currentSearch
        LoadDetailedReservations(currentSearch)
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search orders..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42)
            searchTextBox1.BorderColor = Color.FromArgb(99, 102, 241)
        End If
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search orders..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
            searchTextBox1.BorderColor = Color.FromArgb(226, 232, 240)
        End If
    End Sub

    Private Sub dgvDetails_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetails.CellFormatting
        If dgvDetails.Columns(e.ColumnIndex).Name = "Status" AndAlso e.Value IsNot Nothing Then
            Dim status = e.Value.ToString().Trim()
            Select Case status
                Case "Confirmed"
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74) ' Green
                Case "Pending"
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6) ' Amber
                Case "Cancelled"
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38) ' Red
                Case "Completed"
                    e.CellStyle.ForeColor = Color.FromArgb(71, 85, 105) ' Slate
                Case "Seated"
                    e.CellStyle.ForeColor = Color.FromArgb(99, 102, 241) ' Indigo
                Case "No-Show"
                    e.CellStyle.ForeColor = Color.FromArgb(249, 115, 22) ' Orange/Red
            End Select
            e.CellStyle.Font = New Font("Segoe UI Semibold", 9.0!)
        End If
    End Sub

    Private Sub RoundedPane25_Paint(sender As Object, e As PaintEventArgs)

    End Sub
End Class