Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class CoreMethods

    'Objeto que encapsula os atributos para a conexão com DXServer (Ip, número de tentativas para conexão, timeout e a porta)
    Private dxserverSpec As New DXServerSpec
    'Instância da class DXConnect(que faz a conexão com o DXServer) a ser usada pelos métodos
    Public dxconnect As New DXConnect
    Private ExportDir As String
    Private dxoutput As New DXOutputProcessing
    Private pdfRepoDir As String
    Private regExTemplates As Dictionary(Of String, String)
    Private DicPDFRepoDir As Dictionary(Of String, String) = New Dictionary(Of String, String)
    'Função que lê o valor da porta no arquivo DXSERVER.CFG
    Private Function setEnv() As Int32
        dxserverSpec.Port = 2000
        dxserverSpec.IP = "localhost"
        Try
            Using filereader As New StreamReader(My.Application.Info.DirectoryPath + "\dxserver.cfg")
                Dim line As String
                Dim values As String()
                line = filereader.ReadLine()
                Do While (Not line Is Nothing)
                    If line.ToUpper.Contains("PORT=") And Not line.StartsWith(";") Then
                        values = Regex.Split(line, "\=")
                        dxserverSpec.Port = Convert.ToInt32(values(1))
                    End If
                    If line.ToUpper.Contains("IPADDR=") And Not line.StartsWith(";") Then
                        values = Regex.Split(line, "\=")
                        dxserverSpec.IP = values(1)
                    End If
                    If line.ToUpper.Contains("DIRLOG=") And Not line.StartsWith(";") Then
                        values = Regex.Split(line, "\=")
                        Logger.dirlog = values(1) + "\\"
                    End If
                    If line.ToUpper.StartsWith("PDFREPODIR") And Not line.StartsWith(";") Then
                        values = line.Split("=")
                        DicPDFRepoDir.Add(values(0).Split("_").GetValue(1).ToString.ToUpper, values(1))
                    End If
                    If line.ToUpper.Contains("LAYOUTCFGFILE=") And Not line.StartsWith(";") Then
                        values = Regex.Split(line, "\=")
                        Dim layoutFile As String = values(1).ToUpper().Replace(" ", "")
                        Dim path As String = My.Application.Info.DirectoryPath + "\"
                        If layoutFile.ToUpper.StartsWith("C:\") Or layoutFile.ToUpper.StartsWith("D:\") Then
                            path = ""
                        End If
                        Try
                            Using fReader As New StreamReader(path + layoutFile)
                                Dim layoutLine = fReader.ReadLine()
                                regExTemplates = New Dictionary(Of String, String)
                                Do While (Not layoutLine Is Nothing)
                                    If Not layoutLine.StartsWith(";") Then
                                        regExTemplates.Add(layoutLine.Split("=").GetValue(0).ToString.Trim, layoutLine.Split("=").GetValue(1).ToString.Trim)
                                    End If
                                    layoutLine = fReader.ReadLine()
                                Loop
                            End Using
                        Catch ex As Exception
                            Logger.WriteToLog(ex.Message)
                        End Try
                    End If
                    line = filereader.ReadLine()
                Loop
            End Using
        Catch ex As Exception
            Logger.WriteToLog(ex.Message)
        End Try
        Return 0
    End Function

    'Construtores dos atributos de conexão e o diretório para exportação no caso de um arquivo png ou csv(para os métodos que não recebem o caminho do arquivo)
    Public Sub New()
        dxserverSpec.Timeout = 30 * 1000
        dxserverSpec.Tries = 10
        setEnv()
        ExportDir = Directory.GetCurrentDirectory.Replace("/", "\")
    End Sub

    Public Sub New(ByVal pTimeout As Int32, ByVal pTry As Int32, ByVal pDir As String)
        dxserverSpec.Timeout = pTimeout * 1000
        dxserverSpec.Tries = pTry
        setEnv()
        ExportDir = pDir
    End Sub

    Public Sub New(ByVal pTimeout As Int32, ByVal pTry As Int32)
        dxserverSpec.Timeout = pTimeout * 1000
        dxserverSpec.Tries = pTry
        setEnv()
        ExportDir = Directory.GetCurrentDirectory.Replace("/", "\")
    End Sub

    Public Sub New(ByVal pIP As String, ByVal pPort As Int32, ByVal pTimeout As Int32, ByVal pTry As Int32, ByVal pDir As String)
        dxserverSpec.IP = pIP
        dxserverSpec.Port = pPort
        dxserverSpec.Timeout = pTimeout * 1000
        dxserverSpec.Tries = pTry
        ExportDir = pDir
    End Sub

    Public Function Search(ByVal gaveta As String, ByVal pasta As String, ByVal relatorio As String, ByVal nomeIndices As String, ByVal criterios As String, ByVal indexOperator As String, ByVal DataInicial As Date, ByVal DataFinal As Date) As DataTable
        Dim rByte() As Byte = Nothing
        Try
            Dim dxcmd As New DXCommand(gaveta, pasta, relatorio, nomeIndices, criterios, indexOperator, "NO_EXPORT", "", {DataInicial, DataFinal})
            rByte = dxconnect.sendCommand(dxserverSpec, dxcmd.Command)
            Return dxoutput.BuildDataTable(Encoding.Default.GetString(rByte))
        Catch ex As DXServerErrorException
            If dxconnect.mErrorCode <> 11 Then
                Logger.WriteToLog(ex.Message)
            End If
            Return Nothing
        Catch ex As DXConnectionFailedException
            Logger.WriteToLog(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function mGet(ByVal gaveta As String, ByVal pasta As String, ByVal relatorio As String, ByVal pagina As String, ByVal formato_saida As String) As String
        Dim rByte() As Byte = Nothing
        Dim filepath As String = ExportDir + "\DXServer" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "\extrato.png"
        Try
            Dim dxcmd As New DXCommand(gaveta, pasta, relatorio, pagina, formato_saida, filepath)
            rByte = dxconnect.sendCommand(dxserverSpec, dxcmd.Command)
            If (formato_saida.ToUpper = "PDF" Or formato_saida.ToUpper = "AFP") Then
                Return Convert.ToBase64String(rByte)
            Else
                Return Encoding.Default.GetString(rByte)
            End If
        Catch ex As DXServerErrorException
            If dxconnect.mErrorCode <> 11 Then
                Logger.WriteToLog(ex.Message)
            End If
            Return Nothing
        Catch ex As DXConnectionFailedException
            Logger.WriteToLog(ex.Message)
            Return Nothing
        Catch ex As KeyNotFoundException
            Logger.WriteToLog("ERRO:Formato " + formato_saida + " inválido")
            Return Nothing
        End Try
    End Function

    Public Function GetWriteToFile(ByVal gaveta As String, ByVal pasta As String, ByVal relatorio As String, ByVal pagina As String, ByVal formato_saida As String, ByVal arquivo As String) As Boolean
        Dim rByte() As Byte
        Try
            Dim dxcmd As New DXCommand(gaveta, pasta, relatorio, pagina, formato_saida, arquivo)
            rByte = dxconnect.sendCommand(dxserverSpec, dxcmd.Command)
            If Not formato_saida = "PNG" And Not formato_saida = "CSV" Then
                Dim fs As New FileStream(arquivo, FileMode.Create)
                fs.Write(rByte, 0, rByte.Length)
                fs.Close()
            End If
            Return True
        Catch ex As DXServerErrorException
            If dxconnect.mErrorCode <> 11 Then
                Logger.WriteToLog(ex.Message)
            End If
            Return False
        Catch ex As DXConnectionFailedException
            Logger.WriteToLog(ex.Message)
            Return False
        Catch ex As IOException
            Logger.WriteToLog("ERRO:Erro ao gravar:" + arquivo)
            Logger.WriteToLog(ex.Message)
            Return False
        Catch ex As KeyNotFoundException
            Logger.WriteToLog("ERRO:Formato " + formato_saida + " inválido")
            Return False
        End Try
    End Function

    Public Function SearchGet(ByVal gaveta As String, ByVal pasta As String, ByVal relatorio As String, ByVal nomeIndices As String, ByVal criterios As String, ByVal indexOperator As String, ByVal formato_saida As String, ByVal DataInicial As Date, ByVal DataFinal As Date) As String
        Dim rByte() As Byte = Nothing
        Dim resultLine As String = Nothing
        Dim filepath As String = ExportDir + "\DXServer" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "\extrato.png"
        Try
            Dim dxcmd As New DXCommand(gaveta, pasta, relatorio, nomeIndices, criterios, indexOperator, formato_saida, filepath, {DataInicial, DataFinal})
            rByte = dxconnect.sendCommand(dxserverSpec, dxcmd.Command)
            If (formato_saida.ToUpper = "PDF" Or formato_saida.ToUpper = "AFP") Then
                Return Convert.ToBase64String(rByte)
            Else
                If (formato_saida.ToUpper = "RESULT_LINE") Then
                    setPDFRepositoryDir(gaveta)
                    Return dxoutput.cleanResultLines(Encoding.Default.GetString(rByte))
                End If
            End If
        Catch ex As DXServerErrorException
            If dxconnect.mErrorCode <> 11 Then
                Logger.WriteToLog(ex.Message)
            End If
            Return Nothing
        Catch ex As DXConnectionFailedException
            Logger.WriteToLog(ex.Message)
            Return Nothing
        Catch ex As KeyNotFoundException
            Logger.WriteToLog("Core.SearchGet() > ERRO:Formato " + formato_saida + " inválido")
            Return Nothing
        End Try
    End Function

    Public Function SearchGetWriteToFile(ByVal gaveta As String, ByVal pasta As String, ByVal relatorio As String, ByVal nomeIndices As String, ByVal criterios As String, ByVal indexOperator As String, ByVal formato_saida As String, ByVal arquivo As String, ByVal DataInicial As Date, ByVal DataFinal As Date) As Boolean
        Dim rByte() As Byte
        Try
            Dim dxcmd As New DXCommand(gaveta, pasta, relatorio, nomeIndices, criterios, indexOperator, formato_saida, arquivo, {DataInicial, DataFinal})
            rByte = dxconnect.sendCommand(dxserverSpec, dxcmd.Command)
            If Not formato_saida = "PNG" And Not formato_saida = "CSV" Then
                Dim fs As New FileStream(arquivo, FileMode.Create)
                fs.Write(rByte, 0, rByte.Length)
                fs.Close()
            End If
            Return True
        Catch ex As DXServerErrorException
            If dxconnect.mErrorCode <> 11 Then
                Logger.WriteToLog(ex.Message)
            End If
            Return False
        Catch ex As DXConnectionFailedException
            Logger.WriteToLog(ex.Message)
            Return False
        Catch ex As IOException
            Logger.WriteToLog("ERRO:Erro ao gravar:" + arquivo)
            Logger.WriteToLog(ex.Message)
            Return False
        Catch ex As KeyNotFoundException
            Logger.WriteToLog("ERRO:Formato " + formato_saida + " inválido")
            Return False
        End Try
    End Function

    Public Function sendCommandToDXServer(ByVal command As String) As Byte()
        Dim rByte() As Byte = Nothing
        Try
            rByte = dxconnect.sendCommand(dxserverSpec, command)
            Return rByte
        Catch ex As DXServerErrorException
            If dxconnect.mErrorCode <> 11 Then
                Logger.WriteToLog(ex.Message)
            End If
            Return Nothing
        Catch ex As DXConnectionFailedException
            Logger.WriteToLog(ex.Message)
            Return Nothing
        End Try
    End Function
    Private Sub setPDFRepositoryDir(ByVal gaveta As String)
        Dim path As String

        If Not DicPDFRepoDir.ContainsKey(gaveta.ToUpper) Then
            Logger.WriteToLog("Core.setPDFRepositoryDir() > Repositório PDF não encontrado para a gaveta '" + gaveta + "' no arquivo dxserver.cfg")
            Return
        End If

        DicPDFRepoDir.TryGetValue(gaveta, path)
        If Path.EndsWith("\") Then
            pdfRepoDir = Path
        Else
            pdfRepoDir = Path + "\"
        End If
    End Sub
    Public Function getPDFRepositoryDir() As String
        Return pdfRepoDir
    End Function

    Public Function getRegExpTemplatesList() As Dictionary(Of String, String)
        Return regExTemplates
    End Function
End Class