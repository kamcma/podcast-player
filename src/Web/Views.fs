module WebApp.Views

open System
open Giraffe.GiraffeViewEngine
open iTunesClient.Models
open Microsoft.SyndicationFeed

let searchForm =
    form [ _method "GET"; _action "/search" ] [
        input [ _type "text"; _name "query" ]
        input [ _type "submit"]
    ]

let layout content =
    html [] [
        head [] [
            meta [ _charset "utf-8" ]
            title [] [ rawText "Podcast Player" ]
        ]
        body [] ([ searchForm ] @ content)
    ]

let searchResult (result : Result) =
    [
        a [ _href (sprintf "/podcast?iTunesId=%d" result.CollectionId) ] [
            img [ _src result.ArtworkUrl100 ];
        ]
        p [] [ rawText result.CollectionName ]
    ]

let searchResults (results : Result list) =
    let listItems =
        List.map (fun (result : Result) -> li [] (searchResult result)) results
    let list = ol [] listItems
    [ list ]
    |> layout

let episode (item : ISyndicationItem) =
    [
        h4 [] [ rawText item.Title];
        rawText (item.Published.ToString("d"));
        rawText item.Description
    ]

let episodes (items : seq<ISyndicationItem>) =
    let listItems =
        Seq.map (fun (item : ISyndicationItem) -> li [] (episode item)) items
    let list = ol [] (List.ofSeq listItems)
    [ list ]
    |> layout
