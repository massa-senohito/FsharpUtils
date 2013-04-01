module test
open Lib
//[<EntryPoint>]
let main (args:string array)=
  let usage= @"
  ConsoleAppli -i C:\ --search imagefiles
  ConsoleAppli -a C:\ --search moviefiles

  ConsoleAppli -ad C:\ --search moviefiles,output Directry only
              "
//parser = OptionParser()
//parser.add_option("-u", "--update",dest="update", default=False, action="store_true", help="update setup.ini before install")
//parser.add_option("-m", "--mirror",dest="mirror", help="set the mirror path where we get the packages")
//parser.add_option("-c", "--cache",dest="cache", help="set the local cache path")
//parser.add_option("-f", "--file",dest="file", help="rename the package with FILE")
//parser.add_option("-n", "--noscript", dest="noscript", default=False, action="store_true", help="do not run post script file after install")
//pythonのオプションパーサなかなかいいやね
//-a avi mpg AVI MPG 
//-p jpg png gif
//-ignore Program
//って言うような設定ファイルに
  if (args.Length=0 || args.Length>3) then  exit(1)
  //判別共有体にするべき
  let searchmode=
    match args.[0] with
      |"-i" -> Files.ImageTypes
      |"-a" -> Files.MovieTypes
      |"-d" -> Files.DocumentTypes
      |_ ->[]
  //ほんとはenumfile >>= enumdir >>= ignoredir
  if searchmode=[] then Files.getset |> Lib.Seq.print2
  let i=Lib.Files.findonece args.[1] (Files.isTargetType searchmode) |> Set.ofList
  i |> Seq.print2
  //エラー出力とかに投げないと、これまでファイルに入る
  0