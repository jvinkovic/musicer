namespace Musicer.Repositories

open System.Data.SQLite
open Musicer

type RatingsRepository() =
    member __.GetRatings(songId: int64) = seq {
        let connection = new SQLiteConnection(Common.connectionString)
        connection.Open()

        let selectSql = "select * from ratings where songid=" + songId.ToString()
        use selectCommand = new SQLiteCommand(selectSql, connection)
        use reader = selectCommand.ExecuteReader()

        while reader.Read() do
            yield new SongRating(rating = (System.Enum.Parse(typedefof<Rating>, reader.["Rating"].ToString()) :?> Rating),
                songId = System.Convert.ToInt32(reader.["SongId"]),
                created = System.Convert.ToDateTime(reader.["Created"])
            )
        }

    member __.InsertRating(rating: SongRating) =
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
