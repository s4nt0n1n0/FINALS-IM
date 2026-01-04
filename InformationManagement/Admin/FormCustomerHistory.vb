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
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim sYear As Integer = Reports.SelectedYear
        Dim sMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                 periodFilter = $" AND DATE(OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "
            Case "Weekly"
                 periodFilter = $" AND YEARWEEK(OrderDate, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1) "

            Case "Monthly"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(OrderDate) = {sYear} AND MONTH(OrderDate) = {sMonth} "
                End If
            Case "Yearly"
                periodFilter = $" AND YEAR(OrderDate) = {sYear} "
        End Select

        Dim query As String = "SELECT COUNT(*) FROM orders WHERE (OrderID LIKE @search OR OrderType LIKE @search OR OrderStatus LIKE @search)" & periodFilter
        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Function FetchCustomerHistoryTable(searchText As String, offset As Integer, limit As Integer) As DataTable
        ' Get period filter from Reports form
        Dim periodFilter As String = ""
        Dim sYear As Integer = Reports.SelectedYear
        Dim sMonth As Integer = Reports.SelectedMonth

        Select Case Reports.SelectedPeriod
            Case "Daily"
                 periodFilter = $" AND DATE(o.OrderDate) = '{Reports.GlobalFilterDate:yyyy-MM-dd}' "
            Case "Weekly"
                 periodFilter = $" AND YEARWEEK(o.OrderDate, 1) = YEARWEEK('{Reports.GlobalFilterDate:yyyy-MM-dd}', 1) "

            Case "Monthly"
                If sMonth = 0 Then
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
                Else
                    periodFilter = $" AND YEAR(o.OrderDate) = {sYear} AND MONTH(o.OrderDate) = {sMonth} "
                End If
            Case "Yearly"
                periodFilter = $" AND YEAR(o.OrderDate) = {sYear} "
        End Select

        ' Optimized query targeting only necessary columns and using paging with search
        Dim query As String =
            "SELECT " &
            "  o.OrderDate, " &
            "  o.OrderID, " &
            "  o.OrderType, " &
            "  (SELECT GROUP_CONCAT(CONCAT(ProductName, ' (', Quantity, ')') SEPARATOR ', ') " &
            "   FROM order_items WHERE OrderID = o.OrderID) AS Items, " &
            "  IFNULL(o.TotalAmount, 0) AS TotalAmount, " &
            "  o.OrderStatus " &
            "FROM orders o " &
            "WHERE (o.OrderID LIKE @search OR o.OrderType LIKE @search OR o.OrderStatus LIKE @search) " & periodFilter & " " &
            "ORDER BY o.OrderDate DESC, o.OrderID DESC " &
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
        ' Use the designer-defined columns; bind to a DataTable for faster loading.
        DataGridView1.AutoGenerateColumns = False

        dateid.DataPropertyName = "OrderDate"
        Orderid.DataPropertyName = "OrderID"
        Type.DataPropertyName = "OrderType"
        Items.DataPropertyName = "Items"
        Amount.DataPropertyName = "TotalAmount"
        Status.DataPropertyName = "OrderStatus"

        dateid.DefaultCellStyle.Format = "yyyy-MM-dd"
        Amount.DefaultCellStyle.Format = "₱#,##0.00"

        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
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



End Class
