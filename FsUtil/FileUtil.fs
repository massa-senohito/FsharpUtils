namespace Lib
  module Files=
    open System.IO
    ///<summary>
    ///文字をpathに示されたパスのファイルに書き込みます
    ///</summary>
    let wf (s:string) (path)=
      use file=System.IO.File.CreateText(path);
      file.WriteLine(s)
    let pathSep=System.IO.Path.DirectorySeparatorChar
    let wfResult s=
      wf s "./result"
    let mkdir dirname path=System.IO.Directory.CreateDirectory(path+string pathSep+dirname)
    //もしかしてエラーリポートしないほうが早い？
    //stringで返さないで型に乗せたい＞レコードが適任？
    let enumfile path=
      try
        let files=System.IO.Directory.EnumerateFiles(path) 
        files |> Seq.toList
      with f->Util.err f; List.Empty

    let enumdir path=
      try
        System.IO.Directory.EnumerateDirectories(path) |> Seq.toList
      with f->Util.err f; List.empty
///    同じ名前のファイルをdiff
//    let diffSameName dir1 dir2=
//      let System.Diagnostics.Process 
    //指定されたディレクトリの同じ名前のファイルのdiffなど

    let enumFilterdfile filter path=
      enumfile path |> List.filter filter
    let parentDir fullname=
      Directory.GetParent(fullname)
	
    ///<summary>
    ///フォルダ内で2つのファイルを用いた射影変換を行う
    ///<summary>
    let compareInSameFolder fullname f=
      let par=
        let p=parentDir fullname
        p.FullName |>enumfile
      Seq.fromTop f par

    let openFile path=
      File.Open(path,FileMode.Open)
    ///<summary>
    ///pathに示されたファイルのサイズを取得します
    ///</summary>
    let getSize path=
      let finfo=new FileInfo(path)
      finfo.Length

    let mutable sets=List<string>.Empty
    ///<summary>
    ///pathに示されたファイルの拡張子
    ///</summary>
    let getType path=
      let finfo=new FileInfo(path)
      
      let ext=finfo.Extension.Remove(0,1)
      //とりこぼしある可能性--ひとつでも対象のファイル見つけるとやめちゃうし
      sets<-ext::sets
      ext

    let fileName path=
      let finfo=new FileInfo(path)
      finfo.Name
    //あとで同じファイルが検出されるパターンをalloyで検出してみようか
    let rec private findRonece path acc f=
      let find func list=
        try List.find func list with|f-> null 
      //let (+?) (a:'a option) b=if a.IsSome then a::b else b
      let (+?) a b=if a=null then b else a::b
      //let enumf f ign=enumfile f
      let accumlatefile fi=findRonece fi ((find f <| enumfile fi) +? acc)  f
      match enumdir path with
      |[]->acc
      |fil-> fil |> List.map accumlatefile |> List.concat
    ///条件を満たすファイルをディレクトリのなかから一つだけ抽出します
    let findonece path f=findRonece path [] f
    
    let rec private findRmain path acc=
      let rr i= findRmain i (List.append acc (enumfile i))
      match enumdir path with
        |[] ->acc
        |f->f |> List.map rr  |> List.concat

    let withUpper s=
      s |>List.append(s |> List.map(fun (u:string)->u.ToUpper()))
    let ImageTypes=
      let ty=["jpg";"png";"gif";"jpeg";"JPG";"PNG";"GIF";"JPEG";]
      ty
    let MovieTypes=
      ["avi";"mpg";"mp4";"mkv";]|>withUpper
    let DocumentTypes=
      ["doc";"odg";]|>withUpper

    let findRec path=
      findRmain path []
    let isImageFile path=
      ImageTypes  |> List.exists (fun x->getType path=x)
    //enumfile > process "clain"
    let isTargetType t path=
      t |> List.exists (fun x->getType path=x)
    let getset=sets |>Set.ofList
