namespace Musicer

open System

type SongRating(created: DateTime, songId: int, rating: Rating) =
    let mutable _id: int64 = 0L

    member this.Id
        with get() = _id
        and set(v) = _id <- v

    member this.Created = created
    member this.SongId = songId
    member this.Rating = rating
