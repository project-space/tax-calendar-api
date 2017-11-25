namespace Design.Models

type TaxationSystem =
    | OSNO
    | USN
    | ENVD
    | PSN
    | ESHN

(* Данные о пользователе (за конкретный год) используемые для генерации событий *)
type YearSettings = 
    { FirmId: uint64
      Year: uint16 
      TaxationSystem: TaxationSystem }