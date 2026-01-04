Imports MySqlConnector
Imports System.IO
Imports System.Net

Public Class MenuItems

    ' =============================================================
    ' CONFIGURATION: Set your XAMPP htdocs path and localhost URL
    ' =============================================================
    Private Const WEB_BASE_URL As String = "http://localhost/TrialWeb/TrialWorkIM/Tabeya/"

    ' Track if buttons already added
    Dim ButtonsAdded As Boolean = False

    Private Sub MenuItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCategories()
        LoadMenuItems()
        InitializeSearchBox()
    End Sub

    ' =======================================================
    ' LOAD CATEGORIES
    ' =======================================================
    Private Sub LoadCategories()
        Category.Items.Clear()
        Category.Items.Add("All Categories")
        Category.Items.AddRange(New String() {
            "SPAGHETTI MEAL",
            "DESSERT",
            "DRINKS & BEVERAGES",
            "PLATTER",
            "RICE MEAL",
            "RICE",
            "Bilao",
            "Snacks"
        })
        Category.SelectedIndex = 0
    End Sub

    ' =======================================================
    ' INITIALIZE SEARCH BOX
    ' =======================================================
    Private Sub InitializeSearchBox()
        txtSearch.ForeColor = Color.Gray
        txtSearch.Text = "Search menu items..."
    End Sub

    ' =======================================================
    ' LOAD DATA - WITH IMAGE PATH PRESERVED
    ' =======================================================
    Private Sub LoadMenuItems(Optional searchTerm As String = "", Optional categoryFilter As String = "All Categories")

        Dim query As String = "
        SELECT 
            Image,
            ProductID, ProductName, Category, Description, Price,
            Availability, ServingSize, DateAdded, LastUpdated,
            ProductCode, OrderCount, PrepTime, PopularityTag, MealTime
        FROM products
        WHERE 1=1
    "

        ' Add search filter
        If Not String.IsNullOrWhiteSpace(searchTerm) AndAlso searchTerm <> "Search menu items..." Then
            query &= " AND (ProductName LIKE @search OR ProductCode LIKE @search OR Description LIKE @search)"
        End If

        ' Add category filter
        If categoryFilter <> "All Categories" Then
            query &= " AND Category = @category"
        End If

        ' Order by ProductID ascending (lowest to highest)
        query &= " ORDER BY ProductID ASC"

        Try
            openConn()

            Dim cmd As New MySqlCommand(query, conn)

            If Not String.IsNullOrWhiteSpace(searchTerm) AndAlso searchTerm <> "Search menu items..." Then
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")
            End If

            ' FIXED: Map "Bilao" display label to "NOODLES & PASTA" database value
            If categoryFilter <> "All Categories" Then
                Dim actualCategory As String = categoryFilter

                ' Map display labels to actual database category values
                If categoryFilter = "Bilao" Then
                    actualCategory = "NOODLES & PASTA"
                End If

                cmd.Parameters.AddWithValue("@category", actualCategory)
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            ' ---------------------------------------------------
            ' LOAD IMAGES FROM URL PATHS
            ' ---------------------------------------------------
            If dt IsNot Nothing AndAlso dt.Columns.Contains("Image") Then
                ' Rename Image column to ImagePath temporarily
                Dim imgColSource = dt.Columns("Image")
                If imgColSource IsNot Nothing Then
                    imgColSource.ColumnName = "ImagePath"
                End If

                ' Add new Image column for actual Image objects if it doesn't exist
                If Not dt.Columns.Contains("ImageDisplay") Then
                    dt.Columns.Add("ImageDisplay", GetType(Image))
                End If

                For Each row As DataRow In dt.Rows
                    ' Get the image path from database
                    If row.IsNull("ImagePath") OrElse String.IsNullOrEmpty(row("ImagePath").ToString()) Then
                        row("ImageDisplay") = Nothing
                        Continue For
                    End If

                    Try
                        Dim imagePath As String = row("ImagePath").ToString()
                        Dim imageUrl As String = ConvertToWebUrl(imagePath)

                        ' Load image from URL
                        Using webClient As New WebClient()
                            Dim imageBytes() As Byte = webClient.DownloadData(imageUrl)
                            If imageBytes IsNot Nothing AndAlso imageBytes.Length > 0 Then
                                Using ms As New MemoryStream(imageBytes)
                                    row("ImageDisplay") = Image.FromStream(ms)
                                End Using
                            Else
                                row("ImageDisplay") = Nothing
                            End If
                        End Using
                    Catch ex As Exception
                        ' If image fails to load, set to Nothing
                        row("ImageDisplay") = Nothing
                        Console.WriteLine($"Failed to load image: {ex.Message}")
                    End Try
                Next
            End If

            If DataGridMenu IsNot Nothing Then
                DataGridMenu.AutoGenerateColumns = False
                DataGridMenu.Columns.Clear()
                DataGridMenu.DataSource = dt

                ' Add columns manually
                AddDataGridColumns()
            End If

            ' BUTTONS ADDED ONLY ONCE
            If Not ButtonsAdded Then
                AddActionButtons()
                ButtonsAdded = True
            End If

            ' FORMAT IMAGE COLUMN
            If DataGridMenu.Columns.Contains("ImageDisplay") Then
                Dim imgCol As DataGridViewImageColumn =
                CType(DataGridMenu.Columns("ImageDisplay"), DataGridViewImageColumn)
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
                imgCol.Width = 120
                imgCol.DisplayIndex = 0
            End If

            If DataGridMenu.Columns.Contains("ImagePath") Then
                DataGridMenu.Columns("ImagePath").Visible = False
            End If

            ' Format other columns
            FormatColumns()

            ' Column Header Styling
            With DataGridMenu
                .EnableHeadersVisualStyles = False
                .RowHeadersVisible = False ' Hide the row selector column
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing ' Prevent auto-sizing overriding height
                .ColumnHeadersHeight = 45
                .BorderStyle = BorderStyle.None
                
                .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 38, 50)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(26, 38, 50)
                .ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(26, 38, 50)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                
                ' Default Cell Style
                .DefaultCellStyle.BackColor = SystemColors.Window
                .DefaultCellStyle.Font = New Font("Segoe UI", 8.25F)
                .DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64)
                .DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
                .DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With




        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            conn.Close()

        End Try

    End Sub

    ' =======================================================
    ' ADD DATAGRID COLUMNS
    ' =======================================================
    Private Sub AddDataGridColumns()
        With DataGridMenu
            ' Image Column
            Dim imgCol As New DataGridViewImageColumn()
            imgCol.Name = "ImageDisplay"
            imgCol.HeaderText = "Image"
            imgCol.DataPropertyName = "ImageDisplay"
            imgCol.Width = 120
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
            .Columns.Add(imgCol)

            ' Hidden ImagePath Column (IMPORTANT: Keep this for fullscreen view)
            Dim pathCol As New DataGridViewTextBoxColumn()
            pathCol.Name = "ImagePath"
            pathCol.HeaderText = "ImagePath"
            pathCol.DataPropertyName = "ImagePath"
            pathCol.Visible = False
            .Columns.Add(pathCol)

            ' ProductID
            Dim idCol As New DataGridViewTextBoxColumn()
            idCol.Name = "ProductID"
            idCol.HeaderText = "ID"
            idCol.DataPropertyName = "ProductID"
            idCol.Width = 80
            .Columns.Add(idCol)

            ' ProductName
            Dim nameCol As New DataGridViewTextBoxColumn()
            nameCol.Name = "ProductName"
            nameCol.HeaderText = "Product Name"
            nameCol.DataPropertyName = "ProductName"
            nameCol.Width = 200
            .Columns.Add(nameCol)

            ' Category
            Dim catCol As New DataGridViewTextBoxColumn()
            catCol.Name = "Category"
            catCol.HeaderText = "Category"
            catCol.DataPropertyName = "Category"
            catCol.Width = 120
            .Columns.Add(catCol)

            ' Description
            Dim descCol As New DataGridViewTextBoxColumn()
            descCol.Name = "Description"
            descCol.HeaderText = "Description"
            descCol.DataPropertyName = "Description"
            descCol.Width = 250
            .Columns.Add(descCol)

            ' Price
            Dim priceCol As New DataGridViewTextBoxColumn()
            priceCol.Name = "Price"
            priceCol.HeaderText = "Price"
            priceCol.DataPropertyName = "Price"
            priceCol.Width = 100
            priceCol.DefaultCellStyle.Format = "₱#,##0.00"
            .Columns.Add(priceCol)

            ' Availability
            Dim availCol As New DataGridViewTextBoxColumn()
            availCol.Name = "Availability"
            availCol.HeaderText = "Availability"
            availCol.DataPropertyName = "Availability"
            availCol.Width = 100
            .Columns.Add(availCol)

            ' ServingSize
            Dim sizeCol As New DataGridViewTextBoxColumn()
            sizeCol.Name = "ServingSize"
            sizeCol.HeaderText = "Serving Size"
            sizeCol.DataPropertyName = "ServingSize"
            sizeCol.Width = 100
            .Columns.Add(sizeCol)

            ' PrepTime
            Dim prepCol As New DataGridViewTextBoxColumn()
            prepCol.Name = "PrepTime"
            prepCol.HeaderText = "Prep Time"
            prepCol.DataPropertyName = "PrepTime"
            prepCol.Width = 100
            .Columns.Add(prepCol)

            ' PopularityTag
            Dim popCol As New DataGridViewTextBoxColumn()
            popCol.Name = "PopularityTag"
            popCol.HeaderText = "Popularity"
            popCol.DataPropertyName = "PopularityTag"
            popCol.Width = 120
            .Columns.Add(popCol)

            ' MealTime
            Dim mealCol As New DataGridViewTextBoxColumn()
            mealCol.Name = "MealTime"
            mealCol.HeaderText = "Meal Time"
            mealCol.DataPropertyName = "MealTime"
            mealCol.Width = 100
            .Columns.Add(mealCol)

            ' OrderCount
            Dim orderCol As New DataGridViewTextBoxColumn()
            orderCol.Name = "OrderCount"
            orderCol.HeaderText = "Orders"
            orderCol.DataPropertyName = "OrderCount"
            orderCol.Width = 90
            .Columns.Add(orderCol)

            ' ProductCode
            Dim codeCol As New DataGridViewTextBoxColumn()
            codeCol.Name = "ProductCode"
            codeCol.HeaderText = "Product Code"
            codeCol.DataPropertyName = "ProductCode"
            codeCol.Width = 120
            .Columns.Add(codeCol)

            ' DateAdded
            Dim dateCol As New DataGridViewTextBoxColumn()
            dateCol.Name = "DateAdded"
            dateCol.HeaderText = "Date Added"
            dateCol.DataPropertyName = "DateAdded"
            dateCol.Width = 150
            .Columns.Add(dateCol)

            ' LastUpdated
            Dim updateCol As New DataGridViewTextBoxColumn()
            updateCol.Name = "LastUpdated"
            updateCol.HeaderText = "Last Updated"
            updateCol.DataPropertyName = "LastUpdated"
            updateCol.Width = 150
            .Columns.Add(updateCol)
        End With
    End Sub

    ' =======================================================
    ' FORMAT COLUMNS
    ' =======================================================
    Private Sub FormatColumns()
        With DataGridMenu
            If .Columns.Contains("ProductID") Then .Columns("ProductID").Width = 80
            If .Columns.Contains("ProductName") Then .Columns("ProductName").Width = 200
            If .Columns.Contains("Category") Then .Columns("Category").Width = 120
            If .Columns.Contains("Description") Then .Columns("Description").Width = 250
            If .Columns.Contains("Price") Then
                .Columns("Price").Width = 100
                .Columns("Price").DefaultCellStyle.Format = "₱#,##0.00"
            End If
            If .Columns.Contains("Availability") Then .Columns("Availability").Width = 100
            If .Columns.Contains("ServingSize") Then .Columns("ServingSize").Width = 100
            If .Columns.Contains("PrepTime") Then .Columns("PrepTime").Width = 100
            If .Columns.Contains("PopularityTag") Then .Columns("PopularityTag").Width = 120
            If .Columns.Contains("MealTime") Then .Columns("MealTime").Width = 100
            If .Columns.Contains("OrderCount") Then .Columns("OrderCount").Width = 90
            If .Columns.Contains("ProductCode") Then .Columns("ProductCode").Width = 120
        End With
    End Sub

    ' =======================================================
    ' ADD ACTION BUTTONS (VIEW IMAGE, EDIT, DELETE)
    ' =======================================================
    Private Sub AddActionButtons()

        ' VIEW IMAGE BUTTON 🖼️
        Dim viewImageBtn As New DataGridViewButtonColumn()
        viewImageBtn.HeaderText = "Image"
        viewImageBtn.Text = "🖼️ View"
        viewImageBtn.Name = "ViewImageButton"
        viewImageBtn.UseColumnTextForButtonValue = True
        viewImageBtn.Width = 100
        DataGridMenu.Columns.Add(viewImageBtn)

        ' EDIT BUTTON ✏️
        Dim editBtn As New DataGridViewButtonColumn()
        editBtn.HeaderText = "Edit"
        editBtn.Text = "✏️ Edit"
        editBtn.Name = "EditButton"
        editBtn.UseColumnTextForButtonValue = True
        editBtn.Width = 80
        DataGridMenu.Columns.Add(editBtn)

        ' DELETE BUTTON 🗑️
        Dim deleteBtn As New DataGridViewButtonColumn()
        deleteBtn.HeaderText = "Delete"
        deleteBtn.Text = "🗑️ Delete"
        deleteBtn.Name = "DeleteButton"
        deleteBtn.UseColumnTextForButtonValue = True
        deleteBtn.Width = 90
        DataGridMenu.Columns.Add(deleteBtn)

    End Sub
    ' =======================================================
    ' ✨ NEW: HANDLE IMAGE CLICK IN DATAGRIDVIEW
    ' =======================================================
    Private Sub DataGridMenu_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridMenu.CellClick
        ' Check if clicked on the Image column
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            If DataGridMenu.Columns(e.ColumnIndex).Name = "ImageDisplay" Then
                Dim imagePath As String = ""
                Dim productName As String = ""

                ' Get the image path
                If DataGridMenu.Columns.Contains("ImagePath") AndAlso
               DataGridMenu.Rows(e.RowIndex).Cells("ImagePath").Value IsNot Nothing Then
                    imagePath = DataGridMenu.Rows(e.RowIndex).Cells("ImagePath").Value.ToString()
                End If

                ' Get the product name
                If DataGridMenu.Columns.Contains("ProductName") AndAlso
               DataGridMenu.Rows(e.RowIndex).Cells("ProductName").Value IsNot Nothing Then
                    productName = DataGridMenu.Rows(e.RowIndex).Cells("ProductName").Value.ToString()
                End If

                ' Show fullscreen if image exists
                If Not String.IsNullOrEmpty(imagePath) Then
                    ShowProductImageFullscreen(imagePath, productName)
                Else
                    MessageBox.Show("No image available for this product.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    ' =======================================================
    ' BUTTON HANDLERS
    ' =======================================================
    Private Sub DataGridMenu_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridMenu.CellContentClick

        If e.RowIndex < 0 Then Exit Sub

        Dim id As Integer = DataGridMenu.Rows(e.RowIndex).Cells("ProductID").Value
        Dim productName As String = DataGridMenu.Rows(e.RowIndex).Cells("ProductName").Value.ToString()

        ' ========================================
        ' 🖼️ VIEW IMAGE BUTTON - FULLSCREEN VIEWER
        ' ========================================
        If DataGridMenu.Columns(e.ColumnIndex).Name = "ViewImageButton" Then
            Dim imagePath As String = ""
            If DataGridMenu.Columns.Contains("ImagePath") AndAlso
               DataGridMenu.Rows(e.RowIndex).Cells("ImagePath").Value IsNot Nothing Then
                imagePath = DataGridMenu.Rows(e.RowIndex).Cells("ImagePath").Value.ToString()
            End If

            If String.IsNullOrEmpty(imagePath) Then
                MessageBox.Show("No image available for this product.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ShowProductImageFullscreen(imagePath, productName)
            End If
            Exit Sub
        End If

        ' ========================================
        ' ✏️ EDIT BUTTON - LINK TO FormEditMenu.vb
        ' ========================================
        If DataGridMenu.Columns(e.ColumnIndex).Name = "EditButton" Then
            Try
                ' Create instance of FormEditMenu
                Dim editForm As New FormEditMenu()

                ' Load the product data into the edit form
                editForm.LoadProductData(id)

                ' Show the form as dialog
                editForm.StartPosition = FormStartPosition.CenterScreen

                If editForm.ShowDialog() = DialogResult.OK Then
                    ' Refresh the data after successful edit
                    LoadMenuItems(txtSearch.Text, Category.Text)
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Catch ex As Exception
                MessageBox.Show("Error opening edit form: " & ex.Message & vbCrLf & vbCrLf &
                              "Please ensure FormEditMenu.vb exists in your project.",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Exit Sub
        End If

        ' ========================================
        ' 🗑️ DELETE BUTTON - WITH CONFIRMATION
        ' ========================================
        If DataGridMenu.Columns(e.ColumnIndex).Name = "DeleteButton" Then
            DeleteProductWithConfirmation(id, productName)
            Exit Sub
        End If

    End Sub

    ' =============================================================
    ' 🖼️ SHOW PRODUCT IMAGE IN FULLSCREEN MODE
    ' =============================================================
    Private Sub ShowProductImageFullscreen(imagePath As String, productName As String)
        Try
            ' Create fullscreen form
            Dim imageForm As New Form()
            imageForm.Text = "Product Image - " & productName
            imageForm.WindowState = FormWindowState.Maximized
            imageForm.BackColor = Color.Black
            imageForm.FormBorderStyle = FormBorderStyle.None
            imageForm.StartPosition = FormStartPosition.CenterScreen
            imageForm.KeyPreview = True

            ' Create PictureBox to display image
            Dim pictureBox As New PictureBox()
            pictureBox.Dock = DockStyle.Fill
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom
            pictureBox.BackColor = Color.Black

            ' Create panel for controls at the top
            Dim controlPanel As New Panel()
            controlPanel.Dock = DockStyle.Top
            controlPanel.Height = 60
            controlPanel.BackColor = Color.FromArgb(220, 30, 30, 30)

            ' Create close button (✕ Close)
            Dim btnClose As New Button()
            btnClose.Text = "✕ Close (ESC)"
            btnClose.Location = New Point(15, 15)
            btnClose.Size = New Size(140, 30)
            btnClose.BackColor = Color.FromArgb(220, 53, 69)
            btnClose.ForeColor = Color.White
            btnClose.FlatStyle = FlatStyle.Flat
            btnClose.FlatAppearance.BorderSize = 0
            btnClose.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            btnClose.Cursor = Cursors.Hand
            AddHandler btnClose.Click, Sub() imageForm.Close()

            ' Create label for product name
            Dim lblProductName As New Label()
            lblProductName.Text = "📸 " & productName
            lblProductName.Location = New Point(170, 20)
            lblProductName.AutoSize = True
            lblProductName.ForeColor = Color.White
            lblProductName.Font = New Font("Segoe UI", 12, FontStyle.Bold)

            ' Create instruction label
            Dim lblInstruction As New Label()
            lblInstruction.Text = "Press ESC or click Close to exit"
            lblInstruction.Location = New Point(imageForm.Width - 300, 22)
            lblInstruction.AutoSize = True
            lblInstruction.ForeColor = Color.LightGray
            lblInstruction.Font = New Font("Segoe UI", 9, FontStyle.Italic)
            lblInstruction.Anchor = AnchorStyles.Top Or AnchorStyles.Right

            controlPanel.Controls.Add(btnClose)
            controlPanel.Controls.Add(lblProductName)
            controlPanel.Controls.Add(lblInstruction)

            ' Add controls to form
            imageForm.Controls.Add(pictureBox)
            imageForm.Controls.Add(controlPanel)

            ' Handle ESC key to close
            AddHandler imageForm.KeyDown, Sub(s, e)
                                              If e.KeyCode = Keys.Escape Then
                                                  imageForm.Close()
                                              End If
                                          End Sub

            ' Handle click on picturebox to close
            AddHandler pictureBox.Click, Sub() imageForm.Close()

            ' Convert path to URL
            Dim finalUrl As String = ConvertToWebUrl(imagePath)

            ' Load image from URL
            Try
                Dim webClient As New WebClient()
                Dim imageBytes() As Byte = webClient.DownloadData(finalUrl)
                Using ms As New MemoryStream(imageBytes)
                    pictureBox.Image = Image.FromStream(ms)
                End Using
            Catch ex As Exception
                MessageBox.Show("❌ Error loading image from server" & vbCrLf & vbCrLf &
                              "URL: " & finalUrl & vbCrLf & vbCrLf &
                              "Error: " & ex.Message & vbCrLf & vbCrLf &
                              "Please ensure:" & vbCrLf &
                              "✓ XAMPP Apache is running" & vbCrLf &
                              "✓ The image file exists in your htdocs folder" & vbCrLf &
                              "✓ Path: " & imagePath,
                              "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                imageForm.Close()
                Return
            End Try

            ' Show the form
            imageForm.ShowDialog()

            ' Dispose image after closing
            If pictureBox.Image IsNot Nothing Then
                pictureBox.Image.Dispose()
            End If

        Catch ex As Exception
            MessageBox.Show("Error displaying product image: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =============================================================
    ' CONVERT FILE PATH TO WEB URL
    ' =============================================================
    Private Function ConvertToWebUrl(imagePath As String) As String
        ' If already a URL, return as-is
        If imagePath.StartsWith("http://") OrElse imagePath.StartsWith("https://") Then
            Return imagePath
        End If

        ' If path contains full system path with htdocs
        If imagePath.Contains(":\") AndAlso imagePath.ToLower().Contains("htdocs") Then
            Dim htdocsIndex As Integer = imagePath.ToLower().IndexOf("htdocs")
            If htdocsIndex > 0 Then
                Dim webPath As String = imagePath.Substring(htdocsIndex + 7) ' Skip "htdocs\"
                webPath = webPath.Replace("\", "/")
                Return "http://localhost/" & webPath
            End If
        End If

        ' If relative path (like "uploads/products/...")
        ' Combine with base URL
        Dim cleanPath As String = imagePath.Replace("\", "/")
        If cleanPath.StartsWith("/") Then
            cleanPath = cleanPath.Substring(1)
        End If

        Return WEB_BASE_URL & cleanPath
    End Function

    ' =======================================================
    ' 🗑️ DELETE FUNCTION WITH ENHANCED CONFIRMATION
    ' =======================================================
    Private Sub DeleteProductWithConfirmation(productId As Integer, productName As String)

        Dim query As String = "DELETE FROM products WHERE ProductID = @id"

        Try
            ' Show confirmation dialog with product details
            Dim result As DialogResult = MessageBox.Show(
                "Are you sure you want to delete this product?" & vbCrLf & vbCrLf &
                "Product ID: " & productId & vbCrLf &
                "Product Name: " & productName & vbCrLf & vbCrLf &
                "⚠️ This action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            )

            If result = DialogResult.Yes Then
                openConn()

                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@id", productId)
                cmd.ExecuteNonQuery()

                ' Log Activity
                ActivityLogger.LogUserActivity(
                    "Product Deleted", 
                    "Product", 
                    $"Deleted Product: {productName} (ID: {productId})", 
                    "Admin Panel"
                )

                MessageBox.Show("✓ Product deleted successfully!" & vbCrLf & vbCrLf &
                              "Product: " & productName,
                              "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Refresh the data grid
                LoadMenuItems(txtSearch.Text, Category.Text)
            End If

        Catch ex As Exception
            MessageBox.Show("❌ Error deleting product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    ' =======================================================
    ' SEARCH FUNCTIONALITY
    ' =======================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If txtSearch.ForeColor = Color.Black Then
            LoadMenuItems(txtSearch.Text, Category.Text)
        End If
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        If txtSearch.Text = "Search menu items..." Then
            txtSearch.Text = ""
            txtSearch.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            txtSearch.Text = "Search menu items..."
            txtSearch.ForeColor = Color.Gray
        End If
    End Sub

    ' =======================================================
    ' CATEGORY FILTER
    ' =======================================================
    Private Sub Category_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Category.SelectedIndexChanged
        LoadMenuItems(txtSearch.Text, Category.Text)
    End Sub

    ' =======================================================
    ' BUTTON EVENTS
    ' =======================================================
    Private Sub AddMenuItemsbtn_Click(sender As Object, e As EventArgs) Handles AddMenuItemsbtn.Click
        With FormAddNewmenuItem
            .StartPosition = FormStartPosition.CenterScreen
            If .ShowDialog() = DialogResult.OK Then
                LoadMenuItems(txtSearch.Text, Category.Text)
            End If
        End With
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Text = "Search menu items..."
        txtSearch.ForeColor = Color.Gray
        Category.SelectedIndex = 0
        LoadMenuItems()
        MessageBox.Show("✓ Data refreshed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnToggleAvailability_Click(sender As Object, e As EventArgs) Handles btnToggleAvailability.Click
        If DataGridMenu.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to toggle availability.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim productId As Integer = Convert.ToInt32(DataGridMenu.SelectedRows(0).Cells("ProductID").Value)

        Try
            openConn()

            ' Get current value from DB
            Dim cmdGet As New MySqlCommand("SELECT Availability FROM products WHERE ProductID = @id", conn)
            cmdGet.Parameters.AddWithValue("@id", productId)
            Dim current As String = If(cmdGet.ExecuteScalar(), "").ToString()

            ' Toggle correctly based on your ENUM values
            Dim newStatus As String = If(current = "Available", "Not Available", "Available")

            ' Update database
            Dim cmdUpdate As New MySqlCommand("UPDATE products SET Availability = @status, LastUpdated = NOW() WHERE ProductID = @id", conn)
            cmdUpdate.Parameters.AddWithValue("@status", newStatus)
            cmdUpdate.Parameters.AddWithValue("@id", productId)
            cmdUpdate.ExecuteNonQuery()

            MessageBox.Show($"✓ Availability changed to: {newStatus}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Log Activity
            ActivityLogger.LogUserActivity(
                "Product Availability Updated", 
                "Product", 
                $"Updated availability for Product ID {productId} to '{newStatus}'", 
                "Admin Panel"
            )

            ' Force full refresh
            DataGridMenu.DataSource = Nothing
            LoadMenuItems(txtSearch.Text, Category.Text)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "CSV files (*.csv)|*.csv"
            saveDialog.FileName = $"MenuItems_{DateTime.Now:yyyyMMdd_HHmmss}.csv"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim csv As New System.Text.StringBuilder()

                ' Add headers
                Dim headers As New List(Of String)
                For Each col As DataGridViewColumn In DataGridMenu.Columns
                    If col.Name <> "ViewImageButton" And col.Name <> "EditButton" And col.Name <> "DeleteButton" And col.Name <> "ImageDisplay" And col.Name <> "ImagePath" Then
                        headers.Add(col.HeaderText)
                    End If
                Next
                csv.AppendLine(String.Join(",", headers))

                ' Add rows
                For Each row As DataGridViewRow In DataGridMenu.Rows
                    Dim values As New List(Of String)
                    For Each col As DataGridViewColumn In DataGridMenu.Columns
                        If col.Name <> "ViewImageButton" And col.Name <> "EditButton" And col.Name <> "DeleteButton" And col.Name <> "ImageDisplay" And col.Name <> "ImagePath" Then
                            Dim value As String = If(row.Cells(col.Name).Value?.ToString(), "")
                            values.Add($"""{value}""")
                        End If
                    Next
                    csv.AppendLine(String.Join(",", values))
                Next

                System.IO.File.WriteAllText(saveDialog.FileName, csv.ToString())
                MessageBox.Show("✓ Data exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error exporting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================
    ' 🥘 CHECK INGREDIENTS BUTTON - NEW FEATURE
    ' =======================================================
    Private Sub btnCheckIngredients_Click(sender As Object, e As EventArgs) Handles btnCheckIngredients.Click
        Try
            Dim ingredientsForm As New FormCheckIngredients()
            ingredientsForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error opening ingredients viewer: " & ex.Message & vbCrLf & vbCrLf &
                          "Please ensure FormCheckIngredients.vb exists in your project.",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DataGridMenu_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridMenu.DataBindingComplete
        ' Hide columns
        If DataGridMenu.Columns.Contains("ProductID") Then
            DataGridMenu.Columns("ProductID").Visible = False
        End If
        If DataGridMenu.Columns.Contains("ImagePath") Then
            DataGridMenu.Columns("ImagePath").Visible = False
        End If

        ' Update total items count AFTER data binding is complete
        If lblTotalItems IsNot Nothing Then
            Try
                Dim rowCount As Integer = 0
                If DataGridMenu.DataSource IsNot Nothing Then
                    Dim dt As DataTable = TryCast(DataGridMenu.DataSource, DataTable)
                    If dt IsNot Nothing Then
                        ' Use the actual data source to get distinct IDs
                        ' True means distinct
                        rowCount = dt.DefaultView.ToTable(True, "ProductID").Rows.Count
                    Else
                        ' Fallback for other data sources
                        Dim distinctIds As New HashSet(Of String)()
                        For Each row As DataGridViewRow In DataGridMenu.Rows
                            If Not row.IsNewRow AndAlso row.Cells("ProductID").Value IsNot Nothing Then
                                distinctIds.Add(row.Cells("ProductID").Value.ToString())
                            End If
                        Next
                        rowCount = distinctIds.Count
                    End If
                End If
                lblTotalItems.Text = "Total Items: " & rowCount.ToString()
            Catch ex As Exception
                ' Final fallback to ensure the label is not empty
                lblTotalItems.Text = "Total Items: " & DataGridMenu.Rows.Count.ToString()
            End Try
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Edit.Click

    End Sub

    ' ===============================
    ' EDIT BUTTON (Panel button)
    ' ===============================
    Private Sub Edit_Click(sender As Object, e As EventArgs) Handles Edit.Click

        If DataGridMenu.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to edit.", "No Selection")
            Exit Sub
        End If

        Dim id As Integer = DataGridMenu.SelectedRows(0).Cells("ProductID").Value

        Dim editForm As New FormEditMenu()
        editForm.LoadProductData(id)

        If editForm.ShowDialog() = DialogResult.OK Then
            LoadMenuItems(txtSearch.Text, Category.Text)
        End If

    End Sub


    ' ===============================
    ' DELETE BUTTON (Panel button)
    ' ===============================
    Private Sub Delete_Click(sender As Object, e As EventArgs) Handles Delete.Click

        If DataGridMenu.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to delete.", "No Selection")
            Exit Sub
        End If

        Dim id As Integer = DataGridMenu.SelectedRows(0).Cells("ProductID").Value
        Dim name As String = DataGridMenu.SelectedRows(0).Cells("ProductName").Value.ToString()

        Dim result = MessageBox.Show($"Delete this product: {name}?",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            Try
                openConn()
                Dim cmd As New MySqlCommand("DELETE FROM products WHERE ProductID=@id", conn)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()
                conn.Close()

                MessageBox.Show("Deleted successfully!")
                LoadMenuItems(txtSearch.Text, Category.Text)

            Catch ex As Exception
                MessageBox.Show("Error deleting: " & ex.Message)
            End Try
        End If

    End Sub


End Class