using System;

namespace Den.Tools
{
	[Serializable]
	public class Edges<T>
	{
		public CoordRect rect;

		public T[] array;

		public T this[int x, int z]
		{
			get
			{
				if (x < rect.offset.x || x >= rect.offset.x + rect.size.x)
				{
					throw new Exception("Edges: x:" + x + " is out of range:" + rect.offset.x + "-" + (rect.offset.x + rect.size.x));
				}
				if (z < rect.offset.z || z >= rect.offset.z + rect.size.z)
				{
					throw new Exception("Edges: z:" + z + " is out of range:" + rect.offset.z + "-" + (rect.offset.z + rect.size.z));
				}
				if (z == rect.offset.z)
				{
					return array[x - rect.offset.x];
				}
				if (z == rect.offset.z + rect.size.z - 1)
				{
					return array[rect.size.x + x - rect.offset.x];
				}
				if (x == rect.offset.x)
				{
					return array[rect.size.x * 2 + z - rect.offset.z];
				}
				if (x == rect.offset.x + rect.size.x - 1)
				{
					return array[rect.size.x * 2 + rect.size.z + z - rect.offset.z];
				}
				string[] obj = new string[6]
				{
					"Edges: improper x:",
					x.ToString(),
					" and z:",
					z.ToString(),
					" while rect is:",
					null
				};
				CoordRect coordRect = rect;
				obj[5] = coordRect.ToString();
				throw new Exception(string.Concat(obj));
			}
			set
			{
				if (x < rect.offset.x || x >= rect.offset.x + rect.size.x)
				{
					throw new Exception("Edges: x:" + x + " is out of range:" + rect.offset.x + "-" + (rect.offset.x + rect.size.x));
				}
				if (z < rect.offset.z || z >= rect.offset.z + rect.size.z)
				{
					throw new Exception("Edges: z:" + z + " is out of range:" + rect.offset.z + "-" + (rect.offset.z + rect.size.z));
				}
				if (z == rect.offset.z)
				{
					array[x - rect.offset.x] = value;
					return;
				}
				if (z == rect.offset.z + rect.size.z - 1)
				{
					array[rect.size.x + x - rect.offset.x] = value;
					return;
				}
				if (x == rect.offset.x)
				{
					array[rect.size.x * 2 + z - rect.offset.z] = value;
					return;
				}
				if (x == rect.offset.x + rect.size.x - 1)
				{
					array[rect.size.x * 2 + rect.size.z + z - rect.offset.z] = value;
					return;
				}
				string[] obj = new string[6]
				{
					"Edges: improper x:",
					x.ToString(),
					" and z:",
					z.ToString(),
					" while rect is:",
					null
				};
				CoordRect coordRect = rect;
				obj[5] = coordRect.ToString();
				throw new Exception(string.Concat(obj));
			}
		}

		public Edges()
		{
		}

		public Edges(CoordRect rect)
		{
			this.rect = rect;
			array = new T[(rect.size.x + rect.size.z) * 2];
		}
	}
}
