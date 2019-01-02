// Write your Javascript code.
console.log("Javascript Running!");

$(document).ready(function () {
    console.log("Inside!");
    $.ajax({
        type: "POST",
        url: '/HomeController/UploadFileToRemoteLocation',
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        alert(data);
    }

    function errorFunc(status, data, excep) {
        console.log(status);
        console.log(data);
        console.log(excep);
    }
});