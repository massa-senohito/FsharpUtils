using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestInvoker
{
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Method)]
	class ReportAttribute : Attribute
	{
		public System.ConsoleColor WriteColor;
		public ReportAttribute(System.ConsoleColor c)
		{
			WriteColor = c;
		}
	}
}
