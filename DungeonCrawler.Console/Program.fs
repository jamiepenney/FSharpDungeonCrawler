open DungeonCrawler.Core

type Action =
    | Continue of World
    | Error of World * string
    | Exit

[<EntryPoint>]
let main argv = 
    printfn "Dungeon Crawler 2014\n"

    printfn "What is your name?"
    let name = System.Console.ReadLine()
    let player = new Player(name)
    printfn "Welcome %s\nYou are at the start of a massive cave system" player.Name

    let start_world = new World(player, new Room("A dark cave", Exit.West))

    let getAction (world : World) = 
        printfn "You see:\n%s" (world.CurrentRoom.GetDescription())
        printfn "What do you want to do?"
        let message = System.Console.ReadLine()
        
        match Parser.Parse(message) with
        | MoveCommand(exit) -> 
            match world.Move(exit) with
            | Some(w) -> Continue(w)
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
            printfn "%s" err
            run f w
    run getAction start_world |> ignore
    
    0 // return an integer exit code
