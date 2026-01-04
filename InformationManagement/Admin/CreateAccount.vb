Imports MySqlConnector

Public Class CreateAccount
    Public Property LinkedEmployeeID As Integer = 0

    Private Sub CreateAccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RoundButton(btnCreate)
        RoundButton(btnCancel)
        chkShowPass.Checked = True
    End Sub
    

    Public Sub LoadEmployeeData(id As Integer, name As String, role As String, email As String)
        LinkedEmployeeID = id
        txtFullName.Text = name
        
        ' Use email as username, fallback to EmployeeID if no email is provided
        txtUsername.Text = If(Not String.IsNullOrEmpty(email), email.Trim(), "user_" & id)
        
        ' Hide username fields as they are now automated
        LabelUser.Visible = False
        txtUsername.Visible = False
        
        ' Shift other controls up to fill the gap
        LabelPass.Top -= 60
        txtPassword.Top -= 60
        LabelRole.Top -= 60
        cmbRole.Top -= 60
        chkShowPass.Top -= 60
        btnCreate.Top -= 60
        btnCancel.Top -= 60
        Me.Height -= 60
        If role = "Staff" OrElse role = "Employee" Then
            cmbRole.SelectedItem = role
        Else
            cmbRole.SelectedItem = "Employee"
        End If
        
        ' Lock name as it comes from employee record
        txtFullName.Enabled = False
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        ' Validation
        Dim name As String = txtFullName.Text.Trim()
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()
        Dim role As String = If(cmbRole.SelectedItem IsNot Nothing, cmbRole.SelectedItem.ToString(), "")

        If String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(role) Then
            MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            openConn()
            
            ' Check duplicate username
            Dim checkSql As String = "SELECT COUNT(*) FROM user_accounts WHERE username = @user"
            Dim checkCmd As New MySqlCommand(checkSql, conn)
            checkCmd.Parameters.AddWithValue("@user", username)
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count > 0 Then
                MessageBox.Show("Username already exists. Please choose another.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Insert
            Dim userType As Integer = If(role = "Staff", 1, 2)
            Dim encryptedPass As String = Encrypt(password)

            Dim sql As String = "INSERT INTO user_accounts (employee_id, name, username, password, type, position, status, created_at) " & _
                                "VALUES (@eid, @name, @user, @pass, @type, @position, 'Active', NOW())"
                                
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@eid", LinkedEmployeeID)
            cmd.Parameters.AddWithValue("@name", name)
            cmd.Parameters.AddWithValue("@user", username)
            cmd.Parameters.AddWithValue("@pass", encryptedPass)
            cmd.Parameters.AddWithValue("@type", userType)
            cmd.Parameters.AddWithValue("@position", role)

            cmd.ExecuteNonQuery()
            
            MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()
            
        Catch ex As Exception
            MessageBox.Show("Error creating account: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub chkShowPass_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowPass.CheckedChanged
        If chkShowPass.Checked Then
            txtPassword.PasswordChar = ControlChars.NullChar
        Else
            txtPassword.PasswordChar = "*"c
        End If
    End Sub

    Private Sub RoundButton(btn As Button)
        Dim radius As Integer = 10
        Dim path As New Drawing2D.GraphicsPath()
        path.StartFigure()
        path.AddArc(New Rectangle(0, 0, radius, radius), 180, 90)
        path.AddArc(New Rectangle(btn.Width - radius, 0, radius, radius), 270, 90)
        path.AddArc(New Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90)
        path.AddArc(New Rectangle(0, btn.Height - radius, radius, radius), 90, 90)
        path.CloseFigure()
        btn.Region = New Region(path)
    End Sub
End Class