const sqlite3 = require('sqlite3').verbose();
let db = new sqlite3.Database('./db/sample.db');

exports.InsertTime = function(time)
{
  db.serialize(function(){
    db.run('create table if not exists testDB(data varchar(50))');
    db.run('insert into testDB(data) values(?)', [time], function(err, rows){
      if(err){
        return console.log(err.message);
      }
      console.log("Length : " + rows);
    });
  });
  db.close();
  console.log(time);
}

exports.GetList = function(){
  db.serialize(function(){
    db.run('create table if not exists testDB(id integer primary key not null, data varchar(50))');

    db.all('select rowid as id, data from testDB order by id desc limit 10', function(err, rows){
      if(err){
        console.log("errrrrrrrr!!!");
        return console.log(err.message);
      }

      for(var i=0, len=rows.length; i<len; i++){
        console.log(rows[i].id + " : " + rows[i].data);
      }
    });
  });
  db.close();
}
