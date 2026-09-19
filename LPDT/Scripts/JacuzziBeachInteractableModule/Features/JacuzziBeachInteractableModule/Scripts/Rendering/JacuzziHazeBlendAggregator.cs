using Features.PostProcessingModule.Scripts;
using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts.Rendering
{
	public class JacuzziHazeBlendAggregator
	{
		private readonly PostProcessingModel _postProcessingModel;

		private float _jacuzziBlend;

		private float _drunkBlend;

		private float _lastApplied = -1f;

		public JacuzziHazeBlendAggregator(PostProcessingModel postProcessingModel)
		{
			_postProcessingModel = postProcessingModel;
		}

		public void SetJacuzziBlend(float blend01)
		{
			_jacuzziBlend = Mathf.Clamp01(blend01);
			Apply();
		}

		public void SetDrunkBlend(float blend01)
		{
			_drunkBlend = Mathf.Clamp01(blend01);
			Apply();
		}

		public void Apply()
		{
			float num = Mathf.Max(_jacuzziBlend, _drunkBlend);
			if (!Mathf.Approximately(num, _lastApplied) && _postProcessingModel.ActiveVolumes.TryGetValue(PostProcessingType.SessionSceneMain, out var value) && value.profile.TryGet<JacuzziHazeVolume>(out var component))
			{
				component.isEnabled.Override(num > 0f);
				component.SetBlend(num);
				_lastApplied = num;
			}
		}
	}
}
