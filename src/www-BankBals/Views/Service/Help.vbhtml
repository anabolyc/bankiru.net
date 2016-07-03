@Imports www.Bankbals.Data

@ModelType IEnumerable(Of Export.HelpDataRow)

@Code
    ViewData("Title") = "Help"
    Layout = "~/Views/Shared/_Frame.vbhtml"
End Code

<div class='div-frame'>
    @*<input class='sw' id='srch' type='text'/><br>
    @@ TODO: search
    *@
    <span class='pseudolink' id='span-expand'>Раскрыть</span>&nbsp;<span class='pseudolink' id='span-collapse'>Скрыть</span><br /><br />
    <ul>
    @Code
        Dim LevelID As Integer = -1
        Dim index As Integer = 100
        For Each item As Export.HelpDataRow In Model
            If item.LevelID <> LevelID Then
                If LevelID <> -1 Then
                    For i As Integer = item.LevelID To LevelID
                        @:<ul class='ul-list2 g@(index)'>
                    Next
                    For i As Integer = LevelID To item.LevelID
                        @:</ul>
                    Next
                End If
                LevelID = item.LevelID
            End If
            index += 1
            If item.Ticker = "" Then
                @<li class='li-header' GroupID='g@(index)'><span>@item.Name</span></li>
            Else
                @<li@Html.Raw(IIf(item.LevelID = 0, "", " class='li-pseudolink'")) GroupID='g@(index)'>
                    <span>@item.Ticker@(IIf(item.Type = "", "", "(" + item.Type +")")) : @item.Name</span>
                 </li>                
            End If
        Next
    End Code
    </ul>
</div>
		
@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

<script type="text/javascript">
    $("#span-collapse").click(function () {
	    $(".ul-list2").hide();
    });

    $("#span-expand").click(function () {
	    $(".ul-list2").show();
	});

	$("li").click(function () {
	    window.log($(this).attr('GroupID'))
	    $(this).parent().find("." + $(this).attr('GroupID')).toggle("fast");
	});

	$(".ul-list2").hide()
    /*
	var last_srch = ""
	$("#srch").focus(function () {
	    intervalID = window.setInterval(function () {
	        s = $("#srch").attr("value");
	        if (s != last_srch) {
	            window.log(s)
	            //$("#ul-help").replaceWith(viewListString(s.toLowerCase()))
	            $("li:contains('" + s + "')").show();
	            last_srch = s
	        }
	    }, 250)
	});
	
    $("#srch").blur(function () { clearInterval(intervalID); last_s = ''; });
    */
</script>