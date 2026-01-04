Imports MySqlConnector

Public Class UsersAccounts
    ' Pagination variables
    Private currentPage As Integer = 1
    Private pageSize As Integer = 20
    Private totalRecords As Integer = 0
    Private totalPages As Integer = 0
    Private allStaffData As DataTable
    Private searchText As String = ""
    Private _lastSearchText As String = ""
    Private isInitializing As Boolean = True
    Private initialLoadComplete As Boolean = False
    Private currentViewMode As String = "Staff" ' Types: "Staff", "Employee"
    Private WithEvents btnShowStaff As Button
    Private WithEvents btnShowEmployee As Button
    Private WithEvents btnUpdateStatus As Button
    Private WithEvents btnAddNew As Button ' Keep reference but might hide

    Private Sub UsersAccounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDataGridView()
        RoundPaginationButtons()
        SetupToggleButtons() ' Add toggle buttons
        ' SetupAddButton() ' Disable original Add button favor of Employee view workflow
        InitializeSearchBox()
        isInitializing = False
        LoadDataBasedOnMode()
        initialLoadComplete = True
        AdjustControlsToScreen()
    End Sub

    Private Sub UsersAccounts_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If initialLoadComplete Then
            AdjustControlsToScreen()
        End If
    End Sub

    Private Sub AdjustControlsToScreen()
        Try
            ' Get the form's client area dimensions
            Dim formWidth As Integer = Me.ClientSize.Width
            Dim formHeight As Integer = Me.ClientSize.Height

            ' Set margins
            Dim leftMargin As Integer = 30
            Dim rightMargin As Integer = 30
            Dim topMargin As Integer = 30

            ' Title Label
            Label1.Location = New Point(leftMargin, topMargin)

            ' Stats Card
            Dim statsCardWidth As Integer = Math.Min(300, (formWidth - leftMargin - rightMargin) \ 3)
            RoundedPane22.Width = statsCardWidth

            ' Add Button Position (Legacy or if needed)
            ' If btnAddNew IsNot Nothing Then
            '    btnAddNew.Location = New Point(formWidth - rightMargin - btnAddNew.Width, topMargin)
            ' End If

            ' Toggle Buttons Position - Fixed Overlay Issue
            If btnShowStaff IsNot Nothing AndAlso btnShowEmployee IsNot Nothing Then
                Dim buttonY As Integer = RoundedPane22.Bottom + 20 ' Place below the stats card
                btnShowStaff.Location = New Point(leftMargin, buttonY)
                btnShowEmployee.Location = New Point(btnShowStaff.Right + 10, buttonY)

                ' Position Update Status Button at Top Right (relative to where Add button was)
                If btnUpdateStatus IsNot Nothing Then
                    btnUpdateStatus.Location = New Point(formWidth - rightMargin - btnUpdateStatus.Width, topMargin + 10)
                End If
            End If

            ' DataGridView - Calculate available space
            Dim gridTop As Integer
            If btnShowStaff IsNot Nothing Then
                gridTop = btnShowStaff.Bottom + 20
            Else
                gridTop = RoundedPane22.Bottom + 20
            End If
            Dim gridWidth As Integer = formWidth - leftMargin - rightMargin
            Dim paginationHeight As Integer = 60
            Dim gridHeight As Integer = formHeight - gridTop - paginationHeight - 20

            UsersAccountData.Location = New Point(leftMargin, gridTop)
            UsersAccountData.Size = New Size(gridWidth, gridHeight)

            ' Adjust DataGridView column widths proportionally
            AdjustColumnWidths()

            ' Pagination Panel
            PaginationPanel.Location = New Point(leftMargin, UsersAccountData.Bottom + 10)
            PaginationPanel.Width = gridWidth

            ' Center pagination controls
            CenterPaginationControls()

        Catch ex As Exception
            ' Silently handle resize errors to prevent crashes
            Debug.WriteLine("Resize error: " & ex.Message)
        End Try
    End Sub

    Private Sub AdjustColumnWidths()
        Try
            If UsersAccountData.Columns.Count = 0 Then Return

            Dim totalWidth As Integer = UsersAccountData.Width - 20 ' Account for scrollbar

            ' Set column widths proportionally based on Designer columns
            If UsersAccountData.Columns.Contains("txtName") Then
                UsersAccountData.Columns("txtName").Width = CInt(totalWidth * 0.25) ' 25% - Name
            End If

            If UsersAccountData.Columns.Contains("colRole") Then
                UsersAccountData.Columns("colRole").Width = CInt(totalWidth * 0.15) ' 15% - Role
            End If

            If UsersAccountData.Columns.Contains("colStatus") Then
                UsersAccountData.Columns("colStatus").Width = CInt(totalWidth * 0.15) ' 15% - Status
            End If

            If UsersAccountData.Columns.Contains("colJoinDate") Then
                UsersAccountData.Columns("colJoinDate").Width = CInt(totalWidth * 0.25) ' 25% - Join Date
            End If

            If UsersAccountData.Columns.Contains("colUsername") Then
                UsersAccountData.Columns("colUsername").Width = CInt(totalWidth * 0.15)
            End If

            If UsersAccountData.Columns.Contains("colPassword") Then
                UsersAccountData.Columns("colPassword").Width = CInt(totalWidth * 0.15)
            End If

            If UsersAccountData.Columns.Contains("colEdit") Then
                UsersAccountData.Columns("colEdit").Width = 60
            End If

            If UsersAccountData.Columns.Contains("colDelete") Then
                UsersAccountData.Columns("colDelete").Width = 60
            End If

        Catch ex As Exception
            Debug.WriteLine("Column width adjustment error: " & ex.Message)
        End Try
    End Sub

    Private Sub CenterPaginationControls()
        Try
            Dim panelWidth As Integer = PaginationPanel.Width
            Dim totalButtonWidth As Integer = btnFirstPage.Width + btnPreviousPage.Width +
                                              btnNextPage.Width + btnLastPage.Width
            Dim spacing As Integer = 10
            Dim labelWidth As Integer = 100

            Dim totalWidth As Integer = totalButtonWidth + (spacing * 3) + labelWidth
            Dim startX As Integer = (panelWidth - totalWidth) \ 2

            btnFirstPage.Location = New Point(startX, btnFirstPage.Top)
            btnPreviousPage.Location = New Point(btnFirstPage.Right + spacing, btnPreviousPage.Top)
            lblPageInfo.Location = New Point(btnPreviousPage.Right + spacing, lblPageInfo.Top)
            lblPageInfo.Width = labelWidth
            btnNextPage.Location = New Point(lblPageInfo.Right + spacing, btnNextPage.Top)
            btnLastPage.Location = New Point(btnNextPage.Right + spacing, btnLastPage.Top)

        Catch ex As Exception
            Debug.WriteLine("Pagination centering error: " & ex.Message)
        End Try
    End Sub

    Private Sub InitializeDataGridView()
        ' Enable double buffering for smoother rendering
        UsersAccountData.DoubleBuffered(True)
        UsersAccountData.SuspendLayout()
        UsersAccountData.Rows.Clear()

        ' Column Header Styling
        With UsersAccountData
            .EnableHeadersVisualStyles = False
            .RowHeadersVisible = False ' Hide the row selector column
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 38, 50)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(26, 38, 50)
            .ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            
            ' Default Cell Style
            .DefaultCellStyle.BackColor = SystemColors.Window
            .DefaultCellStyle.Font = New Font("Segoe UI", 8.25F)
            .DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64)
            .DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
            .DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With

        ' Force styles on existing columns (Fix for mismatched header styles)
        For Each col As DataGridViewColumn In UsersAccountData.Columns
            col.HeaderCell.Style = Nothing ' Clear individual styles so they inherit default
        Next

        ' Ensure Name column has proper header
        If UsersAccountData.Columns.Contains("txtName") Then
            UsersAccountData.Columns("txtName").HeaderText = "Name"
        End If

        ' Add Create Account Action Column
        If Not UsersAccountData.Columns.Contains("colCreateAccount") Then
            Dim btnCol As New DataGridViewButtonColumn()
            btnCol.Name = "colCreateAccount"
            btnCol.HeaderText = "Action"
            btnCol.Text = "Create Account"
            btnCol.UseColumnTextForButtonValue = True
            UsersAccountData.Columns.Add(btnCol)
            btnCol.Visible = False
        End If

        UsersAccountData.ResumeLayout()

        ' Set alternating row colors for better readability
        UsersAccountData.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 255)

        ' Remove selection highlighting on focus
        UsersAccountData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 244, 250)
        UsersAccountData.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Add hidden columns for data handling
        If Not UsersAccountData.Columns.Contains("colUsername") Then
            UsersAccountData.Columns.Add("colUsername", "Username")
            UsersAccountData.Columns("colUsername").Visible = True
            If Not UsersAccountData.Columns.Contains("colPassword") Then
                UsersAccountData.Columns.Add("colPassword", "Password")
            End If
            UsersAccountData.Columns("colPassword").Visible = True
        End If

        If Not UsersAccountData.Columns.Contains("colEmployeeID") Then
            UsersAccountData.Columns.Add("colEmployeeID", "EmployeeID")
            UsersAccountData.Columns("colEmployeeID").Visible = False
        End If
    End Sub

    Private Sub LoadDataBasedOnMode()
        If currentViewMode = "Staff" Then
            LoadStaffData()
        Else
            LoadEmployeeData()
        End If
        UpdateToggleButtonStyles()
    End Sub

    Private Sub LoadEmployeeData()
        Try
            ' Configure Columns for Employee Mode
            If UsersAccountData.Columns.Contains("colEdit") Then UsersAccountData.Columns("colEdit").Visible = False
            If UsersAccountData.Columns.Contains("colDelete") Then UsersAccountData.Columns("colDelete").Visible = False
            If UsersAccountData.Columns.Contains("colCreateAccount") Then UsersAccountData.Columns("colCreateAccount").Visible = True

            openConn()
            Dim query As String = "
                SELECT 
                    EmployeeID as id,
                    CONCAT(FirstName, ' ', LastName) as name,
                    Position as position,
                    HireDate as DateCreated,
                    Email as username,
                    EmployeeID as employee_id
                FROM employee
                WHERE EmploymentStatus = 'Active' 
                AND Position = 'Employee'
                AND EmployeeID NOT IN (SELECT IFNULL(employee_id,0) FROM user_accounts)
                ORDER BY FirstName"

            Dim cmd As New MySqlCommand(query, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            allStaffData = New DataTable()
            adapter.Fill(allStaffData)

            totalRecords = allStaffData.Rows.Count
            totalPages = If(totalRecords > 0, Math.Ceiling(totalRecords / pageSize), 1)
            lblStaffs.Text = totalRecords.ToString()

            ApplySearchFilter()

        Catch ex As Exception
            MessageBox.Show("Error loading employee data: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub LoadStaffData()
        Try
            ' Configure Columns for Staff Mode
            If UsersAccountData.Columns.Contains("colEdit") Then UsersAccountData.Columns("colEdit").Visible = True
            If UsersAccountData.Columns.Contains("colDelete") Then UsersAccountData.Columns("colDelete").Visible = True
            If UsersAccountData.Columns.Contains("colCreateAccount") Then UsersAccountData.Columns("colCreateAccount").Visible = False

            openConn()
            ' FIXED QUERY: Load staff with status from Employee table
            Dim query As String = "
                SELECT 
                    ua.id,
                    ua.name,
                    ua.username,
                    ua.password,
                    ua.position,
                    ua.status,
                    ua.created_at as DateCreated,
                    ua.employee_id
                FROM user_accounts ua
                LEFT JOIN employee e ON ua.employee_id = e.EmployeeID
                WHERE ua.position != 'Administrator' AND ua.position != 'Admin' AND ua.type != 0
                ORDER BY ua.created_at DESC
                LIMIT 1000"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.CommandTimeout = 30

            Dim adapter As New MySqlDataAdapter(cmd)
            allStaffData = New DataTable()
            adapter.Fill(allStaffData)

            totalRecords = allStaffData.Rows.Count
            totalPages = If(totalRecords > 0, Math.Ceiling(totalRecords / pageSize), 1)

            ' Update staff count
            lblStaffs.Text = allStaffData.Rows.Count.ToString()

            ' Apply search and load first page
            ApplySearchFilter()

        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message & vbCrLf & vbCrLf &
                          "Make sure the 'Position' column contains 'Staff' entries.",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error loading staff data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub ApplySearchFilter()
        If allStaffData Is Nothing Then Return

        Dim filteredData As DataTable

        If String.IsNullOrWhiteSpace(searchText) Then
            filteredData = allStaffData
        Else
            filteredData = allStaffData.Clone()
            For Each row As DataRow In allStaffData.Rows
                Dim name As String = If(row("name") IsNot DBNull.Value, row("name").ToString().ToLower(), "")
                Dim username As String = If(row("username") IsNot DBNull.Value, row("username").ToString().ToLower(), "")
                Dim position As String = If(row("position") IsNot DBNull.Value, row("position").ToString().ToLower(), "")

                If name.Contains(searchText.ToLower()) OrElse
                   username.Contains(searchText.ToLower()) OrElse
                   position.Contains(searchText.ToLower()) Then
                    filteredData.ImportRow(row)
                End If
            Next
        End If

        totalRecords = filteredData.Rows.Count
        totalPages = If(totalRecords > 0, Math.Ceiling(totalRecords / pageSize), 1)

        ' Load first page of filtered data
        LoadPage(1, filteredData)
    End Sub

    Private Sub LoadPage(pageNumber As Integer, Optional dataSource As DataTable = Nothing)
        If dataSource Is Nothing Then dataSource = allStaffData
        If dataSource Is Nothing OrElse dataSource.Rows.Count = 0 Then
            UsersAccountData.Rows.Clear()
            UpdatePaginationControls()
            lblStaffs.Text = "0"
            Return
        End If

        ' Validate page number
        If pageNumber < 1 Then pageNumber = 1
        If pageNumber > totalPages Then pageNumber = totalPages

        currentPage = pageNumber
        Dim startIndex As Integer = (currentPage - 1) * pageSize
        Dim endIndex As Integer = Math.Min(startIndex + pageSize, dataSource.Rows.Count)

        ' Suspend layout for smoother loading
        UsersAccountData.SuspendLayout()
        UsersAccountData.Rows.Clear()

        Try
            ' Use bulk operation for better performance
            For i As Integer = startIndex To endIndex - 1
                Dim row As DataRow = dataSource.Rows(i)

                ' Get name
                Dim fullName As String = If(row("name") IsNot DBNull.Value, row("name").ToString().Trim(), "N/A")
                Dim username As String = If(row("username") IsNot DBNull.Value, row("username").ToString().Trim(), "")
                Dim employeeId As Integer = If(row("employee_id") IsNot DBNull.Value, Convert.ToInt32(row("employee_id")), 0)

                ' Get position/role
                Dim position As String = If(row("position") IsNot DBNull.Value, row("position").ToString().Trim(), "Staff")

                ' Get status
                Dim status As String = "Active"
                If dataSource.Columns.Contains("status") AndAlso row("status") IsNot DBNull.Value Then
                    status = row("status").ToString()
                End If

                ' Format hire date
                Dim hireDate As String = "N/A"
                If row("DateCreated") IsNot DBNull.Value Then
                    Try
                        hireDate = Convert.ToDateTime(row("DateCreated")).ToString("MMMM dd, yyyy")
                    Catch
                        hireDate = row("DateCreated").ToString()
                    End Try
                End If

                ' Add row to DataGridView
                Dim rowIndex As Integer = UsersAccountData.Rows.Add()
                Dim newRow As DataGridViewRow = UsersAccountData.Rows(rowIndex)

                ' Set cell values (matching Designer column names)
                If UsersAccountData.Columns.Contains("txtName") Then
                    newRow.Cells("txtName").Value = fullName
                End If

                If UsersAccountData.Columns.Contains("colRole") Then
                    newRow.Cells("colRole").Value = position
                End If

                If UsersAccountData.Columns.Contains("colStatus") Then
                    newRow.Cells("colStatus").Value = status
                End If

                If UsersAccountData.Columns.Contains("colJoinDate") Then
                    newRow.Cells("colJoinDate").Value = hireDate
                End If

                If UsersAccountData.Columns.Contains("colUsername") Then
                    newRow.Cells("colUsername").Value = username
                End If

                If UsersAccountData.Columns.Contains("colPassword") Then
                    Dim rawPass As String = If(row.Table.Columns.Contains("password") AndAlso row("password") IsNot DBNull.Value, row("password").ToString(), "")
                    If Not String.IsNullOrEmpty(rawPass) Then
                        Try
                            newRow.Cells("colPassword").Value = Decrypt(rawPass)
                        Catch
                            newRow.Cells("colPassword").Value = "******"
                        End Try
                    Else
                        newRow.Cells("colPassword").Value = ""
                    End If
                End If

                If UsersAccountData.Columns.Contains("colEmployeeID") Then
                    newRow.Cells("colEmployeeID").Value = employeeId
                End If

                ' Store ID for delete/edit operations
                newRow.Tag = If(row("id") IsNot DBNull.Value, Convert.ToInt32(row("id")), 0)
            Next

        Catch ex As Exception
            MessageBox.Show("Error displaying data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UsersAccountData.ResumeLayout()
            UpdatePaginationControls()
        End Try
    End Sub

    Private Sub UpdatePaginationControls()
        ' Update page info label
        lblPageInfo.Text = $"Page {currentPage} of {totalPages}"

        ' Enable/disable buttons based on current page
        btnFirstPage.Enabled = (currentPage > 1)
        btnPreviousPage.Enabled = (currentPage > 1)
        btnNextPage.Enabled = (currentPage < totalPages)
        btnLastPage.Enabled = (currentPage < totalPages)

        ' Visual feedback for disabled buttons
        btnFirstPage.BackColor = If(btnFirstPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnPreviousPage.BackColor = If(btnPreviousPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnNextPage.BackColor = If(btnNextPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnLastPage.BackColor = If(btnLastPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
    End Sub

    Private Sub UsersAccountData_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles UsersAccountData.CellClick
        If e.RowIndex < 0 Then Return
        If e.ColumnIndex < 0 Then Return

        Dim selectedRow As DataGridViewRow = UsersAccountData.Rows(e.RowIndex)

        ' Get the full name from the combined Name column
        Dim fullName As String = "Unknown"
        If UsersAccountData.Columns.Contains("txtName") AndAlso selectedRow.Cells("txtName").Value IsNot Nothing Then
            fullName = selectedRow.Cells("txtName").Value.ToString().Trim()
        End If

        Dim userID As Integer = If(selectedRow.Tag IsNot Nothing, CInt(selectedRow.Tag), 0)

        If userID = 0 Then
            MessageBox.Show("Invalid user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' DELETE BUTTON
        If UsersAccountData.Columns.Contains("colDelete") AndAlso e.ColumnIndex = UsersAccountData.Columns("colDelete").Index Then
            Dim result As DialogResult = MessageBox.Show(
                $"Are you sure you want to delete {fullName}?{vbNewLine}{vbNewLine}This action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If result = DialogResult.Yes Then
                DeleteStaffMember(userID, fullName)
            End If
        End If

        ' EDIT BUTTON
        If UsersAccountData.Columns.Contains("colEdit") AndAlso e.ColumnIndex = UsersAccountData.Columns("colEdit").Index Then
            Dim empId As Integer = 0
            Dim uName As String = ""

            If UsersAccountData.Columns.Contains("colEmployeeID") AndAlso selectedRow.Cells("colEmployeeID").Value IsNot Nothing Then
                empId = CInt(selectedRow.Cells("colEmployeeID").Value)
            End If

            If UsersAccountData.Columns.Contains("colUsername") AndAlso selectedRow.Cells("colUsername").Value IsNot Nothing Then
                uName = selectedRow.Cells("colUsername").Value.ToString()
            End If

            Dim frm As New FormEdit()
            ' Pass data for editing
            ' Use LoadUserData but set UserID property directly or via overload
            frm.UserID = userID
            frm.LoadUserData(empId, uName, selectedRow.Cells("colRole").Value.ToString())

            If frm.ShowDialog() = DialogResult.OK Then
                ' MDA FIX: Clear selection to release accessibility references
                UsersAccountData.CurrentCell = Nothing
                ' Defer reload to next message loop
                Me.BeginInvoke(Sub() LoadDataBasedOnMode())
            End If
        End If

        ' CREATE ACCOUNT BUTTON
        If UsersAccountData.Columns.Contains("colCreateAccount") AndAlso e.ColumnIndex = UsersAccountData.Columns("colCreateAccount").Index Then
            ' Get data from the Employee row
            Dim empId As Integer = CInt(selectedRow.Cells("colEmployeeID").Value)
            Dim empName As String = selectedRow.Cells("txtName").Value.ToString()
            Dim empRole As String = selectedRow.Cells("colRole").Value.ToString()

            Dim frm As New CreateAccount()
            ' Pre-fill for linking
            frm.LoadEmployeeData(empId, empName, empRole)

            If frm.ShowDialog() = DialogResult.OK Then
                ' MDA FIX
                UsersAccountData.CurrentCell = Nothing
                Me.BeginInvoke(Sub() LoadDataBasedOnMode())
            End If
        End If
    End Sub

    Private Sub DeleteStaffMember(userID As Integer, username As String)
        Try
            openConn()
            Dim query As String = "DELETE FROM user_accounts WHERE id = @id"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@id", userID)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show($"{username} has been deleted successfully.",
                              "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDataBasedOnMode() ' Reload data
            Else
                MessageBox.Show("No records were deleted. Staff member may not exist.",
                              "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As MySqlException
            MessageBox.Show($"Database error while deleting staff member: {ex.Message}",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"Error deleting staff member: {ex.Message}",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Pagination button handlers
    Private Sub btnFirstPage_Click(sender As Object, e As EventArgs) Handles btnFirstPage.Click
        If currentPage > 1 Then
            ApplySearchFilter() ' Reload from page 1
        End If
    End Sub

    Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
        If currentPage > 1 Then
            Dim filteredData As DataTable = GetFilteredData()
            LoadPage(currentPage - 1, filteredData)
        End If
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If currentPage < totalPages Then
            Dim filteredData As DataTable = GetFilteredData()
            LoadPage(currentPage + 1, filteredData)
        End If
    End Sub

    Private Sub btnLastPage_Click(sender As Object, e As EventArgs) Handles btnLastPage.Click
        If currentPage < totalPages Then
            Dim filteredData As DataTable = GetFilteredData()
            LoadPage(totalPages, filteredData)
        End If
    End Sub

    Private Function GetFilteredData() As DataTable
        If allStaffData Is Nothing Then Return Nothing

        If String.IsNullOrWhiteSpace(searchText) Then
            Return allStaffData
        Else
            Dim filteredData As DataTable = allStaffData.Clone()
            For Each row As DataRow In allStaffData.Rows
                Dim name As String = If(row("name") IsNot DBNull.Value, row("name").ToString().ToLower(), "")
                Dim position As String = If(row("position") IsNot DBNull.Value, row("position").ToString().ToLower(), "")

                If name.Contains(searchText.ToLower()) OrElse
                   position.Contains(searchText.ToLower()) Then
                    filteredData.ImportRow(row)
                End If
            Next
            Return filteredData
        End If
    End Function

    ' Search functionality
    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitializing Then Return

        Dim currentSearch As String = TextBoxSearch.Text.Trim()
        If currentSearch = "Search staff..." Then currentSearch = ""

        ' Only reload if search term actually changed
        If currentSearch = _lastSearchText Then Return
        _lastSearchText = currentSearch

        searchText = currentSearch
        ApplySearchFilter()
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search staff..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42) ' Dark slate color
        End If
        txtSearch.BorderColor = Color.FromArgb(99, 102, 241) ' Purple/Indigo border
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        if String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search staff..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184) ' Slate-400
        End If
        txtSearch.BorderColor = Color.FromArgb(226, 232, 240) ' Default slate-200
    End Sub

    ' =======================================================
    ' INITIALIZE SEARCH BOX
    ' =======================================================
    Private Sub InitializeSearchBox()
        TextBoxSearch.Text = "Search staff..."
        TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
    End Sub
      ' UI Helper Methods
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

    Private Sub RoundPaginationButtons()
        RoundButton(btnFirstPage)
        RoundButton(btnPreviousPage)
        RoundButton(btnNextPage)
        RoundButton(btnLastPage)
    End Sub

    ' Public methods for external refresh
    Public Sub RefreshData()
        LoadStaffData()
    End Sub

    Private Sub SetupToggleButtons()
        btnShowStaff = New Button()
        btnShowStaff.Text = "Staff Accounts"
        btnShowStaff.Size = New Size(140, 35)
        btnShowStaff.FlatStyle = FlatStyle.Flat
        btnShowStaff.Cursor = Cursors.Hand

        btnShowEmployee = New Button()
        btnShowEmployee.Text = "Employees List"
        btnShowEmployee.Size = New Size(140, 35)
        btnShowEmployee.FlatStyle = FlatStyle.Flat
        btnShowEmployee.Cursor = Cursors.Hand

        btnUpdateStatus = New Button()
        btnUpdateStatus.Text = "Update Status"
        btnUpdateStatus.Size = New Size(130, 35)
        btnUpdateStatus.BackColor = Color.FromArgb(59, 130, 246)
        btnUpdateStatus.ForeColor = Color.White
        btnUpdateStatus.FlatStyle = FlatStyle.Flat
        btnUpdateStatus.Cursor = Cursors.Hand
        btnUpdateStatus.Visible = False ' Only visible in Staff mode

        RoundButton(btnShowStaff)
        RoundButton(btnShowEmployee)
        RoundButton(btnUpdateStatus)

        Me.Controls.Add(btnShowStaff)
        Me.Controls.Add(btnShowEmployee)
        Me.Controls.Add(btnUpdateStatus)
        btnShowStaff.BringToFront()
        btnShowEmployee.BringToFront()
        btnUpdateStatus.BringToFront()
    End Sub

    Private Sub UpdateToggleButtonStyles()
        Dim activeColor As Color = Color.FromArgb(59, 130, 246) ' Blue
        Dim inactiveColor As Color = Color.White
        Dim activeText As Color = Color.White
        Dim inactiveText As Color = Color.Black

        If currentViewMode = "Staff" Then
            btnShowStaff.BackColor = activeColor
            btnShowStaff.ForeColor = activeText
            btnShowEmployee.BackColor = inactiveColor
            btnShowEmployee.ForeColor = inactiveText
            If btnUpdateStatus IsNot Nothing Then btnUpdateStatus.Visible = True
        Else
            btnShowStaff.BackColor = inactiveColor
            btnShowStaff.ForeColor = inactiveText
            btnShowEmployee.BackColor = activeColor
            btnShowEmployee.ForeColor = activeText
            If btnUpdateStatus IsNot Nothing Then btnUpdateStatus.Visible = False
        End If
    End Sub

    Private Sub btnShowStaff_Click(sender As Object, e As EventArgs) Handles btnShowStaff.Click
        currentViewMode = "Staff"
        LoadDataBasedOnMode()
    End Sub

    Private Sub btnShowEmployee_Click(sender As Object, e As EventArgs) Handles btnShowEmployee.Click
        currentViewMode = "Employee"
        LoadDataBasedOnMode()
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        ' 1. Select Employee First
        Dim selectEmp As New FormSelectEmployee()
        If selectEmp.ShowDialog() = DialogResult.OK Then
            ' 2. Open FormEdit with pre-filled data
            Dim frm As New FormEdit()
            ' Pass 0 as ID (new user), but provide EmployeeID and details
            frm.LoadUserData(selectEmp.SelectedEmployeeID, selectEmp.SelectedEmployeeName, selectEmp.SelectedEmployeeRole)

            If frm.ShowDialog() = DialogResult.OK Then
                LoadStaffData() ' Reload grid
            End If
        End If
    End Sub

    Public Sub LoadUsers()
        LoadDataBasedOnMode()
    End Sub

    Private Sub btnUpdateStatus_Click(sender As Object, e As EventArgs) Handles btnUpdateStatus.Click
        If UsersAccountData.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to update status.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow As DataGridViewRow = UsersAccountData.SelectedRows(0)
        Dim userId As Integer = If(selectedRow.Tag IsNot Nothing, CInt(selectedRow.Tag), 0)
        Dim empId As Integer = If(selectedRow.Cells("colEmployeeID").Value IsNot Nothing, CInt(selectedRow.Cells("colEmployeeID").Value), 0)
        Dim name As String = If(selectedRow.Cells("txtName").Value IsNot Nothing, selectedRow.Cells("txtName").Value.ToString(), "Unknown")
        Dim currentStatus As String = If(selectedRow.Cells("colStatus").Value IsNot Nothing, selectedRow.Cells("colStatus").Value.ToString(), "Active")

        Dim frm As New FormUpdateStatus()
        frm.LoadData(userId, empId, name, currentStatus)

        If frm.ShowDialog() = DialogResult.OK Then
            LoadStaffData()
        End If
    End Sub

End Class

' Extension module for DataGridView double buffering
Module DataGridViewExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.GetType()
        Dim pi As Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
        If pi IsNot Nothing Then
            pi.SetValue(dgv, setting, Nothing)
        End If
    End Sub
End Module