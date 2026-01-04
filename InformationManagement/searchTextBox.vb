Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Public Class SearchTextBox
    Inherits Panel

    Private WithEvents txt As New TextBox()
    Private WithEvents btnSearch As New Button()
    Private WithEvents btnClear As New Button()
    Private isFocused As Boolean = False
    Private _borderRadius As Integer = 8
    Private _backColor As Color = Color.White
    Private _focusBorderColor As Color = Color.DodgerBlue
    Private _normalBorderColor As Color = Color.FromArgb(200, 200, 200)
    Private _showSearchButton As Boolean = True
    Private _showClearButton As Boolean = True

    ' Events
    Public Event SearchButtonClick(sender As Object, e As EventArgs)
    Public Event SearchTextChanged(sender As Object, searchText As String)
    Public Event ClearButtonClick(sender As Object, e As EventArgs)

    Public Sub New()
        ' Configure Panel
        Me.Size = New Size(250, 35)
        Me.BackColor = Color.Transparent
        Me.MinimumSize = New Size(100, 25)

        ' Configure TextBox
        txt.BorderStyle = BorderStyle.None
        txt.Font = New Font("Segoe UI", 10)
        txt.TextAlign = HorizontalAlignment.Left
        txt.Anchor = AnchorStyles.Left Or AnchorStyles.Right

        ' Configure Search Button
        btnSearch.Text = "🔍"
        btnSearch.FlatStyle = FlatStyle.Flat
        btnSearch.FlatAppearance.BorderSize = 0
        btnSearch.BackColor = Color.Transparent
        btnSearch.Cursor = Cursors.Hand
        btnSearch.Font = New Font("Segoe UI", 10)
        btnSearch.Size = New Size(30, 25)

        ' Configure Clear Button
        btnClear.Text = "✕"
        btnClear.FlatStyle = FlatStyle.Flat
        btnClear.FlatAppearance.BorderSize = 0
        btnClear.BackColor = Color.Transparent
        btnClear.Cursor = Cursors.Hand
        btnClear.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        btnClear.Size = New Size(25, 25)
        btnClear.Visible = False

        ' Add controls to panel
        Me.Controls.Add(txt)
        Me.Controls.Add(btnSearch)
        Me.Controls.Add(btnClear)

        ' Apply colors
        txt.BackColor = _backColor
        txt.ForeColor = Color.Black

        UpdateControlPositions()

        ' Set double buffering
        Me.SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        Me.UpdateStyles()
    End Sub

    ' Properties
    Public Overrides Property Text As String
        Get
            Return txt.Text
        End Get
        Set(value As String)
            txt.Text = value
        End Set
    End Property

    Public Property PlaceholderText As String
        Get
            Return If(txt.Tag IsNot Nothing, txt.Tag.ToString(), "")
        End Get
        Set(value As String)
            txt.Tag = value
        End Set
    End Property

    Public Property TextFont As Font
        Get
            Return txt.Font
        End Get
        Set(value As Font)
            txt.Font = value
        End Set
    End Property

    Public Property BorderRadius As Integer
        Get
            Return _borderRadius
        End Get
        Set(value As Integer)
            If value >= 0 Then
                _borderRadius = value
                Me.Invalidate()
            End If
        End Set
    End Property

    Public Property TextBoxBackColor As Color
        Get
            Return _backColor
        End Get
        Set(value As Color)
            _backColor = value
            txt.BackColor = value
            Me.Invalidate()
        End Set
    End Property

    Public Property TextColor As Color
        Get
            Return txt.ForeColor
        End Get
        Set(value As Color)
            txt.ForeColor = value
        End Set
    End Property

    Public Property FocusBorderColor As Color
        Get
            Return _focusBorderColor
        End Get
        Set(value As Color)
            _focusBorderColor = value
            If isFocused Then Me.Invalidate()
        End Set
    End Property

    Public Property NormalBorderColor As Color
        Get
            Return _normalBorderColor
        End Get
        Set(value As Color)
            _normalBorderColor = value
            If Not isFocused Then Me.Invalidate()
        End Set
    End Property

    Public Property ShowSearchButton As Boolean
        Get
            Return _showSearchButton
        End Get
        Set(value As Boolean)
            _showSearchButton = value
            btnSearch.Visible = value
            UpdateControlPositions()
        End Set
    End Property

    Public Property ShowClearButton As Boolean
        Get
            Return _showClearButton
        End Get
        Set(value As Boolean)
            _showClearButton = value
            UpdateClearButtonVisibility()
        End Set
    End Property

    Public Property MaxLength As Integer
        Get
            Return txt.MaxLength
        End Get
        Set(value As Integer)
            txt.MaxLength = value
        End Set
    End Property

    Public Property [ReadOnly] As Boolean
        Get
            Return txt.ReadOnly
        End Get
        Set(value As Boolean)
            txt.ReadOnly = value
        End Set
    End Property

    ' Methods
    Public Sub Clear()
        txt.Clear()
        UpdateClearButtonVisibility()
    End Sub

    Public Shadows Sub Focus()
        txt.Focus()
    End Sub

    Public Function PerformSearch() As String
        RaiseEvent SearchButtonClick(Me, EventArgs.Empty)
        Return txt.Text.Trim()
    End Function

    Private Sub UpdateControlPositions()
        Dim textBoxHeight As Integer = txt.PreferredHeight
        Dim verticalPadding As Integer = (Me.Height - textBoxHeight) \ 2
        Dim horizontalPadding As Integer = 10

        If verticalPadding < 5 Then verticalPadding = 5

        ' Position search button (right side)
        If btnSearch.Visible Then
            btnSearch.Location = New Point(Me.Width - btnSearch.Width - 5, (Me.Height - btnSearch.Height) \ 2)
        End If

        ' Position clear button (before search button)
        If btnClear.Visible Then
            Dim clearX As Integer = If(btnSearch.Visible, btnSearch.Left - btnClear.Width - 2, Me.Width - btnClear.Width - 5)
            btnClear.Location = New Point(clearX, (Me.Height - btnClear.Height) \ 2)
        End If

        ' Position textbox
        Dim rightPadding As Integer = horizontalPadding
        If btnSearch.Visible Then rightPadding += btnSearch.Width + 5
        If btnClear.Visible Then rightPadding += btnClear.Width + 2

        txt.Location = New Point(horizontalPadding, verticalPadding)
        txt.Width = Me.Width - horizontalPadding - rightPadding
    End Sub

    Private Sub UpdateClearButtonVisibility()
        btnClear.Visible = _showClearButton AndAlso txt.Text.Length > 0
        UpdateControlPositions()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim rect As New Rectangle(0, 0, Me.Width - 1, Me.Height - 1)
        Dim path As GraphicsPath = GetRoundedRectangle(rect, _borderRadius)

        ' Fill background
        Using brush As New SolidBrush(_backColor)
            g.FillPath(brush, path)
        End Using

        ' Draw border
        Dim borderColor As Color
        Dim borderWidth As Single

        If isFocused Then
            borderColor = _focusBorderColor
            borderWidth = 2.0F
        Else
            borderColor = _normalBorderColor
            borderWidth = 1.5F
        End If

        Using pen As New Pen(borderColor, borderWidth)
            g.DrawPath(pen, path)
        End Using
    End Sub

    Private Function GetRoundedRectangle(bounds As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2
        Dim arc As New Rectangle(bounds.X, bounds.Y, diameter, diameter)

        path.AddArc(arc, 180, 90)
        arc.X = bounds.Right - diameter
        path.AddArc(arc, 270, 90)
        arc.Y = bounds.Bottom - diameter
        path.AddArc(arc, 0, 90)
        arc.X = bounds.Left
        path.AddArc(arc, 90, 90)
        path.CloseFigure()

        Return path
    End Function

    ' Event Handlers
    Private Sub txt_Enter(sender As Object, e As EventArgs) Handles txt.Enter
        isFocused = True
        Me.Invalidate()
    End Sub

    Private Sub txt_Leave(sender As Object, e As EventArgs) Handles txt.Leave
        isFocused = False
        Me.Invalidate()
    End Sub

    Private Sub txt_TextChanged(sender As Object, e As EventArgs) Handles txt.TextChanged
        UpdateClearButtonVisibility()
        RaiseEvent SearchTextChanged(Me, txt.Text)
        OnTextChanged(e)
    End Sub

    Private Sub txt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            PerformSearch()
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        PerformSearch()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Clear()
        txt.Focus()
        RaiseEvent ClearButtonClick(Me, EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        UpdateControlPositions()
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        txt.Font = Me.Font
        UpdateControlPositions()
    End Sub
End Class