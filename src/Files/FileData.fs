module Structs

open System

[<CustomEquality;NoComparison>]
type FileData = 
    {
        Filename : string;
        Path : string;
        Size : int64;
        MD5 : string;
    }
    
    override x.Equals(o) = 
        match o with 
        | :? FileData as f -> 
            f.Filename = x.Filename && 
            f.Path = x.Path &&
            f.Size = x.Size
        | _ -> false

    override x.GetHashCode() =
        x.MD5.GetHashCode()

    override x.ToString() =
        String.Format(
            "Filename: {0} - Path: {1} - Size: {2} - MD5: {3}",
            x.Filename,
            x.Path,
            x.Size,
            x.MD5)