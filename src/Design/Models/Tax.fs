namespace Design.Models

module Tax =
    open System

    (* Уникальные идентификаторы налогов, используются в базе данных - значения не менять *)
    [<FlagsAttribute>]
    type TaxType =
        (* Федеральные налоги *)
        | VAT               = 1 (* Налог на добавленную стоимость *)
        | Excises           = 2 (* Акцизы *)
        | PersonalIncome    = 3 (* Подоходный налог для физ. лиц *)
        | CorporateIncome   = 4 (* Налог на прибыль для организаций *)
        | Water             = 5 (* Водны налог *)
        | GovernmentDuty    = 6 (* Гос. пошлина *)
        | MineralExtraction = 7 (* Налог на добычу полезных ископаемых *)

        (* Региональные налоги и сборы *)
        | Transport         = 8 (* Транспортный налог *)
        | GamblingBusiness  = 9 (* Налог на игорный бизнес *)
        | CorporateProperty = 10 (* Налог на имущество для организаций *)
        
        (* Местные налоги и сборы *)
        | Land              = 11 (* Земельный налог *)
        | PersonalProperty  = 12 (* Налог на имущество для физ. лиц *)
        | TradingFee        = 13 (* Торговый сбор *)

        (* Страховые взносы в Российской Федерации *)
        | InsurancePremiums = 14 (* Страховые взносы *)

    type Tax =
        { Id               : TaxType
          Name             : string
          IntroductionYear : int16
          CancellationYear : int16
          Fines            : string }

    (* Возможные типы налогового периода *)
    [<FlagsAttribute>]
    type TaxPeriodType =
        | Annual    = 0
        | Quarterly = 1
        | Monthly   = 2

    (* Период, за который оплачивается налог *)
    type TaxPeriod =
        { Id      : int64
          TaxId   : TaxType
          Type    : TaxPeriodType
          Year    : int16
          Quarter : byte
          Month   : byte }