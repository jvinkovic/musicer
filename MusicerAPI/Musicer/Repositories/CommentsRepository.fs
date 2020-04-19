namespace Musicer.Repositories

open System.Data.SQLite
open Musicer

type CommentsRepository() =
    member __.GetComments(songId: int64) = seq {
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select * from comments where songid=" + songId.ToString()
        use selectCommand = new SQLiteCommand(selectSql, connection)
        use reader = selectCommand.ExecuteReader()

        while reader.Read() do
            yield new Comment(text = reader.["Text"].ToString(),
                songId = System.Convert.ToInt32(reader.["SongId"]),
                date = System.Convert.ToDateTime(reader.["Date"])
            )
        }

    member __.InsertComment(comment: Comment) =
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let insertSql =
            "insert into comments (date,Text,SongId) VALUES('" + comment.Date.ToString(Common.dateTimeFormat) + "','" + comment.Text + "'," + comment.SongId.ToString() + ")"
        use insertCommand = new SQLiteCommand(insertSql, connection)
        let count = insertCommand.ExecuteNonQuery()
        let id = connection.LastInsertRowId
        connection.Close()

        comment.Id <- id
        comment
