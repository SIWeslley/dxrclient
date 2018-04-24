Imports System.IO

Class BigBinaryReader
    Inherits BinaryReader
    Private a16 As Byte() = New Byte(1) {}
    Private a32 As Byte() = New Byte(3) {}
    Private a64 As Byte() = New Byte(7) {}
    Public Sub New(ByVal stream As System.IO.Stream)
        MyBase.New(stream)
    End Sub
    Public Overrides Function ReadInt32() As Integer
        a32 = MyBase.ReadBytes(4)
        Array.Reverse(a32)
        Return BitConverter.ToInt32(a32, 0)
    End Function
    Public Overrides Function ReadInt16() As Int16
        a16 = MyBase.ReadBytes(2)
        Array.Reverse(a16)
        Return BitConverter.ToInt16(a16, 0)
    End Function
    Public Overrides Function ReadInt64() As Int64
        a64 = MyBase.ReadBytes(8)
        Array.Reverse(a64)
        Return BitConverter.ToInt64(a64, 0)
    End Function
    Public Overrides Function ReadUInt32() As UInt32
        a32 = MyBase.ReadBytes(4)
        Array.Reverse(a32)
        Return BitConverter.ToUInt32(a32, 0)
    End Function

End Class