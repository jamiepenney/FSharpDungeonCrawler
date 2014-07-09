namespace DungeonCrawler.Core
open System

[<Flags>]
type Exit = 
    | North = 1
    | South = 2
    | East = 4
    | West = 8

type Room(description : string, exits : Exit, id) =
    member x.Description = description
    member x.Exits = exits
    member x.Id = id

    member x.GetDescription() =
        x.Description + Environment.NewLine + 
        "Exits are to the " + x.Exits.ToString()

    override x.ToString() = x.Id.ToString()
