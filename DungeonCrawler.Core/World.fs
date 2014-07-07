namespace DungeonCrawler.Core

type World(player : Player, current_room : Room) =
    member x.Player = player
    member x.CurrentRoom = current_room

    member x.Move(exit) =
        if(x.CurrentRoom.Exits.HasFlag(exit)) 
            then Some(World(player, new Room("Generic room", Exit.East ||| Exit.South)))
            else None
