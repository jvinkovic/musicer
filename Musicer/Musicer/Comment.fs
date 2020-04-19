namespace Musicer

open System

type Comment(date:DateTime, text: string, songId: int) =
    let mutable _id: int64 = 0L

    member this.Id
        with get() = _id
        and set(v) = _id <- v

    member this.Date = date
    member this.Text = text
    member this.SongId = songId
