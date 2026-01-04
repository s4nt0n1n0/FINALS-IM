<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MenuItems
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.AddMenuItemsbtn = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtSearch = New InformationManagement.RoundedPane2()
        Me.TextBoxSearch = New System.Windows.Forms.TextBox()
        Me.btnCheckIngredients = New System.Windows.Forms.Button()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.Category = New System.Windows.Forms.ComboBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnToggleAvailability = New System.Windows.Forms.Button()
        Me.Edit = New System.Windows.Forms.Button()
        Me.Delete = New System.Windows.Forms.Button()
        Me.DataGridMenu = New System.Windows.Forms.DataGridView()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblTotalItems = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.txtSearch.SuspendLayout()
        CType(Me.DataGridMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.GhostWhite
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1370, 79)
        Me.Panel1.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(22, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(373, 40)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Menu Items Management"
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnExport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExport.FlatAppearance.BorderSize = 0
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnExport.ForeColor = System.Drawing.Color.White
        Me.btnExport.Location = New System.Drawing.Point(1199, 10)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(130, 35)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = " Export CSV"
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'AddMenuItemsbtn
        '
        Me.AddMenuItemsbtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.AddMenuItemsbtn.Cursor = System.Windows.Forms.Cursors.Hand
        Me.AddMenuItemsbtn.FlatAppearance.BorderSize = 0
        Me.AddMenuItemsbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddMenuItemsbtn.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.AddMenuItemsbtn.ForeColor = System.Drawing.Color.White
        Me.AddMenuItemsbtn.Location = New System.Drawing.Point(1024, 54)
        Me.AddMenuItemsbtn.Name = "AddMenuItemsbtn"
        Me.AddMenuItemsbtn.Size = New System.Drawing.Size(154, 35)
        Me.AddMenuItemsbtn.TabIndex = 3
        Me.AddMenuItemsbtn.Text = "➕ Add Menu Item"
        Me.AddMenuItemsbtn.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.GhostWhite
        Me.Panel2.Controls.Add(Me.txtSearch)
        Me.Panel2.Controls.Add(Me.btnCheckIngredients)
        Me.Panel2.Controls.Add(Me.btnExport)
        Me.Panel2.Controls.Add(Me.lblFilter)
        Me.Panel2.Controls.Add(Me.AddMenuItemsbtn)
        Me.Panel2.Controls.Add(Me.lblSearch)
        Me.Panel2.Controls.Add(Me.Category)
        Me.Panel2.Controls.Add(Me.btnRefresh)
        Me.Panel2.Controls.Add(Me.btnToggleAvailability)
        Me.Panel2.Controls.Add(Me.Edit)
        Me.Panel2.Controls.Add(Me.Delete)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel2.Location = New System.Drawing.Point(0, 79)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(9)
        Me.Panel2.Size = New System.Drawing.Size(1370, 100)
        Me.Panel2.TabIndex = 3
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.Transparent
        Me.txtSearch.BorderColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtSearch.BorderThickness = 1
        Me.txtSearch.Controls.Add(Me.TextBoxSearch)
        Me.txtSearch.CornerRadius = 10
        Me.txtSearch.FillColor = System.Drawing.Color.Snow
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(108, 9)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(657, 38)
        Me.txtSearch.TabIndex = 12
        '
        'TextBoxSearch
        '
        Me.TextBoxSearch.BackColor = System.Drawing.Color.Snow
        Me.TextBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TextBoxSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.TextBoxSearch.Location = New System.Drawing.Point(15, 10)
        Me.TextBoxSearch.Name = "TextBoxSearch"
        Me.TextBoxSearch.Size = New System.Drawing.Size(250, 18)
        Me.TextBoxSearch.TabIndex = 0
        Me.TextBoxSearch.Text = "Search orders..."
        '
        'btnCheckIngredients
        '
        Me.btnCheckIngredients.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.btnCheckIngredients.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCheckIngredients.FlatAppearance.BorderSize = 0
        Me.btnCheckIngredients.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCheckIngredients.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnCheckIngredients.ForeColor = System.Drawing.Color.White
        Me.btnCheckIngredients.Location = New System.Drawing.Point(851, 54)
        Me.btnCheckIngredients.Name = "btnCheckIngredients"
        Me.btnCheckIngredients.Size = New System.Drawing.Size(166, 35)
        Me.btnCheckIngredients.TabIndex = 2
        Me.btnCheckIngredients.Text = "Check Ingredients"
        Me.btnCheckIngredients.UseVisualStyleBackColor = False
        '
        'lblFilter
        '
        Me.lblFilter.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblFilter.Location = New System.Drawing.Point(794, 21)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(100, 23)
        Me.lblFilter.TabIndex = 0
        Me.lblFilter.Text = "Filter Category:"
        '
        'lblSearch
        '
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSearch.Location = New System.Drawing.Point(34, 19)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(52, 23)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search:"
        '
        'Category
        '
        Me.Category.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Category.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Category.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Category.ItemHeight = 29
        Me.Category.Location = New System.Drawing.Point(900, 10)
        Me.Category.Name = "Category"
        Me.Category.Size = New System.Drawing.Size(270, 35)
        Me.Category.TabIndex = 1
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(37, 56)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(110, 35)
        Me.btnRefresh.TabIndex = 4
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnToggleAvailability
        '
        Me.btnToggleAvailability.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer), CType(CType(7, Byte), Integer))
        Me.btnToggleAvailability.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnToggleAvailability.FlatAppearance.BorderSize = 0
        Me.btnToggleAvailability.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnToggleAvailability.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnToggleAvailability.ForeColor = System.Drawing.Color.White
        Me.btnToggleAvailability.Location = New System.Drawing.Point(1184, 54)
        Me.btnToggleAvailability.Name = "btnToggleAvailability"
        Me.btnToggleAvailability.Size = New System.Drawing.Size(146, 35)
        Me.btnToggleAvailability.TabIndex = 5
        Me.btnToggleAvailability.Text = "🔄 Toggle Status"
        Me.btnToggleAvailability.UseVisualStyleBackColor = False
        '
        'Edit
        '
        Me.Edit.BackColor = System.Drawing.Color.FromArgb(CType(CType(23, Byte), Integer), CType(CType(162, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.Edit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Edit.FlatAppearance.BorderSize = 0
        Me.Edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Edit.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Edit.ForeColor = System.Drawing.Color.White
        Me.Edit.Location = New System.Drawing.Point(154, 56)
        Me.Edit.Name = "Edit"
        Me.Edit.Size = New System.Drawing.Size(133, 35)
        Me.Edit.TabIndex = 7
        Me.Edit.Text = "Edit Selected"
        Me.Edit.UseVisualStyleBackColor = False
        '
        'Delete
        '
        Me.Delete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.Delete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Delete.FlatAppearance.BorderSize = 0
        Me.Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Delete.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Delete.ForeColor = System.Drawing.Color.White
        Me.Delete.Location = New System.Drawing.Point(293, 56)
        Me.Delete.Name = "Delete"
        Me.Delete.Size = New System.Drawing.Size(146, 35)
        Me.Delete.TabIndex = 8
        Me.Delete.Text = "Delete Selected"
        Me.Delete.UseVisualStyleBackColor = False
        '
        'DataGridMenu
        '
        Me.DataGridMenu.AllowUserToAddRows = False
        Me.DataGridMenu.AllowUserToDeleteRows = False
        Me.DataGridMenu.BackgroundColor = System.Drawing.Color.White
        Me.DataGridMenu.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridMenu.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.DataGridMenu.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridMenu.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridMenu.ColumnHeadersHeight = 45
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(219, Byte), Integer))
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridMenu.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridMenu.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridMenu.Location = New System.Drawing.Point(0, 179)
        Me.DataGridMenu.MultiSelect = False
        Me.DataGridMenu.Name = "DataGridMenu"
        Me.DataGridMenu.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridMenu.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridMenu.RowHeadersVisible = False
        Me.DataGridMenu.RowHeadersWidth = 45
        Me.DataGridMenu.RowTemplate.Height = 80
        Me.DataGridMenu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridMenu.Size = New System.Drawing.Size(1370, 486)
        Me.DataGridMenu.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel4.Controls.Add(Me.lblTotalItems)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 665)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1370, 35)
        Me.Panel4.TabIndex = 1
        '
        'lblTotalItems
        '
        Me.lblTotalItems.AutoSize = True
        Me.lblTotalItems.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalItems.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.lblTotalItems.Location = New System.Drawing.Point(13, 9)
        Me.lblTotalItems.Name = "lblTotalItems"
        Me.lblTotalItems.Size = New System.Drawing.Size(98, 19)
        Me.lblTotalItems.TabIndex = 0
        Me.lblTotalItems.Text = "Total Items: 0"
        '
        'MenuItems
        '
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1370, 700)
        Me.Controls.Add(Me.DataGridMenu)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.Name = "MenuItems"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Menu Items Management - Tabeya"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.txtSearch.ResumeLayout(False)
        Me.txtSearch.PerformLayout()
        CType(Me.DataGridMenu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblSearch As Label
    Friend WithEvents AddMenuItemsbtn As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnToggleAvailability As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents btnCheckIngredients As Button
    Friend WithEvents Edit As Button
    Friend WithEvents Delete As Button
    Friend WithEvents lblFilter As Label
    Friend WithEvents Category As ComboBox
    Friend WithEvents DataGridMenu As DataGridView
    Friend WithEvents Panel4 As Panel
    Friend WithEvents lblTotalItems As Label
    Friend WithEvents txtSearch As RoundedPane2
    Friend WithEvents TextBoxSearch As TextBox
End Class