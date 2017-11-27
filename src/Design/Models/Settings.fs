namespace Design.Models

module Settings =
    open System

    (* Системы налогообложения, используются в базе данных, значения не изменять *)
    [<FlagsAttribute>]
    type TaxationSystem =
        | OSNO = 1
        | USN  = 2
        | ENVD = 3
        | PSN  = 4
        | ESHN = 5

    (* Данные используемые для генерации событий *)
    type YearSettingsValues = 
        { TaxationSystem: TaxationSystem }

    (* Данные по фирме за конкретный год*)
    type YearSettings = 
        { FirmId : int64
          Year   : int16 
          Values : YearSettingsValues }