namespace Lib
open Lib
open System.Net
module TCPUtil=
  type ConnectionStatus<'a>=
    |Disconnected
    |Error of string
    |Connected of 'a
  type TSocket=
    {ip:IPAddress;port:int}
  let PermittedSocket=
    {ip=IPAddress.Parse("");port=12000}

  let permit (ac:NetworkAccess)=
    new SocketPermission(ac,TransportType.Tcp,"",12000)

  let beginConnect (cl:Sockets.TcpClient) (ip:IPAddress) port=
    fun (res,state) -> cl.BeginConnect(ip,port,res,state);

  let makeConnection host port=
    new Sockets.TcpClient(host,port)

  let waitConnection ip port=
    Sockets.TcpListener(ip,port)
  //let aConnect onConnect=
  //  Async.FromBeginEnd(
      
  let serverCallback (listener:Sockets.TcpListener) onConnect onReceive=
    listener.Start(10)
    
    async{
      
      let! ac=Async.FromBeginEnd(listener.BeginAcceptTcpClient,listener.EndAcceptTcpClient)
      //let con=Async.RunSynchronously(Async.FromBeginEnd(beginConnect ac PermittedSocket.ip PermittedSocket.port,ac.EndConnect))
      return ac
      }
  module SocketA=
    open System.Net.Sockets
    let s=new Sockets.Socket(AddressFamily.InterNetworkV6,SocketType.Stream,ProtocolType.IPv6)
    let receiveFile (e:Socket)=
      let asyncres=new SocketAsyncEventArgs()
      e.ReceiveAsync(asyncres)
      asyncres.Buffer
  module P2PUtil=
    let p2p=new System.Net.Cookie();
