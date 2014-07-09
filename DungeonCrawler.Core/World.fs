namespace DungeonCrawler.Core

type RoomTransition = {
    Source : Room
    Exit : Exit
    Target : Room
}


type World(player : Player, current_room : Room, rooms : Room list, roomTransitions : RoomTransition list) =
    member x.Player = player
    member x.CurrentRoom = current_room
    member x.Rooms = rooms
    member x.RoomTransitions = roomTransitions

    member x.Move(exit) =
        if(x.CurrentRoom.Exits.HasFlag(exit)) then
            let transition = List.tryFind (fun (transition : RoomTransition) -> transition.Source = x.CurrentRoom && transition.Exit = exit) roomTransitions
            match transition with
            | Some(t) -> Some(World(player, t.Target, rooms, roomTransitions))
            | _ -> None

        else None
