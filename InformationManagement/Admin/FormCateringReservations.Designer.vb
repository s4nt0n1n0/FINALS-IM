<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCateringReservations
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCateringReservations))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.LabelSubHeader = New System.Windows.Forms.Label()
        Me.LabelSubtitle1 = New System.Windows.Forms.Label()
        Me.LabelSubtitle2 = New System.Windows.Forms.Label()
        Me.LabelSubtitle3 = New System.Windows.Forms.Label()
        Me.RoundedPane21 = New InformationManagement.RoundedPane2()
        Me.RoundedPane221 = New InformationManagement.RoundedPane2()
        Me.PictureBox11 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.RoundedPane22 = New InformationManagement.RoundedPane2()
        Me.RoundedPane25 = New InformationManagement.RoundedPane2()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RoundedPane23 = New InformationManagement.RoundedPane2()
        Me.RoundedPane218 = New InformationManagement.RoundedPane2()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RoundedPane24 = New InformationManagement.RoundedPane2()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Period = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Reservations = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalGuests = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PaginationContainer = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.lblPageStatus = New System.Windows.Forms.Label()
        Me.RoundedPane21.SuspendLayout()
        Me.RoundedPane221.SuspendLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane22.SuspendLayout()
        Me.RoundedPane25.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane23.SuspendLayout()
        Me.RoundedPane218.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RoundedPane24.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PaginationContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(75, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(133, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Reservations"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(75, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Total Events"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(75, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(150, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Total Revenue"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(32, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(209, 25)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Reservation Breakdown"
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoSize = True
        Me.LabelHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 22.0!, System.Drawing.FontStyle.Bold)
        Me.LabelHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.LabelHeader.Location = New System.Drawing.Point(30, 25)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(448, 41)
        Me.LabelHeader.TabIndex = 11
        Me.LabelHeader.Text = "Catering Reservations Analytics"
        '
        'LabelSubHeader
        '
        Me.LabelSubHeader.AutoSize = True
        Me.LabelSubHeader.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.LabelSubHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.LabelSubHeader.Location = New System.Drawing.Point(34, 68)
        Me.LabelSubHeader.Name = "LabelSubHeader"
        Me.LabelSubHeader.Size = New System.Drawing.Size(406, 19)
        Me.LabelSubHeader.TabIndex = 12
        Me.LabelSubHeader.Text = "Detailed overview of reservation patterns and event performance"

        '
        'LabelSubtitle1
        '
        Me.LabelSubtitle1.AutoSize = True
        Me.LabelSubtitle1.BackColor = System.Drawing.Color.Transparent
        Me.LabelSubtitle1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LabelSubtitle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LabelSubtitle1.Location = New System.Drawing.Point(75, 50)
        Me.LabelSubtitle1.Name = "LabelSubtitle1"
        Me.LabelSubtitle1.Size = New System.Drawing.Size(85, 15)
        Me.LabelSubtitle1.TabIndex = 2
        Me.LabelSubtitle1.Text = "Total bookings"
        '
        'LabelSubtitle2
        '
        Me.LabelSubtitle2.AutoSize = True
        Me.LabelSubtitle2.BackColor = System.Drawing.Color.Transparent
        Me.LabelSubtitle2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LabelSubtitle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LabelSubtitle2.Location = New System.Drawing.Point(75, 50)
        Me.LabelSubtitle2.Name = "LabelSubtitle2"
        Me.LabelSubtitle2.Size = New System.Drawing.Size(101, 15)
        Me.LabelSubtitle2.TabIndex = 3
        Me.LabelSubtitle2.Text = "Confirmed events"
        '
        'LabelSubtitle3
        '
        Me.LabelSubtitle3.AutoSize = True
        Me.LabelSubtitle3.BackColor = System.Drawing.Color.Transparent
        Me.LabelSubtitle3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LabelSubtitle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LabelSubtitle3.Location = New System.Drawing.Point(75, 50)
        Me.LabelSubtitle3.Name = "LabelSubtitle3"
        Me.LabelSubtitle3.Size = New System.Drawing.Size(102, 15)
        Me.LabelSubtitle3.TabIndex = 3
        Me.LabelSubtitle3.Text = "Total collections"
        '
        'RoundedPane21
        '
        Me.RoundedPane21.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane21.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane21.BorderThickness = 1
        Me.RoundedPane21.Controls.Add(Me.LabelSubtitle1)
        Me.RoundedPane21.Controls.Add(Me.RoundedPane221)
        Me.RoundedPane21.Controls.Add(Me.Label5)
        Me.RoundedPane21.Controls.Add(Me.Label1)
        Me.RoundedPane21.CornerRadius = 15
        Me.RoundedPane21.FillColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(133, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.RoundedPane21.Location = New System.Drawing.Point(30, 105)
        Me.RoundedPane21.Name = "RoundedPane21"
        Me.RoundedPane21.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane21.TabIndex = 8
        '
        'RoundedPane221
        '
        Me.RoundedPane221.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane221.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane221.BorderThickness = 1
        Me.RoundedPane221.Controls.Add(Me.PictureBox11)
        Me.RoundedPane221.CornerRadius = 8
        Me.RoundedPane221.FillColor = System.Drawing.Color.FromArgb(CType(CType(96, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.RoundedPane221.Location = New System.Drawing.Point(25, 22)
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
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(75, 75)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 32)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "0"
        '
        'RoundedPane22
        '
        Me.RoundedPane22.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane22.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane22.BorderThickness = 1
        Me.RoundedPane22.Controls.Add(Me.LabelSubtitle2)
        Me.RoundedPane22.Controls.Add(Me.RoundedPane25)
        Me.RoundedPane22.Controls.Add(Me.Label6)
        Me.RoundedPane22.Controls.Add(Me.Label2)
        Me.RoundedPane22.CornerRadius = 15
        Me.RoundedPane22.FillColor = System.Drawing.Color.FromArgb(CType(CType(251, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.RoundedPane22.Location = New System.Drawing.Point(384, 105)
        Me.RoundedPane22.Name = "RoundedPane22"
        Me.RoundedPane22.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane22.TabIndex = 9
        '
        'RoundedPane25
        '
        Me.RoundedPane25.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane25.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane25.BorderThickness = 1
        Me.RoundedPane25.Controls.Add(Me.PictureBox1)
        Me.RoundedPane25.CornerRadius = 8
        Me.RoundedPane25.FillColor = System.Drawing.Color.FromArgb(CType(CType(251, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(36, Byte), Integer))
        Me.RoundedPane25.Location = New System.Drawing.Point(25, 22)
        Me.RoundedPane25.Name = "RoundedPane25"
        Me.RoundedPane25.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane25.TabIndex = 7
        '
        'PictureBox1
        '
        Me.PictureBox1.AccessibleName = "252, 166, 5"
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
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
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "0"
        '
        'RoundedPane23
        '
        Me.RoundedPane23.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane23.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane23.BorderThickness = 1
        Me.RoundedPane23.Controls.Add(Me.LabelSubtitle3)
        Me.RoundedPane23.Controls.Add(Me.RoundedPane218)
        Me.RoundedPane23.Controls.Add(Me.Label7)
        Me.RoundedPane23.Controls.Add(Me.Label3)
        Me.RoundedPane23.CornerRadius = 15
        Me.RoundedPane23.FillColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer))
        Me.RoundedPane23.Location = New System.Drawing.Point(738, 105)
        Me.RoundedPane23.Name = "RoundedPane23"
        Me.RoundedPane23.Size = New System.Drawing.Size(337, 125)
        Me.RoundedPane23.TabIndex = 10
        '
        'RoundedPane218
        '
        Me.RoundedPane218.BackColor = System.Drawing.Color.Transparent
        Me.RoundedPane218.BorderColor = System.Drawing.Color.Transparent
        Me.RoundedPane218.BorderThickness = 1
        Me.RoundedPane218.Controls.Add(Me.PictureBox5)
        Me.RoundedPane218.CornerRadius = 8
        Me.RoundedPane218.FillColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(113, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.RoundedPane218.Location = New System.Drawing.Point(25, 22)
        Me.RoundedPane218.Name = "RoundedPane218"
        Me.RoundedPane218.Size = New System.Drawing.Size(43, 38)
        Me.RoundedPane218.TabIndex = 6
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
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(75, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(78, 32)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "₱0.00"
        '
        'RoundedPane24
        '
        Me.RoundedPane24.BorderColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RoundedPane24.BorderThickness = 1
        Me.RoundedPane24.Controls.Add(Me.DataGridView1)
        Me.RoundedPane24.Controls.Add(Me.Label4)
        Me.RoundedPane24.Controls.Add(Me.PaginationContainer)
        Me.RoundedPane24.CornerRadius = 15
        Me.RoundedPane24.FillColor = System.Drawing.Color.White
        Me.RoundedPane24.Location = New System.Drawing.Point(30, 255)
        Me.RoundedPane24.Name = "RoundedPane24"
        Me.RoundedPane24.Size = New System.Drawing.Size(1045, 470)
        Me.RoundedPane24.TabIndex = 4
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridView1.ColumnHeadersHeight = 45
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Period, Me.Reservations, Me.TotalGuests, Me.TotalAmount})
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.DataGridView1.Location = New System.Drawing.Point(25, 61)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 40
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(993, 358)
        Me.DataGridView1.TabIndex = 6
        '
        'Period
        '
        Me.Period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Period.HeaderText = "Period"
        Me.Period.MinimumWidth = 150
        Me.Period.Name = "Period"
        '
        'Reservations
        '
        Me.Reservations.HeaderText = "Reservations"
        Me.Reservations.MinimumWidth = 6
        Me.Reservations.Name = "Reservations"
        Me.Reservations.Width = 200
        '
        'TotalGuests
        '
        Me.TotalGuests.HeaderText = "Total Guests"
        Me.TotalGuests.MinimumWidth = 6
        Me.TotalGuests.Name = "TotalGuests"
        Me.TotalGuests.Width = 300
        '
        'TotalAmount
        '
        Me.TotalAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.TotalAmount.HeaderText = "Total Amount"
        Me.TotalAmount.MinimumWidth = 150
        Me.TotalAmount.Name = "TotalAmount"
        Me.TotalAmount.Width = 150
        '
        'PaginationContainer
        '
        Me.PaginationContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PaginationContainer.BackColor = System.Drawing.Color.Transparent
        Me.PaginationContainer.Controls.Add(Me.btnNext)
        Me.PaginationContainer.Controls.Add(Me.btnPrev)
        Me.PaginationContainer.Controls.Add(Me.lblPageStatus)
        Me.PaginationContainer.Location = New System.Drawing.Point(25, 420)
        Me.PaginationContainer.Name = "PaginationContainer"
        Me.PaginationContainer.Size = New System.Drawing.Size(993, 40)
        Me.PaginationContainer.TabIndex = 12
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNext.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.btnNext.Location = New System.Drawing.Point(908, 5)
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
        Me.btnPrev.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.btnPrev.Location = New System.Drawing.Point(822, 5)
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
        Me.lblPageStatus.Location = New System.Drawing.Point(10, 12)
        Me.lblPageStatus.Name = "lblPageStatus"
        Me.lblPageStatus.Size = New System.Drawing.Size(65, 15)
        Me.lblPageStatus.TabIndex = 0
        Me.lblPageStatus.Text = "Page 1 of 1"
        '
        'FormCateringReservations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1150, 609)
        Me.Controls.Add(Me.LabelSubHeader)
        Me.Controls.Add(Me.LabelHeader)
        Me.Controls.Add(Me.RoundedPane24)
        Me.Controls.Add(Me.RoundedPane23)
        Me.Controls.Add(Me.RoundedPane22)
        Me.Controls.Add(Me.RoundedPane21)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Name = "FormCateringReservations"
        Me.Text = "Catering Reservations Analytics"
        Me.RoundedPane21.ResumeLayout(False)
        Me.RoundedPane21.PerformLayout()
        Me.RoundedPane221.ResumeLayout(False)
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane22.ResumeLayout(False)
        Me.RoundedPane22.PerformLayout()
        Me.RoundedPane25.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane23.ResumeLayout(False)
        Me.RoundedPane23.PerformLayout()
        Me.RoundedPane218.ResumeLayout(False)
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RoundedPane24.ResumeLayout(False)
        Me.RoundedPane24.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PaginationContainer.ResumeLayout(False)
        Me.PaginationContainer.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LabelHeader As Label
    Friend WithEvents LabelSubHeader As Label
    Friend WithEvents LabelSubtitle1 As Label
    Friend WithEvents LabelSubtitle2 As Label
    Friend WithEvents LabelSubtitle3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Period As DataGridViewTextBoxColumn
    Friend WithEvents Reservations As DataGridViewTextBoxColumn
    Friend WithEvents TotalGuests As DataGridViewTextBoxColumn
    Friend WithEvents TotalAmount As DataGridViewTextBoxColumn

    Friend WithEvents RoundedPane21 As RoundedPane2
    Friend WithEvents RoundedPane22 As RoundedPane2
    Friend WithEvents RoundedPane23 As RoundedPane2
    Friend WithEvents RoundedPane24 As RoundedPane2
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents RoundedPane218 As RoundedPane2
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents RoundedPane221 As RoundedPane2
    Friend WithEvents PictureBox11 As PictureBox
    Friend WithEvents RoundedPane25 As RoundedPane2
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox25 As PictureBox
    Friend WithEvents PaginationContainer As Panel
    Friend WithEvents btnNext As Button
    Friend WithEvents btnPrev As Button
    Friend WithEvents lblPageStatus As Label
End Class
