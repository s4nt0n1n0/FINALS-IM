Imports MySqlConnector

Public Class InventoryMovementHistory
    Private _ingredientID As Integer
    Private _ingredientName As String

    ' Constructor for viewing all movements
    Public Sub New()
        InitializeComponent()
        _ingredientID = 0
        _ingredientName = "All Ingredients"
    End Sub

    ' Constructor for viewing specific ingredient movements
    Public Sub New(ingredientID As Integer, ingredientName As String)
        InitializeComponent()
        _ingredientID = ingredientID
        _ingredientName = ingredientName
    End Sub

    Private Sub InventoryMovementHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Set form properties
            Me.WindowState = FormWindowState.Maximized
            Me.Text = "Inventory Movement History - " & _ingredientName
            Me.lblTitle.Text = "Inventory Movement History - " & _ingredientName

            ' Add Overall Total Cost Label programmatically if it doesn't exist
            If Me.Controls.Find("lblOverallTotalCost", True).Length = 0 Then
                Dim lblOverallCost As New Label()
                lblOverallCost.Name = "lblOverallTotalCost"
                lblOverallCost.Text = "Overall Total Cost: ₱0.00"
                lblOverallCost.Font = New Font("Segoe UI", 14, FontStyle.Bold)
                lblOverallCost.ForeColor = Color.DarkGreen
                lblOverallCost.AutoSize = True
                lblOverallCost.BackColor = Color.Transparent

                ' Position it to the left of Close button
                ' Adjust these coordinates based on your Close button position
                lblOverallCost.Left = 20
                lblOverallCost.Top = btnClose.Top + ((btnClose.Height - lblOverallCost.Height) \ 2)

                Me.Controls.Add(lblOverallCost)
                lblOverallCost.BringToFront()
            End If

            ' Initialize filters
            InitializeFilters()

            ' Load movement data
            LoadMovementHistory()

            ' Load statistics
            LoadMovementStatistics()

            ' Load total cost
            LoadTotalCost()

        Catch ex As Exception
            MessageBox.Show("Error loading movement history: " & ex.Message,
                          "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeFilters()
        ' Date filters
        dtpStartDate.Value = Date.Now.AddMonths(-1)
        dtpEndDate.Value = Date.Now

        ' Source filter
        cmbSource.Items.Clear()
        cmbSource.Items.Add("All Sources")
        cmbSource.Items.Add("POS")
        cmbSource.Items.Add("WEBSITE")
        cmbSource.Items.Add("ADMIN")
        cmbSource.Items.Add("SYSTEM")
        cmbSource.SelectedIndex = 0

        ' Change type filter
        cmbChangeType.Items.Clear()
        cmbChangeType.Items.Add("All Types")
        cmbChangeType.Items.Add("ADD")
        cmbChangeType.Items.Add("DEDUCT")
        cmbChangeType.Items.Add("ADJUST")
        cmbChangeType.Items.Add("DISCARD")

        cmbChangeType.SelectedIndex = 0
    End Sub

    Private Sub LoadMovementHistory()
        Try
            openConn()

            Dim sql As String = "
            SELECT 
                iml.MovementID AS 'Movement ID',
                iml.MovementDate AS 'Date & Time',
                i.IngredientName AS 'Ingredient',
                COALESCE(ic.CategoryName, 'Uncategorized') AS 'Category',
                ib.BatchNumber AS 'Batch #',
                iml.ChangeType AS 'Type',

                -- Display values with unit separately
                FORMAT(iml.QuantityChanged, 2) AS 'Change',
                FORMAT(iml.StockBefore, 2) AS 'Stock Before',
                FORMAT(iml.StockAfter, 2) AS 'Stock After',
                iml.UnitType AS 'Unit',

                -- ADD COST COLUMNS
                FORMAT(ib.CostPerUnit, 2) AS 'Cost/Unit',
                FORMAT(ABS(iml.QuantityChanged) * ib.CostPerUnit, 2) AS 'Movement Cost',

                iml.Reason,
                iml.Source,
                COALESCE(iml.SourceName, 'System') AS 'Performed By',
                CASE 
                    WHEN iml.OrderID IS NOT NULL THEN CONCAT('Order #', iml.OrderID)
                    WHEN iml.ReservationID IS NOT NULL THEN CONCAT('Reservation #', iml.ReservationID)
                    ELSE iml.ReferenceNumber
                END AS 'Reference',
                ib.StorageLocation AS 'Storage',
                iml.Notes
            FROM inventory_movement_log iml
            INNER JOIN ingredients i ON iml.IngredientID = i.IngredientID
            LEFT JOIN ingredient_categories ic ON i.CategoryID = ic.CategoryID
            INNER JOIN inventory_batches ib ON iml.BatchID = ib.BatchID
            WHERE 1=1
        "

            ' Add filters
            If _ingredientID > 0 Then
                sql &= " AND iml.IngredientID = @ingredientID"
            End If

            sql &= " AND DATE(iml.MovementDate) BETWEEN @startDate AND @endDate"

            If cmbSource.SelectedIndex > 0 Then
                sql &= " AND iml.Source = @source"
            End If

            If cmbChangeType.SelectedIndex > 0 Then
                sql &= " AND iml.ChangeType = @changeType"
            End If

            If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                sql &= " AND (i.IngredientName LIKE @search OR ib.BatchNumber LIKE @search OR iml.Reason LIKE @search)"
            End If

            sql &= " ORDER BY iml.MovementDate ASC, iml.MovementID ASC LIMIT 1000"

            Dim cmd As New MySqlCommand(sql, conn)

            If _ingredientID > 0 Then
                cmd.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If

            cmd.Parameters.AddWithValue("@startDate", dtpStartDate.Value.Date)
            cmd.Parameters.AddWithValue("@endDate", dtpEndDate.Value.Date)

            If cmbSource.SelectedIndex > 0 Then
                cmd.Parameters.AddWithValue("@source", cmbSource.Text)
            End If

            If cmbChangeType.SelectedIndex > 0 Then
                cmd.Parameters.AddWithValue("@changeType", cmbChangeType.Text)
            End If

            If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                cmd.Parameters.AddWithValue("@search", "%" & txtSearch.Text & "%")
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvMovements.DataSource = dt

            FormatMovementGrid()
            ColorCodeMovements()

            ' Update the record count in subtitle
            Me.lblSubtitle.Text = "Showing " & dt.Rows.Count.ToString() & " records (oldest to newest)"

            ' Auto-scroll to bottom to show latest entry
            If dgvMovements.Rows.Count > 0 Then
                dgvMovements.FirstDisplayedScrollingRowIndex = dgvMovements.Rows.Count - 1
                dgvMovements.CurrentCell = dgvMovements.Rows(dgvMovements.Rows.Count - 1).Cells(1)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading movements: " & ex.Message,
                      "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub ScrollToLatestEntry()
        Try
            If dgvMovements.Rows.Count > 0 Then
                dgvMovements.FirstDisplayedScrollingRowIndex = dgvMovements.Rows.Count - 1
                dgvMovements.ClearSelection()
                dgvMovements.Rows(dgvMovements.Rows.Count - 1).Selected = True
                dgvMovements.CurrentCell = dgvMovements.Rows(dgvMovements.Rows.Count - 1).Cells(1)
            End If
        Catch ex As Exception
            ' Silently fail if scrolling fails
        End Try
    End Sub

    Private Sub FormatMovementGrid()
        Try
            With dgvMovements
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .RowTemplate.Height = 35
                .DefaultCellStyle.Font = New Font("Segoe UI", 9)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250)
                .ReadOnly = True
                .AllowUserToAddRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect

                ' Hide Movement ID
                If .Columns.Contains("Movement ID") Then
                    .Columns("Movement ID").Visible = False
                End If

                ' Set column widths
                If .Columns.Contains("Date & Time") Then
                    .Columns("Date & Time").Width = 150
                    .Columns("Date & Time").DefaultCellStyle.Format = "MMM dd, yyyy HH:mm"
                End If

                If .Columns.Contains("Ingredient") Then
                    .Columns("Ingredient").Width = 150
                End If

                If .Columns.Contains("Category") Then
                    .Columns("Category").Width = 120
                End If

                If .Columns.Contains("Batch #") Then
                    .Columns("Batch #").Width = 130
                End If

                If .Columns.Contains("Type") Then
                    .Columns("Type").Width = 80
                    .Columns("Type").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Columns("Type").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End If

                If .Columns.Contains("Change") Then
                    .Columns("Change").Width = 100
                    .Columns("Change").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("Change").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End If

                If .Columns.Contains("Stock Before") Then
                    .Columns("Stock Before").Width = 120
                    .Columns("Stock Before").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If

                If .Columns.Contains("Stock After") Then
                    .Columns("Stock After").Width = 120
                    .Columns("Stock After").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If

                ' FORMAT NEW COST COLUMNS
                If .Columns.Contains("Cost/Unit") Then
                    .Columns("Cost/Unit").Width = 100
                    .Columns("Cost/Unit").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("Cost/Unit").HeaderText = "Cost/Unit (₱)"
                End If

                If .Columns.Contains("Movement Cost") Then
                    .Columns("Movement Cost").Width = 120
                    .Columns("Movement Cost").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("Movement Cost").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    .Columns("Movement Cost").DefaultCellStyle.ForeColor = Color.DarkGreen
                    .Columns("Movement Cost").HeaderText = "Movement Cost (₱)"
                End If

                If .Columns.Contains("Reason") Then
                    .Columns("Reason").Width = 200
                End If

                If .Columns.Contains("Source") Then
                    .Columns("Source").Width = 80
                    .Columns("Source").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Columns("Source").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End If

                If .Columns.Contains("Performed By") Then
                    .Columns("Performed By").Width = 140
                End If

                If .Columns.Contains("Reference") Then
                    .Columns("Reference").Width = 130
                End If

                If .Columns.Contains("Storage") Then
                    .Columns("Storage").Width = 150
                End If

                If .Columns.Contains("Notes") Then
                    .Columns("Notes").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns("Notes").MinimumWidth = 200
                End If
            End With

        Catch ex As Exception
            ' Silent fail for formatting
        End Try
    End Sub

    Private Sub ColorCodeMovements()
        Try
            For Each row As DataGridViewRow In dgvMovements.Rows
                If Not row.IsNewRow Then
                    ' Color code by change type
                    If row.Cells("Type").Value IsNot Nothing Then
                        Dim changeType As String = row.Cells("Type").Value.ToString()

                        Select Case changeType
                            Case "ADD"
                                row.Cells("Type").Style.BackColor = Color.FromArgb(40, 167, 69)
                                row.Cells("Type").Style.ForeColor = Color.White
                                row.Cells("Change").Style.ForeColor = Color.FromArgb(40, 167, 69)

                            Case "DEDUCT"
                                row.Cells("Type").Style.BackColor = Color.FromArgb(220, 53, 69)
                                row.Cells("Type").Style.ForeColor = Color.White
                                row.Cells("Change").Style.ForeColor = Color.FromArgb(220, 53, 69)

                            Case "ADJUST"
                                row.Cells("Type").Style.BackColor = Color.FromArgb(255, 193, 7)
                                row.Cells("Type").Style.ForeColor = Color.Black
                                row.Cells("Change").Style.ForeColor = Color.FromArgb(255, 193, 7)

                            Case "DISCARD"
                                row.Cells("Type").Style.BackColor = Color.FromArgb(108, 117, 125)
                                row.Cells("Type").Style.ForeColor = Color.White
                                row.Cells("Change").Style.ForeColor = Color.FromArgb(108, 117, 125)

                            Case "TRANSFER"
                                row.Cells("Type").Style.BackColor = Color.FromArgb(23, 162, 184)
                                row.Cells("Type").Style.ForeColor = Color.White
                                row.Cells("Change").Style.ForeColor = Color.FromArgb(23, 162, 184)
                        End Select
                    End If

                    ' Color code by source
                    If row.Cells("Source").Value IsNot Nothing Then
                        Dim source As String = row.Cells("Source").Value.ToString()

                        Select Case source
                            Case "POS"
                                row.Cells("Source").Style.BackColor = Color.FromArgb(23, 162, 184)
                                row.Cells("Source").Style.ForeColor = Color.White

                            Case "WEBSITE"
                                row.Cells("Source").Style.BackColor = Color.FromArgb(111, 66, 193)
                                row.Cells("Source").Style.ForeColor = Color.White

                            Case "ADMIN"
                                row.Cells("Source").Style.BackColor = Color.FromArgb(253, 126, 20)
                                row.Cells("Source").Style.ForeColor = Color.White

                            Case "SYSTEM"
                                row.Cells("Source").Style.BackColor = Color.FromArgb(108, 117, 125)
                                row.Cells("Source").Style.ForeColor = Color.White
                        End Select
                    End If
                End If
            Next

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub LoadMovementStatistics()
        Try
            openConn()

            Dim startDate As Date = dtpStartDate.Value.Date
            Dim endDate As Date = dtpEndDate.Value.Date

            ' Total movements
            Dim sqlTotal As String = "
                SELECT COUNT(*) 
                FROM inventory_movement_log
                WHERE DATE(MovementDate) BETWEEN @startDate AND @endDate
            "
            If _ingredientID > 0 Then
                sqlTotal &= " AND IngredientID = @ingredientID"
            End If

            Dim cmdTotal As New MySqlCommand(sqlTotal, conn)
            cmdTotal.Parameters.AddWithValue("@startDate", startDate)
            cmdTotal.Parameters.AddWithValue("@endDate", endDate)
            If _ingredientID > 0 Then
                cmdTotal.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If
            lblTotalValue.Text = cmdTotal.ExecuteScalar().ToString()

            ' POS movements
            Dim sqlPOS As String = "
                SELECT COUNT(*) 
                FROM inventory_movement_log
                WHERE Source = 'POS' 
                AND DATE(MovementDate) BETWEEN @startDate AND @endDate
            "
            If _ingredientID > 0 Then
                sqlPOS &= " AND IngredientID = @ingredientID"
            End If

            Dim cmdPOS As New MySqlCommand(sqlPOS, conn)
            cmdPOS.Parameters.AddWithValue("@startDate", startDate)
            cmdPOS.Parameters.AddWithValue("@endDate", endDate)
            If _ingredientID > 0 Then
                cmdPOS.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If
            lblPOSValue.Text = cmdPOS.ExecuteScalar().ToString()

            ' Website movements
            Dim sqlWebsite As String = "
                SELECT COUNT(*) 
                FROM inventory_movement_log
                WHERE Source = 'WEBSITE' 
                AND DATE(MovementDate) BETWEEN @startDate AND @endDate
            "
            If _ingredientID > 0 Then
                sqlWebsite &= " AND IngredientID = @ingredientID"
            End If

            Dim cmdWebsite As New MySqlCommand(sqlWebsite, conn)
            cmdWebsite.Parameters.AddWithValue("@startDate", startDate)
            cmdWebsite.Parameters.AddWithValue("@endDate", endDate)
            If _ingredientID > 0 Then
                cmdWebsite.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If
            lblWebValue.Text = cmdWebsite.ExecuteScalar().ToString()

            ' Admin movements
            Dim sqlAdmin As String = "
                SELECT COUNT(*) 
                FROM inventory_movement_log
                WHERE Source = 'ADMIN' 
                AND DATE(MovementDate) BETWEEN @startDate AND @endDate
            "
            If _ingredientID > 0 Then
                sqlAdmin &= " AND IngredientID = @ingredientID"
            End If

            Dim cmdAdmin As New MySqlCommand(sqlAdmin, conn)
            cmdAdmin.Parameters.AddWithValue("@startDate", startDate)
            cmdAdmin.Parameters.AddWithValue("@endDate", endDate)
            If _ingredientID > 0 Then
                cmdAdmin.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If
            lblAdminValue.Text = cmdAdmin.ExecuteScalar().ToString()

        Catch ex As Exception
            MessageBox.Show("Error loading statistics: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' NEW METHOD: Load Total Cost - Calculates overall total from ALL data in inventory_batches table
    Private Sub LoadTotalCost()
        Try
            openConn()

            ' Calculate TOTAL MOVEMENT COST from movement logs (resets when history is cleared)
            Dim sqlOverall As String = "
                SELECT 
                    COALESCE(SUM(ABS(iml.QuantityChanged) * ib.CostPerUnit), 0) AS TotalMovementCost
                FROM inventory_movement_log iml
                INNER JOIN inventory_batches ib ON iml.BatchID = ib.BatchID
                WHERE 1=1
            "

            ' Add ingredient filter if viewing specific ingredient
            If _ingredientID > 0 Then
                sqlOverall &= " AND iml.IngredientID = @ingredientID"
            End If

            Dim cmdOverall As New MySqlCommand(sqlOverall, conn)

            If _ingredientID > 0 Then
                cmdOverall.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If

            Dim totalMovementCost As Decimal = Convert.ToDecimal(cmdOverall.ExecuteScalar())

            ' Display in the label next to close button
            If Me.Controls.Find("lblOverallTotalCost", True).Length > 0 Then
                Dim lblOverallCost As Label = CType(Me.Controls.Find("lblOverallTotalCost", True)(0), Label)
                lblOverallCost.Text = "Total Movement Cost: ₱" & totalMovementCost.ToString("#,##0.00")
                lblOverallCost.Font = New Font("Segoe UI", 14, FontStyle.Bold)
                lblOverallCost.ForeColor = Color.DarkGreen
            End If

            ' Also calculate filtered movement cost for the current view
            Dim sqlFiltered As String = "
                SELECT 
                    COALESCE(SUM(ABS(iml.QuantityChanged) * ib.CostPerUnit), 0) AS FilteredCost
                FROM inventory_movement_log iml
                INNER JOIN inventory_batches ib ON iml.BatchID = ib.BatchID
                WHERE DATE(iml.MovementDate) BETWEEN @startDate AND @endDate
            "

            If _ingredientID > 0 Then
                sqlFiltered &= " AND iml.IngredientID = @ingredientID"
            End If

            If cmbSource.SelectedIndex > 0 Then
                sqlFiltered &= " AND iml.Source = @source"
            End If

            If cmbChangeType.SelectedIndex > 0 Then
                sqlFiltered &= " AND iml.ChangeType = @changeType"
            End If

            Dim cmdFiltered As New MySqlCommand(sqlFiltered, conn)
            cmdFiltered.Parameters.AddWithValue("@startDate", dtpStartDate.Value.Date)
            cmdFiltered.Parameters.AddWithValue("@endDate", dtpEndDate.Value.Date)

            If _ingredientID > 0 Then
                cmdFiltered.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If

            If cmbSource.SelectedIndex > 0 Then
                cmdFiltered.Parameters.AddWithValue("@source", cmbSource.Text)
            End If

            If cmbChangeType.SelectedIndex > 0 Then
                cmdFiltered.Parameters.AddWithValue("@changeType", cmbChangeType.Text)
            End If

            Dim filteredCost As Decimal = Convert.ToDecimal(cmdFiltered.ExecuteScalar())

            ' Display filtered movement cost if label exists
            If Me.Controls.Find("lblFilteredCost", True).Length > 0 Then
                Dim lblFiltered As Label = CType(Me.Controls.Find("lblFilteredCost", True)(0), Label)
                lblFiltered.Text = "Filtered Movement Cost: ₱" & filteredCost.ToString("#,##0.00")
                lblFiltered.Font = New Font("Segoe UI", 11, FontStyle.Bold)
                lblFiltered.ForeColor = Color.DarkBlue
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading total cost: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' Filter event handlers
    Private Sub btnApplyFilters_Click(sender As Object, e As EventArgs) Handles btnApplyFilters.Click
        LoadMovementHistory()
        LoadMovementStatistics()
        LoadTotalCost()
    End Sub

    Private Sub btnResetFilters_Click(sender As Object, e As EventArgs) Handles btnResetFilters.Click
        dtpStartDate.Value = Date.Now.AddMonths(-1)
        dtpEndDate.Value = Date.Now
        cmbSource.SelectedIndex = 0
        cmbChangeType.SelectedIndex = 0
        txtSearch.Clear()
        LoadMovementHistory()
        LoadMovementStatistics()
        LoadTotalCost()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadMovementHistory()
        LoadMovementStatistics()
        LoadTotalCost()
        ScrollToLatestEntry()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ' Auto-search after typing stops (can implement debouncing if needed)
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        Try
            ' Show simple confirmation without date reference
            Dim result As DialogResult = MessageBox.Show(
                "This will permanently delete ALL movement history records." & vbCrLf & vbCrLf &
                "Are you sure you want to clear the history?" & vbCrLf & vbCrLf &
                "This action cannot be undone!",
                "Confirm Clear History",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2)

            If result = DialogResult.No Then Return

            openConn()

            ' Use direct DELETE to ensure ALL types are cleared, not just ADD
            Dim sqlDelete As String = "DELETE FROM inventory_movement_log WHERE MovementDate <= @beforeDate"

            ' Apply ingredient filter if specific ingredient is active
            If _ingredientID > 0 Then
                sqlDelete &= " AND IngredientID = @ingredientID"
            End If

            Dim cmd As New MySqlCommand(sqlDelete, conn)

            ' Use current date/time as the cutoff (clears everything up to NOW)
            cmd.Parameters.AddWithValue("@beforeDate", Date.Now)

            If _ingredientID > 0 Then
                cmd.Parameters.AddWithValue("@ingredientID", _ingredientID)
            End If

            ' ExecuteNonQuery returns the number of rows affected (deleted)
            Dim rowsDeleted As Integer = cmd.ExecuteNonQuery()

            MessageBox.Show(
                "History cleared successfully!" & vbCrLf & vbCrLf &
                "Records deleted: " & rowsDeleted,
                "Clear History Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

            ' Log Activity
            Dim targetLog As String = If(_ingredientID > 0, $"Ingredient ID {_ingredientID}", "All Ingredients")
            ActivityLogger.LogUserActivity("History Cleared", "Inventory", $"Cleared inventory movement history for {targetLog} (Records: {rowsDeleted})", "Admin Panel")

            LoadMovementHistory()
            LoadMovementStatistics()
            LoadTotalCost()

        Catch ex As Exception
            MessageBox.Show("Error clearing history: " & ex.Message,
                          "Clear History Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub lblOverallTotalCost_Click(sender As Object, e As EventArgs) Handles lblOverallTotalCost.Click

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