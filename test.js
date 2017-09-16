var db = require('./db');
var date = require('node.date-time');
var inputTime = new Date().format('Y-M-dd H:m:S');

db.InsertTime(inputTime);

//db.GetList();

//db.GetCount();

//db.DeleteTop();
