module UtilTest

module DocumentComXmlReader=let i=0
  //#I "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client"
  //#r "\System.Xml.Linq.dll"
//  open System.Xml.Linq
//  open Lib.XDoc
//  open Lib
//  let isnot=Lib.Util.isnot
//  type Xml=
//    |Dxml of XDocument
//    |Other of XDocument
//  let isDocMetaData (d:XDocument)=
//       isnot (d.Descendants(xname "member")) Seq.empty
//  let (|Doc|Oth|) (d:XDocument)=
//      if isDocMetaData d then Doc d else Oth d    
//  let Allmem (d:Xml)=
//    match d with
//      |Dxml s->s
//      |Other s->s
//  let ignoreemp (x:XElement)= 
//    x.Attribute(xname "name").Value="" |> not
//  let getMemberName (x:XElement)=x.Attribute(xname "name").Value 
//  let getSummary (x:XContainer)=x |> getValueOrEmpty "summary"
//  let getElements s (x:XDocument)=x.Descendants(xname s)
//  let addSummaryDocument (s:string)= @"///<summary>" + Util.envNewLine + @"///" +
//                                     (Regex.replaceMatch (s.Trim()) "\n|\r" "") + 
//                                     Util.envNewLine + "///</summary>" + Util.envNewLine 
//  
//  //実はguiチェッカーほしくなってきた
//  let str2ele s=XElement.Parse(s)
//open Lib
//open DocumentComXmlReader
//let thisdir= "."
//let ele=Files.enumfile(thisdir) |> Seq.filter (fun (s:string)-> s.Contains("xml")) |> Seq.map XDoc.Document
////アセンブリ銘はいまはいらない,たぶんresultFparsecみたいにファイルを分けることになるだろう
//let mem=ele |> Seq.map (fun x->x.Descendants(XDoc.xname "member"))
//         |> Seq.concat
//         |> Seq.filter (ignoreemp) //<member name="メンバー名"><summary>サマリ、ないものmoある
////mem |> Seq.iter (fun x->Util.pf (getsummary x |> addSummaryDocument);Util.newline;Util.pf (getMemberName x))
//let s=mem |> Seq.map(fun x->(getSummary x |>addSummaryDocument) + (getMemberName x)+Util.envNewLine) |> Seq.reduce(fun x y->x + y)
//Files.wfResult s
//Util.waitkey()

