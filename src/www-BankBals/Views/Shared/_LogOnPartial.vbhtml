@If Common.Placement() = Common.Placements.PRODUCTION Then
    If Request.IsAuthenticated Then
        @<text>Вошел <span class='span-identity'>@User.Identity.Name</span>
        <a href="@VirtualPathUtility.ToAbsolute("~/Account/LogOff")"> 
            <img id='img-logon' src="@Url.Content("~/Content/logout.png")" /> Выйти 
        </a>
        </text>
    Else
        @<text>
        <a href="@VirtualPathUtility.ToAbsolute("~/Account/LogOn?returnUrl=" + Request.Url.LocalPath )"> 
            <img id='img-logon' src="@Url.Content("~/Content/logon.png")" /> Войти 
        </a>
        </text>
    End If
Else
    @<text>
        &nbsp;
    </text>
End If