module QuatUtils
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.DerivedPatterns
open Microsoft.FSharp.Quotations.ExprShape

let add x y = x + y
let mul x y = x * y

///object graphを作成する
//let rec getGraph (expr: Expr) =
//  let parse args =
//    List.foldBack (fun acc v -> acc ^ (if acc.Length > 0 then "," else "") ^ getGraph v) "" args
//  let descr s = function
//    | Some v -> "(* instance " ^ s ^ "*) " ^ getGraph v
//    | _ -> "(* static " ^ s ^ "*)"
//  match expr with
//  | Int32 i -> string i
//  | String s -> sprintf "\"%s\"" s
//  | Value (o,t) -> sprintf "%A" o
//  | Call (e, methodInfo, av) ->
//    sprintf "%s.%s(%s)" (descr "method" e) methodInfo.Name (parse av)
//  | PropertyGet(e, methodInfo, av) ->
//    sprintf "%s.%s(%s)" (descr "property" e) methodInfo.Name (parse av)
//  | _ -> failwithf "I'm don't understand such expression's form yet: %A" expr

///文から関数を作る
let rec substituteExpr expression =
    match expression with
    | SpecificCall <@@ add @@> (_, _, exprList) ->
        let lhs = substituteExpr exprList.Head
        let rhs = substituteExpr exprList.Tail.Head
        <@@ mul %%lhs %%rhs @@>
    | ShapeVar var -> Expr.Var var
    | ShapeLambda (var, expr) -> Expr.Lambda (var, substituteExpr expr)
    | ShapeCombination(shapeComboObject, exprList) ->
        RebuildShapeCombination(shapeComboObject, List.map substituteExpr exprList)

let expr1 = <@@ 1 + (add 2 (add 3 4)) @@>
//println expr1
let expr2 = substituteExpr expr1
//println expr2
let createFun expr=
  match expr with
  |Lambda(param,body)->param.Name
  |Call(exprOpt, methodInfo, exprList)->""
  |_->""
let println expr =
    let rec print expr =
        match expr with
        | Application(expr1, expr2) ->
            // Function application.
            print expr1
            printf " "
            print expr2

        | Call(exprOpt, methodInfo, exprList) ->
            // Method or module function call. 
            match exprOpt with
            | Some expr -> print expr
            | None -> printf "%s" methodInfo.DeclaringType.Name
            printf ".%s(" methodInfo.Name
            if (exprList.IsEmpty) then printf ")" else
            print exprList.Head
            for expr in exprList.Tail do
                printf ","
                print expr
            printf ") }"

        | Lambda(param, body) ->
            // Lambda expression.
            printf "fun (%s %s){"  (param.Type.ToString()) param.Name//木の下にLambdaが現れる限り再帰して状態を作るか
            print body
        | Let(var, expr1, expr2) ->
            // Let binding. 
            if (var.IsMutable) then
                printf "let mutable %s = " var.Name
            else
                printf "let %s = " var.Name
            print expr1
            printf " in "
            print expr2
        | PropertyGet(_, propOrValInfo, _) ->
            printf "%s" propOrValInfo.Name

        | Value(value, typ) ->
            printf "%s" (value.ToString())
        | Var(var) ->
            printf "%s" var.Name
        | _ -> printf "%s" (expr.ToString())
    print expr
    printfn ""
//ここから自作
let connma s s2= s+ "," + s2
let toOp (minfo:System.Reflection.MethodInfo)=
  match minfo.Name with
    |"op_Addition"->"+"//というかerlang用の前置関数にできるじゃん
    |x->x
let rec lamc expr=
  match expr with |Lambda(p,b)->p.Name + "," + (lamc b) |_->")"
  
let isLam expr=
  match expr with
  |Lambda(para,body)->true
  |_->false
 
let rec ccfun expr sta= //fun a b->add a b #> add(x,y)
  match expr with
  |Lambda(para,body)-> //let lamn= (para.Name + lamc body)
                      ccfun body (para.Name::sta)
  |Call(exprOpt, methodInfo, exprList) ->
    toOp methodInfo + "(" + 
      (exprList |> Seq.map(fun x-> ccfun x sta) //<@ fun x y z->add x (mul y z) @>
                                                //"add(mul(x,y,z,x,y,z),x,y,z)"でなくてadd(x,mul(y,z))
      |> Seq.reduce(fun x y->y + "," + x)) + ")"
  |_->sta |>Seq.reduce (fun x y->y + "," + x)

//let Bind(x,f)=
//  match x with
//  |Some x->f x
//  |None->None
let bind(x,f)=
  match x with
  |Lambda(p,b)->f b
  |x->x
let ret x=
  <@ x@>
type LamBuilder()=
  member this.Bind(x,f)=bind(x,f)
  member this.Return x=ret x

let lamb=LamBuilder()

let l=
  lamb{
    let! l= <@ fun x y->x*y @>
    return (fun x y->x)//普通にwhileでいいんじゃね
  }
println <@  fun a ->a*2 @>