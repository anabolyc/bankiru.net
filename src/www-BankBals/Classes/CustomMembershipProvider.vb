Imports System.Reflection
Imports www.BankBals.Models
Imports www.BankBals.Data

Namespace Classes

    Public Class CustomMembershipProvider
        Inherits SqlMembershipProvider
        Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
            MyBase.Initialize(name, config)
            Dim connectionString As String = Tools.ConnectionString(Tools.SQL_DB_ASP)
            Dim connectionStringField = [GetType]().BaseType.GetField("_sqlConnectionString", BindingFlags.Instance Or BindingFlags.NonPublic)
            connectionStringField.SetValue(Me, connectionString)
        End Sub
    End Class

    Public Class CustomProfileProvider
        Inherits SqlProfileProvider
        Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
            MyBase.Initialize(name, config)
            Dim connectionString As String = Tools.ConnectionString(Tools.SQL_DB_ASP)
            Dim connectionStringField = [GetType]().BaseType.GetField("_sqlConnectionString", BindingFlags.Instance Or BindingFlags.NonPublic)
            connectionStringField.SetValue(Me, connectionString)
        End Sub
    End Class

    Public Class CustomRoleProvider
        Inherits SqlRoleProvider
        Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
            MyBase.Initialize(name, config)
            Dim connectionString As String = Tools.ConnectionString(Tools.SQL_DB_ASP)
            Dim connectionStringField = [GetType]().BaseType.GetField("_sqlConnectionString", BindingFlags.Instance Or BindingFlags.NonPublic)
            connectionStringField.SetValue(Me, connectionString)
        End Sub
    End Class

End Namespace