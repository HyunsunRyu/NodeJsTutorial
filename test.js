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
      //name:guid()
      name:data.name
    };

    clients.push(currentUser);

    console.log("User : " + currentUser.name + " is connected");
    socket.broadcast.emit("USER_CONNECTED", currentUser);
    //socket.emit("SUCCESS_CONNECT", {"users":clients});
    socket.emit("SUCCESS_CONNECT", currentUser);
    socket.emit("List", {"users":clients});
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

function guid() {
    function _p8(s) {
        var p = (Math.random().toString(16)+"000000000").substr(2,8);
        return s ? "-" + p.substr(0,4) + "-" + p.substr(4,4) : p ;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
