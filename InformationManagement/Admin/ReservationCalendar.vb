Imports MySqlConnector
Imports System.Data

Public Class ReservationCalendar
    Private currentMonth As Date
    Private reservationData As List(Of ReservationInfo)

    ' Class to hold reservation info
    Private Class ReservationInfo
        Public Property ReservationID As Integer
        Public Property CustomerName As String
        Public Property EventType As String
        Public Property EventDate As Date
        Public Property EventTime As String
        Public Property NumberOfGuests As Integer
        Public Property ContactNumber As String
        Public Property SpecialRequests As String
        Public Property ReservationStatus As String
    End Class

    Private Sub ReservationCalendar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        currentMonth = New Date(Date.Now.Year, Date.Now.Month, 1)
        LoadReservations()
        DisplayCalendar()
    End Sub

    ' ==========================================
    ' LOAD CONFIRMED AND COMPLETED RESERVATIONS
    ' ==========================================
    Private Sub LoadReservations()
        Try
            reservationData = New List(Of ReservationInfo)

            ' Get start and end of current month
            Dim monthStart As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
            Dim monthEnd As Date = monthStart.AddMonths(1).AddDays(-1)

            Dim query As String =
                "SELECT 
                    r.ReservationID,
                    COALESCE(r.FullName, CONCAT(COALESCE(c.FirstName, ''), ' ', COALESCE(c.LastName, ''))) AS CustomerName,
                    r.EventType,
                    r.EventDate,
                    r.EventTime,
                    r.NumberOfGuests,
                    COALESCE(r.ContactNumber, c.ContactNumber) AS ContactNumber,
                    r.SpecialRequests,
                    r.ReservationStatus
                 FROM reservations r
                 LEFT JOIN customers c ON r.CustomerID = c.CustomerID
                 WHERE r.ReservationStatus IN ('Confirmed', 'Completed')
                 AND r.EventDate >= @startDate
                 AND r.EventDate <= @endDate
                 ORDER BY r.EventDate, r.EventTime"

            openConn()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@startDate", monthStart.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@endDate", monthEnd.ToString("yyyy-MM-dd"))

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim resInfo As New ReservationInfo With {
                            .ReservationID = Convert.ToInt32(reader("ReservationID")),
                            .CustomerName = If(reader("CustomerName").ToString().Trim() = "", "Walk-in Customer", reader("CustomerName").ToString()),
                            .EventType = reader("EventType").ToString(),
                            .EventDate = Convert.ToDateTime(reader("EventDate")),
                            .EventTime = reader("EventTime").ToString(),
                            .NumberOfGuests = Convert.ToInt32(reader("NumberOfGuests")),
                            .ContactNumber = If(IsDBNull(reader("ContactNumber")), "N/A", reader("ContactNumber").ToString()),
                            .SpecialRequests = If(IsDBNull(reader("SpecialRequests")), "None", reader("SpecialRequests").ToString()),
                            .ReservationStatus = reader("ReservationStatus").ToString()
                        }
                        reservationData.Add(resInfo)
                    End While
                End Using
            End Using
            closeConn()

        Catch ex As Exception
            MessageBox.Show("Error loading reservations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            closeConn()
        End Try
    End Sub

    ' ==========================================
    ' DISPLAY CALENDAR
    ' ==========================================
    Private Sub DisplayCalendar()
        Try
            ' Clear existing controls
            pnlCalendar.Controls.Clear()

            ' Update month label
            lblMonth.Text = currentMonth.ToString("MMMM yyyy")

            ' Get first day of month and number of days
            Dim firstDay As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
            Dim daysInMonth As Integer = Date.DaysInMonth(currentMonth.Year, currentMonth.Month)
            Dim startDayOfWeek As Integer = firstDay.DayOfWeek ' 0 = Sunday

            ' CALCULATE CELL SIZE BASED ON PANEL WIDTH
            Dim panelWidth As Integer = pnlCalendar.Width - 20
            Dim cellWidth As Integer = CInt(panelWidth / 7)
            Dim cellHeight As Integer = 100

            Dim currentRow As Integer = 0
            Dim currentCol As Integer = startDayOfWeek

            ' Calculate total rows needed
            Dim totalCells As Integer = startDayOfWeek + daysInMonth
            Dim totalRows As Integer = CInt(Math.Ceiling(totalCells / 7))

            ' Set panel height
            pnlCalendar.AutoScrollMinSize = New Size(panelWidth, totalRows * cellHeight)

            ' Create calendar cells
            For day As Integer = 1 To daysInMonth
                Dim currentDate As Date = New Date(currentMonth.Year, currentMonth.Month, day)

                ' Create day panel
                Dim dayPanel As New Panel()
                dayPanel.Size = New Size(cellWidth - 2, cellHeight - 2)
                dayPanel.Location = New Point(currentCol * cellWidth, currentRow * cellHeight)
                dayPanel.BorderStyle = BorderStyle.FixedSingle
                dayPanel.BackColor = Color.White
                dayPanel.Tag = currentDate ' Store date in tag

                ' Check if date is in the past
                Dim isPastDate As Boolean = currentDate.Date < Date.Today

                If isPastDate Then
                    ' Gray out past dates
                    dayPanel.BackColor = Color.FromArgb(240, 240, 240)
                    dayPanel.Cursor = Cursors.No
                Else
                    ' Highlight today
                    If currentDate.Date = Date.Today Then
                        dayPanel.BackColor = Color.FromArgb(255, 248, 220)
                    End If
                    dayPanel.Cursor = Cursors.Hand
                End If

                ' Day number label
                Dim lblDay As New Label()
                lblDay.Text = day.ToString()
                lblDay.Location = New Point(5, 5)
                lblDay.Size = New Size(40, 25)
                lblDay.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                lblDay.ForeColor = If(isPastDate, Color.Gray, Color.Black)
                dayPanel.Controls.Add(lblDay)

                ' Get reservations for this day
                Dim dayReservations = reservationData.Where(Function(r) r.EventDate.Date = currentDate.Date).ToList()

                If dayReservations.Count > 0 Then
                    ' Check if there are any confirmed reservations (clickable)
                    Dim confirmedCount = dayReservations.Where(Function(r) r.ReservationStatus = "Confirmed").Count()
                    Dim completedCount = dayReservations.Where(Function(r) r.ReservationStatus = "Completed").Count()

                    ' Set background color based on reservation status
                    If confirmedCount > 0 Then
                        ' Light green background for dates with confirmed reservations
                        If isPastDate Then
                            dayPanel.BackColor = Color.FromArgb(220, 237, 220) ' Muted green for past confirmed
                        Else
                            dayPanel.BackColor = Color.FromArgb(232, 245, 233) ' Light green for future confirmed
                        End If
                    ElseIf completedCount > 0 Then
                        ' Light gray for completed only
                        dayPanel.BackColor = Color.FromArgb(245, 245, 245)
                    End If

                    ' Reservation count indicator (center of cell)
                    Dim lblIndicator As New Label()
                    Dim statusText As String = ""
                    
                    If confirmedCount > 0 And completedCount > 0 Then
                        statusText = $"📅 {confirmedCount} Confirmed" & vbCrLf & $"✓ {completedCount} Completed"
                    ElseIf confirmedCount > 0 Then
                        statusText = $"📅 {confirmedCount}" & vbCrLf & "Reservation" & If(confirmedCount > 1, "s", "")
                    Else
                        statusText = $"✓ {completedCount}" & vbCrLf & "Completed"
                    End If
                    
                    lblIndicator.Text = statusText
                    lblIndicator.Location = New Point(10, 35)
                    lblIndicator.Size = New Size(cellWidth - 20, 50)
                    lblIndicator.TextAlign = ContentAlignment.MiddleCenter
                    lblIndicator.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    
                    ' Color based on status: Green for confirmed, Gray for completed only
                    If isPastDate Then
                        lblIndicator.ForeColor = Color.Gray
                    ElseIf confirmedCount > 0 Then
                        lblIndicator.ForeColor = Color.FromArgb(40, 167, 69) ' Green for confirmed
                    Else
                        lblIndicator.ForeColor = Color.FromArgb(108, 117, 125) ' Gray for completed only
                    End If
                    
                    lblIndicator.BackColor = Color.Transparent
                    dayPanel.Controls.Add(lblIndicator)

                    ' Make clickable if has confirmed reservations (regardless of date)
                    ' Only completed reservations are view-only (non-clickable)
                    If confirmedCount > 0 Then
                        ' Has confirmed reservations - always clickable even if past date
                        AddHandler dayPanel.Click, Sub() ShowDayReservations(currentDate)
                        AddHandler lblDay.Click, Sub() ShowDayReservations(currentDate)
                        AddHandler lblIndicator.Click, Sub() ShowDayReservations(currentDate)
                        dayPanel.Cursor = Cursors.Hand
                    ElseIf completedCount > 0 Then
                        ' Has only completed reservations - view-only (non-clickable)
                        dayPanel.Cursor = Cursors.No
                    End If
                Else
                    ' No reservations message
                    If Not isPastDate Then
                        Dim lblNoRes As New Label()
                        lblNoRes.Text = "No reservations"
                        lblNoRes.Location = New Point(10, 40)
                        lblNoRes.Size = New Size(cellWidth - 20, 40)
                        lblNoRes.TextAlign = ContentAlignment.MiddleCenter
                        lblNoRes.Font = New Font("Segoe UI", 8)
                        lblNoRes.ForeColor = Color.Gray
                        dayPanel.Controls.Add(lblNoRes)
                    End If
                End If

                pnlCalendar.Controls.Add(dayPanel)

                ' Move to next position
                currentCol += 1
                If currentCol > 6 Then
                    currentCol = 0
                    currentRow += 1
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Error displaying calendar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ShowDayReservations(selectedDate As Date)
        Try
            ' Get all reservations for this day
            Dim dayReservations = reservationData.Where(Function(r) r.EventDate.Date = selectedDate.Date).OrderBy(Function(r) r.EventTime).ToList()

            If dayReservations.Count = 0 Then
                MessageBox.Show("No reservations found for this date.", "No Reservations", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Create popup form
            Dim popupForm As New Form()
            popupForm.Text = $"Reservations for {selectedDate:MMMM dd, yyyy}"
            popupForm.Size = New Size(800, 550)
            popupForm.StartPosition = FormStartPosition.CenterParent
            popupForm.FormBorderStyle = FormBorderStyle.None ' Remove title bar
            popupForm.MaximizeBox = False
            popupForm.MinimizeBox = False

            ' Add paint handler for border
            AddHandler popupForm.Paint, Sub(sender As Object, e As PaintEventArgs)
                                            Dim borderColor As Color = Color.LightGray
                                            Dim borderThickness As Integer = 1
                                            Dim rect As Rectangle = popupForm.ClientRectangle
                                            rect.Width -= borderThickness
                                            rect.Height -= borderThickness
                                            Using pen As New Pen(borderColor, borderThickness)
                                                e.Graphics.DrawRectangle(pen, rect)
                                            End Using
                                        End Sub

            ' Header panel
            Dim headerPanel As New Panel()
            headerPanel.Dock = DockStyle.Top
            headerPanel.Height = 60
            headerPanel.BackColor = Color.FromArgb(52, 73, 94)

            ' Count confirmed and completed
            Dim confirmedCount = dayReservations.Where(Function(r) r.ReservationStatus = "Confirmed").Count()
            Dim completedCount = dayReservations.Where(Function(r) r.ReservationStatus = "Completed").Count()
            
            Dim lblHeader As New Label()
            Dim headerText As String = $"📅 {selectedDate:dddd, MMMM dd, yyyy}" & vbCrLf
            If confirmedCount > 0 And completedCount > 0 Then
                headerText &= $"{confirmedCount} Confirmed, {completedCount} Completed"
            ElseIf confirmedCount > 0 Then
                headerText &= $"{confirmedCount} Confirmed Reservation" & If(confirmedCount > 1, "s", "")
            Else
                headerText &= $"{completedCount} Completed Reservation" & If(completedCount > 1, "s", "")
            End If
            lblHeader.Text = headerText
            lblHeader.Dock = DockStyle.Fill
            lblHeader.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblHeader.ForeColor = Color.White
            lblHeader.TextAlign = ContentAlignment.MiddleCenter
            headerPanel.Controls.Add(lblHeader)

            ' Scrollable panel replaced with FlowLayoutPanel for auto-fit grid
            Dim flowPanel As New FlowLayoutPanel()
            flowPanel.Dock = DockStyle.Fill
            flowPanel.AutoScroll = True
            ' Add enough top padding to clear the Header (60px) + Extra spacing
            ' Add bottom padding to clear the Footer (60px)
            flowPanel.Padding = New Padding(20, 80, 20, 80)
            flowPanel.BackColor = Color.White
            flowPanel.WrapContents = True
            flowPanel.FlowDirection = FlowDirection.LeftToRight

            For Each res In dayReservations
                ' Determine if this reservation is completed
                Dim isCompleted As Boolean = (res.ReservationStatus = "Completed")
                
                ' Reservation card panel - SQUARE STYLE
                Dim cardPanel As New Panel()
                cardPanel.Size = New Size(250, 250) ' Square size
                cardPanel.Margin = New Padding(10)
                cardPanel.BorderStyle = BorderStyle.FixedSingle
                cardPanel.BackColor = If(isCompleted, Color.FromArgb(245, 245, 245), Color.FromArgb(240, 248, 255))
                cardPanel.Padding = New Padding(10)

                ' Time label (large and prominent) - Convert to 12-hour format
                Dim lblTime As New Label()
                Dim timeDisplay As String = res.EventTime.ToString()
                Try
                    ' Try to parse and format to 12-hour
                    Dim timeParsed As DateTime
                    If DateTime.TryParse(res.EventTime, timeParsed) Then
                        timeDisplay = timeParsed.ToString("h:mm tt")
                    End If
                Catch
                    ' If parsing fails, use original
                End Try
                lblTime.Text = timeDisplay
                lblTime.Location = New Point(10, 10)
                lblTime.Size = New Size(100, 30)
                lblTime.Font = New Font("Segoe UI", 14, FontStyle.Bold)
                lblTime.ForeColor = If(isCompleted, Color.FromArgb(108, 117, 125), Color.FromArgb(52, 152, 219))
                cardPanel.Controls.Add(lblTime)
                
                ' Status badge
                If isCompleted Then
                    Dim lblStatus As New Label()
                    lblStatus.Text = "✓ COMPLETED"
                    lblStatus.Location = New Point(120, 10)
                    lblStatus.Size = New Size(100, 20)
                    lblStatus.Font = New Font("Segoe UI", 8, FontStyle.Bold)
                    lblStatus.ForeColor = Color.White
                    lblStatus.BackColor = Color.FromArgb(108, 117, 125)
                    lblStatus.TextAlign = ContentAlignment.MiddleCenter
                    cardPanel.Controls.Add(lblStatus)
                End If

                ' Reservation details
                Dim lblDetails As New Label()
                lblDetails.Text = $"Customer: {res.CustomerName}" & vbCrLf &
                            $"Event: {res.EventType}" & vbCrLf &
                            $"Guests: {res.NumberOfGuests}   |   Contact: {res.ContactNumber}"
                lblDetails.Location = New Point(If(isCompleted, 100, 120), If(isCompleted, 10, 35))
                lblDetails.Size = New Size(520, 60)
                lblDetails.Font = New Font("Segoe UI", 9)
                lblDetails.ForeColor = Color.FromArgb(52, 73, 94)
                cardPanel.Controls.Add(lblDetails)

                ' Special requests (if any)
                If res.SpecialRequests <> "None" AndAlso Not String.IsNullOrWhiteSpace(res.SpecialRequests) Then
                    Dim lblSpecial As New Label()
                    lblSpecial.Text = "📝 " & res.SpecialRequests
                    lblSpecial.Location = New Point(100, 75)
                    lblSpecial.Size = New Size(520, 25)
                    lblSpecial.Font = New Font("Segoe UI", 8, FontStyle.Italic)
                    lblSpecial.ForeColor = Color.FromArgb(149, 165, 166)
                    cardPanel.Controls.Add(lblSpecial)
                End If

                ' View details button (only for confirmed reservations)
                If Not isCompleted Then
                    Dim btnViewDetails As New Button()
                    btnViewDetails.Text = "View Details"
                    btnViewDetails.Location = New Point(10, 75)
                    btnViewDetails.Size = New Size(80, 25)
                    btnViewDetails.BackColor = Color.FromArgb(52, 152, 219)
                    btnViewDetails.ForeColor = Color.White
                    btnViewDetails.FlatStyle = FlatStyle.Flat
                    btnViewDetails.FlatAppearance.BorderSize = 0
                    btnViewDetails.Font = New Font("Segoe UI", 8)
                    btnViewDetails.Cursor = Cursors.Hand

                    Dim resId As Integer = res.ReservationID
                    AddHandler btnViewDetails.Click, Sub()
                                                         ShowReservationDetails(resId)
                                                     End Sub
                    cardPanel.Controls.Add(btnViewDetails)
                End If

                flowPanel.Controls.Add(cardPanel)
            Next

            ' Close button panel (Footer) for margin/spacing
            Dim footerPanel As New Panel()
            footerPanel.Dock = DockStyle.Bottom
            footerPanel.Height = 60
            footerPanel.BackColor = Color.Transparent ' Or match form background
            footerPanel.Padding = New Padding(10)

            Dim btnClose As New Button()
            btnClose.Text = "Close"
            btnClose.Dock = DockStyle.Fill
            btnClose.BackColor = Color.FromArgb(108, 117, 125)
            btnClose.ForeColor = Color.White
            btnClose.FlatStyle = FlatStyle.Flat
            btnClose.FlatAppearance.BorderSize = 0
            btnClose.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            btnClose.Cursor = Cursors.Hand
            AddHandler btnClose.Click, Sub() popupForm.Close()

            footerPanel.Controls.Add(btnClose)

            ' Add controls to form (explicit Z-order: Edge first, Fill last)
            popupForm.Controls.Add(footerPanel) ' Dock Bottom
            popupForm.Controls.Add(headerPanel) ' Dock Top
            popupForm.Controls.Add(flowPanel)   ' Dock Fill
            
            ' Explicitly set Z-Order
            footerPanel.BringToFront()
            headerPanel.BringToFront()
            flowPanel.SendToBack()


            ' Show popup
            popupForm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Error showing reservations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ' ==========================================
    ' SHOW RESERVATION DETAILS
    ' ==========================================
    Private Sub ShowReservationDetails(reservationID As Integer)
        Try
            Dim res = reservationData.FirstOrDefault(Function(r) r.ReservationID = reservationID)

            If res IsNot Nothing Then
                ' Format time to 12-hour
                Dim timeDisplay As String = res.EventTime.ToString()
                Try
                    Dim timeParsed As DateTime
                    If DateTime.TryParse(res.EventTime, timeParsed) Then
                        timeDisplay = timeParsed.ToString("h:mm tt")
                    End If
                Catch
                    ' If parsing fails, use original
                End Try
                
                ' Dynamic status display
                Dim statusText As String = If(res.ReservationStatus = "Completed", "COMPLETED ✓", "CONFIRMED ✓")
                
                Dim details As String =
                $"═══════════════════════════════════════" & vbCrLf &
                $"         RESERVATION DETAILS" & vbCrLf &
                $"═══════════════════════════════════════" & vbCrLf & vbCrLf &
                $"Status: {statusText}" & vbCrLf & vbCrLf &
                $"Customer Information:" & vbCrLf &
                $"  • Name: {res.CustomerName}" & vbCrLf &
                $"  • Contact: {res.ContactNumber}" & vbCrLf & vbCrLf &
                $"Event Details:" & vbCrLf &
                $"  • Type: {res.EventType}" & vbCrLf &
                $"  • Date: {res.EventDate:MMMM dd, yyyy (dddd)}" & vbCrLf &
                $"  • Time: {timeDisplay}" & vbCrLf &
                $"  • Number of Guests: {res.NumberOfGuests}" & vbCrLf & vbCrLf &
                $"Special Requests:" & vbCrLf &
                $"  {If(res.SpecialRequests = "None", "No special requests", res.SpecialRequests)}" & vbCrLf & vbCrLf &
                $"═══════════════════════════════════════"

                MessageBox.Show(details, "Reservation Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error showing details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ============================================================
    ' ADD RESIZE EVENT HANDLER
    ' ============================================================

    Private Sub OrderCalendar_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' Redraw calendar when form is resized
        If reservationData IsNot Nothing Then
            DisplayCalendar()
        End If
    End Sub

    ' Close button
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' Paint light gray border
    Private Sub ReservationCalendar_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim borderColor As Color = Color.LightGray
        Dim borderThickness As Integer = 1
        Dim rect As Rectangle = Me.ClientRectangle

        ' Adjust rectangle for border thickness
        rect.Width -= borderThickness
        rect.Height -= borderThickness

        Using pen As New Pen(borderColor, borderThickness)
            e.Graphics.DrawRectangle(pen, rect)
        End Using
    End Sub

    ' ==========================================
    ' NAVIGATION BUTTONS
    ' ==========================================
    Private Sub btnPrevMonth_Click(sender As Object, e As EventArgs) Handles btnPrevMonth.Click
        currentMonth = currentMonth.AddMonths(-1)
        LoadReservations()
        DisplayCalendar()
    End Sub

    Private Sub btnNextMonth_Click(sender As Object, e As EventArgs) Handles btnNextMonth.Click
        currentMonth = currentMonth.AddMonths(1)
        LoadReservations()
        DisplayCalendar()
    End Sub

    Private Sub btnToday_Click(sender As Object, e As EventArgs) Handles btnToday.Click
        currentMonth = New Date(Date.Now.Year, Date.Now.Month, 1)
        LoadReservations()
        DisplayCalendar()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadReservations()
        DisplayCalendar()
        MessageBox.Show("Calendar refreshed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class