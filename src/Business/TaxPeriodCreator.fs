namespace Business

module TaxPeriodCreator =
    open Design.Models

    //-------------------------------------------------------
    // Helpers
    //_______________________________________________________

    let private createQuarterlyPeriods year taxType =
        [uint8(1) .. uint8(4)]
            |> List.map (fun quarter ->
                { Id = uint64(0)
                  Tax = taxType
                  Type = TaxPeriodType.Quarterly
                  Year = year
                  Quarter = quarter
                  Month = uint8(0) })

    //-------------------------------------------------------
    // Creators for every tax type
    //_______________________________________________________

    let private createEnvdPeriods (validityPeriod: TaxValidityPeriod) =
        [validityPeriod.IntroductionYear .. validityPeriod.CancellationYear - uint16(1)]
            |> List.collect (fun year -> createQuarterlyPeriods year TaxType.ENVD)

    //-------------------------------------------------------
    // Composition
    //_______________________________________________________

    let public Create (validityPeriods: TaxValidityPeriod list) =
        let toTaxPeriods validityPeriod =
            match validityPeriod with
                | period when period.Tax = TaxType.ENVD -> createEnvdPeriods period 
                | period ->
                    failwith (sprintf "Неподдерживаемый налог [%s]" (period.Tax.ToString()))

        List.collect toTaxPeriods validityPeriods