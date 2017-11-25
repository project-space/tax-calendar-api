namespace Design.Models

[<AutoOpen>]
module Calendar =
    open System        
    
    (* Сущности являющиеся причиной события *)
    type EventEntityType =
        | Tax

    (* Возможные состояния события *)
    type EventState =
        | Default   = 0
        | Removed   = 1
        | Completed = 2

    (* Конкретное событие *)
    type Event =
        { Id: uint64
          FirmId: uint64
          State: EventState
          Start: DateTime
          End: DateTime
          EntityId: uint64
          EntityType: EventEntityType }