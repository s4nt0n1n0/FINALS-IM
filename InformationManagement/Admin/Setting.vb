Imports MySqlConnector

Public Class Setting

    Private Sub Setting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Default View
        SwitchView("Admin")
        
        ' Load Admin Data
        LoadCurrentUserData()
        
        ' Load DB Data
        LoadDBConfigData()

        ' Show password by default
        chkShowPassword.Checked = True
    End Sub
    

    Private Sub LoadCurrentUserData()
        Try
            ' Load the current logged-in admin's data
            txtName.Text = CurrentLoggedUser.name
            txtUsername.Text = CurrentLoggedUser.username

            ' Populate current password (decrypted)
            If Not String.IsNullOrEmpty(CurrentLoggedUser.password) Then
                txtCurrentPassword.Text = Decrypt(CurrentLoggedUser.password)
            Else
                txtCurrentPassword.Text = ""
            End If

            txtNewPassword.Text = ""
        Catch ex As Exception
            MessageBox.Show("Error loading user data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadDBConfigData()
        modDB.LoadDatabaseConfig()
        txtServerIP.Text = modDB.db_server
        txtDBName.Text = modDB.db_name
        txtDBUser.Text = modDB.db_uid
        txtDBPass.Text = modDB.db_pwd
    End Sub

    ' VIEW SWITCHING LOGIC
    Private Sub btnViewAdmin_Click(sender As Object, e As EventArgs) Handles btnViewAdmin.Click
        SwitchView("Admin")
    End Sub

    Private Sub btnViewDB_Click(sender As Object, e As EventArgs) Handles btnViewDB.Click
        SwitchView("DB")
    End Sub

    Private Sub SwitchView(view As String)
        If view = "Admin" Then
            pnlAdminInfo.Visible = True
            pnlDBConfig.Visible = False
            lblTitle.Text = "Admin Information"

            ' Active Style
            btnViewAdmin.BackColor = Color.FromArgb(41, 128, 185)
            btnViewAdmin.ForeColor = Color.White

            ' Inactive Style
            btnViewDB.BackColor = Color.FromArgb(41, 128, 185)
            btnViewDB.ForeColor = Color.White
        Else
            pnlAdminInfo.Visible = False
            pnlDBConfig.Visible = True
            lblTitle.Text = "Database Configuration"

            ' Active Style
            btnViewDB.BackColor = Color.FromArgb(41, 128, 185)
            btnViewDB.ForeColor = Color.White

            ' Inactive Style
            btnViewAdmin.BackColor = Color.FromArgb(41, 128, 185)
            btnViewAdmin.ForeColor = Color.White
        End If
    End Sub

    Private Sub chkShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowPassword.CheckedChanged
        ' Toggle password visibility for RoundedTextBox
        If chkShowPassword.Checked Then
            txtCurrentPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Else
            txtCurrentPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)  ' Asterisk *
            txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)  ' Asterisk *
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If pnlAdminInfo.Visible Then
            SaveAdminInfo()
        ElseIf pnlDBConfig.Visible Then
            SaveDBConfig()
        End If
    End Sub

    Private Sub SaveDBConfig()
        Try
            If String.IsNullOrWhiteSpace(txtServerIP.Text) OrElse String.IsNullOrWhiteSpace(txtDBName.Text) Then
                MessageBox.Show("Server IP and Database Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            modDB.SaveDatabaseConfig(txtServerIP.Text.Trim(), txtDBName.Text.Trim(), txtDBUser.Text.Trim(), txtDBPass.Text.Trim())

            MessageBox.Show($"✔ These settings are stored in:{vbCrLf}db_config.ini which is in Bin/Debug folder.{vbCrLf}{vbCrLf}Configuration updated successfully!", 
                            "Database Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
             MessageBox.Show("Error saving DB config: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveAdminInfo()
        ' Validate inputs
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
            Return
        End If

        ' Check if trying to change password
        Dim isChangingPassword As Boolean = Not String.IsNullOrWhiteSpace(txtNewPassword.Text)

        If isChangingPassword Then
            ' If changing password, current password is required
            If String.IsNullOrWhiteSpace(txtCurrentPassword.Text) Then
                MessageBox.Show("Please enter your current password to change it.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCurrentPassword.Focus()
                Return
            End If

            ' Verify current password
            If Not VerifyCurrentPassword(txtCurrentPassword.Text) Then
                MessageBox.Show("Current password is incorrect.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtCurrentPassword.Focus()
                Return
            End If

            ' Validate new password length
            If txtNewPassword.Text.Length < 4 Then
                MessageBox.Show("New password must be at least 4 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtNewPassword.Focus()
                Return
            End If
        End If

        ' Confirm changes
        Dim confirmMessage As String = "Are you sure you want to update your account settings?"
        If isChangingPassword Then
            confirmMessage = "Are you sure you want to update your username and password?"
        End If

        Dim result As DialogResult = MessageBox.Show(confirmMessage, "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        ' Save changes
        Try
            openConn()

            ' Check if new username already exists (excluding current user)
            Dim checkQuery As String = "SELECT COUNT(*) FROM user_accounts WHERE username = @username AND id <> @id"
            Dim checkCmd As New MySqlCommand(checkQuery, conn)
            checkCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
            checkCmd.Parameters.AddWithValue("@id", CurrentLoggedUser.id)
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count > 0 Then
                MessageBox.Show("Username already exists. Please choose a different username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                closeConn()
                txtUsername.Focus()
                Return
            End If

            ' Update query
            Dim updateQuery As String
            Dim updateCmd As MySqlCommand

            If isChangingPassword Then
                ' Update both username and password
                Dim encryptedPassword As String = Encrypt(txtNewPassword.Text.Trim())
                updateQuery = "UPDATE user_accounts SET username = @username, password = @password WHERE id = @id"
                updateCmd = New MySqlCommand(updateQuery, conn)
                updateCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                updateCmd.Parameters.AddWithValue("@password", encryptedPassword)
                updateCmd.Parameters.AddWithValue("@id", CurrentLoggedUser.id)
            Else
                ' Update only username
                updateQuery = "UPDATE user_accounts SET username = @username WHERE id = @id"
                updateCmd = New MySqlCommand(updateQuery, conn)
                updateCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                updateCmd.Parameters.AddWithValue("@id", CurrentLoggedUser.id)
            End If

            updateCmd.ExecuteNonQuery()
            closeConn()

            ' Update the CurrentLoggedUser structure
            CurrentLoggedUser.username = txtUsername.Text.Trim()
            If isChangingPassword Then
                CurrentLoggedUser.password = Encrypt(txtNewPassword.Text.Trim())
            End If

            ' Log the activity with detailed information
            Dim changeDetails As String = ""
            If isChangingPassword Then
                changeDetails = $"Updated username to '{txtUsername.Text.Trim()}' and changed password"
                ActivityLogger.LogUserActivity(
                    "Account Settings Updated",
                    "User Management",
                    changeDetails,
                    "Admin Panel"
                )
            Else
                changeDetails = $"Updated username to '{txtUsername.Text.Trim()}'"
                ActivityLogger.LogUserActivity(
                    "Account Settings Updated",
                    "User Management",
                    changeDetails,
                    "Admin Panel"
                )
            End If

            ' Also log with old system for compatibility
            Logs("Updated account settings (Username: " & txtUsername.Text.Trim() & ")", "Settings_Save")

            MessageBox.Show("Account settings updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear password fields and reload data
            txtCurrentPassword.Text = ""
            txtNewPassword.Text = ""
            LoadCurrentUserData()

        Catch ex As Exception
            MessageBox.Show("Error updating account settings: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Function VerifyCurrentPassword(password As String) As Boolean
        Try
            ' Check if CurrentLoggedUser.password is populated
            If String.IsNullOrEmpty(CurrentLoggedUser.password) Then
                MessageBox.Show("Session password not found. Please log out and log in again.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' Encrypt the entered password and compare with stored password
            Dim encryptedPassword As String = Encrypt(password)
            Return encryptedPassword = CurrentLoggedUser.password
        Catch ex As Exception
            MessageBox.Show("Error verifying password: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class