<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Payroll
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Payroll))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Employee = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Position = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Hours = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HourlyRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Overtime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GrossPay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NetPay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Actions = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.txtSearch = New InformationManagement.RoundedPane2()
        Me.TextBoxSearch = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PaginationPanel = New System.Windows.Forms.Panel()
        Me.btnFirstPage = New System.Windows.Forms.Button()
        Me.btnPreviousPage = New System.Windows.Forms.Button()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.btnLastPage = New System.Windows.Forms.Button()
        Me.RoundedPane24 = New InformationManagement.RoundedPane2()
        Me.RoundedPane27 = New InformationManagement.RoundedPane2()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.E = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.RoundedPane23 = New InformationManagement.RoundedPane2()
        Me.RoundedPane26 = New InformationManagement.RoundedPane2()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.TotalHours = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.RoundedPane22 = New InformationManagement.RoundedPane2()
        Me.RoundedPane25 = New InformationManagement.RoundedPane2()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.lblTotalNetPay = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.RoundedPane221 = New InformationManagement.RoundedPane2()
        Me.PictureBox11 = New System.Windows.Forms.PictureBox()
        Me.lblTotalGrossPay = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.AddNewPayrollRecordbtn = New InformationManagement.RoundedButton()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.txtSearch.SuspendLayout()
        Me.PaginationPanel.SuspendLayout()
        Me.RoundedPane24.SuspendLayout()
        Me.RoundedPane27.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane23.SuspendLayout()
        Me.RoundedPane26.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane22.SuspendLayout()
        Me.RoundedPane25.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane21.SuspendLayout()
        Me.RoundedPane221.SuspendLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(29, 26)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(304, 40)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Payroll Management"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeight = 40
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Employee, Me.Position, Me.Hours, Me.HourlyRate, Me.Overtime, Me.GrossPay, Me.NetPay, Me.Status, Me.Actions})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.Location = New System.Drawing.Point(34, 276)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.Size = New System.Drawing.Size(1039, 422)
        Me.DataGridView1.TabIndex = 4
        '
        'Employee
        '
        Me.Employee.Frozen = True
        Me.Employee.HeaderText = "Employee"
        Me.Employee.MinimumWidth = 6
        Me.Employee.Name = "Employee"
        Me.Employee.ReadOnly = True
        Me.Employee.Width = 150
        '
        'Position
        '
        Me.Position.Frozen = True
        Me.Position.HeaderText = "Position"
        Me.Position.MinimumWidth = 6
        Me.Position.Name = "Position"
        Me.Position.ReadOnly = True
        Me.Position.Width = 120
        '
        'Hours
        '
        Me.Hours.Frozen = True
        Me.Hours.HeaderText = "Hours"
        Me.Hours.MinimumWidth = 6
        Me.Hours.Name = "Hours"
        Me.Hours.ReadOnly = True
        Me.Hours.Width = 125
        '
        'HourlyRate
        '
        Me.HourlyRate.Frozen = True
        Me.HourlyRate.HeaderText = "Hourly Rate"
        Me.HourlyRate.MinimumWidth = 6
        Me.HourlyRate.Name = "HourlyRate"
        Me.HourlyRate.ReadOnly = True
        Me.HourlyRate.Width = 150
        '
        'Overtime
        '
        Me.Overtime.Frozen = True
        Me.Overtime.HeaderText = "Overtime"
        Me.Overtime.MinimumWidth = 6
        Me.Overtime.Name = "Overtime"
        Me.Overtime.ReadOnly = True
        Me.Overtime.Width = 125
        '
        'GrossPay
        '
        Me.GrossPay.Frozen = True
        Me.GrossPay.HeaderText = "Gross Pay"
        Me.GrossPay.MinimumWidth = 6
        Me.GrossPay.Name = "GrossPay"
        Me.GrossPay.ReadOnly = True
        Me.GrossPay.Width = 120
        '
        'NetPay
        '
        Me.NetPay.Frozen = True
        Me.NetPay.HeaderText = "Net Pay"
        Me.NetPay.MinimumWidth = 6
        Me.NetPay.Name = "NetPay"
        Me.NetPay.ReadOnly = True
        Me.NetPay.Width = 120
        '
        'Status
        '
        Me.Status.Frozen = True
        Me.Status.HeaderText = "Status"
        Me.Status.MinimumWidth = 6
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        Me.Status.Width = 120
        '
        'Actions
        '
        Me.Actions.HeaderText = "Actions"
        Me.Actions.MinimumWidth = 6
        Me.Actions.Name = "Actions"
        Me.Actions.ReadOnly = True
        Me.Actions.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Actions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Actions.Width = 150
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(870, 236)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Pay Period:"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Dec 1-15, 2024", "Nov 16-30, 2025", "Nov 1-15, 2024"})
        Me.ComboBox1.Location = New System.Drawing.Point(952, 236)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox1.TabIndex = 7
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.Transparent
        Me.txtSearch.BorderColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtSearch.BorderThickness = 1
        Me.txtSearch.Controls.Add(Me.TextBoxSearch)
        Me.txtSearch.CornerRadius = 10
        Me.txtSearch.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(94, 226)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(382, 38)
        Me.txtSearch.TabIndex = 8
        '
        'TextBoxSearch
        '
        Me.TextBoxSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.TextBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TextBoxSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.TextBoxSearch.Location = New System.Drawing.Point(15, 10)
        Me.TextBoxSearch.Name = "TextBoxSearch"
        Me.TextBoxSearch.Size = New System.Drawing.Size(350, 18)
        Me.TextBoxSearch.TabIndex = 0
        Me.TextBoxSearch.Text = "Search payroll..."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(38, 237)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 17)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Search:"
        '
        'PaginationPanel
        '
        Me.PaginationPanel.BackColor = System.Drawing.Color.White
        Me.PaginationPanel.Controls.Add(Me.btnFirstPage)
        Me.PaginationPanel.Controls.Add(Me.btnPreviousPage)
        Me.PaginationPanel.Controls.Add(Me.lblPageInfo)
        Me.PaginationPanel.Controls.Add(Me.btnNextPage)
        Me.PaginationPanel.Controls.Add(Me.btnLastPage)
        Me.PaginationPanel.Location = New System.Drawing.Point(34, 716)
        Me.PaginationPanel.Name = "PaginationPanel"
        Me.PaginationPanel.Size = New System.Drawing.Size(1039, 50)
        Me.PaginationPanel.TabIndex = 10
        '
        'btnFirstPage
        '
        Me.btnFirstPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnFirstPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFirstPage.FlatAppearance.BorderSize = 0
        Me.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFirstPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFirstPage.Location = New System.Drawing.Point(340, 10)
        Me.btnFirstPage.Name = "btnFirstPage"
        Me.btnFirstPage.Size = New System.Drawing.Size(70, 30)
        Me.btnFirstPage.TabIndex = 0
        Me.btnFirstPage.Text = "First"
        Me.btnFirstPage.UseVisualStyleBackColor = False
        '
        'btnPreviousPage
        '
        Me.btnPreviousPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnPreviousPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPreviousPage.FlatAppearance.BorderSize = 0
        Me.btnPreviousPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPreviousPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPreviousPage.Location = New System.Drawing.Point(420, 10)
        Me.btnPreviousPage.Name = "btnPreviousPage"
        Me.btnPreviousPage.Size = New System.Drawing.Size(70, 30)
        Me.btnPreviousPage.TabIndex = 1
        Me.btnPreviousPage.Text = "Prev"
        Me.btnPreviousPage.UseVisualStyleBackColor = False
        '
        'lblPageInfo
        '
        Me.lblPageInfo.AutoSize = True
        Me.lblPageInfo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPageInfo.Location = New System.Drawing.Point(500, 16)
        Me.lblPageInfo.MinimumSize = New System.Drawing.Size(80, 0)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(80, 17)
        Me.lblPageInfo.TabIndex = 2
        Me.lblPageInfo.Text = "Page 1 of 1"
        Me.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnNextPage
        '
        Me.btnNextPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnNextPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNextPage.FlatAppearance.BorderSize = 0
        Me.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNextPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextPage.Location = New System.Drawing.Point(590, 10)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(70, 30)
        Me.btnNextPage.TabIndex = 3
        Me.btnNextPage.Text = "Next"
        Me.btnNextPage.UseVisualStyleBackColor = False
        '
        'btnLastPage
        '
        Me.btnLastPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnLastPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLastPage.FlatAppearance.BorderSize = 0
        Me.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLastPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLastPage.Location = New System.Drawing.Point(670, 10)
        Me.btnLastPage.Name = "btnLastPage"
        Me.btnLastPage.Size = New System.Drawing.Size(70, 30)
        Me.btnLastPage.TabIndex = 4
        Me.btnLastPage.Text = "Last"
        Me.btnLastPage.UseVisualStyleBackColor = False
        '
        'RoundedPane24
        '
        Me.RoundedPane24.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane24.BorderThickness = 1
        Me.RoundedPane24.Controls.Add(Me.RoundedPane27)
        Me.RoundedPane24.Controls.Add(Me.PictureBox4)
        Me.RoundedPane24.Controls.Add(Me.E)
        Me.RoundedPane24.Controls.Add(Me.Label5)
        Me.RoundedPane24.CornerRadius = 15
        Me.RoundedPane24.FillColor = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.RoundedPane24.ForeColor = System.Drawing.Color.White
        Me.RoundedPane24.Location = New System.Drawing.Point(824, 86)
        Me.RoundedPane24.Name = "RoundedPane24"
        Me.RoundedPane24.Size = New System.Drawing.Size(249, 123)
        Me.RoundedPane24.TabIndex = 13
        '
        'RoundedPane27
        '
        Me.RoundedPane27.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane27.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane27.BorderThickness = 1
        Me.RoundedPane27.Controls.Add(Me.PictureBox6)
        Me.RoundedPane27.CornerRadius = 8
        Me.RoundedPane27.FillColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(132, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.RoundedPane27.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane27.Name = "RoundedPane27"
        Me.RoundedPane27.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane27.TabIndex = 10
        '
        'PictureBox6
        '
        Me.PictureBox6.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox6.TabIndex = 4
        Me.PictureBox6.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(275, 23)
        Me.PictureBox4.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(33, 23)
        Me.PictureBox4.TabIndex = 1
        Me.PictureBox4.TabStop = False
        '
        'E
        '
        Me.E.AutoSize = True
        Me.E.BackColor = System.Drawing.Color.Transparent
        Me.E.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold)
        Me.E.Location = New System.Drawing.Point(18, 72)
        Me.E.Name = "E"
        Me.E.Size = New System.Drawing.Size(25, 30)
        Me.E.TabIndex = 2
        Me.E.Text = "3"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label5.Location = New System.Drawing.Point(72, 31)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 17)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Employees"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RoundedPane23
        '
        Me.RoundedPane23.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane23.BorderThickness = 1
        Me.RoundedPane23.Controls.Add(Me.RoundedPane26)
        Me.RoundedPane23.Controls.Add(Me.PictureBox5)
        Me.RoundedPane23.Controls.Add(Me.TotalHours)
        Me.RoundedPane23.Controls.Add(Me.Label11)
        Me.RoundedPane23.CornerRadius = 15
        Me.RoundedPane23.FillColor = System.Drawing.Color.FromArgb(CType(CType(14, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.RoundedPane23.Location = New System.Drawing.Point(561, 86)
        Me.RoundedPane23.Name = "RoundedPane23"
        Me.RoundedPane23.Size = New System.Drawing.Size(249, 123)
        Me.RoundedPane23.TabIndex = 12
        '
        'RoundedPane26
        '
        Me.RoundedPane26.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane26.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane26.BorderThickness = 1
        Me.RoundedPane26.Controls.Add(Me.PictureBox3)
        Me.RoundedPane26.CornerRadius = 8
        Me.RoundedPane26.FillColor = System.Drawing.Color.FromArgb(CType(CType(56, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.RoundedPane26.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane26.Name = "RoundedPane26"
        Me.RoundedPane26.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane26.TabIndex = 10
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox3.TabIndex = 4
        Me.PictureBox3.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(206, 20)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(25, 19)
        Me.PictureBox5.TabIndex = 1
        Me.PictureBox5.TabStop = False
        '
        'TotalHours
        '
        Me.TotalHours.AutoSize = True
        Me.TotalHours.BackColor = System.Drawing.Color.Transparent
        Me.TotalHours.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold)
        Me.TotalHours.ForeColor = System.Drawing.Color.White
        Me.TotalHours.Location = New System.Drawing.Point(50, 72)
        Me.TotalHours.Name = "TotalHours"
        Me.TotalHours.Size = New System.Drawing.Size(49, 30)
        Me.TotalHours.TabIndex = 2
        Me.TotalHours.Text = "120"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label11.Location = New System.Drawing.Point(72, 31)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(78, 17)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Total Hours"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RoundedPane22
        '
        Me.RoundedPane22.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane22.BorderThickness = 1
        Me.RoundedPane22.Controls.Add(Me.RoundedPane25)
        Me.RoundedPane22.Controls.Add(Me.PictureBox2)
        Me.RoundedPane22.Controls.Add(Me.lblTotalNetPay)
        Me.RoundedPane22.Controls.Add(Me.Label9)
        Me.RoundedPane22.CornerRadius = 15
        Me.RoundedPane22.FillColor = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(129, Byte), Integer))
        Me.RoundedPane22.Location = New System.Drawing.Point(297, 86)
        Me.RoundedPane22.Name = "RoundedPane22"
        Me.RoundedPane22.Size = New System.Drawing.Size(249, 123)
        Me.RoundedPane22.TabIndex = 11
        '
        'RoundedPane25
        '
        Me.RoundedPane25.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane25.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane25.BorderThickness = 1
        Me.RoundedPane25.Controls.Add(Me.PictureBox1)
        Me.RoundedPane25.CornerRadius = 8
        Me.RoundedPane25.FillColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(153, Byte), Integer))
        Me.RoundedPane25.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane25.Name = "RoundedPane25"
        Me.RoundedPane25.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane25.TabIndex = 9
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
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(206, 20)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(25, 19)
        Me.PictureBox2.TabIndex = 1
        Me.PictureBox2.TabStop = False
        '
        'lblTotalNetPay
        '
        Me.lblTotalNetPay.AutoSize = True
        Me.lblTotalNetPay.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalNetPay.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTotalNetPay.ForeColor = System.Drawing.Color.White
        Me.lblTotalNetPay.Location = New System.Drawing.Point(18, 72)
        Me.lblTotalNetPay.Name = "lblTotalNetPay"
        Me.lblTotalNetPay.Size = New System.Drawing.Size(134, 30)
        Me.lblTotalNetPay.TabIndex = 2
        Me.lblTotalNetPay.Text = "₱104,681.25"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label9.Location = New System.Drawing.Point(72, 31)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 17)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Total Net Pay"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RoundedPane21
        '
        Me.RoundedPane21.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.RoundedPane221)
        Me.RoundedPane21.Controls.Add(Me.lblTotalGrossPay)
        Me.RoundedPane21.Controls.Add(Me.Label2)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.RoundedPane21.Location = New System.Drawing.Point(34, 86)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(249, 123)
        Me.RoundedPane21.TabIndex = 10
        '
        'RoundedPane221
        '
        Me.RoundedPane221.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane221.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane221.BorderThickness = 1
        Me.RoundedPane221.Controls.Add(Me.PictureBox11)
        Me.RoundedPane221.CornerRadius = 8
        Me.RoundedPane221.FillColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.RoundedPane221.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane221.Name = "RoundedPane221"
        Me.RoundedPane221.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane221.TabIndex = 7
        '
        'PictureBox11
        '
        Me.PictureBox11.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox11.Image = CType(resources.GetObject("PictureBox11.Image"), System.Drawing.Image)
        Me.PictureBox11.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox11.Name = "PictureBox11"
        Me.PictureBox11.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox11.TabIndex = 4
        Me.PictureBox11.TabStop = False
        '
        'lblTotalGrossPay
        '
        Me.lblTotalGrossPay.AutoSize = True
        Me.lblTotalGrossPay.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalGrossPay.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalGrossPay.ForeColor = System.Drawing.Color.White
        Me.lblTotalGrossPay.Location = New System.Drawing.Point(18, 72)
        Me.lblTotalGrossPay.Name = "lblTotalGrossPay"
        Me.lblTotalGrossPay.Size = New System.Drawing.Size(134, 30)
        Me.lblTotalGrossPay.TabIndex = 2
        Me.lblTotalGrossPay.Text = "₱126,125.00"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Location = New System.Drawing.Point(72, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Total Gross Pay"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AddNewPayrollRecordbtn
        '
        Me.AddNewPayrollRecordbtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.AddNewPayrollRecordbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddNewPayrollRecordbtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(254, Byte))
        Me.AddNewPayrollRecordbtn.ForeColor = System.Drawing.Color.White
        Me.AddNewPayrollRecordbtn.Image = CType(resources.GetObject("AddNewPayrollRecordbtn.Image"), System.Drawing.Image)
        Me.AddNewPayrollRecordbtn.Location = New System.Drawing.Point(834, 25)
        Me.AddNewPayrollRecordbtn.Name = "AddNewPayrollRecordbtn"
        Me.AddNewPayrollRecordbtn.Size = New System.Drawing.Size(239, 41)
        Me.AddNewPayrollRecordbtn.TabIndex = 3
        Me.AddNewPayrollRecordbtn.Text = "   Add New Payroll Record"
        Me.AddNewPayrollRecordbtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AddNewPayrollRecordbtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.AddNewPayrollRecordbtn.UseVisualStyleBackColor = False
        '
        'Payroll
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1252, 609)
        Me.Controls.Add(Me.PaginationPanel)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.RoundedPane24)
        Me.Controls.Add(Me.RoundedPane23)
        Me.Controls.Add(Me.RoundedPane22)
        Me.Controls.Add(Me.RoundedPane21)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.AddNewPayrollRecordbtn)
        Me.Controls.Add(Me.Label1)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Payroll"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Payroll"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.txtSearch.ResumeLayout(False)
        Me.txtSearch.PerformLayout()
        Me.PaginationPanel.ResumeLayout(False)
        Me.PaginationPanel.PerformLayout()
        Me.RoundedPane24.ResumeLayout(False)
        Me.RoundedPane24.PerformLayout()
        Me.RoundedPane27.ResumeLayout(False)
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane23.ResumeLayout(False)
        Me.RoundedPane23.PerformLayout()
        Me.RoundedPane26.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane22.ResumeLayout(False)
        Me.RoundedPane22.PerformLayout()
        Me.RoundedPane25.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        Me.RoundedPane221.ResumeLayout(False)
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents AddNewPayrollRecordbtn As RoundedButton
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents RoundedPane21 As RoundedPane2
    Friend WithEvents lblTotalGrossPay As Label
    Friend WithEvents RoundedPane22 As RoundedPane2
    Friend WithEvents lblTotalNetPay As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents RoundedPane23 As RoundedPane2
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents TotalHours As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents RoundedPane24 As RoundedPane2
    Friend WithEvents E As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Employee As DataGridViewTextBoxColumn
    Friend WithEvents Position As DataGridViewTextBoxColumn
    Friend WithEvents Hours As DataGridViewTextBoxColumn
    Friend WithEvents HourlyRate As DataGridViewTextBoxColumn
    Friend WithEvents Overtime As DataGridViewTextBoxColumn
    Friend WithEvents GrossPay As DataGridViewTextBoxColumn
    Friend WithEvents NetPay As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents Actions As DataGridViewButtonColumn
    Friend WithEvents txtSearch As InformationManagement.RoundedPane2
    Friend WithEvents TextBoxSearch As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents PaginationPanel As Panel
    Friend WithEvents btnFirstPage As Button
    Friend WithEvents btnPreviousPage As Button
    Friend WithEvents lblPageInfo As Label
    Friend WithEvents btnNextPage As Button
    Friend WithEvents btnLastPage As Button
    Friend WithEvents RoundedPane221 As RoundedPane2
    Friend WithEvents PictureBox11 As PictureBox
    Friend WithEvents RoundedPane25 As RoundedPane2
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents RoundedPane26 As RoundedPane2
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents RoundedPane27 As RoundedPane2
    Friend WithEvents PictureBox6 As PictureBox
End Class