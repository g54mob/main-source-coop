using System.Collections.Generic;
using System.ComponentModel;

namespace Rewired.Utils.Classes.Data
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InspectorValue<T>
	{
		private T BvurWbZYLXuBnjFBzvpicuZrERkD;

		private bool bmlrulMKUGnjmuKdRkozPZVrmGyd;

		public bool isSet => bmlrulMKUGnjmuKdRkozPZVrmGyd;

		public T value
		{
			get
			{
				return BvurWbZYLXuBnjFBzvpicuZrERkD;
			}
			set
			{
				BvurWbZYLXuBnjFBzvpicuZrERkD = value;
				bmlrulMKUGnjmuKdRkozPZVrmGyd = true;
			}
		}

		public bool SetIfChanged(T value)
		{
			if (!bmlrulMKUGnjmuKdRkozPZVrmGyd)
			{
				this.value = value;
				return false;
			}
			if (!EqualityComparer<T>.Default.Equals(BvurWbZYLXuBnjFBzvpicuZrERkD, value))
			{
				this.value = value;
				return true;
			}
			return false;
		}

		public void Clear()
		{
			bmlrulMKUGnjmuKdRkozPZVrmGyd = false;
			BvurWbZYLXuBnjFBzvpicuZrERkD = default(T);
		}
	}
}
