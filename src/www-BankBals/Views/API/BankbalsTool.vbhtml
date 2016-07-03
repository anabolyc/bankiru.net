@Code
    ViewData("Title") = "API Index"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("API", "Index", "API") &#8594; 
    BankbalsTool
</div>

@Code
If Common.Placement() = Common.Placements.PRODUCTION Then
    
    @<p>Этот тул мы сделали для того, чтобы у вас был практически полноценный доступ к нашей базе отчетностей.</p>
    @<p>Он берет данные напрямую из веб-сервисов, которые мы сделали специально для этого. </p>
    @<p>В экселе 3 страницы, для выгрузки данных в разрезе банка, даты или показателя.</p>
    @<p>Чтобы тул заработал, вам нужно разрешить запуск макросов. Мы специально оставили код макросов открытым, чтобы вы могли убедиться, что там нет вирусов или шпионского кода.</p>
End If
End Code
<p>
<a href="@VirtualPathUtility.ToAbsolute("~/Content/BankBalsTool/BankBalsTool-Web.xlsm")">
    <img src="@VirtualPathUtility.ToAbsolute("~/Content/BankBalsTool/xlsm.png")" title="BankbalsTool.xlsm"/>
</a>
<a href="@VirtualPathUtility.ToAbsolute("~/Content/BankBalsTool/BankBalsTool-Web.xlsm")">Скачать BankbalsTool.xlsm</a>

</p>
<p>
<img src="@VirtualPathUtility.ToAbsolute("~/Content/BankBalsTool/1.png")" title="ScreenShot 1"/>
<img src="@VirtualPathUtility.ToAbsolute("~/Content/BankBalsTool/2.png")" title="ScreenShot 2" />
<img src="@VirtualPathUtility.ToAbsolute("~/Content/BankBalsTool/3.png")" title="ScreenShot 3" />
</p>