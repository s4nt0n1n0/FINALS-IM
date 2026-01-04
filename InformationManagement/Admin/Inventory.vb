Imports MySqlConnector

Public Class Inventory
    Private isShowingAlerts As Boolean = False
    Private WithEvents btnInventoryAlerts As Button
    Private _lastSearchText As String = ""
    Private isInitializing As Boolean = True
    Private Sub Inventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Set form to maximized
            Me.WindowState = FormWindowState.Maximized

            ' Apply responsive layout
            ApplyResponsiveLayout()

            ' Initialize and position the alerts button
            InitializeAlertsButton()

            ' Load categories
            LoadCategories()

            ' Load data
            LoadInventorySummary()
            LoadInventoryStatistics()

            ' Restore usage history button
            If btnNotifications IsNot Nothing Then
                btnNotifications.Text = "🔔 View Usage History"
                btnNotifications.Visible = True
            End If

            ' Update the alerts notification button count
            UpdateNotificationBadge()

            InitializeSearchBox()
            isInitializing = False

        Catch ex As Exception
            MessageBox.Show("Error loading form: " & ex.Message,
                          "Load Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    ' Load categories into dropdown
    Private Sub LoadCategories()
        Try
            openConn()

            Dim sql As String = "SELECT CategoryName FROM ingredient_categories ORDER BY CategoryName"
            Dim cmd As New MySqlCommand(sql, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Category.Items.Clear()
            Category.Items.Add("All")

            While reader.Read()
                Category.Items.Add(reader("CategoryName").ToString())
            End While

            reader.Close()
            Category.SelectedIndex = 0

        Catch ex As Exception
            MessageBox.Show("Error loading categories: " & ex.Message,
                          "Database Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Handle form resize
    Private Sub Inventory_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ApplyResponsiveLayout()
    End Sub

    ' Apply responsive layout based on screen size
    Private Sub ApplyResponsiveLayout()
        Try
            Dim formWidth As Integer = Me.ClientSize.Width
            Dim formHeight As Integer = Me.ClientSize.Height

            ' Calculate margins and spacing
            Dim leftMargin As Integer = CInt(formWidth * 0.03) ' 3% margin
            Dim topMargin As Integer = 120
            Dim spacing As Integer = CInt(formWidth * 0.015) ' 1.5% spacing

            ' Position and size for header section
            If Me.Controls.Contains(Splitter1) Then
                Splitter1.Height = 105
            End If

            ' Position summary cards
            Dim cardWidth As Integer = CInt((formWidth - (leftMargin * 2) - spacing) / 2)
            Dim cardHeight As Integer = 170

            ' Total Items card
            If Me.Controls.Contains(RoundedPane21) Then
                RoundedPane21.Location = New Point(leftMargin, topMargin)
                RoundedPane21.Size = New Size(cardWidth, cardHeight)
            End If

            ' Total Value card
            If Me.Controls.Contains(RoundedPane22) Then
                RoundedPane22.Location = New Point(leftMargin + cardWidth + spacing, topMargin)
                RoundedPane22.Size = New Size(cardWidth, cardHeight)
            End If

            ' Search and filter section
            Dim searchTop As Integer = topMargin + cardHeight + 40

            ' Search label
            If Me.Controls.Contains(Label6) Then

            End If



            ' Adjust DataGrid columns
            AdjustGridColumns()

            ' Position both buttons
            If btnNotifications IsNot Nothing AndAlso btnInventoryAlerts IsNot Nothing Then
                Dim btnWidth As Integer = 200
                Dim btnHeight As Integer = 40
                Dim rightMargin As Integer = Me.ClientSize.Width - 30

                ' View Usage History Button
                btnNotifications.Size = New Size(btnWidth, btnHeight)
                btnNotifications.Location = New Point(rightMargin - btnWidth, 10)

                ' Inventory Alerts Button
                btnInventoryAlerts.Size = New Size(btnWidth, btnHeight)
                btnInventoryAlerts.Location = New Point(rightMargin - btnWidth, 10 + btnHeight + 10)
            End If

        Catch ex As Exception
            ' Silent fail during resize
        End Try
    End Sub

    ' Adjust DataGrid column widths
    Private Sub AdjustGridColumns()
        Try
            If InventoryGrid.Columns.Count > 0 AndAlso InventoryGrid.Width > 0 Then
                Dim totalWidth As Integer = InventoryGrid.Width - 40

                If InventoryGrid.Columns.Contains("Item Name") Then
                    InventoryGrid.Columns("Item Name").Width = CInt(totalWidth * 0.18)
                End If

                If InventoryGrid.Columns.Contains("Category") Then
                    InventoryGrid.Columns("Category").Width = CInt(totalWidth * 0.12)
                End If

                If InventoryGrid.Columns.Contains("Total Quantity") Then
                    InventoryGrid.Columns("Total Quantity").Width = CInt(totalWidth * 0.1)
                End If

                If InventoryGrid.Columns.Contains("Unit") Then
                    InventoryGrid.Columns("Unit").Width = CInt(totalWidth * 0.08)
                End If

                If InventoryGrid.Columns.Contains("Status") Then
                    InventoryGrid.Columns("Status").Width = CInt(totalWidth * 0.1)
                End If

                If InventoryGrid.Columns.Contains("Active Batches") Then
                    InventoryGrid.Columns("Active Batches").Width = CInt(totalWidth * 0.1)
                End If

                If InventoryGrid.Columns.Contains("Next Expiration") Then
                    InventoryGrid.Columns("Next Expiration").Width = CInt(totalWidth * 0.12)
                End If

                If InventoryGrid.Columns.Contains("Total Value") Then
                    InventoryGrid.Columns("Total Value").Width = CInt(totalWidth * 0.12)
                End If

                If InventoryGrid.Columns.Contains("ViewBatches") Then
                    InventoryGrid.Columns("ViewBatches").Width = 120
                End If

                If InventoryGrid.Columns.Contains("EditItem") Then
                    InventoryGrid.Columns("EditItem").Width = 80
                End If

                If InventoryGrid.Columns.Contains("DeleteItem") Then
                    InventoryGrid.Columns("DeleteItem").Width = 80
                End If
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    ' Load main inventory grid
    Private Sub LoadInventorySummary()
        Try
            openConn()

            Dim sql As String = "
                SELECT 
                    i.IngredientID AS 'Ingredient ID',
                    i.IngredientName AS 'Item Name',
                    COALESCE(ic.CategoryName, 'Uncategorized') AS 'Category',
                    COALESCE(SUM(ib.StockQuantity), 0) AS 'Total Quantity',
                    i.UnitType AS 'Unit',
                    CASE 
                        WHEN COALESCE(SUM(ib.StockQuantity), 0) = 0 THEN 'Out of Stock'
                        WHEN COALESCE(SUM(ib.StockQuantity), 0) < i.MinStockLevel THEN 'Low Stock'
                        WHEN COALESCE(SUM(ib.StockQuantity), 0) > i.MaxStockLevel THEN 'Overstocked'
                        ELSE 'In Stock'
                    END AS 'Status',
                    COUNT(CASE WHEN ib.BatchStatus = 'Active' THEN 1 END) AS 'Active Batches',
                    MIN(CASE WHEN ib.BatchStatus = 'Active' THEN ib.ExpirationDate END) AS 'Next Expiration',
                    COALESCE(SUM(CASE WHEN ib.BatchStatus = 'Active' THEN ib.StockQuantity * ib.CostPerUnit END), 0) AS 'Total Value',
                    i.MinStockLevel AS 'Min Level',
                    i.MaxStockLevel AS 'Max Level'
                FROM ingredients i
                LEFT JOIN ingredient_categories ic ON i.CategoryID = ic.CategoryID
                LEFT JOIN inventory_batches ib ON i.IngredientID = ib.IngredientID
                WHERE i.IsActive = 1
                GROUP BY i.IngredientID, i.IngredientName, ic.CategoryName, 
                         i.UnitType, i.MinStockLevel, i.MaxStockLevel
                ORDER BY i.IngredientName
            "

            Dim cmd As New MySqlCommand(sql, conn)
            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            ' Set datasource
            InventoryGrid.DataSource = Nothing
            InventoryGrid.Columns.Clear()
            InventoryGrid.DataSource = dt

            ' Hide ID and level columns
            If InventoryGrid.Columns.Contains("Ingredient ID") Then
                InventoryGrid.Columns("Ingredient ID").Visible = False
            End If
            If InventoryGrid.Columns.Contains("Min Level") Then
                InventoryGrid.Columns("Min Level").Visible = False
            End If
            If InventoryGrid.Columns.Contains("Max Level") Then
                InventoryGrid.Columns("Max Level").Visible = False
            End If

            ' Format grid FIRST
            FormatInventoryGrid()

            ' IMPORTANT: Apply color coding AFTER the grid is fully bound and formatted
            ' Use BeginInvoke to ensure DataGridView has finished rendering
            Me.BeginInvoke(New MethodInvoker(Sub()
                                                 ColorCodeStatusColumn()
                                             End Sub))

            ' Update total value card after grid refresh
            UpdateTotalValueCard()

            ' Update notification badge
            UpdateNotificationBadge()

        Catch ex As Exception
            MessageBox.Show("Error loading inventory: " & ex.Message,
                          "Database Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Format the grid
    Private Sub FormatInventoryGrid()
        Try
            With InventoryGrid
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .RowTemplate.Height = 35
                .DefaultCellStyle.Font = New Font("Segoe UI", 9)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250)
                .ReadOnly = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect

                ' Format columns
                If .Columns.Contains("Total Value") Then
                    .Columns("Total Value").DefaultCellStyle.Format = "#,##0.00"
                    .Columns("Total Value").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("Total Value").ReadOnly = True
                End If

                If .Columns.Contains("Total Quantity") Then
                    .Columns("Total Quantity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Columns("Total Quantity").ReadOnly = True
                End If

                If .Columns.Contains("Active Batches") Then
                    .Columns("Active Batches").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Columns("Active Batches").ReadOnly = True
                End If

                If .Columns.Contains("Next Expiration") Then
                    .Columns("Next Expiration").DefaultCellStyle.Format = "MMM dd, yyyy"
                    .Columns("Next Expiration").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Columns("Next Expiration").ReadOnly = True
                End If

                If .Columns.Contains("Status") Then
                    .Columns("Status").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Columns("Status").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    .Columns("Status").ReadOnly = True
                End If

                ' Make all columns read-only except action buttons
                For Each col As DataGridViewColumn In .Columns
                    If col.Name <> "ViewBatches" AndAlso
                       col.Name <> "EditItem" AndAlso
                       col.Name <> "DeleteItem" Then
                        col.ReadOnly = True
                    End If
                Next
            End With

            ' Add View Batches button
            If Not InventoryGrid.Columns.Contains("ViewBatches") Then
                Dim btnView As New DataGridViewButtonColumn()
                btnView.Name = "ViewBatches"
                btnView.HeaderText = "Batches"
                btnView.Text = "View Batches"
                btnView.UseColumnTextForButtonValue = True
                btnView.Width = 120
                btnView.FlatStyle = FlatStyle.Flat
                InventoryGrid.Columns.Add(btnView)
            End If

            ' Add Edit button
            If Not InventoryGrid.Columns.Contains("EditItem") Then
                Dim btnEdit As New DataGridViewButtonColumn()
                btnEdit.Name = "EditItem"
                btnEdit.HeaderText = "Edit"
                btnEdit.Text = "Edit"
                btnEdit.UseColumnTextForButtonValue = True
                btnEdit.Width = 80
                btnEdit.FlatStyle = FlatStyle.Flat
                InventoryGrid.Columns.Add(btnEdit)
            End If

            ' Add Delete button
            If Not InventoryGrid.Columns.Contains("DeleteItem") Then
                Dim btnDelete As New DataGridViewButtonColumn()
                btnDelete.Name = "DeleteItem"
                btnDelete.HeaderText = "Delete"
                btnDelete.Text = "Delete"
                btnDelete.UseColumnTextForButtonValue = True
                btnDelete.Width = 80
                btnDelete.FlatStyle = FlatStyle.Flat
                InventoryGrid.Columns.Add(btnDelete)
            End If

            AdjustGridColumns()

        Catch ex As Exception
            MessageBox.Show("Error formatting grid: " & ex.Message)
        End Try
    End Sub

    ' Color code status
    Private Sub ColorCodeStatusColumn()
        Try
            ' Force the grid to complete any pending layout operations
            InventoryGrid.Update()
            Application.DoEvents()

            For Each row As DataGridViewRow In InventoryGrid.Rows
                If Not row.IsNewRow AndAlso row.Cells("Status").Value IsNot Nothing Then
                    Dim status As String = row.Cells("Status").Value.ToString()

                    Select Case status
                        Case "Out of Stock"
                            row.Cells("Status").Style.BackColor = Color.FromArgb(220, 53, 69)
                            row.Cells("Status").Style.ForeColor = Color.White
                        Case "Low Stock"
                            row.Cells("Status").Style.BackColor = Color.FromArgb(255, 193, 7)
                            row.Cells("Status").Style.ForeColor = Color.Black
                        Case "In Stock"
                            row.Cells("Status").Style.BackColor = Color.FromArgb(40, 167, 69)
                            row.Cells("Status").Style.ForeColor = Color.White
                        Case "Overstocked"
                            row.Cells("Status").Style.BackColor = Color.FromArgb(23, 162, 184)
                            row.Cells("Status").Style.ForeColor = Color.White
                    End Select

                    ' Highlight expiring items
                    If row.Cells("Next Expiration").Value IsNot Nothing AndAlso
                       Not IsDBNull(row.Cells("Next Expiration").Value) Then
                        Try
                            Dim expiryDate As Date = Convert.ToDateTime(row.Cells("Next Expiration").Value)
                            Dim daysLeft As Integer = (expiryDate - Date.Now).Days

                            If daysLeft <= 0 Then
                                row.Cells("Next Expiration").Style.BackColor = Color.FromArgb(139, 0, 0)
                                row.Cells("Next Expiration").Style.ForeColor = Color.White
                                row.Cells("Next Expiration").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                            ElseIf daysLeft <= 3 Then
                                row.Cells("Next Expiration").Style.BackColor = Color.FromArgb(220, 53, 69)
                                row.Cells("Next Expiration").Style.ForeColor = Color.White
                            ElseIf daysLeft <= 7 Then
                                row.Cells("Next Expiration").Style.BackColor = Color.FromArgb(255, 193, 7)
                                row.Cells("Next Expiration").Style.ForeColor = Color.Black
                            End If
                        Catch
                            ' Skip if date conversion fails
                        End Try
                    End If
                End If
            Next

            ' Force the grid to refresh and display the new colors
            InventoryGrid.Refresh()

        Catch ex As Exception
            ' Silent fail but log for debugging
            Debug.WriteLine("Error color coding: " & ex.Message)
        End Try
    End Sub

    ' Update the total value card based on the visible batches in the grid
    Private Sub UpdateTotalValueCard()
        Try
            Dim totalValue As Decimal = 0D

            For Each row As DataGridViewRow In InventoryGrid.Rows
                If row.IsNewRow OrElse Not row.Visible Then
                    Continue For
                End If

                Dim valueCell = row.Cells("Total Value").Value

                If valueCell IsNot Nothing AndAlso Not IsDBNull(valueCell) Then
                    totalValue += Convert.ToDecimal(valueCell)
                End If
            Next

            If Me.Controls.Contains(Label11) Then
                Label11.Text = "₱" & totalValue.ToString("#,##0.00")
            End If
        Catch ex As Exception
            ' Silent fail for visual total update
        End Try
    End Sub

    ' Load statistics in the top panels
    Private Sub LoadInventoryStatistics()
        Try
            openConn()

            ' Total Items - Count only ingredients that have active inventory batches
            Dim sqlTotalItems As String = "
            SELECT COUNT(DISTINCT ib.IngredientID) 
            FROM inventory_batches ib
            INNER JOIN ingredients i ON ib.IngredientID = i.IngredientID
            WHERE ib.BatchStatus = 'Active' 
              AND i.IsActive = 1
        "
            Dim cmdTotal As New MySqlCommand(sqlTotalItems, conn)
            Dim totalItems As Integer = Convert.ToInt32(cmdTotal.ExecuteScalar())
            Label5.Text = totalItems.ToString()

            ' Total Value - Sum from active batches only
            Dim sqlTotalValue As String = "
            SELECT COALESCE(SUM(ib.StockQuantity * ib.CostPerUnit), 0)
            FROM inventory_batches ib
            INNER JOIN ingredients i ON ib.IngredientID = i.IngredientID
            WHERE ib.BatchStatus = 'Active'
              AND i.IsActive = 1
        "
            Dim cmdValue As New MySqlCommand(sqlTotalValue, conn)
            Dim totalValue As Decimal = Convert.ToDecimal(cmdValue.ExecuteScalar())
            Label11.Text = "₱" & totalValue.ToString("#,##0.00")

        Catch ex As Exception
            MessageBox.Show("Error loading statistics: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' Handle View Batches button click
    Private Sub InventoryGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles InventoryGrid.CellContentClick
        Try
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                Dim columnName As String = InventoryGrid.Columns(e.ColumnIndex).Name

                If columnName = "ViewBatches" Then
                    Dim ingredientID As Integer = Convert.ToInt32(InventoryGrid.Rows(e.RowIndex).Cells("Ingredient ID").Value)
                    Dim ingredientName As String = InventoryGrid.Rows(e.RowIndex).Cells("Item Name").Value.ToString()

                    ' Open Batch Management form
                    Dim batchForm As New BatchManagement(ingredientID, ingredientName)
                    batchForm.StartPosition = FormStartPosition.CenterScreen
                    batchForm.ShowDialog()

                    ' Refresh after closing
                    LoadInventorySummary()
                    LoadInventoryStatistics()

                ElseIf columnName = "EditItem" Then
                    HandleEditItem(e.RowIndex)

                ElseIf columnName = "DeleteItem" Then
                    HandleDeleteItem(e.RowIndex)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error opening batch details: " & ex.Message,
                          "Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    ' Handle editing an inventory item (unit and stock levels)
    Private Sub HandleEditItem(rowIndex As Integer)
        Try
            Dim row As DataGridViewRow = InventoryGrid.Rows(rowIndex)
            Dim ingredientID As Integer = Convert.ToInt32(row.Cells("Ingredient ID").Value)
            Dim editForm As New AddNewItems(ingredientID)
            editForm.StartPosition = FormStartPosition.CenterScreen

            If editForm.ShowDialog() = DialogResult.OK Then
                LoadInventorySummary()
                LoadInventoryStatistics()
            End If
        Catch ex As Exception
            MessageBox.Show("Error editing item: " & ex.Message,
                          "Edit Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    ' Handle deleting (archiving) an inventory item
    Private Sub HandleDeleteItem(rowIndex As Integer)
        Try
            Dim row As DataGridViewRow = InventoryGrid.Rows(rowIndex)
            Dim ingredientID As Integer = Convert.ToInt32(row.Cells("Ingredient ID").Value)
            Dim ingredientName As String = row.Cells("Item Name").Value.ToString()

            Dim result As DialogResult = MessageBox.Show(
                "Are you sure you want to delete this item?" & vbCrLf & vbCrLf &
                "Item: " & ingredientName & vbCrLf &
                "This will remove it from the active inventory list." & vbCrLf &
                "Existing batch history will be kept.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If result <> DialogResult.Yes Then
                Return
            End If

            openConn()
            Dim transaction As MySqlTransaction = conn.BeginTransaction()

            Try
                ' Mark ingredient as inactive
                Dim sqlIngredient As String = "
                    UPDATE ingredients
                    SET IsActive = 0
                    WHERE IngredientID = @id
                "
                Dim cmdIngredient As New MySqlCommand(sqlIngredient, conn, transaction)
                cmdIngredient.Parameters.AddWithValue("@id", ingredientID)
                cmdIngredient.ExecuteNonQuery()

                ' Optionally mark active batches as discarded so they no longer count in summaries
                Dim sqlBatches As String = "
                    UPDATE inventory_batches
                    SET BatchStatus = 'Discarded'
                    WHERE IngredientID = @id AND BatchStatus = 'Active'
                "
                Dim cmdBatches As New MySqlCommand(sqlBatches, conn, transaction)
                cmdBatches.Parameters.AddWithValue("@id", ingredientID)
                cmdBatches.ExecuteNonQuery()

                transaction.Commit()

                ' Log the deletion activity
                ActivityLogger.LogUserActivity("Inventory Item Deleted", "Inventory", $"Moved {ingredientName} (ID: {ingredientID}) to Archive", "Admin Panel")

                MessageBox.Show($"{ingredientName} has been moved to archive.", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)

                LoadInventorySummary()
                LoadInventoryStatistics()

            Catch ex As Exception
                transaction.Rollback()
                Throw
            End Try

        Catch ex As Exception
            MessageBox.Show("Error deleting item: " & ex.Message,
                          "Delete Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' =======================================================
    ' SEARCH FUNCTIONALITY
    ' =======================================================
    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitializing Then Return

        Try
            If InventoryGrid.DataSource IsNot Nothing Then
                Dim dt As DataTable = DirectCast(InventoryGrid.DataSource, DataTable)
                Dim searchText As String = TextBoxSearch.Text.Trim()

                If searchText = "Search inventory..." Then searchText = ""

                ' Only filter if the actual search criteria changed
                If searchText = _lastSearchText Then Return
                _lastSearchText = searchText

                If String.IsNullOrEmpty(searchText) Then
                    dt.DefaultView.RowFilter = ""
                Else
                    dt.DefaultView.RowFilter = String.Format(
                        "[Item Name] LIKE '%{0}%' OR [Category] LIKE '%{0}%'",
                        searchText.Replace("'", "''"))
                End If

                ' Re-apply colors after filtering
                Me.BeginInvoke(New MethodInvoker(Sub()
                                                     ColorCodeStatusColumn()
                                                 End Sub))
                UpdateTotalValueCard()
            End If
        Catch ex As Exception
            ' Silent fail for search
        End Try
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search inventory..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42)
            txtSearch.BorderColor = Color.FromArgb(99, 102, 241)
        End If
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search inventory..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
            txtSearch.BorderColor = Color.FromArgb(226, 232, 240)
        End If
    End Sub

    ' =======================================================
    ' INITIALIZE SEARCH BOX
    ' =======================================================
    Private Sub InitializeSearchBox()
        TextBoxSearch.Text = "Search inventory..."
        TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
    End Sub
    ' Category filter
    Private Sub Category_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Category.SelectedIndexChanged
        Try
            If InventoryGrid.DataSource IsNot Nothing Then
                Dim dt As DataTable = DirectCast(InventoryGrid.DataSource, DataTable)
                Dim selectedCategory As String = Category.Text

                If String.IsNullOrEmpty(selectedCategory) OrElse selectedCategory = "All" Then
                    dt.DefaultView.RowFilter = ""
                Else
                    dt.DefaultView.RowFilter = String.Format(
                        "[Category] = '{0}'",
                        selectedCategory.Replace("'", "''"))
                End If

                ' Re-apply colors after filtering
                Me.BeginInvoke(New MethodInvoker(Sub()
                                                     ColorCodeStatusColumn()
                                                 End Sub))
                UpdateTotalValueCard()
            End If
        Catch ex As Exception
            ' Silent fail for filter
        End Try
    End Sub

    ' Add new batch
    Private Sub AddItem_Click(sender As Object, e As EventArgs)
        Try
            Dim addForm As New AddNewItems()
            addForm.StartPosition = FormStartPosition.CenterScreen

            If addForm.ShowDialog() = DialogResult.OK Then
                LoadInventorySummary()
                LoadInventoryStatistics()
            End If
        Catch ex As Exception
            MessageBox.Show("Error opening add form: " & ex.Message,
                          "Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    ' Restore original functionality: View Usage History
    Private Sub btnNotifications_Click(sender As Object, e As EventArgs) Handles btnNotifications.Click
        Try
            Dim usageForm As New ProductIngredientUsageHistory()
            usageForm.StartPosition = FormStartPosition.CenterScreen
            usageForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error opening usage history: " & ex.Message,
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error)
        End Try
    End Sub

    ' New button handler: Toggle Inventory Alerts Filter
    Private Sub btnInventoryAlerts_Click(sender As Object, e As EventArgs) Handles btnInventoryAlerts.Click
        Try
            If InventoryGrid.DataSource IsNot Nothing Then
                Dim dt As DataTable = DirectCast(InventoryGrid.DataSource, DataTable)

                If Not isShowingAlerts Then
                    ' Filter for Low Stock or Out of Stock
                    dt.DefaultView.RowFilter = "[Status] = 'Low Stock' OR [Status] = 'Out of Stock'"
                    btnInventoryAlerts.Text = "🚨 Show All Items"
                    btnInventoryAlerts.BackColor = Color.FromArgb(45, 45, 45) ' Dark neutral
                    isShowingAlerts = True
                Else
                    ' Clear filter
                    dt.DefaultView.RowFilter = ""
                    UpdateNotificationBadge()
                    isShowingAlerts = False
                End If

                ' Re-apply colors after filtering
                Me.BeginInvoke(New MethodInvoker(Sub()
                                                     ColorCodeStatusColumn()
                                                 End Sub))
                UpdateTotalValueCard()
            End If
        Catch ex As Exception
            MessageBox.Show("Error filtering alerts: " & ex.Message,
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeAlertsButton()
        If btnInventoryAlerts Is Nothing Then
            btnInventoryAlerts = New Button()
            btnInventoryAlerts.Name = "btnInventoryAlerts"
            btnInventoryAlerts.FlatStyle = FlatStyle.Flat
            btnInventoryAlerts.FlatAppearance.BorderSize = 0
            btnInventoryAlerts.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            btnInventoryAlerts.ForeColor = Color.White
            btnInventoryAlerts.Cursor = Cursors.Hand
            btnInventoryAlerts.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            Me.Controls.Add(btnInventoryAlerts)
            btnInventoryAlerts.BringToFront()
        End If
    End Sub

    Private Sub UpdateNotificationBadge()
        Try
            openConn()

            ' 1. Inventory Alerts (Low Stock)
            Dim sqlAlerts As String = "
                SELECT COUNT(*) 
                FROM (
                    SELECT 
                        i.IngredientID,
                        COALESCE(SUM(ib.StockQuantity), 0) AS TotalStock,
                        i.MinStockLevel
                    FROM ingredients i
                    LEFT JOIN inventory_batches ib ON i.IngredientID = ib.IngredientID AND ib.BatchStatus = 'Active'
                    WHERE i.IsActive = 1
                    GROUP BY i.IngredientID, i.MinStockLevel
                    HAVING TotalStock < i.MinStockLevel
                ) AS LowStockItems
            "
            Dim cmdAlerts As New MySqlCommand(sqlAlerts, conn)
            Dim alertCount As Integer = Convert.ToInt32(cmdAlerts.ExecuteScalar())

            If btnInventoryAlerts IsNot Nothing Then
                If alertCount > 0 Then
                    btnInventoryAlerts.Text = $"🚨 {alertCount} Alerts"
                    btnInventoryAlerts.BackColor = Color.FromArgb(220, 53, 69) ' Crimson/Red
                Else
                    btnInventoryAlerts.Text = "✅ No Alerts"
                    btnInventoryAlerts.BackColor = Color.FromArgb(40, 167, 69) ' Green
                End If
                btnInventoryAlerts.Visible = True
            End If

            ' 2. Usage History Notifications (Recent Deductions)
            ' Count "Rows" (Distinct Orders/Reservations) not raw log entries
            Dim sqlUsage As String = "
                SELECT COUNT(DISTINCT 
                    CASE 
                        WHEN OrderID IS NOT NULL THEN CONCAT('ORDER-', OrderID)
                        WHEN ReservationID IS NOT NULL THEN CONCAT('RES-', ReservationID)
                        ELSE 'MANUAL'
                    END
                ) 
                FROM inventory_movement_log 
                WHERE ChangeType = 'DEDUCT' 
                AND MovementDate >= DATE_SUB(NOW(), INTERVAL 24 HOUR)
            "
            Dim cmdUsage As New MySqlCommand(sqlUsage, conn)
            Dim usageCount As Integer = Convert.ToInt32(cmdUsage.ExecuteScalar())

            If btnNotifications IsNot Nothing Then
                If usageCount > 0 Then
                    btnNotifications.Text = $"🔔 View Usage History ({usageCount})"
                    btnNotifications.BackColor = Color.FromArgb(111, 66, 193) ' Keep original purple
                Else
                    btnNotifications.Text = "🔔 View Usage History"
                    btnNotifications.BackColor = Color.FromArgb(111, 66, 193)
                End If
                btnNotifications.Visible = True
            End If

        Catch ex As Exception
            ' Silent fail for background badge update
            Debug.WriteLine("Error updating badges: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' These methods are kept but will not be called since button is hidden
    Private Function GetRecentDeductionCount() As Integer
        Try
            openConn()

            Dim sql As String = "
            SELECT COUNT(*) 
            FROM inventory_movement_log 
            WHERE ChangeType = 'DEDUCT' 
            AND MovementDate >= DATE_SUB(NOW(), INTERVAL 24 HOUR)
        "

            Dim cmd As New MySqlCommand(sql, conn)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            Return count
        Catch ex As Exception
            Return 0
        Finally
            closeConn()
        End Try
    End Function

    Private Sub UpdateNotificationButton()
        ' Method kept for compatibility but does nothing since button is hidden
    End Sub

    ' Public refresh method
    Public Sub RefreshInventory()
        LoadInventorySummary()
        LoadInventoryStatistics()
        UpdateNotificationBadge()
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub
End Class