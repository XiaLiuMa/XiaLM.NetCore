var json = [];
//nameList与widthList的数组长度要一致
var nameList = ["", "序号", "服务器地址", "服务器端口", "用户名", "密码"]; //table的列名
var widthList = [10, 10, 100, 100, 100, 100]; //table每列的宽度

$(function () {
    // 加载页面时初始化表格
    UpdataPage();

    // 点击添加按钮
    $("#add").on("click", function () {
        console.log(JSON.parse(localStorage.getItem("ftpArr")));
        $("#confirm").attr("czlx", "0");
        $(".peizhi").css("display", "block");


        $.ajax({
            url: "http://" + params + "/FtpServer/Add",
            type: "get",
            data: {
                ip: ip,
                port: port,
                uname: uname,
                pwd: pwd
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                // 如果请求成功，将返回的json数组赋给数组json
                if (res) {
                    if (res == "false") {
                        alert("添加失败");
                    }
                    else {
                        FtpServerQuery();   //刷新数据
                    }
                }
            }
        });
    });

    // 点击修改按钮
    $("#update").on("click", function () {
        if ($("input[name='checktr']:checked").length != 1) {
            alert("请选择其中一项数据进行修改！");
            return false;
        }
        $(".trcheck").each(function (i, item) {
            if ($(this).prop("checked")) {
                console.log(json[i]);
                var tds = $(this).parents("td").siblings();
                $("input[name='svip']").val(tds.eq(1).text());
                $("input[name='svport']").val(tds.eq(2).text());
                $("input[name='sname']").val(tds.eq(3).text());
                $("input[name='spwd']").val(tds.eq(4).text());
                $("#confirm").attr("guid", tds.eq(5).text());
            }
        });
        $("#confirm").attr("czlx", "1");
        $(".peizhi").css("display", "block");


        $.ajax({
            url: "http://" + params + "/FtpServer/Alert",
            type: "get",
            data: {
                id: id,
                fguid: fguid,
                ip: ip,
                port: port,
                uname: uname,
                pwd: pwd
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                // 如果请求成功，将返回的json数组赋给数组json
                if (res) {
                    if (res == "false") {
                        alert("修改失败");
                    }
                    else {
                        FtpServerQuery();   //刷新数据
                    }
                }
            }
        });
    });

    // 点击关闭按钮
    $(".close").on("click", function () {
        $(".peizhi").css("display", "none");
        $(".pz-content input[type='text']").val("");
    });
    // 点击确定按钮
    $("#confirm").on("click", function () {
        var sname = $("input[name='sname']").val();
        var svip = $("input[name='svip']").val();
        var dbport = $("input[name='svport']").val();
        var spwd = $("input[name='spwd']").val();
        if ($(this).attr("czlx") == "0") {
            // 如果czlx值为0执行添加操作
            var Guid = getNum();
            var insertele = {
                Ip: svip,
                Port: dbport,
                Uname: sname,
                Pwd: spwd,
                Guid: Guid
            };
            var addArr = JSON.parse(localStorage.getItem("ftpArr"));
            addArr.push(insertele);
            // console.log(addArr);
            localStorage.setItem("ftpArr", JSON.stringify(addArr));
        } else {
            // 如果czlx值为1执行修改操作操作
            var guid = $("#confirm").attr("guid");
            var updateele = {
                Ip: svip,
                Port: dbport,
                Uname: sname,
                Pwd: spwd,
                Guid: guid
            };
            var updateArr = JSON.parse(localStorage.getItem("ftpArr"));
            for (var i = 0; i < updateArr.length; i++) {
                if (updateArr[i].guid == guid) {
                    updateArr[i] = updateele;
                }
            }
            console.log(updateArr);
            localStorage.setItem("ftpArr", JSON.stringify(updateArr));
        }

        $(".peizhi").css("display", "none");
        fn();
        // location.reload();
    });
    // 点击取消按钮
    $("#cancel").on("click", function () {
        $(".peizhi").css("display", "none");
        $(".pz-content input[type='text']").val("");
    });

    // 点击查询按钮
    $("#select").on("click", function () {
        var ip = $("input[name='ip']").val();
        if (!ip) {
            window.location.href = "ftp.html";
        } else {
            if (isIP(ip)) {
                window.location.href = "ftp.html?ip=" + ip;
            } else {
                alert("请输入正确的ip地址进行查询！");
            }
        }
    });

    // 点击删除按钮
    $("#delete").on("click", function () {
        var delArr = [];
        var ftparr = JSON.parse(localStorage.getItem("ftpArr"));
        // 删除选择的列表项
        $(".trcheck").each(function (i, item) {
            if ($(this).prop("checked")) {
                // $(this).parents("tr").remove();
                delArr.push(ftparr[i]);
            }
        });
        for (var i = 0; i < delArr.length; i++) {
            ftparr.splice(getArrIndex(ftparr, delArr[i]), 1);

        }
        console.log(ftparr)
        localStorage.setItem("ftpArr", JSON.stringify(ftparr));



        $.ajax({
            url: "http://" + params + "/FtpServer/Delete",
            type: "get",
            data: {
                ids: ids
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                // 如果请求成功，将返回的json数组赋给数组json
                if (res) {
                    if (res == "false") {
                        alert("删除失败");
                    }
                    else {
                        FtpServerQuery();   //刷新数据
                    }
                }
            }
        });
    });
});

//查询Ftp服务器
function FtpServerQuery() {
    var query = $("input[name='query']").val();
    if (!sTime || !eTime) {
        alert("请正确填写要查询的时间段！");
    }
    else {
        $.ajax({
            url: "http://" + params + "/FtpServer/Query",
            type: "get",
            data: {
                query: query,
                page: 1,
                rows: 10
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                // 如果请求成功，将返回的json数组赋给数组json
                if (res) {
                    json = res;
                    //localStorage.setItem("operationLogArr", JSON.stringify(res));
                }
            }
        });
    }
}