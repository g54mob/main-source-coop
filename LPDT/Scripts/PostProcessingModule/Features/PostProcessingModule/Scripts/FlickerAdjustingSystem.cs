using System;
using Features.PostProcessingModule.Scripts.Rendering;
using UnityEngine.Rendering;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class FlickerAdjustingSystem : IInitializable, IDisposable
	{
		private readonly PostProcessingModel _postProcessingModel;

		private readonly FlickerAdjustingModel _flickerAdjustingModel;

		public FlickerAdjustingSystem(PostProcessingModel postProcessingModel, FlickerAdjustingModel flickerAdjustingModel)
		{
			_postProcessingModel = postProcessingModel;
			_flickerAdjustingModel = flickerAdjustingModel;
		}

		public void Initialize()
		{
			_flickerAdjustingModel.OnBlendChanged += ApplyBlendToProfiles;
			ApplyBlendToProfiles(0f);
		}

		public void Dispose()
		{
			_flickerAdjustingModel.OnBlendChanged -= ApplyBlendToProfiles;
		}

		private void ApplyBlendToProfiles(float blend)
		{
			foreach (Volume value in _postProcessingModel.ActiveVolumes.Values)
			{
				if (value.profile.TryGet<FlickerVolume>(out var component))
				{
					component.blend.Override(blend);
				}
			}
		}
	}
}
