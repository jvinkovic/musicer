namespace Musicer

open System

type SongRating() =
    let mutable _id: int64 = 0L
    let mutable _created: DateTime = DateTime.UtcNow
    let mutable _songId: int64 = 0L
    let mutable _rating: int = 1

    let cleanRating(r) =
        if r > 5 then
            5
        else if r < 1 then
            1
        else
            r

    member this.Id
        with get() = _id
        and set(v) = _id <- v

    member this.Created
        with get() = _created
        and set(v) = _created <- v

    member this.SongId
        with get() = _songId
        and set(v) = _songId <- v

    member this.Rating
        with get() = _rating
        and set(v) = _rating <- cleanRating(v)
