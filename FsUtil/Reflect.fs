namespace Lib
  module MSIL=
    open System.Reflection
    open System.Reflection.Emit
  
  //type generic (x) = member this.funct = 1
  
    let opcodes = 
      typeof<OpCodes>.GetFields() 
      |> Seq.map (fun x -> x.GetValue() :?> OpCode |> fun o -> o.Value, o.Name)
      |> Map.ofSeq
    
    let find_opcode (b : byte array) =
      Seq.map (fun bs -> Map.tryFind (int16 bs) opcodes) b
      
    let getMSIL F =  
      let t1 = F.GetType().GetFields(BindingFlags.NonPublic ||| BindingFlags.Instance) 
               |> Array.map (fun field -> field.Name)
      
      t1.GetType().GetMethods()
      |> Seq.map (fun mb -> try mb.GetMethodBody().GetILAsByteArray() with ex -> [|0uy|])
      |> Seq.map find_opcode
      |> Seq.concat
      |> Seq.filter ((<>) None) //lol
      
    let printMSIL F =
      getMSIL F |> Seq.iter (fun x-> printfn "%s" x.Value) 
      //prints about 1200 loc msil? surely that isnt right for something like let x = 1 printMSIL x, guess its printing whole assembly?
      //ILのコードがずらずらっと列挙される
  
  module Reflec=
    type Type=System.Type
    open System.Reflection.Emit
    open System.Reflection
    //指定されたクラスのpublicフィールドを探す,のにパーサーは使えないなぁ（ほかの言語ならありうるけど）
    
    //全部のメソッドを実行して結果をレポートするとか
    //クラスを渡してほしい名前のメソッド、プロパティを再帰的に探す
    //しかし循環がありうる＞すでにsetにそのクラスがあるなら＞どうやら可視化の実力を
    let getPrivate (typ:Type)=typ.GetFields( BindingFlags.NonPublic ||| BindingFlags.Instance )
    let getfield (a:Assembly) (typename:string) (b:BindingFlags)=
      a.GetType(typename).GetFields(b)
    //モジュールから名前空間だけ探したりしたい
    let findTypes =
      let t=Assembly.GetExecutingAssembly().GetTypes()
      Seq.map (fun (t:Type)->t.Name) t

    //let loadAssem (a:Assembly)=a.GetTypes() |> Seq.
    let tryInvoke (f:MethodInfo) self arg=try Some (f.Invoke(self,Seq.objAr arg)) with |e->None
    //これenumみたいなので管理したほうがいいかも
    //type RefType.Public.Method Public.Field 
    let publicInstance=BindingFlags.Public ||| BindingFlags.Instance
    let publicfields=
         BindingFlags.GetProperty ||| BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy
         ||| BindingFlags.GetField
    let publics (typename:Type) =
      let mem=typename.GetMembers(publicfields) |> Seq.filter(fun m->m.MemberType=MemberTypes.Method |> not)
      mem
    //let getFSharpRapper= =>"let window<'a>=CVUtil.UIUtil.Window"

    open System.Reflection
    //refあげればいいんだっけ
    //let invokeAllWithClone ins=ins.GetType().GetMethods() |> Seq.map(fun minfo->minfo.ToString(),tryInvoke
    let invokeAll (t:'a) arg=t.GetType().GetMethods() |> Seq.map(fun minfo->minfo.ToString(),tryInvoke minfo t arg ) 
                          |> Seq.filter(fun t-> snd t |> Util.isSome ) |> Seq.map (fun x->fst x,(snd x).Value)
    //副作用持つコードだと影響が出る、勝手にDispose呼んで後に影響出たり
    //invokeAll "a" "a"(*これだとcharになっちゃって引数で落ちる*)
    //invokeAll "a" seq{for i in }
    //invokeAll "a" ["a";]
    //ide上で表示される仮引数をそのまま入力するとか
    
    let getPublicField (typename:string)=
      Assembly.GetExecutingAssembly().GetType(typename,false,true).GetFields(BindingFlags.Public)
    //replでは動かない

    let createFsfun (m:MemberInfo)=
      "let " + m.Name + " x =x." + m.Name

    ///プライベートメンバ含むすべてのメンバが取れる
    let gpf t=
      t.GetType().GetMembers() |>Seq.map(fun m->"let " + m.Name + " (x:" + t.GetType().ToString() + ") =x." + m.Name)
    ///パブリックプロパティすべて取れる
    let gpp t=
      t.GetType().GetProperties() 

    let createCsProp t=
      gpp t |> Seq.map(fun x->(string x.PropertyType),x.Name)
        |> Seq.map(fun x->"public " + (fst x) + " " + (snd x) + ";")
    let gpname t=
      gpp t |>Seq.map(fun p->p.Name)

    type MethodI=
      {name:string;retType:Type;recType:Type list}
    type PropI=
      {name:string;keepType:Type;}
    type ClassId=
      {name:string;methodList:MethodI list;propList:PropI list}
    type ClassTree=
      |Class of ClassId list
      |Method of MethodI list
      |Prop of PropI list * ClassTree
    let typeAttribute t at=
      t.GetType() //System.Attribute.GetCustomAttribute(
  open System.Linq
  open System.Reflection
  module test=
    open Reflec
    let makeClass cn fn pn=
      {name=cn;methodList=fn;propList=pn}
      
    let gameobj=
      0//Reflec.ClassTree(Prop({name="gameobject";keepType=typeof<Console>;}))
    
    
    
    let assem=Assembly.LoadFrom("TestInvoker.exe");
    let types (assem:Assembly) = assem.GetTypes() |> Seq.find(fun t->t.Name.Contains("Con"))
    
    //let typeAndAttribute=types |> Seq.map(fun t -> t, t.GetCustomAttributesData()) |> Seq.map snd;
    
