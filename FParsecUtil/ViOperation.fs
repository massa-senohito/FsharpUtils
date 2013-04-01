module ViOperation
  type ViObj<'a>=
    {value:'a}
    member this.Save(stream)=stream this.value
    member this.Load(data)={value=data}
    //delete
    //sort
    //yunc
