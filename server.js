var guid = require('./guid');

var io = require('socket.io')();
var date = require('node.date-time');

io.listen(4567);

io.on('connection', function(socket){
	console.log("connected");

	socket.on('INPUTTIME', function(data){
		var inputTime = new Date().format('Y-M-dd H:m:S');
		socket.emit("INPUTTIME", {"time" : inputTime});
	});

	socket.emit("SUCCESS_CONNECT");

	socket.on('disconnect', function(data){
		console.log("disconnected");
	});
});
