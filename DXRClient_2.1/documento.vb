Imports System.Text.RegularExpressions

Public Class Documento
    Public campos As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Public Shared Function ParseLinesPDF(ByVal lines As String, ByVal regExpTemplaytList As Dictionary(Of String, String)) As List(Of Documento)
        Dim arrLines As String() = Regex.Split(lines, vbCrLf)
        Dim dList As List(Of Documento) = New List(Of Documento)()
        Dim templateId As String
        Dim regExp As String = Nothing

        For Each line As String In arrLines
            Dim documento As Documento = New Documento()
            'O ID do template são os 3 primeiros bytes da linha
            templateId = line.Substring(0, 3)

            If Not regExpTemplaytList.ContainsKey(templateId) Then
                Logger.WriteToLog("Documento.ParseLinesPDF() > Template ID '" + templateId + "' não encontrado no arquivo de layout")
                Return Nothing
            End If

            regExpTemplaytList.TryGetValue(templateId, regExp)

            If regExp Is Nothing Then
                Logger.WriteToLog("Documento.ParseLinesPDF() > Expressão regular não definida para Template ID '" + templateId + "'")
                Return Nothing
            End If

            Try
                Dim cRegExp As Regex = New Regex(regExp)
                Dim grupo As Group
                documento.campos.Add("templateID", templateId)
                For Each match As Match In cRegExp.Matches(line)
                    For groupCtr As Integer = 1 To match.Groups.Count - 1
                        grupo = match.Groups(groupCtr)
                        documento.campos.Add(cRegExp.GroupNameFromNumber(groupCtr), grupo.Value)
                    Next
                Next
                dList.Add(documento)
            Catch e As Exception
                Logger.WriteToLog("Documento.ParseLinesPDF() > Erro ao fazer análise de expressão regular na linha do resultado" + vbCrLf + e.Message)
                Return Nothing
            End Try
        Next
        Return dList
    End Function

    Public Shared Function ParseLinesXML(ByVal lines As String, ByVal regExpTemplaytList As Dictionary(Of String, String)) As String
        Dim arrLines As String() = Regex.Split(lines, vbCrLf)
        Dim xml As String = "<?xml version=""1.0"" encoding=""UTF-8""?>" + vbCrLf + "<documentos>" + vbCrLf
        Dim templateId As String
        Dim regExp As String = Nothing
        Dim cRegExp As Regex

        For Each line As String In arrLines
            'O ID do template são os 3 primeiros bytes da linha
            templateId = line.Substring(0, 3)

            If Not regExpTemplaytList.ContainsKey(templateId) Then
                xml = xml.Substring(0, xml.IndexOf("?>") + 4)
                xml += "<erro>" + vbCrLf + "<codigo>8</codigo>" + vbCrLf + "<descricao>Template ID '" + templateId + "' não definido no arquivo de layout</descricao>" + vbCrLf + "</erro>"
                Logger.WriteToLog("Documento.ParseLinesXML() > Template ID '" + templateId + "' não encontrado no arquivo de layout")
                Return xml
            End If

            xml += "<documento templateID=""" + templateId + """>" + vbCrLf
            regExpTemplaytList.TryGetValue(templateId, regExp)

            If regExp Is Nothing Then
                xml = xml.Substring(0, xml.IndexOf("?>") + 4)
                xml += "<erro>" + vbCrLf + "<codigo>9</codigo>" + vbCrLf + "<descricao>Expressão regular não definida para Template ID '" + templateId + "'</descricao>" + vbCrLf + "</erro>"
                Logger.WriteToLog("Documento.ParseLinesXML() > Expressão regular não definida para Template ID '" + templateId + "'")
                Return xml
            End If

            Try
                cRegExp = New Regex(regExp)
                Dim grupo As Group
                For Each match As Match In cRegExp.Matches(line)
                    For groupCtr As Integer = 1 To match.Groups.Count - 1
                        grupo = match.Groups(groupCtr)
                        xml += "<" + cRegExp.GroupNameFromNumber(groupCtr) + ">" + grupo.Value.Trim + "</" + cRegExp.GroupNameFromNumber(groupCtr) + ">" + vbCrLf
                    Next
                Next
                xml += "</documento>" + vbCrLf
            Catch e As Exception
                xml = xml.Substring(0, xml.IndexOf("?>") + 4)
                xml += "<erro>" + vbCrLf + "<codigo>10</codigo>" + vbCrLf + "<descricao>Erro ao fazer análise de expressão regular na linha do resultado. Consultar log de erro para maiores informações</descricao>" + vbCrLf + "</erro>"
                Logger.WriteToLog("Documento.ParseLinesXML() > Erro ao fazer análise de expressão regular na linha do resultado" + vbCrLf + e.Message)
                Return xml
            End Try
        Next
        xml += "</documentos>"
        Return xml
    End Function
End Class