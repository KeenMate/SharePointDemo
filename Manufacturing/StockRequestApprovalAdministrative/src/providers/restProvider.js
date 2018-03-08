import $ from "jquery"

export default {
    GetData: function (url, body) {
        console.dir("body to send" + body);
        var def = $.Deferred()
        url = url || "RequestsOverview/GetData"
        $.ajax({
            url: url,
            method: "post",
            accept: "application/json",
            data: body || null
        }).done(function (data) {
            def.resolve(data)
        }).fail(function (response) {
            def.reject(response)
        })
        return def
    }
}