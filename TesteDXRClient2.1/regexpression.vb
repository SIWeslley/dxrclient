'Imports iTextSharp
'Imports iTextSharp.text.pdf
Imports iText.Forms
Imports dxrclient
Imports System.Text.RegularExpressions
Imports System.IO

Namespace TesteDXRClient21
    Public Class regxpression

        Public Shared Sub Main()
            'Dim pattern As String = "^(?<nome>.{20})(?<data>.{10})"
            'Dim pattern As String = "^.{10}(?<Agencia>.{4}).{1}(?<Conta>.{7})(?<DiaOperacao>.{2}).{231}(?<Hash>.{40}).{41}(?<CEP>.{9})"
            'Dim pattern As String = "^.{5}(?<Teste>.{5})(?<Conta><Agencia>.{4}.{1}<NroConta>.{7})"
            '
            'Dim str As String = "alvaro monteiro de s12/09/1982"
            'Dim str As String = "003 01405 0036 57585-015/02/2011PAULO VIETRI                  ATIMO-Fabrica       0043411000010813X0          4191940504600000C00ATIMO COM REPR E SERVICOS LTDA68.223.189/0001-15215/02/2011341003606:36:32BANKLINE                                    6.886,60BAFC50DA5889E66CB0090499C0207E28617F1416                        0015/02/20110036 63302-2100"
            'Dim xml As String = "<?xml version=""1.0"" encoding=""UTF-8""?>" + vbCrLf + "<documento>" + vbCrLf

            'Console.WriteLine("iniciando")
            'Console.Read()
            'Dim regexp As Regex = New Regex(pattern)
            'For Each match As Match In regexp.Matches(str)
            'Console.WriteLine("for")
            'Console.Read()
            'Dim collection As GroupCollection = match.Groups
            'For groupCtr As Integer = 1 To match.Groups.Count - 1
            'For groupCtr As Integer = 1 To collection.Count
            'Dim group As Group = match.Groups(groupCtr)
            'Dim group As Group = collection(groupCtr)
            'Xml += "<" + regexp.GroupNameFromNumber(groupCtr) + ">" + group.Value + "</" + regexp.GroupNameFromNumber(groupCtr) + ">" + vbCrLf
            'Console.WriteLine("   Group {0}: {1}", groupCtr, group.Value)
            'Console.ReadLine()
            'For captureCtr As Integer = 0 To group.Captures.Count - 1
            'Console.WriteLine("      Capture {0}: ", group.Captures(captureCtr).Value)
            'Console.Read()
            'Next
            'Next
            'Next
            'Xml += "</documento>"
            'Console.WriteLine(xml)
            'Console.ReadLine()

            Try
                Dim file As String = "\\sup03\temp\XML_Analyze(iconproc.dat).txt"
                Using fReader As New StreamReader(file)
                    Dim layoutLine = fReader.ReadLine()
                    Do While (Not layoutLine Is Nothing)
                        Console.WriteLine(fReader.ReadLine())
                    Loop
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        End Sub

    End Class
End Namespace
