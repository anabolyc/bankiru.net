window.toolbox = {}

window.toolbox.ie = ($.browser.msie == true)

window.toolbox.newid = function (label) {
    return label + (((1 + Math.random()) * 0xFFFFFFFF) | 0).toString(16)
}

window.toolbox.space = function (num) {
    return Array(num).join("&nbsp;")
}

window.toolbox.addSpace = function (nStr, num) {
    nStr = nStr + ''
    if (nStr.length < num)
        nStr = Array(num + 1 - nStr.length).join("&nbsp;") + nStr
    return nStr
}

window.toolbox.isInteger = function (s) {
    return !isNaN(parseInt(s))
}

window.toolbox.addCommas = function (nStr) {
    if (nStr == null || nStr === "")
        return "-"
    if (parseFloat(nStr) == 0)
        return nStr
    if (nStr.length < 4)
        return nStr
    nStr += ''
    x = nStr.split('.')
    x1 = x[0]
    x2 = x.length > 1 ? '.' + x[1] : ''
    var rgx = /(\d+)(\d{3})/
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ' ' + '$2')
    }
    return x1 + x2
}

window.toolbox.getChangeElement = function (a) {
    var chgValue = a
    var chgClass = "span-diff"
    if (!window.toolbox.isInteger(a)) {
        chgValue = ""
    } else {
        if (a < 0) {
            if (chgValue <= -99999)
                chgValue = "▼▼▼"
            else
                chgValue = window.toolbox.addSpace(chgValue / 100, 5) + "%"
            chgClass += " span-red"
        }
        if (chgValue > 0) {
            if (chgValue >= 99999)
                chgValue = "▲▲▲"
            else
                chgValue = window.toolbox.addSpace("+" + chgValue / 100, 5) + "%"
            chgClass += " span-green"
        }
        if (chgValue == 0) {
            chgValue = "~0%"
        }
    }
    return document.createElementEx("span", chgValue, chgClass)
}

window.toolbox.getRankElement = function (rnkValue) {
    return document.createElementEx("span", window.toolbox.addSpace('#' + rnkValue, 5), "span-lightGrey")
}

window.toolbox.getPercElement = function (a) {
    var chgValue = a
    if (!window.toolbox.isInteger(a)) {
        chgValue = ""
    } else {
        if (chgValue >= 99999)
            chgValue = "▲▲▲"
        else
            chgValue = window.toolbox.addSpace(chgValue / 100, 5) + "%"
    }
    return document.createElementEx("span", chgValue, "span-perc")
}

window.toolbox.top = function (e) {
    return $(e).offset().top
}

window.toolbox.visible = function (e) {
    var top = window.toolbox.top(e)
    var lowcheck = (top < $(window).height() + $(window).scrollTop()) ? true : false
    var highcheck = (top + $(e).height() > $(window).scrollTop()) ? true : false
    //window.log(e.id + "|" + lowcheck + "|" + highcheck)
    return (lowcheck && highcheck)
}

document.createElementEx = function (tagName, innerHTML, className) {
    var e = document.createElement(tagName)
    if (innerHTML)
        e.innerHTML = innerHTML
    if (className)
        e.className = className
    return e
}

document.createElementEx2 = function (tagName, innerText, className) {
    var e = document.createElement(tagName)
    if (innerText) {
        if (window.toolbox.ie)
            e.innerText = innerText
        else
            e.innerHTML = innerText
    }
    if (className)
        e.className = className
    return e
}

parseFLOAT = function (str) {
    if (isNaN(parseFloat(str)))
        return null
    else
        return parseFloat(str)
}
