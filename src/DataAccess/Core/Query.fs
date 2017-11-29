namespace DataAccess.Core

open Connection
open Dapper

module Query =
    let inline (=>) key value = key, box value

    let public QueryAsync<'T> (query: string) (param: obj) = async {
        use connection = getConnection()

        return! 
            connection.QueryAsync<'T>(query, param)
            |> Async.AwaitTask<'T seq>
    }

    let public QuerySingleAsync<'T> (query:string) (param: obj) = async {
        use connection = getConnection()

        return! 
            connection.QuerySingleOrDefaultAsync(query, param)
            |> Async.AwaitTask<'T>
    }

    let public ExecuteAsync (query: string) (param: obj) = async {
        use connection = getConnection()

        return! 
            connection.ExecuteAsync(query, param)
            |> Async.AwaitTask
    }