namespace Musicer.Controllers

open Musicer
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("[controller]")>]
type RatingsController (logger : ILogger<RatingsController>) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.Get(songId: int) : SongRating[] =
        [|
            // todo get rtatings for a song

        |]
