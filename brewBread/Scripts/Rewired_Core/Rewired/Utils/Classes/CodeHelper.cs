using System.ComponentModel;

namespace Rewired.Utils.Classes
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public abstract class CodeHelper
	{
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string ToString()
		{
			return base.ToString();
		}
	}
}
