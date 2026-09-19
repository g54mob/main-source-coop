using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class VolumeAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private Volume _volume;

		[SerializeField]
		private PostProcessingType _postProcessingType;

		private PostProcessingModel _postProcessingModel;

		[Inject]
		public void InjectDependencies(PostProcessingModel postProcessingModel)
		{
			_postProcessingModel = postProcessingModel;
		}

		private void OnEnable()
		{
			_postProcessingModel.RegisterVolume(_postProcessingType, _volume);
		}

		private void OnDisable()
		{
			_postProcessingModel?.UnregisterVolume(_postProcessingType);
		}
	}
}
