open System
open System.IO
open System.Text
open System.Security.Cryptography

let MD5 = new MD5CryptoServiceProvider()

let alignMD5 (md : string) = 
    md.Replace("-","").ToLower() 

let md5 (s : string) = 
    Encoding.UTF8.GetBytes s
    |> MD5.ComputeHash
    |> BitConverter.ToString
    |> alignMD5

let md5File (s : string) = 
    use fs = File.OpenRead s
    use stream = new BufferedStream(fs, 1024 * 10)
    stream
    |> MD5.ComputeHash
    |> BitConverter.ToString
    |> alignMD5

type FileData = {
    Filename : string;
    Path : string;
    Size : int64;
    MD5 : string
}

let rec getAllFileNames path =
    seq {   yield! Directory.GetFiles(path)
            for dir in Directory.GetDirectories(path) do 
                yield! getAllFileNames dir }

let loadFile path =
    let f = new FileInfo(path)
    { Filename = f.Name; Path = f.Directory.FullName; Size = f.Length; MD5 = path |> md5File }

let loadAllFileData files =
    Seq.map loadFile files 

[<EntryPoint>]
let main argv =     
    let path = @"C:\temp\fotos\"

    let allFiles = 
        getAllFileNames path 
        |> loadAllFileData

    Seq.iter (fun d -> Console.WriteLine d.MD5) allFiles 

    Console.ReadLine() |> ignore

    0 // return an integer exit code

