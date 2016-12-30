
class Utils {

    constructor() { }

    public static FormFunctions() {
    }

    public static GetREST(url: string, fsuccess: (response) => any, ferror: (response) => any) {
        $.ajax({
            type: "GET",
            url: this.getBaseUrl() + url,
            dataType: "xml",
            success: fsuccess,
            cache: false,
            error: ferror
        });
    }

    public static PutREST(url, data, fsuccess: (response, textStatus, xhr) => any, ferror: (response, textStatus, xhr) => any) {
        $.ajax({
            type: "PUT",
            url: this.getBaseUrl() + url,
            dataType: "xml",
            contentType: "application/xml;charset=UTF-8",
            data: data,
            success: fsuccess,
            error: ferror
        });
    }

    public static PostREST(url, data, fsuccess: (response, textStatus, xhr) => any, ferror: (response, textStatus, xhr) => any) {
        $.ajax({
            type: "POST",
            url: this.getBaseUrl() + url,
            dataType: "xml",
            contentType: "application/xml;charset=UTF-8",
            data: data,
            success: fsuccess,
            error: ferror
        });
    }

    public static DeleteREST(url, fsuccess: (response) => any, ferror: (response) => any) {
        $.ajax({
            type: "DELETE",
            url: this.getBaseUrl() + url,
            success: fsuccess,
            error: ferror
        });
    }
    public static JsonREST = function (url, data, fsuccess, ferror) {
        $.ajax({
            type: "POST",
            url: this.getBaseUrl() + url,
            dataType: "json",
            data: data,
            success: fsuccess,
            error: ferror
        });
    };



    public static JsonGetREST = function (url, fsuccess, ferror) {

        $.ajax({
            type: "GET",
            url: this.getBaseUrl() + url,
            dataType: "json",
            success: fsuccess,
            error: ferror
        });
    };


    public static getBaseUrl() {
        if (window.location.toString().indexOf("safenetpro") >= 0)
            return "http://netsafepro.safenetpro.com/api/";
        else if (window.location.toString().indexOf("buyschoolbook") >= 0)
            return "http://buyschoolbook.com/api/";
        else if (window.location.toString().indexOf("localhost") >= 0)
            return "http://localhost:50751/api/";
        else if (window.location.toString().indexOf("netsafepro") >= 0)
            return "https://www.netsafepro.com/api/";
    }
}