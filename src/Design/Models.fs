namespace Design

module Models =
    
    //--------------------------------------------------------
    // Tax model description
    //________________________________________________________

    (* Уникальные идентификаторы налогов, используются в базе данных - значения не менять *)
    type TaxType =
        | USN = 0
        | ENVD = 1

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
          Querter: uint8
          Month: uint8 }

    (* Время действия какого либо налога, начиная с даты принятия и заканчивая датой отмены \ упразднения *)
    type TaxValidityPeriod =
        { Tax: TaxType
          IntroductionYear: uint16
          CancellationYear: uint16 }

    //--------------------------------------------------------
    // Event model description
    //________________________________________________________

    (* Сущности являющиеся причиной события *)
    type EventEntityType =
        | Tax

    (* Возможные состояния события *)
    type EventState =
        | Default   = 0
        | Removed   = 1
        | Completed = 2

    (* Конкретное событие *)
    type Event =
        { Id: uint64
          State: EventState
          EntityId: uint64
          EntityType: EventEntityType }