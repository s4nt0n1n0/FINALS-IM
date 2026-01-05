Imports MySqlConnector

Public Class FormPayroll
    ' Pagination variables
    Private currentPage As Integer = 1
    Private pageSize As Integer = 15
    Private totalRecords As Integer = 0
    Private totalPages As Integer = 0
    Private allPayrollData As DataTable
    Private searchText As String = ""

    Private _lastSearchText As String = ""
    Private isInitializing As Boolean = True

    Private Class PayPeriodItem
        Public StartDate As Date
        Public EndDate As Date
        Public DisplayText As String

        Public Overrides Function ToString() As String
            Return DisplayText
        End Function
    End Class
    Private Sub FormPayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Hide the Add New Payroll Record button
        If Me.Controls.Contains(AddNewPayrollRecordbtn) Then
            AddNewPayrollRecordbtn.Visible = False
        End If

        ' Make DataGridView responsive
        ConfigureResponsiveGrid()

        InitializeSearchBox()
        PopulatePayPeriods()

        isInitializing = False

        ' Select current period by default
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        Else
            LoadEmployees()
        End If
    End Sub

    Private Sub ConfigureResponsiveGrid()
        Try
            ' STEP 1: Unfreeze all columns FIRST (must do before setting AutoSize mode)
            If DataGridView1.Columns.Count > 0 Then
                For Each col As DataGridViewColumn In DataGridView1.Columns
                    col.Frozen = False
                Next
            End If

            ' STEP 2: Now set AutoSize mode (after unfreezing)
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            ' STEP 3: Set grid to fill available space
            DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

            ' STEP 4: Set individual column fill weights for better distribution
            If DataGridView1.Columns.Count > 0 Then
                ' Set fill weights (relative widths) - check if columns exist first
                If DataGridView1.Columns.Contains("Employee") Then DataGridView1.Columns("Employee").FillWeight = 25
                If DataGridView1.Columns.Contains("Position") Then DataGridView1.Columns("Position").FillWeight = 12
                If DataGridView1.Columns.Contains("Hours") Then DataGridView1.Columns("Hours").FillWeight = 10
                If DataGridView1.Columns.Contains("HourlyRate") Then DataGridView1.Columns("HourlyRate").FillWeight = 13
                If DataGridView1.Columns.Contains("Overtime") Then DataGridView1.Columns("Overtime").FillWeight = 12
                If DataGridView1.Columns.Contains("GrossPay") Then DataGridView1.Columns("GrossPay").FillWeight = 13
                If DataGridView1.Columns.Contains("NetPay") Then DataGridView1.Columns("NetPay").FillWeight = 13
                If DataGridView1.Columns.Contains("Status") Then DataGridView1.Columns("Status").FillWeight = 12

                ' Actions column uses absolute width
                If DataGridView1.Columns.Contains("Actions") Then
                    DataGridView1.Columns("Actions").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    DataGridView1.Columns("Actions").Width = 150
                    DataGridView1.Columns("Actions").ReadOnly = False ' Ensure clickable
                End If
            End If
        Catch ex As Exception
            ' Log error but don't crash the form
            MessageBox.Show("Error configuring grid layout: " & ex.Message & vbCrLf & "The grid will use default settings.",
                          "Grid Configuration Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ' Helper function to format currency as Philippine Peso
    Private Function FormatPeso(amount As Decimal) As String
        ' Use Unicode Peso sign safely even if the source file encoding is not UTF-8.
        Return ChrW(&H20B1) & amount.ToString("N2")
    End Function

    Public Sub LoadEmployees(Optional startDate As Date? = Nothing, Optional endDate As Date? = Nothing)
        Try
            openConn()

            ' Get date range
            Dim startRange As Date = If(startDate.HasValue, startDate.Value, New Date(DateTime.Now.Year, DateTime.Now.Month, 1))
            Dim endRange As Date = If(endDate.HasValue, endDate.Value, startRange.AddMonths(1).AddDays(-1))

            ' Calculate expected working days for this period (Mon-Fri)
            Dim workingDays As Integer = GetWorkingDays(startRange, endRange)
            Dim maxRegularHours As Decimal = workingDays * 8

            ' Load only employees with staff accounts (user_accounts)
            ' Position is hardcoded to 'Staff' as per requirements
            ' HourlyRate is derived from Salary (Daily Rate) / 8
            Dim query As String = "SELECT 
                e.EmployeeID,
                CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
                'Staff' AS Position,
                e.Salary,
                -- Total hours for THIS specific period
                IFNULL(attn.TotalHours, 0) as TotalHours,
                -- Hourly Rate = Daily Rate / 8
                IFNULL(e.Salary / 8.0, 0) as HourlyRate,
                -- Get payroll info for THIS specific period
                IFNULL(p.BasicSalary, -1) as BasicSalary, -- Use -1 to detect 'No Record' practically
                IFNULL(p.Overtime, 0) as Overtime,
                IFNULL(p.Deductions, 0) as Deductions,
                IFNULL(p.Bonuses, 0) as Bonuses,
                IFNULL(p.NetPay, 0) as NetPay,
                IFNULL(p.Status, 'No Record') as Status,
                p.PayrollID
                FROM employee e
                INNER JOIN user_accounts u ON e.EmployeeID = u.employee_id
                LEFT JOIN (
                    SELECT EmployeeID, SUM(WorkHours) as TotalHours
                    FROM employee_attendance
                    WHERE AttendanceDate BETWEEN @startDate AND @endDate
                    GROUP BY EmployeeID
                ) attn ON e.EmployeeID = attn.EmployeeID
                LEFT JOIN payroll p ON e.EmployeeID = p.EmployeeID 
                    AND p.PayPeriodStart = @startDate 
                    AND p.PayPeriodEnd = @endDate
                WHERE e.EmploymentStatus = 'Active'
                ORDER BY e.FirstName, e.LastName"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@startDate", startRange)
            cmd.Parameters.AddWithValue("@endDate", endRange)

            Dim adapter As New MySqlDataAdapter(cmd)
            allPayrollData = New DataTable()
            adapter.Fill(allPayrollData)

            ' Calculate totals from filtered data
            Dim totalGross As Decimal = 0
            Dim totalNet As Decimal = 0
            Dim sumHours As Decimal = 0

            For Each row As DataRow In allPayrollData.Rows
                Dim hours As Decimal = If(row("TotalHours") IsNot DBNull.Value, Convert.ToDecimal(row("TotalHours")), 0)
                Dim hourlyRate As Decimal = If(row("HourlyRate") IsNot DBNull.Value, Convert.ToDecimal(row("HourlyRate")), 0)
                Dim status As String = row("Status").ToString()

                Dim basicSalary As Decimal = 0
                Dim overtimePay As Decimal = 0
                Dim netPay As Decimal = 0
                Dim deductions As Decimal = 0

                If status <> "No Record" Then
                    ' Use existing payroll record values
                    basicSalary = If(row("BasicSalary") IsNot DBNull.Value AndAlso Convert.ToDecimal(row("BasicSalary")) >= 0, Convert.ToDecimal(row("BasicSalary")), 0)
                    overtimePay = If(row("Overtime") IsNot DBNull.Value, Convert.ToDecimal(row("Overtime")), 0)
                    deductions = If(row("Deductions") IsNot DBNull.Value, Convert.ToDecimal(row("Deductions")), 0)
                    netPay = If(row("NetPay") IsNot DBNull.Value, Convert.ToDecimal(row("NetPay")), 0)
                Else
                    ' Calculate Estimates
                    ' Split hours into Regular vs Overtime
                    Dim regularHours As Decimal = hours
                    Dim overtimeHours As Decimal = 0

                    If hours > maxRegularHours Then
                        regularHours = maxRegularHours
                        overtimeHours = hours - maxRegularHours
                    End If

                    basicSalary = regularHours * hourlyRate
                    overtimePay = overtimeHours * (hourlyRate * 1.5D)

                    ' Deductions (Placeholder logic - can be expanded if attendance status is available)
                    deductions = 0

                    netPay = basicSalary + overtimePay - deductions

                    ' Update row with calculated estimates for display if needed (or just keep them for the Tag)
                End If

                totalGross += (basicSalary + overtimePay)
                totalNet += netPay
                sumHours += hours
            Next

            ' Update summary labels
            lblTotalGrossPay.Text = FormatPeso(totalGross)
            lblTotalNetPay.Text = FormatPeso(totalNet)
            TotalHours.Text = sumHours.ToString("F2") & " hrs"
            E.Text = allPayrollData.Rows.Count.ToString()

            ' Store total records and calculate pages
            ApplySearchFilter()

        Catch ex As Exception
            MessageBox.Show("Error loading employees: " & ex.Message & vbCrLf & vbCrLf &
                          "Stack Trace: " & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Helper to count working days (Mon-Fri)
    Private Function GetWorkingDays(startDate As Date, endDate As Date) As Integer
        Dim days As Integer = 0
        Dim curr As Date = startDate
        While curr <= endDate
            If curr.DayOfWeek <> DayOfWeek.Saturday AndAlso curr.DayOfWeek <> DayOfWeek.Sunday Then
                days += 1
            End If
            curr = curr.AddDays(1)
        End While
        Return days
    End Function

    Private Sub ApplySearchFilter()
        If allPayrollData Is Nothing Then Return

        Dim filteredData As DataTable

        If String.IsNullOrWhiteSpace(searchText) Then
            filteredData = allPayrollData
        Else
            filteredData = allPayrollData.Clone()
            For Each row As DataRow In allPayrollData.Rows
                Dim employeeName As String = If(row("EmployeeName") IsNot DBNull.Value, row("EmployeeName").ToString().ToLower(), "")
                Dim position As String = If(row("Position") IsNot DBNull.Value, row("Position").ToString().ToLower(), "")
                Dim status As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString().ToLower(), "")

                If employeeName.Contains(searchText.ToLower()) OrElse
                   position.Contains(searchText.ToLower()) OrElse
                   status.Contains(searchText.ToLower()) Then
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
        If dataSource Is Nothing Then dataSource = allPayrollData
        If dataSource Is Nothing OrElse dataSource.Rows.Count = 0 Then
            DataGridView1.Rows.Clear()
            UpdatePaginationControls()
            Return
        End If

        ' Validate page number
        If pageNumber < 1 Then pageNumber = 1
        If pageNumber > totalPages Then pageNumber = totalPages

        currentPage = pageNumber
        Dim startIndex As Integer = (currentPage - 1) * pageSize
        Dim endIndex As Integer = Math.Min(startIndex + pageSize, dataSource.Rows.Count)

        DataGridView1.Rows.Clear()

        For i As Integer = startIndex To endIndex - 1
            Dim row As DataRow = dataSource.Rows(i)
            Dim rowIndex As Integer = DataGridView1.Rows.Add()
            Dim newRow As DataGridViewRow = DataGridView1.Rows(rowIndex)

            newRow.Cells("Employee").Value = row("EmployeeName").ToString()
            newRow.Cells("Position").Value = row("Position").ToString()

            ' Get hours from attendance
            Dim hours As Decimal = If(row("TotalHours") IsNot DBNull.Value, Convert.ToDecimal(row("TotalHours")), 0)
            Dim hourlyRate As Decimal = If(row("HourlyRate") IsNot DBNull.Value, Convert.ToDecimal(row("HourlyRate")), 0)

            newRow.Cells("Hours").Value = If(hours > 0, hours.ToString("F2"), "-")
            newRow.Cells("HourlyRate").Value = If(hourlyRate > 0, FormatPeso(hourlyRate), "-")

            ' --- Calculation Logic for Row Display ---

            ' Check working days context from ComboBox if possible, or recalculate range 
            ' (Assuming standard calculation for display similar to LoadEmployees)
            ' Note: We don't have direct access to range here easily without passing it, 
            ' but we can assume the calculations are needed for 'Status = No Record'.

            Dim status As String = row("Status").ToString()
            Dim basicSalary As Decimal = 0
            Dim overtimePay As Decimal = 0
            Dim netPay As Decimal = 0
            Dim deductions As Decimal = 0

            If status <> "No Record" Then
                ' Saved values
                basicSalary = If(row("BasicSalary") IsNot DBNull.Value AndAlso Convert.ToDecimal(row("BasicSalary")) >= 0, Convert.ToDecimal(row("BasicSalary")), 0)
                overtimePay = If(row("Overtime") IsNot DBNull.Value, Convert.ToDecimal(row("Overtime")), 0)
                netPay = If(row("NetPay") IsNot DBNull.Value, Convert.ToDecimal(row("NetPay")), 0)
            Else
                ' Calculate on the fly for display
                Dim currentPeriod As PayPeriodItem = TryCast(ComboBox1.SelectedItem, PayPeriodItem)
                Dim maxRegularHours As Decimal = 80 ' Fallback

                If currentPeriod.StartDate <> Nothing Then
                    Dim wDays As Integer = GetWorkingDays(currentPeriod.StartDate, currentPeriod.EndDate)
                    maxRegularHours = wDays * 8
                End If

                Dim regularHours As Decimal = hours
                Dim overtimeHours As Decimal = 0

                If hours > maxRegularHours Then
                    regularHours = maxRegularHours
                    overtimeHours = hours - maxRegularHours
                End If

                basicSalary = regularHours * hourlyRate
                overtimePay = overtimeHours * (hourlyRate * 1.5D)
                netPay = basicSalary + overtimePay ' Deductions assumed 0 for estimate
            End If

            newRow.Cells("Overtime").Value = If(overtimePay > 0, FormatPeso(overtimePay), "-")

            Dim gross As Decimal = basicSalary + overtimePay
            newRow.Cells("GrossPay").Value = If(gross > 0, FormatPeso(gross), "-")
            newRow.Cells("NetPay").Value = If(netPay > 0, FormatPeso(netPay), "-")
            newRow.Cells("Status").Value = status

            ' Color code rows based on status
            Select Case status.ToLower()
                Case "paid"
                    newRow.DefaultCellStyle.BackColor = Color.LightGreen
                Case "pending", "approved"
                    newRow.DefaultCellStyle.BackColor = Color.LightYellow
                Case "no record"
                    If hours > 0 Then
                        newRow.DefaultCellStyle.BackColor = Color.LightCoral
                    End If
            End Select

            ' Smart Actions button based on status
            Dim actionText As String = "View"
            Select Case status.ToLower()
                Case "no record"
                    actionText = If(hours > 0, "Generate", "-")
                Case "pending"
                    actionText = "Edit | Approve"
                Case "approved"
                    actionText = "Mark as Paid"
                Case "paid"
                    actionText = "Completed"
            End Select

            newRow.Cells("Actions").Value = actionText

            ' Store Data in Tag for Action Handling
            newRow.Tag = New With {
                    .EmployeeID = row("EmployeeID"),
                    .PayrollID = If(row("PayrollID") IsNot DBNull.Value, Convert.ToInt32(row("PayrollID")), 0),
                    .Hours = hours,
                    .Rate = hourlyRate,
                    .BasicSalary = basicSalary,
                    .OvertimePay = overtimePay,
                    .NetPay = netPay
                }

        Next

        UpdatePaginationControls()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            ' Check if valid row and Actions column clicked
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                If DataGridView1.Columns(e.ColumnIndex).Name = "Actions" Then
                    Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                    ' Check if row has tag data
                    If selectedRow.Tag Is Nothing Then
                        Return
                    End If

                    Dim tagData = selectedRow.Tag

                    ' Safely get button text
                    Dim buttonText As String = ""
                    If selectedRow.Cells("Actions").Value IsNot Nothing Then
                        buttonText = selectedRow.Cells("Actions").Value.ToString()
                    End If

                    ' Skip if no action or just "-"
                    If String.IsNullOrEmpty(buttonText) OrElse buttonText = "-" Then
                        Return
                    End If

                    ' Safely get employee name
                    Dim employeeName As String = "Unknown"
                    If selectedRow.Cells("Employee").Value IsNot Nothing Then
                        employeeName = selectedRow.Cells("Employee").Value.ToString()
                    End If

                    ' Extract data from tag
                    Dim employeeID As Integer = If(tagData.EmployeeID IsNot Nothing, tagData.EmployeeID, 0)
                    Dim payrollID As Integer = If(tagData.PayrollID IsNot Nothing, tagData.PayrollID, 0)
                    Dim hours As Decimal = If(tagData.Hours IsNot Nothing, tagData.Hours, 0)
                    Dim rate As Decimal = If(tagData.Rate IsNot Nothing, tagData.Rate, 0)

                    Dim basicSalary As Decimal = If(tagData.BasicSalary IsNot Nothing, tagData.BasicSalary, 0)
                    Dim overtimePay As Decimal = If(tagData.OvertimePay IsNot Nothing, tagData.OvertimePay, 0)
                    Dim netPay As Decimal = If(tagData.NetPay IsNot Nothing, tagData.NetPay, 0)

                    ' Handle different actions
                    If buttonText.Contains("Generate") Then
                        HandleGenerateAction(employeeID, hours, rate, basicSalary, overtimePay, netPay)

                    ElseIf buttonText = "Edit | Approve" Then
                        ' Ask user what they want to do
                        Dim result As DialogResult = MessageBox.Show(
                            $"Select action for {employeeName}:" & vbCrLf & vbCrLf &
                            "[Yes] - Approve for Payment" & vbCrLf &
                            "[No] - Edit Details" & vbCrLf &
                            "[Cancel] - Do nothing",
                            "Pending Payroll Action",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question)

                        If result = DialogResult.Yes Then
                            HandleApproveAction(payrollID, employeeName)
                        ElseIf result = DialogResult.No Then
                            HandleEditAction(payrollID, employeeID, employeeName)
                        End If

                    ElseIf buttonText.Contains("Edit") Then
                        HandleEditAction(payrollID, employeeID, employeeName)

                    ElseIf buttonText.Contains("Approve") Then
                        HandleApproveAction(payrollID, employeeName)

                    ElseIf buttonText.Contains("Mark as Paid") Then
                        HandleMarkAsPaidAction(payrollID, employeeName)

                    ElseIf buttonText = "Completed" Then
                        ' Do nothing, just show as completed
                        Return
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error processing action: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HandleGenerateAction(employeeID As Integer, hours As Decimal, rate As Decimal, basic As Decimal, overtime As Decimal, net As Decimal)
        Dim result As DialogResult = MessageBox.Show(
            $"Generate payroll for this employee?" & vbCrLf & vbCrLf &
            $"Total Hours: {hours:F2}" & vbCrLf &
            $"Hourly Rate: {FormatPeso(rate)}" & vbCrLf &
            $"Basic Pay: {FormatPeso(basic)}" & vbCrLf &
            $"Overtime Pay: {FormatPeso(overtime)}" & vbCrLf &
            $"Net Pay: {FormatPeso(net)}",
            "Generate Payroll",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            GeneratePayrollFromAttendance(employeeID, hours, rate, basic, overtime)
        End If
    End Sub

    Private Sub HandleEditAction(payrollID As Integer, employeeID As Integer, employeeName As String)
        If payrollID > 0 Then
            ' Edit form is now available!
            Dim editForm As New FormEditPayroll(payrollID, employeeID, employeeName)
            editForm.ShowDialog()
        Else
            MessageBox.Show("No payroll record to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub HandleApproveAction(payrollID As Integer, employeeName As String)
        Dim result As DialogResult = MessageBox.Show(
            $"Approve payroll for {employeeName}" & vbCrLf & vbCrLf &
            "This will change status to 'Approved' and allow payment processing.",
            "Approve Payroll",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            UpdatePayrollStatus(payrollID, "Approved")
        End If
    End Sub

    Private Sub HandleMarkAsPaidAction(payrollID As Integer, employeeName As String)
        Dim result As DialogResult = MessageBox.Show(
            $"Mark payroll as PAID for {employeeName}?" & vbCrLf & vbCrLf &
            "This action marks the payroll as completed." & vbCrLf &
            "Make sure payment has been processed!",
            "Mark as Paid",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            UpdatePayrollStatus(payrollID, "Paid")
        End If
    End Sub

    Private Sub HandleViewReceiptAction(payrollID As Integer, employeeName As String)
        ' TODO: Implement receipt viewing/printing
        MessageBox.Show($"Payroll receipt for {employeeName}" & vbCrLf &
                      $"Payroll ID: {payrollID}" & vbCrLf & vbCrLf &
                      "Receipt viewing/printing coming soon!",
                      "Payroll Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub GeneratePayrollFromAttendance(employeeID As Integer, hours As Decimal, rate As Decimal, basic As Decimal, overtime As Decimal)
        Try
            openConn()

            ' Use selected period or fallback to current month
            Dim periodStart As Date
            Dim periodEnd As Date

            If ComboBox1.SelectedItem IsNot Nothing AndAlso TypeOf ComboBox1.SelectedItem Is PayPeriodItem Then
                Dim period = DirectCast(ComboBox1.SelectedItem, PayPeriodItem)
                periodStart = period.StartDate
                periodEnd = period.EndDate
            Else
                periodStart = New Date(DateTime.Now.Year, DateTime.Now.Month, 1)
                periodEnd = periodStart.AddMonths(1).AddDays(-1)
            End If

            Dim net As Decimal = basic + overtime ' No deductions yet

            Dim query As String = "INSERT INTO payroll 
                (EmployeeID, PayPeriodStart, PayPeriodEnd, HoursWorked, HourlyRate, BasicSalary, 
                 Overtime, Deductions, Bonuses, NetPay, Status, CreatedDate) 
                VALUES (@empID, @start, @end, @hours, @rate, @basic, @overtime, 0, 0, @net, 'Pending', NOW())"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@empID", employeeID)
            cmd.Parameters.AddWithValue("@start", periodStart)
            cmd.Parameters.AddWithValue("@end", periodEnd)
            cmd.Parameters.AddWithValue("@hours", hours)
            cmd.Parameters.AddWithValue("@rate", rate)
            cmd.Parameters.AddWithValue("@basic", basic)
            cmd.Parameters.AddWithValue("@overtime", overtime)
            cmd.Parameters.AddWithValue("@net", net)

            cmd.ExecuteNonQuery()
            closeConn()

            MessageBox.Show("Payroll generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadEmployees(periodStart, periodEnd) ' Refresh

        Catch ex As Exception
            MessageBox.Show("Error generating payroll: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Public Sub UpdatePayrollStatus(payrollID As Integer, newStatus As String)
        Try
            openConn()
            Dim query As String = "UPDATE payroll SET Status = @status, ProcessedDate = NOW() WHERE PayrollID = @id"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@id", payrollID)
            cmd.ExecuteNonQuery()
            closeConn()

            LoadEmployees()
            MessageBox.Show("Payroll status updated to " & newStatus & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error updating status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        If allPayrollData Is Nothing Then Return Nothing

        If String.IsNullOrWhiteSpace(searchText) Then
            Return allPayrollData
        Else
            Dim filteredData As DataTable = allPayrollData.Clone()
            For Each row As DataRow In allPayrollData.Rows
                Dim employeeName As String = If(row("EmployeeName") IsNot DBNull.Value, row("EmployeeName").ToString().ToLower(), "")
                Dim position As String = If(row("Position") IsNot DBNull.Value, row("Position").ToString().ToLower(), "")
                Dim status As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString().ToLower(), "")

                If employeeName.Contains(searchText.ToLower()) OrElse
                   position.Contains(searchText.ToLower()) OrElse
                   status.Contains(searchText.ToLower()) Then
                    filteredData.ImportRow(row)
                End If
            Next
            Return filteredData
        End If
    End Function

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

    ' Search functionality
    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitializing Then Return

        Dim currentSearch As String = TextBoxSearch.Text.Trim()
        If currentSearch = "Search payroll..." Then currentSearch = ""

        ' Only reload if search term actually changed
        If currentSearch = _lastSearchText Then Return
        _lastSearchText = currentSearch

        searchText = currentSearch
        ApplySearchFilter()
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search payroll..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42) ' Dark slate color
        End If
        txtSearch.BorderColor = Color.FromArgb(99, 102, 241) ' Purple/Indigo border
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search payroll..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184) ' Slate-400
        End If
        txtSearch.BorderColor = Color.FromArgb(226, 232, 240) ' Default slate-200
    End Sub

    ' =======================================================
    ' INITIALIZE SEARCH BOX
    ' =======================================================
    Private Sub InitializeSearchBox()
        TextBoxSearch.Text = "Search payroll..."
        TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
    End Sub

    Private Sub PopulatePayPeriods()
        ComboBox1.Items.Clear()

        ' Add current month periods
        Dim today As Date = DateTime.Now
        AddPeriod(today.Year, today.Month, 16, Date.DaysInMonth(today.Year, today.Month)) ' 16-End
        AddPeriod(today.Year, today.Month, 1, 15) ' 1-15

        ' Add last month periods
        Dim lastMonth As Date = today.AddMonths(-1)
        AddPeriod(lastMonth.Year, lastMonth.Month, 16, Date.DaysInMonth(lastMonth.Year, lastMonth.Month))
        AddPeriod(lastMonth.Year, lastMonth.Month, 1, 15)

        ' Add month before last
        Dim monthBefore As Date = today.AddMonths(-2)
        AddPeriod(monthBefore.Year, monthBefore.Month, 16, Date.DaysInMonth(monthBefore.Year, monthBefore.Month))
        AddPeriod(monthBefore.Year, monthBefore.Month, 1, 15)
    End Sub

    Private Sub AddPeriod(year As Integer, month As Integer, dayStart As Integer, dayEnd As Integer)
        Dim startDate As New Date(year, month, dayStart)
        Dim endDate As New Date(year, month, dayEnd)
        Dim item As New PayPeriodItem With {
            .StartDate = startDate,
            .EndDate = endDate,
            .DisplayText = startDate.ToString("MMM d") & "-" & endDate.Day & ", " & startDate.Year
        }
        ComboBox1.Items.Add(item)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If isInitializing Then Return

        If ComboBox1.SelectedItem IsNot Nothing AndAlso TypeOf ComboBox1.SelectedItem Is PayPeriodItem Then
            Dim period = DirectCast(ComboBox1.SelectedItem, PayPeriodItem)
            LoadEmployees(period.StartDate, period.EndDate)
        End If
    End Sub
    Private Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) _
        Handles ComboBox1.DrawItem

        If e.Index < 0 Then Return
        Dim cmb As ComboBox = DirectCast(sender, ComboBox)
        e.DrawBackground()
        e.Graphics.DrawString(cmb.Items(e.Index).ToString(), cmb.Font, Brushes.Black, e.Bounds)
        e.DrawFocusRectangle()
    End Sub

End Class