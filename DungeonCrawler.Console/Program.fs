open DungeonCrawler.Core

type Action =
    | Continue of World
    | Error of World * string
    | Exit

[<EntryPoint>]
let main argv = 
    printfn "Welcome to Dungeon Crawler"
    let player = new Player("Jamie")
    let start_world = new World(player, new Room("Start of the cave system", Exit.West))

    let getAction (world : World) = 
        printfn "%s" (world.CurrentRoom.GetDescription())
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
