// Write your Javascript code.
$(document).ready(function() {
    $("#PasswordForm").submit(function(e){
        let password = $("#Password").val(), passwordConfirm = $("#PasswordConfirm").val();
        if (password.length < 8) {
            $("#PasswordErrors").val("Passwords must be 8 or more characters");
            e.preventDefault();
        }
        else if (password != passwordConfirm)
        {
            $("#PasswordErrors").val("Password fields must match");
            e.preventDefault();
        }
    });
    $("#CommentForm").submit(function(e) {
        let comment = $("#Comment").val()
        if (comment.length <= 2) {
            $("#CommentErrors").val("Comments must be 2 or more characters.");
            $("#CommentErrors").html("Comments must be 2 or more characters.")
            e.preventDefault();
        } else if (comment.length > 500) {
            $("#CommentErrors").val("Comments must be 500 or fewer characters.");
            e.preventDefault();
        }
    });
    $(".ReplyForm").submit(function(e) {
        let reply = this.firstElementChild.value, targetString = "#ReplyError" + this[1].value;
        if (reply.length < 3) {
            e.preventDefault();
            $(targetString).val("Replies must be 3 or more characters");
        }
        if (reply.length > 500) {
            e.preventDefault();
            $(targetString).val("Replies must be 3 or more characters");
        }
    });
})