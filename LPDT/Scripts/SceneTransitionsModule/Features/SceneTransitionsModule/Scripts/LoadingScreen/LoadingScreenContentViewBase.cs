using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public abstract class LoadingScreenContentViewBase : MonoBehaviour
	{
		[SerializeField]
		private LoadingScreenTipsView _tipsView;

		public LoadingScreenTipsViewBase TipsView => _tipsView;

		public virtual void OnShown()
		{
		}

		public virtual void OnHidden()
		{
		}
	}
}
