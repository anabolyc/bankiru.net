<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="author" content="Enthusiast group" />
    <meta name="description" content="Изучай и исследуй отчетности (РСБУ) российских банков" />
    <meta name="keywords" content="Балансы, Внебалансовые счета, Отчет о прибылях и убытках, Коэффициенты баланса, Коэффициенты отчета о прибылях и убытках, Расчет собственных средств, Обязательные нормативы, Информация о состоянии российских банков, Отчетности Российских банков по РСБУ, Российский стандарт банковского учета" />
    
    <title>@ViewData("Title")</title>
    @If Common.Placement() = Common.Placements.PRODUCTION Then
        Html.Raw("<link href='http://fonts.googleapis.com/css?family=Ubuntu+Mono&subset=latin,cyrillic' rel='stylesheet' type='text/css' />")
    Else
        
    End If
    <!--[if lt IE 8]>
    <link href="@Url.Content("~/Content/ie6-7.css")" rel="stylesheet" type="text/css" />
    <![endif]-->
    <link href="@Url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
    @If Common.Placement() <> Common.Placements.PRODUCTION Then
        @<link href="@Url.Content("~/Content/nonproduction.css")" rel="stylesheet" type="text/css" />
        @:<!--[if lt IE 8]>
        @<link href="@Url.Content("~/Content/nonproduction-ie.css")" rel="stylesheet" type="text/css" />
        @:<![endif]-->
    End If
    <link href="@Url.Content("~/Scripts/jquery-ui-1.8.23.custom/css/redmond/jquery-ui-1.8.23.custom.css")" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="@Url.Content("~/Content/b.png")" />
    <script src="@Url.Content("~/Scripts/jquery-ui-1.8.23.custom/js/jquery-1.8.0.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery-ui-1.8.23.custom/js/jquery-ui-1.8.23.custom.min.js")" type="text/javascript"></script>
</head>
<body>
    <div class="page">
        <header>
            @Code 
                If Common.Placement() = Common.Placements.PRODUCTION Then
                    @<div id='title'><a href='@VirtualPathUtility.ToAbsolute("~/")'>
                        <h1 class='h1-logo'>Bankiru<span class='span-dot'>.NET</span></h1></a>
                     </div>
                Else
                    @<div id='title'>
                        <h1 class='h1-logo'> <a href="/">Home</a> &gt; 
                        <a href='@VirtualPathUtility.ToAbsolute("~/")'>
                        Bank Balances</a></h1>
                     </div>
                End If
                @<div id="logindisplay">
                    @Html.Partial("_LogOnPartial")
                </div>
            End Code
            <nav>
                <ul id="menu">
                    <li id="li-tab1"><a href="@VirtualPathUtility.ToAbsolute("~/")"><img src="@Url.Content("~/Content/tabs/home.png")" alt="Home" />Главная</a></li>
                    <li id="li-tab2"><a href="@VirtualPathUtility.ToAbsolute("~/Bank/List/")"><img src="@Url.Content("~/Content/tabs/bank.png")" alt="Banks" />Банки</a></li>
                    <li id="li-tab3"><a href="@VirtualPathUtility.ToAbsolute("~/View/List/")"><img src="@Url.Content("~/Content/tabs/rank.png")" alt="Rankings" />Рэнкинги</a></li>
                    <li id="li-tab5"><a href="@VirtualPathUtility.ToAbsolute("~/Comparison/")"><img src="@Url.Content("~/Content/tabs/compare.png")" alt="Comparison" />Сравнение</a></li>
                </ul>
            </nav>
        </header>
        <div id="main">
            @RenderBody()
        </div>
        <footer>
            @If Common.Placement() = Common.Placements.PRODUCTION Then
            @<script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
            @<ins class="adsbygoogle"
                 style="display:inline-block;width:728px;height:90px"
                 data-ad-client="ca-pub-8862971728885811"
                 data-ad-slot="2730231487"></ins>
            @<script>(adsbygoogle = window.adsbygoogle || []).push({});</script>
            End If
            <div  id="footer">
            @Code
                If Common.Placement() = Common.Placements.PRODUCTION Then
                    WriteLiteral("© " & DateTime.Now.Year & " Bankiru.net. Проект создан и развивается <a href='mailto:feedback@bankiru.net'>энтузиастами</a>")
                Else
                    WriteLiteral("© " & DateTime.Now.Year & " ГПБ (ОАО)")
                End If
            
                If Common.Placement() = Common.Placements.PRODUCTION Or Request.IsAuthenticated Then
                @<text> | </text> @Html.ActionLink("Service Pages", "Index", "Service") 
                @<text> | </text> @Html.ActionLink("API", "Index", "API") 
                End If
            End Code
            </div>
        </footer>
    </div>

    @If Common.Placement() = Common.Placements.PRODUCTION Then
    @<script type="text/javascript">
        // GOOGLE ANALYTICS
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-30220044-1']);
        _gaq.push(['_trackPageview']);
        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
    End If
</body>
</html>
