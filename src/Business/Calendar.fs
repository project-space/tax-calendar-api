namespace Business

open Design.Models
open Design.Models.Calendar
open DTO.Settings

module Calendar =

    let createEvents setting =
        ()

    let private updateEvents setting change = 
        let taxPeriods = Seq.empty<Tax.Period>
        let existingEvents = Seq.empty<Event.T>

        let events = createEvents setting
        ()

    let public OnSettingsChanged (settings: Setting.T) (change: ChangeRequest) =
        updateEvents settings change