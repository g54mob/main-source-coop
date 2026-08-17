using System;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEMaterialSettings
	{
		public bool baseMask;

		public bool useMultiMask;

		public bool meshMaskRG;

		public bool meshMaskBA;

		public bool useProjMask;

		public bool useLumaMask;

		public bool texCoords;

		public bool useImpostorShader;

		public bool useImpostorFeature;
	}
}
