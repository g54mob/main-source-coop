using UnityEngine.Rendering.HighDefinition;

namespace EvilCore.CustomPass
{
	internal class CustomPassHandle
	{
		public string Id { get; }

		public CustomPassVolume Volume { get; }

		public bool IsEnabled
		{
			get
			{
				if (Volume != null)
				{
					return Volume.enabled;
				}
				return false;
			}
			set
			{
				if (Volume != null)
				{
					Volume.enabled = value;
				}
			}
		}

		public CustomPassHandle(string id, CustomPassVolume volume)
		{
			Id = id;
			Volume = volume;
		}

		public FullScreenCustomPass FindFullScreenPass()
		{
			foreach (UnityEngine.Rendering.HighDefinition.CustomPass customPass in Volume.customPasses)
			{
				if (customPass is FullScreenCustomPass result)
				{
					return result;
				}
			}
			return null;
		}

		public DrawRenderersCustomPass FindDrawRenderersPass()
		{
			foreach (UnityEngine.Rendering.HighDefinition.CustomPass customPass in Volume.customPasses)
			{
				if (customPass is DrawRenderersCustomPass result)
				{
					return result;
				}
			}
			return null;
		}
	}
}
