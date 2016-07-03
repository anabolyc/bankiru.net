@Imports www.BankBals.Data
@ModelType List(Of A_VIEWITEMS_ALL)

@Code
    ViewData("Title") = "Список отображений"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    Рэнкинги
</div>

<div class='div-datatable-cont' id='cont'> 
    <input class='sw' id='view-srch' type='text'/> <br/><br/>
    <span class='pseudolink' id='span-expand'>Раскрыть</span>&nbsp;<span class='pseudolink' id='span-collapse'>Скрыть</span><br /><br />
    <table class='texttable' id='table-list'>
    @Code   
        Dim ViewID As Integer = 0
        For Each item As A_VIEWITEMS_ALL In Model
            If ViewID <> item.ViewID Then
                If ViewID <> 0 Then
                    @:</ul></td></tr><tr><td><span class='span-li pseudolink'>@item.ViewNameRus</span><ul class='ul-list'>
                Else
                    @:<tr><td><span class='span-li pseudolink'>@item.ViewNameRus</span><ul class='ul-list'>                    
                End If
                ViewID = item.ViewID
            End If
            
            If Not item.AggItemID.HasValue Then
                @<li>
                    <span>@Html.Raw(Space(Math.Max(item.LevelDepth.Value - 1, 0) * 3).Replace(" ", "&nbsp;"))</span>
                    <strong>@item.NameRus</strong>
                </li>
            Else
                @<li>
                    <img form='@item.Form' aggitemid='@item.AggItemID' class='img-help' src='@Url.Content("~/Content/help.png")' title='show help' />
                    <span>@Html.Raw(Space(Math.Max(item.LevelDepth.Value - 1, 0) * 3).Replace(" ", "&nbsp;"))</span>
                    <a href='@VirtualPathUtility.ToAbsolute("~/View/Data/")?ViewID=@(item.ViewID)&ViewItemID=@(item.ViewItemID)'>@(item.NameRus)</a>
                </li>
            End If
        Next
    End Code
    </ul></td></tr>
    </table>
</div>

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

<script type="text/javascript">

    $(document).ready(function () {
        $("li#li-tab3").addClass("selected")

        $("#span-collapse").click(function () {
            $(".ul-list").hide();
        });
        
        $("#span-expand").click(function () {
            $(".ul-list").show();
        });

        $("img.img-help").click( function () {
            var url = "/Service/Help/?Form=" + $(this).attr("form") + "&AggItemID=" + $(this).attr("aggitemid")
            lightbox(url)
        })

        $(".ul-list").hide();
                
        $(".span-li").click(function () {
            $(this).parent().find(".ul-list").toggle("fast");
        })

        @Html.Partial("applet.view-search.js")
        @Html.Partial("applet.lightbox.js")

    });
</script>
