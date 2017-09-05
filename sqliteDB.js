const sqlite3 = require('sqlite3').verbose();

let db = new sqlite3.Database('./db/sample.db');

db.run('INSERT INTO test(name) VALUES(?)', ['C'], function(err){
  if(err){
    return console.log("ERROR INSERT : " + err.message);
  }
  console.log(`A row has been inserted with rowid ${this.lastID}`);
});

db.close();
