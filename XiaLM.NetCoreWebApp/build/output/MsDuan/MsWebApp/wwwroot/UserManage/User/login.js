$(".login_btn").on("click", function () {
    var uname = $("input[name='uname']").val();
    var pwd = $("input[name='pwd']").val();
    console.log(location.href);

    $.ajax({
        url: "http://" + params + "/User/CkLogin",
        type: "Post",
        data: {
            uname: uname,
            pwd: pwd
        },
        dataType: 'json',
        success: function (res) {
            console.log(res);
            switch (res.rstate)
            {
                case 1002:  //登陆成功
                    {
                        window.location.href = "http://" + params + "/Home/Index";
                    }
                    break;
                case 1004:  //登陆失败
                    {
                        alert("用户名或密码错误！");
                    }
                    break;
            }

            //if (res == true) {
            //    //// 如果成功就跳转到主页
            //    //if (window.location.href.indexOf("wwwroot") != -1) {
            //    //    window.location.href = "index.html";
            //    //}
            //    //else {
            //    //    window.location.href = "wwwroot/index.html";
            //    //}
            //    //// window.location.href="wwwroot/index.html"
            //    window.location.href = "http://" + params + "/Home/Index";
            //} else {
            //    alert("用户名或密码错误！");
            //}
        }
    })
})
$(function () {
    $('.pagecontent').css('height', document.documentElement.clientHeight - 70 + 'px');
});