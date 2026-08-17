using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public class CwModifierList
	{
		[SerializeReference]
		private List<CwModifier> modifiers;

		public int Count
		{
			get
			{
				if (modifiers != null)
				{
					return modifiers.Count;
				}
				return 0;
			}
		}

		public List<CwModifier> Instances
		{
			get
			{
				if (modifiers == null)
				{
					modifiers = new List<CwModifier>();
				}
				return modifiers;
			}
		}

		public void ModifyAngle(ref float angle, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyAngle(ref angle, pressure);
				}
			}
		}

		public void ModifyColor(ref Color color, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyColor(ref color, pressure);
				}
			}
		}

		public void ModifyHardness(ref float hardness, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyHardness(ref hardness, pressure);
				}
			}
		}

		public void ModifyOpacity(ref float opacity, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyOpacity(ref opacity, pressure);
				}
			}
		}

		public void ModifyRadius(ref float radius, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyRadius(ref radius, pressure);
				}
			}
		}

		public void ModifyScale(ref Vector3 scale, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyScale(ref scale, pressure);
				}
			}
		}

		public void ModifyTexture(ref Texture texture, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyTexture(ref texture, pressure);
				}
			}
		}

		public void ModifyPosition(ref Vector3 position, bool preview, float pressure)
		{
			if (modifiers == null)
			{
				return;
			}
			foreach (CwModifier modifier in modifiers)
			{
				if (modifier != null && (modifier.Preview || !preview))
				{
					modifier.ModifyPosition(ref position, pressure);
				}
			}
		}
	}
}
