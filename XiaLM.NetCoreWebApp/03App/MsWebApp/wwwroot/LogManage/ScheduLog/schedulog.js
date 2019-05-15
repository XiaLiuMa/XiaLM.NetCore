var json = [];  //列表数据
//nameList与widthList的数组长度要一致
var nameList = ["序号", "任务名称", "任务开始时间", "任务结束时间", "备注"]; //table的列名
var widthList = [10, 100, 100, 100, 100]; //table每列的宽度

//Main
$(function () {
    UpdataPage();   //表格初始化
    PaggInit(); //页面初始化

    // 点击查询按钮
    $("#select").on("click", function () {
        ScheduLogQuery();
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
            url: "http://" + params + "/ScheduLog/Delete",
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
                    ScheduLogQuery();  //刷新界面
                }
            }
        });

    });

    // 点击清除按钮
    $("#clear").on("click", function () {
        $.ajax({
            url: "http://" + params + "/ScheduLog/Clear",
            type: "post",
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                if (res) {
                    ScheduLogQuery();  //刷新界面
                }
            }
        });
    });

    // 点击导出当前页按钮
    $("#exportpage").on("click", function () {
        var stime = $("input[name='sTime']").val();
        var etime = $("input[name='eTime']").val();
        var jobname = $("input[name='jobName']").val();
        $.ajax({
            url: "http://" + params + "/ScheduLog/ExportPage",
            type: "post",
            data: {
                sTime: stime,
                eTime: etime,
                jobName: jobname,
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
        var jobname = $("input[name='jobName']").val();
        $.ajax({
            url: "http://" + params + "/ScheduLog/ExportAll",
            type: "post",
            data: {
                sTime: stime,
                eTime: etime,
                jobName: jobname
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
    ScheduLogQuery();  //操作日志查询
}

//查询调度日志
function ScheduLogQuery() {
    var stime = $("input[name='sTime']").val();
    var etime = $("input[name='eTime']").val();
    var jobname = $("input[name='jobName']").val();
    var sort = "",   //正序或倒序
    var order = "",   //排序依据
    if (!sTime || !eTime) {
        alert("请正确填写要查询的时间段！");
    }
    else {
        $.ajax({
            url: "http://" + params + "/ScheduLog/Query",
            type: "get",
            data: {
                sTime: stime,
                eTime: etime,
                jobName: jobname,
                page: 1,
                rows: 10,
                sort: sort,
                order: order
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


