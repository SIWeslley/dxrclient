Imports System.IO
'Classe que contém a lógica para montagem do cabeçalho de um comando a ser enviado
Public Class dxrhead
    Shared ReadOnly baHeaderName As Byte() = {68, 88, 82, 32, 32, 32, 32, 32}
    Public dwDataLength As Integer
    Public byType As Byte
    Public byVersion As Byte
    Public wErrorCode As Short
    Public byNumberOfFiles As Byte = 0
    Public baUnused14 As Byte() = {120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120}
    Public byHeaderCheckSum As Byte = 0
    Shared ReadOnly DST_PACKET_LENGTH_OFFSET As Integer = 7
    Shared ReadOnly DST_DATA_LENGTH_OFFSET As Integer = 28
    Public baDSTHeader As Byte() = {70, 65, 78, 72,
    48, 48, 49,
    48, 48, 48, 48, 48,
    48, 48, 48, 48, 48,
    48, 48, 49,
    48, 48, 48, 48, 48,
    48, 48, 49,
    48, 48, 48, 48, 48,
    48, 48, 48, 48, 48,
    80, 83, 73, 32, 32,
    32, 32, 32, 32, 32,
    32, 32, 32,
    116, 64, 80, 83, 73,
    32, 32, 32, 32, 32,
    32, 32, 32, 32, 32,
    32, 32,
    48, 48, 48, 48,
    48, 48,
    48, 48,
    80, 83, 73, 68, 88,
    82, 87, 69, 66, 32,
    105, 110, 102, 111, 64, 112,
    115, 105, 97, 117, 115, 116,
    105, 110, 46, 99, 111, 109,
    32, 32, 32, 32, 32, 32,
    32, 32, 32, 32, 32, 32,
    32, 32, 32, 32, 32, 32,
    32, 32, 32, 32, 32, 32,
    32, 32, 32, 32, 32, 32,
    32, 32,
    32, 32, 32, 32, 32,
    32, 32, 32, 32, 32,
    78}

    Public Function makeHeader() As Byte()
        Dim header() As Byte
        Dim MemStream As MemoryStream = New MemoryStream()
        Dim BigEndianWriter As New BigBinaryWriter(MemStream)
        BigEndianWriter.Write(baHeaderName)
        BigEndianWriter.WriteInt(dwDataLength)
        BigEndianWriter.Write(byType)
        BigEndianWriter.Write(byVersion)
        BigEndianWriter.WriteShort(wErrorCode)
        BigEndianWriter.Write(byNumberOfFiles)
        BigEndianWriter.Write(baUnused14)
        BigEndianWriter.Write(byHeaderCheckSum)
        header = MemStream.ToArray
        Return header
    End Function

End Class