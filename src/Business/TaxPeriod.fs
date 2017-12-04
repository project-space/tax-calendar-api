namespace Business

module TaxPeriod =
    open Design.Models
    open System

    //-------------------------------------------------------
    // VAT
    let private ``create VAT periods`` (tax: Tax.T) =
        list<Tax.T>.Empty

    //-------------------------------------------------------
    // Composition
    //_______________________________________________________
    let public Create (taxes: Tax.T list) =
        let toTaxPeriods (tax: Tax.T) =
            match tax.Id with
                | Tax.Id.VAT -> ``create VAT periods`` tax
                | Tax.Id.Excises -> failwith "Налог на акцизы не поддерживается на данный момент"
                | Tax.Id.PersonalIncome -> failwith "Налог на доходы физ. лиц не поддерживается на данный момент"
                | Tax.Id.CorporateIncome -> failwith "Нало на доходы организации не поддерживается на данный момент"
                | Tax.Id.Water -> failwith "Водный налог не поддерживается на данный момент"
                | Tax.Id.GovernmentDuty -> failwith "Госпошлина не поддерживается на данный момент"
                | Tax.Id.MineralExtraction -> failwith "Налог на добычу полезных ископаемых не поддерживается на данный момент"
                | Tax.Id.Transport -> failwith "Транспортный налог не поддерживается на данный момент"
                | Tax.Id.GamblingBusiness -> failwith "Налог на игорный бизнес не поддерживается на данный момент"
                | Tax.Id.CorporateProperty -> failwith "Налог на имущество организации не поддерживается на данный момент"
                | Tax.Id.Land -> failwith "Земельны налог не поддерживается на данный момент"
                | Tax.Id.PersonalProperty -> failwith "Налог на имущество физических лиц не поддерживается на данный момент"
                | Tax.Id.TradingFee -> failwith "Торговый сбор не поддерживается на данный момент"
                | Tax.Id.InsurancePremiums -> failwith "Страховые взносы не поддерживаются на данный момент"
                | unsupportedTax -> failwith (sprintf "Неподдерживаемый налог [%s]" (unsupportedTax.ToString()))

        List.collect toTaxPeriods taxes