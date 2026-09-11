using UnityEngine.UIElements;

namespace Zorro.Core.CLI
{
	public abstract class DebugPage : VisualElement
	{
		public DebugPage()
		{
			base.styleSheets.Add(SingletonAsset<CoreGlobalDependencies>.Instance.DebugPageStyleSheets);
		}

		private void Removed(DetachFromPanelEvent e)
		{
		}

		public virtual void Update()
		{
		}
	}
}
