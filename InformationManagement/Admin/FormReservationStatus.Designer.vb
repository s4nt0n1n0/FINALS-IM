<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormReservationStatus
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormReservationStatus))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RoundedPane25 = New InformationManagement.RoundedPane2()
        Me.PaginationContainer = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.lblPageStatus = New System.Windows.Forms.Label()
        Me.searchTextBox1 = New InformationManagement.RoundedPane2()
        Me.TextBoxSearch = New System.Windows.Forms.TextBox()
        Me.dgvDetails = New System.Windows.Forms.DataGridView()
        Me.btnExportPdf = New System.Windows.Forms.Button()
        Me.dtpFilter = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RoundedPane24 = New InformationManagement.RoundedPane2()
        Me.RoundedPane29 = New InformationManagement.RoundedPane2()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblCancelled = New System.Windows.Forms.Label()
        Me.Cancelled = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RoundedPane23 = New InformationManagement.RoundedPane2()
        Me.RoundedPane28 = New InformationManagement.RoundedPane2()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblConfirmed = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Confirmed = New System.Windows.Forms.Label()
        Me.RoundedPane22 = New InformationManagement.RoundedPane2()
        Me.RoundedPane26 = New InformationManagement.RoundedPane2()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.lblPending = New System.Windows.Forms.Label()
        Me.Pending = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.RoundedPane27 = New InformationManagement.RoundedPane2()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.lblTotalReservations = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RoundedPane25.SuspendLayout()
        Me.PaginationContainer.SuspendLayout()
        Me.searchTextBox1.SuspendLayout()
        CType(Me.dgvDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane24.SuspendLayout()
        Me.RoundedPane29.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane23.SuspendLayout()
        Me.RoundedPane28.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane22.SuspendLayout()
        Me.RoundedPane26.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane21.SuspendLayout()
        Me.RoundedPane27.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1248, 749)
        Me.Panel1.TabIndex = 11
        '
        'RoundedPane25
        '
        Me.RoundedPane25.AutoScroll = True
        Me.RoundedPane25.BackColor = System.Drawing.Color.White
        Me.RoundedPane25.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane25.BorderThickness = 1
        Me.RoundedPane25.Controls.Add(Me.PaginationContainer)
        Me.RoundedPane25.Controls.Add(Me.searchTextBox1)
        Me.RoundedPane25.Controls.Add(Me.dgvDetails)
        Me.RoundedPane25.Controls.Add(Me.btnExportPdf)
        Me.RoundedPane25.Controls.Add(Me.dtpFilter)
        Me.RoundedPane25.Controls.Add(Me.Label4)
        Me.RoundedPane25.CornerRadius = 15
        Me.RoundedPane25.FillColor = System.Drawing.Color.White
        Me.RoundedPane25.ForeColor = System.Drawing.Color.LightGray
        Me.RoundedPane25.Location = New System.Drawing.Point(32, 228)
        Me.RoundedPane25.Name = "RoundedPane25"
        Me.RoundedPane25.Size = New System.Drawing.Size(1140, 463)
        Me.RoundedPane25.TabIndex = 10
        '
        'PaginationContainer
        '
        Me.PaginationContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PaginationContainer.Controls.Add(Me.btnNext)
        Me.PaginationContainer.Controls.Add(Me.btnPrev)
        Me.PaginationContainer.Controls.Add(Me.lblPageStatus)
        Me.PaginationContainer.Location = New System.Drawing.Point(28, 411)
        Me.PaginationContainer.Name = "PaginationContainer"
        Me.PaginationContainer.Size = New System.Drawing.Size(945, 40)
        Me.PaginationContainer.TabIndex = 23
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNext.Location = New System.Drawing.Point(1600, 5)
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
        Me.btnPrev.Location = New System.Drawing.Point(1510, 5)
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
        Me.lblPageStatus.Location = New System.Drawing.Point(14, 15)
        Me.lblPageStatus.Name = "lblPageStatus"
        Me.lblPageStatus.Size = New System.Drawing.Size(65, 15)
        Me.lblPageStatus.TabIndex = 0
        Me.lblPageStatus.Text = "Page 1 of 1"
        '
        'searchTextBox1
        '
        Me.searchTextBox1.BackColor = System.Drawing.Color.Transparent
        Me.searchTextBox1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.searchTextBox1.BorderThickness = 1
        Me.searchTextBox1.Controls.Add(Me.TextBoxSearch)
        Me.searchTextBox1.CornerRadius = 10
        Me.searchTextBox1.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.searchTextBox1.Location = New System.Drawing.Point(503, 18)
        Me.searchTextBox1.Name = "searchTextBox1"
        Me.searchTextBox1.Size = New System.Drawing.Size(482, 38)
        Me.searchTextBox1.TabIndex = 22
        '
        'TextBoxSearch
        '
        Me.TextBoxSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.TextBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TextBoxSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.TextBoxSearch.Location = New System.Drawing.Point(15, 10)
        Me.TextBoxSearch.Name = "TextBoxSearch"
        Me.TextBoxSearch.Size = New System.Drawing.Size(250, 18)
        Me.TextBoxSearch.TabIndex = 0
        Me.TextBoxSearch.Text = "Search orders..."
        '
        'dgvDetails
        '
        Me.dgvDetails.AllowUserToAddRows = False
        Me.dgvDetails.AllowUserToDeleteRows = False
        Me.dgvDetails.AllowUserToResizeRows = False
        Me.dgvDetails.BackgroundColor = System.Drawing.Color.White
        Me.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.dgvDetails.ColumnHeadersHeight = 40
        Me.dgvDetails.GridColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.dgvDetails.Location = New System.Drawing.Point(30, 83)
        Me.dgvDetails.Name = "dgvDetails"
        Me.dgvDetails.ReadOnly = True
        Me.dgvDetails.RowHeadersVisible = False
        Me.dgvDetails.RowTemplate.Height = 50
        Me.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDetails.Size = New System.Drawing.Size(1037, 313)
        Me.dgvDetails.TabIndex = 21
        '
        'btnExportPdf
        '
        Me.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(CType(CType(99, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportPdf.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExportPdf.FlatAppearance.BorderSize = 0
        Me.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportPdf.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnExportPdf.ForeColor = System.Drawing.Color.White
        Me.btnExportPdf.Image = CType(resources.GetObject("btnExportPdf.Image"), System.Drawing.Image)
        Me.btnExportPdf.Location = New System.Drawing.Point(991, 17)
        Me.btnExportPdf.Name = "btnExportPdf"
        Me.btnExportPdf.Size = New System.Drawing.Size(135, 38)
        Me.btnExportPdf.TabIndex = 9
        Me.btnExportPdf.Text = "   Export PDF"
        Me.btnExportPdf.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportPdf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnExportPdf.UseVisualStyleBackColor = False
        '
        'dtpFilter
        '
        Me.dtpFilter.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.dtpFilter.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFilter.Location = New System.Drawing.Point(331, 28)
        Me.dtpFilter.Name = "dtpFilter"
        Me.dtpFilter.Size = New System.Drawing.Size(151, 25)
        Me.dtpFilter.TabIndex = 20
        Me.dtpFilter.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(24, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(173, 25)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Reservation Details"
        '
        'RoundedPane24
        '
        Me.RoundedPane24.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane24.BorderThickness = 1
        Me.RoundedPane24.Controls.Add(Me.RoundedPane29)
        Me.RoundedPane24.Controls.Add(Me.lblCancelled)
        Me.RoundedPane24.Controls.Add(Me.Cancelled)
        Me.RoundedPane24.Controls.Add(Me.Label7)
        Me.RoundedPane24.CornerRadius = 15
        Me.RoundedPane24.FillColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer))
        Me.RoundedPane24.Location = New System.Drawing.Point(830, 64)
        Me.RoundedPane24.Name = "RoundedPane24"
        Me.RoundedPane24.Size = New System.Drawing.Size(246, 141)
        Me.RoundedPane24.TabIndex = 9
        '
        'RoundedPane29
        '
        Me.RoundedPane29.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane29.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane29.BorderThickness = 1
        Me.RoundedPane29.Controls.Add(Me.PictureBox3)
        Me.RoundedPane29.CornerRadius = 8
        Me.RoundedPane29.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(113, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.RoundedPane29.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane29.Name = "RoundedPane29"
        Me.RoundedPane29.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane29.TabIndex = 12
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
        'lblCancelled
        '
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.BackColor = System.Drawing.Color.Transparent
        Me.lblCancelled.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelled.ForeColor = System.Drawing.Color.White
        Me.lblCancelled.Location = New System.Drawing.Point(23, 73)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(26, 30)
        Me.lblCancelled.TabIndex = 4
        Me.lblCancelled.Text = "5"
        '
        'Cancelled
        '
        Me.Cancelled.AutoSize = True
        Me.Cancelled.BackColor = System.Drawing.Color.Transparent
        Me.Cancelled.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancelled.ForeColor = System.Drawing.Color.White
        Me.Cancelled.Location = New System.Drawing.Point(73, 32)
        Me.Cancelled.Name = "Cancelled"
        Me.Cancelled.Size = New System.Drawing.Size(65, 17)
        Me.Cancelled.TabIndex = 0
        Me.Cancelled.Text = "Cancelled"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(254, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(25, 107)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Cancellations"
        '
        'RoundedPane23
        '
        Me.RoundedPane23.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane23.BorderThickness = 1
        Me.RoundedPane23.Controls.Add(Me.RoundedPane28)
        Me.RoundedPane23.Controls.Add(Me.lblConfirmed)
        Me.RoundedPane23.Controls.Add(Me.Label5)
        Me.RoundedPane23.Controls.Add(Me.Confirmed)
        Me.RoundedPane23.CornerRadius = 15
        Me.RoundedPane23.FillColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.RoundedPane23.Location = New System.Drawing.Point(563, 64)
        Me.RoundedPane23.Name = "RoundedPane23"
        Me.RoundedPane23.Size = New System.Drawing.Size(246, 141)
        Me.RoundedPane23.TabIndex = 8
        '
        'RoundedPane28
        '
        Me.RoundedPane28.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane28.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane28.BorderThickness = 1
        Me.RoundedPane28.Controls.Add(Me.PictureBox1)
        Me.RoundedPane28.CornerRadius = 8
        Me.RoundedPane28.FillColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.RoundedPane28.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane28.Name = "RoundedPane28"
        Me.RoundedPane28.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane28.TabIndex = 12
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
        'lblConfirmed
        '
        Me.lblConfirmed.AutoSize = True
        Me.lblConfirmed.BackColor = System.Drawing.Color.Transparent
        Me.lblConfirmed.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmed.ForeColor = System.Drawing.Color.White
        Me.lblConfirmed.Location = New System.Drawing.Point(23, 73)
        Me.lblConfirmed.Name = "lblConfirmed"
        Me.lblConfirmed.Size = New System.Drawing.Size(39, 30)
        Me.lblConfirmed.TabIndex = 4
        Me.lblConfirmed.Text = "28"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(254, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(25, 107)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Ready to serve"
        '
        'Confirmed
        '
        Me.Confirmed.AutoSize = True
        Me.Confirmed.BackColor = System.Drawing.Color.Transparent
        Me.Confirmed.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Confirmed.ForeColor = System.Drawing.Color.White
        Me.Confirmed.Location = New System.Drawing.Point(73, 32)
        Me.Confirmed.Name = "Confirmed"
        Me.Confirmed.Size = New System.Drawing.Size(71, 17)
        Me.Confirmed.TabIndex = 0
        Me.Confirmed.Text = "Confirmed"
        '
        'RoundedPane22
        '
        Me.RoundedPane22.AutoScroll = True
        Me.RoundedPane22.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane22.BorderThickness = 1
        Me.RoundedPane22.Controls.Add(Me.RoundedPane26)
        Me.RoundedPane22.Controls.Add(Me.lblPending)
        Me.RoundedPane22.Controls.Add(Me.Pending)
        Me.RoundedPane22.Controls.Add(Me.Label3)
        Me.RoundedPane22.CornerRadius = 15
        Me.RoundedPane22.FillColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.RoundedPane22.Location = New System.Drawing.Point(297, 64)
        Me.RoundedPane22.Name = "RoundedPane22"
        Me.RoundedPane22.Size = New System.Drawing.Size(246, 141)
        Me.RoundedPane22.TabIndex = 7
        '
        'RoundedPane26
        '
        Me.RoundedPane26.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane26.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane26.BorderThickness = 1
        Me.RoundedPane26.Controls.Add(Me.PictureBox5)
        Me.RoundedPane26.CornerRadius = 8
        Me.RoundedPane26.FillColor = System.Drawing.Color.FromArgb(CType(CType(251, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(36, Byte), Integer))
        Me.RoundedPane26.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane26.Name = "RoundedPane26"
        Me.RoundedPane26.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane26.TabIndex = 11
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox5.TabIndex = 4
        Me.PictureBox5.TabStop = False
        '
        'lblPending
        '
        Me.lblPending.AutoSize = True
        Me.lblPending.BackColor = System.Drawing.Color.Transparent
        Me.lblPending.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPending.ForeColor = System.Drawing.Color.Transparent
        Me.lblPending.Location = New System.Drawing.Point(23, 73)
        Me.lblPending.Name = "lblPending"
        Me.lblPending.Size = New System.Drawing.Size(39, 30)
        Me.lblPending.TabIndex = 4
        Me.lblPending.Text = "12"
        '
        'Pending
        '
        Me.Pending.AutoSize = True
        Me.Pending.BackColor = System.Drawing.Color.Transparent
        Me.Pending.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Pending.ForeColor = System.Drawing.Color.White
        Me.Pending.Location = New System.Drawing.Point(73, 32)
        Me.Pending.Name = "Pending"
        Me.Pending.Size = New System.Drawing.Size(58, 17)
        Me.Pending.TabIndex = 0
        Me.Pending.Text = "Pending"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(254, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(25, 107)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Awaiting Confirmation"
        '
        'RoundedPane21
        '
        Me.RoundedPane21.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.RoundedPane27)
        Me.RoundedPane21.Controls.Add(Me.lblTotalReservations)
        Me.RoundedPane21.Controls.Add(Me.Label2)
        Me.RoundedPane21.Controls.Add(Me.Label1)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.RoundedPane21.Location = New System.Drawing.Point(32, 64)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(246, 141)
        Me.RoundedPane21.TabIndex = 6
        '
        'RoundedPane27
        '
        Me.RoundedPane27.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane27.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane27.BorderThickness = 1
        Me.RoundedPane27.Controls.Add(Me.PictureBox2)
        Me.RoundedPane27.CornerRadius = 8
        Me.RoundedPane27.FillColor = System.Drawing.Color.FromArgb(CType(CType(96, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.RoundedPane27.Location = New System.Drawing.Point(21, 20)
        Me.RoundedPane27.Name = "RoundedPane27"
        Me.RoundedPane27.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane27.TabIndex = 12
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
        'lblTotalReservations
        '
        Me.lblTotalReservations.AutoSize = True
        Me.lblTotalReservations.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalReservations.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalReservations.ForeColor = System.Drawing.Color.White
        Me.lblTotalReservations.Location = New System.Drawing.Point(23, 73)
        Me.lblTotalReservations.Name = "lblTotalReservations"
        Me.lblTotalReservations.Size = New System.Drawing.Size(39, 30)
        Me.lblTotalReservations.TabIndex = 3
        Me.lblTotalReservations.Text = "45"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(254, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(25, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "This Month"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(73, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Reservations"
        '
        'FormReservationStatus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1248, 749)
        Me.Controls.Add(Me.RoundedPane25)
        Me.Controls.Add(Me.RoundedPane24)
        Me.Controls.Add(Me.RoundedPane23)
        Me.Controls.Add(Me.RoundedPane22)
        Me.Controls.Add(Me.RoundedPane21)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.Name = "FormReservationStatus"
        Me.Text = "FormReservationStatus"
        Me.RoundedPane25.ResumeLayout(False)
        Me.RoundedPane25.PerformLayout()
        Me.PaginationContainer.ResumeLayout(False)
        Me.PaginationContainer.PerformLayout()
        Me.searchTextBox1.ResumeLayout(False)
        Me.searchTextBox1.PerformLayout()
        CType(Me.dgvDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane24.ResumeLayout(False)
        Me.RoundedPane24.PerformLayout()
        Me.RoundedPane29.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane23.ResumeLayout(False)
        Me.RoundedPane23.PerformLayout()
        Me.RoundedPane28.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane22.ResumeLayout(False)
        Me.RoundedPane22.PerformLayout()
        Me.RoundedPane26.ResumeLayout(False)
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        Me.RoundedPane27.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Pending As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Confirmed As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Cancelled As Label
    Friend WithEvents RoundedPane21 As RoundedPane2
    Friend WithEvents RoundedPane22 As RoundedPane2
    Friend WithEvents RoundedPane23 As RoundedPane2
    Friend WithEvents RoundedPane24 As RoundedPane2
    Friend WithEvents lblTotalReservations As Label
    Friend WithEvents lblPending As Label
    Friend WithEvents lblConfirmed As Label
    Friend WithEvents lblCancelled As Label
    Friend WithEvents RoundedPane25 As RoundedPane2
    Friend WithEvents Label4 As Label
    Friend WithEvents btnExportPdf As Button
    Friend WithEvents dtpFilter As DateTimePicker
    Friend WithEvents Panel1 As Panel

    Friend WithEvents RoundedPane27 As RoundedPane2
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents RoundedPane26 As RoundedPane2
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents RoundedPane28 As RoundedPane2
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents RoundedPane29 As RoundedPane2
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents dgvDetails As DataGridView
    Friend WithEvents searchTextBox1 As RoundedPane2
    Friend WithEvents TextBoxSearch As TextBox
    Friend WithEvents PaginationContainer As Panel
    Friend WithEvents btnNext As Button
    Friend WithEvents btnPrev As Button
    Friend WithEvents lblPageStatus As Label
End Class
