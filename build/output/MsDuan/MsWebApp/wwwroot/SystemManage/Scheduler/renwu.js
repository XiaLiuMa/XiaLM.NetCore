//标准json格式 目前只支持[{a:b,c:d},{a:b,c:d}]此种格式 
var json =[
    // {
    //   "JobName": "出入境人员生物信息备份",
    //   "ServiceName": "Jobs_CRJSWXXBF.CRJSWXXBFService",
    //   "Expresstion": "0 0/1 * * * ?",
    //   "State": true,
    //   "DbGuid": "7570bedd-7518-4326-aaf5-89afb71bf54b",
    //   "Guid": "da08a5f4-6ffa-44c9-9564-f030c44b789e"
    // },
    // {
    //   "JobName": "出入境人员生物信息备份",
    //   "ServiceName": "Jobs_CRJSWXXBF.CRJSWXXBFService",
    //   "Expresstion": "0 0/1 * * * ?",
    //   "State": true,
    //   "DbGuid": "7570bedd-7518-4326-aaf5-89afb71bf54b",
    //   "Guid": "da08a5f4-6ffa-44c9-9564-f030c44b789e"
    // },
    // {
    //   "JobName": "出入境人员生物信息备份",
    //   "ServiceName": "JobsCRJSWXXBF.CRJSWXXBFService",
    //   "Expresstion": "0 0/1 * * * ?",
    //   "State": true,
    //   "DbGuid": "7570bedd-7518-4326-aaf5-89afb71bf54b",
    //   "Guid": "da08a5f4-6ffa-44c9-9564-f030c44b789e"
    // }
    ]

var urlTask = GetUrlParam("jobName");
if (urlTask == "") {
//    页面加载前url没有数据时请求表格数据
   $.ajax({
      url:"http://"+params+"/configQuartz/GetData",
      type:"get",
      async:false,
      success:function(res){
          if(res){
            if(res=="nologin"){
                window.location.href = "login.html"
            }else{
              json = res;
              localStorage.setItem("taskArr", JSON.stringify(json));
            }
          }
          // 如果请求成功，将返回的json数组赋给数组json 并保存到本地
         
      }
  })
} else {
  console.log(urlTask);
  $("input[name='task']").val(urlTask);
  //  页面加载前url有ip数据时请求表格数据
  $.ajax({
    url: "http://"+params+"/configQuartz/QueryData",
    type: "get",
    data: {
        jobName: urlTask
    },
    dataType:"json",
    async: false,
    success: function(res) {
      if(res){
        if(res=="nologin"){
            window.location.href = "login.html"
        }else{
          json = res;
        }
      }
     ;
    }
  });
}

localStorage.setItem("taskArr", JSON.stringify(json));
// 初始化任务名select
$.ajax({
    url:"http://"+params+"/configQuartz/GetServices",
    type:"get",
    data:{},
    success:function(res){
        // 如果请求返回数据正常 初始化
        if(res){
          // console.log(res);
            // 传入返回的数组
          selectHtml2(res);   
        }
    }
})
// 初始化数据路select
$.ajax({
    url:"http://"+params+"/dbConfig/GetData",
    type:"get",
    data:{},
    success:function(res){
        // 如果请求返回数据正常 初始化
        console.log(res);
        if(res){
            // 传入返回的数组
          selectHtml(res);   
        }
    }
})

// 退出登录操作
$(".logout").on("click", function() {
  $.ajax({
    url: "http://"+params+"/login/OutLogin",
    type: "post",
    data: {},
    success: function(res) {
      if (res == true) {
        window.location.href = "login.html";
      }
    }
  });
});

// 初始化表格
function updataPage() {
  mypage.setCfg({
    table: "table",
    bar: "pageBar",
    limit: 2,   //控制默认一页显示多少行数据
    color: "#1E9FFF",
    layout: ["count", "prev", "page", "next", "limit", "skip"]
  }); //初始化完成
}
//nameList与widthList的数组长度要一致
var nameList = ['','序号','调度任务名称','调度服务名','运行计划','运行状态'] //table的列名
var widthList = [10,10, 100, 100,100,100] //table每列的宽度
// 点击删除按钮
$("#delete").on("click", function() {
  var delArr = [];
  // 删除选择的列表项
  var taskarr = JSON.parse(localStorage.getItem("taskArr"));
  $(".trcheck").each(function(i, item) {
    if ($(this).prop("checked")) {
      // $(this).parents("tr").remove();
      delArr.push(taskarr[i]);
    }
  });
  console.log(delArr);
  for (var i = 0; i < delArr.length; i++) {
    taskarr.splice(getArrIndex(taskarr,delArr[i]), 1);
  }
  localStorage.setItem("taskArr",JSON.stringify(taskarr));
  fn();
});

