// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var checker = document.getElementById('checkme');
var sendbtn = document.getElementById('finalVoteBtn');
// when unchecked or checked, run the function
checker.onchange = function () {
    if (this.checked) {
        sendbtn.disabled = false;
    } else {
        sendbtn.disabled = true;
    }

}
