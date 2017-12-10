namespace DataAccess.Core

module TVP =
    open System
    open System.Data
    open System.Reflection
    open System.Data.SqlClient

    open Dapper

    type T = 
        { Name         : string
          CreateScript : string
          DropScript   : string
          Data         : DataTable }

    (* move to another place *)
    let private propIsOption (prop: PropertyInfo) =
        let genericTypeDefinition = 
            match prop.PropertyType.IsGenericType with
            | true -> Some (prop.PropertyType.GetGenericTypeDefinition())
            | false -> None
            
        let isOption = 
            prop.PropertyType.IsGenericType &&
            genericTypeDefinition.Value = typedefof<option<_>>

        let underlyingType = 
            match isOption with 
            | true  -> genericTypeDefinition.Value.GenericTypeArguments |> Array.tryLast
            | false -> None

        underlyingType


    (* function is public only for test coverage, see create *)
    let prepareCreateScript
        (name : string) 
        (rowProps : PropertyInfo array) =
        
        let join (separator: string) (items: string array) = String.Join(separator, items)
        let propToColumnDefinition (prop : PropertyInfo) =
            let propIsOpt = propIsOption prop
            let sqlTypeDefinition = 
                let sqlType = 
                    match propIsOpt with
                    | Some t -> SqlTypeMapper.fromType t
                    | None   -> SqlTypeMapper.fromType prop.PropertyType

                match sqlType with
                | Some (sql) -> sql
                | None       -> failwith (sprintf "Unsupported DotNet type %s" prop.PropertyType.FullName)

            let nullableDefinition = 
                match propIsOpt with
                | Some _  -> "null"
                | None    -> "not null"  

            sprintf "%s %s %s" prop.Name sqlTypeDefinition nullableDefinition

        let columnDefinition = 
            rowProps
            |> Array.map propToColumnDefinition
            |> (join ",")
        
        sprintf "create table #%s ( %s )" name columnDefinition

    let private prepareDropScript (name : string) =
        sprintf "drop table #%s" name

    let private prepareData
        name 
        (items : _ seq)
        (rowProps : PropertyInfo array) =
        
        let table = new DataTable(name)
        let columns =
            Array.map (
                (fun prop -> (prop, propIsOption prop)) >> 
                (fun (prop, isOption) -> 
                    match isOption with
                    | Some underlyingType -> new DataColumn(prop.Name, underlyingType)
                    | None -> new DataColumn(prop.Name, prop.PropertyType))
            ) rowProps

        table.Columns.AddRange (columns)

        let getPropValue item (prop : PropertyInfo) = 
            prop.GetValue(item)

        items |> Seq.iter (fun item ->
            rowProps |> Array.map (getPropValue item) 
                     |> (fun row -> table.Rows.Add(row)) |> ignore)

        table

    let create<'TRow> 
        name
        (items : 'TRow seq) =

        let rowType = typeof<'TRow>
        let rowProps = rowType.GetProperties()

        { Name = name 
          CreateScript = prepareCreateScript name rowProps
          DropScript = prepareDropScript name
          Data = prepareData name items rowProps }

    let apply
        (connection : IDbConnection)
        (tvp : T option) = async {
            if tvp.IsSome then
                connection.Open()
                let! _ = connection.ExecuteAsync (tvp.Value.CreateScript) |> Async.AwaitTask
                use bulkCopy = new SqlBulkCopy((connection :?> SqlConnection))
                bulkCopy.DestinationTableName <- (sprintf "#%s" tvp.Value.Name)
                do! bulkCopy.WriteToServerAsync (tvp.Value.Data) |> Async.AwaitTask      
        }

    let release
        (connection : IDbConnection)
        (tvp : T option) = async {
            if tvp.IsSome then
                let! _ = connection.ExecuteAsync (tvp.Value.DropScript) |> Async.AwaitTask
                return ()
        }