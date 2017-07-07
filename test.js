var a = function(){
  var clients = [];

  var user1 = {
    a:"a",
    b:"b"
  };

  var user2 = {
    a:"a",
    b:"b"
  };

  clients.push(user1);
  clients.push(user2);

  var json = JSON.stringify(clients);
  
  console.log(json);
};

a();
