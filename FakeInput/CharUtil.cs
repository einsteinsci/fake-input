using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeInput
{
	public static class CharUtil
	{
		public static char ToUpper(this char c)
		{
			string s = "" + c;
			string upper = s.ToUpper();
			return upper[0];
		}
		public static char ToLower(this char c)
		{
			string s = "" + c;
			string lower = s.ToLower();
			return lower[0];
		}

		public static bool IsCapital(this char c)
		{
			const int A = 0x41;
			const int Z = 0x5A;

			return c >= A && c <= Z;
		}

		public static bool IsLetter(this char c)
		{
			return c.ToUpper().IsCapital();
		}

		public static bool IsNumber(this char c)
		{
			const int ZERO = 0x30;
			const int NINE = 0x39;

			return c >= ZERO && c <= NINE;
		}

		public static bool IsAlphanumeric(this char c)
		{
			return c.IsLetter() || c.IsNumber();
		}
	}
}
