namespace Lib


  module FParsecUtil=
    open FParsec
    //open Lib
    let pushuser s=setUserState s

    let ws=FParsec.CharParsers.spaces
    /// <summary>与えられた述語関数を満たす任意の文字をパースします。</summary>
    let satisfy f =FParsec.CharParsers.satisfy f
    /// <summary>引数のcharシーケンスに含まれる任意の文字をパースします。</summary>
    let anyof =FParsec.CharParsers.anyOf
    /// <summary>2つのパーサをうけとって、それぞれを射影関数に引き渡します</summary>
    let pi=pipe2

    let ps x=pstring x .>> ws
    //let user =pstringCI ""<| userStateSatisfies(fun u->u=0)
    //コメント和訳するのにXmlをなめる
    let testString p s u=FParsec.CharParsers.runParserOnString p u "" s

    let getResult r=
      match r with
      |Success(r,u,p)-> Util.pf (r,p)|Failure(s,e,u)->Util.pf (s,e)