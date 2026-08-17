using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Objectives
{
	[ExecuteAlways]
	public sealed class ObjectivesPanelMaxHeightFitter : MonoBehaviour
	{
		[SerializeField]
		private RectTransform target;

		[SerializeField]
		private RectTransform content;

		[SerializeField]
		private float maxHeight = 350f;

		[SerializeField]
		private float minHeight;

		private void LateUpdate()
		{
			if (!(target == null) && !(content == null))
			{
				float num = Mathf.Clamp(LayoutUtility.GetPreferredHeight(content), minHeight, maxHeight);
				Vector2 sizeDelta = target.sizeDelta;
				if (!Mathf.Approximately(sizeDelta.y, num))
				{
					target.sizeDelta = new Vector2(sizeDelta.x, num);
				}
			}
		}
	}
}
