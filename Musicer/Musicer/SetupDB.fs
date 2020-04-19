module SetupDB

open System.Data.SQLite
open System.IO

let initDB =
    // if does not exists, create db file
    if File.Exists(Repository.databaseFilename) = false then SQLiteConnection.CreateFile(Repository.databaseFilename)

    use connection = new SQLiteConnection(Repository.connectionString)

    connection.Open()

    let initSQL =
        "CREATE TABLE IF NOT EXISTS Songs (" +
        "id INTEGER primary key autoincrement, " +
        "uploaded DATETIME, " +
        "title NVARCHAR(150), " +
        "artist NVARCHAR(150), " +
        "genre NVARCHAR(150)); "
        +
        "CREATE TABLE IF NOT EXISTS Comments (" +
        "id INTEGER primary key autoincrement, " +
        "date DATETIME, " +
        "text NVARCHAR(1500), " +
        "songId INTEGER" +
        "); "
        +
        "CREATE TABLE IF NOT EXISTS Ratings (" +
        "id INTEGER primary key autoincrement, " +
        "created DATETIME, " +
        "rating INTEGER, " +
        "songId INTEGER" +
        "); "

    let structureCommand = new SQLiteCommand(initSQL, connection)
    structureCommand.ExecuteNonQuery() |> ignore

    connection.Close()
    connection.Dispose()
