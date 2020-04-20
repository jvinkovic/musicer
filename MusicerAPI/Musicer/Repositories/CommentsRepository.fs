namespace Musicer.Repositories

open System.Data.SQLite
open Musicer
open System

type CommentsRepository() =
    member __.GetComments(songId: int64) = seq {
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select * from comments where songid=" + songId.ToString()
        use selectCommand = new SQLiteCommand(selectSql, connection)
        use reader = selectCommand.ExecuteReader()

        while reader.Read() do
                let comment =  new Comment()
                comment.Text <- reader.["Text"].ToString()
                comment.SongId <- System.Convert.ToInt64(reader.["SongId"])
                comment.Date <- System.Convert.ToDateTime(reader.["Date"])

                yield comment
        }

    member __.InsertComment(comment: Comment) =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        comment.Date <- DateTime.UtcNow;
        let insertSql =
            "insert into comments (date,Text,SongId) VALUES('" + comment.Date.ToString(Common.dateTimeFormat) + "','" + comment.Text + "'," + comment.SongId.ToString() + ")"
        use insertCommand = new SQLiteCommand(insertSql, connection)
        let count = insertCommand.ExecuteNonQuery()
        let id = connection.LastInsertRowId
        connection.Close()

        comment.Id <- id
        comment
