namespace Musicer.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Musicer

[<ApiController>]
[<Route("[controller]")>]
type CommentsController (logger : ILogger<CommentsController>) =
    inherit ControllerBase()

    let summaries = [| "Freezing"; "Bracing"; "Chilly"; "Cool"; "Mild"; "Warm"; "Balmy"; "Hot"; "Sweltering"; "Scorching" |]

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.Get() : Comment[] =
        [|
            // get comments from DB
        |]
