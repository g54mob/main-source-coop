using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Features.StreamersSupportModule.Scripts
{
	public class StandsAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private List<StreamerStand> _stands;

		private StandsModel _standsModel;

		[Inject]
		private void InjectDependencies(StandsModel standsModel)
		{
			_standsModel = standsModel;
		}

		private void Awake()
		{
			for (int i = 0; i < _stands.Count; i++)
			{
				_stands[i].Initialize(i);
			}
			_standsModel.RegisterStands(_stands);
		}

		private void OnDestroy()
		{
			_standsModel.UnregisterStands(_stands);
		}
	}
}
