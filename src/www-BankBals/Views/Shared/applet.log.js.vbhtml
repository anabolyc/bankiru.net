<script type="text/javascript">

    (function () {
        $('<div id="div-log"></div>')
@Code
    If (DEBUG()) Then
     @<text>.css("opacity", 0.5)</text>
Else
     @<text>.css("opacity", 0.0).css("z-index", -5)</text>
End If
End Code
        .appendTo($("body"))
    })()

    window.log = {}

    window.log = function (s) {
        var t = new Date()
        document.getElementById("div-log").innerHTML += (t.getTime() % 10000) + " +" + (t.getTime() - window.log.timer.getTime()) + " ms: " + s + "<br>"
        window.log.timer = t
        if (window.log.rows++ > window.log.maxrows) {
            var lines = document.getElementById("div-log").innerHTML.toLowerCase().split("<br>")
            lines.shift()
            document.getElementById("div-log").innerHTML = lines.join("<br>")
        }
    }

    window.log.rows = 0

    window.log.maxrows = 20

    window.log.timer = new Date()

</script>