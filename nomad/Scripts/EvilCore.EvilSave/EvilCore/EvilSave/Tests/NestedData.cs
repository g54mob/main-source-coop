using System;
using System.Collections.Generic;

namespace EvilCore.EvilSave.Tests
{
	[Serializable]
	public class NestedData
	{
		public string label;

		public SimpleData inner;

		public List<int> scores;
	}
}
