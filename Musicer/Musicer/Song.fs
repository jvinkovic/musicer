namespace Musicer

open System

type Song() =
    let mutable _id: int64 = 0L
    let mutable _uploaded: DateTime = DateTime.UtcNow
    let mutable _title: String = null
    let mutable _artist: String = null
    let mutable _genre: String = null
    let mutable _path: String = null

    member this.Id
        with get() = _id
        and set(v) = _id <- v

    // todo make all members as above...
    member this.Uploaded
        with get() = _uploaded
        and set(v) = _uploaded <- v

    member this.Title
        with get() = _title
        and set(v) = _title <- v

    member this.Artist
        with get() = _artist
        and set(v) = _artist <- v

    member this.Genre
        with get() = _genre
        and set(v) = _genre <- v

    member this.Path
        with get() = _path
        and set(v) = _path <- v
