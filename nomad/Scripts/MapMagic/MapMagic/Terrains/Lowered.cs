using System;
using Den.Tools;
using UnityEngine;

namespace MapMagic.Terrains
{
	[Serializable]
	public struct Lowered
	{
		[SerializeField]
		private bool lowered_x;

		[SerializeField]
		private bool lowered_X;

		[SerializeField]
		private bool lowered_z;

		[SerializeField]
		private bool lowered_Z;

		public bool this[Coord dir, Coord thisCoord]
		{
			get
			{
				bool result = false;
				if (dir.x == 0 && dir.z == -1)
				{
					result = lowered_x;
				}
				else if (dir.x == 0 && dir.z == 1)
				{
					result = lowered_X;
				}
				else if (dir.x == -1 && dir.z == 0)
				{
					result = lowered_z;
				}
				else if (dir.x == 1 && dir.z == 0)
				{
					result = lowered_Z;
				}
				string[] obj = new string[6] { "CHECK Low state for ", null, null, null, null, null };
				Coord coord = thisCoord;
				obj[1] = coord.ToString();
				obj[2] = " dir ";
				coord = dir;
				obj[3] = coord.ToString();
				obj[4] = " is ";
				obj[5] = result.ToString();
				Debug.Log(string.Concat(obj));
				return result;
			}
		}

		public bool this[Coord dir]
		{
			get
			{
				if (dir.x == 0 && dir.z == -1)
				{
					return lowered_x;
				}
				if (dir.x == 0 && dir.z == 1)
				{
					return lowered_X;
				}
				if (dir.x == -1 && dir.z == 0)
				{
					return lowered_z;
				}
				if (dir.x == 1 && dir.z == 0)
				{
					return lowered_Z;
				}
				return false;
			}
			set
			{
				if (dir.x == 0 && dir.z == -1)
				{
					lowered_x = value;
				}
				else if (dir.x == 0 && dir.z == 1)
				{
					lowered_X = value;
				}
				else if (dir.x == -1 && dir.z == 0)
				{
					lowered_z = value;
				}
				else if (dir.x == 1 && dir.z == 0)
				{
					lowered_Z = value;
				}
			}
		}
	}
}
