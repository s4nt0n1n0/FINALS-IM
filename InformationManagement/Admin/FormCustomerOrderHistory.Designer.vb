<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCustomerOrderHistory
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabOrders = New System.Windows.Forms.TabPage()
        Me.dgvOrders = New System.Windows.Forms.DataGridView()
        Me.colOrderID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDateTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOrderType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colItemCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTotalAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPayment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tabReservations = New System.Windows.Forms.TabPage()
        Me.dgvReservations = New System.Windows.Forms.DataGridView()
        Me.colResID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colResDateTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colResItems = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colGuests = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colResTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colResStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colResPayment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()

        Me.TabControl1.SuspendLayout()
        Me.tabOrders.SuspendLayout()
        CType(Me.dgvOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabReservations.SuspendLayout()
        CType(Me.dgvReservations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(880, 60)
        Me.Panel1.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.AutoSize = True
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Gray
        Me.btnClose.Location = New System.Drawing.Point(860, 10)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(20, 21)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "X"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtSearch.Location = New System.Drawing.Point(623, 22)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(200, 25)
        Me.txtSearch.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.DimGray
        Me.lblTitle.Location = New System.Drawing.Point(20, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(105, 21)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Order History"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabOrders)
        Me.TabControl1.Controls.Add(Me.tabReservations)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TabControl1.Location = New System.Drawing.Point(0, 60)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(880, 307)
        Me.TabControl1.TabIndex = 1
        '
        'tabOrders
        '
        Me.tabOrders.Controls.Add(Me.dgvOrders)
        Me.tabOrders.Location = New System.Drawing.Point(4, 24)
        Me.tabOrders.Name = "tabOrders"
        Me.tabOrders.Padding = New System.Windows.Forms.Padding(3)
        Me.tabOrders.Size = New System.Drawing.Size(892, 279)
        Me.tabOrders.TabIndex = 0
        Me.tabOrders.Text = "Orders"
        Me.tabOrders.UseVisualStyleBackColor = True
        '
        'dgvOrders
        '
        Me.dgvOrders.AllowUserToAddRows = False
        Me.dgvOrders.AllowUserToDeleteRows = False
        Me.dgvOrders.AllowUserToResizeColumns = False
        Me.dgvOrders.AllowUserToResizeRows = False
        Me.dgvOrders.AutoGenerateColumns = False
        Me.dgvOrders.BackgroundColor = System.Drawing.Color.White
        Me.dgvOrders.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvOrders.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvOrders.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvOrders.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvOrders.ColumnHeadersHeight = 45
        Me.dgvOrders.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colOrderID, Me.colDateTime, Me.colOrderType, Me.colItemCount, Me.colTotalAmount, Me.colPayment, Me.colStatus})
        Me.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvOrders.EnableHeadersVisualStyles = False
        Me.dgvOrders.Location = New System.Drawing.Point(3, 3)
        Me.dgvOrders.Name = "dgvOrders"
        Me.dgvOrders.ReadOnly = True
        Me.dgvOrders.RowHeadersVisible = False
        Me.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvOrders.Size = New System.Drawing.Size(886, 273)
        Me.dgvOrders.TabIndex = 1
        '
        'colOrderID
        '
        Me.colOrderID.HeaderText = "Order ID"
        Me.colOrderID.Name = "colOrderID"
        Me.colOrderID.ReadOnly = True
        '
        'colDateTime
        '
        Me.colDateTime.HeaderText = "Date & Time"
        Me.colDateTime.Name = "colDateTime"
        Me.colDateTime.ReadOnly = True
        Me.colDateTime.Width = 150
        '
        'colOrderType
        '
        Me.colOrderType.HeaderText = "Order Type"
        Me.colOrderType.Name = "colOrderType"
        Me.colOrderType.ReadOnly = True
        '
        'colItemCount
        '
        Me.colItemCount.HeaderText = "Item Count"
        Me.colItemCount.Name = "colItemCount"
        Me.colItemCount.ReadOnly = True
        '
        'colTotalAmount
        '
        Me.colTotalAmount.HeaderText = "Total Amount"
        Me.colTotalAmount.Name = "colTotalAmount"
        Me.colTotalAmount.ReadOnly = True
        '
        'colPayment
        '
        Me.colPayment.HeaderText = "Payment"
        Me.colPayment.Name = "colPayment"
        Me.colPayment.ReadOnly = True
        '
        'colStatus
        '
        Me.colStatus.HeaderText = "Status"
        Me.colStatus.Name = "colStatus"
        Me.colStatus.ReadOnly = True
        '
        'tabReservations
        '
        Me.tabReservations.Controls.Add(Me.dgvReservations)
        Me.tabReservations.Location = New System.Drawing.Point(4, 24)
        Me.tabReservations.Name = "tabReservations"
        Me.tabReservations.Padding = New System.Windows.Forms.Padding(3)
        Me.tabReservations.Size = New System.Drawing.Size(892, 279)
        Me.tabReservations.TabIndex = 1
        Me.tabReservations.Text = "Reservations"
        Me.tabReservations.UseVisualStyleBackColor = True
        '
        'dgvReservations
        '
        Me.dgvReservations.AllowUserToAddRows = False
        Me.dgvReservations.AllowUserToDeleteRows = False
        Me.dgvReservations.AllowUserToResizeColumns = False
        Me.dgvReservations.AllowUserToResizeRows = False
        Me.dgvReservations.AutoGenerateColumns = False
        Me.dgvReservations.BackgroundColor = System.Drawing.Color.White
        Me.dgvReservations.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvReservations.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvReservations.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvReservations.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvReservations.ColumnHeadersHeight = 45
        Me.dgvReservations.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colResID, Me.colResDateTime, Me.colResItems, Me.colGuests, Me.colResTotal, Me.colResStatus, Me.colResPayment})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvReservations.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvReservations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvReservations.EnableHeadersVisualStyles = False
        Me.dgvReservations.Location = New System.Drawing.Point(3, 3)
        Me.dgvReservations.Name = "dgvReservations"
        Me.dgvReservations.ReadOnly = True
        Me.dgvReservations.RowHeadersVisible = False
        Me.dgvReservations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvReservations.Size = New System.Drawing.Size(886, 273)
        Me.dgvReservations.TabIndex = 2
        '
        'colResID
        '
        Me.colResID.HeaderText = "ID"
        Me.colResID.Name = "colResID"
        Me.colResID.ReadOnly = True
        '
        'colResDateTime
        '
        Me.colResDateTime.HeaderText = "Date & Time"
        Me.colResDateTime.Name = "colResDateTime"
        Me.colResDateTime.ReadOnly = True
        Me.colResDateTime.Width = 150
        '
        'colResItems
        '
        Me.colResItems.HeaderText = "Items / Promo"
        Me.colResItems.Name = "colResItems"
        Me.colResItems.ReadOnly = True
        Me.colResItems.Width = 200
        '
        'colGuests
        '
        Me.colGuests.HeaderText = "Guests"
        Me.colGuests.Name = "colGuests"
        Me.colGuests.ReadOnly = True
        Me.colGuests.Width = 60
        '
        'colResTotal
        '
        Me.colResTotal.HeaderText = "Total"
        Me.colResTotal.Name = "colResTotal"
        Me.colResTotal.ReadOnly = True
        '
        'colResStatus
        '
        Me.colResStatus.HeaderText = "Status"
        Me.colResStatus.Name = "colResStatus"
        Me.colResStatus.ReadOnly = True
        '
        'colResPayment
        '
        Me.colResPayment.HeaderText = "Payment"
        Me.colResPayment.Name = "colResPayment"
        Me.colResPayment.ReadOnly = True
        '
        'FormCustomerOrderHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(900, 367)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormCustomerOrderHistory"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Customer History"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tabOrders.ResumeLayout(False)
        CType(Me.dgvOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabReservations.ResumeLayout(False)
        CType(Me.dgvReservations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabOrders As System.Windows.Forms.TabPage
    Friend WithEvents tabReservations As System.Windows.Forms.TabPage
    Friend WithEvents dgvOrders As System.Windows.Forms.DataGridView
    Friend WithEvents colOrderID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDateTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colOrderType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colItemCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTotalAmount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPayment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvReservations As System.Windows.Forms.DataGridView
    Friend WithEvents colResID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colResDateTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colResItems As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colGuests As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colResTotal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colResStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colResPayment As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
