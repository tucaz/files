open System
open System.IO
open Structs
open MD5

let rec getAllFileNames path =
    seq { yield! Directory.GetFiles(path)
          for dir in Directory.GetDirectories(path) do 
            yield! getAllFileNames dir }

let loadFilesData files =
    Seq.map (fun path -> 
        let f = new FileInfo(path)
        { Filename = f.Name; Path = f.Directory.FullName; Size = f.Length; MD5 = path |> md5File })
        files

let isDuplicate f1 f2 =
    f1.Equals(f2) = false && f1.Filename = f2.Filename && f1.Size = f2.Size

let findDuplicates f files =
    Seq.filter (fun d -> isDuplicate f d) files

[<EntryPoint>]
let main argv =     
    let path = @"C:\temp\fotos\"

    let allFiles = 
        getAllFileNames path 
        |> loadFilesData

    Seq.iter (fun d -> Console.WriteLine (d.ToString())) allFiles 

    Console.WriteLine ""

    let findDuplicatesInAllFiles f = 
        findDuplicates f allFiles

    allFiles
        |> Seq.map (fun f -> (f, findDuplicatesInAllFiles f))
        |> Seq.iter (fun f -> 
            Console.WriteLine Environment.NewLine
            Console.WriteLine (fst(f).ToString())
            snd(f) |> Seq.iter (fun d -> Console.WriteLine ("\t" + d.ToString()))
            )

    Console.ReadLine() |> ignore

    0 // return an integer exit code

