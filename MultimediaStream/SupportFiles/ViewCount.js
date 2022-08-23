
    $('a').click(function (event) {
        
        var idx = $(this).attr('href').lastIndexOf("/");
        var href = $(this).attr('href').substring(idx + 1);
        $.ajax({
            url: "/api/updatevideo",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({
                URL: href,
            }),
            success: function () {
                console.log("Sucess");
                //alert("Hello");
                //alert("The user has been sucessfully created!, You will be redirected to the login page now!");
                //window.location.href = "login";
            },
            error: function () {
                console.log("Error");
                //alert("Either the user is already exists or try again!")
            }
        })
    });
