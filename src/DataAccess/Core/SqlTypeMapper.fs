namespace DataAccess.Core

module SqlTypeMapper =
    open System

    let private mapOfTypes =
        [
            (typeof<bool>, "byte")
            (typeof<byte>, "varbinary(max)")
            (typeof<byte>, "tinyint")
            (typeof<char>, "varchar(max)")
            (typeof<DateTime>, "datetime2")
            (typeof<decimal>, "decimal(20,4)")
            (typeof<double>, "float")
            (typeof<float>, "real")
            (typeof<int>, "int")
            (typeof<int16>, "short")
            (typeof<int>, "bigint")
            (typeof<string>, "varchar(max)")
            (typeof<TimeSpan>, "time")
        ]

    let fromType (propType : Type) =
        mapOfTypes
        |> List.tryFind (fun (key, _) -> key = propType)
        |> Option.map (fun (_, sql) -> sql)