namespace Business

open Design.Models
open Calendar.Event

module Mappers =

    module Tax =

        module Period =

            let toEvent 
                (firmId : int) 
                (period : Tax.Period) =

                { Id         = 0
                  FirmId     = firmId
                  State      = State.Default
                  Start      = period.Start
                  End        = period.End
                  EntityId   = period.Id
                  EntityType = EntityType.TaxPeriod }