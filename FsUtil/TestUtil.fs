namespace Lib
  module TestUtil=
    let randStr len=
      let contstr=["\n";"a";"T";"あ"]
      let rand=new System.Random()
      [0..len] |> Seq.map (fun i-> contstr.[rand.Next(Seq.length contstr)]) |> Seq.fold(fun acc i->acc+i) ""
    //type FontColor={color:System.draw.Color;font:System.font};
    type Args=
        {filename:string;args:string}
    let toArgs (args:array<string>)=
      try 
        let ar=args.[1..] |> Seq.agl(fun acc i->acc + " " + i)
        {filename=args.[0];args=ar}
      with|_->(raise (System.ArgumentException()))

    //start provided exe
    let processBegin args (f:System.Diagnostics.DataReceivedEventArgs->unit)=
      let a=toArgs args
      let p=new System.Diagnostics.Process()
      p.StartInfo.FileName<-a.filename
      p.StartInfo.Arguments<-a.args
      p.StartInfo.UseShellExecute<-true
      p.StartInfo.RedirectStandardOutput<-true

      p.StartInfo.RedirectStandardError <-true
      let disp=p.OutputDataReceived.Subscribe(f)
      p.Start()
      p.BeginOutputReadLine()
      p.BeginErrorReadLine()
      disp

    let writeConsole s font=9
    let writeRich s font rich=0