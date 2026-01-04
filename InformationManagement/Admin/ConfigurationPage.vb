Imports System.IO
Imports System.Text
Imports MySqlConnector

Public Class ConfigurationPage
    Private configFolderPath As String = Path.Combine(Application.StartupPath, "Config")
    Private mainServerConfigPath As String = ""

    Private Sub ConfigurationPage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize configuration path
        InitializeConfigPath()

        ' Load existing configuration if available
        LoadConfiguration()

        ' Set default values if fields are empty
        If String.IsNullOrWhiteSpace(txtServer.Text) Then
            SetDefaultValues()
        End If
    End Sub

    Private Sub InitializeConfigPath()
        ' Create Config folder if it doesn't exist
        If Not Directory.Exists(configFolderPath) Then
            Directory.CreateDirectory(configFolderPath)
        End If

        ' Set configuration file path
        mainServerConfigPath = Path.Combine(configFolderPath, "MainServer.config")
    End Sub

    Private Sub SetDefaultValues()
        ' Set default XAMPP values
        txtServer.Text = "localhost"
        txtUsername.Text = "root"
        txtPassword.Text = ""
        txtDatabasename.Text = ""
    End Sub

    Private Sub LoadConfiguration()
        ' Load from file if exists
        If File.Exists(mainServerConfigPath) Then
            Try
                Dim lines As String() = File.ReadAllLines(mainServerConfigPath)
                For Each line As String In lines
                    If line.Contains("=") Then
                        Dim parts As String() = line.Split(New Char() {"="c}, 2)
                        If parts.Length = 2 Then
                            Dim key As String = parts(0).Trim()
                            Dim value As String = parts(1).Trim()

                            Select Case key.ToUpper()
                                Case "SERVER", "IP"
                                    txtServer.Text = value
                                Case "DATABASE"
                                    txtDatabasename.Text = value
                                Case "USERNAME"
                                    txtUsername.Text = value
                                Case "PASSWORD"
                                    txtPassword.Text = DecryptPassword(value)
                            End Select
                        End If
                    End If
                Next

                ' Update status label
                lblServerStatus.Text = "Configuration loaded successfully"
                lblServerStatus.ForeColor = Color.Green
            Catch ex As Exception
                MessageBox.Show($"Error loading configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SetDefaultValues()
                lblServerStatus.Text = "Failed to load configuration"
                lblServerStatus.ForeColor = Color.Red
            End Try
        Else
            lblServerStatus.Text = "No saved configuration found"
            lblServerStatus.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub SaveConfiguration()
        ' Validate fields
        If Not ValidateConfigFields() Then
            Return
        End If

        ' CRITICAL: Test connection before saving
        If Not TestConnectionSilent() Then
            Dim result = MessageBox.Show(
                "Connection test failed. The credentials you entered cannot connect to the database." & vbCrLf & vbCrLf &
                "Do you want to save anyway? (Not recommended)",
                "Connection Failed",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If result = DialogResult.No Then
                Return
            End If
        End If

        Try
            ' Build configuration content
            Dim sb As New StringBuilder()
            sb.AppendLine($"CONNECTION_TYPE=DATABASE")
            sb.AppendLine($"SERVER={txtServer.Text.Trim()}")
            sb.AppendLine($"DATABASE={txtDatabasename.Text.Trim()}")
            sb.AppendLine($"USERNAME={txtUsername.Text.Trim()}")
            sb.AppendLine($"PASSWORD={EncryptPassword(txtPassword.Text)}")
            sb.AppendLine($"SAVED_DATE={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}")

            ' Save to file
            File.WriteAllText(mainServerConfigPath, sb.ToString())

            ' Update modDB connection string
            UpdateModDBConnectionString()

            lblServerStatus.Text = "Configuration saved successfully ✓"
            lblServerStatus.ForeColor = Color.Green

            MessageBox.Show("Main Server configuration saved successfully!" & vbCrLf & vbCrLf &
                          "The system will use these credentials to connect to the database.",
                          "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            lblServerStatus.Text = "Failed to save configuration"
            lblServerStatus.ForeColor = Color.Red
            MessageBox.Show($"Error saving configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ValidateConfigFields() As Boolean
        ' Validate Server IP
        If String.IsNullOrWhiteSpace(txtServer.Text) Then
            MessageBox.Show("Please enter Server IP address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtServer.Focus()
            Return False
        End If





        ' Validate Database Name
        If String.IsNullOrWhiteSpace(txtDatabasename.Text) Then
            MessageBox.Show("Please enter Database name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDatabasename.Focus()
            Return False
        End If

        ' Validate Username
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            MessageBox.Show("Please enter Username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
            Return False
        End If

        ' Validate Database Name format (no special characters that could cause issues)
        If Not System.Text.RegularExpressions.Regex.IsMatch(txtDatabasename.Text, "^[a-zA-Z0-9_]+$") Then
            MessageBox.Show("Database name can only contain letters, numbers, and underscores.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDatabasename.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearFields()
        txtServer.Text = ""
        txtDatabasename.Text = ""
        txtUsername.Text = "root"
        txtPassword.Text = ""
        lblServerStatus.Text = ""
    End Sub

    ' Simple encryption/decryption (use stronger encryption in production)
    Private Function EncryptPassword(password As String) As String
        If String.IsNullOrEmpty(password) Then Return ""
        Try
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Return Convert.ToBase64String(bytes)
        Catch
            Return password
        End Try
    End Function

    Private Function DecryptPassword(encryptedPassword As String) As String
        If String.IsNullOrEmpty(encryptedPassword) Then Return ""
        Try
            Dim bytes As Byte() = Convert.FromBase64String(encryptedPassword)
            Return Encoding.UTF8.GetString(bytes)
        Catch
            Return encryptedPassword
        End Try
    End Function

    ' Test database connection with user feedback
    Private Sub TestConnection()
        ' Validate fields first
        If Not ValidateConfigFields() Then
            Return
        End If

        Dim connectionString As String = BuildConnectionString()

        Try
            Using conn As New MySqlConnection(connectionString)
                Me.Cursor = Cursors.WaitCursor
                lblServerStatus.Text = "Testing connection..."
                lblServerStatus.ForeColor = Color.Orange
                Application.DoEvents()

                conn.Open()

                ' Verify database exists and we can query it
                Using cmd As New MySqlCommand("SELECT DATABASE()", conn)
                    Dim dbName As Object = cmd.ExecuteScalar()
                    If dbName Is Nothing OrElse String.IsNullOrEmpty(dbName.ToString()) Then
                        Throw New Exception("Connected but database not selected properly.")
                    End If
                End Using

                Me.Cursor = Cursors.Default
                lblServerStatus.Text = "Connection successful ✓"
                lblServerStatus.ForeColor = Color.Green

                MessageBox.Show("Connection to Main Server successful!" & vbCrLf & vbCrLf &
                              $"Server: {txtServer.Text}" & vbCrLf &
                              $"Database: {txtDatabasename.Text}" & vbCrLf &
                              $"Username: {txtUsername.Text}" & vbCrLf & vbCrLf &
                              "These credentials are correct and can be saved.",
                              "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                conn.Close()
            End Using
        Catch ex As MySqlException
            Me.Cursor = Cursors.Default
            lblServerStatus.Text = "Connection failed ✗"
            lblServerStatus.ForeColor = Color.Red

            Dim errorDetails As String = ""
            Dim solution As String = ""

            Select Case ex.Number
                Case 1045 ' Access denied
                    errorDetails = "Access denied. Username or password is incorrect."
                    solution = "Please verify your MySQL username and password are correct."

                Case 1130 ' Host not allowed
                    Dim clientHostname As String = System.Net.Dns.GetHostName()
                    Dim clientIP As String = ""

                    ' Try to get client IP
                    Try
                        Dim host = System.Net.Dns.GetHostEntry(clientHostname)
                        If host.AddressList.Length > 0 Then
                            For Each addr In host.AddressList
                                If addr.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                                    clientIP = addr.ToString()
                                    Exit For
                                End If
                            Next
                        End If
                    Catch
                        ' Ignore if can't get IP
                    End Try

                    errorDetails = $"Host '{clientHostname}' (IP: {If(String.IsNullOrEmpty(clientIP), "unknown", clientIP)}) is not allowed to connect to this MariaDB/MySQL server."

                    Dim passwordPart As String = If(String.IsNullOrEmpty(txtPassword.Text), "", $"IDENTIFIED BY '{txtPassword.Text}'")

                    solution = "SOLUTION - Run these commands on the MySQL/MariaDB SERVER:" & vbCrLf & vbCrLf &
                              "Option 1: Allow connection from your specific IP (RECOMMENDED):" & vbCrLf &
                              If(String.IsNullOrEmpty(clientIP),
                                 $"GRANT ALL PRIVILEGES ON *.* TO '{txtUsername.Text}'@'YOUR_CLIENT_IP' {passwordPart} WITH GRANT OPTION;",
                                 $"GRANT ALL PRIVILEGES ON *.* TO '{txtUsername.Text}'@'{clientIP}' {passwordPart} WITH GRANT OPTION;") & vbCrLf &
                              "FLUSH PRIVILEGES;" & vbCrLf & vbCrLf &
                              "Option 2: Allow connection from any IP (LESS SECURE):" & vbCrLf &
                              $"GRANT ALL PRIVILEGES ON *.* TO '{txtUsername.Text}'@'%' {passwordPart} WITH GRANT OPTION;" & vbCrLf &
                              "FLUSH PRIVILEGES;" & vbCrLf & vbCrLf &
                              "Option 3: Allow connection from IP range (e.g., 192.168.1.%):" & vbCrLf &
                              $"GRANT ALL PRIVILEGES ON *.* TO '{txtUsername.Text}'@'192.168.1.%' {passwordPart} WITH GRANT OPTION;" & vbCrLf &
                              "FLUSH PRIVILEGES;" & vbCrLf & vbCrLf &
                              "IMPORTANT: Also check that:" & vbCrLf &
                              "1. MySQL/MariaDB is configured to accept remote connections" & vbCrLf &
                              "   (bind-address should be 0.0.0.0 or commented out in my.ini/my.cnf)" & vbCrLf &
                              "2. Firewall allows port 3306" & vbCrLf &
                              "3. The database exists on the server"

                Case 1049 ' Database doesn't exist
                    errorDetails = "Database does not exist. Please verify the database name."
                    solution = $"Create the database first by running:" & vbCrLf &
                              $"CREATE DATABASE {txtDatabasename.Text};"

                Case 0 ' Can't connect to server
                    errorDetails = "Cannot connect to MySQL server. Server may not be running."
                    solution = "Please verify:" & vbCrLf &
                              "1. MySQL/MariaDB service is running" & vbCrLf &
                              "2. Server address and port are correct" & vbCrLf &
                              "3. Firewall is not blocking the connection"

                Case Else
                    errorDetails = ex.Message
                    solution = "Please check your server settings."
            End Select

            MessageBox.Show($"Connection failed!" & vbCrLf & vbCrLf &
                          $"Error: {errorDetails}" & vbCrLf & vbCrLf &
                          solution,
                          "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            lblServerStatus.Text = "Connection failed ✗"
            lblServerStatus.ForeColor = Color.Red

            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Silent connection test for validation (returns true/false)
    Private Function TestConnectionSilent() As Boolean
        If Not ValidateConfigFields() Then
            Return False
        End If

        Dim connectionString As String = BuildConnectionString()

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                ' Verify database exists
                Using cmd As New MySqlCommand("SELECT DATABASE()", conn)
                    Dim dbName As Object = cmd.ExecuteScalar()
                    If dbName Is Nothing OrElse String.IsNullOrEmpty(dbName.ToString()) Then
                        Return False
                    End If
                End Using

                conn.Close()
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function BuildConnectionString() As String
        Return $"Server={txtServer.Text.Trim()};Database={txtDatabasename.Text.Trim()};Uid={txtUsername.Text.Trim()};Pwd={txtPassword.Text};ConnectionTimeout=5;"
    End Function

    ' Update modDB connection string after saving config
    Private Sub UpdateModDBConnectionString()
        Try
            modDB.db_server = txtServer.Text.Trim()
            modDB.db_uid = txtUsername.Text.Trim()
            modDB.db_pwd = txtPassword.Text
            modDB.db_name = txtDatabasename.Text.Trim()

            modDB.strConnection = $"Server={modDB.db_server};Database={modDB.db_name};Uid={modDB.db_uid};Pwd={modDB.db_pwd};AllowUserVariables=True;"
        Catch ex As Exception
            ' Silent fail - not critical
        End Try
    End Sub

    ' Button Event Handlers
    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        TestConnection()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs)
        SaveConfiguration()
    End Sub

    Private Sub btnSaveAndContinue_Click(sender As Object, e As EventArgs) Handles btnSaveAndContinue.Click
        ' Validate fields
        If Not ValidateConfigFields() Then
            Return
        End If

        ' CRITICAL: Must test connection before proceeding
        lblServerStatus.Text = "Validating credentials..."
        lblServerStatus.ForeColor = Color.Orange
        Application.DoEvents()

        If Not TestConnectionSilent() Then
            lblServerStatus.Text = "Invalid credentials ✗"
            lblServerStatus.ForeColor = Color.Red

            MessageBox.Show(
                "Cannot proceed with invalid database credentials!" & vbCrLf & vbCrLf &
                "Please verify:" & vbCrLf &
                "• MySQL server is running" & vbCrLf &
                "• Database exists" & vbCrLf &
                "• Username and password are correct" & vbCrLf &
                "• User has proper permissions" & vbCrLf &
                "• Host is allowed to connect" & vbCrLf & vbCrLf &
                "Click 'Test Connection' to see detailed error information.",
                "Invalid Credentials",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
            Return
        End If

        ' Connection successful, save configuration
        SaveConfiguration()

        ' Verify config file was created
        If File.Exists(mainServerConfigPath) Then
            ' Initialize database tables
            Try
                UpdateModDBConnectionString()
                modDB.CheckAndCreateTables()
            Catch ex As Exception
                MessageBox.Show(
                    "Database connected but failed to initialize tables:" & vbCrLf & vbCrLf &
                    ex.Message & vbCrLf & vbCrLf &
                    "Please check database permissions.",
                    "Initialization Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return
            End Try

            ' Proceed to login
            Dim loginForm As New Adminlogin()
            loginForm.Show()
            Me.Hide()
        Else
            MessageBox.Show(
                "Configuration file was not saved properly." & vbCrLf &
                "Please try again.",
                "Save Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End If
    End Sub

    ' Optional: Add keyboard shortcuts
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.S) Then
            SaveConfiguration()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.T) Then
            TestConnection()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    ' Show configuration info in status label on focus
    Private Sub txtServer_Enter(sender As Object, e As EventArgs) Handles txtServer.Enter
        lblServerStatus.Text = "Enter the IP address or hostname of your MySQL server (e.g., localhost)"
        lblServerStatus.ForeColor = Color.Gray
    End Sub

    Private Sub txtPort_Enter(sender As Object, e As EventArgs)
        lblServerStatus.Text = "Default MySQL port is 3306"
        lblServerStatus.ForeColor = Color.Gray
    End Sub

    Private Sub txtDatabasename_Enter(sender As Object, e As EventArgs) Handles txtDatabasename.Enter
        lblServerStatus.Text = "Enter the exact name of your database (case-sensitive)"
        lblServerStatus.ForeColor = Color.Gray
    End Sub

    Private Sub txtUsername_Enter(sender As Object, e As EventArgs) Handles txtUsername.Enter
        lblServerStatus.Text = "Enter the database username (default: root for XAMPP)"
        lblServerStatus.ForeColor = Color.Gray
    End Sub

    Private Sub txtPassword_Enter(sender As Object, e As EventArgs) Handles txtPassword.Enter
        lblServerStatus.Text = "Enter the database password (leave empty if none for XAMPP)"
        lblServerStatus.ForeColor = Color.Gray
    End Sub
End Class