namespace Lib
  module GCUtil=

    type GC=System.GC
    type GCH= System.Runtime.InteropServices.GCHandle

    type GCHandler=
      HandleObject of GCH
    //http://www.atmarkit.co.jp/fdotnet/directxworld/directxworld06/directxworld06_02.html
    let getTotal o=
      (GC.GetTotalMemory o |> float) / (1000000.0) //1M byte= 1000000 byte
    let maxG =
      GC.MaxGeneration
    let collect=
      GC.Collect()
    let addHandle i=
      GCH.Alloc i |> HandleObject
    let free (HandleObject handle)=
      handle.Free()