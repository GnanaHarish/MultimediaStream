
var name = $("#name").text().trim();

$("#seeown").click(function (event) {
    //alert("Hi");
    event.preventDefault()
    window.location.href = "UserFiles?name=" + name;
})