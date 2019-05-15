var json = [];  //列表数据
//nameList与widthList的数组长度要一致
var nameList = ["", "序号", "时间", "级别", "内容", "方法", "数量", "堆栈"]; //table的列名
var widthList = [10, 10, 100, 100, 100, 100, 100, 100]; //table每列的宽度

//Main
$(function () {
    UpdataPage();   //表格初始化
    InitData(); //初始化界面所需数据
    ConventionLogQuery(); //初始化常规日志数据

    // 点击查询按钮
    $("#select").on("click", function () {
        ConventionLogQuery();   //查询常规日志数据
    });

    // 点击删除按钮
    $("#delete").on("click", function () {
        var delArr = "";
        // 删除选择的列表项
        $(".trcheck").each(function (i, item) {
            if ($(this).prop("checked")) {


                // $(this).parents("tr").remove();
                //delArr.push(arr[i]);
            }
        });

        $.ajax({
            url: "http://" + params + "/ConventionLog/Delete",
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
                    OperationLogQuery("", "", "");  //刷新界面
                }
            }
        });

    });

    // 点击清除按钮
    $("#clear").on("click", function () {
        $.ajax({
            url: "http://" + params + "/ConventionLog/Clear",
            type: "post",
            async: false,
            error: function () {
                console.log("错误");
            },
            success: function (res) {
                if (res) {
                    OperationLogQuery("", "", "");  //刷新界面
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
            url: "http://" + params + "/ConventionLog/ExportPage",
            type: "post",
            data: {
                sTime: stime,
                eTime: etime,
                cType: etype,
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
            url: "http://" + params + "/ConventionLog/ExportAll",
            type: "post",
            data: {
                sTime: stime,
                eTime: etime,
                cType: etype,
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
});


//初始化界面需要的数据
function InitData() {
    //初始化时间控件
    {
        //$(".datetimepicker").datetimepicker({
        //    // startDate: new Date(),
        //    format: "yyyy-mm-dd hh:ii:ss",
        //    autoclose: true,
        //    minView: 0,
        //    minuteStep: 1,
        //    language: "zh-CN",
        //    initialDate: new Date(), //初始化当前日期
        //    todayBtn: true
        //});


        //var now = new Date();
        ////格式化日，如果小于9，前面补0 
        //var day = ("0" + now.getDate()).slice(-2);
        ////格式化月，如果小于9，前面补0 
        //var month = ("0" + (now.getMonth() + 1)).slice(-2);
        ////拼装完整日期格式 
        //var time1 = now.getFullYear() + "/" + (month) + "/" + (day) + " " + "00:00:00";
        //var time2 = now.getFullYear() + "/" + (month) + "/" + (day) + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
        ////完成赋值 
        //$("#sTime").value(time1);
        //$("#eTime").value(time2);
    }


    $.ajax({
        url: "http://" + params + "/ConventionLog/GetServices",
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
                    //html += "<option>" + res[i].serverIp + "</option>";
                    html += "<option>" + res[i] + "</option>";
                }
                $("#serviceSelect").html(html);
            }
        }
    });

    $.ajax({
        url: "http://" + params + "/ConventionLog/GetLevels",
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
                    html += "<option>" + res[i] + "</option>";
                }
                $("#serviceLevel").html(html);
            }
        }
    })
}

//查询常规日志
function ConventionLogQuery() {
    //模拟数据
    {
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
    }

    var sservice = $("input[name='sService']").val();
    var slevel = $("#serviceSelect option:selected").text();
    var stime = $("input[name='sTime']").val();
    var etime = $("input[name='eTime']").val();
    if (!stime || !etime) {
        alert("请正确填写要查询的时间段！");
    }
    else {
        $.ajax({
            url: "http://" + params + "/ConventionLog/Query",
            type: "get",
            data: {
                sService: sservice,
                sLevel: slevel,
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
                    //localStorage.setItem("conventionLogArr", JSON.stringify(res));
                }
            }
        });
    }
}