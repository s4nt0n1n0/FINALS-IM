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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.LabelSubHeader = New System.Windows.Forms.Label()
        Me.btnExportPdf = New System.Windows.Forms.Button()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.PaginationContainer = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.lblPageStatus = New System.Windows.Forms.Label()
        Me.SearchContainer = New InformationManagement.RoundedPane2()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.colCustomerName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colContact = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLastVisit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTotalVisits = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTotalSpent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAction = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Panel1.SuspendLayout()
        Me.RoundedPane21.SuspendLayout()
        Me.PaginationContainer.SuspendLayout()
        Me.SearchContainer.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.LabelHeader)
        Me.Panel1.Controls.Add(Me.LabelSubHeader)
        Me.Panel1.Controls.Add(Me.btnExportPdf)
        Me.Panel1.Controls.Add(Me.RoundedPane21)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1111, 609)
        Me.Panel1.TabIndex = 4
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoSize = True
        Me.LabelHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 22.0!, System.Drawing.FontStyle.Bold)
        Me.LabelHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.LabelHeader.Location = New System.Drawing.Point(22, 20)
        Me.LabelHeader.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(258, 41)
        Me.LabelHeader.TabIndex = 0
        Me.LabelHeader.Text = "Customer History"
        '
        'LabelSubHeader
        '
        Me.LabelSubHeader.AutoSize = True
        Me.LabelSubHeader.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.LabelSubHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.LabelSubHeader.Location = New System.Drawing.Point(24, 55)
        Me.LabelSubHeader.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelSubHeader.Name = "LabelSubHeader"
        Me.LabelSubHeader.Size = New System.Drawing.Size(469, 19)
        Me.LabelSubHeader.TabIndex = 1
        Me.LabelSubHeader.Text = "Comprehensive record of customer transactions, order details, and statuses"
        '
        'btnExportPdf
        '
        Me.btnExportPdf.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(CType(CType(55, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.btnExportPdf.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExportPdf.FlatAppearance.BorderSize = 0
        Me.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportPdf.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnExportPdf.ForeColor = System.Drawing.Color.White
        Me.btnExportPdf.Location = New System.Drawing.Point(1117, 28)
        Me.btnExportPdf.Margin = New System.Windows.Forms.Padding(2)
        Me.btnExportPdf.Name = "btnExportPdf"
        Me.btnExportPdf.Size = New System.Drawing.Size(125, 35)
        Me.btnExportPdf.TabIndex = 3
        Me.btnExportPdf.Text = "Export PDF"
        Me.btnExportPdf.UseVisualStyleBackColor = False
        '
        'RoundedPane21
        '
        Me.RoundedPane21.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RoundedPane21.AutoScroll = True
        Me.RoundedPane21.AutoSize = True
        Me.RoundedPane21.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.PaginationContainer)
        Me.RoundedPane21.Controls.Add(Me.SearchContainer)
        Me.RoundedPane21.Controls.Add(Me.Label1)
        Me.RoundedPane21.Controls.Add(Me.DataGridView1)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.White
        Me.RoundedPane21.Location = New System.Drawing.Point(22, 89)
        Me.RoundedPane21.Margin = New System.Windows.Forms.Padding(2)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(1045, 470)
        Me.RoundedPane21.TabIndex = 2
        '
        'PaginationContainer
        '
        Me.PaginationContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PaginationContainer.Controls.Add(Me.btnNext)
        Me.PaginationContainer.Controls.Add(Me.btnPrev)
        Me.PaginationContainer.Controls.Add(Me.lblPageStatus)
        Me.PaginationContainer.Location = New System.Drawing.Point(15, 423)
        Me.PaginationContainer.Margin = New System.Windows.Forms.Padding(2)
        Me.PaginationContainer.Name = "PaginationContainer"
        Me.PaginationContainer.Size = New System.Drawing.Size(999, 37)
        Me.PaginationContainer.TabIndex = 10
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNext.Location = New System.Drawing.Point(914, 4)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(2)
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
        Me.btnPrev.Location = New System.Drawing.Point(823, 4)
        Me.btnPrev.Margin = New System.Windows.Forms.Padding(2)
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
        Me.lblPageStatus.Location = New System.Drawing.Point(7, 12)
        Me.lblPageStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPageStatus.Name = "lblPageStatus"
        Me.lblPageStatus.Size = New System.Drawing.Size(124, 15)
        Me.lblPageStatus.TabIndex = 0
        Me.lblPageStatus.Text = "Page 1 of 1 (0 records)"
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
        Me.SearchContainer.Location = New System.Drawing.Point(630, 20)
        Me.SearchContainer.Margin = New System.Windows.Forms.Padding(2)
        Me.SearchContainer.Name = "SearchContainer"
        Me.SearchContainer.Size = New System.Drawing.Size(384, 38)
        Me.SearchContainer.TabIndex = 1
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtSearch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(9, 9)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(2)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(191, 18)
        Me.txtSearch.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(30, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(214, 20)
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
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridView1.ColumnHeadersHeight = 45
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colCustomerName, Me.colContact, Me.colType, Me.colLastVisit, Me.colTotalVisits, Me.colTotalSpent, Me.colAction})
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.DataGridView1.Location = New System.Drawing.Point(15, 69)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowTemplate.Height = 50
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(1014, 340)
        Me.DataGridView1.TabIndex = 0
        '
        'colCustomerName
        '
        Me.colCustomerName.HeaderText = "Customer Name"
        Me.colCustomerName.MinimumWidth = 6
        Me.colCustomerName.Name = "colCustomerName"
        Me.colCustomerName.Width = 200
        '
        'colContact
        '
        Me.colContact.HeaderText = "Contact / Email"
        Me.colContact.MinimumWidth = 6
        Me.colContact.Name = "colContact"
        Me.colContact.Width = 200
        '
        'colType
        '
        Me.colType.HeaderText = "Type"
        Me.colType.MinimumWidth = 6
        Me.colType.Name = "colType"
        '
        'colLastVisit
        '
        Me.colLastVisit.HeaderText = "Last Visit"
        Me.colLastVisit.MinimumWidth = 6
        Me.colLastVisit.Name = "colLastVisit"
        Me.colLastVisit.Width = 120
        '
        'colTotalVisits
        '
        Me.colTotalVisits.HeaderText = "Visits"
        Me.colTotalVisits.MinimumWidth = 6
        Me.colTotalVisits.Name = "colTotalVisits"
        Me.colTotalVisits.Width = 80
        '
        'colTotalSpent
        '
        Me.colTotalSpent.HeaderText = "Total Spent"
        Me.colTotalSpent.MinimumWidth = 6
        Me.colTotalSpent.Name = "colTotalSpent"
        Me.colTotalSpent.Width = 120
        '
        'colAction
        '
        Me.colAction.HeaderText = "Action"
        Me.colAction.MinimumWidth = 6
        Me.colAction.Name = "colAction"
        Me.colAction.Text = "View History"
        Me.colAction.UseColumnTextForButtonValue = True
        Me.colAction.Width = 120
        '
        'FormCustomerHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1111, 609)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Name = "FormCustomerHistory"
        Me.Text = "FormCustomerHistory"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        Me.PaginationContainer.ResumeLayout(False)
        Me.PaginationContainer.PerformLayout()
        Me.SearchContainer.ResumeLayout(False)
        Me.SearchContainer.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub


    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnExportPdf As Button
    Friend WithEvents LabelHeader As Label
    Friend WithEvents LabelSubHeader As Label
    Friend WithEvents RoundedPane21 As RoundedPane2
    Friend WithEvents DataGridView1 As DataGridView

    Friend WithEvents PaginationContainer As Panel
    Friend WithEvents btnNext As Button
    Friend WithEvents btnPrev As Button
    Friend WithEvents lblPageStatus As Label
    Friend WithEvents SearchContainer As RoundedPane2
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents colCustomerName As DataGridViewTextBoxColumn
    Friend WithEvents colContact As DataGridViewTextBoxColumn
    Friend WithEvents colType As DataGridViewTextBoxColumn
    Friend WithEvents colLastVisit As DataGridViewTextBoxColumn
    Friend WithEvents colTotalVisits As DataGridViewTextBoxColumn
    Friend WithEvents colTotalSpent As DataGridViewTextBoxColumn
    Friend WithEvents colAction As DataGridViewButtonColumn
    Friend WithEvents Label1 As Label
End Class

