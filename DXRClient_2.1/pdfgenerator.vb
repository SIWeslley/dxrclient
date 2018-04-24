Imports iText.Forms
Imports iText.Forms.Fields
Imports iText.IO.Source
Imports iText.Kernel.Pdf
Imports System.IO

Public Class PDFGenerator

    Public Shared Function createPDF(ByVal documentos As List(Of Documento), ByVal pdfDir As String) As String
        Dim tmp As ByteArrayOutputStream
        Dim pdfResultante As New MemoryStream()
        Dim pdfDocumentR As PdfDocument
        Dim pdfDocumentW As New PdfDocument(New PdfWriter(pdfResultante))
        Dim template, templateId As String

        Dim i As Int16 = 0
        For Each documento As Documento In documentos
            Console.WriteLine("Lendo objeto")
            tmp = New ByteArrayOutputStream()
            documento.campos.TryGetValue("templateID", templateId)
            template = pdfDir + "template_" + templateId + ".pdf"

            If Not File.Exists(template) Then
                Logger.WriteToLog("PDFGenerator.createPDF() > Não foi encontrado o template PDF '" + template + "'")
                Return Nothing
            End If

            'Remove o 'templateID' pois ele não é exibido no PDF
            documento.campos.Remove("templateID")
            pdfDocumentR = New PdfDocument(New PdfReader(template), New PdfWriter(tmp))
            Dim form As PdfAcroForm = PdfAcroForm.GetAcroForm(pdfDocumentR, False)
            Dim fields As IDictionary(Of String, PdfFormField) = form.GetFormFields()
            Dim tf As PdfFormField

            'Recupera as chaves
            Dim keys As Dictionary(Of String, String).KeyCollection = documento.campos.Keys
            Dim value As String
            For Each key As String In keys
                documento.campos.TryGetValue(key, value)
                fields.TryGetValue(key, tf)
                tf.SetValue(value)
            Next

            'form.FlattenFields()
            pdfDocumentR.Close()

            pdfDocumentR = New PdfDocument(New PdfReader(New MemoryStream(tmp.GetBuffer())))
            pdfDocumentR.CopyPagesTo(1, pdfDocumentR.GetNumberOfPages(), pdfDocumentW, New PdfPageFormCopier())
            pdfDocumentR.Close()
        Next
        Console.WriteLine("PDF criado")
        Console.ReadKey()
        pdfDocumentW.Close()
        Return Convert.ToBase64String(pdfResultante.ToArray())
    End Function

End Class
