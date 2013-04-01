//F# の詳細 (http://fsharp.net)
namespace Lib
  type CsTuple<'t1,'t2> (x:'t1,y:'t2) =
    let f,s=x,y
    member this.first=f
    member this.second=s
    member this.toCs (a,b)=new CsTuple<'a,'b>(a,b)
  type IE= System.Collections.IEnumerable
  module mbox=
    //open Microsoft.FSharp.Reflection.
    open Microsoft.FSharp.Control
    let mb f=
      let m=new MailboxProcessor<string>(f)
      m.Start()
  module Util=
    let ActionToFSharpFunc (f:System.Action<'a>)=FuncConvert.ToFSharpFunc f
    let ToFSharpFunc (f:System.Func<'a,'b>)=f.Invoke //function| a->FuncConvert.ToFSharpFunc(new System.Converter()
    let some (s:option<'a> seq)=s |> Seq.filter(fun m->m.IsSome) |> Seq.map(fun m ->m.Value)
    let isNullOrEmpty (s:string)=s=null ||s.Length=0
    let isnot a b= not (a.Equals(b))
    let isPartof (ori:string) (check:string)=
      ori.Contains(check)
    let isNotPartof (ori:string) (check:string)=
      ori.Contains(check) |> not
    let err a=eprintfn "%A" a
    let isSome (s:option<'a>)=s.IsSome

    let someValue (s:option<'a>)=s.Value
    ///<summary>
    ///環境の改行文字を取得します。
    ///</summary>
    let envNewLine=System.Environment.NewLine
//    let capitalize (s:string)=
//      if isNullOrEmpty s then ""
//        else if s.[0]>'Z' then s
//        else System.Char.ToUpper( s.[0] )

    let capcases c=if c<'a' then c else int c-32|>char
    let dcapcases c=if c<'A' then c else int c+32|>char

    let capitalize (str:string) cases=
      let conca=List.fold (fun (acc:string) (i:char)->acc+(string i)) "" 
      let charlis=str.ToCharArray() |>Array.toList
      match charlis with
      |x::xs->(cases x|>string) + (xs|>conca)
      |[]->""
    let cap a=capitalize a capcases
    let dcap a=capitalize a dcapcases
//downcap
    let makeList()=for i in [65..90] do (char i |>string)  |> printf "%A;"
    let sepStrByNewLine (a:string)=a.Split('\n')
    let Flip f g x= f x g
    let Curry f a b=f(a,b)
    let unCurry f=fun (a,b)->f a b
    let pf s=printfn "%A" s
    //Microsoft.FSharp.Control.
    let newline=printfn "%s" "\n"
    let waitkey() = 
      System.Console.Read() |> ignore
    let question (s:'a)=
      System.Console.WriteLine(s.ToString())
      System.Console.ReadLine()

    //上のを一般化させたら
    //    let is a b f=
    //      f a b  

  ///<summary>
  ///正規表現を扱うクラス
  ///</summary>
  module Regex=
    open System.Text.RegularExpressions
    ///<summary>
    ///パターンにマッチしたマッチオブジェクトをシーケンスで返します
    ///</summary>
    /// <param name="pat">マッチさせるパターン</param>
    /// <param name="input"></param>
    let matches pat input=seq{for i in Regex.Matches(input,pat) -> i}

    let (-) (sourcestr:string) (matchstr:string)= Regex.Replace(sourcestr,matchstr,"")
    //let ms pat input=Seq.cast<Match> (Regex.Matches(pat,input))
    ///<summary>
    ///文字列sのパターンにマッチしたすべての文字を置き換えます
    ///</summary>
    let replaceMatch s (pattern:string) (into:string)=Regex.Replace(s,pattern,into)
    let splitmatch s (pattern:string) = Regex.Split(s,pattern)

    let toProst f s=
      let mats= matches @"\w+" s |> Seq.mapi f
      (mats |>Seq.reduce(fun acc i->acc+i)) - ".$" + ");"

    let toCst s=
      toProst (fun i x->if (i=0) then x.Value + "(" else x.Value + ",") s
    let toPhp s=
      toProst (fun i x->if (i=0) then x.Value + "(" else "$" + x.Value + ",") s

    let regullize s=
      3
      //if d="1" then 3 else 5 ->if\nd="1"\nthen\n
//  module XDoc=
//    open System.Xml.Linq
//    let Document (path:string)=
//      XDocument.Load(path)
//    let xname s=
//      XName.Get(s)  
//    let getValueOrEmpty s (x:XContainer)=
//      let e=x.Element(xname s)
//      if not (e=null) then e.Value else ""

  module Col=
    let iter f (e:IE)=for i in e do f i
    let map f (e:System.Collections.IEnumerable)=seq{for i in e -> f i}

    
  module Seq=
  //CVUtilで何か例外はいてたような
    let objAr (i:'a seq)= i |> Seq.cast<obj> |>Seq.toArray
    //似たようななにかのSetを抽出する（似たようなstring）
    //日本語を抽出＞そのsetを構成＞別の2つの要素を比べる（共通の文字の数）＞それが閾値を越えると似ていると判定
    //よく似ていること自体は学習によりクラスわけできるような
    //OpenCVで
    let toString s= seq{ for i in s ->i.ToString()}
    let Do f e=seq{for i in e -> f i |> ignore;i}
//    let rec agl2 (f:'a->'b->'a) (e:seq<'b>)=
//      match e with
//       |x::xs -> f (agl2 f xs) x //list専用なのでhead,tailに分けられない,Seq.tolist?なんかおいしくないなぁ
//       |_->Unchecked.defaultof<_>
    let some (s:option<'a> seq)=s |> Seq.filter(fun m->m.IsSome) |> Seq.map(fun m ->m.Value)
    let rec len x=
      match x with
        |[]->0
        |y::ys-> 1 + len ys
    //let print s=seq{for i in s -> printfn "%A" i} なぜか結果が異なる場合がある
    let print2 f=Seq.iter (Util.pf) f
    let rec fromTop f s=
        match s with
        |x::xs -> Seq.map (fun i->f i x) xs::fromTop f xs
        |[]->[]
    let rec fromTop2 f ft s=
        match s with
        |x::xs ->f (fun i->ft i x) xs::fromTop2 f ft xs
        |[]->[]      
    let rec filterFromTop f s=
        match s with
        |x::xs -> Seq.filter (fun i->f i x) xs::filterFromTop f xs
        |[]->[]
      
    let agl (f:'a->'b->'a) e=
      let mutable acc=Unchecked.defaultof<'a>
      for i in e do
       acc<-f acc i
      acc
    let contains i s=
      s |> Seq.exists (fun x->x.Equals i)
    let containsAll ar s=
      ar |> Seq.forall (fun x->contains x s)
    //何か定義がおかしい
//    let intersectby f xs ys=
//      match xs,ys with
//        |[],_->[] 
//        |_,[]->[]
//        |xs,ys-> [for i in xs ->
//                    List.exists (f i) ys 
//                 ]
    let rec generate ini cond cont cont2=
      match cond <| cont ini with
        |true -> cont ini::generate (cont2 ini) cond cont cont2
        |false -> []
        
    //generate 0 (fun (i:string)->i.length<2) (fun i->string i) i++
    let intersectby2 f ar s=
      seq{for i in s do for j in ar -> if f i j then Some i else None} |> 
        Seq.filter(fun x->x.IsNone |> not) |> Seq.map(fun x->x.Value)

  module Test=
    open System
    open System.Reflection
    let getEnum en=seq{ for i in Enum.GetValues(en) -> i.ToString()}
    //一部じゃないって言う関数でないと
    let bindin=getEnum typeof<BindingFlags> |> Seq.intersectby2 (Util.isPartof) ["Inv";"Ignore"]
    //BindingFlags.SuppressChangeType BindingFlags.IgnoreCase BindingFlags.InvokeMethod

    let publicfields=
         BindingFlags.GetProperty ||| BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy
         ||| BindingFlags.GetField
    let publics (typename:Type) =
      let mem=typename.GetMembers(publicfields) |> Seq.filter(fun m->m.MemberType=MemberTypes.Method |> not)
      mem
    let tt=publics typeof<Type> |> Seq.map(fun m->m.Name)
    //getfields,getmemberのそれぞれフラグを勝手に試してくれれば
    //let test=publics typeof<Type> |> containsAll ["Assembly","Attributes"]
    
