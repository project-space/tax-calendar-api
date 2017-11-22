namespace Business

module TaxPeriodCreator =
    open Design.Models

    let public Create (validityPeriods: TaxValidityPeriod list) =
        [
            { Id = uint64(0)
              Tax = TaxType.ENVD
              Type = TaxPeriodType.Quarterly
              Year = uint16(2003)
              Quarter = uint8(1)
              Month = uint8(0) }
        ]

    let private ``Create ENVD periods`` validityPeriod =
        []    