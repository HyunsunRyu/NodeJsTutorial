var guid = require('./guid');
var db = require('./db');
var io = require('socket.io')();
var date = require('node.date-time');

io.listen(4567);

io.on('connection', function(socket){
	console.log("connected");

	socket.emit("SUCCESS_CONNECT");

	socket.on('INPUTTIME', function(data){
		var inputTime = new Date().format('Y-M-dd H:m:S');
		var err = db.InsertTime(inputTime);
		if(err){
			console.log(err);
		}
		else {
			socket.emit("INPUTTIME", {"time" : inputTime});
		}
	});

	socket.on('disconnect', function(data){
		console.log("disconnected");
	});
});
