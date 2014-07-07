namespace DungeonCrawler.Core
open System

[<Flags>]
type Exit = 
    | North = 1
    | South = 2
    | East = 4
    | West = 8

type Room(description : string, exits : Exit) = 
    member x.Description = description
    member x.Exits = exits

    member x.GetDescription() =
        x.Description + Environment.NewLine + 
        "Exits are to the " + x.Exits.ToString()