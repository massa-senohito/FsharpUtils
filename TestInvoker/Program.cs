using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace TestInvoker
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{

			//IEE.Generate(0, i => i < 20, i => i, _ => _ + 1).ForEach(Console.WriteLine);//.Select(i => i * 2).ForEach(Console.WriteLine);
			var proc = new Process();
			try
			{
				proc.StartInfo.FileName = args[0];
				if (args.Length >= 2) proc.StartInfo.Arguments = args[1];
			}
			catch (Exception)
			{
				Console.WriteLine("引数の数が足りません");
				return;
			}
			var outputhandle = Lib.TestUtil.processBegin(args, Lib.Util.ActionToFSharpFunc<DataReceivedEventArgs>(proc_OutputDataReceived));
			StartMain(outputhandle);
			//var assem=Assembly.LoadFrom(args[0]);
			//var types = assem.GetTypes().Where(x => x.GetCustomAttributes(typeof(ReportAttribute), true).Count()!=0); 
			//Func<MethodInfo,ReportAttribute> att=m=> Attribute.GetCustomAttributes(typeof(ReportAttribute), true).First() as ReportAttribute;
			////アセンブリを受け取って、クラスから標準で使う色を記憶する、クラスのメソッドの属性から色を変えて
			//var typeAndAttribute=types.Select(t => Tuple.Create(t.GetMethods(),t.GetMethods().Select(att)));
			////SelectMany(t => t.GetMethods()).Where(m=>m.GetCustomAttributes); //ここもリフレクションutilでなんとかする
			////引数渡す方法・・・
			////標準出力くいとってこっちから出すほうが楽かも
			////typeAndAttribute.Do(x=>x.Item1. )
		}
		static void StartMain(IDisposable d)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form1 f1=new Form1(d);
			Application.Run(f1);
		}
		static void proc_OutputDataReceived(DataReceivedEventArgs e)
		{
			var dr=e.Data;

		}
	}
}
