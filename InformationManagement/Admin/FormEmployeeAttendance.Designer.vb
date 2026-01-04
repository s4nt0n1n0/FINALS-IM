<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEmployeeAttendance
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
                ' Dispose the search debounce timer if it exists
                If _searchDebounceTimer IsNot Nothing Then
                    _searchDebounceTimer.Stop()
                    _searchDebounceTimer.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormEmployeeAttendance))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.RoundedPane28 = New InformationManagement.RoundedPane2()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RoundedPane22 = New InformationManagement.RoundedPane2()
        Me.RoundedPane29 = New InformationManagement.RoundedPane2()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RoundedPane23 = New InformationManagement.RoundedPane2()
        Me.RoundedPane27 = New InformationManagement.RoundedPane2()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RoundedPane24 = New InformationManagement.RoundedPane2()
        Me.PaginationContainer = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.lblPageStatus = New System.Windows.Forms.Label()
        Me.SearchContainer = New InformationManagement.RoundedPane2()
        Me.TextBoxSearch = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.LabelSubHeader = New System.Windows.Forms.Label()
        Me.RoundedPane21.SuspendLayout()
        Me.RoundedPane28.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane22.SuspendLayout()
        Me.RoundedPane29.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane23.SuspendLayout()
        Me.RoundedPane27.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane24.SuspendLayout()
        Me.PaginationContainer.SuspendLayout()
        Me.SearchContainer.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RoundedPane21
        '
        Me.RoundedPane21.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.RoundedPane28)
        Me.RoundedPane21.Controls.Add(Me.Label5)
        Me.RoundedPane21.Controls.Add(Me.Label4)
        Me.RoundedPane21.Controls.Add(Me.Label1)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.RoundedPane21.Location = New System.Drawing.Point(30, 105)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane21.TabIndex = 3
        '
        'RoundedPane28
        '
        Me.RoundedPane28.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane28.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane28.BorderThickness = 1
        Me.RoundedPane28.Controls.Add(Me.PictureBox1)
        Me.RoundedPane28.CornerRadius = 8
        Me.RoundedPane28.FillColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.RoundedPane28.Location = New System.Drawing.Point(24, 35)
        Me.RoundedPane28.Name = "RoundedPane28"
        Me.RoundedPane28.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane28.TabIndex = 13
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(75, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 20)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "On Duty Today"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(75, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 41)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "18"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(75, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Present"
        '
        'RoundedPane22
        '
        Me.RoundedPane22.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane22.BorderThickness = 1
        Me.RoundedPane22.Controls.Add(Me.RoundedPane29)
        Me.RoundedPane22.Controls.Add(Me.Label8)
        Me.RoundedPane22.Controls.Add(Me.Label6)
        Me.RoundedPane22.Controls.Add(Me.Label2)
        Me.RoundedPane22.CornerRadius = 15
        Me.RoundedPane22.FillColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer))
        Me.RoundedPane22.Location = New System.Drawing.Point(384, 105)
        Me.RoundedPane22.Name = "RoundedPane22"
        Me.RoundedPane22.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane22.TabIndex = 4
        '
        'RoundedPane29
        '
        Me.RoundedPane29.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane29.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane29.BorderThickness = 1
        Me.RoundedPane29.Controls.Add(Me.PictureBox4)
        Me.RoundedPane29.CornerRadius = 8
        Me.RoundedPane29.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(113, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.RoundedPane29.Location = New System.Drawing.Point(22, 42)
        Me.RoundedPane29.Name = "RoundedPane29"
        Me.RoundedPane29.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane29.TabIndex = 13
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(9, 7)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox4.TabIndex = 4
        Me.PictureBox4.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(75, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(109, 20)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Not in building"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(75, 75)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 41)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "2"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(75, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 25)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Absences"
        '
        'RoundedPane23
        '
        Me.RoundedPane23.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane23.BorderThickness = 1
        Me.RoundedPane23.Controls.Add(Me.RoundedPane27)
        Me.RoundedPane23.Controls.Add(Me.Label9)
        Me.RoundedPane23.Controls.Add(Me.Label7)
        Me.RoundedPane23.Controls.Add(Me.Label3)
        Me.RoundedPane23.CornerRadius = 15
        Me.RoundedPane23.FillColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(133, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.RoundedPane23.Location = New System.Drawing.Point(738, 105)
        Me.RoundedPane23.Name = "RoundedPane23"
        Me.RoundedPane23.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane23.TabIndex = 5
        '
        'RoundedPane27
        '
        Me.RoundedPane27.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane27.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane27.BorderThickness = 1
        Me.RoundedPane27.Controls.Add(Me.PictureBox2)
        Me.RoundedPane27.CornerRadius = 8
        Me.RoundedPane27.FillColor = System.Drawing.Color.FromArgb(CType(CType(96, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.RoundedPane27.Location = New System.Drawing.Point(26, 39)
        Me.RoundedPane27.Name = "RoundedPane27"
        Me.RoundedPane27.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane27.TabIndex = 13
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox2.TabIndex = 4
        Me.PictureBox2.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(75, 50)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(117, 20)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Approved Leave"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(75, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 41)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "4"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(75, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 25)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "On Leave"
        '
        'RoundedPane24
        '
        Me.RoundedPane24.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane24.BorderThickness = 1
        Me.RoundedPane24.Controls.Add(Me.PaginationContainer)
        Me.RoundedPane24.Controls.Add(Me.SearchContainer)

        Me.RoundedPane24.Controls.Add(Me.Label10)
        Me.RoundedPane24.Controls.Add(Me.DataGridView1)
        Me.RoundedPane24.CornerRadius = 15
        Me.RoundedPane24.FillColor = System.Drawing.Color.White
        Me.RoundedPane24.Location = New System.Drawing.Point(30, 255)
        Me.RoundedPane24.Name = "RoundedPane24"
        Me.RoundedPane24.Size = New System.Drawing.Size(1334, 565)
        Me.RoundedPane24.TabIndex = 7
        '
        'PaginationContainer
        '
        Me.PaginationContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PaginationContainer.Controls.Add(Me.btnNext)
        Me.PaginationContainer.Controls.Add(Me.btnPrev)
        Me.PaginationContainer.Controls.Add(Me.lblPageStatus)
        Me.PaginationContainer.Location = New System.Drawing.Point(24, 513)
        Me.PaginationContainer.Name = "PaginationContainer"
        Me.PaginationContainer.Size = New System.Drawing.Size(1289, 40)
        Me.PaginationContainer.TabIndex = 12
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNext.Location = New System.Drawing.Point(1199, 5)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(80, 30)
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "Next →"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrev.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrev.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnPrev.Location = New System.Drawing.Point(1109, 5)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(80, 30)
        Me.btnPrev.TabIndex = 1
        Me.btnPrev.Text = "← Prev"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'lblPageStatus
        '
        Me.lblPageStatus.AutoSize = True
        Me.lblPageStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPageStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.lblPageStatus.Location = New System.Drawing.Point(0, 10)
        Me.lblPageStatus.Name = "lblPageStatus"
        Me.lblPageStatus.Size = New System.Drawing.Size(158, 20)
        Me.lblPageStatus.TabIndex = 0
        Me.lblPageStatus.Text = "Page 1 of 1 (0 records)"
        '
        'SearchContainer
        '
        Me.SearchContainer.BackColor = System.Drawing.Color.Transparent
        Me.SearchContainer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.SearchContainer.BorderThickness = 1
        Me.SearchContainer.Controls.Add(Me.TextBoxSearch)
        Me.SearchContainer.CornerRadius = 10
        Me.SearchContainer.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.SearchContainer.Location = New System.Drawing.Point(462, 20)
        Me.SearchContainer.Name = "SearchContainer"
        Me.SearchContainer.Size = New System.Drawing.Size(547, 45)
        Me.SearchContainer.TabIndex = 11
        '
        'TextBoxSearch
        '
        Me.TextBoxSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.TextBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSearch.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.TextBoxSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.TextBoxSearch.Location = New System.Drawing.Point(15, 12)
        Me.TextBoxSearch.Name = "TextBoxSearch"
        Me.TextBoxSearch.Size = New System.Drawing.Size(250, 25)
        Me.TextBoxSearch.TabIndex = 0
        Me.TextBoxSearch.Text = "Search employees..."


        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(32, 28)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(202, 30)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "Attendance Details"
        '
        'DataGridView1
        '
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeight = 50
        Me.DataGridView1.Location = New System.Drawing.Point(24, 85)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.Size = New System.Drawing.Size(1289, 422)
        Me.DataGridView1.TabIndex = 10
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoSize = True
        Me.LabelHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 22.0!, System.Drawing.FontStyle.Bold)
        Me.LabelHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.LabelHeader.Location = New System.Drawing.Point(30, 25)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(513, 50)
        Me.LabelHeader.TabIndex = 8
        Me.LabelHeader.Text = "Employee Attendance Report"
        '
        'LabelSubHeader
        '
        Me.LabelSubHeader.AutoSize = True
        Me.LabelSubHeader.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.LabelSubHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.LabelSubHeader.Location = New System.Drawing.Point(32, 68)
        Me.LabelSubHeader.Name = "LabelSubHeader"
        Me.LabelSubHeader.Size = New System.Drawing.Size(451, 25)
        Me.LabelSubHeader.TabIndex = 9
        Me.LabelSubHeader.Text = "Monitor employee presence, absences, and leave status"
        '
        'FormEmployeeAttendance
        '
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1398, 848)
        Me.Controls.Add(Me.LabelSubHeader)
        Me.Controls.Add(Me.LabelHeader)
        Me.Controls.Add(Me.RoundedPane24)
        Me.Controls.Add(Me.RoundedPane23)
        Me.Controls.Add(Me.RoundedPane22)
        Me.Controls.Add(Me.RoundedPane21)
        Me.DoubleBuffered = True
        Me.Name = "FormEmployeeAttendance"
        Me.Text = "Attendance Management"
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        Me.RoundedPane28.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane22.ResumeLayout(False)
        Me.RoundedPane22.PerformLayout()
        Me.RoundedPane29.ResumeLayout(False)
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane23.ResumeLayout(False)
        Me.RoundedPane23.PerformLayout()
        Me.RoundedPane27.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane24.ResumeLayout(False)
        Me.RoundedPane24.PerformLayout()
        Me.PaginationContainer.ResumeLayout(False)
        Me.PaginationContainer.PerformLayout()
        Me.SearchContainer.ResumeLayout(False)
        Me.SearchContainer.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelHeader As Label
    Friend WithEvents LabelSubHeader As Label
    Friend WithEvents RoundedPane21 As RoundedPane2
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents RoundedPane22 As RoundedPane2
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents RoundedPane23 As RoundedPane2
    Friend WithEvents Label9 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents RoundedPane24 As RoundedPane2
    Friend WithEvents SearchContainer As RoundedPane2
    Friend WithEvents TextBoxSearch As TextBox

    Friend WithEvents Label10 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents PaginationContainer As Panel
    Friend WithEvents btnNext As Button
    Friend WithEvents btnPrev As Button
    Friend WithEvents lblPageStatus As Label
    Friend WithEvents RoundedPane29 As RoundedPane2
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents RoundedPane27 As RoundedPane2
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents RoundedPane28 As RoundedPane2
    Friend WithEvents PictureBox1 As PictureBox
End Class