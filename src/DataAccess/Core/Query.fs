namespace DataAccess.Core

module Query =
    open Connection
    open Dapper
    open System.Collections.Generic

    type Options =
        { Script     : string 
          Parameters : IDictionary<string, obj>
          TVP        : TVP.T option }
        with
            static member Default = 
                { Script = string()
                  Parameters = null
                  TVP = None }

    (* -------------------------------------------------------------- *)
    // HELPERS
    (* -------------------------------------------------------------- *)
    let inline (=>) key value = key, box value    
    let inline private await task = task |> Async.AwaitTask
         
    (* -------------------------------------------------------------- *)
    // QUERIES  
    (* -------------------------------------------------------------- *)
    let queryAsync<'T> options = async {
        use connection = getConnection()

        do! TVP.apply connection options.TVP
        let! rows = await (connection.QueryAsync<'T>(options.Script, options.Parameters))
        do! TVP.release connection options.TVP

        return rows        
    }

    let querySingleAsync<'T> options = async {
        use connection = getConnection()

        do! TVP.apply connection options.TVP
        let! rows = await (connection.QuerySingleOrDefaultAsync<'T>(options.Script, options.Parameters))
        do! TVP.release connection options.TVP

        return rows
    }

    let executeAsync options = async {
        use connection = getConnection()

        do! TVP.apply connection options.TVP
        let! affectedRows = await (connection.ExecuteAsync(options.Script, options.Parameters))
        do! TVP.release connection options.TVP

        return affectedRows
    }