$(function() {
  // 加载页面时初始化表格
  updataPage();
  // 点击添加按钮
  $("#add").on("click", function() {
    console.log(JSON.parse(localStorage.getItem("taskArr")));
    $("#confirm").attr("czlx", "0");
    $(".peizhi").css("display", "block");
  });
  // 点击修改按钮
  $("#update").on("click", function() {
    if ($("input[name='checktr']:checked").length != 1) {
      alert("请选择其中一项数据进行修改！");
      return false;
    }
    $(".trcheck").each(function(i, item) {
      if ($(this).prop("checked")) {
        var tds = $(this).parents("td").siblings();
        $("input[name='taskName']").val(tds.eq(1).text());
        $("#ms").val(tds.eq(3).text());
        $("#confirm").attr("dbid", tds.eq(5).text());
        $("#confirm").attr("guid", tds.eq(6).text());
      }
    });
  
    $("#confirm").attr("czlx", "1");
    $(".peizhi").css("display", "block");
  });

  // 点击关闭按钮
  $(".close").on("click", function() {
    $(".peizhi").css("display", "none");
    $(".pz-content input[type='text']").val("");
    $(".pz-textarea").val("");
  });
  // 点击确定按钮
  $("#confirm").on("click", function() {
    var jobname = $("input[name='taskName']").val();
    var servername = $("#serverSelect  option:selected").text();
    var expression = $("#ms").val();
    var state = $("#stateSelect  option:selected").text();
    var dbid =  $("#sqlSelect  option:selected").text();
    var isstate = "";
    console.log(expression);
    if(state=="开启"){
        isstate=true;
    }else{
        isstate=false;
    }
    if ($(this).attr("czlx") == "0") {
      // 如果czlx值为0执行添加操作
      var Guid = getNum();
      var insertele = {
        "JobName": jobname,
        "ServiceName": servername,
        "Expresstion": expression,
        "State": isstate,
        "DbGuid": dbid,
        "Guid": Guid
      };
      var addArr = JSON.parse(localStorage.getItem("taskArr"));
      addArr.push(insertele);
      localStorage.setItem("taskArr", JSON.stringify(addArr));
      fn();
    } else {
      // 如果czlx值为1执行修改操作操作
      var guid = $("#confirm").attr("guid");
      var updateele = {
        "JobName": jobname,
        "ServiceName": servername,
        "Expresstion": expression,
        "State": isstate,
        "DbGuid": dbid,
        "Guid": guid
      };
      var updateArr = JSON.parse(localStorage.getItem("taskArr"));
      for (var i = 0; i < updateArr.length; i++) {
        if (updateArr[i].guid == guid) {
          updateArr[i] = updateele;
        }
      }
      console.log(updateArr);
      localStorage.setItem("taskArr", JSON.stringify(updateArr));
      fn();
    }

    $(".peizhi").css("display", "none");
    // location.reload();
  });
  // 点击取消按钮
  $("#cancel").on("click", function() {
    $(".peizhi").css("display", "none");
    $(".pz-content input[type='text']").val("");
    $(".pz-textarea").val("");
  });

  // 点击查询按钮
  $("#select").on("click", function() {
    var task = $("input[name='task']").val();
    if (!task) {
      window.location.href = "renwu.html";
    } else {
        window.location.href = "renwu.html?jobName=" + task;
    }
  });
// 全面启用
  $("#qmqy").on("click",function(){
    
    var taskarr = JSON.parse(localStorage.getItem("taskArr"));
    for (var i = 0; i < taskarr.length; i++) {
     taskarr[i].state=true;
    }
    localStorage.setItem("taskArr",JSON.stringify(taskarr));
    fn();
  })
  // 全面停用
  $("#qmty").on("click",function(){
    var taskarr = JSON.parse(localStorage.getItem("taskArr"));
    for (var i = 0; i < taskarr.length; i++) {
     taskarr[i].state=false;
    }
    localStorage.setItem("taskArr",JSON.stringify(taskarr));
    fn();
  })
//   启用
  $("#qy").on("click",function(){
    if ($("input[name='checktr']:checked").length == 0) {
      alert("请选择启用项!");
      return false;
    }
    var qylArr = [];
  // 启用选择的列表项
  var taskarr = JSON.parse(localStorage.getItem("taskArr"));
  $(".trcheck").each(function(i, item) {
    if ($(this).prop("checked")) {
      qylArr.push(taskarr[i]);
    }
  });
  for (var i = 0; i < qylArr.length; i++) {
    qylArr[i].state=true;
  }
  for (var i = 0; i < qylArr.length; i++) {
    taskarr.splice(taskarr.indexOf(qylArr[i]), 1,qylArr[i]);
  }
 
  localStorage.setItem("taskArr",JSON.stringify(taskarr));
  fn();
  })

  //  停用
  $("#ty").on("click",function(){
    if ($("input[name='checktr']:checked").length == 0) {
      alert("请选择停用项!");
      return false;
    }
    var tylArr = [];
  var taskarr = JSON.parse(localStorage.getItem("taskArr"));
  $(".trcheck").each(function(i, item) {
    if ($(this).prop("checked")) {
      tylArr.push(taskarr[i]);
    }
  });
  for (var i = 0; i < tylArr.length; i++) {
    tylArr[i].State=false;
  }
  for (var i = 0; i < tylArr.length; i++) {
    taskarr.splice(taskarr.indexOf(tylArr[i]), 1,tylArr[i]);
  }
  console.log(taskarr);
  localStorage.setItem("taskArr",JSON.stringify(taskarr));
  fn();
  })
});

