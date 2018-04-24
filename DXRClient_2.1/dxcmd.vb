Imports System.Text
'Classe que encapsula a lógica de montagem de um comando
Public Class DXCommand
    Public Command As String
    Private params As New Parameters
    Public inDates(1) As Date
    Private qDates As String()
    Const qt As String = """"
    Private ReadOnly exitformats As New Dictionary(Of String, String) From {
        {"PDF", "/PF /AP"}, {"TXT", "/TP /AP"}, {"XML", "/XM /AP"}, {"NO_EXPORT", " /SL"}, {"AFP", "/GO /AP"}, {"RESULT_LINE", "/SL/RL"}}

    Public Sub New(ByVal pDraw As String, ByVal pFolder As String, ByVal pReport As String, ByVal pPage As String, ByVal pExitFormat As String, ByVal pFileName As String)
        params.Drawer = qt + pDraw + qt
        params.filenm = pFileName
        params.Folder = qt + pFolder + qt
        params.Page = qt + pPage + qt
        params.Report = qt + pReport.Replace(",", """,""") + qt
        params.exitFormat = pExitFormat
        PageSearchCommand()
    End Sub

    Public Sub New(ByVal pDraw As String, ByVal pFolder As String, ByVal pReport As String, ByVal pIndex As String, ByVal pCrit As String, ByVal pIndexOperator As String, ByVal pExitFormat As String, ByVal pFileName As String, ByVal dtarray As Date())
        inDates = dtarray
        params.Drawer = qt + pDraw + qt
        params.filenm = pFileName
        params.Folder = qt + pFolder + qt
        params.Index = qt + pIndex.Replace("|", """,""") + qt
        params.Report = qt + pReport.Replace("|", """,""") + qt
        params.Criteria = qt + pCrit.Replace("|", """,""") + qt
        If Not pIndexOperator Is Nothing Then
            params.IndexOperator = qt + pIndexOperator.Replace("|", """,""") + qt
        End If
        params.exitFormat = pExitFormat
        qDates = {qt + inDates(0).ToString("yyyy-MM-dd") + qt, qt + inDates(1).ToString("yyyy-MM-dd") + qt}
        IndexCriteriaSearchCommand()
    End Sub

    Private Function IndexCriteriaSearchCommand()
        Dim sb As New StringBuilder()
        sb.Append("/DR").Append(params.Drawer).Append("/FF").Append(params.Folder)
        If params.Report.Replace(qt, "") = "AllReports" Then
            sb.Append("/MR")
        ElseIf inDates(0) = Nothing Or inDates(1) = Nothing Then
            sb.Append("/LR").Append(params.Report)
        Else
            sb.Append("/FR").Append(qDates(0)).Append("-").Append(qDates(1))
        End If
        sb.Append(" /IH").Append(params.Index).Append(" /KW").Append(params.Criteria)
        If Not params.IndexOperator Is Nothing Then
            sb.Append(" /IO").Append(params.IndexOperator)
        End If
        If params.exitFormat = "NO_EXPORT" Then
            sb.Append(" /IS")
            sb.Append(params.Index)
        End If
        sb.Append(getExportString(params.exitFormat))
        Command = sb.ToString
        Return True
    End Function

    Private Function PageSearchCommand()
        Dim sb As New StringBuilder()
        sb.Append("/DR").Append(params.Drawer).Append("/FF").Append(params.Folder)
        If params.Report.Replace(qt, "") = "AllReports" Then
            sb.Append("/MR")
        Else
            sb.Append("/LR").Append(params.Report)
        End If
        sb.Append(" /PN").Append(params.Page)
        If params.exitFormat = "NO_EXPORT" Then
            sb.Append(" /IS")
            sb.Append(params.Index)
        End If
        sb.Append(getExportString(params.exitFormat))
        Command = sb.ToString
        Return True
    End Function

    Private Function getExportString(ByVal val As String) As String
        Select Case val
            Case "PNG"
                Return (" /CI " + params.filenm + " /IT43 /IB24 /SR100 /AP")
            Case "CSV"
                Return (" /CV " + params.filenm.Replace(".png", ".csv") + " /AP")
            Case Else
                Return exitformats.Item(val)
        End Select
    End Function
End Class
