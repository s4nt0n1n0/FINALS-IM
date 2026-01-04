Imports MySqlConnector
Imports System.Data

Public Class FormPayroll
    ' Database connection string

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 10
    Private _totalRecords As Integer = 0
    Private _totalPages As Integer = 0

    '====================================
    ' FORM LOAD
    '====================================
    Private Sub FormPayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
        LoadPayrollData()
    End Sub

    '====================================
    ' SETUP GRID
    '====================================
    Private Sub SetupDataGridView()
        With DataGridView1
             .AutoGenerateColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .ReadOnly = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .RowHeadersVisible = False
                .BackgroundColor = Color.White
                .BorderStyle = BorderStyle.None
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .GridColor = Color.FromArgb(241, 245, 249)
                .DefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 250, 252)
                .DefaultCellStyle.SelectionForeColor = Color.Black ' Changed to Black for better readability on select
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
                .ColumnHeadersDefaultCellStyle.BackColor = Color.White
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
                .ColumnHeadersHeight = 50
                .RowTemplate.Height = 50
                .EnableHeadersVisualStyles = False
            
            .Columns.Clear()
            .Columns.Add(CreateColumn("EmployeeName", "Employee", 200))
            .Columns.Add(CreateColumn("Position", "Role/Type", 120))
            .Columns.Add(CreateColumn("BasicSalary", "Basic Pay", 120, "₱#,##0"))
            .Columns.Add(CreateColumn("Overtime", "OT Pay", 120, "₱#,##0"))
            .Columns.Add(CreateColumn("Bonuses", "Tips/Bonus", 120, "₱#,##0"))
            .Columns.Add(CreateColumn("GrossPay", "Gross", 120, "₱#,##0"))
            .Columns.Add(CreateColumn("Deductions", "Deductions", 120, "₱#,##0"))
            .Columns.Add(CreateColumn("NetPay", "Net Pay", 120, "₱#,##0"))
            ' Status column or Action button could be added here
        End With
    End Sub

    Private Function CreateColumn(name As String, header As String, width As Integer, Optional format As String = "") As DataGridViewTextBoxColumn
        Dim col As New DataGridViewTextBoxColumn With {
            .Name = name,
            .DataPropertyName = name,
            .HeaderText = header,
            .Width = width
        }
        If Not String.IsNullOrEmpty(format) Then
            col.DefaultCellStyle.Format = format
        End If
        Return col
    End Function

    '====================================
    ' LOAD PAYROLL DATA
    '====================================
    Private Sub LoadPayrollData()
        Try
            Using conn As New MySqlConnection(modDB.strConnection)
                conn.Open()

                Dim sYear As Integer = Reports.SelectedYear
                Dim sMonth As Integer = Reports.SelectedMonth
                If sMonth = 0 Then sMonth = DateTime.Now.Month

                ' --- 1. FETCH SUMMARY TOTALS ---
                Dim querySummary As String = "SELECT " &
                                             "IFNULL(SUM(NetPay), 0) as TotalNet, " &
                                             "IFNULL(SUM(Deductions), 0) as TotalDeductions, " &
                                             "IFNULL(SUM(BasicSalary + Overtime + Bonuses), 0) as TotalGross " &
                                             "FROM payroll " &
                                             $"WHERE Status = 'Paid' "

                ' Date Filter Logic
                If Reports.SelectedPeriod = "Daily" AndAlso dtpFilter IsNot Nothing Then
                     querySummary &= $"AND DATE(PayPeriodStart) = '{dtpFilter.Value:yyyy-MM-dd}'"
                ElseIf Reports.SelectedPeriod = "Monthly" Then
                     querySummary &= $"AND MONTH(PayPeriodStart) = {sMonth} AND YEAR(PayPeriodStart) = {sYear}"
                ElseIf Reports.SelectedPeriod = "Weekly" AndAlso dtpFilter IsNot Nothing Then
                     querySummary &= $"AND YEARWEEK(PayPeriodStart, 1) = YEARWEEK('{dtpFilter.Value:yyyy-MM-dd}', 1)"
                ElseIf Reports.SelectedPeriod = "Yearly" Then
                     querySummary &= $"AND YEAR(PayPeriodStart) = {sYear}"
                Else
                     ' Default to year if unknown
                     querySummary &= $"AND YEAR(PayPeriodStart) = {sYear}"
                End If

                Using cmd As New MySqlCommand(querySummary, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Label4.Text = "₱" & Convert.ToDecimal(reader("TotalNet")).ToString("N0") ' Net Pay Card
                            Label6.Text = "₱" & Convert.ToDecimal(reader("TotalDeductions")).ToString("N0") ' Deductions Card
                            Label7.Text = "₱" & Convert.ToDecimal(reader("TotalGross")).ToString("N0") ' Gross Card
                        End If
                    End Using
                End Using

                ' Update Card Labels to match context
                Label1.Text = "Total Net Pay"
                Label2.Text = "Total Deductions"
                Label3.Text = "Gross Payroll"
                Label5.Visible = False ' Hide trend/subtitle for now or update it
                Label8.Visible = False
                Label9.Visible = False


                ' --- 2. PAGINATION & GRID DATA ---
                FetchTotalCount(conn, sMonth, sYear)
                
                Dim offset As Integer = (_currentPage - 1) * _pageSize

                Dim queryGrid As String = "SELECT " &
                                          "p.PayrollID, " &
                                          "CONCAT(e.FirstName, ' ', e.LastName) as EmployeeName, " &
                                          "e.Position, " &
                                          "p.BasicSalary, " &
                                          "p.Overtime, " &
                                          "p.Bonuses, " &
                                          "(-IFNULL(p.Deductions, 0)) as Deductions, " &
                                          "p.NetPay, " &
                                          "(p.BasicSalary + p.Overtime + p.Bonuses) as GrossPay " &
                                          "FROM payroll p " &
                                          "JOIN employee e ON p.EmployeeID = e.EmployeeID " &
                                          "WHERE 1=1 "

                If Reports.SelectedPeriod = "Daily" AndAlso dtpFilter IsNot Nothing Then
                     queryGrid &= $"AND DATE(p.PayPeriodStart) = '{dtpFilter.Value:yyyy-MM-dd}' "
                ElseIf Reports.SelectedPeriod = "Monthly" Then
                     queryGrid &= $"AND MONTH(p.PayPeriodStart) = {sMonth} AND YEAR(p.PayPeriodStart) = {sYear} "
                ElseIf Reports.SelectedPeriod = "Weekly" AndAlso dtpFilter IsNot Nothing Then
                     queryGrid &= $"AND YEARWEEK(p.PayPeriodStart, 1) = YEARWEEK('{dtpFilter.Value:yyyy-MM-dd}', 1) "
                ElseIf Reports.SelectedPeriod = "Yearly" Then
                     queryGrid &= $"AND YEAR(p.PayPeriodStart) = {sYear} "
                Else
                     queryGrid &= $"AND YEAR(p.PayPeriodStart) = {sYear} "
                End If

                queryGrid &= "ORDER BY p.PayPeriodStart DESC, e.FirstName ASC " &
                                          $"LIMIT {_pageSize} OFFSET {offset}"

                Using adapter As New MySqlDataAdapter(queryGrid, conn)
                    Dim dt As New DataTable()
                    adapter.Fill(dt)
                    DataGridView1.DataSource = dt
                    FormatGrid()
                End Using
                
                UpdatePaginationUI()

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading payroll: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub FetchTotalCount(conn As MySqlConnection, sMonth As Integer, sYear As Integer)
        Dim query As String = "SELECT COUNT(*) FROM payroll p WHERE 1=1 "
        
        If Reports.SelectedPeriod = "Daily" AndAlso dtpFilter IsNot Nothing Then
             query &= $"AND DATE(p.PayPeriodStart) = '{dtpFilter.Value:yyyy-MM-dd}'"
        ElseIf Reports.SelectedPeriod = "Monthly" Then
             query &= $"AND MONTH(p.PayPeriodStart) = {sMonth} AND YEAR(p.PayPeriodStart) = {sYear}"
        ElseIf Reports.SelectedPeriod = "Weekly" AndAlso dtpFilter IsNot Nothing Then
             query &= $"AND YEARWEEK(p.PayPeriodStart, 1) = YEARWEEK('{dtpFilter.Value:yyyy-MM-dd}', 1)"
        ElseIf Reports.SelectedPeriod = "Yearly" Then
             query &= $"AND YEAR(p.PayPeriodStart) = {sYear}"
        Else
             query &= $"AND YEAR(p.PayPeriodStart) = {sYear}"
        End If
        Using cmd As New MySqlCommand(query, conn)
            _totalRecords = Convert.ToInt32(cmd.ExecuteScalar())
            _totalPages = Math.Ceiling(_totalRecords / _pageSize)
            If _totalPages = 0 Then _totalPages = 1
        End Using
    End Sub

    Private Sub UpdatePaginationUI()
        lblPageStatus.Text = $"Page {_currentPage} of {_totalPages}"
        btnPrev.Enabled = _currentPage > 1
        btnNext.Enabled = _currentPage < _totalPages
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If _currentPage > 1 Then
            _currentPage -= 1
            LoadPayrollData()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If _currentPage < _totalPages Then
            _currentPage += 1
            LoadPayrollData()
        End If
    End Sub

    Private Sub FormatGrid()
        For Each row As DataGridViewRow In DataGridView1.Rows
            ' Style Net Pay (Green)
            row.Cells("NetPay").Style.ForeColor = Color.SeaGreen
            row.Cells("NetPay").Style.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)

            ' Style Deductions (Red if negative)
            Dim ded As Decimal
            If Decimal.TryParse(row.Cells("Deductions").Value.ToString(), ded) AndAlso ded < 0 Then
                row.Cells("Deductions").Style.ForeColor = Color.Crimson
            End If
        Next
    End Sub

    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        If Reports.Instance IsNot Nothing Then
            Reports.Instance.ExportCurrentReport()
        Else
            MessageBox.Show("Please open the Reports screen to export.", "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
    End Sub
    ' =======================================================================
    ' REFRESH DATA
    ' =======================================================================
    Public Sub RefreshData()
        _currentPage = 1
        LoadPayrollData()
    End Sub

    Private Sub dtpFilter_ValueChanged(sender As Object, e As EventArgs) Handles dtpFilter.ValueChanged
        RefreshData()
    End Sub

End Class