Imports System.IO
Imports System.Text

Public Class Logger
    Public Shared dirlog = My.Application.Info.DirectoryPath + "\\"
    Public Shared filename = "DXLog.txt"
    Public Shared Function WriteToLog(ByVal text As String) As Boolean
        Dim now As DateTime = DateTime.Now
        Dim fs As New FileStream(dirlog + filename, FileMode.Append)
        Dim dtBytes As Byte() = Encoding.Default.GetBytes(now.ToString("yyyy-MM-dd-hh-mm-ss:"))
        Dim textBytes As Byte() = Encoding.Default.GetBytes(text)
        fs.Write(dtBytes, 0, dtBytes.Length)
        fs.Write(textBytes, 0, textBytes.Length)
        fs.Write(Encoding.Default.GetBytes(Environment.NewLine), 0, 2)
        fs.close()
        Return True
    End Function
End Class
