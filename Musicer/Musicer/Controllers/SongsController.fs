namespace Musicer.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Musicer

[<ApiController>]
[<Route("[controller]")>]
type SongsController (logger : ILogger<SongsController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.GetAll() : Song[] =
            Repository.GetSongs |> Seq.toArray

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.Get(songId: int) : Song =
            // todo get song
            new Song()

    [<HttpPost>]
    member __.Post([<FromBody>] song: Song) : Song =
        Repository.InsertSong(song)

    [<HttpDelete>]
    [<Route("{songId}")>]
    member __.Delete(songId: int): bool =
        // todo delete song
        true
