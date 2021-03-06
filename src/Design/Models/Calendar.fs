﻿namespace Design.Models

open System        

module Calendar =        

    module Event =

        (* Сущности являющиеся причиной события *)
        [<FlagsAttribute>]
        type EntityType =
            | TaxPeriod = 1

        (* Возможные состояния события *)
        [<FlagsAttribute>]
        type State =
            | Default   = 1
            | Removed   = 2
            | Completed = 3

        (* Конкретное событие *)
        [<CLIMutableAttribute>]
        [<StructuralEquality>]
        [<StructuralComparison>]
        type T =
            { Id         : int
              FirmId     : int
              State      : State
              Start      : DateTime
              End        : DateTime
              EntityId   : int
              EntityType : EntityType }