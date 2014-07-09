namespace DungeonCrawler.Core
open System.Text.RegularExpressions

type Command = 
    | MoveCommand of Exit
    | QuitCommand
    | CommandError of string

module Parser =
    let Parse(str : string) =
        let (|Match|_|) pattern input =
            let m = Regex.Match(input, pattern) in
            if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None
        match str.ToLower() with
        | Match "(go|move) (north|south|east|west).*" result ->
            match result.Head.ToLower() with
            | "north" -> MoveCommand(Exit.North)
            | "south" -> MoveCommand(Exit.South)
            | "east" -> MoveCommand(Exit.East)
            | "west" -> MoveCommand(Exit.West)
            | _ -> CommandError(str)
        | "quit" -> QuitCommand
        | _ -> CommandError(str)