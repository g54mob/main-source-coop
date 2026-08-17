using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEElementMaterialData
	{
		public Shader shader;

		public string shaderName = "";

		public List<TVEElementPropertyData> props;
	}
}
