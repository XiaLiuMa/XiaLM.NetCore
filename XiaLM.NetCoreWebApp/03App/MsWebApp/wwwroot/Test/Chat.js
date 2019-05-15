//最新的用这个：const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
const connection = new signalR.HubConnection('/chathub');

//绑定接收方法
connection.on('RecieveMsg', (timestamp, user, message) => {
    const encodedUser = user;
    const encodedMsg = message;
    const listItem = document.createElement('li');
    listItem.innerHTML = timestamp + ' <b>' + encodedUser + '</b>: ' + encodedMsg;
    document.getElementById('messages').appendChild(listItem);
});

document.getElementById('send').addEventListener('click', event => {
    const msg = document.getElementById('message').value;
    const usr = document.getElementById('user').value;

    //委托后台Hub类中的SendMsg方法发送消息给前台的接收(RecieveMsg)方法。
    connection.invoke('SendMsg', usr, msg).catch(err => showErr(err));
    event.preventDefault();
});

//异常情况显示的内容。
function showErr(msg) {
    const listItem = document.createElement('li');
    listItem.setAttribute("style", "color: red");
    listItem.innerText = msg.toString();
    document.getElementById('messages').appendChild(listItem);
}

//开启连接
connection.start().catch(err => showErr(err));