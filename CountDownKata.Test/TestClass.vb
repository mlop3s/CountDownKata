Option Explicit On
' NUnit 3 tests
' See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
Imports System
Imports NUnit.Framework

Namespace CountDownKata.Test

    <TestFixture>
    Public Class TestClass

        <Test>
        Public Sub TestMethod()
            ' TODO Add your test code here
            Dim answer = 42
            Assert.That(answer, [Is].EqualTo(42), "Some useful error message")
        End Sub

    End Class

End Namespace