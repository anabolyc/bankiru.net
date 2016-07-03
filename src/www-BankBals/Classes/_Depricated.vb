'Public Class Authorize401Attribute
'    Inherits AuthorizeAttribute

'    Private Class Http401Result
'        Inherits ActionResult
'        Public Overrides Sub ExecuteResult(ByVal context As ControllerContext)
'            context.HttpContext.Response.StatusCode = 401
'            context.HttpContext.Response.End()
'        End Sub
'    End Class

'    Protected Overrides Sub HandleUnauthorizedRequest(ByVal filterContext As AuthorizationContext)
'        filterContext.Result = New Http401Result()
'    End Sub
'End Class
