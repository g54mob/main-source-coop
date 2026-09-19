using System;

namespace UnityHFSM.Inspection
{
	public class RootStateMachinePath : StateMachinePath, IEquatable<RootStateMachinePath>
	{
		public const string name = "Root";

		public static readonly RootStateMachinePath instance = new RootStateMachinePath();

		public override string LastNodeName => "Root";

		private RootStateMachinePath()
			: base(null)
		{
		}

		public override int GetHashCode()
		{
			return "Root".GetHashCode();
		}

		public override bool Equals(StateMachinePath other)
		{
			return Equals(other as RootStateMachinePath);
		}

		public bool Equals(RootStateMachinePath other)
		{
			return (object)other != null;
		}

		public override string ToString()
		{
			return "Root";
		}
	}
}
