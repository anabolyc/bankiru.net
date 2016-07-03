    function lightbox(url) {
        
        bankvisible = $("<div id='div-frame'><img id='img-bankclose' title='close' src='@Url.Content("~/Content/close.png")' /><div id='div-frame-inner'></div> </div>").appendTo($("body"));

        $("<div id='div-fade'></div>").css("opacity", 0.1).appendTo($("body")).fadeIn("fast");

        $("#div-fade").click(function () {
            bankvisible = !$("#div-frame").fadeOut("fast", function () {
                $("#div-frame").remove();
                $("#div-fade").remove();
            });
        });

        $("#img-bankclose").click(function () {
            bankvisible = !$("#div-frame").fadeOut("fast", function () {
                $("#div-frame").remove();
                $("#div-fade").remove();
            });
        });
        
        $("#div-frame-inner").html("<iframe frameBorder='0' src='" + url + "' />")
    }

