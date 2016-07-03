<!DOCTYPE html>

<html>
<head runat="server">
    <title>@ViewData("Title")</title>
    <link href="@Url.Content("~/Content/Chart.css")" rel="stylesheet" type="text/css" />
    <script src="@Url.Content("~/Scripts/jquery-ui-1.8.23.custom/js/jquery-1.8.0.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/Highcharts-3.0.0/js/highcharts.js")" type="text/javascript"></script>
</head>
<body>
    @RenderBody()

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
