using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TestInvoker
{
	[Report(ConsoleColor.Blue)]
	class Content
	{
		string error;
		string result;

	}
	class Reporter
	{
		void Error(Content text)
		{
			var att =text.GetType().Attributes;
		}
	}
}
