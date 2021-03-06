namespace Design.Models

open System

module Tax =

    (* Уникальные идентификаторы налогов, используются в базе данных - значения не менять *)
    [<FlagsAttribute>]
    type Id =
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

    type T =
        { Id               : Id
          Name             : string
          Fines            : string
          IntroductionYear : int
          CancellationYear : int }

    (* Возможные типы налогового периода *)
    [<FlagsAttribute>]
    type PeriodType =
        | Annual    = 0
        | Quarterly = 1
        | Monthly   = 2

    (* Период, за который оплачивается налог *)
    [<CLIMutableAttribute>]
    type Period =
        { Id      : int
          TaxId   : Id
          Type    : PeriodType
          Year    : int
          Quarter : int
          Month   : int
          Start   : DateTime
          End     : DateTime }
        with
            static member Default =
                { Id = 0
                  TaxId = Id.VAT
                  Type = PeriodType.Monthly
                  Year = 0
                  Quarter = 0
                  Month = 0
                  Start = DateTime.Now
                  End = DateTime.Now }              
