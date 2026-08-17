using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AeLa.EasyFeedback.UI.Toaster
{
	public class Toaster : MonoBehaviour
	{
		public enum PopoutDirection
		{
			Up = 0,
			Down = 1,
			Right = 2,
			Left = 3
		}

		public enum ToastAnchor
		{
			TopLeft = 0,
			TopRight = 1,
			BottomRight = 2,
			BottomLeft = 3
		}

		[FormerlySerializedAs("toastPrefab")]
		[Tooltip("The toast prefab object")]
		[SerializeField]
		protected Toast ToastPrefab;

		[FormerlySerializedAs("viewportAnchor")]
		[Tooltip("Where the toast will appear on screen")]
		[SerializeField]
		protected ToastAnchor ViewportAnchor = ToastAnchor.TopRight;

		[FormerlySerializedAs("popupDirection")]
		[Tooltip("Direction the toast will move when it appears")]
		[SerializeField]
		protected PopoutDirection PopupDirection = PopoutDirection.Down;

		[FormerlySerializedAs("duration")]
		[Tooltip("How long (seconds) a message remains on screen")]
		[SerializeField]
		protected float Duration = 1.5f;

		[FormerlySerializedAs("animationDuration")]
		[Tooltip("How long (seconds) the slide in/out animation takes")]
		[SerializeField]
		protected float AnimationDuration = 0.25f;

		private List<Toast> inactive = new List<Toast>();

		public void Toast(string message)
		{
			StartCoroutine(ShowToast(message));
		}

		private IEnumerator ShowToast(string message)
		{
			Toast toast = GetToast(message);
			RectTransform rt = toast.RectTransform;
			float speed = 1f / AnimationDuration;
			Vector2 animationDirection = GetAnimationDirection(PopupDirection);
			Vector2 pivotIn = rt.pivot;
			Vector2 pivotOut = pivotIn - animationDirection;
			yield return SlideAnim(rt, pivotIn, pivotOut, speed);
			yield return new WaitForSeconds(Duration);
			yield return SlideAnim(rt, pivotOut, pivotIn, speed);
			ReturnToast(toast);
		}

		private IEnumerator SlideAnim(RectTransform rt, Vector2 from, Vector2 to, float speed)
		{
			for (float t = 0f; t <= 1f; t += Time.deltaTime * speed)
			{
				float t2 = ((t >= 1f) ? 1f : (1f - Mathf.Pow(2f, -10f * t)));
				rt.pivot = Vector2.Lerp(from, to, t2);
				yield return null;
			}
		}

		private Toast GetToast(string message)
		{
			if (inactive.Count == 0)
			{
				inactive.Add(UnityEngine.Object.Instantiate(ToastPrefab, base.transform));
			}
			Toast toast = inactive[0];
			inactive.RemoveAt(0);
			RectTransform rectTransform = toast.RectTransform;
			Vector2 pivot = rectTransform.pivot;
			switch (ViewportAnchor)
			{
			case ToastAnchor.TopLeft:
			{
				Vector2 anchorMax = (rectTransform.anchorMin = new Vector2(0f, 1f));
				rectTransform.anchorMax = anchorMax;
				pivot.x = 0f;
				pivot.y = 1f;
				break;
			}
			case ToastAnchor.TopRight:
			{
				Vector2 anchorMax = (rectTransform.anchorMin = Vector2.one);
				rectTransform.anchorMax = anchorMax;
				pivot.x = 1f;
				pivot.y = 1f;
				break;
			}
			case ToastAnchor.BottomRight:
			{
				Vector2 anchorMax = (rectTransform.anchorMin = new Vector2(1f, 0f));
				rectTransform.anchorMax = anchorMax;
				pivot.x = 1f;
				pivot.y = 0f;
				break;
			}
			case ToastAnchor.BottomLeft:
			{
				Vector2 anchorMax = (rectTransform.anchorMin = Vector2.zero);
				rectTransform.anchorMax = anchorMax;
				pivot.x = 0f;
				pivot.y = 0f;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			switch (PopupDirection)
			{
			case PopoutDirection.Up:
				pivot.y = 1f;
				break;
			case PopoutDirection.Down:
				pivot.y = 0f;
				break;
			case PopoutDirection.Right:
				pivot.x = 1f;
				break;
			case PopoutDirection.Left:
				pivot.x = 0f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			rectTransform.pivot = pivot;
			rectTransform.anchoredPosition = new Vector2(0f, 0f);
			toast.Message = message;
			toast.gameObject.SetActive(value: true);
			return toast;
		}

		private void ReturnToast(Toast toast)
		{
			toast.gameObject.SetActive(value: false);
			inactive.Add(toast);
		}

		private Vector2 GetAnimationDirection(PopoutDirection direction)
		{
			return direction switch
			{
				PopoutDirection.Up => Vector2.up, 
				PopoutDirection.Down => Vector2.down, 
				PopoutDirection.Right => Vector2.right, 
				PopoutDirection.Left => Vector2.left, 
				_ => throw new ArgumentOutOfRangeException("direction", direction, null), 
			};
		}
	}
}
