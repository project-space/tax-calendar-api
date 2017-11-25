namespace DataAccess.Core

open Dapper
open Connection

module Query =
    let inline (=>) key value = key, box value

    let public QueryAsync<'T> (query: string) (param: obj) = async {
        use connection = getConnection()

        return! 
            connection.QueryAsync(query, param)
            |> Async.AwaitTask
    }

    let public QuerySingleAsync<'T> (query:string) (param: obj) = async {
        use connection = getConnection()

        return! 
            connection.QuerySingleOrDefaultAsync(query, param)
            |> Async.AwaitTask
    }

    let public ExecuteAsync (query: string) (param: obj) = async {
        use connection = getConnection()

        return! 
            connection.ExecuteAsync(query, param)
            |> Async.AwaitTask
    }