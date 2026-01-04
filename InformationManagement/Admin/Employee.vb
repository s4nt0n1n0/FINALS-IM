Imports MySqlConnector
Imports System.Data

Public Class Employee
    Private _isLoading As Boolean = False
    Private _lastSearchText As String = ""
    Private isInitializing As Boolean = True
    
    ' Pagination state
    Private currentPage As Integer = 1
    Private recordsPerPage As Integer = 20
    Private totalRecords As Integer = 0
    Private totalPages As Integer = 0
    Private _currentCondition As String = ""

    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Disable Update Status button initially
        btnUpdateStatus.Enabled = False
        RoundPaginationButtons()
        
        LoadEmployees()
        InitializeSearchBox()
        isInitializing = False
    End Sub

    ' =======================================================
    ' INITIALIZE SEARCH BOX
    ' =======================================================
    Private Sub InitializeSearchBox()
        TextBoxSearch.Text = "Search employees..."
        TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
    End Sub
      '====================================
    ' MAIN LOADER
    '====================================
    Public Async Sub LoadEmployees(Optional condition As String = "", Optional searchText As String = "", Optional resetPage As Boolean = False)
        If _isLoading Then Return
        _isLoading = True
        SetLoadingState(True)

        If resetPage Then currentPage = 1
        If condition <> "" OrElse (condition = "" AndAlso Not resetPage AndAlso _currentCondition <> "") Then
             If condition <> "" Then _currentCondition = condition
        Else
             _currentCondition = ""
        End If

        Try
            ' Get search text from UI if not provided
            If String.IsNullOrEmpty(searchText) AndAlso TextBoxSearch IsNot Nothing Then
                searchText = TextBoxSearch.Text.Trim()
            End If

            If searchText = "Search employees..." Then searchText = ""
              ' Get total count
            totalRecords = Await Task.Run(Function() FetchTotalEmployeeCount(searchText, _currentCondition))
            totalPages = Math.Ceiling(totalRecords / recordsPerPage)
            If totalPages = 0 Then totalPages = 1
            If currentPage > totalPages Then currentPage = totalPages

            Dim offset As Integer = (currentPage - 1) * recordsPerPage
            
            Dim query As String =
                "SELECT EmployeeID, FirstName, LastName, Gender, DateOfBirth, ContactNumber, Email, Address, HireDate, Position, MaritalStatus, EmploymentStatus, EmploymentType, EmergencyContact, WorkShift, Salary FROM employee"

            Dim finalCondition As String = ""
            If _currentCondition <> "" Then
                finalCondition = _currentCondition
            End If

            If searchText <> "" Then
                Dim searchPart As String = "(FirstName LIKE @search OR LastName LIKE @search OR Position LIKE @search)"
                If finalCondition <> "" Then
                    finalCondition &= " AND " & searchPart
                Else
                    finalCondition = searchPart
                End If
            End If

            If finalCondition <> "" Then
                query &= " WHERE " & finalCondition
            End If

            query &= " ORDER BY FirstName, LastName LIMIT @limit OFFSET @offset"

            Await Task.Run(Sub() LoadToDGV(query, DataGridView1, searchText, offset, recordsPerPage))

            UpdatePaginationControls()

        Catch ex As Exception
            If Not Me.IsDisposed Then
                MessageBox.Show("Error loading employees: " & ex.Message)
            End If
        Finally
            If Not Me.IsDisposed Then
                SetLoadingState(False)
                _isLoading = False
            End If
        End Try
    End Sub

    Private Function FetchTotalEmployeeCount(searchText As String, condition As String) As Integer
        Dim query As String = "SELECT COUNT(*) FROM employee"
        Dim finalCondition As String = condition
        
        If searchText <> "" Then
            Dim searchPart As String = "(FirstName LIKE @search OR LastName LIKE @search OR Position LIKE @search)"
            If finalCondition <> "" Then
                finalCondition &= " AND " & searchPart
            Else
                finalCondition = searchPart
            End If
        End If

        If finalCondition <> "" Then
            query &= " WHERE " & finalCondition
        End If

        Try
            openConn()
            Using cmd As New MySqlCommand(query, conn)
                If searchText <> "" Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        Finally
            closeConn()
        End Try
    End Function

    '====================================
    ' UNIVERSAL LOADER FOR DATAGRIDVIEW
    '====================================
    Private Sub LoadToDGV(query As String, dgv As DataGridView, searchText As String, offset As Integer, limit As Integer)
        Try
            openConn()

            Using cmd As New MySqlCommand(query, conn)
                If searchText <> "" Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If
                cmd.Parameters.AddWithValue("@limit", limit)
                cmd.Parameters.AddWithValue("@offset", offset)
                
                Using adapter As New MySqlDataAdapter(cmd)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    
                    Me.Invoke(Sub()
                                  dgv.DataSource = table
                                  ' ✅ HIDE EMPLOYEE ID COLUMN
                                  If dgv.Columns.Contains("EmployeeID") Then
                                      dgv.Columns("EmployeeID").Visible = False
                                  End If

                                  ' Column Header Styling
                                  dgv.EnableHeadersVisualStyles = False
                                  dgv.RowHeadersVisible = False
                                  dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                                  dgv.ColumnHeadersHeight = 45
                                  dgv.BorderStyle = BorderStyle.None

                                  dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 38, 50)
                                  dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
                                  dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                                  dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(26, 38, 50)
                                  dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White
                                  dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                  
                                  ' Default Cell Style
                                  dgv.DefaultCellStyle.BackColor = SystemColors.Window
                                  dgv.DefaultCellStyle.Font = New Font("Segoe UI", 8.25F)
                                  dgv.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64)
                                  dgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
                                  dgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
                                  dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                              End Sub)
                End Using
            End Using

        Catch ex As Exception
            If Not Me.IsDisposed Then
                Me.Invoke(Sub() MessageBox.Show("Error loading table: " & ex.Message))
            End If
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            Me.UseWaitCursor = isLoading
            DataGridView1.Enabled = Not isLoading
            btnRefresh.Enabled = Not isLoading
            AddEmployee.Enabled = Not isLoading
            EditEmployee.Enabled = Not isLoading
            btnDelete.Enabled = Not isLoading
            
            ' Pagination buttons are handled in UpdatePaginationControls
        Catch
        End Try
    End Sub

    '====================================
    ' PAGINATION LOGIC
    '====================================
    Private Sub UpdatePaginationControls()
        lblPageInfo.Text = $"Page {currentPage} of {totalPages}"
        lblTotalEmployees.Text = $"Total Employees: {totalRecords:N0}"

        ' Enable/Disable navigation buttons
        btnFirstPage.Enabled = (currentPage > 1)
        btnPrevPage.Enabled = (currentPage > 1)
        btnNextPage.Enabled = (currentPage < totalPages)
        btnLastPage.Enabled = (currentPage < totalPages)

        ' Visual feedback
        Dim enabledColor As Color = Color.FromArgb(240, 244, 250)
        Dim disabledColor As Color = Color.FromArgb(230, 230, 230)

        btnFirstPage.BackColor = If(btnFirstPage.Enabled, enabledColor, disabledColor)
        btnPrevPage.BackColor = If(btnPrevPage.Enabled, enabledColor, disabledColor)
        btnNextPage.BackColor = If(btnNextPage.Enabled, enabledColor, disabledColor)
        btnLastPage.BackColor = If(btnLastPage.Enabled, enabledColor, disabledColor)

        CenterPaginationControls()
    End Sub

    Private Sub RoundButton(btn As Button)
        Dim radius As Integer = 8
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
        RoundButton(btnPrevPage)
        RoundButton(btnNextPage)
        RoundButton(btnLastPage)
    End Sub

    Private Sub CenterPaginationControls()
        Try
            If Panel4 Is Nothing Then Return

            Dim panelWidth As Integer = Panel4.Width
            Dim totalButtonWidth As Integer = btnFirstPage.Width + btnPrevPage.Width +
                                              btnNextPage.Width + btnLastPage.Width
            Dim spacing As Integer = 10
            Dim labelWidth As Integer = lblPageInfo.Width

            Dim centerGroupWidth As Integer = totalButtonWidth + (spacing * 4) + labelWidth
            Dim startX As Integer = (panelWidth - centerGroupWidth) \ 2

            ' 1. Position Total Label LEFT
            lblTotalEmployees.Location = New Point(10, 16)
            lblTotalEmployees.Top = (Panel4.Height - lblTotalEmployees.Height) \ 2

            ' 2. Position Center Group
            btnFirstPage.Location = New Point(startX, 10)
            btnPrevPage.Location = New Point(btnFirstPage.Right + spacing, 10)
            
            lblPageInfo.AutoSize = True
            lblPageInfo.Location = New Point(btnPrevPage.Right + spacing, 16)
            lblPageInfo.Top = (Panel4.Height - lblPageInfo.Height) \ 2
            
            btnNextPage.Location = New Point(lblPageInfo.Right + spacing, 10)
            btnLastPage.Location = New Point(btnNextPage.Right + spacing, 10)

        Catch ex As Exception
            ' Squelch sizing errors
        End Try
    End Sub

    Private Sub Employee_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        CenterPaginationControls()
    End Sub

    Private Sub btnFirstPage_Click(sender As Object, e As EventArgs) Handles btnFirstPage.Click
        currentPage = 1
        LoadEmployees()
    End Sub

    Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        If currentPage > 1 Then
            currentPage -= 1
            LoadEmployees()
        End If
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If currentPage < totalPages Then
            currentPage += 1
            LoadEmployees()
        End If
    End Sub

    Private Sub btnLastPage_Click(sender As Object, e As EventArgs) Handles btnLastPage.Click
        currentPage = totalPages
        LoadEmployees()
    End Sub

    ' =======================================================
    ' SEARCH FUNCTIONALITY
    ' =======================================================
    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitializing Then Return

        Dim currentSearch = TextBoxSearch.Text.Trim()
        If currentSearch = "Search employees..." Then currentSearch = ""

        ' Only refresh if the actual search criteria changed
        If currentSearch = _lastSearchText Then Return

        _lastSearchText = currentSearch
        LoadEmployees(resetPage:=True, searchText:=currentSearch)
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search employees..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42)
            txtSearch.BorderColor = Color.FromArgb(99, 102, 241)
        End If
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search employees..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
            txtSearch.BorderColor = Color.FromArgb(226, 232, 240)
        End If
    End Sub
      '====================================
    ' ADD EMPLOYEE
    '====================================
    Private Sub AddEmployee_Click(sender As Object, e As EventArgs) Handles AddEmployee.Click
        Dim frm As New AddEmployee()

        frm.StartPosition = FormStartPosition.CenterScreen
        frm.Show()
        frm.BringToFront()
    End Sub

    '====================================
    ' EDIT EMPLOYEE
    '====================================
    Private Sub EditEmployee_Click(sender As Object, e As EventArgs) Handles EditEmployee.Click

        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Select an employee to edit.")
            Exit Sub
        End If

        Dim empID As Integer = DataGridView1.SelectedRows(0).Cells("EmployeeID").Value

        Dim frm As New EditEmployee()
        frm.EmployeeIDValue = empID     ' pass ID to edit form
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.Show()
        frm.BringToFront()

    End Sub

    '====================================
    ' FILTER BUTTONS
    '====================================
    Private Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        LoadEmployees(condition:="", resetPage:=True)
        lblFilter.Text = "Showing: All Employees"
    End Sub

    Private Sub btnViewActive_Click(sender As Object, e As EventArgs) Handles btnViewActive.Click
        LoadEmployees(condition:="EmploymentStatus = 'Active'", resetPage:=True)
        lblFilter.Text = "Showing: Active Employees"
    End Sub

    Private Sub btnViewInactive_Click(sender As Object, e As EventArgs) Handles btnViewInactive.Click
        LoadEmployees(condition:="EmploymentStatus = 'Resigned'", resetPage:=True)
        lblFilter.Text = "Showing: Resigned Employees"
    End Sub

    Private Sub btnViewOnLeave_Click(sender As Object, e As EventArgs) Handles btnViewOnLeave.Click
        LoadEmployees(condition:="EmploymentStatus = 'On Leave'", resetPage:=True)
        lblFilter.Text = "Showing: Employees On Leave"
    End Sub

    '====================================
    ' REFRESH LIST
    '====================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        InitializeSearchBox()
        _lastSearchText = ""
        LoadEmployees(resetPage:=True)
        lblFilter.Text = "Showing: All Employees"
    End Sub
      '====================================
    ' DELETE EMPLOYEE
    '====================================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Select an employee to delete.")
            Exit Sub
        End If

        Dim empID As Integer = DataGridView1.SelectedRows(0).Cells("EmployeeID").Value

        If MessageBox.Show("Delete Employee #" & empID & "?",
                           "Confirm Deletion",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

        Try
            openConn()

            Dim cmd As New MySqlCommand("DELETE FROM employee WHERE EmployeeID=@id", conn)
            cmd.Parameters.AddWithValue("@id", empID)
            cmd.ExecuteNonQuery()

            closeConn()

            MessageBox.Show("Employee deleted successfully.")

            ' Log Activity
            ActivityLogger.LogUserActivity(
                action:="Delete",
                actionCategory:="User Management",
                description:=$"Deleted Employee ID: {empID}",
                sourceSystem:="Admin Panel",
                referenceID:=empID.ToString(),
                referenceTable:="employee",
                oldValue:="Active",
                newValue:="Deleted"
            )
            LoadEmployees(resetPage:=True)

        Catch ex As Exception
            MessageBox.Show("Error deleting employee: " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        ' Enable/disable Update Status button based on selection
        If DataGridView1.SelectedRows.Count > 0 Then
            btnUpdateStatus.Enabled = True
        Else
            btnUpdateStatus.Enabled = False
        End If
    End Sub

    Private Sub btnUpdateStatus_Click(sender As Object, e As EventArgs) Handles btnUpdateStatus.Click
        ' Validate selection
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select an employee to update status.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim employeeId As Integer = DataGridView1.SelectedRows(0).Cells("EmployeeID").Value
        Dim employeeName As String = DataGridView1.SelectedRows(0).Cells("FirstName").Value.ToString() & " " & DataGridView1.SelectedRows(0).Cells("LastName").Value.ToString()
        Dim currentStatus As String = DataGridView1.SelectedRows(0).Cells("EmploymentStatus").Value?.ToString()

        ' Show status selection dialog
        Dim statusForm As New Form()
        statusForm.Text = "Update Employment Status"
        statusForm.Size = New Size(400, 250)
        statusForm.StartPosition = FormStartPosition.CenterParent
        statusForm.FormBorderStyle = FormBorderStyle.FixedDialog
        statusForm.MaximizeBox = False
        statusForm.MinimizeBox = False

        Dim lblInfo As New Label()
        lblInfo.Text = $"Employee: {employeeName}{Environment.NewLine}Current Status: {currentStatus}{Environment.NewLine}{Environment.NewLine}Select new status:"
        lblInfo.Location = New Point(20, 20)
        lblInfo.Size = New Size(350, 60)
        lblInfo.Font = New Font("Segoe UI", 10)

        Dim cmbStatus As New ComboBox()
        cmbStatus.Items.AddRange(New Object() {"Active", "On Leave", "Resigned"})
        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList
        cmbStatus.Location = New Point(20, 90)
        cmbStatus.Size = New Size(340, 30)
        cmbStatus.Font = New Font("Segoe UI", 11)

        ' Set current status
        Dim index As Integer = cmbStatus.FindStringExact(currentStatus)
        If index >= 0 Then
            cmbStatus.SelectedIndex = index
        Else
            cmbStatus.SelectedIndex = 0
        End If

        Dim btnOK As New Button()
        btnOK.Text = "Update"
        btnOK.Location = New Point(180, 140)
        btnOK.Size = New Size(90, 35)
        btnOK.DialogResult = DialogResult.OK
        btnOK.BackColor = Color.FromArgb(245, 158, 11)
        btnOK.ForeColor = Color.White
        btnOK.FlatStyle = FlatStyle.Flat

        Dim btnCancel As New Button()
        btnCancel.Text = "Cancel"
        btnCancel.Location = New Point(280, 140)
        btnCancel.Size = New Size(90, 35)
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.BackColor = Color.Gray
        btnCancel.ForeColor = Color.White
        btnCancel.FlatStyle = FlatStyle.Flat

        statusForm.Controls.AddRange(New Control() {lblInfo, cmbStatus, btnOK, btnCancel})
        statusForm.AcceptButton = btnOK
        statusForm.CancelButton = btnCancel

        If statusForm.ShowDialog() = DialogResult.Cancel Then
            Return
        End If

        Dim newStatus As String = cmbStatus.SelectedItem?.ToString()
        If String.IsNullOrEmpty(newStatus) Then
            MessageBox.Show("Please select a status.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if status is the same
        If currentStatus = newStatus Then
            MessageBox.Show($"Employee is already {currentStatus}. No changes made.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' Update database
            openConn()
            Dim query As String = "UPDATE employee SET EmploymentStatus = @status WHERE EmployeeID = @id"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@status", newStatus)
                cmd.Parameters.AddWithValue("@id", employeeId)
                cmd.ExecuteNonQuery()
            End Using
            closeConn()

            MessageBox.Show($"Employment status updated successfully to '{newStatus}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Log Activity
            ActivityLogger.LogUserActivity(
                action:="Employment Status Updated",
                actionCategory:="User Management",
                description:=$"Changed employment status for {employeeName} (ID: {employeeId}) from '{currentStatus}' to '{newStatus}'",
                sourceSystem:="Admin Panel",
                referenceID:=employeeId.ToString(),
                referenceTable:="employee",
                oldValue:=currentStatus,
                newValue:=newStatus
            )

            ' Refresh the grid
            LoadEmployees(resetPage:=False)

        Catch ex As Exception
            MessageBox.Show("Error updating employment status: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

End Class