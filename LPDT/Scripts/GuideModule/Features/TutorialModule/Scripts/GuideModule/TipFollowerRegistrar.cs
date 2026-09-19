using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipFollowerRegistrar : MonoBehaviour
	{
		[SerializeField]
		private TipFollower _tipFollower;

		private TipFollowerDataHolder _tipFollowerDataHolder;

		[Inject]
		public void InjectDependencies(TipFollowerDataHolder tipFollowerDataHolder)
		{
			_tipFollowerDataHolder = tipFollowerDataHolder;
		}

		private void OnEnable()
		{
			_tipFollowerDataHolder.TipFollower = _tipFollower;
		}

		private void OnDisable()
		{
			_tipFollowerDataHolder.TipFollower = null;
		}
	}
}
