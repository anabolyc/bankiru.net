Imports NLog

Namespace Controllers

    Public Class HomeController
        Inherits BaseController

        Private log As Logger = LogManager.GetLogger("404")

        Function Index() As ActionResult
            Return View()
        End Function

        Function PageNotFound() As ActionResult
            Return View()
        End Function

    End Class

End Namespace