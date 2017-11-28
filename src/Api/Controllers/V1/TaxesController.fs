namespace Api.Controllers

module Taxes =
    open Giraffe.Tasks
    open Microsoft.AspNetCore.Http
    open Design.Models.Tax
    open Giraffe.HttpHandlers

    let GetTaxes =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! json [
                { Id               = TaxType.VAT
                  Name             = "НДС"
                  IntroductionYear = 2003s
                  CancellationYear = 2019s
                  Fines            = "Штрафов нет" }
            ] next context
        }

    let GetTax (taxId: int) = 
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! json
                { Id               = TaxType.VAT
                  Name             = "НДС"
                  IntroductionYear = 2003s
                  CancellationYear = 2019s
                  Fines            = "Штрафов нет" } next context
        }

    let PostTax  =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! json
                { Id               = TaxType.VAT
                  Name             = "НДС"
                  IntroductionYear = 2003s
                  CancellationYear = 2019s
                  Fines            = "Штрафов нет" } next context
        }