using System.Collections;
using UnityEngine;

namespace solidocean
{
	public class Credits : MonoBehaviour
	{
		public float speed = 100f;

		public float beginPosition = -825f;

		public float endPosition = 825f;

		private RectTransform creditsRect;

		[SerializeField]
		private bool canLoop;

		private void OnEnable()
		{
			creditsRect = GetComponent<RectTransform>();
			StartCoroutine(AutoScroll());
		}

		private void OnDisable()
		{
			creditsRect.localPosition = new Vector3(creditsRect.localPosition.x, beginPosition, creditsRect.localPosition.z);
		}

		private IEnumerator AutoScroll()
		{
			while (creditsRect.localPosition.y < endPosition)
			{
				creditsRect.Translate(Vector3.up * speed * Time.deltaTime);
				if (creditsRect.localPosition.y > endPosition)
				{
					if (!canLoop)
					{
						break;
					}
					creditsRect.localPosition = Vector3.up * beginPosition;
				}
				yield return null;
			}
		}
	}
}
