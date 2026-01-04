<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AdminDashboard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AdminDashboard))
        Me.SidePanel = New System.Windows.Forms.Panel()
        Me.Setting = New System.Windows.Forms.Button()
        Me.ActivityLogs = New System.Windows.Forms.Button()
        Me.Feedback = New System.Windows.Forms.Button()
        Me.Customer = New System.Windows.Forms.Button()
        Me.Employee = New System.Windows.Forms.Button()
        Me.Inventory = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.btnPayroll = New System.Windows.Forms.Button()
        Me.btnOrders = New System.Windows.Forms.Button()
        Me.btnReservations = New System.Windows.Forms.Button()
        Me.btnMenuItems = New System.Windows.Forms.Button()
        Me.btnUserAccounts = New System.Windows.Forms.Button()
        Me.btnDashboard = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SidePanel.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SidePanel
        '
        Me.SidePanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.SidePanel.Controls.Add(Me.Setting)
        Me.SidePanel.Controls.Add(Me.ActivityLogs)
        Me.SidePanel.Controls.Add(Me.Feedback)
        Me.SidePanel.Controls.Add(Me.Customer)
        Me.SidePanel.Controls.Add(Me.Employee)
        Me.SidePanel.Controls.Add(Me.Inventory)
        Me.SidePanel.Controls.Add(Me.btnReports)
        Me.SidePanel.Controls.Add(Me.btnPayroll)
        Me.SidePanel.Controls.Add(Me.btnOrders)
        Me.SidePanel.Controls.Add(Me.btnReservations)
        Me.SidePanel.Controls.Add(Me.btnMenuItems)
        Me.SidePanel.Controls.Add(Me.btnUserAccounts)
        Me.SidePanel.Controls.Add(Me.btnDashboard)
        Me.SidePanel.Controls.Add(Me.Panel3)
        Me.SidePanel.Controls.Add(Me.Panel2)
        Me.SidePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.SidePanel.Location = New System.Drawing.Point(0, 0)
        Me.SidePanel.Name = "SidePanel"
        Me.SidePanel.Size = New System.Drawing.Size(261, 660)
        Me.SidePanel.TabIndex = 0
        '
        'Setting
        '
        Me.Setting.FlatAppearance.BorderSize = 0
        Me.Setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Setting.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Setting.ForeColor = System.Drawing.Color.White
        Me.Setting.Image = CType(resources.GetObject("Setting.Image"), System.Drawing.Image)
        Me.Setting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Setting.Location = New System.Drawing.Point(20, 589)
        Me.Setting.Margin = New System.Windows.Forms.Padding(2)
        Me.Setting.Name = "Setting"
        Me.Setting.Size = New System.Drawing.Size(190, 28)
        Me.Setting.TabIndex = 13
        Me.Setting.Text = "     Settings"
        Me.Setting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Setting.UseVisualStyleBackColor = True
        '
        'ActivityLogs
        '
        Me.ActivityLogs.FlatAppearance.BorderSize = 0
        Me.ActivityLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ActivityLogs.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ActivityLogs.ForeColor = System.Drawing.Color.White
        Me.ActivityLogs.Image = CType(resources.GetObject("ActivityLogs.Image"), System.Drawing.Image)
        Me.ActivityLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ActivityLogs.Location = New System.Drawing.Point(20, 552)
        Me.ActivityLogs.Margin = New System.Windows.Forms.Padding(2)
        Me.ActivityLogs.Name = "ActivityLogs"
        Me.ActivityLogs.Size = New System.Drawing.Size(190, 28)
        Me.ActivityLogs.TabIndex = 8
        Me.ActivityLogs.Text = "     Activity Logs"
        Me.ActivityLogs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ActivityLogs.UseVisualStyleBackColor = True
        '
        'Feedback
        '
        Me.Feedback.FlatAppearance.BorderSize = 0
        Me.Feedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Feedback.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Feedback.ForeColor = System.Drawing.Color.White
        Me.Feedback.Image = CType(resources.GetObject("Feedback.Image"), System.Drawing.Image)
        Me.Feedback.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Feedback.Location = New System.Drawing.Point(20, 473)
        Me.Feedback.Name = "Feedback"
        Me.Feedback.Size = New System.Drawing.Size(190, 28)
        Me.Feedback.TabIndex = 12
        Me.Feedback.Text = "     Feedback"
        Me.Feedback.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Feedback.UseVisualStyleBackColor = True
        '
        'Customer
        '
        Me.Customer.FlatAppearance.BorderSize = 0
        Me.Customer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Customer.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Customer.ForeColor = System.Drawing.Color.White
        Me.Customer.Image = CType(resources.GetObject("Customer.Image"), System.Drawing.Image)
        Me.Customer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Customer.Location = New System.Drawing.Point(20, 433)
        Me.Customer.Name = "Customer"
        Me.Customer.Size = New System.Drawing.Size(190, 28)
        Me.Customer.TabIndex = 11
        Me.Customer.Text = "     Customer"
        Me.Customer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Customer.UseVisualStyleBackColor = True
        '
        'Employee
        '
        Me.Employee.FlatAppearance.BorderSize = 0
        Me.Employee.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Employee.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Employee.ForeColor = System.Drawing.Color.White
        Me.Employee.Image = CType(resources.GetObject("Employee.Image"), System.Drawing.Image)
        Me.Employee.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Employee.Location = New System.Drawing.Point(20, 392)
        Me.Employee.Name = "Employee"
        Me.Employee.Size = New System.Drawing.Size(190, 28)
        Me.Employee.TabIndex = 10
        Me.Employee.Text = "     Employee"
        Me.Employee.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Employee.UseVisualStyleBackColor = True
        '
        'Inventory
        '
        Me.Inventory.FlatAppearance.BorderSize = 0
        Me.Inventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Inventory.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Inventory.ForeColor = System.Drawing.Color.White
        Me.Inventory.Image = CType(resources.GetObject("Inventory.Image"), System.Drawing.Image)
        Me.Inventory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Inventory.Location = New System.Drawing.Point(20, 353)
        Me.Inventory.Name = "Inventory"
        Me.Inventory.Size = New System.Drawing.Size(190, 28)
        Me.Inventory.TabIndex = 9
        Me.Inventory.Text = "     Inventory"
        Me.Inventory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Inventory.UseVisualStyleBackColor = True
        '
        'btnReports
        '
        Me.btnReports.FlatAppearance.BorderSize = 0
        Me.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReports.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReports.ForeColor = System.Drawing.Color.White
        Me.btnReports.Image = CType(resources.GetObject("btnReports.Image"), System.Drawing.Image)
        Me.btnReports.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReports.Location = New System.Drawing.Point(20, 511)
        Me.btnReports.Margin = New System.Windows.Forms.Padding(2)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Size = New System.Drawing.Size(190, 28)
        Me.btnReports.TabIndex = 7
        Me.btnReports.Text = "     Reports"
        Me.btnReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnReports.UseVisualStyleBackColor = True
        '
        'btnPayroll
        '
        Me.btnPayroll.FlatAppearance.BorderSize = 0
        Me.btnPayroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPayroll.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPayroll.ForeColor = System.Drawing.Color.White
        Me.btnPayroll.Image = CType(resources.GetObject("btnPayroll.Image"), System.Drawing.Image)
        Me.btnPayroll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPayroll.Location = New System.Drawing.Point(20, 312)
        Me.btnPayroll.Name = "btnPayroll"
        Me.btnPayroll.Size = New System.Drawing.Size(190, 28)
        Me.btnPayroll.TabIndex = 6
        Me.btnPayroll.Text = "     Payroll"
        Me.btnPayroll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPayroll.UseVisualStyleBackColor = True
        '
        'btnOrders
        '
        Me.btnOrders.FlatAppearance.BorderSize = 0
        Me.btnOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOrders.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrders.ForeColor = System.Drawing.Color.White
        Me.btnOrders.Image = CType(resources.GetObject("btnOrders.Image"), System.Drawing.Image)
        Me.btnOrders.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOrders.Location = New System.Drawing.Point(20, 269)
        Me.btnOrders.Name = "btnOrders"
        Me.btnOrders.Size = New System.Drawing.Size(190, 28)
        Me.btnOrders.TabIndex = 5
        Me.btnOrders.Text = "     Orders"
        Me.btnOrders.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnOrders.UseVisualStyleBackColor = True
        '
        'btnReservations
        '
        Me.btnReservations.FlatAppearance.BorderSize = 0
        Me.btnReservations.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReservations.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReservations.ForeColor = System.Drawing.Color.White
        Me.btnReservations.Image = CType(resources.GetObject("btnReservations.Image"), System.Drawing.Image)
        Me.btnReservations.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReservations.Location = New System.Drawing.Point(20, 227)
        Me.btnReservations.Name = "btnReservations"
        Me.btnReservations.Size = New System.Drawing.Size(190, 28)
        Me.btnReservations.TabIndex = 4
        Me.btnReservations.Text = "     Reservations"
        Me.btnReservations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnReservations.UseVisualStyleBackColor = True
        '
        'btnMenuItems
        '
        Me.btnMenuItems.FlatAppearance.BorderSize = 0
        Me.btnMenuItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMenuItems.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMenuItems.ForeColor = System.Drawing.Color.White
        Me.btnMenuItems.Image = CType(resources.GetObject("btnMenuItems.Image"), System.Drawing.Image)
        Me.btnMenuItems.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMenuItems.Location = New System.Drawing.Point(20, 187)
        Me.btnMenuItems.Name = "btnMenuItems"
        Me.btnMenuItems.Size = New System.Drawing.Size(190, 28)
        Me.btnMenuItems.TabIndex = 3
        Me.btnMenuItems.Text = "     Menu Items"
        Me.btnMenuItems.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnMenuItems.UseVisualStyleBackColor = True
        '
        'btnUserAccounts
        '
        Me.btnUserAccounts.FlatAppearance.BorderSize = 0
        Me.btnUserAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUserAccounts.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUserAccounts.ForeColor = System.Drawing.Color.White
        Me.btnUserAccounts.Image = CType(resources.GetObject("btnUserAccounts.Image"), System.Drawing.Image)
        Me.btnUserAccounts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUserAccounts.Location = New System.Drawing.Point(20, 149)
        Me.btnUserAccounts.Name = "btnUserAccounts"
        Me.btnUserAccounts.Size = New System.Drawing.Size(190, 28)
        Me.btnUserAccounts.TabIndex = 2
        Me.btnUserAccounts.Text = "     User Accounts"
        Me.btnUserAccounts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnUserAccounts.UseVisualStyleBackColor = True
        '
        'btnDashboard
        '
        Me.btnDashboard.FlatAppearance.BorderSize = 0
        Me.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDashboard.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDashboard.ForeColor = System.Drawing.Color.White
        Me.btnDashboard.Image = Global.InformationManagement.My.Resources.Resources.dashboard__1_
        Me.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDashboard.Location = New System.Drawing.Point(20, 108)
        Me.btnDashboard.Name = "btnDashboard"
        Me.btnDashboard.Size = New System.Drawing.Size(190, 28)
        Me.btnDashboard.TabIndex = 0
        Me.btnDashboard.Text = "     Dashboard"
        Me.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnDashboard.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.btnLogout)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 617)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(261, 43)
        Me.Panel3.TabIndex = 1
        '
        'btnLogout
        '
        Me.btnLogout.FlatAppearance.BorderSize = 0
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Image = CType(resources.GetObject("btnLogout.Image"), System.Drawing.Image)
        Me.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogout.Location = New System.Drawing.Point(19, 7)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(190, 28)
        Me.btnLogout.TabIndex = 8
        Me.btnLogout.Text = "     Logout"
        Me.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(261, 86)
        Me.Panel2.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(52, 19)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(127, 51)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(261, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(539, 660)
        Me.Panel1.TabIndex = 1
        '
        'AdminDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 660)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.SidePanel)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "AdminDashboard"
        Me.SidePanel.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SidePanel As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnDashboard As Button
    Friend WithEvents btnUserAccounts As Button
    Friend WithEvents btnReports As Button
    Friend WithEvents btnPayroll As Button
    Friend WithEvents btnOrders As Button
    Friend WithEvents btnReservations As Button
    Friend WithEvents btnMenuItems As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Inventory As Button
    Friend WithEvents Employee As Button
    Friend WithEvents Customer As Button
    Friend WithEvents Feedback As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ActivityLogs As Button
    Friend WithEvents Setting As Button
End Class