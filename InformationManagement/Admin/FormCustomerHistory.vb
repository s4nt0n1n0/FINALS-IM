Imports MySqlConnector
Imports System.Text
Imports System.Data
Imports System.Threading.Tasks

Public Class FormCustomerHistory
    Private ReadOnly connectionString As String = modDB.strConnection
    Private _isLoading As Boolean = False
    Private _baseTitle As String = ""
    Private isInitializing As Boolean = True


    ' Pagination state
    Private _currentPage As Integer = 1
    Private ReadOnly _pageSize As Integer = 50
    Private _totalRecords As Integer = 0
    Private _totalPages As Integer = 0

    Private Sub FormCustomerHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _baseTitle = Label1.Text
        ConfigureGrid()
        _currentPage = 1
        'ConfigureDateFilter()
        BeginLoadCustomerHistory()
        isInitializing = False

    End Sub

    Private Async Sub BeginLoadCustomerHistory()
        If _isLoading Then Return
        _isLoading = True
        SetLoadingState(True)

        Try
            Dim searchText As String = txtSearch.Text.Trim()
            ' Get total count with search filter
            _totalRecords = Await Task.Run(Function() FetchTotalHistoryCount(searchText))
            _totalPages = Math.Ceiling(_totalRecords / _pageSize)
            If _totalPages = 0 Then _totalPages = 1
            If _currentPage > _totalPages Then _currentPage = _totalPages

            Dim offset As Integer = (_currentPage - 1) * _pageSize
            Dim table As DataTable = Await Task.Run(Function() FetchCustomerHistoryTable(searchText, offset, _pageSize))

            If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

            DataGridView1.DataSource = table
            UpdatePaginationUI()
        Catch ex As Exception
            If Not Me.IsDisposed Then
                MessageBox.Show("Error loading customer history:" & vbCrLf & ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
            End If
        Finally
            If Not Me.IsDisposed Then SetLoadingState(False)
            _isLoading = False
        End Try
    End Sub

    Private Function FetchTotalHistoryCount(searchText As String) As Integer
        ' Calculate count of ACTIVE customers matching search (AccountStatus = 'Active')
        Dim query As String = "SELECT COUNT(*) FROM customers c WHERE c.AccountStatus = 'Active' AND (CONCAT(c.FirstName, ' ', c.LastName) LIKE @search OR c.Email LIKE @search OR c.CustomerType LIKE @search)"

        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                Dim result = cmd.ExecuteScalar()
                Return If(IsDBNull(result), 0, Convert.ToInt32(result))
            End Using
        End Using
    End Function

    Private Function FetchCustomerHistoryTable(searchText As String, offset As Integer, limit As Integer) As DataTable
        ' Helper to build date condition dynamically for a given "date column name"
        Dim GetDateCondition As Func(Of String, String) = Function(colName)
                                                              Dim cond As String = ""
                                                              Dim sYear As Integer = Reports.SelectedYear
                                                              Dim sMonth As Integer = Reports.SelectedMonth
                                                              Select Case Reports.SelectedPeriod
                                                                  Case "Daily"
                                                                      cond = $" AND DATE({colName}) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "
                                                                  Case "Weekly"
                                                                      cond = $" AND YEARWEEK({colName}, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1) "
                                                                  Case "Monthly"
                                                                      If sMonth = 0 Then
                                                                          cond = $" AND YEAR({colName}) = {sYear} "
                                                                      Else
                                                                          cond = $" AND YEAR({colName}) = {sYear} AND MONTH({colName}) = {sMonth} "
                                                                      End If
                                                                  Case "Yearly"
                                                                      cond = $" AND YEAR({colName}) = {sYear} "
                                                              End Select
                                                              Return cond
                                                          End Function

        Dim orderDateCond As String = GetDateCondition("o.OrderDate")
        Dim resDateCond As String = GetDateCondition("r.EventDate")

        ' Optimizing query to aggregate Orders and Reservations efficiently using subqueries
        ' This avoids Cartesian products and correctly sums visits from both sources.
        Dim query As String =
            "SELECT " &
            "  c.CustomerID, " &
            "  CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName, " &
            "  IF(c.ContactNumber IS NOT NULL AND c.ContactNumber <> '', c.ContactNumber, c.Email) AS ContactInfo, " &
            "  c.CustomerType, " &
            "  CASE WHEN oStats.LastOrder IS NULL THEN rStats.LastRes WHEN rStats.LastRes IS NULL THEN oStats.LastOrder ELSE GREATEST(oStats.LastOrder, rStats.LastRes) END AS LastVisit, " &
            "  (COALESCE(oStats.OrderCount, 0) + COALESCE(rStats.ResCount, 0)) AS TotalVisits, " &
            "  COALESCE(oStats.TotalSpent, 0) AS TotalSpent " &
            "FROM customers c " &
            "LEFT JOIN ( " &
            "   SELECT CustomerID, COUNT(*) as OrderCount, SUM(TotalAmount) as TotalSpent, MAX(OrderDate) as LastOrder " &
            "   FROM orders o " &
            "   WHERE 1=1 " & orderDateCond &
            "   GROUP BY CustomerID " &
            ") oStats ON c.CustomerID = oStats.CustomerID " &
            "LEFT JOIN ( " &
            "   SELECT CustomerID, COUNT(*) as ResCount, MAX(EventDate) as LastRes " &
            "   FROM reservations r " &
            "   WHERE 1=1 " & resDateCond &
            "   GROUP BY CustomerID " &
            ") rStats ON c.CustomerID = rStats.CustomerID " &
            "WHERE c.AccountStatus = 'Active' " &
            "AND (CONCAT(c.FirstName, ' ', c.LastName) LIKE @search OR c.Email LIKE @search OR c.CustomerType LIKE @search) " &
            "ORDER BY TotalSpent DESC " &
            "LIMIT @limit OFFSET @offset;"

        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                cmd.Parameters.AddWithValue("@limit", limit)
                cmd.Parameters.AddWithValue("@offset", offset)
                cmd.CommandTimeout = 120
                Using adapter As New MySqlDataAdapter(cmd)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    Return table
                End Using
            End Using
        End Using
    End Function

    Private Sub ConfigureGrid()
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.EnableHeadersVisualStyles = True

        ' Ensure CustomerID column exists for reference (hidden)
        If Not DataGridView1.Columns.Contains("colCustomerID") Then
            Dim colID As New DataGridViewTextBoxColumn()
            colID.Name = "colCustomerID"
            colID.DataPropertyName = "CustomerID"
            colID.Visible = False
            DataGridView1.Columns.Add(colID)
        End If

        colCustomerName.DataPropertyName = "CustomerName"
        colContact.DataPropertyName = "ContactInfo"
        colType.DataPropertyName = "CustomerType"
        colLastVisit.DataPropertyName = "LastVisit"
        colTotalVisits.DataPropertyName = "TotalVisits"
        colTotalSpent.DataPropertyName = "TotalSpent"
        ' colAction is a button, so no DataPropertyName needed unless binding text, but we use static text "View History"

        colLastVisit.DefaultCellStyle.Format = "yyyy-MM-dd"
        colTotalSpent.DefaultCellStyle.Format = "₱#,##0.00"

        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        
        ' Ensure CellContentClick handler is attached 
        RemoveHandler DataGridView1.CellContentClick, AddressOf DataGridView1_CellContentClick
        AddHandler DataGridView1.CellContentClick, AddressOf DataGridView1_CellContentClick
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = DataGridView1.Columns("colAction").Index Then
            ' Retrieve data from the underlying DataRow to ensure we get the ID correctly
            Dim row As DataRowView = TryCast(DataGridView1.Rows(e.RowIndex).DataBoundItem, DataRowView)

            If row IsNot Nothing Then
                Dim customerID As Integer = Convert.ToInt32(row("CustomerID"))
                Dim customerName As String = row("CustomerName").ToString()

                Using historyForm As New FormCustomerOrderHistory(customerID, customerName)
                    historyForm.ShowDialog()
                End Using
            End If
        End If
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            Me.UseWaitCursor = isLoading
            DataGridView1.Enabled = Not isLoading

            If btnPrev IsNot Nothing Then btnPrev.Enabled = Not isLoading AndAlso _currentPage > 1
            If btnNext IsNot Nothing Then btnNext.Enabled = Not isLoading AndAlso _currentPage < _totalPages

            Label1.Text = If(isLoading, _baseTitle & " (Loading...)", _baseTitle)
        Catch
        End Try
    End Sub

    Private Sub UpdatePaginationUI()
        If lblPageStatus IsNot Nothing Then
            lblPageStatus.Text = $"Page {_currentPage} of {_totalPages} (Total Records: {_totalRecords:N0})"
        End If
        If btnPrev IsNot Nothing Then btnPrev.Enabled = (_currentPage > 1)
        If btnNext IsNot Nothing Then btnNext.Enabled = (_currentPage < _totalPages)
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If _currentPage < _totalPages Then
            _currentPage += 1
            BeginLoadCustomerHistory()
        End If
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If _currentPage > 1 Then
            _currentPage -= 1
            BeginLoadCustomerHistory()
        End If
    End Sub

    ' Search Events
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        _currentPage = 1 ' Reset to first page on search
        BeginLoadCustomerHistory()
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        SearchContainer.BorderColor = Color.FromArgb(99, 102, 241) ' Indigo focus
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        SearchContainer.BorderColor = Color.FromArgb(226, 232, 240)
    End Sub

    ' =======================================================================
    ' REFRESH DATA
    ' =======================================================================
    Public Sub RefreshData()
        _currentPage = 1
        'ConfigureDateFilter()
        BeginLoadCustomerHistory()
    End Sub



    ' =======================================================================
    ' PDF EXPORT
    ' =======================================================================
    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        Try
            ' Call the Reports form's export method
            If Reports.Instance IsNot Nothing Then
                Reports.Instance.ExportCurrentReport()
            Else
                MessageBox.Show("Export functionality is not available at this time.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error exporting to PDF: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
