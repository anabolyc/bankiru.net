(function () {
    $('<div id="div-log"></div>').css("opacity", 0.40).appendTo($("body"))
})()

document.log = {}

document.log = function (s) {
    var t = new Date()
    document.getElementById("div-log").innerHTML += (t.getTime() % 10000) + " +" + (t.getTime() - document.log.timer.getTime()) + " ms: " + s + "<br>"
    document.log.timer = t
    if (document.log.rows++ > document.log.maxrows) {
        var lines = document.getElementById("div-log").innerHTML.toLowerCase().split("<br>")
        lines.shift()
        document.getElementById("div-log").innerHTML = lines.join("<br>")
    }
}

document.log.rows = 0

document.log.maxrows = 20

document.log.timer = new Date()
