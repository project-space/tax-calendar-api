namespace Business

open Design.Models
open Calendar.Event

module Mappers =
    module Tax =
        module Period =

            let ``to event`` 
                (firmId : int64) 
                (period : Tax.Period) =

                { Id         = 0L
                  FirmId     = firmId
                  State      = State.Default
                  Start      = period.Start
                  End        = period.End
                  EntityId   = period.Id
                  EntityType = EntityType.TaxPeriod }