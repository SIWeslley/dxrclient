Imports System.Text
Imports System.Text.RegularExpressions

Public Class DXOutputProcessing
    Private lines As String()
    Private columns As String()
    Private colsLineIndex = 1
    Private fLineIndex = 2

    Public Function BuildDataTable(ByVal input As String) As DataTable
        lines = input.Split(New String() {Environment.NewLine},
          StringSplitOptions.RemoveEmptyEntries)
        'printArray(rep)
        'printArray(lines
        For i As Int32 = 0 To lines.Length - 1
            If (lines(i).EndsWith("♀")) Then
                colsLineIndex = i + 1
                fLineIndex = i + 2
                Exit For
            End If
        Next
        columns = Regex.Split(lines(colsLineIndex), "\t+")
        If lines(fLineIndex).Split(New String() {Chr(9)},
          StringSplitOptions.RemoveEmptyEntries)(columns.Length - 2).Contains(":") Then
            Return buildTable()
        Else
            Return buildSimpleTable()
        End If
    End Function

    Private Function buildTable() As DataTable
        Dim table As New DataTable
        For i As Int32 = 0 To columns.Length - 3
            table.Columns.Add(columns(i), GetType(String))
        Next
        table.Columns.Add("Page", GetType(String))
        table.Columns.Add("Begin", GetType(String))
        table.Columns.Add("End", GetType(String))
        table.Columns.Add("Report", GetType(String))
        'printArray(columns)
        For i As Int32 = fLineIndex To lines.Length - 1
            Dim k = 0
            Dim row As DataRow
            row = table.NewRow()
            Dim values As String()
            values = lines(i).Split(New String() {Chr(9)},
          StringSplitOptions.RemoveEmptyEntries)
            'printArray(values)
            Do While k < values.Length
                row(columns(k)) = values(k)
                k += 1
            Loop
            Dim s As String = row("Page")
            s.Replace(" ", "")
            Dim pages As String() = Regex.Split(s, ":|of")
            row("Page") = pages(0)
            row("Begin") = pages(1)
            row("End") = pages(2)
            table.Rows.Add(row)
        Next

        Return table
    End Function

    Private Function buildSimpleTable() As DataTable
        Dim table As New DataTable
        For i As Int32 = 0 To columns.Length - 1
            table.Columns.Add(columns(i), GetType(String))
        Next
        'printArray(columns)
        For i As Int32 = fLineIndex To lines.Length - 1
            Dim k = 0
            Dim row As DataRow
            row = table.NewRow()
            Dim values As String()
            values = lines(i).Split(New String() {Chr(9)},
          StringSplitOptions.RemoveEmptyEntries)
            'printArray(values)
            Do While k < values.Length
                row(columns(k)) = values(k)
                k += 1
            Loop
            table.Rows.Add(row)
        Next
        Return table

    End Function
    Friend Function cleanResultLines(ByVal resultLines As String) As String
        'Retira as 2 primeiras linhas do retorno
        Dim indexReport As Int32 = resultLines.IndexOf("Report" + Environment.NewLine) + 8
        Dim outuputClean As String = resultLines.Substring(indexReport)
        'Joga as linhas em um array
        Dim lines As String() = outuputClean.Split(New String() {Environment.NewLine},
                                    StringSplitOptions.RemoveEmptyEntries)
        Dim lineResult As String
        Dim lineResultTemp As String()
        'Pega apenas as linhas, removendo informações de página e nome do relatório
        For i As Int32 = 0 To lines.Length - 1
            lineResultTemp = Regex.Split(lines(i), "\t+")
            lineResult += lineResultTemp(0) + Environment.NewLine
        Next
        'Remove 'CRLF' da última linha
        Return lineResult.Substring(0, lineResult.Length - 2)
    End Function


    Private Function printArray(ByVal arr As String()) As Boolean
        For i As Int32 = 0 To arr.Length - 1
            Console.WriteLine(arr(i) + "|")
        Next
        Return True
    End Function

End Class
