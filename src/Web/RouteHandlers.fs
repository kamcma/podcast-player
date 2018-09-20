module WebApp.RouteHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2.ContextInsensitive
open iTunesClient
open iTunesClient.Models
open PodcastClient
open WebApp.Views

let indexHandler : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        htmlView (layout []) next ctx

[<CLIMutable>]
type PodcastSearch = {
    query : string
    offset : int option
}

let podcastSearchHandler : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        let podcastSearch = ctx.BindQueryString<PodcastSearch>()
        let offset =
            match podcastSearch.offset with
            | Some i -> i
            | None -> 0
        let iTunesClient = ctx.GetService<iTunesClient>()
        task {
            let! iTunesRes = iTunesClient.Search(podcastSearch.query, Media.Podcast, offset + 10)
            let results =
                Array.toList iTunesRes.Results
                |> List.skip offset
            return! htmlView (searchResults results) next ctx
        }

[<CLIMutable>]
type Podcast = {
    iTunesId : int64
    offset : int option
}



let podcastHandler : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        let podcast = ctx.BindQueryString<Podcast>()
        let offset =
            match podcast.offset with
            | Some i -> i
            | None -> 0
        let iTunesClient = ctx.GetService<iTunesClient>()
        let podcastClient = ctx.GetService<PodcastClient>()
        task {
            let! iTunesRes = iTunesClient.Lookup podcast.iTunesId
            return! iTunesRes.Results
            |> Array.tryHead
            |> Option.map (fun result -> result.FeedUrl)
            |> function
                | None -> ServerErrors.internalError (text "feed url not found") next ctx
                | Some feedUrl ->
                    task {
                        let! feedItemsSeq = podcastClient.Fetch feedUrl
                        let feedItems =
                            feedItemsSeq
                            |> Seq.skip offset
                            |> Seq.truncate 10
                            |> List.ofSeq
                        return! htmlView (episodes feedItems) next ctx
                    }
        }
