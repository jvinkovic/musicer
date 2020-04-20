namespace Musicer

open System

type Comment() =
    let mutable _id: int64 = 0L
    let mutable _date: DateTime = DateTime.UtcNow
    let mutable _text: string = null
    let mutable _songId: int64 = 0L

    member this.Id
        with get() = _id
        and set(v) = _id <- v

    member this.Date
        with get() = _date
        and set(v) = _date <- v

    member this.Text
        with get() = _text
        and set(v) = _text <- v

    member this.SongId
    
        with get() = _songId
        and set(v) = _songId <- v
