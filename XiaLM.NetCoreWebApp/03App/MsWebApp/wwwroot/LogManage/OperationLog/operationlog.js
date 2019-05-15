var json = [];  //列表数据
//nameList与widthList的数组长度要一致
var nameList = ["", "序号", "操作模块", "操作类型", "操作时间", "操作人", "操作IP", "操作说明"]; //table的列名
var widthList = [10, 10, 100, 100, 100, 100, 100, 100]; //table每列的宽度

//Main
$(function () {
    UpdataPage();   //表格初始化
    PaggInit(); //页面初始化

    // 点击查询按钮
    $("#select").on("click", function () {
        OperationLogQuery();
    });

    // 点击删除按钮
    $("#delete").on("click", function () {
        var delArr = "";
        // 删除选择的列表项
        $(".trcheck").each(function (i, item) {
            if ($(this).prop("checked")) {


                // $(this).parents("tr").remove();
                delArr.push(arr[i]);
            }
        });

        $.ajax({
            url: "http://" + params + "/OperationLog/Delete",
            type: "post",
            data: {
                ids: delArr
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                if (res) {
                    OperationLogQuery();  //刷新界面
                }
            }
        });

    });

    // 点击清除按钮
    $("#clear").on("click", function () {
        $.ajax({
            url: "http://" + params + "/OperationLog/Clear",
            type: "post",
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                if (res) {
                    OperationLogQuery();  //刷新界面
                }
            }
        });
    });

    // 点击导出当前页按钮
    $("#exportpage").on("click", function () {
        var stime = $("input[name='sTime']").val();
        var etime = $("input[name='eTime']").val();
        var etype = $("input[name='eType']").val();
        $.ajax({
            url: "http://" + params + "/OperationLog/ExportPage",
            type: "post",
            data: {
                cType: etype,
                sTime: stime,
                eTime: etime,
                page: 1,
                rows: 10
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                if (res) {
                    alert("导出成功到" + res);
                }
            }
        });
    });

    // 点击导出符合条件所有页按钮
    $("#exportall").on("click", function () {
        var stime = $("input[name='sTime']").val();
        var etime = $("input[name='eTime']").val();
        var etype = $("input[name='eType']").val();
        $.ajax({
            url: "http://" + params + "/OperationLog/ExportAll",
            type: "post",
            data: {
                cType: etype,
                sTime: stime,
                eTime: etime
            },
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                if (res) {
                    alert("导出成功到" + res);
                }
            }
        });
    });
});

//页面初始化
function PaggInit() {
    //初始化操作类型列表
    $.ajax({
        url: "http://" + params + "/OperationLog/GetOperationTypes",
        type: "get",
        async: false,
        error: function () {
            console.log("请求错误");
        },
        success: function (res) {
            console.log(res);
            // 如果请求成功，将返回的json数组赋给数组json
            if (res) {
                var html = "";
                for (var i = 0; i < res.length; i++) {
                    html += "<option>" + res[i].serverIp + "</option>";
                }
                $("#typeSelect").html(html);
            }
        }
    })

    OperationLogQuery();  //操作日志查询
}

//初始化操作类型列表
function OperationLogQuery() {
    var stime = $("input[name='sTime']").val();
    var etime = $("input[name='eTime']").val();
    var slevel = $("#typeSelect option:selected").text();
    if (!sTime || !eTime) {
        alert("请正确填写要查询的时间段！");
    }
    else {
        $.ajax({
            url: "http://" + params + "/OperationLog/Query",
            type: "get",
            data: {
                cType: etype,
                sTime: stime,
                eTime: etime,
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


