module TaxPeriodCreatorTests

open Expecto
open Business
open Design.Models

[<Tests>]
let ``tests for Create`` =
    testList "Создание периодов для ЕНВД" [
        let withEnvdValidityPeriod test () =
            let validityPeriod = [
                { Tax = TaxType.ENVD
                  IntroductionYear = uint16(2003) 
                  CancellationYear = uint16(2021) }]
            
            let templatePeriod = 
                { Id = uint64(0)
                  Tax = TaxType.ENVD
                  Type = TaxPeriodType.Quarterly
                  Year = uint16(2003)
                  Quarter = uint8(0)
                  Month = uint8(0) }

            test (validityPeriod, templatePeriod)

        yield! testFixture withEnvdValidityPeriod [
            "Должны быть созданы периоды начиная с года введения налога",
            fun (validityPeriods, templatePeriod) ->
                let expected = [
                    { templatePeriod with Quarter = uint8(1); Year = uint16(2003) }
                    { templatePeriod with Quarter = uint8(2); Year = uint16(2003) }
                    { templatePeriod with Quarter = uint8(3); Year = uint16(2003) }
                    { templatePeriod with Quarter = uint8(4); Year = uint16(2003) }
                ]

                let actual = TaxPeriodCreator.Create validityPeriods

                Expect.containsAll actual expected "Нет квартальных периодов на год введения налога"

            "Должны быть созданы периоды до года отмены налога",
            fun (validityPeriods, templatePeriod) ->
                let expected = [
                    { templatePeriod with Quarter = uint8(1); Year = uint16(2021) }
                    { templatePeriod with Quarter = uint8(2); Year = uint16(2021) }
                    { templatePeriod with Quarter = uint8(3); Year = uint16(2021) }
                    { templatePeriod with Quarter = uint8(4); Year = uint16(2021) }
                ]

                let actual = TaxPeriodCreator.Create validityPeriods

                Expect.containsAll actual expected "Нет квартальных периодов на год введения налога"


            "Должны быть созданы все 4 квартальных периода в году",
            fun (validityPeriods, templatePeriod) ->
                let expected = [
                    { templatePeriod with Quarter = uint8(1) }
                    { templatePeriod with Quarter = uint8(2) }
                    { templatePeriod with Quarter = uint8(3) }
                    { templatePeriod with Quarter = uint8(4) }
                ]

                let actual = TaxPeriodCreator.Create validityPeriods

                Expect.containsAll actual expected "Созданы не все квартальные периоды"
        ]
    ]