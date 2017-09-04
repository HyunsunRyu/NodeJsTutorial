var io = require('socket.io')();
var sqlite3 = require('sqlite3').verbose();
var date = require('node.date-time');
var mainDB = 'data/mydb';

function getStatus(database, callback){
    var db = new sqlite3.Database(database);
    // Get the last record
    db.get("SELECT * FROM status ORDER BY id DESC LIMIT 1", function(err, all) {
        console.log("Current status..."+all.status);
        callback(err, all);
    });
    db.close();
}
function setStatus(database, status){
    console.log("Set Status..."+status);
    var db = new sqlite3.Database(database);
    db.run("INSERT INTO status(status, time) VALUES(?,?)", status, Date.now());
    db.close();
}

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

//guid 생성하는 코드. //
// https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript#comment29646908_873856 //
function guid() {
    function _p8(s) {
        var p = (Math.random().toString(16)+"000000000").substr(2,8);
        return s ? "-" + p.substr(0,4) + "-" + p.substr(4,4) : p ;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
