Public Class DXServerSpec
    Protected szIP As String
    Protected szPort As Int32
    Protected szTimeout As Int32
    Protected szServers() As String
    Protected szTries As Int32

    Public Sub New(ByVal pIP As String, ByVal pPort As Int32, ByVal pTout As Int32, ByVal pServers As String())

        szIP = pIP
        szPort = pPort
        szTimeout = pTout
        szServers = pServers
    End Sub

    Public Sub New(ByVal pIP As String, ByVal pPort As Int32, ByVal pTout As Int32)

        szIP = pIP
        szPort = pPort
        szTimeout = pTout
    End Sub

    Public Sub New(ByVal pIP As String, ByVal pPort As Int32)

        szIP = pIP
        szPort = pPort
    End Sub

    Sub New()

    End Sub

    Public Property IP() As String
        Get
            Return szIP
        End Get
        Set(ByVal value As String)
            szIP = value
        End Set
    End Property

    Public Property Port() As Int32
        Get
            Return szPort
        End Get
        Set(ByVal value As Int32)
            szPort = value
        End Set
    End Property

    Public Property Timeout() As Int32
        Get
            Return szTimeout
        End Get
        Set(ByVal value As Int32)
            szTimeout = value
        End Set
    End Property

    Public Property ServeList() As String()
        Get
            Return szServers
        End Get
        Set(ByVal value As String())
            szServers = value
        End Set
    End Property

    Public Property Tries() As Int32
        Get
            Return szTries
        End Get
        Set(ByVal value As Int32)
            szTries = value
        End Set
    End Property

End Class
