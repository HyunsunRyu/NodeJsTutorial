var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io').listen(server);

app.set('port', process.env.PORT || 3000);

var userList = [];

io.on("connection", function(socket){

  var currentUser;

  socket.on("USER_ACCESS", function(data){

    var spawnPoint = [];

    if(userList.length === 0) {
      spawnPoint[0] = 250.0;
      spawnPoint[1] = 0.0;
      spawnPoint[2] = 250.0;
    }
    else {
      userList.forEach(function(value, index, ar){

      });
    }

    currentUser = {
      id:guid(),
      name:"",
      position:spawnPoint
    };

    //userList.push(currentUser);

    socket.emit("SUCCESS_ACCESS", currentUser);
    //socket.emit("SUCCESS_ACCESS", {"users":userList});
  });

  socket.on("USER_CONNECT", function(data){

    currentUser = {
      id:data.id,
      name:data.name,
      position:data.position
    };

    userList.push(currentUser);

    socket.emit("SUCCESS_CONNECT", currentUser);
    socket.broadcast.emit("USER_CONNECTED", currentUser);
  });

  socket.on("MOVE", function(data){
    currentUser.position = data.position;
    socket.emit("MOVE", currentUser);
    socket.broadcast.emit("MOVE", currentUser);
  });

  socket.on("disconnect", function(){
    socket.broadcast.emit("USER_DISCONNECTED", currentUser);
    for(var i=0;i<userList.length; i++){
      if(userList[i].name == currentUser.name){
        console.log("User " + userList[i].name + " disconnected");
        userList.splice(i, 1);
      }
    }
  });
});

server.listen(app.get('port'), function(){
  console.log("----- SERVER IS RUNNING -----");
});

//guid 생성하는 코드. //
// https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript#comment29646908_873856 //
/*
function guid() {
    function _p8(s) {
        var p = (Math.random().toString(16)+"000000000").substr(2,8);
        return s ? "-" + p.substr(0,4) + "-" + p.substr(4,4) : p ;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
*/
function guid() {
  function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
  }
  return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
    s4() + '-' + s4() + s4() + s4();
}
