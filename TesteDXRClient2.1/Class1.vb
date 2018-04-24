'Imports iTextSharp
'Imports iTextSharp.text.pdf
Imports iText.Forms
Imports iText.Forms.Fields
Imports iText.IO.Source
Imports iText.Kernel.Pdf
Imports System.IO
Imports System.Dynamic
Imports dxrclient
Imports System.Text.RegularExpressions

Namespace TesteDXRClient21
    Public Class Class1

        Public Shared Sub Main1()
            Dim SRC As String = "C:\\PDFTemplates\\template_001.pdf"
            Dim DEST As String = "C:\\PDFTemplates\\out.pdf"
            'Dim output As String = Nothing
            Dim output As String
            Dim bool As Boolean = False
            Dim dtinit As Date = New DateTime(2014, 10, 1)
            Dim dtfinal As Date = New DateTime(2014, 12, 1)
            Dim core As New CoreMethods(20, 10, "C:\dxexport")
            Dim regExpTemplateList As Dictionary(Of String, String)
            'Console.WriteLine("Saida:")
            output = core.SearchGet("Teste", "2010-05", "2010-05-01", "Agencia Debito|Conta Debito", "0284|12799-0", Nothing, "RESULT_LINE", Nothing, Nothing)
            'Console.WriteLine("resultado retornado")
            'output = "001 01405 8533 07001-015/02/2011SERGIO GOMES VITAL                                0043411000010163X0          4146440504600000A00ASSOC RECREATIVA DESP ROCK    10.377.063/0001-15215/02/2011341853306:35:30BANKLINE                                    2.500,007224D1919F03E51079A387B70385A27CFCF49538                        0015/02/20118533 12285-2100"
            regExpTemplateList = core.getRegExpTemplatesList()

            If output Is Nothing Then
                Console.WriteLine("output nulo")
            Else
                Console.WriteLine("output: " + output)
            End If
            Console.ReadKey()

            'Dim documentos As List(Of Documento) = Documento.ParseLinesPDF(output, regExpTemplateList)

            'Console.WriteLine("Retorno: " + output)
            'Dim baos As ByteArrayOutputStream = PDFGenerator.createPDF(comprovantes, SRC)
            'Dim str As String = PDFGenerator.createPDF(documentos, core.getPDFRepositoryDir())
            'File.WriteAllBytes(DEST, Convert.FromBase64String(str))
            'Console.WriteLine("Gravei PDF")
            Dim xml As String = Documento.ParseLinesXML(output, regExpTemplateList)
            'Dim fs As File = File("C:\dxexport", FileMode.Create)
            File.WriteAllText("C:\PDFTemplates\saida.txt", xml)
            Console.WriteLine("xml criado")

            Console.ReadKey()

            'For Each linha As String In lines
            'Dim pdfDocumentR As PdfDocument
            'Dim writer As PdfWriter = New PdfWriter(DEST)
            'writer.SetSmartMode(True)
            'Dim pdfDocumentW As New PdfDocument(writer)
            'Dim tmp As ByteArrayOutputStream
            'Dim form As PdfAcroForm
            'Dim fields As IDictionary(Of String, PdfFormField)
            'Dim tf As PdfFormField

            'For Each documento As Documento In documentos
            'Console.WriteLine("Lendo linha")
            'tmp = New ByteArrayOutputStream()
            'pdfDocumentR = New PdfDocument(New PdfReader(SRC), New PdfWriter(tmp))
            'form = PdfAcroForm.GetAcroForm(pdfDocumentR, False)
            'Fields = form.GetFormFields()

            'Dim i As Int16 = 0
            'For Each c As Object In documento
            'Console.WriteLine("Lendo objeto" + i.ToString())

            'Documento.campos.Remove("templateID")
            'Dim keys As Dictionary(Of String, String).KeyCollection = documento.campos.Keys
            'For Each key As String In keys
            'Dim value As String
            'Documento.campos.TryGetValue(key, value)
            'Console.WriteLine("Chave '{0}', Valor '{1}'", key, value)

            'Fields.TryGetValue(key, tf)
            'tf.SetValue(value)
            'Next
            'i = i + 1
            'Next

            'Fields.TryGetValue("nome", tf)
            'tf.SetValue(line.Substring(29, 33))
            'Fields.TryGetValue("cpf", tf)
            'tf.SetValue(line.Substring(0, 11))
            'Fields.TryGetValue("rg", tf)
            'tf.SetValue(line.Substring(12, 19))
            'form.FlattenFields()
            'pdfDocumentR.Close()

            'pdfDocumentR = New PdfDocument(New PdfReader(New MemoryStream(tmp.GetBuffer())))
            '    pdfDocumentR.CopyPagesTo(1, pdfDocumentR.GetNumberOfPages(), pdfDocumentW, New PdfPageFormCopier())
            'pdfDocumentR.Close()
            'Next
            'Console.WriteLine("PDF criado")
            'Console.ReadKey()
            'pdfDocumentW.Close()

            'Retira as 2 primeiras linhas do retorno
            'Dim indexReport As Int32 = output.IndexOf("Report" + Environment.NewLine) + 8
            'Dim outuputClean As String = output.Substring(indexReport)
            'Dim lines As String() = outuputClean.Split(New String() {Environment.NewLine},
            'StringSplitOptions.RemoveEmptyEntries)
            'Dim lineResult As String = Nothing
            'Dim lineResultTemp As String()
            'For i As Int32 = 0 To lines.Length - 1
            'lineResultTemp = Regex.Split(lines(i), "\t+")
            'lineResult += lineResultTemp(0) + Environment.NewLine
            'Next
            'Console.WriteLine(lineResult)
            'Console.ReadKey()

            'Dim teste As Class1 = New Class1()
            'teste.manipulatePdf(teste.SRC, teste.DEST)
            'manipulatePdf(SRC, DEST)
            'htmltest(c)
        End Sub

        Public Sub manipulatePdf(src As String, dest As String)
            Console.WriteLine("Reading template form...")
            'Dim reader = New PdfReader(src)
            'Dim fs = New FileStream(dest, FileMode.Create)
            'Dim stamper = New PdfStamper(reader, fs)
            'Dim form = stamper.AcroFields
            'Console.WriteLine("Setting Fields...")
            'form.SetField("nome", "Rafael Verceze")
            'form.SetField("cpf", "427.645.098-55")
            'form.SetField("rg", "47.816.883-4")
            'stamper.Close()
            'Console.WriteLine("Done!")
        End Sub

        Public Sub htmltest()
            'DXWcfClient client = New DXWcfClient();
            'Byte[] ba = client.GetHTMLDocument1(c);
            'File.WriteAllBytes(@"C:\\XSLTemplates\\out.html", ba);
        End Sub


    End Class
End Namespace