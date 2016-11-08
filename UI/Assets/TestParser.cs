using System;
using Assets.Scripts;

namespace AssemblyCSharp
{
	public class TestParser
	{
		public TestParser ()
		{
			String s = "hello";

			GoogleVisionParser.parseAllAnnotations (s);



		}

		public static void Main() {
			Debug.Log ("hello");
		}

	}
}

