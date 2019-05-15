
//Main
$(function () {

    //退出登录操作
    $(".logout").on("click", function () {
        $.ajax({
            url: "http://" + params + "/User/OutLogin",
            type: "post",
            data: {},
            success: function (res) {
                if (res == true) {
                    window.location.href = "login.html";
                }
            }
        })
    })


    //首页
    $("#Index").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/Home/default.html");
    })


    //系统管理
    $("#SystemManage").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/DataBase/database.html");
    })
    //系统管理--数据库
    $("#SM_DataBase").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/DataBase/database.html");
    })
    //系统管理--Ftp服务器
    $("#SM_FtpServer").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/FtpServer/ftpserver.html");
    })
    //系统管理--发布者配置
    $("#SM_Publisher").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/Publisher/publisher.html");
    })
    //系统管理--消费者配置
    $("#SM_Consumer").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/Consumer/consumer.html");
    })
    //系统管理--手动备份
    $("#SM_BeiFen").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/BeiFen/beifen.html");
    })



    //日志管理
    $("#LogManage").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/LogManage/ConventionLog/conventionlog.html");
    })
    //日志管理--常规日志
    $("#LM_ConventionLog").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/LogManage/ConventionLog/conventionlog.html");
    })
    //日志管理--操作日志
    $("#LM_OperationLog").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/LogManage/OperationLog/operationlog.html");
    })
    //日志管理--调度日志
    $("#LM_ScheduLog").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/LogManage/ScheduLog/schedulog.html");
    })



    //系统配置
    $("#SystemConfig").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemConfig/BaseConfig/baseconfig.html");
    })
    //系统配置--基础配置
    $("#SC_BaseConfig").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemConfig/BaseConfig/baseconfig.html");
    })
    //系统配置--串口配置
    $("#SC_SerialPort").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/SerialPort/serialport.html");
    })
    //系统配置--任务调度
    $("#SC_Scheduler").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/Scheduler/scheduler.html");
    })
    //系统配置--MQ配置
    $("#SC_RabbitMq").click(function () {
        $(".Ms_iframe").attr("src", "../wwwroot/SystemManage/RabbitMq/rabbitmq.html");
    })


});