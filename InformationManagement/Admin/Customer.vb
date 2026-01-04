Imports System.Drawing.Drawing2D
Imports MySqlConnector
Imports System.Threading.Tasks
Imports System.Data

Public Class Customer
    Private connectionString As String = modDB.strConnection
    Private _currentFilterStatus As String = ""
    
    ' Pagination Variables
    Private currentPage As Integer = 1
    Private recordsPerPage As Integer = 20
    Private totalRecords As Integer = 0
    Private totalPages As Integer = 0

    Private Async Sub Customer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup search bar focus effects
        txtSearch.Text = "Search customers..."
        txtSearch.ForeColor = Color.FromArgb(148, 163, 184)
        
        ' Disable Update Status button initially
        btnUpdateStatus.Enabled = False
        RoundPaginationButtons()
        
        Await RefreshCustomersAsync()
    End Sub

    Private Async Function RefreshCustomersAsync() As Task
        Try
            ' Load data in background
            Dim searchText As String = If(txtSearch.Text = "Search customers...", "", txtSearch.Text)
            
            ' 1. Get Count
            totalRecords = Await Task.Run(Function() GetCustomerCount(searchText, _currentFilterStatus))
            totalPages = Math.Ceiling(totalRecords / recordsPerPage)
            If totalPages < 1 Then totalPages = 1
            
            ' Validate Page
            If currentPage > totalPages Then currentPage = totalPages
            If currentPage < 1 Then currentPage = 1

            ' 2. Get Data
            Dim dt As DataTable = Await Task.Run(Function() LoadCustomerDataFromDB(searchText, _currentFilterStatus, currentPage, recordsPerPage))
            
            ' Update UI on UI thread
            Me.Invoke(Sub()
                DataGridView1.DataSource = dt
                FormatDataGridView()
                UpdateSummaryTiles(dt) ' Note: Summary tiles currently use the PAGINATED data for counts (e.g. Active/New). Ideally should separate summary query, but for now this is consistent with old 'dt'.
                UpdatePaginationControls()
            End Sub)
            
        Catch ex As Exception
            MessageBox.Show("Error refreshing customer data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Function GetCustomerCount(searchText As String, Optional statusFilter As String = "") As Integer
        Dim count As Integer = 0
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM customers WHERE CONCAT(FirstName, ' ', LastName, ' ', Email, ' ', ContactNumber) LIKE @search"
                
                If Not String.IsNullOrEmpty(statusFilter) Then
                    query &= " AND AccountStatus = @status"
                End If

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    If Not String.IsNullOrEmpty(statusFilter) Then
                        cmd.Parameters.AddWithValue("@status", statusFilter)
                    End If
                    count = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            ' Return 0 on error
        End Try
        Return count
    End Function

    Private Function LoadCustomerDataFromDB(searchText As String, Optional statusFilter As String = "", Optional page As Integer = 1, Optional pageSize As Integer = 20) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As New MySqlConnection(connectionString)
                Dim query As String = "
                    SELECT CustomerID, FirstName, LastName, Email, ContactNumber, CustomerType,
                           FeedbackCount, TotalOrdersCount, ReservationCount, LastTransactionDate,
                           LastLoginDate, CreatedDate, AccountStatus, SatisfactionRating
                    FROM customers
                    WHERE CONCAT(FirstName, ' ', LastName, ' ', Email, ' ', ContactNumber) LIKE @search"

                If Not String.IsNullOrEmpty(statusFilter) Then
                    query &= " AND AccountStatus = @status"
                End If

                query &= " ORDER BY CustomerID DESC LIMIT @limit OFFSET @offset"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    If Not String.IsNullOrEmpty(statusFilter) Then
                        cmd.Parameters.AddWithValue("@status", statusFilter)
                    End If
                    cmd.Parameters.AddWithValue("@limit", pageSize)
                    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize)

                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Private Sub UpdateSummaryTiles(dt As DataTable)
        Try
            ' Total Customers (Use the totalRecords calculated from the count query)
            Label4.Text = totalRecords.ToString("N0")

            ' Note: The following counts (Active/New) will now only reflect the CURRENT PAGE due to optimization.
            ' To get global counts with filters, we would need separate aggregate queries.
            ' For now, we display the stats based on the visible records or keep them as is.

            ' Active Customers (on current page)
            Dim activeCount As Integer = dt.Select("AccountStatus = 'Active'").Length
            Label6.Text = activeCount.ToString("N0")

            ' New Customers (Joined this month - on current page)
            Dim newCount As Integer = 0
            Dim firstOfMonth As New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("CreatedDate")) Then
                    Dim joinedDate As DateTime = Convert.ToDateTime(row("CreatedDate"))
                    If joinedDate >= firstOfMonth Then
                        newCount += 1
                    End If
                End If
            Next
            Label7.Text = newCount.ToString("N0")

        Catch ex As Exception
            ' Silent error for summaries
        End Try
    End Sub

    Private Sub FormatDataGridView()
        ' Column Header Styling
        With DataGridView1
            .EnableHeadersVisualStyles = False
            .RowHeadersVisible = False
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .ColumnHeadersHeight = 45
            .BorderStyle = BorderStyle.None

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

        If DataGridView1.Columns.Contains("CustomerID") Then
            DataGridView1.Columns("CustomerID").Visible = False
        End If

        ' Modern headers
        Dim headers As New Dictionary(Of String, String) From {
            {"FirstName", "First Name"},
            {"LastName", "Last Name"},
            {"ContactNumber", "Contact"},
            {"CustomerType", "Type"},
            {"TotalOrdersCount", "Orders"},
            {"ReservationCount", "Reservations"},
            {"LastTransactionDate", "Last Order"},
            {"LastLoginDate", "Last Login"},
            {"CreatedDate", "Joined"},
            {"AccountStatus", "Status"},
            {"SatisfactionRating", "Rating"}
        }

        For Each col As DataGridViewColumn In DataGridView1.Columns
            If headers.ContainsKey(col.Name) Then
                col.HeaderText = headers(col.Name)
            End If
            
            ' Specific styling
            If col.Name.Contains("Date") Then
                col.DefaultCellStyle.Format = "MMM dd, yyyy"
            End If
            
            If col.Name = "SatisfactionRating" Then
                col.DefaultCellStyle.Format = "0.0"
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
        Next

        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    ' Search Box Logic
    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        If txtSearch.Text = "Search customers..." Then
            txtSearch.Text = ""
            txtSearch.ForeColor = Color.FromArgb(15, 23, 42)
        End If
        SearchContainer.BorderColor = Color.FromArgb(99, 102, 241) ' Indigo focus
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            txtSearch.Text = "Search customers..."
            txtSearch.ForeColor = Color.FromArgb(148, 163, 184)
        End If
        SearchContainer.BorderColor = Color.FromArgb(226, 232, 240)
    End Sub

    Private Async Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ' Using a small delay or debouncing would be better, but for now direct async call
        Await RefreshCustomersAsync()
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a customer profile to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim customerId As Integer = DataGridView1.SelectedRows(0).Cells("CustomerID").Value
        Dim name As String = DataGridView1.SelectedRows(0).Cells("FirstName").Value.ToString()

        If MessageBox.Show($"Are you sure you want to delete the profile for {name}? This action might archive their data.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await Task.Run(Sub()
                    Using conn As New MySqlConnection(connectionString)
                        conn.Open()
                        Dim cmd As New MySqlCommand("CALL ArchiveCustomer(@id)", conn)
                        cmd.Parameters.AddWithValue("@id", customerId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Sub)

                MessageBox.Show("Customer profile has been archived and removed from active list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Log Activity
                ActivityLogger.LogUserActivity(
                    action:="Delete",
                    actionCategory:="User Management",
                    description:=$"Deleted/Archived Customer Profile: {name} (ID: {customerId})",
                    sourceSystem:="Admin Panel",
                    referenceID:=customerId.ToString(),
                    referenceTable:="customers",
                    oldValue:="Active",
                    newValue:="Archived"
                )

                Await RefreshCustomersAsync()

            Catch ex As Exception
                MessageBox.Show("Error deleting customer: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        ' Enable/disable Update Status button based on selection
        If DataGridView1.SelectedRows.Count > 0 Then
            btnUpdateStatus.Enabled = True
        Else
            btnUpdateStatus.Enabled = False
        End If
    End Sub

    Private Async Sub btnUpdateStatus_Click(sender As Object, e As EventArgs) Handles btnUpdateStatus.Click
        ' Validate selection
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a customer to update status.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim customerId As Integer = DataGridView1.SelectedRows(0).Cells("CustomerID").Value
        Dim customerName As String = DataGridView1.SelectedRows(0).Cells("FirstName").Value.ToString() & " " & DataGridView1.SelectedRows(0).Cells("LastName").Value.ToString()
        Dim currentStatus As String = DataGridView1.SelectedRows(0).Cells("AccountStatus").Value?.ToString()

        ' Show status selection dialog
        Dim statusForm As New Form()
        statusForm.Text = "Update Account Status"
        statusForm.Size = New Size(400, 250)
        statusForm.StartPosition = FormStartPosition.CenterParent
        statusForm.FormBorderStyle = FormBorderStyle.FixedDialog
        statusForm.MaximizeBox = False
        statusForm.MinimizeBox = False

        Dim lblInfo As New Label()
        lblInfo.Text = $"Customer: {customerName}{Environment.NewLine}Current Status: {currentStatus}{Environment.NewLine}{Environment.NewLine}Select new status:"
        lblInfo.Location = New Point(20, 20)
        lblInfo.Size = New Size(350, 60)
        lblInfo.Font = New Font("Segoe UI", 10)

        Dim cmbStatus As New ComboBox()
        cmbStatus.Items.AddRange(New Object() {"Active", "Suspended", "Inactive"})
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
            MessageBox.Show($"Customer is already {currentStatus}. No changes made.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' Update database
            Await Task.Run(Sub()
                Using conn As New MySqlConnection(connectionString)
                    conn.Open()
                    Dim query As String = "UPDATE customers SET AccountStatus = @status WHERE CustomerID = @id"
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@status", newStatus)
                        cmd.Parameters.AddWithValue("@id", customerId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            End Sub)

            MessageBox.Show($"Account status updated successfully to '{newStatus}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Log Activity
            ActivityLogger.LogUserActivity(
                action:="Account Status Updated",
                actionCategory:="User Management",
                description:=$"Changed account status for {customerName} (ID: {customerId}) from '{currentStatus}' to '{newStatus}'",
                sourceSystem:="Admin Panel",
                referenceID:=customerId.ToString(),
                referenceTable:="customers",
                oldValue:=currentStatus,
                newValue:=newStatus
            )

            ' Refresh the grid
            Await RefreshCustomersAsync()

        Catch ex As Exception
            MessageBox.Show("Error updating account status: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '====================================
    ' FILTER BUTTONS
    '====================================
    Private Async Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        _currentFilterStatus = ""
        lblFilter.Text = "Filter Status: All"
        Await RefreshCustomersAsync()
    End Sub

    Private Async Sub btnViewActive_Click(sender As Object, e As EventArgs) Handles btnViewActive.Click
        _currentFilterStatus = "Active"
        lblFilter.Text = "Filter Status: Active"
        Await RefreshCustomersAsync()
    End Sub

    Private Async Sub btnViewSuspended_Click(sender As Object, e As EventArgs) Handles btnViewSuspended.Click
        _currentFilterStatus = "Suspended"
        lblFilter.Text = "Filter Status: Suspended"
        Await RefreshCustomersAsync()
    End Sub

    Private Async Sub btnViewInactive_Click(sender As Object, e As EventArgs) Handles btnViewInactive.Click
        _currentFilterStatus = "Inactive"
        lblFilter.Text = "Filter Status: Inactive"
        Await RefreshCustomersAsync()
    End Sub

    '====================================
    ' PAGINATION LOGIC
    '====================================
    Private Sub UpdatePaginationControls()
        lblPageInfo.Text = $"Page {currentPage} of {totalPages}"
        lblTotalCustomers.Text = $"Total Customers: {totalRecords:N0}"

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
            lblTotalCustomers.Location = New Point(10, 16)
            lblTotalCustomers.Top = (Panel4.Height - lblTotalCustomers.Height) \ 2

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

    Private Sub Customer_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        CenterPaginationControls()
    End Sub

    Private Async Sub btnFirstPage_Click(sender As Object, e As EventArgs) Handles btnFirstPage.Click
        currentPage = 1
        Await RefreshCustomersAsync()
    End Sub

    Private Async Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        If currentPage > 1 Then
            currentPage -= 1
            Await RefreshCustomersAsync()
        End If
    End Sub

    Private Async Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If currentPage < totalPages Then
            currentPage += 1
            Await RefreshCustomersAsync()
        End If
    End Sub

    Private Async Sub btnLastPage_Click(sender As Object, e As EventArgs) Handles btnLastPage.Click
        currentPage = totalPages
        Await RefreshCustomersAsync()
    End Sub

End Class