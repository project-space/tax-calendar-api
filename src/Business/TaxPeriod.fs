namespace Business

module TaxPeriod =
    open Design.Models.Tax
    open System

    //-------------------------------------------------------
    // VAT
    let private ``create VAT periods`` (tax: Tax) =
        list<Tax>.Empty

    //-------------------------------------------------------
    // Composition
    //_______________________________________________________
    let public Create (taxes: Tax list) =
        let toTaxPeriods (tax: Tax) =
            match tax.Id with
                | TaxType.VAT -> ``create VAT periods`` tax
                | TaxType.Excises -> failwith "Налог на акцизы не поддерживается на данный момент"
                | TaxType.PersonalIncome -> failwith "Налог на доходы физ. лиц не поддерживается на данный момент"
                | TaxType.CorporateIncome -> failwith "Нало на доходы организации не поддерживается на данный момент"
                | TaxType.Water -> failwith "Водный налог не поддерживается на данный момент"
                | TaxType.GovernmentDuty -> failwith "Госпошлина не поддерживается на данный момент"
                | TaxType.MineralExtraction -> failwith "Налог на добычу полезных ископаемых не поддерживается на данный момент"
                | TaxType.Transport -> failwith "Транспортный налог не поддерживается на данный момент"
                | TaxType.GamblingBusiness -> failwith "Налог на игорный бизнес не поддерживается на данный момент"
                | TaxType.CorporateProperty -> failwith "Налог на имущество организации не поддерживается на данный момент"
                | TaxType.Land -> failwith "Земельны налог не поддерживается на данный момент"
                | TaxType.PersonalProperty -> failwith "Налог на имущество физических лиц не поддерживается на данный момент"
                | TaxType.TradingFee -> failwith "Торговый сбор не поддерживается на данный момент"
                | TaxType.InsurancePremiums -> failwith "Страховые взносы не поддерживаются на данный момент"
                | unsupportedTax -> failwith (sprintf "Неподдерживаемый налог [%s]" (unsupportedTax.ToString()))

        List.collect toTaxPeriods taxes