// 生成32位随机数
function getNum() {
  var chars = [
    "0",
    "1",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7",
    "8",
    "9",
    "A",
    "B",
    "C",
    "D",
    "E",
    "F",
    "G",
    "H",
    "I",
    "J",
    "K",
    "L",
    "M",
    "N",
    "O",
    "P",
    "Q",
    "R",
    "S",
    "T",
    "U",
    "V",
    "W",
    "X",
    "Y",
    "Z",
    "a",
    "b",
    "c",
    "d",
    "e",
    "f",
    "g",
    "h",
    "i",
    "j",
    "k",
    "l",
    "m",
    "n",
    "o",
    "p",
    "q",
    "r",
    "s",
    "t",
    "u",
    "v",
    "w",
    "x",
    "y",
    "z"
  ];
  var nums = "";
  for (var i = 0; i < 32; i++) {
    var id = parseInt(Math.random() * 61);
    nums += chars[id];
  }
  return nums;
}
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


function fn(){
    var jsonArr = JSON.parse(localStorage.getItem("taskArr"));
    var obj = {
      QuartzArray:jsonArr
    }
    var str = JSON.stringify(obj);
    $.ajax({
        url:"http://"+params+"/configQuartz/SaveData",
        type:"post",
        data:str,
        contentType:"application/json",
        success:function(res){
            // 如果请求返回数据正常 刷新页面
            if(res){
              if(res=="nologin"){
                  window.location.href = "login.html"
              }else{
                location.reload();
              }
            }
        }
    })
}


// 初始化数据库设置
function selectHtml(arr){
    var html="";
    for(var i=0;i<arr.length;i++){
        html+="<option>"+arr[i].serverIp+"</option>";
    }
    $("#sqlSelect").html(html);
}
// 初始化调度服务名
function selectHtml2(arr){
    var html="";
    for(var i=0;i<arr.length;i++){
        html+="<option>"+arr[i]+"</option>";
    }
    $("#serverSelect").html(html);
}

function getArrIndex(arr, obj) {
  let index = null;
  let key = Object.keys(obj)[0];
  arr.every(function(value, i) {
      if (value[key] === obj[key]) {
          index = i;
          return false;
      }
      return true;
  });
  return index;
};
