Imports MySqlConnector

Public Class BatchManagement
    Private _ingredientID As Integer
    Private _ingredientName As String
    Private buttonsAdded As Boolean = False

    ' Constructor
    Public Sub New(ingredientID As Integer, ingredientName As String)
        InitializeComponent()
        _ingredientID = ingredientID
        _ingredientName = ingredientName
    End Sub

    ' Form Load
    Private Sub BatchManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Batch Management - " & _ingredientName
        lblIngredientName.Text = _ingredientName

        ' Add action buttons ONCE only
        If Not buttonsAdded Then
            AddActionButtons()
            buttonsAdded = True
        End If

        AutoDiscardExpiredBatches()
        AutoArchiveDepletedBatches()
        LoadBatchData()
    End Sub

    ' Automatic Discard for Expired Batches
    Private Sub AutoDiscardExpiredBatches()
        Dim expiredBatches As New List(Of Integer)()
        Dim details As New List(Of String)()

        Try
            openConn()
            ' Find expired but active batches
            Dim sql As String = "SELECT BatchID, BatchNumber, StockQuantity FROM inventory_batches " &
                                "WHERE IngredientID = @ingID AND BatchStatus = 'Active' " &
                                "AND ExpirationDate <= CURDATE()"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ingID", _ingredientID)
            
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                expiredBatches.Add(reader.GetInt32("BatchID"))
                details.Add($"Batch #{reader.GetString("BatchNumber")} (Qty: {reader.GetDecimal("StockQuantity")})")
            End While
            reader.Close()

            ' Process discards
            For i As Integer = 0 To expiredBatches.Count - 1
                Dim batchID As Integer = expiredBatches(i)
                
                ' reuse the stored procedure logic
                Dim cmdDiscard As New MySqlCommand("CALL DiscardBatch(@id, @reason, @notes)", conn)
                cmdDiscard.Parameters.AddWithValue("@id", batchID)
                cmdDiscard.Parameters.AddWithValue("@reason", "Auto-discarded: Expired")
                cmdDiscard.Parameters.AddWithValue("@notes", "System auto-discard due to expiration")
                cmdDiscard.ExecuteNonQuery()

                ' Log individual activity
                ActivityLogger.LogUserActivity("Auto-Discard", "Inventory", 
                    $"System auto-discarded {details(i)} due to expiration.", "System")
            Next

        Catch ex As Exception
            ' Silent fail or log to debug, to not block the user from opening the form
            ' We can optionally show a message but user requested "automatically discard... remove and log"
        Finally
            closeConn()
        End Try
    End Sub

    ' Auto-Archive Depleted Batches (Zero Qty)
    Private Sub AutoArchiveDepletedBatches()
        Dim depletedBatches As New List(Of Integer)()
        Dim details As New List(Of String)()

        Try
            openConn()
            ' Find active batches with zero stock
            Dim sql As String = "SELECT BatchID, BatchNumber FROM inventory_batches " &
                                "WHERE IngredientID = @ingID AND BatchStatus = 'Active' " &
                                "AND StockQuantity <= 0"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ingID", _ingredientID)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                depletedBatches.Add(reader.GetInt32("BatchID"))
                details.Add(reader.GetString("BatchNumber"))
            End While
            reader.Close()

            ' Process updates
            For i As Integer = 0 To depletedBatches.Count - 1
                Dim batchID As Integer = depletedBatches(i)
                Dim batchNum As String = details(i)

                ' Update status to Depleted
                Dim updateSql As String = "UPDATE inventory_batches SET BatchStatus = 'Depleted', UpdatedDate = NOW() WHERE BatchID = @id"
                Dim updateCmd As New MySqlCommand(updateSql, conn)
                updateCmd.Parameters.AddWithValue("@id", batchID)
                updateCmd.ExecuteNonQuery()

                ' Log Movement
                Dim logSql As String = "INSERT INTO inventory_movement_log " &
                                       "(IngredientID, BatchID, ChangeType, QuantityChanged, StockBefore, StockAfter, Reason, Source, MovementDate, Notes) " &
                                       "VALUES (@ingID, @batchID, 'STATUS', 0, 0, 0, 'Depleted', 'SYSTEM', NOW(), @notes)"
                Dim logCmd As New MySqlCommand(logSql, conn)
                logCmd.Parameters.AddWithValue("@ingID", _ingredientID)
                logCmd.Parameters.AddWithValue("@batchID", batchID)
                logCmd.Parameters.AddWithValue("@notes", $"Batch #{batchNum} marked as Depleted (Zero Stock)")
                logCmd.ExecuteNonQuery()

                ' Log Activity
                ActivityLogger.LogUserActivity("Batch Depleted", "Inventory",
                    $"Batch #{batchNum} auto-archived due to zero stock.", "System")
            Next

        Catch ex As Exception
            ' Silent fail
        Finally
            closeConn()
        End Try
    End Sub

    ' Add Edit + Discard buttons only (Delete button removed)
    Private Sub AddActionButtons()
        ' (1) Edit Button
        Dim btnEdit As New DataGridViewButtonColumn()
        btnEdit.Name = "btnEdit"
        btnEdit.HeaderText = "Edit"
        btnEdit.Text = "Edit"
        btnEdit.UseColumnTextForButtonValue = True
        dgvBatches.Columns.Add(btnEdit)

        ' (2) Discard Button
        Dim btnDiscard As New DataGridViewButtonColumn()
        btnDiscard.Name = "btnDiscard"
        btnDiscard.HeaderText = "Discard"
        btnDiscard.Text = "Discard"
        btnDiscard.UseColumnTextForButtonValue = True
        dgvBatches.Columns.Add(btnDiscard)
    End Sub

    ' Load batches
    Private Sub LoadBatchData()
        Try
            openConn()

            Dim sql As String = "
                SELECT 
                    BatchID AS 'Batch ID',
                    BatchNumber AS 'Batch Number',
                    StockQuantity AS 'Current Stock',
                    OriginalQuantity AS 'Original Qty',
                    UnitType AS 'Unit',
                    CostPerUnit AS 'Cost/Unit',
                    (StockQuantity * CostPerUnit) AS 'Total Cost',
                    PurchaseDate AS 'Purchase Date',
                    ExpirationDate AS 'Expiration',
                    CASE 
                        WHEN ExpirationDate IS NULL THEN NULL
                        ELSE DATEDIFF(ExpirationDate, CURDATE())
                    END AS 'Days Left',
                    CASE 
                        WHEN BatchStatus = 'Expired' THEN 'EXPIRED'
                        WHEN BatchStatus = 'Depleted' THEN 'Depleted'
                        WHEN ExpirationDate IS NULL THEN 'No Expiry'
                        WHEN ExpirationDate <= CURDATE() THEN 'EXPIRED'
                        WHEN DATEDIFF(ExpirationDate, CURDATE()) <= 3 THEN 'CRITICAL'
                        WHEN DATEDIFF(ExpirationDate, CURDATE()) <= 7 THEN 'WARNING'
                        WHEN DATEDIFF(ExpirationDate, CURDATE()) <= 14 THEN 'Monitor'
                        ELSE 'Fresh'
                    END AS 'Alert',
                    BatchStatus AS 'Status',
                    StorageLocation AS 'Storage Location',
                    ROUND((StockQuantity / OriginalQuantity) * 100, 1) AS 'Remaining %',
                    Notes
                FROM inventory_batches
                WHERE IngredientID = @ingredientID
                 AND BatchStatus = 'Active'
                ORDER BY ExpirationDate ASC;
            "

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ingredientID", _ingredientID)

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvBatches.DataSource = dt

            FormatBatchGrid()
            ColorCodeBatches()
            LoadBatchStatistics()

        Catch ex As Exception
            MessageBox.Show("Error loading batches: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' Format grid
    Private Sub FormatBatchGrid()
        With dgvBatches
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
             .RowTemplate.Height = 35
                .DefaultCellStyle.Font = New Font("Segoe UI", 9)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        ' Format currency
        If dgvBatches.Columns.Contains("Cost/Unit") Then
            dgvBatches.Columns("Cost/Unit").DefaultCellStyle.Format = "₱#,##0.00"
        End If
        If dgvBatches.Columns.Contains("Total Cost") Then
            dgvBatches.Columns("Total Cost").DefaultCellStyle.Format = "₱#,##0.00"
        End If

        ' Date formatting
        If dgvBatches.Columns.Contains("Purchase Date") Then
            dgvBatches.Columns("Purchase Date").DefaultCellStyle.Format = "MMM dd, yyyy"
        End If
        If dgvBatches.Columns.Contains("Expiration") Then
            dgvBatches.Columns("Expiration").DefaultCellStyle.Format = "MMM dd, yyyy"
        End If

        ' Hide Batch ID
        If dgvBatches.Columns.Contains("Batch ID") Then
            dgvBatches.Columns("Batch ID").Visible = False
        End If

        ' Move Edit to the end
        If dgvBatches.Columns.Contains("btnEdit") Then
            dgvBatches.Columns("btnEdit").DisplayIndex = dgvBatches.Columns.Count - 1
        End If

        ' Move Discard to the end (will be after Edit)
        If dgvBatches.Columns.Contains("btnDiscard") Then
            dgvBatches.Columns("btnDiscard").DisplayIndex = dgvBatches.Columns.Count - 1
        End If
    End Sub

    ' Apply row color codes
    Private Sub ColorCodeBatches()
        For Each row As DataGridViewRow In dgvBatches.Rows
            Dim alert As String = row.Cells("Alert").Value?.ToString()

            Select Case alert
                Case "EXPIRED"
                    row.Cells("Alert").Style.BackColor = Color.DarkRed
                    row.Cells("Alert").Style.ForeColor = Color.White

                Case "CRITICAL"
                    row.Cells("Alert").Style.BackColor = Color.Red
                    row.Cells("Alert").Style.ForeColor = Color.White

                Case "WARNING"
                    row.Cells("Alert").Style.BackColor = Color.Gold

                Case "Monitor"
                    row.Cells("Alert").Style.BackColor = Color.Khaki

                Case "Fresh"
                    row.Cells("Alert").Style.BackColor = Color.Green
                    row.Cells("Alert").Style.ForeColor = Color.White
            End Select
        Next
    End Sub

    ' Load statistics - Now fully dynamic with proper parameters
    Private Sub LoadBatchStatistics()
        Try
            openConn()

            ' Total stock
            Dim cmdTotalStock As New MySqlCommand("
                SELECT COALESCE(SUM(StockQuantity), 0)
                FROM inventory_batches
                WHERE IngredientID = @ingredientID AND BatchStatus='Active'", conn)
            cmdTotalStock.Parameters.AddWithValue("@ingredientID", _ingredientID)
            lblTotalStock.Text = cmdTotalStock.ExecuteScalar().ToString()

            ' Active batches
            Dim cmdActiveBatches As New MySqlCommand("
                SELECT COUNT(*)
                FROM inventory_batches
                WHERE IngredientID = @ingredientID AND BatchStatus='Active'", conn)
            cmdActiveBatches.Parameters.AddWithValue("@ingredientID", _ingredientID)
            lblActiveBatches.Text = cmdActiveBatches.ExecuteScalar().ToString()

            ' Total value
            Dim cmdTotalValue As New MySqlCommand("
                SELECT COALESCE(SUM(StockQuantity * CostPerUnit), 0)
                FROM inventory_batches
                WHERE IngredientID = @ingredientID AND BatchStatus='Active'", conn)
            cmdTotalValue.Parameters.AddWithValue("@ingredientID", _ingredientID)
            lblTotalValue.Text = "₱" & Convert.ToDecimal(cmdTotalValue.ExecuteScalar()).ToString("#,##0.00")

        Catch ex As Exception
            MessageBox.Show("Statistics error: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' Handle action button clicks (Delete button removed)
    Private Sub dgvBatches_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBatches.CellContentClick
        If e.RowIndex < 0 Then Exit Sub

        Dim batchID As Integer = dgvBatches.Rows(e.RowIndex).Cells("Batch ID").Value
        Dim batchStock As Decimal = dgvBatches.Rows(e.RowIndex).Cells("Current Stock").Value

        Select Case dgvBatches.Columns(e.ColumnIndex).Name
            Case "btnEdit"
                Dim f As New EditBatch(batchID, _ingredientName)
                If f.ShowDialog() = DialogResult.OK Then LoadBatchData()

            Case "btnDiscard"
                DiscardBatch(batchID, batchStock)
        End Select
    End Sub

    ' Discard batch
    ' Discard batch
    Private Sub DiscardBatch(id As Integer, stock As Decimal)
        Dim reason As String = InputBox("Enter reason for discarding this batch:", "Discard Reason", "Expired/Damaged")

        If String.IsNullOrWhiteSpace(reason) Then
            MessageBox.Show("Please provide a reason for discarding.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Discard this batch? This will mark it as discarded and set stock to zero." & vbCrLf & vbCrLf & "Stock to discard: " & stock.ToString("N2"), "Confirm Discard", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

        Try
            openConn()

            ' Call stored procedure to properly log the discard
            Dim cmd As New MySqlCommand("CALL DiscardBatch(@id, @reason, @notes)", conn)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.Parameters.AddWithValue("@reason", reason)
            cmd.Parameters.AddWithValue("@notes", "Batch discarded from Batch Management interface")
            cmd.ExecuteNonQuery()

            MessageBox.Show("Batch discarded successfully." & vbCrLf & "Discarded quantity: " & stock.ToString("N2"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            
            ' Log Activity
            ActivityLogger.LogUserActivity("Batch Discarded", "Inventory", $"Discarded Batch #{id} - Reason: {reason}", "Admin Panel")

            LoadBatchData()

        Catch ex As Exception
            MessageBox.Show("Error discarding batch: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' View History
    Private Sub btnViewHistory_Click(sender As Object, e As EventArgs) Handles btnViewHistory.Click
        Try
            ' Open the InventoryMovementHistory form filtered for this ingredient
            Dim historyForm As New InventoryMovementHistory(_ingredientID, _ingredientName)
            historyForm.ShowDialog()

            ' Refresh batch data after closing history (in case adjustments were made)
            LoadBatchData()

        Catch ex As Exception
            MessageBox.Show("Error opening movement history: " & ex.Message,
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAddBatch_Click(sender As Object, e As EventArgs) Handles btnAddBatch.Click
        Dim addForm As New AddNewBatch(_ingredientID, _ingredientName, "")
        If addForm.ShowDialog() = DialogResult.OK Then LoadBatchData()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' Draw Border
    Private Sub Form_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim borderColor As Color = Color.LightGray
        Dim borderThickness As Integer = 1
        ControlPaint.DrawBorder(e.Graphics, Me.ClientRectangle,
                                borderColor, borderThickness, ButtonBorderStyle.Solid,
                                borderColor, borderThickness, ButtonBorderStyle.Solid,
                                borderColor, borderThickness, ButtonBorderStyle.Solid,
                                borderColor, borderThickness, ButtonBorderStyle.Solid)
    End Sub
End Class