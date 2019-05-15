var json = [];
//nameList与widthList的数组长度要一致
var nameList = ['', '序号', '数据库名', '服务器地址', '服务器端口', '是否默认连接'] //table的列名
var widthList = [5, 10, 100, 100, 100, 100, 100] //table每列的宽度

$(function () {
    UpdataPage();   // 加载页面时初始化表格
    DataBaseQuery();    //初始化表格数据

    // 点击查询按钮
    $("#select").on("click", function () {
        var ip = $("input[name='ip']").val();
        if (!ip) {
            alert("请输入要查询的ip！");
        }
        else {
            DataBaseQuery();
        }
    })

    // 点击删除按钮
    $("#delete").on("click", function () {
        if ($("input[name='checktr']:checked").length <= 0) {
            alert("请至少选择其中一项数据进行删除！")
            return false;
        }
        var delArr = [];
        // 删除选择的列表项
        $(".trcheck").each(function (i, item) {
            if ($(this).prop('checked')) {
                var a = $(this).parents("tr").find("td").eq(7).text();
                delArr.push(a);
            }
        })
        $.ajax({
            url: "http://" + params + "/DataBase/Delete",
            type: "post",
            data: {
                ids: delArr
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                // 如果请求成功，将返回的json数组赋给数组json
                if (res == "true") {
                    DataBaseQuery();
                    location.reload();
                } else {
                    alert(res)
                }
            }
        })
    })

    // 点击添加按钮
    $("#add").on("click", function () {
        $("#confirm").attr("czlx", "0");
        $(".peizhi").css("display", "block");
    })
    // 点击修改按钮
    $("#update").on("click", function () {
        if ($("input[name='checktr']:checked").length != 1) {
            alert("请选择其中一项数据进行修改！")
            return false;
        }
        $(".trcheck").each(function (i, item) {
            if ($(this).prop('checked')) {
                console.log(json);
                $("input[name='dbname']").val(json[i].serviceName);
                $("input[name='svip']").val(json[i].serverIp);
                $("input[name='svport']").val(json[i].serverPort);
                $("input[name='ljcheck']").prop("checked", json[i].default);
            }
        })
        $("#confirm").attr("czlx", "1");
        $(".peizhi").css("display", "block");
    })

    // 点击关闭按钮
    $(".close").on("click", function () {
        $(".peizhi").css("display", "none");
        $(".pz-content input[type='text']").val("");
    })
    // 点击确定按钮
    $("#confirm").on("click", function () {
        var dbname = $("input[name='dbname']").val();
        var svip = $("input[name='svip']").val();
        var dbport = parseInt($("input[name='svport']").val());
        var idefault = $("input[name='ljcheck']").is(':checked') ? "是" : "否";

        if ($(this).attr("czlx") == "0") {
            //添加操作
            $.ajax({
                url: "http://" + params + "/DataBase/Add",
                type: "post",
                data: {
                    serverName: dbname,
                    serverIp: svip,
                    serverPort: dbport,
                    isDefault: idefault
                },
                async: false,
                error: function () {
                    console.log("错误");
                },
                success: function (res) {
                    // 如果请求成功，将返回的json数组赋给数组json
                    if (res == "true") {
                        DataBaseQuery();
                        location.reload();
                    } else {
                        alert(res)
                    }
                }
            })
        } else {
            var id;
            var guid;
            $(".trcheck").each(function (i, item) {
                if ($(this).prop('checked')) {
                    id = $(this).parents("tr").find("td").eq(6).text();
                    guid = $(this).parents("tr").find("td").eq(7).text();
                }
            })
            //修改操作
            $.ajax({
                url: "http://" + params + "/DataBase/Alert",
                type: "post",
                data: {
                    id: id,
                    dGuid: guid,
                    serverName: dbname,
                    serverIp: svip,
                    serverPort: dbport,
                    isDefault: idefault
                },
                async: false,
                error: function () {
                    console.log("错误");
                },
                success: function (res) {
                    // 如果请求成功，将返回的json数组赋给数组json
                    if (res == "true") {
                        DataBaseQuery();
                        location.reload();
                    } else {
                        alert(res)
                    }
                }
            })
        }
        $(".peizhi").css("display", "none");
    })
    // 点击取消按钮
    $("#cancel").on("click", function () {
        $(".peizhi").css("display", "none");
        $(".pz-content input[type='text']").val("");
    })
});


//数据库数据查询
function DataBaseQuery() {
    var query = $("input[name='ip']").val();
    $.ajax({
        url: "http://" + params + "/DataBase/Query",
        type: "get",
        data: {
            query: query,
            page: 1,
            rows: 10
        },
        async: false,
        success: function (res) {
            // 如果请求成功，将返回的json数组赋给数组json
            if (res) {
                json = res;
            }
        }
    })
}
