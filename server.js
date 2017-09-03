var io = require('socket.io')();
var date = require('node.date-time');

io.listen(4567);

io.on('connection', function(socket){
	console.log("connected");

	socket.on('disconnect', function(data){
    console.log("disconnected");
  });
/*
	socket.on('INPUTDATE', function(data){
		//var strDate = new Date().format('Y-M-dd H:m:S');
		//console.log(strDate);
		//socket.emit("R_INPUTDATE", strDate);
		socket.emit("R_INPUTDATE", "Hi");
		//문제 : emit으로 보내는 데이터는 반드시 Json형태여야 한다. //
	});

	socket.emit("SUCCESS_CONNECT");
*/

		socket.on('test1', function(){
			//socket.emit('test1', {"hello" : 'world'});
			socket.emit('test1', "Hello");
		});

		socket.on('test2', function(){
			socket.emit('test2');
		});

	  socket.on('test3', function(name, fn){
	    fn('woot');
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
