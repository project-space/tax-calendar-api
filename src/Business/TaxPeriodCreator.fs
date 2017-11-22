namespace Business


module TaxPeriodCreator =
    open Design.Models

    let private ``Create ENVD periods`` (validityPeriod: TaxValidityPeriod) =
        let templatePeriod = 
            { Id = uint64(0)
              Tax = TaxType.ENVD
              Type = TaxPeriodType.Quarterly
              Year = uint16(0)
              Quarter = uint8(0)
              Month = uint8(0) }

        List.collect (fun year -> 
            List.map (fun quarter -> 
                { templatePeriod with Year = year; Quarter = quarter }
            ) [uint8(1) .. uint8(4)]
        ) [validityPeriod.IntroductionYear .. validityPeriod.CancellationYear]

    let public Create (validityPeriods: TaxValidityPeriod list) =
        let toTaxPeriods validityPeriod =
            match validityPeriod with
                | period when period.Tax = TaxType.ENVD -> ``Create ENVD periods`` period 
                | period ->
                    failwith (sprintf "Неподдерживаемый налог [%s]" (period.Tax.ToString()))

        List.collect toTaxPeriods validityPeriods