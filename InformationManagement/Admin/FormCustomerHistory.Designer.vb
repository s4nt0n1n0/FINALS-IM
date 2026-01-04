<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormCustomerHistory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCustomerHistory))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.LabelSubHeader = New System.Windows.Forms.Label()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.PaginationContainer = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.lblPageStatus = New System.Windows.Forms.Label()
        Me.SearchContainer = New InformationManagement.RoundedPane2()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()

        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.dateid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Orderid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Items = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Amount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RoundedPane21.SuspendLayout()
        Me.PaginationContainer.SuspendLayout()
        Me.SearchContainer.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoSize = True
        Me.LabelHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 22.0!, System.Drawing.FontStyle.Bold)
        Me.LabelHeader.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42)
        Me.LabelHeader.Location = New System.Drawing.Point(30, 25)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(256, 41)
        Me.LabelHeader.TabIndex = 0
        Me.LabelHeader.Text = "Customer History"
        '
        'LabelSubHeader
        '
        Me.LabelSubHeader.AutoSize = True
        Me.LabelSubHeader.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.LabelSubHeader.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
        Me.LabelSubHeader.Location = New System.Drawing.Point(32, 68)
        Me.LabelSubHeader.Name = "LabelSubHeader"
        Me.LabelSubHeader.Size = New System.Drawing.Size(460, 19)
        Me.LabelSubHeader.TabIndex = 1
        Me.LabelSubHeader.Text = "Comprehensive record of customer transactions, order details, and statuses"
        '
        'RoundedPane21 (Main Container)
        '
        Me.RoundedPane21.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RoundedPane21.BorderColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.PaginationContainer)
        Me.RoundedPane21.Controls.Add(Me.SearchContainer)

        Me.RoundedPane21.Controls.Add(Me.Label1)
        Me.RoundedPane21.Controls.Add(Me.DataGridView1)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.White
        Me.RoundedPane21.Location = New System.Drawing.Point(30, 110)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(1516, 610)
        Me.RoundedPane21.TabIndex = 2
        '
        'PaginationContainer
        '
        Me.PaginationContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PaginationContainer.Controls.Add(Me.btnNext)
        Me.PaginationContainer.Controls.Add(Me.btnPrev)
        Me.PaginationContainer.Controls.Add(Me.lblPageStatus)
        Me.PaginationContainer.Location = New System.Drawing.Point(20, 545)
        Me.PaginationContainer.Name = "PaginationContainer"
        Me.PaginationContainer.Size = New System.Drawing.Size(1475, 45)
        Me.PaginationContainer.TabIndex = 10
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNext.Location = New System.Drawing.Point(1375, 7)
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
        Me.btnPrev.Location = New System.Drawing.Point(1285, 7)
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
        Me.lblPageStatus.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
        Me.lblPageStatus.Location = New System.Drawing.Point(0, 15)
        Me.lblPageStatus.Name = "lblPageStatus"
        Me.lblPageStatus.Size = New System.Drawing.Size(125, 15)
        Me.lblPageStatus.TabIndex = 0
        Me.lblPageStatus.Text = "Page 1 of 1 (0 records)"
        '
        'SearchContainer
        '
        Me.SearchContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchContainer.BackColor = System.Drawing.Color.Transparent
        Me.SearchContainer.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240)
        Me.SearchContainer.BorderThickness = 1
        Me.SearchContainer.Controls.Add(Me.txtSearch)
        Me.SearchContainer.CornerRadius = 10
        Me.SearchContainer.FillColor = System.Drawing.Color.FromArgb(248, 250, 252)
        Me.SearchContainer.Location = New System.Drawing.Point(1050, 22)
        Me.SearchContainer.Name = "SearchContainer"
        Me.SearchContainer.Size = New System.Drawing.Size(280, 40)
        Me.SearchContainer.TabIndex = 1
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(248, 250, 252)
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtSearch.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42)
        Me.txtSearch.Location = New System.Drawing.Point(12, 11)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(255, 18)
        Me.txtSearch.TabIndex = 0
        Me.txtSearch.Text = ""
        '


        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(40, 38)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(271, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Customer Order History Report"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
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
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeight = 45
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dateid, Me.Orderid, Me.Type, Me.Items, Me.Amount, Me.Status})
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.GridColor = System.Drawing.Color.FromArgb(241, 245, 249)
        Me.DataGridView1.Location = New System.Drawing.Point(20, 85)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowTemplate.Height = 50
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(1475, 450)
        Me.DataGridView1.TabIndex = 0
        '
        'dateid
        '
        Me.dateid.HeaderText = "Date"
        Me.dateid.MinimumWidth = 6
        Me.dateid.Name = "dateid"
        Me.dateid.Width = 150
        '
        'Orderid
        '
        Me.Orderid.HeaderText = "Order ID"
        Me.Orderid.MinimumWidth = 6
        Me.Orderid.Name = "Orderid"
        Me.Orderid.Width = 150
        '
        'Type
        '
        Me.Type.HeaderText = "Type"
        Me.Type.MinimumWidth = 6
        Me.Type.Name = "Type"
        Me.Type.Width = 220
        '
        'Items
        '
        Me.Items.HeaderText = "Items"
        Me.Items.MinimumWidth = 6
        Me.Items.Name = "Items"
        Me.Items.Width = 230
        '
        'Amount
        '
        Me.Amount.HeaderText = "Amount"
        Me.Amount.MinimumWidth = 6
        Me.Amount.Name = "Amount"
        Me.Amount.Width = 220
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.MinimumWidth = 6
        Me.Status.Name = "Status"
        Me.Status.Width = 150
        '
        'FormCustomerHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1580, 750)
        Me.Controls.Add(Me.LabelSubHeader)
        Me.Controls.Add(Me.LabelHeader)
        Me.Controls.Add(Me.RoundedPane21)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "FormCustomerHistory"
        Me.Text = "FormCustomerHistory"
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
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
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label1 As Label

    Friend WithEvents PaginationContainer As Panel
    Friend WithEvents btnNext As Button
    Friend WithEvents btnPrev As Button
    Friend WithEvents lblPageStatus As Label
    Friend WithEvents SearchContainer As RoundedPane2
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents dateid As DataGridViewTextBoxColumn
    Friend WithEvents Orderid As DataGridViewTextBoxColumn
    Friend WithEvents Type As DataGridViewTextBoxColumn
    Friend WithEvents Items As DataGridViewTextBoxColumn
    Friend WithEvents Amount As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn

End Class

