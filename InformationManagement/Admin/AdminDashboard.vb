Imports Org.BouncyCastle.Math.EC

Public Class AdminDashboard
    Private ReportsInstance As Reports = Nothing
    Private isLoggingOut As Boolean = False
    Public Sub New()
        InitializeComponent()
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or
                    ControlStyles.AllPaintingInWmPaint, True)
        Me.UpdateStyles()
    End Sub


    Private Sub MakeRoundedButton(btn As Button, radius As Integer)
        Dim path As New Drawing2D.GraphicsPath()
        Dim rect As New Rectangle(0, 0, btn.Width, btn.Height)

        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90)
        path.CloseAllFigures()

        btn.Region = New Region(path)
    End Sub
    Private currentButton As Button = Nothing

    Private Sub HighlightButton(clickedButton As Button)
        If currentButton IsNot Nothing Then
            currentButton.BackColor = Color.FromArgb(26, 38, 50)
        End If
        clickedButton.BackColor = Color.FromArgb(110, 120, 135)
        currentButton = clickedButton
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        HighlightButton(btnDashboard)
        With Dashboard
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Dashboard)
            .BringToFront()
            .Show()
        End With

    End Sub

    Private Sub btnMenuItems_Click(sender As Object, e As EventArgs) Handles btnMenuItems.Click
        HighlightButton(btnMenuItems)
        With MenuItems
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(MenuItems)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub btnReservations_Click(sender As Object, e As EventArgs) Handles btnReservations.Click
        HighlightButton(btnReservations)
        With Reservations
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Reservations)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub btnUserAccounts_Click(sender As Object, e As EventArgs) Handles btnUserAccounts.Click
        HighlightButton(btnUserAccounts)

        ' Clear the panel first
        Panel1.Controls.Clear()

        ' Create a NEW instance of the form each time
        Dim userAccountsForm As New UsersAccounts()

        With userAccountsForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Add(userAccountsForm)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub btnOrders_Click(sender As Object, e As EventArgs) Handles btnOrders.Click
        HighlightButton(btnOrders)
        With Orders
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Orders)
            .BringToFront()
            .Show()
        End With

    End Sub

    Private Sub btnPayroll_Click(sender As Object, e As EventArgs) Handles btnPayroll.Click
        HighlightButton(btnPayroll)
        With Payroll
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Payroll)
            .BringToFront()
            .Show()
        End With

    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        HighlightButton(btnReports)
        ' Dispose old Reports instance if exists

        With Reports
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Reports)
            .BringToFront()
            .Show()
        End With

    End Sub

    Private Sub Inventory_Click(sender As Object, e As EventArgs) Handles Inventory.Click
        HighlightButton(Inventory)

        ' Create an instance of your Inventory form
        Dim inventoryForm As New Inventory() ' Make sure "Inventory" is your FORM, not the button name

        With inventoryForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(inventoryForm) ' ✅ Correct: add the FORM, not the button
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Employee_Click(sender As Object, e As EventArgs) Handles Employee.Click
        HighlightButton(Employee)

        ' Create an instance of your Employee form
        Dim employeeForm As New Employee() ' Correct variable name

        With employeeForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill

            Panel1.Controls.Clear()
            Panel1.Controls.Add(employeeForm)

            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Customer_Click(sender As Object, e As EventArgs) Handles Customer.Click
        HighlightButton(Customer)

        ' Create an instance of your Customer form
        Dim customerForm As New Customer()

        With customerForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill

            Panel1.Controls.Clear()
            Panel1.Controls.Add(customerForm)

            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Feedback_Click(sender As Object, e As EventArgs) Handles Feedback.Click
        HighlightButton(Feedback) ' ← Correct button to highlight

        ' Load Feedback form inside Panel1
        Dim feedbackForm As New Feedback() ' If this is really your Feedback form

        With feedbackForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill

            Panel1.Controls.Clear()
            Panel1.Controls.Add(feedbackForm)

            .BringToFront()
            .Show()
        End With
    End Sub


    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            isLoggingOut = True
            Dim loginForm As New Adminlogin()
            loginForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub AdminDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        MakeRoundedButton(btnDashboard, 15)
        MakeRoundedButton(btnMenuItems, 15)
        MakeRoundedButton(btnUserAccounts, 15)
        MakeRoundedButton(btnReservations, 15)
        MakeRoundedButton(btnOrders, 15)
        MakeRoundedButton(btnPayroll, 15)
        MakeRoundedButton(btnReports, 15)
        MakeRoundedButton(ActivityLogs, 15)
        MakeRoundedButton(Setting, 15)
        HighlightButton(btnDashboard)
        With Dashboard
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(Dashboard)
            .BringToFront()
            .Show()
        End With

    End Sub


    Private Sub AdminDashboard_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If Not isLoggingOut Then
            Application.Exit()
        End If
    End Sub

    Private Sub ActivityLogs_Click(sender As Object, e As EventArgs) Handles ActivityLogs.Click
        HighlightButton(ActivityLogs)

        ' Create an instance of ActivityLogs form
        Dim activityLogsForm As New ActivityLogsForm()

        With activityLogsForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(activityLogsForm)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Setting_Click(sender As Object, e As EventArgs) Handles Setting.Click
        HighlightButton(Setting)

        ' Create an instance of Setting form
        Dim settingsForm As New Setting()

        With settingsForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
            Panel1.Controls.Clear()
            Panel1.Controls.Add(settingsForm)
            .BringToFront()
            .Show()
        End With
    End Sub
End Class