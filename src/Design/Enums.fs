namespace Design

open System

module Enums =

    [<Flags>]
    type TaxationSystemType =
        | OSNO = 1
        | USN  = 2
        | ENVD = 3
        | PSN  = 4
        | ESHN = 5

    [<Flags>]
    type BusinessFormType = 
        | IP  = 1
        | OOO = 2