Imports MySqlConnector

Public Class FormEdit
    Public Property UserID As Integer = 0
    Public Property LinkedEmployeeID As Integer = 0

    Private Sub FormEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RoundButton(btnAddUser)
        RoundButton(btnCancel)
        chkShowPass.Checked = True
    End Sub
    

    Public Sub LoadUserData(empId As Integer, username As String, role As String)
        LinkedEmployeeID = empId
        txtUsername.Text = username
        
        If UserID > 0 Then
             LoadEmployeeDetails(empId)
             
             ' Fetch and decrypt password
             Try
                 openConn()
                 Dim passQuery As String = "SELECT password FROM user_accounts WHERE id = @uid"
                 Dim passCmd As New MySqlCommand(passQuery, conn)
                 passCmd.Parameters.AddWithValue("@uid", UserID)
                 Dim cipherPass As Object = passCmd.ExecuteScalar()
                 
                 If cipherPass IsNot Nothing AndAlso cipherPass IsNot DBNull.Value Then
                     txtCurrentPassword.Text = Decrypt(cipherPass.ToString())
                 End If
             Catch ex As Exception
                 ' Ignore
             Finally
                 closeConn()
             End Try
        End If
    End Sub

    Private Sub LoadEmployeeDetails(empId As Integer)
        ' Fixed query to use CONCAT
        Try
            openConn()
            Dim sql As String = "SELECT CONCAT(FirstName, ' ', LastName) as Name FROM employee WHERE EmployeeID = @eid"
            Dim subCmd As New MySqlCommand(sql, conn)
            subCmd.Parameters.AddWithValue("@eid", empId)
            Dim result = subCmd.ExecuteScalar()
            
            If result IsNot Nothing Then
                txtFullName.Text = result.ToString()
            End If
        Catch ex As Exception
            MsgBox("Error loading details: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub chkShowPass_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowPass.CheckedChanged
        If chkShowPass.Checked Then
            txtCurrentPassword.PasswordChar = ControlChars.NullChar
            txtNewPassword.PasswordChar = ControlChars.NullChar
        Else
            txtCurrentPassword.PasswordChar = "*"c
            txtNewPassword.PasswordChar = "*"c
        End If
    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim newPassword As String = txtNewPassword.Text.Trim()
        
        If String.IsNullOrEmpty(username) Then
            MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            openConn()
            
            ' Check duplicate username
            Dim checkSql As String = "SELECT COUNT(*) FROM user_accounts WHERE username = @user AND id <> @uid"
            Dim checkCmd As New MySqlCommand(checkSql, conn)
            checkCmd.Parameters.AddWithValue("@user", username)
            checkCmd.Parameters.AddWithValue("@uid", UserID)
            
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            If count > 0 Then
                MessageBox.Show("Username already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Build Update Query
            Dim sql As String = ""
            Dim cmd As New MySqlCommand()
            cmd.Connection = conn
            
            If String.IsNullOrEmpty(newPassword) Then
                ' Update only username
                sql = "UPDATE user_accounts SET username = @user WHERE id = @uid"
            Else
                ' Update username and password
                Dim encryptedPass As String = Encrypt(newPassword)
                sql = "UPDATE user_accounts SET username = @user, password = @pass WHERE id = @uid"
                cmd.Parameters.AddWithValue("@pass", encryptedPass)
            End If
            
            cmd.CommandText = sql
            cmd.Parameters.AddWithValue("@user", username)
            cmd.Parameters.AddWithValue("@uid", UserID)
            
            cmd.ExecuteNonQuery()
            
            MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()
            
        Catch ex As Exception
            MessageBox.Show("Error updating user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
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