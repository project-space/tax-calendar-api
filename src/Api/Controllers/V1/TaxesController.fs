namespace Api.Controllers

module Taxes =
    open Giraffe.Tasks
    open Microsoft.AspNetCore.Http
    open Design.Models.Tax
    open Giraffe.HttpHandlers

    let Get =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            let! taxes = DataAccess.Queries.Taxes.Get ()
            return! json taxes next context
        }

    let GetSingle (taxId: int) = 
        fun (next: HttpFunc) (context: HttpContext) -> task {
            let taxType = TaxType.Parse(taxId.ToString())            
            let! tax = DataAccess.Queries.Taxes.GetSingle taxType
            
            return! json tax next context
        }

    let PostSingle  =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! json
                { Id               = TaxType.VAT
                  Name             = "НДС"
                  IntroductionYear = 2003s
                  CancellationYear = 2019s
                  Fines            = "Штрафов нет" } next context
        }