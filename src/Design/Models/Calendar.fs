namespace Design.Models

[<AutoOpen>]
module Calendar =
    open System        
    
    (* Сущности являющиеся причиной события *)
    [<FlagsAttribute>]
    type EventEntityType =
        | Tax = 1

    (* Возможные состояния события *)
    [<FlagsAttribute>]
    type EventState =
        | Default   = 1
        | Removed   = 2
        | Completed = 3

    (* Конкретное событие *)
    type Event =
        { Id         : int64
          FirmId     : int64
          State      : EventState
          Start      : DateTime
          End        : DateTime
          EntityId   : int64
          EntityType : EventEntityType }