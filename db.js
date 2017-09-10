const sqlite3 = require('sqlite3').verbose();
var date = require('node.date-time');
let db = new sqlite3.Database('./db/sample.db');

exports.InsertTime = function(time)
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

exports.GetList = function(){
  db.serialize(function(){
    db.run('create table if not exists testDB(id integer primary key not null, data varchar(50))');
    let playlistid = 1;
    db.get('select d as data from testDB', [playlistid], function(err, row){
      if(err){
        return console.log(err.message);
      }
      console.log(row.id);
    });
  });
  db.close();
}
