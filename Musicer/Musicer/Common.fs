module Common

open System.Data.SQLite

let databaseFilename = "musicerDB.sqlite"
let connectionString = sprintf "Data Source=%s;Version=3;" databaseFilename

// format for datetime in db
let dateTimeFormat = "yyyy-MM-dd HH:mm:ss"
