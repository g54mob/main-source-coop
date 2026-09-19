using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class SimpleTipEntity : TipEntity
	{
		[SerializeField]
		private Renderer[] _renderers;

		private MaterialPropertyBlock _propertyBlock;

		protected override void InitializeRenderer()
		{
			base.InitializeRenderer();
			Renderer[] renderers = _renderers;
			foreach (Renderer obj in renderers)
			{
				obj.material = new Material(obj.material);
			}
		}

		protected override void ApplyDissolve(float amount)
		{
			if (_renderers == null)
			{
				return;
			}
			if (_propertyBlock == null)
			{
				_propertyBlock = new MaterialPropertyBlock();
			}
			Renderer[] renderers = _renderers;
			foreach (Renderer renderer in renderers)
			{
				if (!(renderer == null))
				{
					renderer.GetPropertyBlock(_propertyBlock);
					_propertyBlock.SetFloat(TipEntity.DissolveId, amount);
					renderer.SetPropertyBlock(_propertyBlock);
				}
			}
		}
	}
}
