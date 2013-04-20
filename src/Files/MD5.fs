module MD5

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