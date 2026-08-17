using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UnityEngine.UI
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class PinchableScrollRect : ScrollRect, IPinchStartHandler, IPinchEndHandler, IPinchZoomHandler
	{
		[Serializable]
		public class PinchEvent : UnityEvent<Vector3>
		{
		}

		[Header("Pinch Settings")]
		[SerializeField]
		protected bool resetOnEnable = true;

		[SerializeField]
		protected bool lockPinchCenter = true;

		private Vector2 initPivot;

		private Vector2 initAnchored;

		private Vector3 initScale;

		private float zoomVelocity;

		private Vector2 zoomPosDelta = Vector2.zero;

		private bool updatePivot;

		protected bool isZooming;

		protected Vector2 pinchStartPos;

		public Vector3 lowerScale = Vector3.one;

		public Vector3 upperScale = new Vector3(2f, 2f, 2f);

		[SerializeField]
		protected float pinchSensitivity = 0.01f;

		[SerializeField]
		protected float zoomMaxSpeed = 0.2f;

		[SerializeField]
		[Range(1f, 0f)]
		protected float zoomDeceleration = 0.8f;

		private bool initialized;

		[SerializeField]
		private PinchEvent _onScaleChanged = new PinchEvent();

		public PinchEvent onScaleChanged
		{
			get
			{
				return _onScaleChanged;
			}
			set
			{
				_onScaleChanged = value;
			}
		}

		protected override void Start()
		{
			base.Start();
			initPivot = base.content.pivot;
			initAnchored = base.content.anchoredPosition;
			initScale = base.content.localScale;
			initialized = true;
			if (resetOnEnable)
			{
				ResetContent();
			}
		}

		protected override void OnEnable()
		{
			if (resetOnEnable && initialized)
			{
				ResetContent();
			}
			ResetZoom();
			base.OnEnable();
		}

		public virtual void OnPinchStart(PinchEventData eventData)
		{
			if (IsActive())
			{
				ResetZoom();
				base.OnEndDrag(eventData.unchangedPointerData);
				pinchStartPos = eventData.midPoint;
			}
		}

		public virtual void OnPinchEnd(PinchEventData eventData)
		{
			if (IsActive())
			{
				OnInitializePotentialDrag(eventData.targetPointerData);
				base.OnBeginDrag(eventData.unchangedPointerData);
			}
		}

		public virtual void OnPinchZoom(PinchEventData eventData)
		{
			if (!IsActive())
			{
				return;
			}
			float num = eventData.distanceDelta * pinchSensitivity;
			RectTransform rectTransform = base.content;
			Vector3 localScale = rectTransform.localScale;
			if ((!(num < 0f) || !(localScale.x <= lowerScale.x) || !(localScale.y <= lowerScale.y) || !(localScale.z <= lowerScale.z)) && (!(num > 0f) || !(localScale.x >= upperScale.x) || !(localScale.y >= upperScale.y) || !(localScale.z >= upperScale.z)))
			{
				Vector2 localPoint = Vector2.zero;
				if ((!lockPinchCenter || RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pinchStartPos, eventData.targetPointerData.pressEventCamera, out localPoint)) && (lockPinchCenter || RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.midPoint, eventData.targetPointerData.pressEventCamera, out localPoint)))
				{
					isZooming = true;
					zoomVelocity = num;
					zoomPosDelta = localPoint;
					updatePivot = true;
				}
			}
		}

		public override void OnScroll(PointerEventData eventData)
		{
			if (IsActive())
			{
				OnInitializePotentialDrag(eventData);
				float num = eventData.scrollDelta.y * base.scrollSensitivity;
				RectTransform rectTransform = base.content;
				Vector3 localScale = rectTransform.localScale;
				if ((!(num < 0f) || !(localScale.x <= lowerScale.x) || !(localScale.y <= lowerScale.y) || !(localScale.z <= lowerScale.z)) && (!(num > 0f) || !(localScale.x >= upperScale.x) || !(localScale.y >= upperScale.y) || !(localScale.z >= upperScale.z)) && RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out var localPoint))
				{
					isZooming = true;
					zoomVelocity = num;
					zoomPosDelta = localPoint;
					updatePivot = true;
				}
			}
		}

		protected virtual void Update()
		{
			if (!(Mathf.Abs(zoomVelocity) > 0.001f))
			{
				return;
			}
			if (zoomVelocity > 0f)
			{
				if (zoomVelocity > zoomMaxSpeed)
				{
					HandleZoom(zoomMaxSpeed);
				}
				else
				{
					HandleZoom(zoomVelocity);
				}
			}
			else if (zoomVelocity < 0f - zoomMaxSpeed)
			{
				HandleZoom(0f - zoomMaxSpeed);
			}
			else
			{
				HandleZoom(zoomVelocity);
			}
			zoomVelocity *= zoomDeceleration;
		}

		protected override void LateUpdate()
		{
			if (base.movementType == MovementType.Clamped)
			{
				base.LateUpdate();
			}
			else if (isZooming)
			{
				isZooming = false;
				UpdatePrevData();
			}
			else
			{
				base.LateUpdate();
			}
		}

		protected virtual void HandleZoom(float zoomValue)
		{
			Vector3 localScale = base.content.localScale;
			Rect rect = base.content.rect;
			if (updatePivot)
			{
				Vector2 anchorMin = base.content.anchorMin;
				Vector2 anchorMax = base.content.anchorMax;
				Vector3 localPosition = base.content.localPosition;
				Vector2 vector = new Vector2(zoomPosDelta.x / rect.width, zoomPosDelta.y / rect.height);
				base.content.anchorMin = new Vector2(0.5f, 0.5f);
				base.content.anchorMax = new Vector2(0.5f, 0.5f);
				UpdateBounds();
				SetContentPivotPosition(base.content.pivot + vector);
				localPosition += new Vector3(zoomPosDelta.x * localScale.x, zoomPosDelta.y * localScale.y);
				base.content.anchorMin = anchorMin;
				base.content.anchorMax = anchorMax;
				base.content.localPosition = localPosition;
			}
			Vector3 vector2 = localScale + Vector3.one * zoomValue;
			vector2 = new Vector3(Mathf.Clamp(vector2.x, lowerScale.x, upperScale.x), Mathf.Clamp(vector2.y, lowerScale.y, upperScale.y), Mathf.Clamp(vector2.z, lowerScale.z, upperScale.z));
			SetContentLocalScale(vector2);
			zoomPosDelta = Vector2.zero;
			updatePivot = false;
		}

		protected virtual void SetContentPivotPosition(Vector2 pivot)
		{
			Vector2 pivot2 = base.content.pivot;
			if (!base.horizontal)
			{
				pivot.x = pivot2.x;
			}
			if (!base.vertical)
			{
				pivot.y = pivot2.y;
			}
			if (!(pivot == pivot2))
			{
				base.content.pivot = pivot;
			}
		}

		protected virtual void SetContentLocalScale(Vector3 newScale)
		{
			base.content.localScale = newScale;
			_onScaleChanged.Invoke(newScale);
		}

		protected void ResetZoom()
		{
			zoomVelocity = 0f;
			zoomPosDelta = Vector2.zero;
			updatePivot = false;
		}

		public void SetNormalizedScale(float normalized)
		{
			Vector3 contentLocalScale = Vector3.Lerp(lowerScale, upperScale, normalized);
			SetContentLocalScale(contentLocalScale);
		}

		public virtual void ResetContent()
		{
			if ((bool)base.content)
			{
				base.content.pivot = initPivot;
				base.content.anchoredPosition = initAnchored;
				base.content.localScale = initScale;
				UpdateBounds();
				_onScaleChanged.Invoke(initScale);
			}
		}

		public override void OnDrag(PointerEventData eventData)
		{
			if (!eventData.used)
			{
				base.OnDrag(eventData);
			}
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (!eventData.used)
			{
				base.OnBeginDrag(eventData);
			}
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			if (!eventData.used)
			{
				base.OnEndDrag(eventData);
			}
		}
	}
}
