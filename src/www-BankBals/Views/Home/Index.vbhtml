@Code
    ViewData("Title") = IIf(Common.Placement() = Common.Placements.PRODUCTION, "Главная - bankiru.net", "Главная")
End Code

<h3> Вы можете узнать всё про @Html.ActionLink("банк", "List", "Bank")
    <input class='sw' id='bank-srch' type='text'/>
</h3>
<div class="div-block">

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1481&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1481s.png")" alt="Сбербанк"/>
        <span>Сбербанк</span>
    </div>
    </a>
    
    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1000&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1000s.png")" alt="ВТБ"/>
        <span>ВТБ</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=354&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/354s.png")" alt="Газпромбанк"/>
        <span>Газпромбанк</span>
    </div>
    </a>
    
    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=3349&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/3349s.png")" alt="Россельхозбанк"/>
        <span>Россельхозбанк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1623&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1000s.png")" alt="ВТБ24"/>
        <span>ВТБ24</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1326&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1326s.png")" alt="Альфабанк"/>
        <span>Альфабанк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1s.png")" alt="Юникредит"/>
        <span>Юникредит</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=2272&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/2272s.png")" alt="Росбанк"/>
        <span>Росбанк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=3292&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/3292s.png")" alt="Райффайзенбанк"/>
        <span>Райффайзенбанк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=3251&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/3251s.png")" alt="Промсвязьбанк"/>
        <span>Промсвязьбанк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=2209&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/2209s.png")" alt="Номос-банк"/>
        <span>Номос-банк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=2275&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/2275s.png")" alt="Уралсиб"/>
        <span>Уралсиб</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=323&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/323s.png")" alt="МДМ"/>
        <span>МДМ</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=2557&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/2557s.png")" alt="Сити-Банк"/>
        <span>Сити-Банк</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1978&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1978s.png")" alt="МКБ"/>
        <span>МКБ</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=2289&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/2289s.png")" alt="Русский Стандарт"/>
        <span>Русский Стандарт</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=1439&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/1439s.png")" alt="Возрождение"/>
        <span>Возрождение</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=316&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/316s.png")" alt="Хоум Кредит"/>
        <span>Хоум Кредит</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=3279&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/3279s.png")" alt="НБ Траст"/>
        <span>НБ Траст</span>
    </div>
    </a>

    <a href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/?BankID=2673&ViewID=1")">
    <div class='div-onebank'>
        <img src="@Url.Content("~/Content/img/2673s.png")" alt="Тинькофф"/>
        <span>Тинькофф</span>
    </div>
    </a>

</div>

<h3> и можете посмотреть @Html.ActionLink("рэнкинг", "List", "View") банков <span id="view-cont"></span> 
    <input class='sw' id='view-srch' type='text'/>
</h3>
<div class="div-block">

    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=1&ViewItemID=1")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f101.png")" alt="Сумма активов"/><span>Сумма активов</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=3&ViewItemID=57")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f102.png")" alt="Чистая прибыль"/><span>Чистая прибыль</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=4&ViewItemID=10")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f101.png")" alt="Процент просрочки по кредитам клиентам"/><span>Процент просрочки по кредитам клиентам</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=4&ViewItemID=32")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f101.png")" alt="Процент резервирование кредитов клиентам"/><span>Процент резервирование кредитов клиентам</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=7&ViewItemID=2")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f135.png")" alt="Норматив Н1"/><span>Норматив Н1</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=4&ViewItemID=17")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f101.png")" alt="Доля кредитов физ. лицам в активах"/><span>Доля кредитов физ. лицам в активах</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=4&ViewItemID=19")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f101.png")" alt="Доля депозитов физ. лиц в пассивах"/><span>Доля депозитов физ. лиц в пассивах</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=4&ViewItemID=3")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f101.png")" alt="Доход на средние активы"/><span>Доход на средние активы</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=3&ViewItemID=1")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f102.png")" alt="Чистые процентные доходы"/><span>Чистые процентные доходы</span></div></a>
    <a href="@VirtualPathUtility.ToAbsolute("~/View/Data/?ViewID=5&ViewItemID=11")"><div class='div-oneview'><img src="@Url.Content("~/Content/img/f102.png")" alt="Доходность кредитов клиентам"/><span>Доходность кредитов клиентам</span></div></a>
     
</div>

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")
 
<script type="text/javascript">
    $("li#li-tab1").addClass("selected")
    
    $(document).ready(function () {
        @Html.Partial("applet.bank-search.js")
        @Html.Partial("applet.view-search.js")
    });
</script>