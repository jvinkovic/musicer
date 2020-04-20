namespace Musicer.Controllers

open Microsoft.AspNetCore.Mvc
open Musicer
open Microsoft.AspNetCore.Hosting
open System.IO
open Microsoft.AspNetCore.Http
open System
open Musicer.Repositories
open Microsoft.AspNetCore.Cors

[<ApiController>]
[<Route("[controller]")>]
type SongsController (webHostEnvironment: IWebHostEnvironment,
                        songsRepository: SongsRepository) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.GetAll() : Song[] =
        songsRepository.GetSongs() |> Seq.toArray

    [<HttpGet>]
    [<Route("{songId}")>]
    member __.Get(songId: int64) : Song =
            songsRepository.GetSong(songId)

    [<HttpGet>]
    [<Route("{songId}/listen")>]
    member __.Listen(songId: int64) =
            songsRepository.ListenSong(songId)

    [<HttpPost>]
    member __.Post([<FromBody>] song: Song) : Song =
        songsRepository.InsertSong(song)

    [<HttpPost>]
    [<Route("{songId}/uploadFile")>]
    member __.Upload(songId: int64, file: IFormFile) : bool =
        let contentRootPath = webHostEnvironment.ContentRootPath;

        let path = Path.Combine(contentRootPath, "App_Data")

        if file.Length > 0L then
            let newName = Guid.NewGuid().ToString().Substring(0, 8) + "-" + file.FileName
            let fileName = Path.Combine(path, newName)
            use fileStream = new FileStream(fileName, FileMode.Create)
            let result = file.CopyToAsync(fileStream)
                            |> Async.AwaitIAsyncResult
                            |> Async.RunSynchronously

            let fileSet = songsRepository.SetSongFile(newName, songId)

            result || fileSet
        else
            false

    [<HttpDelete>]
    [<Route("{songId}")>]
    member __.Delete(songId: int64): bool =
        songsRepository.DeleteSong(songId)
