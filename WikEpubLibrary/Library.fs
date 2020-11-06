namespace WikEpubLibrary

open FSharp.Data

module ScrapeWiki =
    let html = HtmlDocument.Load("https://en.wikipedia.org/wiki/Donald_Trump")

    let getTagAttributes tag attribute (doc:HtmlDocument) = 
        doc.Descendants [tag]
        |> Seq.choose (fun x -> 
            x.TryGetAttribute(attribute)) |> Seq.map(fun x -> x.Value())
   
