using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TestInvoker
{
	class RichFont
	{
		Color ToColor(ConsoleColor c)
		{
			var tem = Color.Red;
			switch (c)
			{
				case ConsoleColor.Black:
					 tem=Color.Black;
					 break;
				case ConsoleColor.Blue:
					 tem = Color.Blue;
					break;
				case ConsoleColor.Cyan:
					tem = Color.Cyan;
					break;
				case ConsoleColor.DarkBlue:
					break;
				case ConsoleColor.DarkCyan:
					break;
				case ConsoleColor.DarkGray:
					break;
				case ConsoleColor.DarkGreen:
					break;
				case ConsoleColor.DarkMagenta:
					break;
				case ConsoleColor.DarkRed:
					break;
				case ConsoleColor.DarkYellow:
					break;
				case ConsoleColor.Gray:
					tem = Color.Gray;
					break;
				case ConsoleColor.Green:
					tem = Color.Green;
					break;
				case ConsoleColor.Magenta:
					tem = Color.Magenta;
					break;
				case ConsoleColor.Red:
					tem = Color.Red;
					break;
				case ConsoleColor.White:
					tem = Color.White;
					break;
				case ConsoleColor.Yellow:
					tem = Color.Yellow;
					break;
				default:
					tem = Color.White;
					break;
			}
			return tem;
		}
		public Font Font { get; private set; }
		public ConsoleColor Ccolor { get; private set; }
		public RichFont(Font font, ConsoleColor color)
		{
			this.Font = font; this.Ccolor = color;
		}
		public Color Color
		{
			get
			{
				return ToColor(Ccolor);
			}
		}
	}
	class RichIO
	{
		RichTextBox box;
		RichFont richfont;
		public RichIO(RichFont rf, RichTextBox rtb)
		{
			richfont = rf;
			box = box;
		}
		public void WriteBox(string str)
		{
			box.AppendText(str);
			box.Select(box.Text.Length - str.Length, str.Length);
			box.SelectionColor = richfont.Color;
			box.SelectionFont = richfont.Font;

		}
		public static void Write(string str, ConsoleColor ccolor)
		{
			Console.ForegroundColor = ccolor;
			Console.WriteLine(str);
			Console.ResetColor();
		}
	}
}
