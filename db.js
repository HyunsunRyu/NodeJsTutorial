const sqlite3 = require('sqlite3').verbose();
var date = require('node.date-time');
let db = new sqlite3.Database('./db/sample.db');

var insertTime = function(time)
{
  db.serialize(function(){
    db.run('create table if not exists testDB(id integer primary key not null, data varchar(50))');
    db.run('insert into testDB(data) values(?)', [time], function(err){
      if(err){
        return console.log(err.message);
      }
    });
  });
  db.close();
  console.log(time);
}

exports.InsertTime = insertTime;
