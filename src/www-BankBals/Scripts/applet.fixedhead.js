document.fixhead = []

document.fixhead.clear = function () {
    for (var index = 0; index < document.fixhead.length; index++) {
        $(document.fixhead[index].clone).remove()
        window.log("removed: " + index)
    }
    document.fixhead.length = 0
}

document.fixhead.apply = function ($table, $container) {
    document.fixhead.push({
        table: $table
    })

    $($.browser.msie ? window : document).scroll(function () {
        for (var index = 0; index < document.fixhead.length; index++) {
            var element = document.fixhead[index].table
            var headoffset = $(element).offset()
            var scroll = $(document).scrollTop()
            if (headoffset.top - scroll <= 0) {
                if (document.fixhead[index].visible) {
                    //already positioned
                } else {
                    if (document.fixhead[index].clone) {
                        //already generated
                    } else {
                        var table = document.createElementEx("table", "", element.get(0).className)
                        var thead = $(element).find("thead").clone()
                        $(thead).find("span.span-switch").replaceWith("<span>&nbsp;</span>")
                        for (var i = 0; i < thead.find("th").length; i++) {
                            thead.find("th").eq(i).width($(element).find("thead").find("th").eq(i).width() + ($.browser.mozilla ? 0 : 1))
                        }
                        $(table).addClass("table-clone")
                        .width($(element).width())
                        .append(thead)
                        document.fixhead[index].clone = table
                        if ($container) {
                            $container.fixheadindex = index
                            $container.scroll(function () {
                                if (document.fixhead[$container.fixheadindex].visible) {
                                    $(table).css("left", $(element).offset().left)
                                }
                            })
                        }
                        $(document.fixhead[index].clone).appendTo($("body"))
                    }

                    $(document.fixhead[index].clone).css("left", $(element).offset().left)
                    $(document.fixhead[index].clone).show()//remove()
                    document.fixhead[index].visible = true
                }
            } else {
                if (document.fixhead[index].visible) {
                    $(document.fixhead[index].clone).hide()//remove()
                    document.fixhead[index].visible = false
                }
            }
        }
    })
}
