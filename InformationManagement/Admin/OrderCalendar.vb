Imports MySqlConnector
Imports System.Data

Public Class OrderCalendar
    Private currentMonth As Date
    Private orderData As List(Of OrderInfo)

    ' Class to hold order info
    Private Class OrderInfo
        Public Property OrderID As Integer
        Public Property CustomerName As String
        Public Property OrderType As String
        Public Property OrderDate As Date
        Public Property OrderTime As String
        Public Property TotalAmount As Decimal
        Public Property ItemsOrderedCount As Integer
        Public Property OrderedProducts As String
        Public Property ContactNumber As String
        Public Property SpecialRequests As String
        Public Property DeliveryAddress As String
        Public Property DeliveryOption As String
        Public Property OrderStatus As String
    End Class

    Private Sub OrderCalendar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        currentMonth = New Date(Date.Now.Year, Date.Now.Month, 1)
        LoadOrders()
        DisplayCalendar()
    End Sub

    ' ==========================================
    ' LOAD CONFIRMED AND COMPLETED ORDERS
    ' ==========================================
    Private Sub LoadOrders()
        Try
            orderData = New List(Of OrderInfo)

            ' Get start and end of current month
            Dim monthStart As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
            Dim monthEnd As Date = monthStart.AddMonths(1).AddDays(-1)

            Dim query As String =
                "SELECT 
                    o.OrderID,
                    CONCAT(COALESCE(c.FirstName, ''), ' ', COALESCE(c.LastName, '')) AS CustomerName,
                    o.OrderType,
                    o.OrderDate,
                    o.OrderTime,
                    o.TotalAmount,
                    o.ItemsOrderedCount,
                    COALESCE(c.ContactNumber, '') AS ContactNumber,
                    COALESCE(o.SpecialRequests, '') AS SpecialRequests,
                    COALESCE(o.DeliveryAddress, '') AS DeliveryAddress,
                    COALESCE(o.DeliveryOption, '') AS DeliveryOption,
                    o.OrderStatus,
                    COALESCE(
                        (SELECT GROUP_CONCAT(
                            CONCAT(ProductName, ' (', Quantity, ')') 
                            ORDER BY OrderItemID 
                            SEPARATOR ', '
                        )
                        FROM order_items 
                        WHERE OrderID = o.OrderID
                        LIMIT 1000), 
                        ''
                    ) AS OrderedProducts
                 FROM orders o
                 LEFT JOIN customers c ON o.CustomerID = c.CustomerID
                 WHERE o.OrderStatus IN ('Confirmed', 'Completed')
                 AND o.OrderDate >= @startDate
                 AND o.OrderDate <= @endDate
                 ORDER BY o.OrderDate, o.OrderTime"

            openConn()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@startDate", monthStart.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@endDate", monthEnd.ToString("yyyy-MM-dd"))

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim orderInfo As New OrderInfo With {
                            .OrderID = Convert.ToInt32(reader("OrderID")),
                            .CustomerName = If(reader("CustomerName").ToString().Trim() = "", "Walk-in Customer", reader("CustomerName").ToString()),
                            .OrderType = reader("OrderType").ToString(),
                            .OrderDate = Convert.ToDateTime(reader("OrderDate")),
                            .OrderTime = reader("OrderTime").ToString(),
                            .TotalAmount = Convert.ToDecimal(reader("TotalAmount")),
                            .ItemsOrderedCount = Convert.ToInt32(reader("ItemsOrderedCount")),
                            .OrderedProducts = If(IsDBNull(reader("OrderedProducts")), "N/A", reader("OrderedProducts").ToString()),
                            .ContactNumber = If(IsDBNull(reader("ContactNumber")), "N/A", reader("ContactNumber").ToString()),
                            .SpecialRequests = If(IsDBNull(reader("SpecialRequests")) OrElse String.IsNullOrWhiteSpace(reader("SpecialRequests").ToString()), "None", reader("SpecialRequests").ToString()),
                            .DeliveryAddress = If(IsDBNull(reader("DeliveryAddress")), "N/A", reader("DeliveryAddress").ToString()),
                            .DeliveryOption = If(IsDBNull(reader("DeliveryOption")), "N/A", reader("DeliveryOption").ToString()),
                            .OrderStatus = reader("OrderStatus").ToString()
                        }
                        orderData.Add(orderInfo)
                    End While
                End Using
            End Using
            closeConn()

        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                dayPanel.Tag = currentDate

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

                ' Get orders for this day
                Dim dayOrders = orderData.Where(Function(o) o.OrderDate.Date = currentDate.Date).ToList()

                If dayOrders.Count > 0 Then
                    ' Check if there are any confirmed orders (clickable)
                    Dim confirmedCount = dayOrders.Where(Function(o) o.OrderStatus = "Confirmed").Count()
                    Dim completedCount = dayOrders.Where(Function(o) o.OrderStatus = "Completed").Count()

                    ' Set background color based on order status
                    If confirmedCount > 0 Then
                        ' Light green background for dates with confirmed orders
                        If isPastDate Then
                            dayPanel.BackColor = Color.FromArgb(220, 237, 220) ' Muted green for past confirmed
                        Else
                            dayPanel.BackColor = Color.FromArgb(232, 245, 233) ' Light green for future confirmed
                        End If
                    ElseIf completedCount > 0 Then
                        ' Light gray for completed only
                        dayPanel.BackColor = Color.FromArgb(245, 245, 245)
                    End If

                    ' Order count indicator (center of cell)
                    Dim lblIndicator As New Label()
                    Dim statusText As String = ""
                    
                    If confirmedCount > 0 And completedCount > 0 Then
                        statusText = $"🛒 {confirmedCount} Confirmed" & vbCrLf & $"✓ {completedCount} Completed"
                    ElseIf confirmedCount > 0 Then
                        statusText = $"🛒 {confirmedCount}" & vbCrLf & "Order" & If(confirmedCount > 1, "s", "")
                    Else
                        statusText = $"✓ {completedCount}" & vbCrLf & "Completed"
                    End If
                    
                    lblIndicator.Text = statusText
                    lblIndicator.Location = New Point(10, 35)
                    lblIndicator.Size = New Size(cellWidth - 20, 50)
                    lblIndicator.TextAlign = ContentAlignment.MiddleCenter
                    lblIndicator.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    
                    ' Color based on status: Blue for confirmed, Gray for completed only
                    If isPastDate AndAlso confirmedCount = 0 Then
                        lblIndicator.ForeColor = Color.Gray
                    ElseIf confirmedCount > 0 Then
                        lblIndicator.ForeColor = Color.FromArgb(0, 123, 255) ' Blue for confirmed
                    Else
                        lblIndicator.ForeColor = Color.FromArgb(108, 117, 125) ' Gray for completed only
                    End If
                    
                    lblIndicator.BackColor = Color.Transparent
                    dayPanel.Controls.Add(lblIndicator)

                    ' Make clickable if has confirmed orders (regardless of date)
                    ' Only completed orders are view-only (non-clickable)
                    If confirmedCount > 0 Then
                        ' Has confirmed orders - always clickable even if past date
                        AddHandler dayPanel.Click, Sub() ShowDayOrders(currentDate)
                        AddHandler lblDay.Click, Sub() ShowDayOrders(currentDate)
                        AddHandler lblIndicator.Click, Sub() ShowDayOrders(currentDate)
                        dayPanel.Cursor = Cursors.Hand
                    ElseIf completedCount > 0 Then
                        ' Has only completed orders - view-only (non-clickable)
                        dayPanel.Cursor = Cursors.No
                    End If
                Else
                    ' No orders message
                    If Not isPastDate Then
                        Dim lblNoOrders As New Label()
                        lblNoOrders.Text = "No orders"
                        lblNoOrders.Location = New Point(10, 40)
                        lblNoOrders.Size = New Size(cellWidth - 20, 40)
                        lblNoOrders.TextAlign = ContentAlignment.MiddleCenter
                        lblNoOrders.Font = New Font("Segoe UI", 8)
                        lblNoOrders.ForeColor = Color.Gray
                        dayPanel.Controls.Add(lblNoOrders)
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

    ' ==========================================
    ' SHOW ALL ORDERS FOR A SPECIFIC DAY IN POPUP
    ' ==========================================
    Private Sub ShowDayOrders(selectedDate As Date)
        Try
            ' Get all orders for this day
            Dim dayOrders = orderData.Where(Function(o) o.OrderDate.Date = selectedDate.Date).OrderBy(Function(o) o.OrderTime).ToList()

            If dayOrders.Count = 0 Then
                MessageBox.Show("No orders found for this date.", "No Orders", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Create popup form
            Dim popupForm As New Form()
            popupForm.Text = $"Orders for {selectedDate:MMMM dd, yyyy}"
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
            Dim confirmedCount = dayOrders.Where(Function(o) o.OrderStatus = "Confirmed").Count()
            Dim completedCount = dayOrders.Where(Function(o) o.OrderStatus = "Completed").Count()
            
            Dim lblHeader As New Label()
            Dim headerText As String = $"🛒 {selectedDate:dddd, MMMM dd, yyyy}" & vbCrLf
            If confirmedCount > 0 And completedCount > 0 Then
                headerText &= $"{confirmedCount} Confirmed, {completedCount} Completed"
            ElseIf confirmedCount > 0 Then
                headerText &= $"{confirmedCount} Confirmed Order" & If(confirmedCount > 1, "s", "")
            Else
                headerText &= $"{completedCount} Completed Order" & If(completedCount > 1, "s", "")
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
            
            Dim yPos As Integer = 10

            For Each ord In dayOrders
                ' Determine if this order is completed
                Dim isCompleted As Boolean = (ord.OrderStatus = "Completed")
                
                ' Order card panel - SQUARE STYLE
                Dim cardPanel As New Panel()
                cardPanel.Size = New Size(250, 250) ' Square size
                cardPanel.Margin = New Padding(10)
                cardPanel.BorderStyle = BorderStyle.FixedSingle
                cardPanel.BackColor = If(isCompleted, Color.FromArgb(245, 245, 245), Color.FromArgb(240, 248, 255))
                cardPanel.Padding = New Padding(10)

                ' Order time (large and prominent) - Convert to 12-hour format
                Dim lblTime As New Label()
                Dim timeDisplay As String = ord.OrderTime.ToString()
                Try
                    ' Try to parse and format to 12-hour
                    Dim timeParsed As DateTime
                    If DateTime.TryParse(ord.OrderTime, timeParsed) Then
                        timeDisplay = timeParsed.ToString("h:mm tt")
                    End If
                Catch
                    ' If parsing fails, use original
                End Try
                lblTime.Text = timeDisplay
                lblTime.Location = New Point(10, 10)
                lblTime.Size = New Size(100, 30)
                lblTime.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                lblTime.ForeColor = If(isCompleted, Color.FromArgb(108, 117, 125), Color.FromArgb(0, 123, 255))
                cardPanel.Controls.Add(lblTime)
                
                ' Status badge
                If isCompleted Then
                    Dim lblStatus As New Label()
                    lblStatus.Text = "✓ COMPLETED"
                    lblStatus.Location = New Point(130, 10) ' Top right
                    lblStatus.Size = New Size(110, 20)
                    lblStatus.Font = New Font("Segoe UI", 8, FontStyle.Bold)
                    lblStatus.ForeColor = Color.White
                    lblStatus.BackColor = Color.FromArgb(108, 117, 125)
                    lblStatus.TextAlign = ContentAlignment.MiddleCenter
                    cardPanel.Controls.Add(lblStatus)
                End If

                ' Order details
                Dim lblDetails As New Label()
                lblDetails.Text = $"{ord.CustomerName}" & vbCrLf &
                                $"{ord.OrderType}" & vbCrLf &
                                $"{ord.ItemsOrderedCount} Item(s)" & vbCrLf &
                                $"Total: ₱{ord.TotalAmount:N2}"
                lblDetails.Location = New Point(10, 45)
                lblDetails.Size = New Size(230, 80)
                lblDetails.Font = New Font("Segoe UI", 10)
                lblDetails.ForeColor = Color.FromArgb(52, 73, 94)
                lblDetails.TextAlign = ContentAlignment.TopLeft
                cardPanel.Controls.Add(lblDetails)

                ' Special Request (if any)
                If ord.SpecialRequests <> "None" AndAlso Not String.IsNullOrWhiteSpace(ord.SpecialRequests) Then
                   Dim lblSpecial As New Label()
                   lblSpecial.Text = "📝 " & ord.SpecialRequests
                   lblSpecial.Location = New Point(10, 130)
                   lblSpecial.Size = New Size(230, 60)
                   lblSpecial.Font = New Font("Segoe UI", 9, FontStyle.Italic)
                   lblSpecial.ForeColor = Color.FromArgb(149, 165, 166)
                   lblSpecial.AutoEllipsis = True
                   cardPanel.Controls.Add(lblSpecial)
                End If

                ' View details button (only for confirmed orders)
                If Not isCompleted Then
                    Dim btnViewDetails As New Button()
                    btnViewDetails.Text = "View Details"
                    btnViewDetails.Location = New Point(10, 205) ' Bottom
                    btnViewDetails.Size = New Size(230, 35)
                    btnViewDetails.BackColor = Color.FromArgb(0, 123, 255)
                    btnViewDetails.ForeColor = Color.White
                    btnViewDetails.FlatStyle = FlatStyle.Flat
                    btnViewDetails.FlatAppearance.BorderSize = 0
                    btnViewDetails.Font = New Font("Segoe UI", 9)
                    btnViewDetails.Cursor = Cursors.Hand

                    Dim ordId As Integer = ord.OrderID
                    AddHandler btnViewDetails.Click, Sub()
                                                         ShowOrderDetails(ordId)
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
            MessageBox.Show("Error showing orders: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ==========================================
    ' SHOW ORDER DETAILS
    ' ==========================================
    Private Sub ShowOrderDetails(orderID As Integer)
        Try
            Dim ord = orderData.FirstOrDefault(Function(o) o.OrderID = orderID)

            If ord IsNot Nothing Then
                ' Format time to 12-hour
                Dim timeDisplay As String = ord.OrderTime.ToString()
                Try
                    Dim timeParsed As DateTime
                    If DateTime.TryParse(ord.OrderTime, timeParsed) Then
                        timeDisplay = timeParsed.ToString("h:mm tt")
                    End If
                Catch
                    ' If parsing fails, use original
                End Try
                
                ' Dynamic status display
                Dim statusText As String = If(ord.OrderStatus = "Completed", "COMPLETED ✓", "CONFIRMED ✓")
                
                Dim details As String =
                    $"═══════════════════════════════════════" & vbCrLf &
                    $"            ORDER DETAILS" & vbCrLf &
                    $"═══════════════════════════════════════" & vbCrLf & vbCrLf &
                    $"Status: {statusText}" & vbCrLf & vbCrLf &
                    $"Customer Information:" & vbCrLf &
                    $"  • Name: {ord.CustomerName}" & vbCrLf &
                    $"  • Contact: {ord.ContactNumber}" & vbCrLf & vbCrLf &
                    $"Order Details:" & vbCrLf &
                    $"  • Type: {ord.OrderType}" & vbCrLf &
                    $"  • Date: {ord.OrderDate:MMMM dd, yyyy (dddd)}" & vbCrLf &
                    $"  • Time: {timeDisplay}" & vbCrLf &
                    $"  • Items Ordered: {ord.ItemsOrderedCount}" & vbCrLf &
                    $"  • Total Amount: ₱{ord.TotalAmount:N2}" & vbCrLf & vbCrLf &
                    $"Products:" & vbCrLf &
                    $"  {ord.OrderedProducts}" & vbCrLf & vbCrLf &
                    $"Delivery Information:" & vbCrLf &
                    $"  • Option: {ord.DeliveryOption}" & vbCrLf &
                    $"  • Address: {ord.DeliveryAddress}" & vbCrLf & vbCrLf &
                    $"Special Requests:" & vbCrLf &
                    $"  {ord.SpecialRequests}" & vbCrLf & vbCrLf &
                    $"═══════════════════════════════════════"

                MessageBox.Show(details, "Order Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error showing details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ==========================================
    ' NAVIGATION BUTTONS
    ' ==========================================
    Private Sub btnPrevMonth_Click(sender As Object, e As EventArgs) Handles btnPrevMonth.Click
        currentMonth = currentMonth.AddMonths(-1)
        LoadOrders()
        DisplayCalendar()
    End Sub

    Private Sub btnNextMonth_Click(sender As Object, e As EventArgs) Handles btnNextMonth.Click
        currentMonth = currentMonth.AddMonths(1)
        LoadOrders()
        DisplayCalendar()
    End Sub

    Private Sub btnToday_Click(sender As Object, e As EventArgs) Handles btnToday.Click
        currentMonth = New Date(Date.Now.Year, Date.Now.Month, 1)
        LoadOrders()
        DisplayCalendar()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadOrders()
        DisplayCalendar()
        MessageBox.Show("Calendar refreshed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub OrderCalendar_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If orderData IsNot Nothing Then
            DisplayCalendar()
        End If
    End Sub

    ' Close button
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' Paint light gray border
    Private Sub OrderCalendar_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
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
End Class