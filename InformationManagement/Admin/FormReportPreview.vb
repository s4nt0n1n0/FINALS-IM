Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.Drawing

Public Class FormReportPreview
    Inherits Form

    Private WithEvents btnPrint, btnZoomIn, btnZoomOut As Button
    Private previewControl As PrintPreviewControl
    Private lblTitle, lblZoom As Label
    Private headerPanel As Panel, separatorLine As Panel
    Private loadingPanel As Panel
    Private WithEvents _doc As PrintDocument

    Public Sub New(doc As PrintDocument, title As String)
        _doc = doc
        InitializeCustomComponent()
        lblTitle.Text = title
        
        ' Show loading state initially
        loadingPanel.Visible = True
        
        previewControl.Document = doc
        previewControl.Zoom = 1.0 ' Default to full page visibility or standard zoom
        UpdateZoomLabel()

        AddHandler Me.Load, Sub() previewControl.Focus()
    End Sub
    
    Private Sub OnPrintEnd(sender As Object, e As PrintEventArgs) Handles _doc.EndPrint
        ' Hide loading panel when generation finishes
        If loadingPanel.InvokeRequired Then
            loadingPanel.Invoke(Sub() loadingPanel.Visible = False)
        Else
            loadingPanel.Visible = False
        End If
    End Sub

    Private Sub InitializeCustomComponent()
        Me.Size = New Size(1000, 850)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.BackColor = Color.White
        Me.ShowIcon = False
        Me.Text = "Report Preview"
        Me.KeyPreview = True ' Important for form-level key handling

        ' 1. Header Panel
        headerPanel = New Panel()
        headerPanel.Width = 950 ' Set explicit width matching form to prevent anchor drift
        headerPanel.Dock = DockStyle.Top
        headerPanel.Height = 70
        headerPanel.BackColor = Color.White
        headerPanel.Padding = New Padding(25, 0, 25, 0)

        ' Title
        lblTitle = New Label()
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblTitle.Location = New Point(25, 22)
        lblTitle.ForeColor = Color.FromArgb(30, 41, 59) ' Slate 800
        lblTitle.Text = "Report Preview"

        ' Print Button
        btnPrint = New Button()
        btnPrint.Text = "Save as PDF"
        btnPrint.Size = New Size(140, 40)
        btnPrint.BackColor = Color.FromArgb(220, 38, 38) ' Red
        btnPrint.ForeColor = Color.White
        btnPrint.Location = New Point(Me.Width - 180, 15)
        btnPrint.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnPrint.FlatStyle = FlatStyle.Flat
        btnPrint.FlatAppearance.BorderSize = 0
        btnPrint.Font = New Font("Segoe UI Semibold", 10)
        btnPrint.Cursor = Cursors.Hand

        ' --- Zoom Controls ---
        btnZoomIn = New Button()
        btnZoomIn.Text = "+"
        btnZoomIn.Size = New Size(40, 40)
        btnZoomIn.Location = New Point(Me.Width - 360, 15)
        btnZoomIn.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnZoomIn.FlatStyle = FlatStyle.Flat
        btnZoomIn.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnZoomIn.Cursor = Cursors.Hand
        AddHandler btnZoomIn.Click, Sub() AdjustZoom(0.1)

        btnZoomOut = New Button()
        btnZoomOut.Text = "-"
        btnZoomOut.Size = New Size(40, 40)
        btnZoomOut.Location = New Point(Me.Width - 460, 15)
        btnZoomOut.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnZoomOut.FlatStyle = FlatStyle.Flat
        btnZoomOut.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnZoomOut.Cursor = Cursors.Hand
        AddHandler btnZoomOut.Click, Sub() AdjustZoom(-0.1)

        lblZoom = New Label()
        lblZoom.AutoSize = False
        lblZoom.TextAlign = ContentAlignment.MiddleCenter
        lblZoom.Size = New Size(60, 40)
        lblZoom.Location = New Point(Me.Width - 420, 15)
        lblZoom.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblZoom.Font = New Font("Segoe UI", 10)
        lblZoom.Text = "100%"

        headerPanel.Controls.Add(lblTitle)
        headerPanel.Controls.Add(btnPrint)
        headerPanel.Controls.Add(btnZoomIn)
        headerPanel.Controls.Add(btnZoomOut)
        headerPanel.Controls.Add(lblZoom)
        
        ' Separator Line
        separatorLine = New Panel()
        separatorLine.Dock = DockStyle.Top
        separatorLine.Height = 1
        separatorLine.BackColor = Color.FromArgb(226, 232, 240) ' Slate 200

        ' 2. Preview Control
        previewControl = New PrintPreviewControl()
        previewControl.Dock = DockStyle.Fill
        previewControl.BackColor = Color.FromArgb(241, 245, 249) ' Slate 100 bg for contrast
        previewControl.Zoom = 1.0
        previewControl.StartPage = 0
        previewControl.Columns = 1
        previewControl.Rows = 1
        previewControl.UseAntiAlias = True
        
        ' Enable double buffering for smoother scroll
        Dim pi = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
        pi?.SetValue(previewControl, True, Nothing)

        AddHandler previewControl.MouseWheel, AddressOf OnMouseWheel
        
        ' 3. Loading/Progress Panel (Centered Overlay)
        loadingPanel = New Panel()
        loadingPanel.Size = New Size(300, 100)
        loadingPanel.BackColor = Color.White
        ' Center manually in constructor or resize, but here we can try standard centering logic
        loadingPanel.Location = New Point((Me.Width - 300) \ 2, (Me.Height - 100) \ 2)
        loadingPanel.Anchor = AnchorStyles.None
        loadingPanel.BorderStyle = BorderStyle.FixedSingle
        
        Dim lblLoading As New Label()
        lblLoading.Text = "Generating Report..."
        lblLoading.AutoSize = True
        lblLoading.Font = New Font("Segoe UI Semibold", 12)
        lblLoading.Location = New Point(70, 20)
        
        Dim pBar As New ProgressBar()
        pBar.Style = ProgressBarStyle.Marquee
        pBar.Size = New Size(240, 20)
        pBar.Location = New Point(30, 55)
        
        loadingPanel.Controls.Add(lblLoading)
        loadingPanel.Controls.Add(pBar)

        ' Add Controls (Order matters for Dock)
        ' Add loadingPanel last so it is on top? No, Controls.Add adds to beginning of Z-order usually?
        ' Let's use BringToFront later
        Me.Controls.Add(loadingPanel)
        Me.Controls.Add(previewControl)
        Me.Controls.Add(separatorLine)
        Me.Controls.Add(headerPanel)
        
        loadingPanel.BringToFront()
    End Sub

    Private Sub AdjustZoom(delta As Double)
        Dim newZoom As Double = previewControl.Zoom + delta
        If newZoom < 0.3 Then newZoom = 0.3
        If newZoom > 3.0 Then newZoom = 3.0
        previewControl.Zoom = newZoom
        UpdateZoomLabel()
    End Sub

    Private Sub UpdateZoomLabel()
        lblZoom.Text = Math.Round(previewControl.Zoom * 100).ToString() & "%"
    End Sub

    Private Sub OnMouseWheel(sender As Object, e As MouseEventArgs)
        ' Intercept mouse wheel to scroll or change page
        ' PrintPreviewControl is a bit picky about scrolling via wheel if not focused correctly
        Try
            ' If no modifiers, scroll page or change page if at boundary
            ' For simplicity, we just change StartPage if zoomed out, or scroll if zoomed in
            If previewControl.Zoom <= 1.0 Then
                If e.Delta < 0 Then
                    ' Scroll Down / Next Page
                    If previewControl.StartPage < 100 Then ' Arbitrary limit
                        previewControl.StartPage += 1
                    End If
                Else
                    ' Scroll Up / Prev Page
                    If previewControl.StartPage > 0 Then
                        previewControl.StartPage -= 1
                    End If
                End If
            Else
                ' Zoomed in: We need to send WM_VSCROLL messages to the control
                ' For now, simpler approach: change page
                 If e.Delta < 0 Then
                    previewControl.StartPage = Math.Min(100, previewControl.StartPage + 1)
                Else
                    previewControl.StartPage = Math.Max(0, previewControl.StartPage - 1)
                End If
            End If
        Catch
        End Try
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        
        Select Case e.KeyCode
            Case Keys.PageDown, Keys.Right, Keys.Down
                previewControl.StartPage += 1
                e.Handled = True
            Case Keys.PageUp, Keys.Left, Keys.Up
                If previewControl.StartPage > 0 Then
                    previewControl.StartPage -= 1
                End If
                e.Handled = True
            Case Keys.Home
                previewControl.StartPage = 0
                e.Handled = True
            Case Keys.Add, Keys.Oemplus
                If e.Control Then AdjustZoom(0.1)
            Case Keys.Subtract, Keys.OemMinus
                If e.Control Then AdjustZoom(-0.1)
        End Select
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        ' Attempt to default to Microsoft Print to PDF if available
        Dim printerName As String = "Microsoft Print to PDF"
        Dim printerSettings As New PrinterSettings()
        
        Dim pdfPrinterExists As Boolean = printerSettings.InstalledPrinters.Cast(Of String)().Any(Function(p) p.Equals(printerName, StringComparison.OrdinalIgnoreCase))
        
        If pdfPrinterExists Then
             _doc.PrinterSettings.PrinterName = printerName
             _doc.PrinterSettings.PrintToFile = True
             
             ' Prompt for filename
             Dim sfd As New SaveFileDialog()
             sfd.Filter = "PDF Document|*.pdf"
             sfd.Title = "Save Report as PDF"
             sfd.FileName = "Report_" & DateTime.Now.ToString("yyyyMMdd_HHmm") & ".pdf"
             
             If sfd.ShowDialog() = DialogResult.OK Then
                 _doc.PrinterSettings.PrintFileName = sfd.FileName
                 
                 ' Show loading panel again
                 loadingPanel.Visible = True
                 _doc.Print()
             End If
        Else
            ' Fallback to standard print dialog if PDF printer not found
            Dim pd As New PrintDialog()
            pd.Document = _doc
            pd.UseEXDialog = True 
            
            If pd.ShowDialog() = DialogResult.OK Then
                 _doc.Print()
            End If
        End If
    End Sub
End Class
