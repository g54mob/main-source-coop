using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	public class LightObjectAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private List<LightObjectGroup> _lightObjectTypes = new List<LightObjectGroup>();

		private LightObjectsModel _lightObjectsModel;

		private Light _light;

		[Inject]
		private void InjectDependencies(LightObjectsModel lightObjectsModel)
		{
			_lightObjectsModel = lightObjectsModel;
		}

		private void Awake()
		{
			_light = base.gameObject.GetComponent<Light>();
			if (_light != null)
			{
				_lightObjectsModel.AddLightObject(_lightObjectTypes, _light);
			}
		}

		private void OnDestroy()
		{
			if (!(_light == null) && _lightObjectsModel != null)
			{
				_lightObjectsModel.RemoveLightObject(_lightObjectTypes, _light);
			}
		}
	}
}
