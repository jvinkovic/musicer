namespace Musicer.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Musicer
open Microsoft.AspNetCore.Hosting
open System.IO
open Microsoft.AspNetCore.Http
open System

[<ApiController>]
[<Route("[controller]")>]
type SongsController (logger: ILogger<SongsController>, webHostEnvironment: IWebHostEnvironment) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.GetAll() : Song[] =
            Repository.GetSongs |> Seq.toArray

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.Get(songId: int64) : Song =
            // todo get song
            new Song()

    [<HttpPost>]
    member __.Post([<FromBody>] song: Song) : Song =
        Repository.InsertSong(song)

    [<HttpPost>]
    [<Route("{songId}/uploadFile")>]
    member __.Upload(songId: int64, file: IFormFile) : bool =
        let webRootPath = webHostEnvironment.WebRootPath;
        let contentRootPath = webHostEnvironment.ContentRootPath;

        let path = Path.Combine(webRootPath , contentRootPath, "App_Data")

        if file.Length > 0L then
            let fileName = Path.Combine(path, Guid.NewGuid().ToString() + "-" + file.FileName)
            use fileStream = new FileStream(fileName, FileMode.Create)
            let result = file.CopyToAsync(fileStream)
                            |> Async.AwaitIAsyncResult
                            |> Async.RunSynchronously

            let fileSet = Repository.SetSongFile(fileName, songId)
            result || fileSet
        else
            false

    [<HttpDelete>]
    [<Route("{songId}")>]
    member __.Delete(songId: int): bool =
        // todo delete song
        true
