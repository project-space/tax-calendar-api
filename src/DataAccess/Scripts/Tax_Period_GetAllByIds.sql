select * from TaxPeriod as taxPeriod
    join #Identificators as identificators on
        taxPeriod.Id = identificators.Value