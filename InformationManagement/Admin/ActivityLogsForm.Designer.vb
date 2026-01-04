<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ActivityLogsForm
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
        Me.dgvActivityLogs = New System.Windows.Forms.DataGridView()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.cboUserType = New System.Windows.Forms.ComboBox()
        Me.cboActionCategory = New System.Windows.Forms.ComboBox()
        Me.cboSourceSystem = New System.Windows.Forms.ComboBox()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnApplyFilters = New System.Windows.Forms.Button()
        Me.btnResetFilters = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExportCSV = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnClearLogs = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnLastPage = New System.Windows.Forms.Button()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.btnPrevPage = New System.Windows.Forms.Button()
        Me.btnFirstPage = New System.Windows.Forms.Button()
        Me.lblTotalLogs = New System.Windows.Forms.Label()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.lblUserType = New System.Windows.Forms.Label()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.lblStatusFilter = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.pnlFilters = New System.Windows.Forms.Panel()
        CType(Me.dgvActivityLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.pnlFilters.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvActivityLogs
        '
        Me.dgvActivityLogs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvActivityLogs.BackgroundColor = System.Drawing.Color.White
        Me.dgvActivityLogs.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvActivityLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.dgvActivityLogs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.dgvActivityLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvActivityLogs.Location = New System.Drawing.Point(12, 180)
        Me.dgvActivityLogs.Name = "dgvActivityLogs"
        Me.dgvActivityLogs.Size = New System.Drawing.Size(1176, 450)
        Me.dgvActivityLogs.TabIndex = 0
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(15, 35)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(150, 23)
        Me.dtpStartDate.TabIndex = 1
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(180, 35)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(150, 23)
        Me.dtpEndDate.TabIndex = 3
        '
        'cboUserType
        '
        Me.cboUserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserType.FormattingEnabled = True
        Me.cboUserType.Location = New System.Drawing.Point(345, 35)
        Me.cboUserType.Name = "cboUserType"
        Me.cboUserType.Size = New System.Drawing.Size(150, 23)
        Me.cboUserType.TabIndex = 5
        '
        'cboActionCategory
        '
        Me.cboActionCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActionCategory.FormattingEnabled = True
        Me.cboActionCategory.Location = New System.Drawing.Point(510, 35)
        Me.cboActionCategory.Name = "cboActionCategory"
        Me.cboActionCategory.Size = New System.Drawing.Size(150, 23)
        Me.cboActionCategory.TabIndex = 7
        '
        'cboSourceSystem
        '
        Me.cboSourceSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSourceSystem.FormattingEnabled = True
        Me.cboSourceSystem.Location = New System.Drawing.Point(15, 90)
        Me.cboSourceSystem.Name = "cboSourceSystem"
        Me.cboSourceSystem.Size = New System.Drawing.Size(150, 23)
        Me.cboSourceSystem.TabIndex = 9
        '
        'cboStatus
        '
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.FormattingEnabled = True
        Me.cboStatus.Location = New System.Drawing.Point(180, 90)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(150, 23)
        Me.cboStatus.TabIndex = 11
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtSearch.Location = New System.Drawing.Point(345, 90)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(250, 25)
        Me.txtSearch.TabIndex = 12
        '
        'btnApplyFilters
        '
        Me.btnApplyFilters.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.btnApplyFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApplyFilters.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnApplyFilters.ForeColor = System.Drawing.Color.White
        Me.btnApplyFilters.Location = New System.Drawing.Point(15, 130)
        Me.btnApplyFilters.Name = "btnApplyFilters"
        Me.btnApplyFilters.Size = New System.Drawing.Size(100, 30)
        Me.btnApplyFilters.TabIndex = 14
        Me.btnApplyFilters.Text = "Apply Filters"
        Me.btnApplyFilters.UseVisualStyleBackColor = False
        '
        'btnResetFilters
        '
        Me.btnResetFilters.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnResetFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnResetFilters.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnResetFilters.ForeColor = System.Drawing.Color.White
        Me.btnResetFilters.Location = New System.Drawing.Point(125, 130)
        Me.btnResetFilters.Name = "btnResetFilters"
        Me.btnResetFilters.Size = New System.Drawing.Size(100, 30)
        Me.btnResetFilters.TabIndex = 15
        Me.btnResetFilters.Text = "Reset Filters"
        Me.btnResetFilters.UseVisualStyleBackColor = False
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSearch.ForeColor = System.Drawing.Color.White
        Me.btnSearch.Location = New System.Drawing.Point(610, 88)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(80, 28)
        Me.btnSearch.TabIndex = 13
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'btnExportCSV
        '
        Me.btnExportCSV.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnExportCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportCSV.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnExportCSV.ForeColor = System.Drawing.Color.White
        Me.btnExportCSV.Location = New System.Drawing.Point(235, 130)
        Me.btnExportCSV.Name = "btnExportCSV"
        Me.btnExportCSV.Size = New System.Drawing.Size(100, 30)
        Me.btnExportCSV.TabIndex = 16
        Me.btnExportCSV.Text = "Export CSV"
        Me.btnExportCSV.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(23, Byte), Integer), CType(CType(162, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(345, 130)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(100, 30)
        Me.btnRefresh.TabIndex = 17
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnClearLogs
        '
        Me.btnClearLogs.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearLogs.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClearLogs.ForeColor = System.Drawing.Color.White
        Me.btnClearLogs.Location = New System.Drawing.Point(455, 130)
        Me.btnClearLogs.Name = "btnClearLogs"
        Me.btnClearLogs.Size = New System.Drawing.Size(100, 30)
        Me.btnClearLogs.TabIndex = 18
        Me.btnClearLogs.Text = "Clear Logs"
        Me.btnClearLogs.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.Controls.Add(Me.lblPageInfo)
        Me.Panel4.Controls.Add(Me.btnLastPage)
        Me.Panel4.Controls.Add(Me.btnNextPage)
        Me.Panel4.Controls.Add(Me.btnPrevPage)
        Me.Panel4.Controls.Add(Me.btnFirstPage)
        Me.Panel4.Controls.Add(Me.lblTotalLogs)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 640)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1200, 50)
        Me.Panel4.TabIndex = 2
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
        'lblTotalLogs
        '
        Me.lblTotalLogs.AutoSize = True
        Me.lblTotalLogs.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalLogs.Location = New System.Drawing.Point(12, 18)
        Me.lblTotalLogs.Name = "lblTotalLogs"
        Me.lblTotalLogs.Size = New System.Drawing.Size(75, 15)
        Me.lblTotalLogs.TabIndex = 0
        Me.lblTotalLogs.Text = "Total Logs: 0"
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblStartDate.Location = New System.Drawing.Point(15, 15)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(61, 15)
        Me.lblStartDate.TabIndex = 0
        Me.lblStartDate.Text = "Start Date:"
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblEndDate.Location = New System.Drawing.Point(180, 15)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(57, 15)
        Me.lblEndDate.TabIndex = 2
        Me.lblEndDate.Text = "End Date:"
        '
        'lblUserType
        '
        Me.lblUserType.AutoSize = True
        Me.lblUserType.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblUserType.Location = New System.Drawing.Point(345, 15)
        Me.lblUserType.Name = "lblUserType"
        Me.lblUserType.Size = New System.Drawing.Size(61, 15)
        Me.lblUserType.TabIndex = 4
        Me.lblUserType.Text = "User Type:"
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblCategory.Location = New System.Drawing.Point(510, 15)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(58, 15)
        Me.lblCategory.TabIndex = 6
        Me.lblCategory.Text = "Category:"
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSource.Location = New System.Drawing.Point(15, 70)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(46, 15)
        Me.lblSource.TabIndex = 8
        Me.lblSource.Text = "Source:"
        '
        'lblStatusFilter
        '
        Me.lblStatusFilter.AutoSize = True
        Me.lblStatusFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblStatusFilter.Location = New System.Drawing.Point(180, 70)
        Me.lblStatusFilter.Name = "lblStatusFilter"
        Me.lblStatusFilter.Size = New System.Drawing.Size(42, 15)
        Me.lblStatusFilter.TabIndex = 10
        Me.lblStatusFilter.Text = "Status:"
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSearch.Location = New System.Drawing.Point(345, 70)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(45, 15)
        Me.lblSearch.TabIndex = 18
        Me.lblSearch.Text = "Search:"
        '
        'pnlFilters
        '
        Me.pnlFilters.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.pnlFilters.Controls.Add(Me.lblStartDate)
        Me.pnlFilters.Controls.Add(Me.dtpStartDate)
        Me.pnlFilters.Controls.Add(Me.lblEndDate)
        Me.pnlFilters.Controls.Add(Me.dtpEndDate)
        Me.pnlFilters.Controls.Add(Me.lblUserType)
        Me.pnlFilters.Controls.Add(Me.cboUserType)
        Me.pnlFilters.Controls.Add(Me.lblCategory)
        Me.pnlFilters.Controls.Add(Me.cboActionCategory)
        Me.pnlFilters.Controls.Add(Me.lblSource)
        Me.pnlFilters.Controls.Add(Me.cboSourceSystem)
        Me.pnlFilters.Controls.Add(Me.lblStatusFilter)
        Me.pnlFilters.Controls.Add(Me.cboStatus)
        Me.pnlFilters.Controls.Add(Me.lblSearch)
        Me.pnlFilters.Controls.Add(Me.txtSearch)
        Me.pnlFilters.Controls.Add(Me.btnSearch)
        Me.pnlFilters.Controls.Add(Me.btnApplyFilters)
        Me.pnlFilters.Controls.Add(Me.btnResetFilters)
        Me.pnlFilters.Controls.Add(Me.btnExportCSV)
        Me.pnlFilters.Controls.Add(Me.btnRefresh)
        Me.pnlFilters.Controls.Add(Me.btnClearLogs)
        Me.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFilters.Location = New System.Drawing.Point(0, 0)
        Me.pnlFilters.Name = "pnlFilters"
        Me.pnlFilters.Size = New System.Drawing.Size(1200, 170)
        Me.pnlFilters.TabIndex = 1
        '
        'ActivityLogsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 690)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.dgvActivityLogs)
        Me.Controls.Add(Me.pnlFilters)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MinimumSize = New System.Drawing.Size(1000, 600)
        Me.Name = "ActivityLogsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Activity Logs - System Audit Trail"
        CType(Me.dgvActivityLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.pnlFilters.ResumeLayout(False)
        Me.pnlFilters.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvActivityLogs As DataGridView
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents cboUserType As ComboBox
    Friend WithEvents cboActionCategory As ComboBox
    Friend WithEvents cboSourceSystem As ComboBox
    Friend WithEvents cboStatus As ComboBox
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnApplyFilters As Button
    Friend WithEvents btnResetFilters As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnExportCSV As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnClearLogs As Button
    Friend WithEvents lblStartDate As Label
    Friend WithEvents lblEndDate As Label
    Friend WithEvents lblUserType As Label
    Friend WithEvents lblCategory As Label
    Friend WithEvents lblSource As Label
    Friend WithEvents lblStatusFilter As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents pnlFilters As Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnFirstPage As System.Windows.Forms.Button
    Friend WithEvents btnPrevPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents btnLastPage As System.Windows.Forms.Button
    Friend WithEvents lblPageInfo As System.Windows.Forms.Label
    Friend WithEvents lblTotalLogs As System.Windows.Forms.Label
End Class