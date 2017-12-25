module RestrictionFilterTest

open Business.Restrictions
open Design.Models
open Design.Enums
open Expecto

[<Tests>]
let tests = 
    testList "Restriction filter tests" [ 
        testList "BUSINESS FORM" [
            test "filter must return true when business forms are equal" {
                let settingValues = { Setting.Values.Default with BusinessFormType = BusinessFormType.OOO }
                let restrictions = [BusinessForm (BusinessFormType.OOO)]
                let period = Tax.Period.Default

                let filterResult = filter settingValues restrictions period

                Expect.isTrue filterResult "Filter return false when business forms are equal"
            }
        ]

        testList "TAXATION SYSTEM" [
            test "filter must return true when taxation systems are equal in period" {
                let settingValues = 
                    { Setting.Values.Default with 
                        TaxationSystemTypes = 
                            [(TaxationSystemType.OSNO, 2010)
                             (TaxationSystemType.ENVD, 2018)] }

                let restrictions = [TaxationSystem (TaxationSystemType.OSNO)]
                let period = { Tax.Period.Default with Year = 2015 }

                let filterResult = filter settingValues restrictions period

                Expect.isTrue filterResult "Filter return false when taxation systems are equal in period"
            }
        ]
    ]