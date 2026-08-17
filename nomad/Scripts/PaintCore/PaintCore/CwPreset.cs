using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPreset")]
	[AddComponentMenu("")]
	public class CwPreset : MonoBehaviour
	{
		[SerializeField]
		private string title;

		[SerializeField]
		private List<string> shaderPaths;

		private static List<CwPreset> cachedPresets;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		public List<string> ShaderPaths
		{
			get
			{
				if (shaderPaths == null)
				{
					shaderPaths = new List<string>();
				}
				return shaderPaths;
			}
		}
	}
}
