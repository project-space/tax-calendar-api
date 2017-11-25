namespace Design.Models

[<AutoOpen>]
module Tax =
    
    (* Уникальные идентификаторы налогов, используются в базе данных - значения не менять *)
    type TaxType =
        | USN = 0
        | ENVD = 1

    type TaxDescription =
        { Tax: TaxType
          Name: string
          Fines: string }

    (* Возможные типы налогового периода *)
    type TaxPeriodType =
        | Annual        = 0
        | Quarterly     = 1
        | Monthly       = 2

    (* Период, за который оплачивается налог *)
    type TaxPeriod =
        { Id: uint64
          Tax: TaxType
          Type: TaxPeriodType
          Year: uint16
          Quarter: uint8
          Month: uint8 }

    (* Время действия какого либо налога, начиная с даты принятия и заканчивая датой отмены \ упразднения *)
    type TaxValidityPeriod =
        { Tax: TaxType
          IntroductionYear: uint16
          CancellationYear: uint16 }