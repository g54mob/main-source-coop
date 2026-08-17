using System;
using UnityEngine;

namespace MapMagic.Nodes
{
	public sealed class GeneratorMenuAttribute : Attribute
	{
		public string menu;

		public int section;

		public string name;

		public string menuName;

		public string iconName;

		public bool disengageable;

		public bool disabled;

		public int priority;

		public string helpLink;

		public bool lookLikePortal;

		public bool drawInlets = true;

		public bool drawOutlet = true;

		public bool drawButtons = true;

		public bool advancedOptions;

		public Type colorType;

		public Type updateType;

		public string codeFile;

		public int codeLine;

		public string nameUpper;

		public float nameWidth;

		public Texture2D icon;

		public Type type;

		public Color color;
	}
}
