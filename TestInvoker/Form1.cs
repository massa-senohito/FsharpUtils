using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestInvoker
{
	public partial class Form1 : Form
	{
		public Form1(IDisposable d)
		{
			InitializeComponent();
			var timer = new Timer();
			timer.Interval = 5000;
			timer.Tick += (sender, e) => { this.Close(); d.Dispose(); };
			timer.Start();
			var rich = new RichTextBox();
			//var font=new RichFont()
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			//textBox1.WordWraped
			//boolを返すフィールドを探し、ラップが必要かどうかを調べるフィールドを探すExpressiontreeでリフレクション
			var t1 = textBox1;
			var maxleng = textBox1.Text.Split('\n').Max(x => x.Length);
			var f = t1.Font.SizeInPoints *maxleng;
			var e3 = 2;
		}
	}
}
