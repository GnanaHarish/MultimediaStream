    $("#regbutton").click(function (event) {
        /*alert("Hi");*/
        event.preventDefault()
        $.ajax({
            url: "/api/register",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({
                Email: $('#email').val(),
                Password: $('#password').val().trim(),
                UserName: $('#username').val().trim()
            }),
            success: function () {
                console.log("Sucess");
                //alert("Hello");
                alert("The user has been sucessfully created!, You will be redirected to the login page now!");
                window.location.href = "login";
            },
            error: function () {
                alert("Either the user is already exists or try again!")
            }
        })
    })

$("#registerredirect").click(function (event) {
    event.preventDefault();
    window.location.href = "Register";
})

$("#loginredirect").click(function (event) {
    event.preventDefault();
    window.location.href = "login";
})


$("#login").click(function (event) {
    //alert("Hi");
    event.preventDefault()
    $.ajax({
        url: "/api/login",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({
            Email: $('#email').val().trim(),
            Password: $('#password').val().trim(),
            UserName : ""
        }),
        success: function () {
            /*//console.log("Sucess");*/
            //alert("Hello");
            window.location.href = "homepage?email=" + $('#email').val().trim();
        },
        error: function () {
            alert("Login Failed, Please try again with correct, Email and Password");
        }
    })
})




$("#uploadmore").click(function (event) {
    event.preventDefault();
    var name = $("#name").text().trim();
    window.location.href = "upload?name="+name;
})


$("#logout").click(function (event) {
    event.preventDefault();
    $.ajax({
        url: "/api/logout",
        type: "GET",
        success: function () {
            /*//console.log("Sucess");*/
            //alert("Hello");
            window.location.href = "Login";
        },
        error: function () {
            alert("Login Failed, Please try again with correct, Email and Password");
        }
    })
})
