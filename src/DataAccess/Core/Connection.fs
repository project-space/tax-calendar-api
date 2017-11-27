namespace DataAccess.Core

open System.Data
open System.Data.SqlClient

module Connection =

    let private _connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=TaxCalendar;Integrated Security=SSPI;"

    let getConnection () : IDbConnection =
        new SqlConnection(_connectionString) :> IDbConnection