namespace DungeonCrawler.Core
open System

module WorldGenerator =
    let roomDescriptions = ["A dark cave"; "A scary cave"; "A well lived in cave"; "A disused mining cave"]
    let oppositeOf(exit : Exit) =
        match exit with
        | Exit.North -> Exit.South
        | Exit.South -> Exit.North
        | Exit.West -> Exit.East
        | Exit.East -> Exit.West
        | _ -> Exit.North

    let Generate(player) =
        let random = new Random();
        let roomGenerator (sourceDirection : Exit, sourceRoom : Room) =
            let random_exits = enum<Exit> (random.Next(1, 16)) ||| oppositeOf(sourceDirection)
            let random_description = roomDescriptions.[random.Next(0, roomDescriptions.Length)]
            let new_room = new Room(random_description, random_exits, Guid.NewGuid().ToString())
            {Source=sourceRoom; Exit=sourceDirection; Target=new_room}

        let start_room = new Room("A dark cave", Exit.West, "1")

        let rec generateRooms (existingRooms : Room list, existingTransitions : RoomTransition list, rooms : Room list, depth) =
            if(depth = 3) then rooms, existingTransitions
            else
                let mapRoom(room : Room) =
                    let allValues = Enum.GetValues(typeof<Exit>) :?> Exit[]
                    let exits = Array.filter (fun v -> room.Exits.HasFlag(v)) allValues
                    List.ofArray <| Array.map (fun exit -> roomGenerator(exit, room)) exits
                let newTransitions = List.collect mapRoom rooms
                let oppositeTransitions = List.map (fun t -> {Source=t.Target;Exit=oppositeOf(t.Exit);Target=t.Source}) newTransitions
                let newRooms = List.map (fun (transition : RoomTransition) -> transition.Target) newTransitions

                generateRooms(existingRooms @ newRooms, existingTransitions @ newTransitions @ oppositeTransitions, newRooms, depth + 1)

        let rooms, transitions = generateRooms([start_room], [], [start_room], 1)
        new World(player, start_room, rooms, transitions)
