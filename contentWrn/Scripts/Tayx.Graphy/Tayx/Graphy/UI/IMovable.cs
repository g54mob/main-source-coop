using UnityEngine;

namespace Tayx.Graphy.UI
{
	public interface IMovable
	{
		void SetPosition(GraphyManager.ModulePosition newModulePosition, Vector2 offset);
	}
}
