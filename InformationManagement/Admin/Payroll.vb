Imports MySqlConnector
Imports System.IO
Imports System.Text

Public Class Payroll
    ' Pagination variables
    Private currentPage As Integer = 1
    Private pageSize As Integer = 15
    Private totalRecords As Integer = 0
    Private totalPages As Integer = 0
    Private allPayrollData As DataTable
    Private searchText As String = ""
    Private _lastSearchText As String = ""
    Private isInitializing As Boolean = True

    ' Filter variables
    Private currentStatusFilter As String = "All"

    Private Class PayPeriodItem
        Public StartDate As Date
        Public EndDate As Date
        Public DisplayText As String

        Public Overrides Function ToString() As String
            Return DisplayText
        End Function
    End Class

    Private Sub Payroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Me.Controls.Contains(AddNewPayrollRecordbtn) Then
            AddNewPayrollRecordbtn.Visible = False
        End If

        ConfigureResponsiveGrid()
        InitializeSearchBox()
        InitializeStatusFilter()
        PopulatePayPeriods()

        isInitializing = False

        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        Else
            LoadEmployees()
        End If
    End Sub

    Private Sub ConfigureResponsiveGrid()
        Try
            If DataGridView1.Columns.Count > 0 Then
                For Each col As DataGridViewColumn In DataGridView1.Columns
                    col.Frozen = False
                Next
            End If

            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

            If DataGridView1.Columns.Count > 0 Then
                If DataGridView1.Columns.Contains("Employee") Then DataGridView1.Columns("Employee").FillWeight = 25
                If DataGridView1.Columns.Contains("Position") Then DataGridView1.Columns("Position").FillWeight = 12
                If DataGridView1.Columns.Contains("Hours") Then DataGridView1.Columns("Hours").FillWeight = 10
                If DataGridView1.Columns.Contains("HourlyRate") Then DataGridView1.Columns("HourlyRate").FillWeight = 13
                If DataGridView1.Columns.Contains("Overtime") Then DataGridView1.Columns("Overtime").FillWeight = 12
                If DataGridView1.Columns.Contains("GrossPay") Then DataGridView1.Columns("GrossPay").FillWeight = 13
                If DataGridView1.Columns.Contains("NetPay") Then DataGridView1.Columns("NetPay").FillWeight = 13
                If DataGridView1.Columns.Contains("Status") Then DataGridView1.Columns("Status").FillWeight = 12

                If DataGridView1.Columns.Contains("Actions") Then
                    DataGridView1.Columns("Actions").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    DataGridView1.Columns("Actions").Width = 150
                    DataGridView1.Columns("Actions").ReadOnly = False
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error configuring grid layout: " & ex.Message, "Grid Configuration Warning",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Function FormatPeso(amount As Decimal) As String
        Return ChrW(&H20B1) & amount.ToString("N2")
    End Function

    Public Sub LoadEmployees(Optional startDate As Date? = Nothing, Optional endDate As Date? = Nothing)
        Try
            openConn()

            Dim startRange As Date = If(startDate.HasValue, startDate.Value, New Date(DateTime.Now.Year, DateTime.Now.Month, 1))
            Dim endRange As Date = If(endDate.HasValue, endDate.Value, startRange.AddMonths(1).AddDays(-1))

            Dim workingDays As Integer = GetWorkingDays(startRange, endRange)
            Dim maxRegularHours As Decimal = workingDays * 8

            Dim query As String = "SELECT 
                e.EmployeeID,
                CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
                'Staff' AS Position,
                e.Salary,
                IFNULL(attn.TotalHours, 0) as TotalHours,
                IFNULL(e.Salary / 8.0, 0) as HourlyRate,
                IFNULL(p.BasicSalary, -1) as BasicSalary,
                IFNULL(p.Overtime, 0) as Overtime,
                IFNULL(p.Deductions, 0) as Deductions,
                IFNULL(p.Bonuses, 0) as Bonuses,
                IFNULL(p.NetPay, 0) as NetPay,
                IFNULL(p.Status, 'No Record') as Status,
                p.PayrollID,
                attn.LateCount,
                attn.AbsentCount
                FROM employee e
                INNER JOIN user_accounts u ON e.EmployeeID = u.employee_id
                LEFT JOIN (
                    SELECT EmployeeID, 
                           SUM(WorkHours) as TotalHours,
                           SUM(CASE WHEN Status = 'Late' THEN 1 ELSE 0 END) as LateCount,
                           SUM(CASE WHEN Status = 'Absent' THEN 1 ELSE 0 END) as AbsentCount
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

            Dim totalGross As Decimal = 0
            Dim totalNet As Decimal = 0
            Dim sumHours As Decimal = 0
            Dim totalDeductions As Decimal = 0

            For Each row As DataRow In allPayrollData.Rows
                Dim hours As Decimal = If(row("TotalHours") IsNot DBNull.Value, Convert.ToDecimal(row("TotalHours")), 0)
                Dim hourlyRate As Decimal = If(row("HourlyRate") IsNot DBNull.Value, Convert.ToDecimal(row("HourlyRate")), 0)
                Dim status As String = row("Status").ToString()

                Dim basicSalary As Decimal = 0
                Dim overtimePay As Decimal = 0
                Dim netPay As Decimal = 0
                Dim deductions As Decimal = 0

                If status <> "No Record" Then
                    basicSalary = If(row("BasicSalary") IsNot DBNull.Value AndAlso Convert.ToDecimal(row("BasicSalary")) >= 0,
                                   Convert.ToDecimal(row("BasicSalary")), 0)
                    overtimePay = If(row("Overtime") IsNot DBNull.Value, Convert.ToDecimal(row("Overtime")), 0)
                    netPay = If(row("NetPay") IsNot DBNull.Value, Convert.ToDecimal(row("NetPay")), 0)
                    deductions = If(row("Deductions") IsNot DBNull.Value, Convert.ToDecimal(row("Deductions")), 0)
                Else
                    Dim regularHours As Decimal = Math.Min(hours, maxRegularHours)
                    Dim overtimeHours As Decimal = Math.Max(0, hours - maxRegularHours)

                    basicSalary = regularHours * hourlyRate
                    overtimePay = overtimeHours * (hourlyRate * 1.5D)

                    ' Calculate automatic deductions for late/absent
                    Dim lateCount As Integer = If(row("LateCount") IsNot DBNull.Value, Convert.ToInt32(row("LateCount")), 0)
                    Dim absentCount As Integer = If(row("AbsentCount") IsNot DBNull.Value, Convert.ToInt32(row("AbsentCount")), 0)
                    deductions = CalculateAttendanceDeductions(lateCount, absentCount, hourlyRate)

                    netPay = basicSalary + overtimePay - deductions
                End If

                totalGross += (basicSalary + overtimePay)
                totalNet += netPay
                sumHours += hours
                totalDeductions += deductions
            Next

            lblTotalGrossPay.Text = FormatPeso(totalGross)
            lblTotalNetPay.Text = FormatPeso(totalNet)
            TotalHours.Text = sumHours.ToString("F2") & " hrs"
            E.Text = allPayrollData.Rows.Count.ToString()

            ' Add deductions label if exists
            If Me.Controls.ContainsKey("lblTotalDeductions") Then
                Me.Controls("lblTotalDeductions").Text = FormatPeso(totalDeductions)
            End If

            ApplySearchFilter()

        Catch ex As Exception
            MessageBox.Show("Error loading employees: " & ex.Message & vbCrLf & vbCrLf &
                          "Stack Trace: " & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Function CalculateAttendanceDeductions(lateCount As Integer, absentCount As Integer, hourlyRate As Decimal) As Decimal
        ' Deduction policy: 
        ' - 1 hour pay per late (configurable)
        ' - 1 day (8 hours) pay per absence
        Dim lateDeduction As Decimal = lateCount * hourlyRate
        Dim absentDeduction As Decimal = absentCount * (hourlyRate * 8)
        Return lateDeduction + absentDeduction
    End Function

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

        Dim filteredData As DataTable = allPayrollData.Clone()

        For Each row As DataRow In allPayrollData.Rows
            Dim matchesSearch As Boolean = True
            Dim matchesStatus As Boolean = True

            ' Search filter
            If Not String.IsNullOrWhiteSpace(searchText) Then
                Dim employeeName As String = If(row("EmployeeName") IsNot DBNull.Value, row("EmployeeName").ToString().ToLower(), "")
                Dim position As String = If(row("Position") IsNot DBNull.Value, row("Position").ToString().ToLower(), "")
                Dim status As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString().ToLower(), "")

                matchesSearch = employeeName.Contains(searchText.ToLower()) OrElse
                               position.Contains(searchText.ToLower()) OrElse
                               status.Contains(searchText.ToLower())
            End If

            ' Status filter
            If currentStatusFilter <> "All" Then
                Dim rowStatus As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString(), "No Record")
                matchesStatus = (rowStatus.Equals(currentStatusFilter, StringComparison.OrdinalIgnoreCase))
            End If

            If matchesSearch AndAlso matchesStatus Then
                filteredData.ImportRow(row)
            End If
        Next

        totalRecords = filteredData.Rows.Count
        totalPages = If(totalRecords > 0, Math.Ceiling(totalRecords / pageSize), 1)

        LoadPage(1, filteredData)
    End Sub

    Private Sub LoadPage(pageNumber As Integer, Optional dataSource As DataTable = Nothing)
        If dataSource Is Nothing Then dataSource = allPayrollData
        If dataSource Is Nothing OrElse dataSource.Rows.Count = 0 Then
            DataGridView1.Rows.Clear()
            UpdatePaginationControls()
            Return
        End If

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

            Dim hours As Decimal = If(row("TotalHours") IsNot DBNull.Value, Convert.ToDecimal(row("TotalHours")), 0)
            Dim hourlyRate As Decimal = If(row("HourlyRate") IsNot DBNull.Value, Convert.ToDecimal(row("HourlyRate")), 0)

            newRow.Cells("Hours").Value = If(hours > 0, hours.ToString("F2"), "-")
            newRow.Cells("HourlyRate").Value = If(hourlyRate > 0, FormatPeso(hourlyRate), "-")

            Dim status As String = row("Status").ToString()
            Dim basicSalary As Decimal = 0
            Dim overtimePay As Decimal = 0
            Dim netPay As Decimal = 0
            Dim deductions As Decimal = 0

            If status <> "No Record" Then
                basicSalary = If(row("BasicSalary") IsNot DBNull.Value AndAlso Convert.ToDecimal(row("BasicSalary")) >= 0,
                               Convert.ToDecimal(row("BasicSalary")), 0)
                overtimePay = If(row("Overtime") IsNot DBNull.Value, Convert.ToDecimal(row("Overtime")), 0)
                netPay = If(row("NetPay") IsNot DBNull.Value, Convert.ToDecimal(row("NetPay")), 0)
            Else
                Dim currentPeriod As PayPeriodItem = TryCast(ComboBox1.SelectedItem, PayPeriodItem)
                Dim maxRegularHours As Decimal = 80

                If currentPeriod IsNot Nothing AndAlso currentPeriod.StartDate <> Nothing Then
                    Dim wDays As Integer = GetWorkingDays(currentPeriod.StartDate, currentPeriod.EndDate)
                    maxRegularHours = wDays * 8
                End If

                Dim regularHours As Decimal = Math.Min(hours, maxRegularHours)
                Dim overtimeHours As Decimal = Math.Max(0, hours - maxRegularHours)

                basicSalary = regularHours * hourlyRate
                overtimePay = overtimeHours * (hourlyRate * 1.5D)

                Dim lateCount As Integer = If(row("LateCount") IsNot DBNull.Value, Convert.ToInt32(row("LateCount")), 0)
                Dim absentCount As Integer = If(row("AbsentCount") IsNot DBNull.Value, Convert.ToInt32(row("AbsentCount")), 0)
                deductions = CalculateAttendanceDeductions(lateCount, absentCount, hourlyRate)

                netPay = basicSalary + overtimePay - deductions
            End If

            newRow.Cells("Overtime").Value = If(overtimePay > 0, FormatPeso(overtimePay), "-")

            Dim gross As Decimal = basicSalary + overtimePay
            newRow.Cells("GrossPay").Value = If(gross > 0, FormatPeso(gross), "-")
            newRow.Cells("NetPay").Value = If(netPay > 0, FormatPeso(netPay), "-")
            newRow.Cells("Status").Value = status

            ' Enhanced color coding
            Select Case status.ToLower()
                Case "paid"
                    newRow.DefaultCellStyle.BackColor = Color.FromArgb(220, 252, 231) ' Light green
                Case "approved"
                    newRow.DefaultCellStyle.BackColor = Color.FromArgb(254, 249, 195) ' Light yellow
                Case "pending"
                    newRow.DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199) ' Light amber
                Case "no record"
                    If hours > 0 Then
                        newRow.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226) ' Light red
                    Else
                        newRow.DefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249) ' Light gray
                    End If
            End Select

            Dim actionText As String = GetActionText(status, hours)
            newRow.Cells("Actions").Value = actionText

            newRow.Tag = New With {
                .EmployeeID = row("EmployeeID"),
                .PayrollID = If(row("PayrollID") IsNot DBNull.Value, Convert.ToInt32(row("PayrollID")), 0),
                .Hours = hours,
                .Rate = hourlyRate,
                .BasicSalary = basicSalary,
                .OvertimePay = overtimePay,
                .NetPay = netPay,
                .Deductions = deductions,
                .LateCount = If(row("LateCount") IsNot DBNull.Value, Convert.ToInt32(row("LateCount")), 0),
                .AbsentCount = If(row("AbsentCount") IsNot DBNull.Value, Convert.ToInt32(row("AbsentCount")), 0)
            }
        Next

        UpdatePaginationControls()
    End Sub

    Private Function GetActionText(status As String, hours As Decimal) As String
        Select Case status.ToLower()
            Case "no record"
                Return If(hours > 0, "Generate", "-")
            Case "pending"
                Return "Edit | Approve"
            Case "approved"
                Return "Mark as Paid | Print"
            Case "paid"
                Return "View Receipt"
            Case Else
                Return "View"
        End Select
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                If DataGridView1.Columns(e.ColumnIndex).Name = "Actions" Then
                    Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                    If selectedRow.Tag Is Nothing Then Return

                    Dim tagData = selectedRow.Tag
                    Dim buttonText As String = If(selectedRow.Cells("Actions").Value?.ToString(), "")

                    If String.IsNullOrEmpty(buttonText) OrElse buttonText = "-" Then Return

                    Dim employeeName As String = If(selectedRow.Cells("Employee").Value?.ToString(), "Unknown")
                    Dim employeeID As Integer = If(tagData.EmployeeID, 0)
                    Dim payrollID As Integer = If(tagData.PayrollID, 0)

                    ' Route to appropriate handler
                    If buttonText.Contains("Generate") Then
                        HandleGenerateAction(selectedRow, tagData)
                    ElseIf buttonText = "Edit | Approve" Then
                        HandlePendingAction(payrollID, employeeID, employeeName)
                    ElseIf buttonText.Contains("Mark as Paid") Then
                        HandleMarkAsPaidAction(payrollID, employeeName)
                    ElseIf buttonText.Contains("Print") Then
                        HandlePrintPayslip(payrollID, employeeName)
                    ElseIf buttonText.Contains("View Receipt") Then
                        HandleViewReceipt(payrollID, employeeName)
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error processing action: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HandleGenerateAction(row As DataGridViewRow, tagData As Object)
        Dim employeeName As String = If(row.Cells("Employee").Value?.ToString(), "Unknown")
        Dim employeeID As Integer = If(tagData.EmployeeID, 0)
        Dim hours As Decimal = If(tagData.Hours, 0)
        Dim rate As Decimal = If(tagData.Rate, 0)
        Dim basic As Decimal = If(tagData.BasicSalary, 0)
        Dim overtime As Decimal = If(tagData.OvertimePay, 0)
        Dim deductions As Decimal = If(tagData.Deductions, 0)
        Dim net As Decimal = If(tagData.NetPay, 0)
        Dim lateCount As Integer = If(tagData.LateCount, 0)
        Dim absentCount As Integer = If(tagData.AbsentCount, 0)

        Dim message As New StringBuilder()
        message.AppendLine($"Generate payroll for {employeeName}?")
        message.AppendLine()
        message.AppendLine($"Total Hours: {hours:F2}")
        message.AppendLine($"Hourly Rate: {FormatPeso(rate)}")
        message.AppendLine($"Basic Pay: {FormatPeso(basic)}")
        message.AppendLine($"Overtime Pay: {FormatPeso(overtime)}")
        If deductions > 0 Then
            message.AppendLine($"Deductions: {FormatPeso(deductions)}")
            If lateCount > 0 Then message.AppendLine($"  - Late: {lateCount}x")
            If absentCount > 0 Then message.AppendLine($"  - Absent: {absentCount}x")
        End If
        message.AppendLine($"Net Pay: {FormatPeso(net)}")

        Dim result As DialogResult = MessageBox.Show(message.ToString(), "Generate Payroll",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            GeneratePayrollFromAttendance(employeeID, hours, rate, basic, overtime, deductions)
        End If
    End Sub

    Private Sub HandlePendingAction(payrollID As Integer, employeeID As Integer, employeeName As String)
        Dim result As DialogResult = MessageBox.Show(
            $"Select action for {employeeName}:" & vbCrLf & vbCrLf &
            "[Yes] - Approve for Payment" & vbCrLf &
            "[No] - Edit Details" & vbCrLf &
            "[Cancel] - Do nothing",
            "Pending Payroll Action",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            UpdatePayrollStatus(payrollID, "Approved")
        ElseIf result = DialogResult.No Then
            HandleEditAction(payrollID, employeeID, employeeName)
        End If
    End Sub

    Private Sub HandleEditAction(payrollID As Integer, employeeID As Integer, employeeName As String)
        If payrollID > 0 Then
            Dim editForm As New FormEditPayroll(payrollID, employeeID, employeeName)
            editForm.ShowDialog()
            RefreshCurrentView()
        Else
            MessageBox.Show("No payroll record to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Sub HandlePrintPayslip(payrollID As Integer, employeeName As String)
        Try
            ' Generate and print payslip
            GeneratePayslip(payrollID)
        Catch ex As Exception
            MessageBox.Show("Error printing payslip: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HandleViewReceipt(payrollID As Integer, employeeName As String)
        Try
            GeneratePayslip(payrollID)
        Catch ex As Exception
            MessageBox.Show("Error viewing receipt: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GeneratePayslip(payrollID As Integer)
        Try
            openConn()

            Dim query As String = "SELECT 
                p.*,
                CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
                e.Salary,
                e.Department
                FROM payroll p
                INNER JOIN employee e ON p.EmployeeID = e.EmployeeID
                WHERE p.PayrollID = @payrollID"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@payrollID", payrollID)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                Dim payslip As New StringBuilder()
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine("           PAYROLL RECEIPT / PAYSLIP")
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine()
                payslip.AppendLine($"Employee: {reader("EmployeeName")}")
                payslip.AppendLine($"Department: {reader("Department")}")
                payslip.AppendLine($"Pay Period: {Convert.ToDateTime(reader("PayPeriodStart")):MMM dd} - {Convert.ToDateTime(reader("PayPeriodEnd")):MMM dd, yyyy}")
                payslip.AppendLine($"Payment Date: {DateTime.Now:MMM dd, yyyy}")
                payslip.AppendLine()
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine("EARNINGS:")
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine($"Hours Worked: {Convert.ToDecimal(reader("HoursWorked")):F2} hrs")
                payslip.AppendLine($"Hourly Rate: {FormatPeso(Convert.ToDecimal(reader("HourlyRate")))}")
                payslip.AppendLine($"Basic Salary: {FormatPeso(Convert.ToDecimal(reader("BasicSalary")))}")
                payslip.AppendLine($"Overtime Pay: {FormatPeso(Convert.ToDecimal(reader("Overtime")))}")
                If Convert.ToDecimal(reader("Bonuses")) > 0 Then
                    payslip.AppendLine($"Bonuses: {FormatPeso(Convert.ToDecimal(reader("Bonuses")))}")
                End If
                payslip.AppendLine()
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine("DEDUCTIONS:")
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine($"Total Deductions: {FormatPeso(Convert.ToDecimal(reader("Deductions")))}")
                payslip.AppendLine()
                payslip.AppendLine("???????????????????????????????????????????")
                Dim netPay As Decimal = Convert.ToDecimal(reader("NetPay"))
                payslip.AppendLine($"NET PAY: {FormatPeso(netPay)}")
                payslip.AppendLine("???????????????????????????????????????????")
                payslip.AppendLine()
                payslip.AppendLine($"Status: {reader("Status")}")
                payslip.AppendLine($"Payroll ID: {payrollID}")

                reader.Close()

                ' Display in message box (could be enhanced to print or save as PDF)
                MessageBox.Show(payslip.ToString(), "Payslip", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Optional: Save to file
                SavePayslipToFile(payrollID, payslip.ToString())
            Else
                reader.Close()
                MessageBox.Show("Payroll record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Error generating payslip: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub SavePayslipToFile(payrollID As Integer, content As String)
        Try
            Dim folderPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Payslips")
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim fileName As String = $"Payslip_{payrollID}_{DateTime.Now:yyyyMMdd}.txt"
            Dim filePath As String = Path.Combine(folderPath, fileName)

            File.WriteAllText(filePath, content)
        Catch ex As Exception
            ' Silent fail - don't interrupt user if file save fails
        End Try
    End Sub

    Private Sub GeneratePayrollFromAttendance(employeeID As Integer, hours As Decimal, rate As Decimal, basic As Decimal, overtime As Decimal, deductions As Decimal)
        Try
            openConn()

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

            ' Check for duplicate payroll
            Dim checkQuery As String = "SELECT COUNT(*) FROM payroll 
                WHERE EmployeeID = @empID AND PayPeriodStart = @start AND PayPeriodEnd = @end"
            Dim checkCmd As New MySqlCommand(checkQuery, conn)
            checkCmd.Parameters.AddWithValue("@empID", employeeID)
            checkCmd.Parameters.AddWithValue("@start", periodStart)
            checkCmd.Parameters.AddWithValue("@end", periodEnd)

            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            If count > 0 Then
                MessageBox.Show("Payroll already exists for this period!", "Duplicate Record",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                closeConn()
                Return
            End If

            Dim net As Decimal = basic + overtime - deductions

            Dim query As String = "INSERT INTO payroll 
                (EmployeeID, PayPeriodStart, PayPeriodEnd, HoursWorked, HourlyRate, BasicSalary, 
                 Overtime, Deductions, Bonuses, NetPay, Status, CreatedDate) 
                VALUES (@empID, @start, @end, @hours, @rate, @basic, @overtime, @deductions, 0, @net, 'Pending', NOW())"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@empID", employeeID)
            cmd.Parameters.AddWithValue("@start", periodStart)
            cmd.Parameters.AddWithValue("@end", periodEnd)
            cmd.Parameters.AddWithValue("@hours", hours)
            cmd.Parameters.AddWithValue("@rate", rate)
            cmd.Parameters.AddWithValue("@basic", basic)
            cmd.Parameters.AddWithValue("@overtime", overtime)
            cmd.Parameters.AddWithValue("@deductions", deductions)
            cmd.Parameters.AddWithValue("@net", net)

            cmd.ExecuteNonQuery()
            closeConn()

            MessageBox.Show("Payroll generated successfully!" & vbCrLf & vbCrLf &
                          $"Basic Pay: {FormatPeso(basic)}" & vbCrLf &
                          $"Overtime: {FormatPeso(overtime)}" & vbCrLf &
                          $"Deductions: {FormatPeso(deductions)}" & vbCrLf &
                          $"Net Pay: {FormatPeso(net)}",
                          "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            RefreshCurrentView()

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

            MessageBox.Show($"Payroll status updated to {newStatus}!", "Success",
                          MessageBoxButtons.OK, MessageBoxIcon.Information)

            RefreshCurrentView()

        Catch ex As Exception
            MessageBox.Show("Error updating status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub RefreshCurrentView()
        If ComboBox1.SelectedItem IsNot Nothing AndAlso TypeOf ComboBox1.SelectedItem Is PayPeriodItem Then
            Dim period = DirectCast(ComboBox1.SelectedItem, PayPeriodItem)
            LoadEmployees(period.StartDate, period.EndDate)
        Else
            LoadEmployees()
        End If
    End Sub

    ' ============================================
    ' PAGINATION CONTROLS
    ' ============================================
    Private Sub btnFirstPage_Click(sender As Object, e As EventArgs) Handles btnFirstPage.Click
        If currentPage > 1 Then
            ApplySearchFilter()
        End If
    End Sub

    Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
        If currentPage > 1 Then
            LoadPage(currentPage - 1, GetFilteredData())
        End If
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If currentPage < totalPages Then
            LoadPage(currentPage + 1, GetFilteredData())
        End If
    End Sub

    Private Sub btnLastPage_Click(sender As Object, e As EventArgs) Handles btnLastPage.Click
        If currentPage < totalPages Then
            LoadPage(totalPages, GetFilteredData())
        End If
    End Sub

    Private Function GetFilteredData() As DataTable
        If allPayrollData Is Nothing Then Return Nothing

        Dim filteredData As DataTable = allPayrollData.Clone()

        For Each row As DataRow In allPayrollData.Rows
            Dim matchesSearch As Boolean = True
            Dim matchesStatus As Boolean = True

            ' Search filter
            If Not String.IsNullOrWhiteSpace(searchText) Then
                Dim employeeName As String = If(row("EmployeeName") IsNot DBNull.Value, row("EmployeeName").ToString().ToLower(), "")
                Dim position As String = If(row("Position") IsNot DBNull.Value, row("Position").ToString().ToLower(), "")
                Dim status As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString().ToLower(), "")

                matchesSearch = employeeName.Contains(searchText.ToLower()) OrElse
                               position.Contains(searchText.ToLower()) OrElse
                               status.Contains(searchText.ToLower())
            End If

            ' Status filter
            If currentStatusFilter <> "All" Then
                Dim rowStatus As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString(), "No Record")
                matchesStatus = (rowStatus.Equals(currentStatusFilter, StringComparison.OrdinalIgnoreCase))
            End If

            If matchesSearch AndAlso matchesStatus Then
                filteredData.ImportRow(row)
            End If
        Next

        Return filteredData
    End Function

    Private Sub UpdatePaginationControls()
        lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} records)"

        btnFirstPage.Enabled = (currentPage > 1)
        btnPreviousPage.Enabled = (currentPage > 1)
        btnNextPage.Enabled = (currentPage < totalPages)
        btnLastPage.Enabled = (currentPage < totalPages)

        btnFirstPage.BackColor = If(btnFirstPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnPreviousPage.BackColor = If(btnPreviousPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnNextPage.BackColor = If(btnNextPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnLastPage.BackColor = If(btnLastPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
    End Sub

    ' ============================================
    ' SEARCH FUNCTIONALITY
    ' ============================================
    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If isInitializing Then Return

        Dim currentSearch As String = TextBoxSearch.Text.Trim()
        If currentSearch = "Search payroll..." Then currentSearch = ""

        If currentSearch = _lastSearchText Then Return
        _lastSearchText = currentSearch

        searchText = currentSearch
        ApplySearchFilter()
    End Sub

    Private Sub TextBoxSearch_Enter(sender As Object, e As EventArgs) Handles TextBoxSearch.Enter
        If TextBoxSearch.Text = "Search payroll..." Then
            TextBoxSearch.Text = ""
            TextBoxSearch.ForeColor = Color.FromArgb(15, 23, 42)
        End If
        txtSearch.BorderColor = Color.FromArgb(99, 102, 241)
    End Sub

    Private Sub TextBoxSearch_Leave(sender As Object, e As EventArgs) Handles TextBoxSearch.Leave
        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) Then
            TextBoxSearch.Text = "Search payroll..."
            TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
        End If
        txtSearch.BorderColor = Color.FromArgb(226, 232, 240)
    End Sub

    Private Sub InitializeSearchBox()
        TextBoxSearch.Text = "Search payroll..."
        TextBoxSearch.ForeColor = Color.FromArgb(148, 163, 184)
    End Sub

    ' ============================================
    ' STATUS FILTER
    ' ============================================
    Private Sub InitializeStatusFilter()
        ' Add this to your form if you have a status filter ComboBox
        ' If you don't have one, this section can be removed or a ComboBox can be added
        If Me.Controls.ContainsKey("cmbStatusFilter") Then
            Dim cmbStatus As ComboBox = CType(Me.Controls("cmbStatusFilter"), ComboBox)
            cmbStatus.Items.Clear()
            cmbStatus.Items.AddRange(New String() {"All", "No Record", "Pending", "Approved", "Paid"})
            cmbStatus.SelectedIndex = 0
            AddHandler cmbStatus.SelectedIndexChanged, AddressOf StatusFilter_Changed
        End If
    End Sub

    Private Sub StatusFilter_Changed(sender As Object, e As EventArgs)
        If isInitializing Then Return

        Dim cmb As ComboBox = CType(sender, ComboBox)
        currentStatusFilter = cmb.SelectedItem.ToString()
        ApplySearchFilter()
    End Sub

    ' ============================================
    ' PAY PERIOD MANAGEMENT
    ' ============================================
    Private Sub PopulatePayPeriods()
        ComboBox1.Items.Clear()

        Dim today As Date = DateTime.Now
        AddPeriod(today.Year, today.Month, 16, Date.DaysInMonth(today.Year, today.Month))
        AddPeriod(today.Year, today.Month, 1, 15)

        Dim lastMonth As Date = today.AddMonths(-1)
        AddPeriod(lastMonth.Year, lastMonth.Month, 16, Date.DaysInMonth(lastMonth.Year, lastMonth.Month))
        AddPeriod(lastMonth.Year, lastMonth.Month, 1, 15)

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

    Private Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles ComboBox1.DrawItem
        If e.Index < 0 Then Return
        Dim cmb As ComboBox = DirectCast(sender, ComboBox)
        e.DrawBackground()
        e.Graphics.DrawString(cmb.Items(e.Index).ToString(), cmb.Font, Brushes.Black, e.Bounds)
        e.DrawFocusRectangle()
    End Sub

    ' ============================================
    ' BULK OPERATIONS
    ' ============================================
    Private Sub BulkGeneratePayroll()
        Try
            If ComboBox1.SelectedItem Is Nothing Then
                MessageBox.Show("Please select a pay period first.", "No Period Selected",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim period = DirectCast(ComboBox1.SelectedItem, PayPeriodItem)

            Dim result As DialogResult = MessageBox.Show(
                $"Generate payroll for ALL employees with 'No Record' status?" & vbCrLf & vbCrLf &
                $"Pay Period: {period.DisplayText}" & vbCrLf &
                "This will create payroll records for all eligible employees.",
                "Bulk Generate Payroll",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.No Then Return

            Dim generatedCount As Integer = 0
            Dim skippedCount As Integer = 0

            For Each row As DataRow In allPayrollData.Rows
                Dim status As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString(), "No Record")
                Dim hours As Decimal = If(row("TotalHours") IsNot DBNull.Value, Convert.ToDecimal(row("TotalHours")), 0)

                If status = "No Record" AndAlso hours > 0 Then
                    Dim employeeID As Integer = Convert.ToInt32(row("EmployeeID"))
                    Dim rate As Decimal = If(row("HourlyRate") IsNot DBNull.Value, Convert.ToDecimal(row("HourlyRate")), 0)

                    Dim workingDays As Integer = GetWorkingDays(period.StartDate, period.EndDate)
                    Dim maxRegularHours As Decimal = workingDays * 8
                    Dim regularHours As Decimal = Math.Min(hours, maxRegularHours)
                    Dim overtimeHours As Decimal = Math.Max(0, hours - maxRegularHours)

                    Dim basic As Decimal = regularHours * rate
                    Dim overtime As Decimal = overtimeHours * (rate * 1.5D)

                    Dim lateCount As Integer = If(row("LateCount") IsNot DBNull.Value, Convert.ToInt32(row("LateCount")), 0)
                    Dim absentCount As Integer = If(row("AbsentCount") IsNot DBNull.Value, Convert.ToInt32(row("AbsentCount")), 0)
                    Dim deductions As Decimal = CalculateAttendanceDeductions(lateCount, absentCount, rate)

                    Try
                        GeneratePayrollRecord(employeeID, period.StartDate, period.EndDate, hours, rate, basic, overtime, deductions)
                        generatedCount += 1
                    Catch ex As Exception
                        skippedCount += 1
                    End Try
                End If
            Next

            MessageBox.Show($"Bulk generation complete!" & vbCrLf & vbCrLf &
                          $"Generated: {generatedCount}" & vbCrLf &
                          $"Skipped: {skippedCount}",
                          "Bulk Generation Complete",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information)

            RefreshCurrentView()

        Catch ex As Exception
            MessageBox.Show("Error in bulk generation: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GeneratePayrollRecord(employeeID As Integer, startDate As Date, endDate As Date,
                                     hours As Decimal, rate As Decimal, basic As Decimal,
                                     overtime As Decimal, deductions As Decimal)
        openConn()

        Dim net As Decimal = basic + overtime - deductions

        Dim query As String = "INSERT INTO payroll 
            (EmployeeID, PayPeriodStart, PayPeriodEnd, HoursWorked, HourlyRate, BasicSalary, 
             Overtime, Deductions, Bonuses, NetPay, Status, CreatedDate) 
            VALUES (@empID, @start, @end, @hours, @rate, @basic, @overtime, @deductions, 0, @net, 'Pending', NOW())"

        Dim cmd As New MySqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@empID", employeeID)
        cmd.Parameters.AddWithValue("@start", startDate)
        cmd.Parameters.AddWithValue("@end", endDate)
        cmd.Parameters.AddWithValue("@hours", hours)
        cmd.Parameters.AddWithValue("@rate", rate)
        cmd.Parameters.AddWithValue("@basic", basic)
        cmd.Parameters.AddWithValue("@overtime", overtime)
        cmd.Parameters.AddWithValue("@deductions", deductions)
        cmd.Parameters.AddWithValue("@net", net)

        cmd.ExecuteNonQuery()
        closeConn()
    End Sub

    ' ============================================
    ' EXPORT TO EXCEL (Optional - requires Excel library)
    ' ============================================
    Private Sub ExportToExcel()
        Try
            If allPayrollData Is Nothing OrElse allPayrollData.Rows.Count = 0 Then
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "CSV files (*.csv)|*.csv"
            saveDialog.FileName = $"Payroll_Export_{DateTime.Now:yyyyMMdd}.csv"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                ExportToCSV(saveDialog.FileName)
                MessageBox.Show("Export completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error exporting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportToCSV(filePath As String)
        Dim csv As New StringBuilder()

        ' Headers
        csv.AppendLine("Employee Name,Position,Hours Worked,Hourly Rate,Basic Salary,Overtime,Deductions,Net Pay,Status")

        ' Data rows
        For Each row As DataRow In GetFilteredData().Rows
            Dim hours As Decimal = If(row("TotalHours") IsNot DBNull.Value, Convert.ToDecimal(row("TotalHours")), 0)
            Dim rate As Decimal = If(row("HourlyRate") IsNot DBNull.Value, Convert.ToDecimal(row("HourlyRate")), 0)
            Dim status As String = row("Status").ToString()

            Dim basic As Decimal = 0
            Dim overtime As Decimal = 0
            Dim net As Decimal = 0
            Dim deductions As Decimal = 0

            If status <> "No Record" Then
                basic = If(row("BasicSalary") IsNot DBNull.Value, Convert.ToDecimal(row("BasicSalary")), 0)
                overtime = If(row("Overtime") IsNot DBNull.Value, Convert.ToDecimal(row("Overtime")), 0)
                net = If(row("NetPay") IsNot DBNull.Value, Convert.ToDecimal(row("NetPay")), 0)
                deductions = If(row("Deductions") IsNot DBNull.Value, Convert.ToDecimal(row("Deductions")), 0)
            End If

            csv.AppendLine($"""{row("EmployeeName")}"",""{row("Position")}"",{hours:F2},{rate:F2},{basic:F2},{overtime:F2},{deductions:F2},{net:F2},{status}")
        Next

        File.WriteAllText(filePath, csv.ToString())
    End Sub

End Class