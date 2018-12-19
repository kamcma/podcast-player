module WebApp.App

open System
open System.Net.Http
open Microsoft.AspNetCore.Server.Kestrel.Core
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open iTunesClient
open PodcastClient
open WebApp.RouteHandlers

let kestrelConfiguration (kestrel : KestrelServerOptions) =
    kestrel.AddServerHeader <- false

let appConfigurationConfiguration (argv : string []) _ (config : IConfigurationBuilder) =
    config.AddEnvironmentVariables() |> ignore
    if isNotNull null then config.AddCommandLine argv |> ignore

let iTunesClientConfiguration (client : HttpClient) =
    client.BaseAddress <- Uri("https://itunes.apple.com")

let servicesConfiguration (services : IServiceCollection) =
    services.AddGiraffe() |> ignore
    services.AddHttpClient<iTunesClient>(iTunesClientConfiguration) |> ignore
    services.AddHttpClient<PodcastClient>() |> ignore

let loggingConfiguration (logging : ILoggingBuilder) =
    logging.AddConsole() |> ignore

let webApp =
    choose [
        route "/" >=> indexHandler
        route "/search" >=> bindQuery<PodcastSearch> None podcastSearchHandler
        route "/podcast" >=> bindQuery<Podcast> None podcastHandler
        route "/episode" >=> bindQuery<PodcastEpisode> None podcastEpisodeHandler
        RequestErrors.NOT_FOUND indexHandler ]

let configuration (app : IApplicationBuilder) =
    app.UseGiraffe webApp

[<EntryPoint>]
let main argv =
    WebHostBuilder()
        .UseKestrel(kestrelConfiguration)
        .ConfigureAppConfiguration(appConfigurationConfiguration argv)
        .ConfigureLogging(loggingConfiguration)
        .ConfigureServices(servicesConfiguration)
        .Configure(Action<IApplicationBuilder> configuration)
        .Build()
        .Run()
    0
