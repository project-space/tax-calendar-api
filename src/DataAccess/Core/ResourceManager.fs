namespace DataAccess.Core

module ResourceManager =
    open System.IO

    type private IMarker = interface end
    let private assembly = (typeof<IMarker>.DeclaringType).Assembly

    let public Get (resourceName: string) =
        let stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)

        reader.ReadToEnd ()