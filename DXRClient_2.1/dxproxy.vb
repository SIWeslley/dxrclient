Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class DXConnect
    Public mErrorCode As Short = 0

    'Método que abre um socket no DXServer envia um command e recebe a reposta
    Public Function sendCommand(ByVal dxserverspec As DXServerSpec, ByVal command As String) As Byte()
        Dim readFailed As Boolean = True
        Dim serialcmd() As Byte
        Dim TcpCli As New TcpClient()
        Dim NetStream As NetworkStream
        Dim errcode As Short
        TcpCli.SendTimeout = dxserverspec.Timeout
        TcpCli.ReceiveTimeout = dxserverspec.Timeout
        'Tenta-se conectar-se ao servidor, o numero de vezes especificado
        For i As Integer = 1 To dxserverspec.Tries
            Try
                'Logger.WriteToLog("INFO:Conectando em " + dxserverspec.IP.ToString + ":" + dxserverspec.Port.ToString)
                TcpCli.Connect(dxserverspec.IP, dxserverspec.Port)
                Exit For
            Catch ex As Exception
                Logger.WriteToLog("ERRO:" + ex.Message)
                Thread.Sleep(2000)
            End Try
        Next
        If Not TcpCli.Connected Then
            Throw New DXConnectionFailedException("ERRO:Falha ao conectar em " + dxserverspec.IP.ToString + " depois de " + dxserverspec.Tries.ToString + " tentativas.")
        End If
        'Serializa o comando em uma array de bytes
        serialcmd = serializeCMD(command)
        NetStream = TcpCli.GetStream()
        'Logger.WriteToLog("INFO:Enviando comando:" + command)
        'Escreve o comando na stream do socket
        NetStream.Write(serialcmd, 0, serialcmd.Length)
        NetStream = TcpCli.GetStream()
        NetStream.Flush()
        'Declara um leitor BigEndian
        Dim BigReader As New BigBinaryReader(NetStream)
        Dim rlen As Int32 = 0
        'Lê os 8 primeiros bytes 
        For j As Int32 = 1 To 5
            Try
                BigReader.ReadBytes(8)
                'Bytes 7 ao 11 contêm o tamanho do retorno como um inteiro 32 bits em BigEndian
                rlen = BigReader.ReadUInt32()
                BigReader.ReadBytes(2)
                'Bytes 14 ao 15 contêm o código do erro, 0 significa que não houve erro
                errcode = BigReader.ReadInt16
                readFailed = False
                Exit For
            Catch ex As IOException
                Logger.WriteToLog("ERRO:Excessão de E/S:" + ex.Message)
                NetStream.Position = 0
            Catch ex As Exception
                Logger.WriteToLog("ERRO:" + ex.Message)
                NetStream.Position = 0
            End Try
        Next
        If readFailed Then
            Throw New DXServerErrorException("ERRO:Não foi possivel ler o retorno do servidor depois de 5 tentativas.")
        End If
        mErrorCode = errcode
        If rlen = 0 And errcode = 0 Then
            'Se o tamanho do retorno for igual a zero e não houve erros significa que se trata de um comando que não gera retorno(como uma exportação em PNG)
            Return Encoding.Default.GetBytes("INFO:Não há retorno do servidor.")
        End If
        If errcode <> 0 Then
            'Se o código de erro for diferente de 0 significa que houve um erro no servidor
            BigReader.Close()

            Throw New DXServerErrorException("ERRO:DXServer retornou erro " + errcode.ToString + ":" + DXServerErrorException.errorMessage(errcode) + " ao executar o comando: " + command)
        End If
        'Lê mais 16 bytes
        BigReader.ReadBytes(16)
        Dim databuff(rlen) As Byte
        'Lê o retorno e armazena em uma array de bytes
        databuff = BigReader.ReadBytes(databuff.Length)
        BigReader.Close()
        Return databuff
    End Function

    'Esse método codifica o comando em uma array de bytes e concatena com o cabeçalho
    Private Function serializeCMD(ByVal cmd As String) As Byte()
        Dim dxrh As New dxrhead
        dxrh.dwDataLength = cmd.Length
        dxrh.byType = 0
        dxrh.byVersion = 1
        dxrh.byHeaderCheckSum = 0
        dxrh.wErrorCode = 0
        Dim buffer() As Byte
        buffer = dxrh.makeHeader
        Dim byteCMD() As Byte
        byteCMD = Encoding.Default.GetBytes(cmd)
        Array.Resize(buffer, 32 + byteCMD.Length)
        Array.Copy(byteCMD, 0, buffer, 32, byteCMD.Length)
        Return buffer
    End Function
End Class
