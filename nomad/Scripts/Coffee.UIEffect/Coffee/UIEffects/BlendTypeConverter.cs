using UnityEngine.Rendering;

namespace Coffee.UIEffects
{
	internal static class BlendTypeConverter
	{
		public static (BlendMode, BlendMode) Convert(this (BlendType type, BlendMode src, BlendMode dst) self)
		{
			return self.type switch
			{
				BlendType.AlphaBlend => (BlendMode.One, BlendMode.OneMinusSrcAlpha), 
				BlendType.Multiply => (BlendMode.DstColor, BlendMode.OneMinusSrcAlpha), 
				BlendType.Additive => (BlendMode.One, BlendMode.One), 
				BlendType.SoftAdditive => (BlendMode.OneMinusDstColor, BlendMode.One), 
				BlendType.MultiplyAdditive => (BlendMode.DstColor, BlendMode.One), 
				_ => (self.src, self.dst), 
			};
		}

		public static BlendType Convert(this (BlendMode src, BlendMode dst) self)
		{
			switch (self.src)
			{
			case BlendMode.One:
				switch (self.dst)
				{
				case BlendMode.OneMinusSrcAlpha:
					return BlendType.AlphaBlend;
				case BlendMode.One:
					return BlendType.Additive;
				}
				break;
			case BlendMode.DstColor:
				switch (self.dst)
				{
				case BlendMode.OneMinusSrcAlpha:
					return BlendType.Multiply;
				case BlendMode.One:
					return BlendType.MultiplyAdditive;
				}
				break;
			case BlendMode.OneMinusDstColor:
			{
				BlendMode item = self.dst;
				if (item != BlendMode.One)
				{
					break;
				}
				return BlendType.SoftAdditive;
			}
			}
			return BlendType.Custom;
		}
	}
}
