Public Class subfrmCust

    Public Delegate Sub SaveEvent()
    Public Event SaveCustomer As SaveEvent

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        RaiseEvent SaveCustomer()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtID.Clear()
        txtName.Clear()

    End Sub
End Class
