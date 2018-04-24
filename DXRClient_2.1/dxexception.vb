Public Class DXServerErrorException : Inherits System.Exception
    Private Shared ReadOnly errorDictionary As New Dictionary(Of Integer, String) From {
        {-1, """Erro desconhecido"""}, {-2, """Erro de conexão"""}, {1, """Página invalida - Numero de página requisitado esta fora do alcançe de paginas na declaração"""},
        {4, """Erro ao ler a compressão de dados."""},
        {10, """Nenhum documento foi aberto."""}, {11, """Nenhum resultado da busca foi encontrado."""}, {12, """Parâmetro invalido."""},
        {14, """Limite de tempo excedido para à atividade."""}, {16, """Pasta não encontrada - Esse erro pode ocorrer em uma pasta que supostamente existe por que os arquivos foram corrompidos, modificados ou deletados"""},
        {17, """Gaveta não encontrada -Esse erro pode ocorrer em uma gaveta que supostamente existe por que os arquivos foram corrompidos, modificados ou deletados"""},
        {18, """Arquvivo da gaveta não encontrado - O arquivo de gaveta referenciado em DXR.CFG não foi encontrado"""}, {22, """Tempo esgotado do soquete(timeout)"""}, {24, """Não foi possível encontrar a JTB para esse arquivo DAT"""},
        {26, """Numero maximo de conexões com o servidor ja excedido"""}, {28, """Não foi possivel ler arquivo DAT"""}, {29, """Não foi possivel ler arquivo I00"""},
        {30, """Não foi possivel ler arquivo P00"""}, {31, """Arquivo P00 inválido"""}, {32, """Numero de índice inválido"""}, {33, """Erro ao ler arquivo de índice"""},
        {37, """Usuário inválido"""}, {42, """Senha inválida"""}, {52, """Servidor não esta respondendo"""}, {54, """Chave não encontrada"""},
        {60, """Necessário parâmetro que esta ausente"""}, {63, """Não foi possivel criar arquivo"""}, {68, """Soquete não pode ser criado"""},
        {72, """Não foi possivel ler arquivo .SRS ou .S00"""}, {79, """O commando passado ao servidor causou uma exceção"""}, {80, """Não ha gavetas"""}, {81, """Não ha pastas"""},
        {82, """Não ha relatórios"""}, {83, """Não ha paginas para exportar"""}, {15, """Relatório não encontrado"""}, {276, """Tipo de dado não suportado para exportação em csv"""}
    }
    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub

    Public Shared Function errorMessage(ByVal errorcode As Int32) As String
        Try
            Dim str As String = errorDictionary.Item(errorcode)
            Return str
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class
