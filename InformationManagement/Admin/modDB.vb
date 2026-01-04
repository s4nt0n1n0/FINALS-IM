Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports MySqlConnector

Module modDB

    Public conn As New MySqlConnection()
    Public cmd As MySqlCommand
    Public cmdRead As MySqlDataReader

    Public db_server As String = "127.0.0.1"
    Public db_uid As String = "root"
    Public db_pwd As String = ""
    Public db_name As String = "tabeya_system"

    Private iniFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_config.ini")

    Public strConnection As String =
        $"Server={db_server};Port=3306;Database={db_name};Uid={db_uid};Pwd={db_pwd};AllowUserVariables=True;"

    Public Structure LoggedUser
        Dim id As Integer
        Dim name As String
        Dim position As String
        Dim username As String
        Dim password As String
        Dim type As Integer
    End Structure

    Public CurrentLoggedUser As LoggedUser

    ' ✔ Open connection
    Public Sub openConn()
        LoadDatabaseConfig() ' Always ensure we currently have the latest config before opening

        Try
            ' Robust check: if connection is anything other than Closed, close it first
            If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then
                conn.Close()
            End If
            
            conn.ConnectionString = strConnection
            conn.Open()
        Catch ex As Exception
            MsgBox("Connection Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Update Connection String
    Private Sub UpdateConnectionString()
        strConnection = $"Server={db_server};Port=3306;Database={db_name};Uid={db_uid};Pwd={db_pwd};AllowUserVariables=True;"
    End Sub

    ' LOAD DB Config from INI
    Public Sub LoadDatabaseConfig()
        Try
            ' Default path: Bin/Debug/db_config.ini
            If File.Exists(iniFilePath) Then
                Dim lines = File.ReadAllLines(iniFilePath)
                For Each line In lines
                    If line.StartsWith("Server=") Then
                        db_server = line.Substring(7).Trim()
                    ElseIf line.StartsWith("Database=") Then
                        db_name = line.Substring(9).Trim()
                    ElseIf line.StartsWith("Uid=") Then
                        db_uid = line.Substring(4).Trim()
                    ElseIf line.StartsWith("Pwd=") Then
                        db_pwd = line.Substring(4).Trim()
                    End If
                Next
                UpdateConnectionString()
            End If
        Catch ex As Exception
            ' Fallback to defaults if error
        End Try
    End Sub

    ' SAVE DB Config to INI
    Public Sub SaveDatabaseConfig(server As String, db As String, uid As String, pwd As String)
        Try
            Dim sb As New StringBuilder()
            sb.AppendLine("[Database]")
            sb.AppendLine($"Server={server}")
            sb.AppendLine($"Database={db}")
            sb.AppendLine($"Uid={uid}")
            sb.AppendLine($"Pwd={pwd}")

            File.WriteAllText(iniFilePath, sb.ToString())
            
            ' Update runtime variables
            db_server = server
            db_name = db
            db_uid = uid
            db_pwd = pwd
            UpdateConnectionString()

        Catch ex As Exception
            Throw New Exception("Failed to save configuration: " & ex.Message)
        End Try
    End Sub

    ' ✔ Close connection (ADDED)
    Public Sub closeConn()
        Try
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' ✔ Read Query
    Public Sub readQuery(ByVal sql As String)
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            cmdRead = cmd.ExecuteReader()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' ✔ Load to DGV
    Function LoadToDGV(query As String, dgv As DataGridView, filter As String) As Integer
        Try
            readQuery(query)
            Dim dt As New DataTable
            dt.Load(cmdRead)
            dgv.DataSource = dt
            dgv.Refresh()
            closeConn() ' ← Added here for cleanup
            Return dgv.Rows.Count
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return 0
    End Function

    ' ✔ Encryption
    Public Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey,
                New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function

    ' ✔ Decrypt
    Public Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey,
                New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

    ' ✔ Log event
    Sub Logs(transaction As String, Optional events As String = "*_Click")
        Try
            readQuery($"INSERT INTO logs(dt, user_accounts_id, event, transactions)
                       VALUES (NOW(), {CurrentLoggedUser.id}, '{events}', '{transaction}')")
            closeConn() ' ← added to prevent open connection lock
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ' ✔ Check and Create Tables
    Public Sub CheckAndCreateTables()
        Try
            openConn()

            ' 1. Create user_accounts table
            Dim sqlUser As String = "
                CREATE TABLE IF NOT EXISTS user_accounts (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    employee_id INT NULL,
                    name VARCHAR(100) NOT NULL,
                    username VARCHAR(50) UNIQUE NOT NULL,
                    password VARCHAR(255) NOT NULL,
                    type INT NOT NULL DEFAULT 1,
                    position VARCHAR(100) NULL,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                )"
            Dim cmdUser As New MySqlCommand(sqlUser, conn)
            cmdUser.ExecuteNonQuery()

            ' Ensure employee_id column exists (for older databases)
            Try
                Dim colCheckSql As String = "SELECT COUNT(*) FROM information_schema.COLUMNS " &
                                            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'user_accounts' AND COLUMN_NAME = 'employee_id'"
                Using colCheckCmd As New MySqlCommand(colCheckSql, conn)
                    Dim colCount As Integer = Convert.ToInt32(colCheckCmd.ExecuteScalar())
                    If colCount = 0 Then
                        Using alterCmd As New MySqlCommand("ALTER TABLE user_accounts ADD COLUMN employee_id INT NULL", conn)
                            alterCmd.ExecuteNonQuery()
                        End Using
                    End If
                End Using
            Catch
                ' Best-effort
            End Try

            ' Ensure status column exists
            Try
                Dim colCheckSqlStatus As String = "SELECT COUNT(*) FROM information_schema.COLUMNS " &
                                            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'user_accounts' AND COLUMN_NAME = 'status'"
                Using colCheckCmd As New MySqlCommand(colCheckSqlStatus, conn)
                    Dim colCount As Integer = Convert.ToInt32(colCheckCmd.ExecuteScalar())
                    If colCount = 0 Then
                        Using alterCmd As New MySqlCommand("ALTER TABLE user_accounts ADD COLUMN status VARCHAR(50) DEFAULT 'Active'", conn)
                            alterCmd.ExecuteNonQuery()
                        End Using
                    End If
                End Using
            Catch
                ' Best-effort
            End Try

            ' 2. Create payroll table
            Dim sqlPayroll As String = "
                CREATE TABLE IF NOT EXISTS payroll (
                    PayrollID INT PRIMARY KEY AUTO_INCREMENT,
                    EmployeeID INT NOT NULL,
                    PayPeriodStart DATE NOT NULL,
                    PayPeriodEnd DATE NOT NULL,
                    BasicSalary DECIMAL(10,2) NOT NULL,
                    Overtime DECIMAL(10,2) DEFAULT 0,
                    Deductions DECIMAL(10,2) DEFAULT 0,
                    Bonuses DECIMAL(10,2) DEFAULT 0,
                    NetPay DECIMAL(10,2) GENERATED ALWAYS AS (BasicSalary + Overtime + Bonuses - Deductions) STORED,
                    Status ENUM('Pending', 'Approved', 'Paid') DEFAULT 'Pending',
                    ProcessedBy INT NULL,
                    ProcessedDate DATETIME NULL,
                    Notes TEXT NULL,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (EmployeeID) REFERENCES employee(EmployeeID) ON DELETE CASCADE
                )"
            Dim cmdPayroll As New MySqlCommand(sqlPayroll, conn)
            cmdPayroll.ExecuteNonQuery()
            ' Add this inside CheckAndCreateTables() method, after the payroll table creation

            ' 3. Create activity_logs table
            Dim sqlActivityLogs As String = "
                CREATE TABLE IF NOT EXISTS activity_logs (
                    LogID INT PRIMARY KEY AUTO_INCREMENT,
                    UserType ENUM('Admin','Staff','Customer') NOT NULL COMMENT 'Type of user performing action',
                    UserID INT NULL COMMENT 'ID of user',
                    Username VARCHAR(100) NULL COMMENT 'Username or name of user',
                    Action VARCHAR(255) NOT NULL COMMENT 'Action performed',
                    ActionCategory ENUM('Login','Logout','Order','Reservation','Payment','Inventory','Product','User Management','Report','System') NOT NULL COMMENT 'Category of action',
                    Description TEXT NULL COMMENT 'Detailed description',
                    SourceSystem ENUM('POS','Website','Admin Panel') NOT NULL COMMENT 'System where action occurred',
                    ReferenceID VARCHAR(50) NULL COMMENT 'Reference ID',
                    ReferenceTable VARCHAR(100) NULL COMMENT 'Table name affected',
                    OldValue TEXT NULL COMMENT 'Previous value',
                    NewValue TEXT NULL COMMENT 'New value',
                    Status ENUM('Success','Failed','Warning') DEFAULT 'Success',
                    SessionID VARCHAR(100) NULL,
                    Timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    INDEX idx_user_type (UserType),
                    INDEX idx_action_category (ActionCategory),
                    INDEX idx_timestamp (Timestamp),
                    INDEX idx_user_id (UserID),
                    INDEX idx_source_system (SourceSystem)
                )"
            Dim cmdActivityLogs As New MySqlCommand(sqlActivityLogs, conn)
            cmdActivityLogs.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("Error initializing database tables: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            closeConn()
        End Try
    End Sub

End Module