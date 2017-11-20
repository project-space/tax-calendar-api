module TaxCalendar.Api.Models

type TaxType =
    | ENVD
    | FixedContributions

type ReportingPeriodType =
    | Quarterly = 1
    | Annual = 2

type CalendarEventMetadata = 
    { Id: uint64
      Tax: TaxType
      YearStarting: uint16
      YearEnding: uint16
      ReportingPeriodType: ReportingPeriodType }