using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
	public static class String
	{
		public static string StripQuotes(string input)
		{
			if (input.Substring(0, 1) == "\"")
			{
				return input.Substring(1, input.Length - 2);
			}
			else
			{
				return input;
			}
		}
	}

}
