using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CurvedUI
{
	[AddComponentMenu("CurvedUI/CurvedUISettings")]
	[RequireComponent(typeof(Canvas))]
	public class CurvedUISettings : MonoBehaviour
	{
		public enum CurvedUIShape
		{
			CYLINDER = 0,
			RING = 1,
			SPHERE = 2,
			CYLINDER_VERTICAL = 3
		}

		public const string Version = "3.4";

		[SerializeField]
		private CurvedUIShape shape;

		[SerializeField]
		private float quality = 1f;

		[SerializeField]
		private bool interactable = true;

		[SerializeField]
		private bool blocksRaycasts = true;

		[SerializeField]
		private bool forceUseBoxCollider;

		[SerializeField]
		private int angle = 90;

		[SerializeField]
		private bool preserveAspect = true;

		[SerializeField]
		private int vertAngle = 90;

		[SerializeField]
		private float ringFill = 0.5f;

		[SerializeField]
		private int ringExternalDiamater = 1000;

		[SerializeField]
		private bool ringFlipVertical;

		private int baseCircleSegments = 16;

		private Vector2 savedRectSize;

		private float savedRadius;

		private Canvas myCanvas;

		private RectTransform m_rectTransform;

		private RectTransform RectTransform
		{
			get
			{
				if (m_rectTransform == null)
				{
					m_rectTransform = base.transform as RectTransform;
				}
				return m_rectTransform;
			}
		}

		public int BaseCircleSegments => baseCircleSegments;

		public int Angle
		{
			get
			{
				return angle;
			}
			set
			{
				if (angle != value)
				{
					SetUIAngle(value);
				}
			}
		}

		public float Quality
		{
			get
			{
				return quality;
			}
			set
			{
				if (quality != value)
				{
					quality = value;
					SetUIAngle(angle);
				}
			}
		}

		public CurvedUIShape Shape
		{
			get
			{
				return shape;
			}
			set
			{
				if (shape != value)
				{
					shape = value;
					SetUIAngle(angle);
				}
			}
		}

		public int VerticalAngle
		{
			get
			{
				return vertAngle;
			}
			set
			{
				if (vertAngle != value)
				{
					vertAngle = value;
					SetUIAngle(angle);
				}
			}
		}

		public float RingFill
		{
			get
			{
				return ringFill;
			}
			set
			{
				if (ringFill != value)
				{
					ringFill = value;
					SetUIAngle(angle);
				}
			}
		}

		public float SavedRadius
		{
			get
			{
				if (savedRadius == 0f)
				{
					savedRadius = GetCyllinderRadiusInCanvasSpace();
				}
				return savedRadius;
			}
		}

		public int RingExternalDiameter
		{
			get
			{
				return ringExternalDiamater;
			}
			set
			{
				if (ringExternalDiamater != value)
				{
					ringExternalDiamater = value;
					SetUIAngle(angle);
				}
			}
		}

		public bool RingFlipVertical
		{
			get
			{
				return ringFlipVertical;
			}
			set
			{
				if (ringFlipVertical != value)
				{
					ringFlipVertical = value;
					SetUIAngle(angle);
				}
			}
		}

		public bool PreserveAspect
		{
			get
			{
				return preserveAspect;
			}
			set
			{
				if (preserveAspect != value)
				{
					preserveAspect = value;
					SetUIAngle(angle);
				}
			}
		}

		public bool Interactable
		{
			get
			{
				return interactable;
			}
			set
			{
				interactable = value;
			}
		}

		public bool ForceUseBoxCollider
		{
			get
			{
				return forceUseBoxCollider;
			}
			set
			{
				forceUseBoxCollider = value;
			}
		}

		public bool BlocksRaycasts
		{
			get
			{
				return blocksRaycasts;
			}
			set
			{
				blocksRaycasts = value;
			}
		}

		[Obsolete("Use RaycastLayerMask property instead.")]
		public bool RaycastMyLayerOnly
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		private void Awake()
		{
			if (base.gameObject.layer == 0)
			{
				base.gameObject.layer = 5;
			}
			savedRectSize = RectTransform.rect.size;
		}

		private void Start()
		{
			if (Application.isPlaying)
			{
				BaseRaycaster[] components = GetComponents<BaseRaycaster>();
				foreach (BaseRaycaster baseRaycaster in components)
				{
					baseRaycaster.enabled = false;
				}
				base.gameObject.AddComponentIfMissing<CurvedUIRaycaster>();
				Canvas[] componentsInChildren = GetComponentsInChildren<Canvas>();
				foreach (Canvas canvas in componentsInChildren)
				{
					if (canvas.gameObject != base.gameObject)
					{
						Transform parent = canvas.transform;
						string text = parent.name;
						while (parent.parent != null)
						{
							text = parent.parent.name + "/" + text;
							parent = parent.parent;
						}
						Debug.LogWarning("CURVEDUI: Interactions on nested canvases are not supported. You won't be able to interact with any child object of [" + text + "]", canvas.gameObject);
					}
				}
			}
			if (myCanvas == null)
			{
				myCanvas = GetComponent<Canvas>();
			}
			savedRadius = GetCyllinderRadiusInCanvasSpace();
		}

		private void OnEnable()
		{
			Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetAllDirty();
			}
		}

		private void OnDisable()
		{
			Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetAllDirty();
			}
		}

		private void Update()
		{
			if (RectTransform.rect.size != savedRectSize)
			{
				savedRectSize = RectTransform.rect.size;
				SetUIAngle(angle);
			}
			if (savedRectSize.x == 0f || savedRectSize.y == 0f)
			{
				Debug.LogError("CurvedUI: Your Canvas size must be bigger than 0!");
			}
		}

		private void SetUIAngle(int newAngle)
		{
			if (myCanvas == null)
			{
				myCanvas = GetComponent<Canvas>();
			}
			if (newAngle == 0)
			{
				newAngle = 1;
			}
			angle = newAngle;
			savedRadius = GetCyllinderRadiusInCanvasSpace();
			CurvedUIVertexEffect[] componentsInChildren = GetComponentsInChildren<CurvedUIVertexEffect>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetDirty();
			}
			Graphic[] componentsInChildren2 = GetComponentsInChildren<Graphic>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].SetAllDirty();
			}
		}

		private Vector3 CanvasToCyllinder(Vector3 pos)
		{
			float f = pos.x / savedRectSize.x * (float)Angle * (MathF.PI / 180f);
			pos.x = Mathf.Sin(f) * (SavedRadius + pos.z);
			pos.z += Mathf.Cos(f) * (SavedRadius + pos.z) - (SavedRadius + pos.z);
			return pos;
		}

		private Vector3 CanvasToCyllinderVertical(Vector3 pos)
		{
			float f = pos.y / savedRectSize.y * (float)Angle * (MathF.PI / 180f);
			pos.y = Mathf.Sin(f) * (SavedRadius + pos.z);
			pos.z += Mathf.Cos(f) * (SavedRadius + pos.z) - (SavedRadius + pos.z);
			return pos;
		}

		private Vector3 CanvasToRing(Vector3 pos)
		{
			float num = pos.y.Remap(savedRectSize.y * 0.5f * (float)(RingFlipVertical ? 1 : (-1)), (0f - savedRectSize.y) * 0.5f * (float)(RingFlipVertical ? 1 : (-1)), (float)RingExternalDiameter * (1f - RingFill) * 0.5f, (float)RingExternalDiameter * 0.5f);
			float f = (pos.x / savedRectSize.x).Remap(-0.5f, 0.5f, MathF.PI / 2f, (float)angle * (MathF.PI / 180f) + MathF.PI / 2f);
			pos.x = num * Mathf.Cos(f);
			pos.y = num * Mathf.Sin(f);
			return pos;
		}

		private Vector3 CanvasToSphere(Vector3 pos)
		{
			float num = SavedRadius;
			float num2 = VerticalAngle;
			if (PreserveAspect)
			{
				num2 = (float)angle * (savedRectSize.y / savedRectSize.x);
				num += ((Angle > 0) ? (0f - pos.z) : pos.z);
			}
			else
			{
				num = savedRectSize.x / 2f + pos.z;
				if (num2 == 0f)
				{
					return Vector3.zero;
				}
			}
			float num3 = (pos.x / savedRectSize.x).Remap(-0.5f, 0.5f, (float)(180 - angle) / 2f - 90f, 180f - (float)(180 - angle) / 2f - 90f);
			num3 *= MathF.PI / 180f;
			float num4 = (pos.y / savedRectSize.y).Remap(-0.5f, 0.5f, (180f - num2) / 2f, 180f - (180f - num2) / 2f);
			num4 *= MathF.PI / 180f;
			pos.z = Mathf.Sin(num4) * Mathf.Cos(num3) * num;
			pos.y = (0f - num) * Mathf.Cos(num4);
			pos.x = Mathf.Sin(num4) * Mathf.Sin(num3) * num;
			if (PreserveAspect)
			{
				pos.z -= num;
			}
			return pos;
		}

		public void AddEffectToChildren()
		{
			Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>(includeInactive: true);
			foreach (Graphic graphic in componentsInChildren)
			{
				if (graphic.GetComponent<CurvedUIVertexEffect>() == null)
				{
					graphic.gameObject.AddComponent<CurvedUIVertexEffect>();
					graphic.SetAllDirty();
				}
			}
			InputField[] componentsInChildren2 = GetComponentsInChildren<InputField>(includeInactive: true);
			foreach (InputField inputField in componentsInChildren2)
			{
				if (inputField.GetComponent<CurvedUIInputFieldCaret>() == null)
				{
					inputField.gameObject.AddComponent<CurvedUIInputFieldCaret>();
				}
			}
			TextMeshProUGUI[] componentsInChildren3 = GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
			foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren3)
			{
				if (textMeshProUGUI.GetComponent<CurvedUITMP>() == null)
				{
					textMeshProUGUI.gameObject.AddComponent<CurvedUITMP>();
					textMeshProUGUI.SetAllDirty();
				}
			}
			TMP_InputField[] componentsInChildren4 = GetComponentsInChildren<TMP_InputField>(includeInactive: true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].AddComponentIfMissing<CurvedUITMPInputFieldCaret>();
			}
		}

		public Vector3 VertexPositionToCurvedCanvas(Vector3 pos)
		{
			return Shape switch
			{
				CurvedUIShape.CYLINDER => CanvasToCyllinder(pos), 
				CurvedUIShape.CYLINDER_VERTICAL => CanvasToCyllinderVertical(pos), 
				CurvedUIShape.RING => CanvasToRing(pos), 
				CurvedUIShape.SPHERE => CanvasToSphere(pos), 
				_ => Vector3.zero, 
			};
		}

		public Vector3 CanvasToCurvedCanvas(Vector3 pos)
		{
			pos = VertexPositionToCurvedCanvas(pos);
			if (float.IsNaN(pos.x) || float.IsInfinity(pos.x))
			{
				return Vector3.zero;
			}
			return base.transform.localToWorldMatrix.MultiplyPoint3x4(pos);
		}

		public Vector3 CanvasToCurvedCanvasNormal(Vector3 pos)
		{
			pos = VertexPositionToCurvedCanvas(pos);
			switch (Shape)
			{
			case CurvedUIShape.CYLINDER:
				return base.transform.localToWorldMatrix.MultiplyVector((pos - new Vector3(0f, 0f, 0f - GetCyllinderRadiusInCanvasSpace())).ModifyY(0f)).normalized;
			case CurvedUIShape.CYLINDER_VERTICAL:
				return base.transform.localToWorldMatrix.MultiplyVector((pos - new Vector3(0f, 0f, 0f - GetCyllinderRadiusInCanvasSpace())).ModifyX(0f)).normalized;
			case CurvedUIShape.RING:
				return -base.transform.forward;
			case CurvedUIShape.SPHERE:
			{
				Vector3 vector = (PreserveAspect ? new Vector3(0f, 0f, 0f - GetCyllinderRadiusInCanvasSpace()) : Vector3.zero);
				return base.transform.localToWorldMatrix.MultiplyVector(pos - vector).normalized;
			}
			default:
				return Vector3.zero;
			}
		}

		public float GetCyllinderRadiusInCanvasSpace()
		{
			float result = ((!PreserveAspect) ? (RectTransform.rect.size.x * 0.5f / Mathf.Sin(Mathf.Clamp(angle, -180f, 180f) * 0.5f * (MathF.PI / 180f))) : ((shape != CurvedUIShape.CYLINDER_VERTICAL) ? (RectTransform.rect.size.x / (MathF.PI * 2f * ((float)angle / 360f))) : (RectTransform.rect.size.y / (MathF.PI * 2f * ((float)angle / 360f)))));
			if (angle != 0)
			{
				return result;
			}
			return 0f;
		}

		public Vector2 GetTesslationSize(bool modifiedByQuality = true)
		{
			Vector2 size = RectTransform.rect.size;
			if (Angle != 0 || (!PreserveAspect && vertAngle != 0))
			{
				switch (shape)
				{
				case CurvedUIShape.CYLINDER:
				case CurvedUIShape.RING:
				case CurvedUIShape.CYLINDER_VERTICAL:
					size /= GetSegmentsByAngle(angle);
					break;
				case CurvedUIShape.SPHERE:
					size.x /= GetSegmentsByAngle(angle);
					if (PreserveAspect)
					{
						size.y = size.x * RectTransform.rect.size.y / RectTransform.rect.size.x;
					}
					else
					{
						size.y /= GetSegmentsByAngle(VerticalAngle);
					}
					break;
				}
			}
			return size / (modifiedByQuality ? Mathf.Clamp(Quality, 0.01f, 10f) : 1f);
		}

		private float GetSegmentsByAngle(float angle)
		{
			if (angle.Abs() <= 1f)
			{
				return 1f;
			}
			if (angle.Abs() < 90f)
			{
				return (float)baseCircleSegments * Mathf.Abs(angle).Remap(0f, 90f, 0.01f, 0.5f);
			}
			return (float)baseCircleSegments * Mathf.Abs(angle).Remap(90f, 360f, 0.5f, 1f);
		}

		public void SetAllChildrenDirty(bool recalculateCurveOnly = false)
		{
			CurvedUIVertexEffect[] componentsInChildren = GetComponentsInChildren<CurvedUIVertexEffect>();
			foreach (CurvedUIVertexEffect curvedUIVertexEffect in componentsInChildren)
			{
				if (recalculateCurveOnly)
				{
					curvedUIVertexEffect.SetDirty();
				}
				else
				{
					curvedUIVertexEffect.CurvingRequired = true;
				}
			}
		}
	}
}
