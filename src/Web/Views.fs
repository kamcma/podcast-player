module WebApp.Views

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

let episode (iTunesId : int64) (indexedItem : int * ISyndicationItem) =
    let idx, item = indexedItem
    [
        a [ _href (sprintf "/episode?iTunesId=%i&offset=%i" iTunesId idx) ] [
            h4 [] [ rawText item.Title];
        ]
        rawText (item.Published.ToString("d"));
    ]

let episodes (iTunesId : int64) (items : seq<ISyndicationItem>) =
    let listItems =
        Seq.indexed items
        |> Seq.map (fun (indexedItem : int * ISyndicationItem) -> li [] (episode iTunesId indexedItem))
    let list = ol [] (List.ofSeq listItems)
    [ list ]
    |> layout

let episodePlayer (item : ISyndicationItem) =
    let link =
        item.Links
        |> Seq.find (fun (link : ISyndicationLink) -> link.RelationshipType = "enclosure")

    [
        audio [ _controls ] [
            source [ _src link.Uri.AbsoluteUri; _type link.MediaType ]
        ]
    ]
    |> layout
