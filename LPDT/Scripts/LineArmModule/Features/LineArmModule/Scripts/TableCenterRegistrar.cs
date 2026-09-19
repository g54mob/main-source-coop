using Features.LineArmModule.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	public class TableCenterRegistrar : MonoBehaviour
	{
		[SerializeField]
		private Transform _tableCenter;

		private TableCenterModel _tableCenterModel;

		[Inject]
		private void InjectDependencies(TableCenterModel tableCenterModel)
		{
			_tableCenterModel = tableCenterModel;
		}

		private void OnEnable()
		{
			Transform center = ((_tableCenter != null) ? _tableCenter : base.transform);
			_tableCenterModel.RegisterCenter(center);
		}

		private void OnDisable()
		{
			Transform center = ((_tableCenter != null) ? _tableCenter : base.transform);
			_tableCenterModel.UnregisterCenter(center);
		}
	}
}
