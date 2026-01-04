Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.Linq
Imports MySqlConnector

Public Class Reports
    Public Shared Property Instance As Reports

    ' === SHARED PROPERTY FOR PERIOD SELECTION ===
    Public Shared Property SelectedPeriod As String = "Daily"
    Public Shared Property SelectedYear As Integer = DateTime.Now.Year
    Public Shared SelectedMonth As Integer = DateTime.Now.Month
    Private Shared _filterDate As DateTime = DateTime.Now

    Public Shared Property GlobalFilterDate As DateTime
        Get
            Return _filterDate
        End Get
        Set(value As DateTime)
            _filterDate = value
        End Set
    End Property

    ' === PDF EXPORT PRIVATE VARIABLES ===
    Private WithEvents prnDoc As New PrintDocument()
    Private activeGrids As New List(Of DataGridView)
    Private activeCharts As New List(Of System.Windows.Forms.DataVisualization.Charting.Chart)
    Private reportTitle As String = ""

    ' Printing State
    Private m_PageIndex As Integer = 0
    Private m_GridIndex As Integer = 0
    Private m_RowIndex As Integer = 0
    Private m_ChartIndex As Integer = 0
    Private m_SummaryLabels As New Dictionary(Of String, String)


    ' === Load Form into Panel1 ===
    Private Sub LoadFormIntoPanel(childForm As Form)
        Panel1.Controls.Clear()

        childForm.TopLevel = False
        childForm.FormBorderStyle = FormBorderStyle.None
        childForm.Dock = DockStyle.Fill

        ' Add to panel and show
        Panel1.Controls.Add(childForm)
        childForm.Show()
    End Sub


    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Instance = Me
        InitializeFilters()
        Me.AutoScroll = True
        Me.AutoScrollMinSize = New Size(Me.Width, Me.Height)
        Panel1.AutoSize = False
        Panel1.AutoScroll = True
        Panel1.BorderStyle = BorderStyle.None

        ' === FLOWLAYOUTPANEL SETTINGS ===
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.WrapContents = False
        FlowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight
        FlowLayoutPanel1.Padding = New Padding(8)
        FlowLayoutPanel1.Margin = New Padding(0)
        FlowLayoutPanel1.BackColor = Color.FromArgb(240, 240, 240)
        FlowLayoutPanel1.Height = 50
        FlowLayoutPanel1.Top = 80   'Adjust below your label
        FlowLayoutPanel1.Left = 20
        FlowLayoutPanel1.Width = Me.ClientSize.Width - 30
        FlowLayoutPanel1.Height = 70

        FlowLayoutPanel1.AutoSize = False

        ' === APPLY ROUNDED CORNERS TO FLOWLAYOUTPANEL ===
        ApplyRoundedCorners(FlowLayoutPanel1, 35)

        ' === MOVE EXISTING BUTTONS TO FLOWLAYOUTPANEL ===
        Dim toMove As New List(Of Control)
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Parent Is Me Then
                ' Exclude header action buttons from moving to navigation bar
                If ctrl IsNot btnRefresh AndAlso ctrl IsNot btnExportGlobal Then
                    toMove.Add(ctrl)
                End If
            End If
        Next

        For Each ctrl As Control In toMove
            FlowLayoutPanel1.Controls.Add(ctrl)
        Next

        ' Ensure header buttons stay on top
        btnRefresh.BringToFront()
        btnExportGlobal.BringToFront()

        ' Bring FlowLayoutPanel forward so buttons are visible
        FlowLayoutPanel1.BringToFront()
        LoadFormIntoPanel(New FormSales())
        HighlightActiveButton(btnSales)
    End Sub

    Private Sub InitializeFilters()
        ' === INITIALIZE COMBOBOX ===
        reportPeriod.SelectedIndex = 0 ' Default to "Daily"
        reportPeriod.DropDownStyle = ComboBoxStyle.DropDownList

        ' === INITIALIZE YEAR COMBOBOX DYNAMICALLY ===
        cmbYear.Items.Clear()
        Dim years As New List(Of Integer)
        Dim currentYear As Integer = DateTime.Now.Year

        Try
            If conn Is Nothing OrElse conn.State <> ConnectionState.Open Then openConn()

            ' Get unique years from both orders and payments
            Dim sql As String = "SELECT DISTINCT YEAR(OrderDate) as Yr FROM orders WHERE OrderDate IS NOT NULL " &
                               "UNION " &
                               "SELECT DISTINCT YEAR(PaymentDate) as Yr FROM payments WHERE PaymentDate IS NOT NULL AND ReservationID IS NOT NULL " &
                               "ORDER BY Yr DESC"

            Using cmd As New MySqlConnector.MySqlCommand(sql, conn)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        If Not IsDBNull(reader("Yr")) Then
                            years.Add(Convert.ToInt32(reader("Yr")))
                        End If
                    End While
                End Using
            End Using
        Catch ex As Exception
            ' Fallback to recent years if query fails
            years.Add(currentYear)
            years.Add(currentYear - 1)
        End Try

        ' Ensure current year is at least available
        If Not years.Contains(currentYear) Then years.Add(currentYear)
        years.Sort()
        years.Reverse()

        For Each y In years
            ' User Requested Exclusion of 2024 and 2027
            If y <> 2024 AndAlso y <> 2027 Then
                cmbYear.Items.Add(y)
            End If
        Next

        If cmbYear.Items.Count > 0 Then
            If cmbYear.Items.Contains(currentYear) Then
                cmbYear.SelectedItem = currentYear
                SelectedYear = currentYear
            Else
                cmbYear.SelectedIndex = 0
                SelectedYear = Convert.ToInt32(cmbYear.SelectedItem)
            End If
        End If

        ' === INITIALIZE MONTH COMBOBOX ===
        cmbMonth.Items.Clear()
        cmbMonth.Items.Add("All Months") ' Index 0
        Dim monthNames As String() = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
        For i As Integer = 0 To 11
            If Not String.IsNullOrEmpty(monthNames(i)) Then
                cmbMonth.Items.Add(monthNames(i))
            End If
        Next
        cmbMonth.SelectedIndex = DateTime.Now.Month ' Select current month by default
        SelectedMonth = cmbMonth.SelectedIndex

        UpdateFilterVisibility()
    End Sub



    Public Sub ResetToDefault()
        LoadFormIntoPanel(New FormSales())
        HighlightActiveButton(btnSales)
        reportPeriod.SelectedIndex = 0 ' Reset to Daily
    End Sub
    ' === APPLY ROUNDED CORNERS TO CONTROL ===
    Private Sub ApplyRoundedCorners(ctrl As Control, radius As Integer)
        Dim gp As New GraphicsPath()
        gp.AddArc(0, 0, radius, radius, 180, 90)
        gp.AddArc(ctrl.Width - radius, 0, radius, radius, 270, 90)
        gp.AddArc(ctrl.Width - radius, ctrl.Height - radius, radius, radius, 0, 90)
        gp.AddArc(0, ctrl.Height - radius, radius, radius, 90, 90)
        gp.CloseFigure()
        ctrl.Region = New Region(gp)
    End Sub

    ' === BUTTON CLICKS ===
    Private Sub Button_Click(sender As Object, e As EventArgs) _
        Handles btnSales.Click, btnOrders.Click, btnPayroll.Click, btnCatering.Click, btnStatus.Click,
                btnDineIn.Click, btnTakeout.Click, btnCustomerHistory.Click, btnEmployeeAttendance.Click, btnProductsPerformance.Click

        Dim clickedBtn As Button = CType(sender, Button)
        HighlightActiveButton(CType(sender, Button))

        Select Case clickedBtn.Name
            Case "btnSales" : LoadFormIntoPanel(New FormSales())
            Case "btnOrders" : LoadFormIntoPanel(New FormOrders())
            Case "btnPayroll" : LoadFormIntoPanel(New FormPayroll())
            Case "btnCatering" : LoadFormIntoPanel(New FormCateringReservations())
            Case "btnStatus" : LoadFormIntoPanel(New FormReservationStatus())
            Case "btnDineIn" : LoadFormIntoPanel(New FormDineInOrders())
            Case "btnTakeout" : LoadFormIntoPanel(New FormTakeOutOrders())
            Case "btnCustomerHistory" : LoadFormIntoPanel(New FormCustomerHistory())
            Case "btnEmployeeAttendance" : LoadFormIntoPanel(New FormEmployeeAttendance())
            Case "btnProductsPerformance" : LoadFormIntoPanel(New FormProductPerformance())
        End Select
    End Sub

    ' === HIGHLIGHT ACTIVE BUTTON WITH PILL SHAPE ===
    Private Sub HighlightActiveButton(activeBtn As Button)
        ' Reset all buttons first
        For Each ctrl As Control In FlowLayoutPanel1.Controls
            If TypeOf ctrl Is Button Then
                Dim btn As Button = CType(ctrl, Button)
                btn.BackColor = Color.FromArgb(240, 240, 240) ' Light gray default
                btn.ForeColor = Color.Black
                btn.FlatAppearance.MouseOverBackColor = btn.BackColor
                btn.Region = Nothing
            End If
        Next

        ' Apply white color to the active (clicked) button
        activeBtn.BackColor = Color.White
        activeBtn.ForeColor = Color.Black
        activeBtn.FlatAppearance.MouseOverBackColor = Color.White

        ' Create pill-shaped rounded corners (fully rounded ends)
        Dim radius As Integer = activeBtn.Height ' Use height as radius for pill shape
        Dim gp As New GraphicsPath()

        ' Left semi-circle
        gp.AddArc(0, 0, radius, radius, 90, 180)
        ' Right semi-circle
        gp.AddArc(activeBtn.Width - radius, 0, radius, radius, 270, 180)

        gp.CloseFigure()
        activeBtn.Region = New Region(gp)
    End Sub

    Private Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) _
       Handles reportPeriod.DrawItem

        If e.Index < 0 Then Return
        Dim cmb As ComboBox = DirectCast(sender, ComboBox)
        e.DrawBackground()
        e.Graphics.DrawString(cmb.Items(e.Index).ToString(), cmb.Font, Brushes.Black, e.Bounds)
        e.DrawFocusRectangle()
    End Sub


    ' === PERIOD SELECTION CHANGED ===
    Private Sub reportPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles reportPeriod.SelectedIndexChanged
        ' Update the shared property
        SelectedPeriod = reportPeriod.SelectedItem.ToString()
        UpdateFilterVisibility()
        RefreshCurrentlyLoadedForm()
    End Sub

    Private Sub cmbYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbYear.SelectedIndexChanged
        If cmbYear.SelectedItem IsNot Nothing Then
            SelectedYear = Convert.ToInt32(cmbYear.SelectedItem)

            ' Synchronize with DatePicker if in Daily/Weekly mode
            If SelectedPeriod = "Daily" OrElse SelectedPeriod = "Weekly" Then
                Try
                    ' Only update if year is different to avoid recursive loops/redundant refreshes
                    If _filterDate.Year <> SelectedYear Then
                        _filterDate = New DateTime(SelectedYear, _filterDate.Month, _filterDate.Day)
                    End If
                Catch
                    ' Fallback for invalid dates (e.g. Feb 29 in non-leap year)
                    Try
                        _filterDate = New DateTime(SelectedYear, _filterDate.Month, 1)
                    Catch
                    End Try
                End Try
            End If

            RefreshCurrentlyLoadedForm()
        End If
    End Sub

    Private Sub cmbMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMonth.SelectedIndexChanged
        SelectedMonth = cmbMonth.SelectedIndex
        RefreshCurrentlyLoadedForm()
    End Sub

    ' === SHARED PROPERTY FOR GLOBAL DATE FILTER ===


    Private Sub UpdateFilterVisibility()
        ' Month selection is only critical for "Monthly"
        Dim isMonthly As Boolean = (SelectedPeriod = "Monthly")
        cmbMonth.Enabled = isMonthly
        lblMonth.Enabled = isMonthly

        ' Date Picker is needed for Daily and Weekly
        Dim isDateSpecific As Boolean = (SelectedPeriod = "Daily" OrElse SelectedPeriod = "Weekly")

        ' Year is usually relevant for Monthly and Yearly, or as context for Daily/Weekly if needed
        ' But for Daily/Weekly, the DatePicker value is the source of truth.
        ' However, we keep it enabled to allow quick shifting if the child form uses it.
        cmbYear.Enabled = True
        lblYear.Enabled = True
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshCurrentlyLoadedForm()
    End Sub

    Private Sub btnExportGlobal_Click(sender As Object, e As EventArgs) Handles btnExportGlobal.Click
        ExportCurrentReport()
    End Sub



    Private Sub RefreshCurrentlyLoadedForm()
        ' Reload the current form data to apply the new filters
        If Panel1.Controls.Count > 0 Then
            Dim currentForm = Panel1.Controls(0)

            ' Try to call RefreshData method using reflection
            Try
                Dim mi = currentForm.GetType().GetMethod("RefreshData")
                If mi IsNot Nothing Then
                    mi.Invoke(currentForm, Nothing)
                    Return ' Successful refresh
                End If
            Catch ex As Exception
                ' Log error if needed
            End Try

            ' Fallback: If RefreshData is not available, we have to reload the form
            ' This is less efficient but ensures the data updates.
            For Each ctrl As Control In FlowLayoutPanel1.Controls
                If TypeOf ctrl Is Button Then
                    Dim btn As Button = CType(ctrl, Button)
                    ' White background indicates the active report button in our UI
                    If btn.BackColor = Color.White Then
                        Select Case btn.Name
                            Case "btnSales" : LoadFormIntoPanel(New FormSales())
                            Case "btnOrders" : LoadFormIntoPanel(New FormOrders())
                            Case "btnPayroll" : LoadFormIntoPanel(New FormPayroll())
                            Case "btnCatering" : LoadFormIntoPanel(New FormCateringReservations())
                            Case "btnStatus" : LoadFormIntoPanel(New FormReservationStatus())
                            Case "btnDineIn" : LoadFormIntoPanel(New FormDineInOrders())
                            Case "btnTakeout" : LoadFormIntoPanel(New FormTakeOutOrders())
                            Case "btnCustomerHistory" : LoadFormIntoPanel(New FormCustomerHistory())
                            Case "btnEmployeeAttendance" : LoadFormIntoPanel(New FormEmployeeAttendance())
                            Case "btnProductsPerformance" : LoadFormIntoPanel(New FormProductPerformance())
                        End Select
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub

    ' === HELPER FUNCTION TO GET SQL DATE GROUPING ===
    Public Shared Function GetDateGrouping(dateColumn As String) As String
        Select Case SelectedPeriod
            Case "Daily"
                Return $"DATE({dateColumn})"
            Case "Weekly"
                Return $"YEARWEEK({dateColumn}, 1)"
            Case "Monthly"
                Return $"DATE_FORMAT({dateColumn}, '%Y-%m')"
            Case "Yearly"
                Return $"YEAR({dateColumn})"
            Case Else
                Return $"DATE({dateColumn})"
        End Select
    End Function

    ' === HELPER FUNCTION TO GET DISPLAY FORMAT ===
    Public Shared Function GetDateDisplayFormat(dateValue As Object) As String
        Select Case SelectedPeriod
            Case "Daily"
                Return Convert.ToDateTime(dateValue).ToString("MMM dd, yyyy")
            Case "Weekly"
                Return $"Week {dateValue}"
            Case "Monthly"
                Return Convert.ToDateTime(dateValue & "-01").ToString("MMM yyyy")
            Case "Yearly"
                Return dateValue.ToString()
            Case Else
                Return dateValue.ToString()
        End Select
    End Function
    Private Sub LoadDefaultForm()
        LoadFormIntoPanel(New FormSales())
        HighlightActiveButton(btnSales)
    End Sub

    ' === PUBLIC METHOD TO LOAD CATERING RESERVATIONS FROM EXTERNAL CALL ===
    ' ============================================
    ' PUBLIC METHOD - Load Catering Reservations Report from Dashboard
    ' ============================================

    ' === PUBLIC METHOD TO LOAD CATERING RESERVATIONS FROM EXTERNAL CALL ===
    Public Sub LoadCateringReservationReport()
        Try
            ' Load FormCateringReservations into Panel1
            LoadFormIntoPanel(New FormReservationStatus())

            ' Highlight the Catering Reservations button as active
            HighlightActiveButton(btnStatus)

            ' Scroll to top of the panel
            Panel1.AutoScrollPosition = New Point(0, 0)

            ' Bring Reports form to front and focus
            Me.BringToFront()
            Me.Activate()
            Me.Focus()

            ' Optional: Set the period dropdown to Daily
            If reportPeriod.Items.Contains("Daily") Then
                reportPeriod.SelectedItem = "Daily"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading catering reservation report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub LoadOrderTrends()
        Try
            ' Load FormCateringReservations into Panel1
            LoadFormIntoPanel(New FormOrders())

            ' Highlight the Catering Reservations button as active
            HighlightActiveButton(btnOrders)

            ' Scroll to top of the panel
            Panel1.AutoScrollPosition = New Point(0, 0)

            ' Bring Reports form to front and focus
            Me.BringToFront()
            Me.Activate()
            Me.Focus()

            ' Optional: Set the period dropdown to Daily
            If reportPeriod.Items.Contains("Daily") Then
                reportPeriod.SelectedItem = "Daily"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading catering reservation report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ' ============================================
    ' ADD THIS TO Reports.vb to handle navigation
    ' ============================================
    Public Sub LoadSalesReport()
        Try
            ' Load FormCateringReservations into Panel1
            LoadFormIntoPanel(New FormSales())

            ' Highlight the Catering Reservations button as active
            HighlightActiveButton(btnSales)

            ' Scroll to top of the panel
            Panel1.AutoScrollPosition = New Point(0, 0)

            ' Bring Reports form to front and focus
            Me.BringToFront()
            Me.Activate()
            Me.Focus()
        Catch ex As Exception
            MessageBox.Show("Error loading catering reservation report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub LoadProductPerformanceReport()
        Try
            ' Load FormCateringReservations into Panel1
            LoadFormIntoPanel(New FormProductPerformance())

            ' Highlight the Catering Reservations button as active
            HighlightActiveButton(btnProductsPerformance)

            ' Scroll to top of the panel
            Panel1.AutoScrollPosition = New Point(0, 0)

            ' Bring Reports form to front and focus
            Me.BringToFront()
            Me.Activate()
            Me.Focus()
        Catch ex As Exception
            MessageBox.Show("Error loading product performance report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ' === PUBLIC METHOD FOR PDF EXPORT / PREVIEW ===
    Public Sub ExportCurrentReport()
        If Panel1.Controls.Count = 0 Then Return

        Dim currentForm = Panel1.Controls(0)

        ' Set report title based on form type
        ' Set report title based on form type - Force nicer names
        reportTitle = currentForm.Text
        If TypeOf currentForm Is FormSales Then reportTitle = "Sales Report"
        If TypeOf currentForm Is FormOrders Then reportTitle = "Orders Report"
        If TypeOf currentForm Is FormPayroll Then reportTitle = "Payroll Report"
        If TypeOf currentForm Is FormCateringReservations Then reportTitle = "Catering Report"
        If TypeOf currentForm Is FormReservationStatus Then reportTitle = "Reservation Status"
        If TypeOf currentForm Is FormDineInOrders Then reportTitle = "Dine-In Orders"
        If TypeOf currentForm Is FormTakeOutOrders Then reportTitle = "Take-Out Orders"
        If TypeOf currentForm Is FormCustomerHistory Then reportTitle = "Customer History"
        If TypeOf currentForm Is FormEmployeeAttendance Then reportTitle = "Employee Attendance"
        If TypeOf currentForm Is FormProductPerformance Then reportTitle = "Product Performance"

        ' Try to find ALL DataGridViews and Charts
        activeGrids.Clear()
        activeCharts.Clear()
        m_SummaryLabels.Clear()

        FindAllControls(Of DataGridView)(currentForm, activeGrids)
        FindAllControls(Of System.Windows.Forms.DataVisualization.Charting.Chart)(currentForm, activeCharts)

        ' === DATA EXTRACTION ===
        ' Extract specific labels based on known form types to ensure headers are correct
        If TypeOf currentForm Is FormReservationStatus Then
            Dim f = DirectCast(currentForm, FormReservationStatus)
            m_SummaryLabels.Add("Total Reservations", f.lblTotalReservations.Text)
            m_SummaryLabels.Add("Pending", f.lblPending.Text)
            m_SummaryLabels.Add("Confirmed", f.lblConfirmed.Text)
            m_SummaryLabels.Add("Cancelled", f.lblCancelled.Text)
        ElseIf TypeOf currentForm Is FormOrders Then
             ' Add logic for FormOrders if needed, e.g.
             ' Dim f = DirectCast(currentForm, FormOrders)
             ' m_SummaryLabels.Add("Total Orders", f.Label4.Text) 
        End If
        
        ' Fallback: Scrape standard labels if no specific extraction above
        If m_SummaryLabels.Count = 0 Then
             For i As Integer = 1 To 10
                Dim lbl As Control = currentForm.Controls.Find("Label" & i, True).FirstOrDefault()
                If lbl IsNot Nothing AndAlso Not String.IsNullOrEmpty(lbl.Text) AndAlso lbl.Text <> "..." Then
                     m_SummaryLabels.Add($"Stat #{i}", lbl.Text)
                End If
            Next
        End If

        ' Add Header Label if exists
        Dim lblHeader As Control = currentForm.Controls.Find("LabelHeader", True).FirstOrDefault()
        If lblHeader IsNot Nothing Then m_SummaryLabels("HeaderExtra") = lblHeader.Text ' Use dictionary key safely

        If activeGrids.Count = 0 AndAlso activeCharts.Count = 0 Then
            MessageBox.Show("No exportable data (Table or Chart) found in the current report.", "Export Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' Suppress the "Printing..." status dialog
            prnDoc.PrintController = New StandardPrintController()

            ' === CUSTOM PRINT PREVIEW FORM ===
            ' Use the new custom form to match the user's design request
            Dim previewForm As New FormReportPreview(prnDoc, $"Report Preview - {reportTitle}")

            ' Setup Page Settings
            If activeGrids.Count > 0 Then
                Dim maxCols = activeGrids.Max(Function(g) g.Columns.Count)
                prnDoc.DefaultPageSettings.Landscape = (maxCols > 6)
            Else
                prnDoc.DefaultPageSettings.Landscape = False
            End If

            previewForm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Error initializing preview: " & ex.Message, "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Helper to find controls recursively (ALL matches)
    Private Sub FindAllControls(Of T As Control)(parent As Control, ByRef list As List(Of T))
        For Each ctrl As Control In parent.Controls
            If TypeOf ctrl Is T Then list.Add(DirectCast(ctrl, T))
            FindAllControls(Of T)(ctrl, list)
        Next
    End Sub

    ' === BEGIN PRINT - RESET STATE ===
    Private Sub prnDoc_BeginPrint(sender As Object, e As PrintEventArgs) Handles prnDoc.BeginPrint
        m_PageIndex = 0
        m_GridIndex = 0
        m_RowIndex = 0
        m_ChartIndex = 0
    End Sub

    ' === PRINT PAGE HANDLER (Visual Upgrade) ===
    Private Sub prnDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles prnDoc.PrintPage
        m_PageIndex += 1
        Dim g As Graphics = e.Graphics
        
        ' Branding Colors
        Dim brandColor As Color = Color.FromArgb(55, 65, 81)   ' Dark Slate
        Dim accentColor As Color = Color.FromArgb(249, 115, 22) ' Orange #F97316
        Dim cardBg As Color = Color.White
        Dim headerBg As Color = Color.FromArgb(243, 244, 246)   ' Light Gray
        
        ' Fonts
        Dim fontTitle As New Font("Segoe UI", 24, FontStyle.Bold)
        Dim fontSubtitle As New Font("Segoe UI", 12, FontStyle.Regular)
        Dim fontCardTitle As New Font("Segoe UI", 9, FontStyle.Regular)
        Dim fontCardValue As New Font("Segoe UI", 14, FontStyle.Bold)
        Dim fontHeader As New Font("Segoe UI", 9, FontStyle.Bold)
        Dim fontRow As New Font("Segoe UI", 9)
        Dim fontRowAlt As New Font("Segoe UI", 9)
        Dim fontFooter As New Font("Segoe UI", 8, FontStyle.Italic)

        ' Brushes/Pens
        Dim brushTitle As New SolidBrush(brandColor)
        Dim brushDim As New SolidBrush(Color.Gray)
        Dim penLine As New Pen(Color.LightGray, 1)
        Dim penCardBorder As New Pen(Color.FromArgb(229, 231, 235), 1)

        ' Margins
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim topMargin As Integer = e.MarginBounds.Top
        Dim bottomMargin As Integer = e.MarginBounds.Bottom
        Dim rightMargin As Integer = e.MarginBounds.Right
        Dim contentWidth As Integer = e.MarginBounds.Width
        
        Dim y As Integer = topMargin

        ' ---------------------------------------------------------
        ' 1. REPORT HEADER (First Page Only)
        ' ---------------------------------------------------------
        If m_PageIndex = 1 Then
            ' Top Branding Bar
            g.FillRectangle(New SolidBrush(accentColor), leftMargin, y - 20, contentWidth, 10)
            y += 20

            ' Title
            g.DrawString("LABEYA SYSTEM", New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.Gray), leftMargin, y)
            y += 25
            g.DrawString(reportTitle, fontTitle, brushTitle, leftMargin, y)
            y += 45
            
            ' Date & Period
            Dim periodStr As String = $"{DateTime.Now:MMM dd, yyyy}  •  {SelectedPeriod}"
            If m_SummaryLabels.ContainsKey("HeaderExtra") Then
                periodStr &= "  •  " & m_SummaryLabels("HeaderExtra")
                m_SummaryLabels.Remove("HeaderExtra") ' Remove so it doesn't print as a card
            End If
            g.DrawString(periodStr, fontSubtitle, brushDim, leftMargin, y)
            y += 35
            
            ' Divider
            g.DrawLine(penLine, leftMargin, y, rightMargin, y)
            y += 30
            
            ' ---------------------------------------------------------
            ' 2. SUMMARY CARDS (First Page Only)
            ' ---------------------------------------------------------
            If m_SummaryLabels.Count > 0 Then
                Dim cardWidth As Integer = 160
                Dim cardHeight As Integer = 70
                Dim xCard As Integer = leftMargin
                Dim cardsPerRow As Integer = contentWidth \ (cardWidth + 10)
                Dim count As Integer = 0

                For Each key In m_SummaryLabels.Keys
                     ' Draw Card Background
                     g.FillRectangle(Brushes.White, xCard, y, cardWidth, cardHeight)
                     g.DrawRectangle(penCardBorder, xCard, y, cardWidth, cardHeight)
                     
                     ' Draw Left Accent Border
                     g.FillRectangle(New SolidBrush(accentColor), xCard, y, 4, cardHeight)

                     ' Text
                     g.DrawString(key, fontCardTitle, Brushes.Gray, xCard + 15, y + 10)
                     
                     ' Value (Truncate if too long)
                     Dim valStr = m_SummaryLabels(key)
                     If valStr.Length > 15 Then valStr = valStr.Substring(0, 12) & "..."
                     g.DrawString(valStr, fontCardValue, Brushes.Black, xCard + 15, y + 30)

                     ' Move X
                     xCard += cardWidth + 15
                     count += 1
                     
                     ' Wrap if needed (basic wrapping)
                     If count >= cardsPerRow Then
                         xCard = leftMargin
                         y += cardHeight + 15
                         count = 0
                     End If
                Next
                
                If count > 0 Then y += cardHeight + 40
            End If
            
        Else
            ' Header on Subsequent Pages
            g.DrawString($"{reportTitle} - Page {m_PageIndex}", fontSubtitle, brushDim, leftMargin, y)
            g.DrawLine(penLine, leftMargin, y + 20, rightMargin, y + 20)
            y += 40
        End If

        ' ---------------------------------------------------------
        ' 3. CHARTS (Flow Layout)
        ' ---------------------------------------------------------
        While m_ChartIndex < activeCharts.Count
            Dim chart = activeCharts(m_ChartIndex)
            If Not chart.Visible Then
                m_ChartIndex += 1
                Continue While
            End If

            ' Calculate desired size to fit width
            Dim targetWidth As Integer = contentWidth
            Dim targetHeight As Integer = 250 ' Fixed height for charts

            ' Check for page overflow
            If y + targetHeight > bottomMargin Then
                e.HasMorePages = True
                Return ' Start new page
            End If

            ' Render Chart
            Try
                ' Create high-res bitmap
                Using bmp As New Bitmap(chart.Width, chart.Height)
                     chart.DrawToBitmap(bmp, New Rectangle(0, 0, chart.Width, chart.Height))
                     g.DrawImage(bmp, New Rectangle(leftMargin, y, targetWidth, targetHeight))
                End Using
            Catch
                 g.DrawString("[Chart Rendering Error]", fontRow, Brushes.Red, leftMargin, y)
            End Try
            
            y += targetHeight + 30
            m_ChartIndex += 1
        End While


        ' ---------------------------------------------------------
        ' 4. DATAGRIDVIEWS (Zebra Striped Table)
        ' ---------------------------------------------------------
        While m_GridIndex < activeGrids.Count
            Dim grid = activeGrids(m_GridIndex)
            If Not grid.Visible OrElse grid.Columns.Count = 0 Then
                m_GridIndex += 1
                m_RowIndex = 0
                Continue While
            End If

            ' --- Calculate Column Widths ---
            Dim colWidths As New List(Of Integer)
            Dim totalGridWidth As Integer = 0
            Dim visibleCols As New List(Of DataGridViewColumn)
            
            For Each col As DataGridViewColumn In grid.Columns
                If col.Visible Then
                    visibleCols.Add(col)
                    totalGridWidth += col.Width
                End If
            Next
            
            If totalGridWidth = 0 Then
                 m_GridIndex += 1
                 Continue While
            End If

            Dim scaleFactor As Single = contentWidth / totalGridWidth

            ' --- Print Grid Header (If starting new grid OR new page) ---
            ' We re-print headers on new pages for continuity
            Dim headerHeight As Integer = 30
            
            ' Check if we even have space for header + 1 row
            If y + headerHeight + 25 > bottomMargin Then
                e.HasMorePages = True
                Return
            End If
            
            ' Draw Header Background
            g.FillRectangle(New SolidBrush(headerBg), New Rectangle(leftMargin, y, contentWidth, headerHeight))
            g.DrawRectangle(penLine, New Rectangle(leftMargin, y, contentWidth, headerHeight))

            Dim curX As Single = leftMargin
            For Each col In visibleCols
                Dim colW As Single = col.Width * scaleFactor
                Dim format As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
                
                ' Right align headers for numeric columns? Optional. keeping Left for headers usually looks better.
                g.DrawString(col.HeaderText, fontHeader, Brushes.Black, New RectangleF(curX + 5, y, colW - 10, headerHeight), format)
                curX += colW
            Next
            y += headerHeight

            ' --- Print Rows ---
            While m_RowIndex < grid.Rows.Count
                Dim row = grid.Rows(m_RowIndex)
                If row.IsNewRow Then
                     m_RowIndex += 1
                     Continue While
                End If
                
                Dim rowHeight As Integer = 25
                
                ' Check Overflow
                If y + rowHeight > bottomMargin Then
                    e.HasMorePages = True
                    Return ' Continue this grid on next page
                End If

                ' Alternating Colors
                If m_RowIndex Mod 2 = 1 Then
                    g.FillRectangle(New SolidBrush(Color.FromArgb(250, 250, 250)), New Rectangle(leftMargin, y, contentWidth, rowHeight))
                End If
                
                curX = leftMargin
                For Each col In visibleCols
                    Dim colW As Single = col.Width * scaleFactor
                    Dim val = If(row.Cells(col.Index).Value, "").ToString()
                    
                    ' Align text based on grid column alignment (Right for money usually)
                    Dim format As New StringFormat() With {.LineAlignment = StringAlignment.Center}
                    If col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight OrElse 
                       col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight OrElse
                       col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight Then
                        format.Alignment = StringAlignment.Far
                    ElseIf col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter Then
                        format.Alignment = StringAlignment.Center
                    Else
                        format.Alignment = StringAlignment.Near
                    End If
                    
                    g.DrawString(val, fontRow, Brushes.Black, New RectangleF(curX + 5, y, colW - 10, rowHeight), format)
                    
                    ' Vertical Grid Lines (Optional - keeping clean look without them for now, just outer border?)
                    ' g.DrawLine(penLine, curX + colW, y, curX + colW, y + rowHeight)
                    
                    curX += colW
                Next
                
                ' Row Bottom Line
                g.DrawLine(New Pen(Color.FromArgb(240, 240, 240)), leftMargin, y + rowHeight, rightMargin, y + rowHeight)
                
                y += rowHeight
                m_RowIndex += 1
            End While
            
            ' Grid Finished
            m_GridIndex += 1
            m_RowIndex = 0
            y += 20 ' Spacer after grid
            
        End While

        ' ---------------------------------------------------------
        ' 4. FOOTER (Every Page)
        ' ---------------------------------------------------------
        Dim footerY As Integer = e.PageBounds.Height - 40
        g.DrawLine(penLine, leftMargin, footerY, rightMargin, footerY)
        g.DrawString("Tabeya System Professional Report", fontFooter, Brushes.Gray, leftMargin, footerY + 5)
        g.DrawString($"Page {m_PageIndex}", fontFooter, Brushes.Gray, rightMargin - 50, footerY + 5)
        
        e.HasMorePages = False
    End Sub

    Private Sub Reports_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If FlowLayoutPanel1 IsNot Nothing Then
            FlowLayoutPanel1.Width = Me.ClientSize.Width - 40
            ApplyRoundedCorners(FlowLayoutPanel1, 35)
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class