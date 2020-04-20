namespace Musicer.Repositories

open System.Data.SQLite
open Musicer
open System
open Microsoft.AspNetCore.Hosting
open System.IO

type SongsRepository(webHostEnvironment: IWebHostEnvironment) =
    member __.GetSongs() =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select * from songs"

        let result = seq {
            use selectCommand = new SQLiteCommand(selectSql, connection)
            use reader = selectCommand.ExecuteReader()

            while reader.Read() do
                let song = new Song()
                song.Id <- Convert.ToInt64(reader.["Id"].ToString())
                song.Artist <- reader.["Artist"].ToString()
                song.Title <- reader.["Title"].ToString()
                song.Genre <- reader.["Genre"].ToString()
                song.Uploaded <- Convert.ToDateTime(reader.["Uploaded"])
                song.File <- reader.["File"].ToString()

                yield song
            }

        result

    member __.GetSong(songId: int64) =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select * from songs where id=" + songId.ToString() + " LIMIT 1;"
        use selectCommand = new SQLiteCommand(selectSql, connection)
        use reader = selectCommand.ExecuteReader()

        let song = new Song()
        while reader.Read() do
            song.Id <- Convert.ToInt64(reader.["Id"].ToString())
            song.Artist <- reader.["Artist"].ToString()
            song.Title <- reader.["Title"].ToString()
            song.Genre <- reader.["Genre"].ToString()
            song.Uploaded <- Convert.ToDateTime(reader.["Uploaded"])
            song.File <- reader.["File"].ToString()

        connection.Close()
        song

    member __.ListenSong(songId: int64) =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select file from songs where id=" + songId.ToString() + " LIMIT 1;"
        use selectCommand = new SQLiteCommand(selectSql, connection)
        use reader = selectCommand.ExecuteReader()

        let mutable file = ""
        while reader.Read() do
            file <- reader.["File"].ToString()

        let contentRootPath = webHostEnvironment.ContentRootPath;
        if file.Length = 0 then
            null
        else
            let path = Path.Combine(contentRootPath, "App_Data", file)
            File.OpenRead(path)

    member __.InsertSong(song: Song): Song =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        song.Uploaded <- DateTime.UtcNow
        let insertSql =
            "insert into songs (uploaded,title,artist,genre) VALUES('" + song.Uploaded.ToString(Common.dateTimeFormat) + "','" + song.Title + "','" + song.Artist + "','" + song.Genre + "')"
        use insertCommand = new SQLiteCommand(insertSql, connection)
        let count = insertCommand.ExecuteNonQueryAsync()
                        |> Async.AwaitTask
                        |> Async.RunSynchronously
        let id = connection.LastInsertRowId
        connection.Close()

        song.Id <- id
        song

    member __.SetSongFile(filename: string, id: int64) =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()
        let updateSql = "UPDATE songs SET file='" + filename + "' WHERE id=" + id.ToString()
        use updateCommand = new SQLiteCommand(updateSql, connection)
        let count = updateCommand.ExecuteNonQueryAsync()
                        |> Async.AwaitTask
                        |> Async.RunSynchronously
        connection.Close()

        count > 0

    member __.DeleteSong(songId: int64) =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()
        let contentRootPath = webHostEnvironment.ContentRootPath;

        let song = __.GetSong(songId)

        let path = Path.Combine(contentRootPath, "App_Data", song.File)
        let  result =
            try
                File.Delete(path)

                let deleteSql = "delete from songs WHERE id=" + songId.ToString()
                use deleteCommend = new SQLiteCommand(deleteSql, connection)
                let count = deleteCommend.ExecuteNonQueryAsync()
                                |> Async.AwaitTask
                                |> Async.RunSynchronously
                count > 0
            with
                | _ -> false

        result
