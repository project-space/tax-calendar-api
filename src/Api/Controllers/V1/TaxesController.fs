namespace Api.Controllers

module Taxes =
    open Design.Models.Tax
    open Giraffe.HttpHandlers

    let PostValidityPeriod (tax: string) = async {
        return
            { Tax = TaxType.VAT
              IntroductionYear = 2013s
              CancellationYear = 2092s }
    }