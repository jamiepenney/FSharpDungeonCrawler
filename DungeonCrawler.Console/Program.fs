open DungeonCrawler.Core
open System

type Action =
    | Continue of World
    | Error of World * string
    | Exit

type Printer() =
    member this.slow(format, [<ParamArray>] args : Object[]) =
        let formatted = String.Format(format, args)
        let printchar c = 
            printf "%c" c
            System.Threading.Thread.Sleep(4);
        String.iter printchar formatted
    member this.slown(format, [<ParamArray>] args : Object[]) =
        this.slow(format + Environment.NewLine, args)

[<EntryPoint>]
let main argv = 
    let printer = new Printer()
    printer.slown "Dungeon Crawler 2014\n"

    printer.slow "What is your name?\n#: "
    let name = System.Console.ReadLine()
    let player = new Player(name)
    printer.slown("Welcome {0}\nYou are at the start of a massive cave system\n", player.Name)

    let start_world = WorldGenerator.Generate player

    let getAction (world : World) = 
        printer.slown ("You see:\n{0}", world.CurrentRoom.GetDescription())
        printer.slow "What do you want to do?\n#: "
        let message = System.Console.ReadLine()
        
        match Parser.Parse(message) with
        | MoveCommand(exit) -> 
            match world.Move(exit) with
            | Some(w) ->
                printer.slown("\nYou move carefully {0}", exit)
                Continue(w)
            | _ -> Action.Error(world, "I can't move that way")
        | CommandError(str) -> 
            Action.Error(world, "I did not understand your command")
        | QuitCommand -> Exit
        
    let rec run f world =
        let state = f(world)
        match state with
        | Exit -> world
        | Continue(w) -> run f w
        | Error(w, err) ->
            printer.slown err
            run f w
    run getAction start_world |> ignore
    
    0
