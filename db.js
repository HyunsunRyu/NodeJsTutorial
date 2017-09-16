const sqlite3 = require('sqlite3').verbose();
const dbPath = './db/sample.db';
const tableName = 'testDB';
const maxDataCount = 5;
//var database = new sqlite3.Database('./db/sample.db');

function GetDB(){
  return new sqlite3.Database(dbPath).run('create table if not exists testDB(data varchar(50))');
}

exports.InsertTime = function(data)
{
  //get db.
  var db = GetDB();

  //insert the present time data to db.
  db.serialize(function(){
    InsertTime(db, data, ()=>{
      db.close();
      console.log('db is closed');
    });
  });

  //the function to insert the present time data.
  function InsertTime(database, time, closer){
    database.run('insert into '+ tableName +'(data) values(?)', [time], function(err, rows){
      if(err){
        console.error(err.message);
        closer();
      }
      else{
        console.log('success to input the time : ' + time);

        //if it succeeded, need to check the count of data in db and then
        //delete the oldest data if its full.
        GetDataCount(database, (dataCount)=>{
          //error.
          if(dataCount < 0){
            console.error('error');
          }
          //its full
          if(dataCount > maxDataCount){
            console.log('its over ' + maxDataCount);
            DeleteTopData(database, dataCount - maxDataCount, closer);
          }
          else{
            if(dataCount < 0){
              console.error('error');
            }
            else{
              console.log('its not full. tabla has just ' + dataCount + ' rows');
            }
            closer();
          }
        });
      }
    });
  }

  function GetDataCount(database, callback){
    database.get('select count(rowid) as cnt from testDB', function(err, rows){
      if(err){
        console.error(err.message);;
        callback(-1);
      }
      else{
        callback(rows.cnt);
      }
    });
  }

  function DeleteTopData(database, deleteCount, closer){
    console.log('delete top data ' + deleteCount);
    db.all('select rowid as id, data from testDB order by rowid asc limit ' + deleteCount, function(err, row){
      if(err){
        console.log('Error');
        return console.error(err.message);
      }

      var index = 0;
      Delete(database, index, deleteCount, row, ()=>{
        console.log('success to delete all');
        closer();
      });

      function Delete(database, idx, max, row, closer){
        if(idx >= max){
          closer();
          return;
        }
        var id = row[idx].id;
        var data = row[idx].data;
        console.log("delete (" + id + "/" + data + ")");

        database.run("delete from testDB where rowid=(?)", id, function(err){
          if(err){
            console.error(err.message);
            closer();
            return;
          }
          else{
            Delete(database, idx + 1, max, row, closer);
          }
        });
      }
    });
  }
}

exports.GetList = function(){
  db.serialize(function(){
    db.run('create table if not exists testDB(id integer primary key not null, data varchar(50))');

    db.all('select rowid as id, data from testDB order by id desc limit 10', function(err, rows){
      if(err){
        console.log("errrrrrrrr!!!");
        return console.error(err.message);
      }

      for(var i=0, len=rows.length; i<len; i++){
        console.log(rows[i].id + " : " + rows[i].data);
      }
    });
    db.close();
  });
}

exports.DeleteTop = function(){
  db.serialize(function(){
    var id;
    var data;
    db.get('select rowid as id, data from testDB order by rowid asc limit 1', function(err, row){
      if(err){
        console.log('Error');
        return console.error(err.message);
      }
      id = row.id;
      data = row.data;

      db.run("delete from testDB where rowid=(?)", id, function(err){
        if(err){
          console.log('error');
          return console.error(err.message);
        }
        console.log('complete to delete ' + id + " : " + data);
        exports.GetCount();
        db.close();
      });
    });
  });
}

exports.GetCount = function(){
  db.serialize(function(){
    db.get('select count(*) as cnt from testDB', function(err, rows){
      if(err){
        console.log("errrrrrrrr!!!");
        return console.log(err.message);
      }
      //var row = rows[0].cnt;
      console.log(rows.cnt + "=============");
    });
  });
}
