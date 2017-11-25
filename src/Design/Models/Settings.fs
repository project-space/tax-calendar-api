namespace Design.Models

module Settings =
    type TaxationSystem =
        | OSNO
        | USN
        | ENVD
        | PSN
        | ESHN

    (* Данные о пользователе (за конкретный год) используемые для генерации событий *)
    type YearSettingsValues = 
        { TaxationSystem: TaxationSystem }

    type YearSettings = 
        { FirmId: uint64
          Year: uint16 
          Values: YearSettingsValues }