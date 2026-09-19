using UnityEngine;
using UnityEngine.UI;

namespace Features.CustomUiModule.Scripts
{
	public class EnchasedScrollRect : ScrollRect
	{
		[SerializeField]
		private bool _isWithHandleReseize = true;

		[SerializeField]
		private float _fixedHandleSize;

		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (_isWithHandleReseize)
			{
				return;
			}
			if (base.horizontalScrollbar != null && base.horizontalScrollbar.size != _fixedHandleSize)
			{
				if (m_ContentBounds.size.y > 0f)
				{
					base.horizontalScrollbar.size = _fixedHandleSize;
				}
				else
				{
					base.horizontalScrollbar.size = 1f;
				}
			}
			if (base.verticalScrollbar != null && base.verticalScrollbar.size != _fixedHandleSize)
			{
				if (m_ContentBounds.size.y > 0f)
				{
					base.verticalScrollbar.size = _fixedHandleSize;
				}
				else
				{
					base.verticalScrollbar.size = 1f;
				}
			}
		}
	}
}
