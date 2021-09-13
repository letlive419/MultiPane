Imports System.IO
Public Class frmMain
    Private strFileName As String
    Private WithEvents Welcome As subfrmWelcome
    Private WithEvents Customer As subfrmCust
    Private WithEvents Order As subfrmOrder

    Private arrSubForms As ArrayList

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        arrSubForms = New ArrayList
        Welcome = New subfrmWelcome
        Customer = New subfrmCust
        Order = New subfrmOrder

        arrSubForms.Add(Welcome)
        arrSubForms.Add(Customer)
        arrSubForms.Add(Order)

        grpWorkArea.Controls.Add(Welcome)
        grpWorkArea.Controls.Add(Customer)
        grpWorkArea.Controls.Add(Order)

        Welcome.Location = CalcLocation(grpWorkArea, Welcome)
        Customer.Location = CalcLocation(grpWorkArea, Customer)
        Order.Location = CalcLocation(grpWorkArea, Order)

        Welcome.Visible = True
    End Sub
    Private Function CalcLocation(grpBox As GroupBox, subForm As UserControl) As Point
        Return New Point((grpBox.Width - subForm.Width) / 2, (grpBox.Height - subForm.Height) / 2)

    End Function
    Private Sub HideAllSubforms()
        For Each obj As UserControl In arrSubForms
            obj.Visible = False
        Next

    End Sub
    Private Sub OpenFile(strType As String)
        Dim intResult As Integer
        ofdData.InitialDirectory = Application.StartupPath
        ofdData.Filter = "All Files (*.*)|*.*|Text files (*.txt)|*.txt"
        ofdData.FilterIndex = 2
        Select Case strType
            Case "CustomerData"
                ofdData.Title = "Select Customer Data File"
            Case "OrderData"
                ofdData.Title = "Select Order Data File"
            Case Else
                MessageBox.Show("Unexpected data type in OpenFile", "Program error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Select
        intResult = ofdData.ShowDialog
        If intResult = DialogResult.Cancel Then
            Exit Sub
        End If
        strFileName = ofdData.FileName
        Try
            ReadInputFile(strFileName, strType)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try
    End Sub
    Public Sub ReadInputFile(strFileIn As String, strType As String)
        Dim fileIn As StreamReader
        Dim strLineIn As String
        Dim strFields() As String
        Dim i As Integer
        fileIn = New StreamReader(strFileIn)
        fileIn.ReadLine()
        Select Case strType
            Case "CustomerData"
                tvwCust.Nodes.Clear()
                For i = 65 To 90
                    tvwCust.Nodes.Add(Chr(i), Chr(i))
                Next
                Try
                    While Not fileIn.EndOfStream
                        strLineIn = fileIn.ReadLine
                        strFields = strLineIn.Split(",")
                        'Dim newNode As New TreeNode
                        'newNode.Text = strFields(1) & "," & strFields(2)
                        'newNode.Tag = strFields(0)
                        'tvwCust.Nodes.Add(newNode)
                        Dim parentNode() As TreeNode
                        parentNode = tvwCust.Nodes.Find(strFields(1).Substring(0, 1), True)
                        parentNode(0).Nodes.Add(strFields(0), strFields(1) & "," & strFields(2))
                    End While
                    tvwCust.ExpandAll()
                    tvwCust.Refresh()
                Catch ex As Exception
                    Throw ex
                End Try


            Case "OrderData"
                tvwOrders.Nodes.Clear()
                Try
                    While Not fileIn.EndOfStream
                        strLineIn = fileIn.ReadLine
                        strFields = strLineIn.Split(Chr(9))
                        tvwOrders.Nodes.Add(strFields(1), strFields(0))
                    End While
                    tvwOrders.ExpandAll()
                    tvwOrders.Refresh()

                Catch ex As Exception
                    Throw ex
                End Try


            Case Else
                MessageBox.Show("Unexpected file type")
        End Select
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        OpenFile("CustomerData")
        OpenFile("OrderData")
    End Sub

    Private Sub tvwCust_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvwCust.NodeMouseClick
        Dim theNode As TreeNode = DirectCast(e.Node, TreeNode)
        HideAllSubforms()
        Customer.txtID.Text = theNode.Name
        Customer.txtName.Text = theNode.Text
        Customer.Visible = True

    End Sub

    Private Sub tvwOrders_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvwOrders.NodeMouseClick
        Dim theNode As TreeNode = DirectCast(e.Node, TreeNode)
        HideAllSubforms()
        Order.txtOrderID.Text = theNode.Text
        Order.txtCustID.Text = theNode.Name

        Dim custNode() As TreeNode
        custNode = tvwCust.Nodes.Find(theNode.Name, True)
        Order.txtCustName.Text = custNode(0).Text
        Order.Visible = True

    End Sub

    Private Sub AddNewCust() Handles Customer.SaveCustomer
        Dim parentNode() As TreeNode
        parentNode = tvwCust.Nodes.Find(Customer.txtName.Text.Substring(0, 1), True)
        parentNode(0).Nodes.Add(Customer.txtID.Text, Customer.txtName.Text)

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub
End Class
