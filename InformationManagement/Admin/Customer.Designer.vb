<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Customer
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
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.LabelSubHeader = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnLastPage = New System.Windows.Forms.Button()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.btnPrevPage = New System.Windows.Forms.Button()
        Me.btnFirstPage = New System.Windows.Forms.Button()
        Me.lblTotalCustomers = New System.Windows.Forms.Label()
        Me.RoundedPane24 = New InformationManagement.RoundedPane2()
        Me.SearchContainer = New InformationManagement.RoundedPane2()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnUpdateStatus = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.btnViewAll = New System.Windows.Forms.Button()
        Me.btnViewInactive = New System.Windows.Forms.Button()
        Me.btnViewSuspended = New System.Windows.Forms.Button()
        Me.btnViewActive = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.RoundedPane23 = New InformationManagement.RoundedPane2()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.RoundedPane22 = New InformationManagement.RoundedPane2()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel4.SuspendLayout()
        Me.RoundedPane24.SuspendLayout()
        Me.SearchContainer.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane23.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane22.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane21.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoSize = True
        Me.LabelHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 22.0!, System.Drawing.FontStyle.Bold)
        Me.LabelHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.LabelHeader.Location = New System.Drawing.Point(30, 25)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(343, 41)
        Me.LabelHeader.TabIndex = 0
        Me.LabelHeader.Text = "Customer Management"
        '
        'LabelSubHeader
        '
        Me.LabelSubHeader.AutoSize = True
        Me.LabelSubHeader.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.LabelSubHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.LabelSubHeader.Location = New System.Drawing.Point(32, 68)
        Me.LabelSubHeader.Name = "LabelSubHeader"
        Me.LabelSubHeader.Size = New System.Drawing.Size(379, 19)
        Me.LabelSubHeader.TabIndex = 1
        Me.LabelSubHeader.Text = "View customer profiles, transaction history, and engagement"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.Controls.Add(Me.lblPageInfo)
        Me.Panel4.Controls.Add(Me.btnLastPage)
        Me.Panel4.Controls.Add(Me.btnNextPage)
        Me.Panel4.Controls.Add(Me.btnPrevPage)
        Me.Panel4.Controls.Add(Me.btnFirstPage)
        Me.Panel4.Controls.Add(Me.lblTotalCustomers)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 699)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1129, 50)
        Me.Panel4.TabIndex = 6
        '
        'lblPageInfo
        '
        Me.lblPageInfo.AutoSize = True
        Me.lblPageInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPageInfo.Location = New System.Drawing.Point(697, 18)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(69, 15)
        Me.lblPageInfo.TabIndex = 5
        Me.lblPageInfo.Text = "Page 1 of 1"
        '
        'btnLastPage
        '
        Me.btnLastPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnLastPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLastPage.FlatAppearance.BorderSize = 0
        Me.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLastPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnLastPage.ForeColor = System.Drawing.Color.Black
        Me.btnLastPage.Location = New System.Drawing.Point(607, 10)
        Me.btnLastPage.Name = "btnLastPage"
        Me.btnLastPage.Size = New System.Drawing.Size(70, 30)
        Me.btnLastPage.TabIndex = 4
        Me.btnLastPage.Text = "Last"
        Me.btnLastPage.UseVisualStyleBackColor = False
        '
        'btnNextPage
        '
        Me.btnNextPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnNextPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNextPage.FlatAppearance.BorderSize = 0
        Me.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNextPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnNextPage.ForeColor = System.Drawing.Color.Black
        Me.btnNextPage.Location = New System.Drawing.Point(531, 10)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(70, 30)
        Me.btnNextPage.TabIndex = 3
        Me.btnNextPage.Text = "Next"
        Me.btnNextPage.UseVisualStyleBackColor = False
        '
        'btnPrevPage
        '
        Me.btnPrevPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnPrevPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPrevPage.FlatAppearance.BorderSize = 0
        Me.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrevPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrevPage.ForeColor = System.Drawing.Color.Black
        Me.btnPrevPage.Location = New System.Drawing.Point(455, 10)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(70, 30)
        Me.btnPrevPage.TabIndex = 2
        Me.btnPrevPage.Text = "Prev"
        Me.btnPrevPage.UseVisualStyleBackColor = False
        '
        'btnFirstPage
        '
        Me.btnFirstPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.btnFirstPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFirstPage.FlatAppearance.BorderSize = 0
        Me.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFirstPage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnFirstPage.ForeColor = System.Drawing.Color.Black
        Me.btnFirstPage.Location = New System.Drawing.Point(379, 10)
        Me.btnFirstPage.Name = "btnFirstPage"
        Me.btnFirstPage.Size = New System.Drawing.Size(70, 30)
        Me.btnFirstPage.TabIndex = 1
        Me.btnFirstPage.Text = "First"
        Me.btnFirstPage.UseVisualStyleBackColor = False
        '
        'lblTotalCustomers
        '
        Me.lblTotalCustomers.AutoSize = True
        Me.lblTotalCustomers.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalCustomers.Location = New System.Drawing.Point(12, 18)
        Me.lblTotalCustomers.Name = "lblTotalCustomers"
        Me.lblTotalCustomers.Size = New System.Drawing.Size(109, 15)
        Me.lblTotalCustomers.TabIndex = 0
        Me.lblTotalCustomers.Text = "Total Customers: 0"
        '
        'RoundedPane24
        '
        Me.RoundedPane24.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RoundedPane24.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane24.BorderThickness = 1
        Me.RoundedPane24.Controls.Add(Me.SearchContainer)
        Me.RoundedPane24.Controls.Add(Me.btnUpdateStatus)
        Me.RoundedPane24.Controls.Add(Me.btnDelete)
        Me.RoundedPane24.Controls.Add(Me.lblFilter)
        Me.RoundedPane24.Controls.Add(Me.btnViewAll)
        Me.RoundedPane24.Controls.Add(Me.btnViewInactive)
        Me.RoundedPane24.Controls.Add(Me.btnViewSuspended)
        Me.RoundedPane24.Controls.Add(Me.btnViewActive)
        Me.RoundedPane24.Controls.Add(Me.Label10)
        Me.RoundedPane24.Controls.Add(Me.DataGridView1)
        Me.RoundedPane24.CornerRadius = 15
        Me.RoundedPane24.FillColor = System.Drawing.Color.White
        Me.RoundedPane24.Location = New System.Drawing.Point(30, 255)
        Me.RoundedPane24.Name = "RoundedPane24"
        Me.RoundedPane24.Size = New System.Drawing.Size(1048, 449)
        Me.RoundedPane24.TabIndex = 5
        '
        'SearchContainer
        '
        Me.SearchContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchContainer.BackColor = System.Drawing.Color.Transparent
        Me.SearchContainer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.SearchContainer.BorderThickness = 1
        Me.SearchContainer.Controls.Add(Me.txtSearch)
        Me.SearchContainer.CornerRadius = 10
        Me.SearchContainer.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.SearchContainer.Location = New System.Drawing.Point(735, 20)
        Me.SearchContainer.Name = "SearchContainer"
        Me.SearchContainer.Size = New System.Drawing.Size(280, 45)
        Me.SearchContainer.TabIndex = 0
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(15, 12)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(250, 20)
        Me.txtSearch.TabIndex = 0
        Me.txtSearch.Text = "Search customers..."
        '
        'btnUpdateStatus
        '
        Me.btnUpdateStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdateStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.btnUpdateStatus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdateStatus.FlatAppearance.BorderSize = 0
        Me.btnUpdateStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdateStatus.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnUpdateStatus.ForeColor = System.Drawing.Color.White
        Me.btnUpdateStatus.Location = New System.Drawing.Point(464, 24)
        Me.btnUpdateStatus.Name = "btnUpdateStatus"
        Me.btnUpdateStatus.Size = New System.Drawing.Size(120, 38)
        Me.btnUpdateStatus.TabIndex = 5
        Me.btnUpdateStatus.Text = "Update Status"
        Me.btnUpdateStatus.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer))
        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelete.FlatAppearance.BorderSize = 0
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(590, 24)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 38)
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "Delete Profile"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblFilter.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblFilter.Location = New System.Drawing.Point(40, 69)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(83, 17)
        Me.lblFilter.TabIndex = 64
        Me.lblFilter.Text = "Filter Status:"
        '
        'btnViewAll
        '
        Me.btnViewAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnViewAll.FlatAppearance.BorderSize = 0
        Me.btnViewAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewAll.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnViewAll.ForeColor = System.Drawing.Color.White
        Me.btnViewAll.Location = New System.Drawing.Point(47, 90)
        Me.btnViewAll.Name = "btnViewAll"
        Me.btnViewAll.Size = New System.Drawing.Size(56, 30)
        Me.btnViewAll.TabIndex = 63
        Me.btnViewAll.Text = "All"
        Me.btnViewAll.UseVisualStyleBackColor = False
        '
        'btnViewInactive
        '
        Me.btnViewInactive.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnViewInactive.FlatAppearance.BorderSize = 0
        Me.btnViewInactive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewInactive.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnViewInactive.ForeColor = System.Drawing.Color.White
        Me.btnViewInactive.Location = New System.Drawing.Point(301, 90)
        Me.btnViewInactive.Name = "btnViewInactive"
        Me.btnViewInactive.Size = New System.Drawing.Size(90, 30)
        Me.btnViewInactive.TabIndex = 62
        Me.btnViewInactive.Text = "Inactive"
        Me.btnViewInactive.UseVisualStyleBackColor = False
        '
        'btnViewSuspended
        '
        Me.btnViewSuspended.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.btnViewSuspended.FlatAppearance.BorderSize = 0
        Me.btnViewSuspended.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewSuspended.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnViewSuspended.ForeColor = System.Drawing.Color.White
        Me.btnViewSuspended.Location = New System.Drawing.Point(205, 90)
        Me.btnViewSuspended.Name = "btnViewSuspended"
        Me.btnViewSuspended.Size = New System.Drawing.Size(90, 30)
        Me.btnViewSuspended.TabIndex = 61
        Me.btnViewSuspended.Text = "Suspended"
        Me.btnViewSuspended.UseVisualStyleBackColor = False
        '
        'btnViewActive
        '
        Me.btnViewActive.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnViewActive.FlatAppearance.BorderSize = 0
        Me.btnViewActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewActive.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnViewActive.ForeColor = System.Drawing.Color.White
        Me.btnViewActive.Location = New System.Drawing.Point(109, 90)
        Me.btnViewActive.Name = "btnViewActive"
        Me.btnViewActive.Size = New System.Drawing.Size(90, 30)
        Me.btnViewActive.TabIndex = 60
        Me.btnViewActive.Text = "Active"
        Me.btnViewActive.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(32, 28)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(176, 25)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Customer Directory"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeight = 40
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.DataGridView1.Location = New System.Drawing.Point(24, 125)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowTemplate.Height = 50
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(1000, 299)
        Me.DataGridView1.TabIndex = 3
        '
        'RoundedPane23
        '
        Me.RoundedPane23.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane23.BorderThickness = 1
        Me.RoundedPane23.Controls.Add(Me.Label9)
        Me.RoundedPane23.Controls.Add(Me.Label7)
        Me.RoundedPane23.Controls.Add(Me.Label3)
        Me.RoundedPane23.Controls.Add(Me.PictureBox3)
        Me.RoundedPane23.CornerRadius = 15
        Me.RoundedPane23.FillColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.RoundedPane23.Location = New System.Drawing.Point(738, 105)
        Me.RoundedPane23.Name = "RoundedPane23"
        Me.RoundedPane23.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane23.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(75, 50)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(102, 15)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Joined this month"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(75, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(28, 32)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(75, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "New Customers"
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.PictureBox3.Image = Global.InformationManagement.My.Resources.Resources.user__7_
        Me.PictureBox3.Location = New System.Drawing.Point(20, 25)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox3.TabIndex = 3
        Me.PictureBox3.TabStop = False
        '
        'RoundedPane22
        '
        Me.RoundedPane22.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane22.BorderThickness = 1
        Me.RoundedPane22.Controls.Add(Me.Label8)
        Me.RoundedPane22.Controls.Add(Me.Label6)
        Me.RoundedPane22.Controls.Add(Me.Label2)
        Me.RoundedPane22.Controls.Add(Me.PictureBox2)
        Me.RoundedPane22.CornerRadius = 15
        Me.RoundedPane22.FillColor = System.Drawing.Color.MediumOrchid
        Me.RoundedPane22.Location = New System.Drawing.Point(384, 105)
        Me.RoundedPane22.Name = "RoundedPane22"
        Me.RoundedPane22.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane22.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(75, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Last 30 days"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(75, 75)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 32)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(75, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Active Customers"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(132, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.PictureBox2.Image = Global.InformationManagement.My.Resources.Resources.user__6_
        Me.PictureBox2.Location = New System.Drawing.Point(20, 25)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox2.TabIndex = 3
        Me.PictureBox2.TabStop = False
        '
        'RoundedPane21
        '
        Me.RoundedPane21.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.Label5)
        Me.RoundedPane21.Controls.Add(Me.Label4)
        Me.RoundedPane21.Controls.Add(Me.Label1)
        Me.RoundedPane21.Controls.Add(Me.PictureBox1)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(133, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.RoundedPane21.Location = New System.Drawing.Point(30, 105)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane21.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(75, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(103, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "All time registered"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(75, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(28, 32)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(75, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Customers"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(102, Byte), Integer), CType(CType(157, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.PictureBox1.Image = Global.InformationManagement.My.Resources.Resources.user__4_
        Me.PictureBox1.Location = New System.Drawing.Point(20, 25)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'Customer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1129, 749)
        Me.Controls.Add(Me.LabelSubHeader)
        Me.Controls.Add(Me.LabelHeader)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.RoundedPane24)
        Me.Controls.Add(Me.RoundedPane23)
        Me.Controls.Add(Me.RoundedPane22)
        Me.Controls.Add(Me.RoundedPane21)
        Me.DoubleBuffered = True
        Me.Name = "Customer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Customer Management"
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.RoundedPane24.ResumeLayout(False)
        Me.RoundedPane24.PerformLayout()
        Me.SearchContainer.ResumeLayout(False)
        Me.SearchContainer.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane23.ResumeLayout(False)
        Me.RoundedPane23.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane22.ResumeLayout(False)
        Me.RoundedPane22.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelHeader As System.Windows.Forms.Label
    Friend WithEvents LabelSubHeader As System.Windows.Forms.Label
    Friend WithEvents RoundedPane21 As InformationManagement.RoundedPane2
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents RoundedPane22 As InformationManagement.RoundedPane2
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents RoundedPane23 As InformationManagement.RoundedPane2
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents RoundedPane24 As InformationManagement.RoundedPane2
    Friend WithEvents SearchContainer As InformationManagement.RoundedPane2
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnUpdateStatus As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblFilter As System.Windows.Forms.Label
    Friend WithEvents btnViewActive As System.Windows.Forms.Button
    Friend WithEvents btnViewSuspended As System.Windows.Forms.Button
    Friend WithEvents btnViewInactive As System.Windows.Forms.Button
    Friend WithEvents btnViewAll As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnFirstPage As System.Windows.Forms.Button
    Friend WithEvents btnPrevPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents btnLastPage As System.Windows.Forms.Button
    Friend WithEvents lblPageInfo As System.Windows.Forms.Label
    Friend WithEvents lblTotalCustomers As System.Windows.Forms.Label
End Class