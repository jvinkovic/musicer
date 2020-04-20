namespace Musicer.Repositories

open System.Data.SQLite
open Musicer
open System

type RatingsRepository() =
    member __.GetRatings(songId: int64) = seq {
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select * from ratings where songid=" + songId.ToString()
        use selectCommand = new SQLiteCommand(selectSql, connection)
        use reader = selectCommand.ExecuteReader()

        while reader.Read() do
            let rating = new SongRating()
            rating.Id <- System.Convert.ToInt64(reader.["Id"])
            rating.Created <- System.Convert.ToDateTime(reader.["Created"])
            rating.SongId <- System.Convert.ToInt64(reader.["SongId"])
            rating.Rating <- System.Convert.ToInt32(reader.["Rating"])

            yield rating
        }

    member __.InsertRating(rating: SongRating) =
        rating.Created <- DateTime.UtcNow

        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let insertSql =
            "insert into ratings (Created,Rating,SongId) VALUES('" + rating.Created.ToString(Common.dateTimeFormat) + "','" + rating.Rating.ToString() + "'," + rating.SongId.ToString() + ")"
        use insertCommand = new SQLiteCommand(insertSql, connection)
        let count = insertCommand.ExecuteNonQuery()
        let id = connection.LastInsertRowId
        connection.Close()

        rating.Id <- id
        rating
