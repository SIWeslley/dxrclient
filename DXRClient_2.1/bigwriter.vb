Imports System.IO

Public Class BigBinaryWriter
    Inherits BinaryWriter
    Public Sub New(ByVal stream As System.IO.Stream)
        MyBase.New(stream)
    End Sub
    Public Shadows Function WriteInt(ByVal p As Int32) As Boolean
        Dim a(3) As Byte
        a = BitConverter.GetBytes(p)
        Array.Reverse(a)
        MyBase.Write(a)
        Return True
    End Function

    Public Shadows Function WriteShort(ByVal p As Int16) As Boolean
        Dim a(1) As Byte
        a = BitConverter.GetBytes(p)
        Array.Reverse(a)
        MyBase.Write(a)
        Return True
    End Function

    Public Shadows Function WriteLong(ByVal p As Int64) As Boolean
        Dim a(7) As Byte
        a = BitConverter.GetBytes(p)
        Array.Reverse(a)
        MyBase.Write(a)
        Return True
    End Function

    Public Shadows Function Write(ByVal p As Byte) As Boolean
        MyBase.Write(p)
        Return True
    End Function

    Public Shadows Function Write(ByVal p As Byte()) As Boolean
        MyBase.Write(p)
        Return True
    End Function
End Class
