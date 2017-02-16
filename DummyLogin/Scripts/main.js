$().ready(function () {

    HideLoader();

    function HideForm()
    {
        $("#loginForm").hide();
    }

    function ShowLoader()
    {
        $("#loader").show();
    }

    function HideLoader() {
        $("#loader").hide();
    }

    $('#login').click(function () {

        var login = new Object();

        login.Username = $("#username").val();
        login.Password = $("#password").val();
        var jLogin = JSON.stringify(login);

        HideForm();

        $.ajax({
            url: "login.asmx/getUser",
            //   url: "http://api.apixu.com/v1/current.json?key=3b6ea19016194bb8905122956171402&q=pretoria",
            contentType: "application/json; charset=utf-8",                                       
            method: "Get",
            dataType: 'json',                    
            data: { login: jLogin },
            beforeSend: function(){
                ShowLoader();
            },
            conplete: function (jqXHR, textStatus) {
                console.log(jqXHR);
                HideLoader();
            },
            success: function(response)
            {
                var _data = JSON.parse(response.d);
                console.log(_data);
                HideLoader();

                if (_data.Success)
                {                          
                    $("#isUserValid").text("username and password valid");
                    $("#isUserValid").css({ 'color': 'green', 'font-size': '150%' });
                    $("#loginForm").hide();
                }
                else
                {
                    $("#isUserValid").text("username and password invalid");
                    $("#isUserValid").css({ 'color': 'red', 'font-size': '150%' });
                    $("#username").css({ 'border-color': 'red' });
                    $("#password").css({ 'border-color': 'red' });
                }                        
            },
            error: function(err)
            {                        
                console.log(err);
                $("#isUserValid").text("An error occured. Please contact the webmaster");
            }

        });
    });
});