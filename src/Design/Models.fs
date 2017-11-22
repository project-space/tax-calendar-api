namespace Design

module Models =
    
    //--------------------------------------------------------
    // Tax model description
    //________________________________________________________

    type TaxType =
        | USN 
        | ENVD

    type TaxPeriodType =
        | Annual        = 0
        | FirstQuarter  = 1
        | SecondQuarter = 2
        | ThirdQuarter  = 3
        | FourthQuarter = 4

    type Tax =
        { Id: TaxType
          Name: string }

    type TaxPeriod =
        { Id: TaxType
          Type: TaxPeriodType
          Year: uint16
          Querter: uint8 }
          

    type TaxValidityPeriod =
        { Id: TaxType
          IntroductionYear: uint16
          CancellationYear: uint16 }

    //--------------------------------------------------------
    // Event model description
    //________________________________________________________

    type EventState =
        | Created   = 0
        | Removed   = 1
        | Completed = 2

    type Event =
        { Id: uint64
          TaxId: uint64
          State: EventState }