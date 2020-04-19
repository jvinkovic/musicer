namespace Musicer.Controllers

open Musicer
open Microsoft.AspNetCore.Mvc
open Musicer.Repositories

[<ApiController>]
[<Route("[controller]")>]
type RatingsController (ratingsRepository: RatingsRepository) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.GetAll(songId: int64) : SongRating[] =
        ratingsRepository.GetRatings(songId) |> Seq.toArray

    [<HttpPost>]
    member __.Post([<FromBody>] rating: SongRating) =
        ratingsRepository.InsertRating(rating)
