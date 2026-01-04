<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Inventory
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Inventory))
        Me.InventoryGrid = New System.Windows.Forms.DataGridView()
        Me.ItemName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.InventoryID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CostUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LostStock = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ExpirationDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Actions = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSearch = New InformationManagement.RoundedPane2()
        Me.TextBoxSearch = New System.Windows.Forms.TextBox()
        Me.Category = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.btnNotifications = New System.Windows.Forms.Button()
        Me.pnlSummaryCards = New System.Windows.Forms.Panel()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.RoundedPane23 = New InformationManagement.RoundedPane2()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RoundedPane22 = New InformationManagement.RoundedPane2()
        Me.RoundedPane221 = New InformationManagement.RoundedPane2()
        Me.PictureBox11 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        CType(Me.InventoryGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.txtSearch.SuspendLayout()
        Me.pnlSummaryCards.SuspendLayout()
        Me.RoundedPane21.SuspendLayout()
        Me.RoundedPane23.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane22.SuspendLayout()
        Me.RoundedPane221.SuspendLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'InventoryGrid
        '
        Me.InventoryGrid.AllowUserToResizeColumns = False
        Me.InventoryGrid.AllowUserToResizeRows = False
        Me.InventoryGrid.BackgroundColor = System.Drawing.Color.White
        Me.InventoryGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.InventoryGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.InventoryGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.InventoryGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.InventoryGrid.ColumnHeadersHeight = 40
        Me.InventoryGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ItemName, Me.InventoryID, Me.ProductID, Me.Quantity, Me.Status, Me.CostUnit, Me.TotalValue, Me.LostStock, Me.Type, Me.ExpirationDate, Me.Actions})
        Me.InventoryGrid.EnableHeadersVisualStyles = False
        Me.InventoryGrid.Location = New System.Drawing.Point(34, 294)
        Me.InventoryGrid.Name = "InventoryGrid"
        Me.InventoryGrid.ReadOnly = True
        Me.InventoryGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.InventoryGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.InventoryGrid.RowHeadersVisible = False
        Me.InventoryGrid.RowHeadersWidth = 51
        Me.InventoryGrid.Size = New System.Drawing.Size(1137, 353)
        Me.InventoryGrid.TabIndex = 40
        '
        'ItemName
        '
        Me.ItemName.DataPropertyName = "colItemName"
        Me.ItemName.Frozen = True
        Me.ItemName.HeaderText = "Item Name"
        Me.ItemName.MinimumWidth = 6
        Me.ItemName.Name = "ItemName"
        Me.ItemName.ReadOnly = True
        Me.ItemName.Width = 150
        '
        'InventoryID
        '
        Me.InventoryID.Frozen = True
        Me.InventoryID.HeaderText = "Inventory ID"
        Me.InventoryID.MinimumWidth = 6
        Me.InventoryID.Name = "InventoryID"
        Me.InventoryID.ReadOnly = True
        Me.InventoryID.Width = 125
        '
        'ProductID
        '
        Me.ProductID.Frozen = True
        Me.ProductID.HeaderText = "Product ID"
        Me.ProductID.MinimumWidth = 6
        Me.ProductID.Name = "ProductID"
        Me.ProductID.ReadOnly = True
        Me.ProductID.Width = 125
        '
        'Quantity
        '
        Me.Quantity.DataPropertyName = "colQuantity"
        Me.Quantity.Frozen = True
        Me.Quantity.HeaderText = "Quantity"
        Me.Quantity.MinimumWidth = 6
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ReadOnly = True
        Me.Quantity.Width = 120
        '
        'Status
        '
        Me.Status.DataPropertyName = "colStatus"
        Me.Status.Frozen = True
        Me.Status.HeaderText = "Status"
        Me.Status.MinimumWidth = 6
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        Me.Status.Width = 120
        '
        'CostUnit
        '
        Me.CostUnit.DataPropertyName = "colCostUnit"
        Me.CostUnit.Frozen = True
        Me.CostUnit.HeaderText = "Cost/Unit"
        Me.CostUnit.MinimumWidth = 6
        Me.CostUnit.Name = "CostUnit"
        Me.CostUnit.ReadOnly = True
        Me.CostUnit.Width = 125
        '
        'TotalValue
        '
        Me.TotalValue.DataPropertyName = "colTotalValue"
        Me.TotalValue.Frozen = True
        Me.TotalValue.HeaderText = "Total Value"
        Me.TotalValue.MinimumWidth = 6
        Me.TotalValue.Name = "TotalValue"
        Me.TotalValue.ReadOnly = True
        Me.TotalValue.Width = 120
        '
        'LostStock
        '
        Me.LostStock.DataPropertyName = "colLostStock"
        Me.LostStock.Frozen = True
        Me.LostStock.HeaderText = "Last Stock"
        Me.LostStock.MinimumWidth = 6
        Me.LostStock.Name = "LostStock"
        Me.LostStock.ReadOnly = True
        Me.LostStock.Width = 120
        '
        'Type
        '
        Me.Type.DataPropertyName = "colType"
        Me.Type.HeaderText = "Unit Type"
        Me.Type.MinimumWidth = 6
        Me.Type.Name = "Type"
        Me.Type.ReadOnly = True
        Me.Type.Width = 120
        '
        'ExpirationDate
        '
        Me.ExpirationDate.DataPropertyName = "colExpirationDate"
        Me.ExpirationDate.HeaderText = "Expiration Date"
        Me.ExpirationDate.MinimumWidth = 6
        Me.ExpirationDate.Name = "ExpirationDate"
        Me.ExpirationDate.ReadOnly = True
        Me.ExpirationDate.Width = 120
        '
        'Actions
        '
        Me.Actions.DataPropertyName = "colActions"
        Me.Actions.HeaderText = "Actions"
        Me.Actions.MinimumWidth = 6
        Me.Actions.Name = "Actions"
        Me.Actions.ReadOnly = True
        Me.Actions.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Actions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Actions.UseColumnTextForButtonValue = True
        Me.Actions.Width = 120
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(759, 258)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 19)
        Me.Label7.TabIndex = 39
        Me.Label7.Text = "Category :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(30, 258)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(90, 19)
        Me.Label6.TabIndex = 32
        Me.Label6.Text = "Search Item :"
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.Transparent
        Me.txtSearch.BorderColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtSearch.BorderThickness = 1
        Me.txtSearch.Controls.Add(Me.TextBoxSearch)
        Me.txtSearch.CornerRadius = 10
        Me.txtSearch.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(126, 248)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(618, 38)
        Me.txtSearch.TabIndex = 38
        '
        'TextBoxSearch
        '
        Me.TextBoxSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.TextBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TextBoxSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.TextBoxSearch.Location = New System.Drawing.Point(15, 10)
        Me.TextBoxSearch.Name = "TextBoxSearch"
        Me.TextBoxSearch.Size = New System.Drawing.Size(579, 18)
        Me.TextBoxSearch.TabIndex = 0
        Me.TextBoxSearch.Text = "Search inventory..."
        '
        'Category
        '
        Me.Category.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Category.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Category.FormattingEnabled = True
        Me.Category.Items.AddRange(New Object() {"Meat & Poultry", "Vegetables", "Dairy", "Dry Goods", "Oils & Condiments", "Herbs & Spices", "Beverages", "Seafood"})
        Me.Category.Location = New System.Drawing.Point(841, 255)
        Me.Category.Margin = New System.Windows.Forms.Padding(2)
        Me.Category.Name = "Category"
        Me.Category.Size = New System.Drawing.Size(188, 25)
        Me.Category.TabIndex = 37
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label2.Location = New System.Drawing.Point(36, 67)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(272, 19)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Track and manage your restaurant supplies"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(29, 26)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(423, 41)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Restaurant Inventory System"
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 0)
        Me.Splitter1.Margin = New System.Windows.Forms.Padding(2)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(1028, 94)
        Me.Splitter1.TabIndex = 31
        Me.Splitter1.TabStop = False
        '
        'btnNotifications
        '
        Me.btnNotifications.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNotifications.BackColor = System.Drawing.Color.FromArgb(CType(CType(111, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(193, Byte), Integer))
        Me.btnNotifications.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNotifications.FlatAppearance.BorderSize = 0
        Me.btnNotifications.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNotifications.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnNotifications.ForeColor = System.Drawing.Color.White
        Me.btnNotifications.Location = New System.Drawing.Point(788, 98)
        Me.btnNotifications.Margin = New System.Windows.Forms.Padding(2)
        Me.btnNotifications.Name = "btnNotifications"
        Me.btnNotifications.Size = New System.Drawing.Size(240, 44)
        Me.btnNotifications.TabIndex = 50
        Me.btnNotifications.Text = "🔔 View Usage History"
        Me.btnNotifications.UseVisualStyleBackColor = False
        '
        'pnlSummaryCards
        '
        Me.pnlSummaryCards.BackColor = System.Drawing.Color.Transparent
        Me.pnlSummaryCards.Controls.Add(Me.RoundedPane21)
        Me.pnlSummaryCards.Controls.Add(Me.RoundedPane22)
        Me.pnlSummaryCards.Location = New System.Drawing.Point(34, 114)
        Me.pnlSummaryCards.Name = "pnlSummaryCards"
        Me.pnlSummaryCards.Size = New System.Drawing.Size(582, 123)
        Me.pnlSummaryCards.TabIndex = 51
        '
        'RoundedPane21
        '
        Me.RoundedPane21.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane21.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.RoundedPane23)
        Me.RoundedPane21.Controls.Add(Me.Label5)
        Me.RoundedPane21.Controls.Add(Me.Label3)
        Me.RoundedPane21.CornerRadius = 20
        Me.RoundedPane21.FillColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.RoundedPane21.Location = New System.Drawing.Point(3, 3)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(274, 111)
        Me.RoundedPane21.TabIndex = 35
        '
        'RoundedPane23
        '
        Me.RoundedPane23.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane23.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane23.BorderThickness = 1
        Me.RoundedPane23.Controls.Add(Me.PictureBox3)
        Me.RoundedPane23.CornerRadius = 8
        Me.RoundedPane23.FillColor = System.Drawing.Color.FromArgb(CType(CType(96, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.RoundedPane23.Location = New System.Drawing.Point(20, 20)
        Me.RoundedPane23.Name = "RoundedPane23"
        Me.RoundedPane23.Size = New System.Drawing.Size(41, 38)
        Me.RoundedPane23.TabIndex = 10
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
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(21, 62)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 32)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "8"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(67, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 21)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Total Items"
        '
        'RoundedPane22
        '
        Me.RoundedPane22.BorderColor = System.Drawing.Color.LightGray
        Me.RoundedPane22.BorderThickness = 1
        Me.RoundedPane22.Controls.Add(Me.RoundedPane221)
        Me.RoundedPane22.Controls.Add(Me.Label4)
        Me.RoundedPane22.Controls.Add(Me.Label10)
        Me.RoundedPane22.Controls.Add(Me.Label11)
        Me.RoundedPane22.CornerRadius = 20
        Me.RoundedPane22.FillColor = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(129, Byte), Integer))
        Me.RoundedPane22.Location = New System.Drawing.Point(295, 3)
        Me.RoundedPane22.Name = "RoundedPane22"
        Me.RoundedPane22.Size = New System.Drawing.Size(274, 111)
        Me.RoundedPane22.TabIndex = 36
        '
        'RoundedPane221
        '
        Me.RoundedPane221.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane221.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane221.BorderThickness = 1
        Me.RoundedPane221.Controls.Add(Me.PictureBox11)
        Me.RoundedPane221.CornerRadius = 8
        Me.RoundedPane221.FillColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(153, Byte), Integer))
        Me.RoundedPane221.Location = New System.Drawing.Point(20, 20)
        Me.RoundedPane221.Name = "RoundedPane221"
        Me.RoundedPane221.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane221.TabIndex = 8
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.Label4.Location = New System.Drawing.Point(25, 98)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(0, 13)
        Me.Label4.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label10.Location = New System.Drawing.Point(67, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(88, 21)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Total Value"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(22, 62)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(190, 32)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "₱10,750,000.00"
        '
        'Inventory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1028, 676)
        Me.Controls.Add(Me.pnlSummaryCards)
        Me.Controls.Add(Me.btnNotifications)
        Me.Controls.Add(Me.InventoryGrid)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.Category)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Splitter1)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Inventory"
        Me.Text = "Inventory"
        CType(Me.InventoryGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.txtSearch.ResumeLayout(False)
        Me.txtSearch.PerformLayout()
        Me.pnlSummaryCards.ResumeLayout(False)
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        Me.RoundedPane23.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane22.ResumeLayout(False)
        Me.RoundedPane22.PerformLayout()
        Me.RoundedPane221.ResumeLayout(False)
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents RoundedPane21 As RoundedPane2
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents RoundedPane22 As RoundedPane2
    Friend WithEvents InventoryGrid As DataGridView
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSearch As RoundedPane2
    Friend WithEvents TextBoxSearch As TextBox
    Friend WithEvents Category As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents ItemName As DataGridViewTextBoxColumn
    Friend WithEvents InventoryID As DataGridViewTextBoxColumn
    Friend WithEvents ProductID As DataGridViewTextBoxColumn
    Friend WithEvents Quantity As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents CostUnit As DataGridViewTextBoxColumn
    Friend WithEvents TotalValue As DataGridViewTextBoxColumn
    Friend WithEvents LostStock As DataGridViewTextBoxColumn
    Friend WithEvents Type As DataGridViewTextBoxColumn
    Friend WithEvents ExpirationDate As DataGridViewTextBoxColumn
    Friend WithEvents Actions As DataGridViewButtonColumn
    Friend WithEvents btnNotifications As Button
    Friend WithEvents pnlSummaryCards As Panel
    Friend WithEvents RoundedPane23 As RoundedPane2
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents RoundedPane221 As RoundedPane2
    Friend WithEvents PictureBox11 As PictureBox
End Class