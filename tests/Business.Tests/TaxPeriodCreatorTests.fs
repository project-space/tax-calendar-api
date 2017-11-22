module TaxPeriodCreatorTests

open Expecto
open Business
open Design.Models
open Design.Models

[<Tests>]
let ``tests for Create`` =
    testList "Создание периодов для ЕНВД" [
        let withEnvdValidityPeriod test () =
            let validityPeriod =
                { Tax = TaxType.ENVD
                  IntroductionYear = uint16(2003) 
                  CancellationYear = uint16(2021) }
            
            let templatePeriod = 
                { Id = uint64(0)
                  Tax = TaxType.ENVD
                  Type = TaxPeriodType.Quarterly
                  Year = uint16(2003)
                  Quarter = uint8(0)
                  Month = uint8(0) }

            test (validityPeriod, templatePeriod)

        yield! testFixture withEnvdValidityPeriod [
            "Должны быть созданы квартальные периоды за года введения налога",
            fun (validityPeriod, templatePeriod) ->
                let expected = [
                    { templatePeriod with Quarter = uint8(1); Year = validityPeriod.IntroductionYear }
                    { templatePeriod with Quarter = uint8(2); Year = validityPeriod.IntroductionYear }
                    { templatePeriod with Quarter = uint8(3); Year = validityPeriod.IntroductionYear }
                    { templatePeriod with Quarter = uint8(4); Year = validityPeriod.IntroductionYear }
                ]

                let actual = TaxPeriodCreator.Create [validityPeriod]

                Expect.containsAll actual expected "Нет квартальных периодов за год введения налога"

            "Должны быть созданы квартальные периоды за последний год действительности налога",
            fun (validityPeriod, templatePeriod) ->
                let expected = [
                    { templatePeriod with Quarter = uint8(1); Year = validityPeriod.CancellationYear - uint16(1) }
                    { templatePeriod with Quarter = uint8(2); Year = validityPeriod.CancellationYear - uint16(1) }
                    { templatePeriod with Quarter = uint8(3); Year = validityPeriod.CancellationYear - uint16(1) }
                    { templatePeriod with Quarter = uint8(4); Year = validityPeriod.CancellationYear - uint16(1) }
                ]

                let actual = TaxPeriodCreator.Create [validityPeriod]

                Expect.containsAll actual expected "Нет квартальных периодов за последний год действительности налога"

            "Не должны быть созданы периоды до введения налога",
            fun (validityPeriod, _) ->
                let actual = TaxPeriodCreator.Create [validityPeriod]
                let hasWrongPeriods = List.exists (fun period -> period.Year < validityPeriod.IntroductionYear) actual

                Expect.isFalse hasWrongPeriods "Есть периоды до введения налога"


            "Не должны быть созданы периоды после отмены налога",
            fun (validityPeriod, _) ->
                let actual = TaxPeriodCreator.Create [validityPeriod]
                let hasWrongPeriods = List.exists (fun period -> period.Year >= validityPeriod.CancellationYear) actual

                Expect.isFalse hasWrongPeriods "Есть периоды после отмены налога"


            "Должны быть созданы все 4 квартальных периода в году",
            fun (validityPeriods, templatePeriod) ->
                let expected = [
                    { templatePeriod with Quarter = uint8(1) }
                    { templatePeriod with Quarter = uint8(2) }
                    { templatePeriod with Quarter = uint8(3) }
                    { templatePeriod with Quarter = uint8(4) }
                ]

                let actual = TaxPeriodCreator.Create [validityPeriods]

                Expect.containsAll actual expected "Созданы не все квартальные периоды"
        ]
    ]