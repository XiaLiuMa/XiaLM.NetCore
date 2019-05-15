//标准json格式 目前只支持[{a:b,c:d},{a:b,c:d}]此种格式 
// var json = [
//     {
//       Ip: "192.168.110.56",
//       Port: 21,
//       Uname: "xlm",
//       Pwd: "123456",
//       Guid: "7570bedd-7518-4326-aaf5-89afb71bf54b"
//     },
//     {
//       Ip: "192.168.110.56",
//       Port: 21,
//       Uname: "xlm",
//       Pwd: "123456",
//       Guid: "7570bedd-7518-4326-aaf5-89afb71bf54b"
//     },
//     {
//       Ip: "192.168.110.56",
//       Port: 21,
//       Uname: "xlm",
//       Pwd: "123456",
//       Guid: "7570bedd-7518-4326-aaf5-89afb71bf54b"
//     }
//   ];
var json = [];
var urlIp = GetUrlParam("ip");
if (urlIp == "") {
    console.log(1);
    //  页面加载前url没有数据时请求表格数据
    $.ajax({
        url: "http://" + params + "/configFtp/GetData",
        type: "get",
        async: false,
        success: function (res) {
            // console.log(res);
            // 如果请求成功，将返回的json数组赋给数组json
            if (res) {
                if (res == "nologin") {
                    window.location.href = "login.html"
                } else {
                    json = res;
                    localStorage.setItem("ftpArr", JSON.stringify(json));
                }
            }

        }
    })
} else {
    $("input[name='ip']").val(urlIp);
    console.log(2);
    //  页面加载前url有ip数据时请求表格数据
    $.ajax({
        url: "http://" + params + "/FtpServer/Query",
        type: "get",
        data: {
            query: urlIp,
            page: 1,
            rows: 10
        },
        async: false,
        success: function (res) {
            // 如果请求成功，将返回的json数组赋给数组json
            if (res) {
                if (res == "nologin") {
                    window.location.href = "login.html"
                } else {
                    json = res;
                }
            }
        }
    });
}

// 初始化表格
function updataPage() {
    mypage.setCfg({
        table: "table",
        bar: "pageBar",
        limit: 10,   //控制默认一页显示多少行数据
        color: "#1E9FFF",
        layout: ["count", "prev", "page", "next", "limit", "skip"]
    }); //初始化完成
}
//nameList与widthList的数组长度要一致
var nameList = ["", "序号", "服务器地址", "服务器端口", "用户名", "密码"]; //table的列名
var widthList = [10, 10, 100, 100, 100, 100]; //table每列的宽度
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
    console.log(delArr)
    for (var i = 0; i < delArr.length; i++) {
        ftparr.splice(getArrIndex(ftparr, delArr[i]), 1);

    }

    console.log(ftparr)
    localStorage.setItem("ftpArr", JSON.stringify(ftparr));
    fn();
    //    location.reload();
});

$(function () {
    // 加载页面时初始化表格
    updataPage();
    // 点击添加按钮
    $("#add").on("click", function () {
        console.log(JSON.parse(localStorage.getItem("ftpArr")));
        $("#confirm").attr("czlx", "0");
        $(".peizhi").css("display", "block");
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
});

// 获取地址栏后的指定参数
function GetUrlParam(paraName) {
    var url = document.location.toString();
    var arrObj = url.split("?");
    if (arrObj.length > 1) {
        var arrPara = arrObj[1].split("&");
        var arr;
        for (var i = 0; i < arrPara.length; i++) {
            arr = arrPara[i].split("=");
            if (arr != null && arr[0] == paraName) {
                return arr[1];
            }
        }
        return "";
    } else {
        return "";
    }
}

function fn() {
    var ftpArr = JSON.parse(localStorage.getItem("ftpArr"));
    var obj = {
        FtpArray: ftpArr
    }
    var str = JSON.stringify(obj);
    $.ajax({
        url: "http://" + params + "/configFtp/SaveData",
        // url:"http://192.168.110.58:3333/testform",
        type: "post",
        async: false,
        data: str,
        //  dataType:"json",
        contentType: "application/json",
        error: function () {
            console.log("错误");
        },
        success: function (res) {
            console.log(res);
            // 如果请求成功，将返回的json数组赋给数组json
            if (res) {
                // json = res;
                // localStorage.setItem("ftpArr", JSON.stringify(json));
                // location.reload();
                if (res == "nologin") {
                    window.location.href = "login.html"
                } else {
                    json = res;
                    localStorage.setItem("ftpArr", JSON.stringify(json));
                    location.reload();
                }
            }
        }
    })
}
function getArrIndex(arr, obj) {
    let index = null;
    let key = Object.keys(obj)[0];
    arr.every(function (value, i) {
        if (value[key] === obj[key]) {
            index = i;
            return false;
        }
        return true;
    });
    return index;
};