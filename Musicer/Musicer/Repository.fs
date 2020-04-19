module Repository

open System.Data.SQLite
open Musicer
open System

let databaseFilename = "musicerDB.sqlite"
let connectionString = sprintf "Data Source=%s;Version=3;" databaseFilename

let GetSongs = seq {
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    let selectSql = "select * from songs"
    use selectCommand = new SQLiteCommand(selectSql, connection)
    use reader = selectCommand.ExecuteReader()

    while reader.Read() do
        let song = new Song()
        song.Id <- Convert.ToInt64(reader.["Id"].ToString())
        song.Artist <- reader.["Artist"].ToString()
        song.Title <- reader.["Title"].ToString()
        song.Genre <- reader.["Genre"].ToString()
        song.Uploaded <- DateTime.UtcNow

        yield song
    }

let GetComments = seq {
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    let selectSql = "select * from comments"
    use selectCommand = new SQLiteCommand(selectSql, connection)
    use reader = selectCommand.ExecuteReader()

    while reader.Read() do
        yield new Comment(text = reader.["Text"].ToString(),
            songId = System.Convert.ToInt32(reader.["SongId"]),
            date = System.Convert.ToDateTime(reader.["Date"])
        )
    }

let GetRatings = seq {
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    let selectSql = "select * from ratings"
    use selectCommand = new SQLiteCommand(selectSql, connection)
    use reader = selectCommand.ExecuteReader()

    while reader.Read() do
        yield new SongRating(rating = (System.Enum.Parse(typedefof<Rating>, reader.["Rating"].ToString()) :?> Rating),
            songId = System.Convert.ToInt32(reader.["SongId"]),
            created = System.Convert.ToDateTime(reader.["Created"])
        )
    }

let InsertSong(song: Song): Song =
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    let insertSql =
        "insert into songs (uploaded,title,artist,genre) VALUES('" + song.Uploaded.ToString() + "','" + song.Title + "','" + song.Artist + "','" + song.Genre + "')"
    use insertCommand = new SQLiteCommand(insertSql, connection)
    let count = insertCommand.ExecuteNonQueryAsync() |> Async.AwaitTask |> Async.RunSynchronously
    let id = connection.LastInsertRowId
    connection.Close()

    song.Id <- id
    song

let InsertComment(comment: Comment) =
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    let insertSql =
        "insert into comments (date,Text,SongId) VALUES('" + comment.Date.ToString() + "','" + comment.Text + "'," + comment.SongId.ToString() + ")"
    use insertCommand = new SQLiteCommand(insertSql, connection)
    let count = insertCommand.ExecuteNonQuery()
    let id = connection.LastInsertRowId
    connection.Close()

    comment.Id <- id
    comment

let InsertRating(rating: SongRating) =
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    let insertSql =
        "insert into ratings (Created,Rating,SongId) VALUES('" + rating.Created.ToString() + "','" + rating.Rating.ToString() + "'," + rating.SongId.ToString() + ")"
    use insertCommand = new SQLiteCommand(insertSql, connection)
    let count = insertCommand.ExecuteNonQuery()
    let id = connection.LastInsertRowId
    connection.Close()

    rating.Id <- id
    rating
