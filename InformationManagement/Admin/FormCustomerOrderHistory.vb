Imports System.Windows.Forms
Imports System.Drawing
Imports MySqlConnector

Public Class FormCustomerOrderHistory
    Inherits Form

    Public Property CustomerID As Integer
    Public Property CustomerName As String

    Private ReadOnly connectionString As String = modDB.strConnection

    Public Sub New(custID As Integer, custName As String)
        InitializeComponent()
        Me.CustomerID = custID
        Me.CustomerName = custName
    End Sub

    Private Sub FormCustomerOrderHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblTitle.Text = $"History - {CustomerName}"
        txtSearch.Text = "Search ID..."
        txtSearch.ForeColor = Color.Gray
        
        LoadOrderHistory()
        LoadReservationHistory()
    End Sub

    ' ==========================================
    ' TAB 1: ORDERS
    ' ==========================================
    Private Sub LoadOrderHistory()
        Dim dt As New DataTable()
        Dim searchText As String = txtSearch.Text.Trim()
        If searchText = "Search ID..." Then searchText = ""

        Dim query As String = "
            SELECT 
                o.OrderID,
                CONCAT(DATE_FORMAT(o.OrderDate, '%b %d, %Y'), ' ', TIME_FORMAT(o.OrderTime, '%h:%i %p')) AS OrderDateTime,
                o.OrderType,
                (SELECT COALESCE(SUM(Quantity), 0) FROM order_items WHERE OrderID = o.OrderID) AS ItemCount,
                o.TotalAmount,
                COALESCE(p.PaymentMethod, 'Cash') AS PaymentMethod,
                o.OrderStatus
            FROM orders o
            LEFT JOIN payments p ON o.OrderID = p.OrderID
            WHERE o.CustomerID = @custID
            AND (CAST(o.OrderID AS CHAR) LIKE @search OR o.OrderStatus LIKE @search)
            ORDER BY o.OrderDate DESC, o.OrderTime DESC"

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@custID", CustomerID)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using



            FormatOrderGrid() ' Set AutoGenerateColumns = False FIRST
            dgvOrders.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        End Try
    End Sub

    Private Sub FormatOrderGrid()
        With dgvOrders
            .AutoGenerateColumns = False
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .DefaultCellStyle.SelectionBackColor = Color.White
            .RowTemplate.Height = 45
            .ColumnHeadersHeight = 45
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(108, 117, 125)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray

            ' Map properties
            If .Columns.Contains("colOrderID") Then .Columns("colOrderID").DataPropertyName = "OrderID"
            If .Columns.Contains("colDateTime") Then .Columns("colDateTime").DataPropertyName = "OrderDateTime"
            If .Columns.Contains("colOrderType") Then .Columns("colOrderType").DataPropertyName = "OrderType"
            If .Columns.Contains("colItemCount") Then .Columns("colItemCount").DataPropertyName = "ItemCount"
            If .Columns.Contains("colTotalAmount") Then .Columns("colTotalAmount").DataPropertyName = "TotalAmount"
            If .Columns.Contains("colPayment") Then .Columns("colPayment").DataPropertyName = "PaymentMethod"
            If .Columns.Contains("colStatus") Then .Columns("colStatus").DataPropertyName = "OrderStatus"

            If .Columns.Contains("colTotalAmount") Then .Columns("colTotalAmount").DefaultCellStyle.Format = "₱#,##0.00"
        End With
    End Sub

    Private Sub dgvOrders_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvOrders.CellFormatting
        If dgvOrders.Columns(e.ColumnIndex).Name = "colStatus" AndAlso e.Value IsNot Nothing Then
            Dim status As String = e.Value.ToString()
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)

            Select Case status
                Case "Completed"
                    e.CellStyle.BackColor = Color.FromArgb(209, 250, 229)
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129)
                Case "Cancelled"
                    e.CellStyle.BackColor = Color.FromArgb(254, 226, 226)
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68)
                Case "Pending"
                    e.CellStyle.BackColor = Color.FromArgb(241, 245, 249)
                    e.CellStyle.ForeColor = Color.FromArgb(100, 116, 139)
            End Select
        End If

        If dgvOrders.Columns(e.ColumnIndex).Name = "colOrderType" AndAlso e.Value IsNot Nothing Then
             ' This logic was originally in the combined grid, keeping it for dgvOrders if needed
             ' Although for a dedicated orders grid, OrderType might always be "Order" or similar.
             ' If there are different order types (e.g., "Dine-in", "Takeaway"), this could be useful.
             ' For now, assuming it's not strictly necessary to change color for "Order" type itself.
             ' If specific order types need coloring, add cases here.
        End If

        If dgvOrders.Columns(e.ColumnIndex).Name = "colOrderID" AndAlso e.Value IsNot Nothing Then
            e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235)
            e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        End If
    End Sub

    ' ==========================================
    ' TAB 2: RESERVATIONS
    ' ==========================================
    Private Sub LoadReservationHistory()
        Dim dt As New DataTable()
        Dim searchText As String = txtSearch.Text.Trim()
        If searchText = "Search ID..." Then searchText = ""

        Dim query As String = "
            SELECT
                r.ReservationID,
                CONCAT(DATE_FORMAT(r.EventDate, '%b %d, %Y'), ' ', TIME_FORMAT(r.EventTime, '%h:%i %p')) AS EventDateTime,
                r.NumberOfGuests,
                r.ReservationStatus,
                COALESCE(p.PaymentMethod, 'N/A') AS PaymentMethod,
                (SELECT COALESCE(SUM(TotalPrice), 0) FROM reservation_items WHERE ReservationID = r.ReservationID) AS TotalAmount,
                (SELECT GROUP_CONCAT(CONCAT(ProductName, ' (', Quantity, ')') SEPARATOR ', ') FROM reservation_items WHERE ReservationID = r.ReservationID) AS ProductSelection
            FROM reservations r
            LEFT JOIN payments p ON r.ReservationID = p.ReservationID
            WHERE r.CustomerID = @custID
            AND (CAST(r.ReservationID AS CHAR) LIKE @search OR r.ReservationStatus LIKE @search)
            ORDER BY r.EventDate DESC, r.EventTime DESC"

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@custID", CustomerID)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            FormatReservationGrid() ' Set AutoGenerateColumns = False FIRST
            dgvReservations.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error loading reservations: " & ex.Message)
        End Try
    End Sub

    Private Sub FormatReservationGrid()
        With dgvReservations
            .AutoGenerateColumns = False
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .DefaultCellStyle.SelectionBackColor = Color.White
            .RowTemplate.Height = 45
            .ColumnHeadersHeight = 45
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 9)
            
            ' Map properties
            If .Columns.Contains("colResID") Then .Columns("colResID").DataPropertyName = "ReservationID"
            If .Columns.Contains("colResDateTime") Then .Columns("colResDateTime").DataPropertyName = "EventDateTime"
            If .Columns.Contains("colResItems") Then .Columns("colResItems").DataPropertyName = "ProductSelection"
            If .Columns.Contains("colGuests") Then .Columns("colGuests").DataPropertyName = "NumberOfGuests"
            If .Columns.Contains("colResTotal") Then .Columns("colResTotal").DataPropertyName = "TotalAmount"
            If .Columns.Contains("colResStatus") Then .Columns("colResStatus").DataPropertyName = "ReservationStatus"
            If .Columns.Contains("colResPayment") Then .Columns("colResPayment").DataPropertyName = "PaymentMethod"

            If .Columns.Contains("colResTotal") Then .Columns("colResTotal").DefaultCellStyle.Format = "₱#,##0.00"
        End With
    End Sub

    Private Sub dgvReservations_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvReservations.CellFormatting
        If dgvReservations.Columns(e.ColumnIndex).Name = "colResStatus" AndAlso e.Value IsNot Nothing Then
            Dim status As String = e.Value.ToString()
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)

            Select Case status
                Case "Confirmed", "Completed"
                    e.CellStyle.BackColor = Color.FromArgb(209, 250, 229)
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129)
                Case "Cancelled"
                    e.CellStyle.BackColor = Color.FromArgb(254, 226, 226)
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68)
                Case "Pending"
                    e.CellStyle.BackColor = Color.FromArgb(241, 245, 249)
                    e.CellStyle.ForeColor = Color.FromArgb(100, 116, 139)
            End Select
        End If

        If dgvReservations.Columns(e.ColumnIndex).Name = "colResID" AndAlso e.Value IsNot Nothing Then
            e.CellStyle.ForeColor = Color.Purple
            e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        End If
    End Sub

    ' ==========================================
    ' COMMON EVENTS
    ' ==========================================
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        If txtSearch.Text = "Search ID..." Then
            txtSearch.Text = ""
            txtSearch.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text = "" Then
            txtSearch.Text = "Search ID..."
            txtSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text <> "Search ID..." Then
            LoadOrderHistory()
            LoadReservationHistory()
        End If
    End Sub

End Class
