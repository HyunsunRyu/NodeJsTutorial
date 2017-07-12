var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io').listen(server);

app.set('port', process.env.PORT || 3000);

var clients = [];

io.on("connection", function(socket){

  var currentUser;

  socket.on("USER_CONNECT", function(data){

    currentUser = {
      id:guid(),
      name:data.name,
      position:data.position
    };

    console.log(data.position + " : " + currentUser.position);

    clients.push(currentUser);

    console.log("User : " + currentUser.name + " is connected");
    socket.broadcast.emit("USER_CONNECTED", currentUser);
    socket.emit("SUCCESS_CONNECT", {"users":clients});
  });

  socket.on("MOVE", function(data){
    currentUser.position = data.position;
    socket.emit("MOVE", currentUser);
    socket.broadcast.emit("MOVE", currentUser);
  });

  socket.on("disconnect", function(){
    socket.broadcast.emit("USER_DISCONNECTED", currentUser);
    for(var i=0;i<clients.length; i++){
      if(clients[i].name == currentUser.name){
        console.log("User " + clients[i].name + " disconnected");
        clients.splice(i, 1);
      }
    }
  });
});

server.listen(app.get('port'), function(){
  console.log("----- SERVER IS RUNNING -----");
});

//guid 생성하는 코드. //
// https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript#comment29646908_873856 //
function guid() {
    function _p8(s) {
        var p = (Math.random().toString(16)+"000000000").substr(2,8);
        return s ? "-" + p.substr(0,4) + "-" + p.substr(4,4) : p ;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
