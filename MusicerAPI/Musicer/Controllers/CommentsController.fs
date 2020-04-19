namespace Musicer.Controllers

open Microsoft.AspNetCore.Mvc
open Musicer.Repositories
open Musicer

[<ApiController>]
[<Route("[controller]")>]
type CommentsController (commentsRepository: CommentsRepository) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.GetAll(songId: int64) : Comment[] =
        commentsRepository.GetComments(songId) |> Seq.toArray

    [<HttpPost>]
    member __.Post([<FromBody>] comment: Comment) =
        commentsRepository.InsertComment(comment)
