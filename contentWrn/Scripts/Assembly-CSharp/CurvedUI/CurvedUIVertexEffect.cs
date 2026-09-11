using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CurvedUI
{
	[DisallowMultipleComponent]
	public class CurvedUIVertexEffect : BaseMeshEffect
	{
		[Tooltip("Check to skip tesselation pass on this object. CurvedUI will not create additional vertices to make this object have a smoother curve. Checking this can solve some issues if you create your own procedural mesh for this object. Default false.")]
		public bool DoNotTesselate;

		private Canvas myCanvas;

		private CurvedUISettings mySettings;

		private Graphic myGraphic;

		private Text myText;

		private TextMeshProUGUI myTMP;

		private CurvedUITMPSubmesh myTMPSubMesh;

		private bool m_tesselationRequired = true;

		private bool curvingRequired = true;

		private bool TransformMisaligned;

		private Matrix4x4 CanvasToWorld;

		private Matrix4x4 CanvasToLocal;

		private Matrix4x4 MyToWorld;

		private Matrix4x4 MyToLocal;

		private List<UIVertex> m_tesselatedVerts = new List<UIVertex>();

		private List<UIVertex> m_curvedVerts = new List<UIVertex>();

		private List<UIVertex> m_vertsInQuads = new List<UIVertex>();

		private UIVertex m_ret;

		private UIVertex[] m_quad = new UIVertex[4];

		private float[] m_weights = new float[4];

		[SerializeField]
		[HideInInspector]
		private Vector3 savedPos;

		[SerializeField]
		[HideInInspector]
		private Vector3 savedUp;

		[SerializeField]
		[HideInInspector]
		private Vector2 savedRectSize;

		[SerializeField]
		[HideInInspector]
		private Color savedColor;

		[SerializeField]
		[HideInInspector]
		private Vector4 savedTextUV0;

		[SerializeField]
		[HideInInspector]
		private float savedFill;

		private float currentAspect;

		public bool useDelayedUpdate;

		private Action updateEvent;

		private WaitForSeconds updateDelay = new WaitForSeconds(0.08f);

		private bool m_isUpdatingMesh;

		private Vector4 _uv0;

		private Vector4 _uv1;

		private Vector3 _pos;

		private bool tesselationRequired
		{
			get
			{
				return m_tesselationRequired;
			}
			set
			{
				m_tesselationRequired = value;
			}
		}

		public bool TesselationRequired
		{
			get
			{
				return tesselationRequired;
			}
			set
			{
				tesselationRequired = value;
			}
		}

		public bool CurvingRequired
		{
			get
			{
				return curvingRequired;
			}
			set
			{
				curvingRequired = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			myGraphic = GetComponent<Graphic>();
			myText = GetComponent<Text>();
			myTMP = GetComponent<TextMeshProUGUI>();
			myTMPSubMesh = GetComponent<CurvedUITMPSubmesh>();
			Camera main = Camera.main;
			currentAspect = ((main != null) ? main.aspect : 0f);
		}

		protected override void OnEnable()
		{
			FindParentSettings();
			if ((bool)myGraphic)
			{
				myGraphic.RegisterDirtyMaterialCallback(TesselationRequiredCallback);
				myGraphic.SetVerticesDirty();
			}
			if ((bool)myText)
			{
				myText.RegisterDirtyVerticesCallback(TesselationRequiredCallback);
				Font.textureRebuilt += FontTextureRebuiltCallback;
			}
		}

		protected override void Start()
		{
			base.Start();
			if ((bool)myCanvas && (bool)GetComponent<Selectable>())
			{
				Vector3 vector = myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
				RectTransform rectTransform = myCanvas.transform as RectTransform;
				if (vector.x.Abs() > rectTransform.rect.width / 2f || vector.y.Abs() > rectTransform.rect.height / 2f)
				{
					Debug.LogWarning("CurvedUI: " + GetComponent<Selectable>().GetType().Name + " \"" + base.gameObject.name + "\" is outside of the canvas. It will not be interactable. Move it inside the canvas rectangle (white border in scene view) for it to work.", base.gameObject);
				}
				if (vector.z.Abs() > 0.1f)
				{
					Debug.LogWarning("CurvedUI: The Z position of \"" + base.gameObject.name + "\" " + GetComponent<Selectable>().GetType().Name + ", or one of its parents is not 0 in relation to the canvas. The interactions may not be accurate.", base.gameObject);
				}
			}
			Invoke("TryUpdateCurvedVertex", 0.2f);
		}

		protected override void OnDisable()
		{
			if ((bool)myGraphic)
			{
				myGraphic.UnregisterDirtyMaterialCallback(TesselationRequiredCallback);
			}
			if ((bool)myText)
			{
				myText.UnregisterDirtyVerticesCallback(TesselationRequiredCallback);
				Font.textureRebuilt -= FontTextureRebuiltCallback;
			}
		}

		private IEnumerator OnUpdate()
		{
			while (true)
			{
				if (!CanvasUpdateRegistry.IsRebuildingLayout() && !CanvasUpdateRegistry.IsRebuildingGraphics())
				{
					updateEvent?.Invoke();
					updateEvent = null;
				}
				yield return updateDelay;
			}
		}

		private void TesselationRequiredCallback()
		{
			TryUpdateCurvedVertex(teselate: true);
		}

		private void FontTextureRebuiltCallback(Font fontie)
		{
			TryUpdateCurvedVertex(teselate: true);
		}

		private void TryUpdateCurvedVertex(bool teselate)
		{
			tesselationRequired = teselate;
			TryUpdateCurvedVertex();
		}

		public void TryUpdateCurvedVertex()
		{
			if (m_isUpdatingMesh)
			{
				return;
			}
			m_isUpdatingMesh = true;
			float aspect = Camera.main.aspect;
			bool flag = currentAspect != aspect;
			currentAspect = aspect;
			try
			{
				UpdateCurvedVertex();
			}
			finally
			{
				m_isUpdatingMesh = false;
				if (flag)
				{
					Invoke("TryUpdateCurvedVertex", 0.1f);
				}
			}
		}

		private void UpdateCurvedVertex()
		{
			if ((bool)myTMP || (bool)myTMPSubMesh)
			{
				return;
			}
			if (!tesselationRequired)
			{
				if ((base.transform as RectTransform).rect.size != savedRectSize)
				{
					tesselationRequired = true;
				}
				else if (myGraphic != null)
				{
					if (myGraphic.color != savedColor)
					{
						tesselationRequired = true;
						savedColor = myGraphic.color;
					}
					else if (myGraphic is Image && (myGraphic as Image).fillAmount != savedFill)
					{
						tesselationRequired = true;
						savedFill = (myGraphic as Image).fillAmount;
					}
				}
			}
			if (!tesselationRequired && !curvingRequired)
			{
				Vector3 a = mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
				if (!a.AlmostEqual(savedPos) && (mySettings.Shape != CurvedUISettings.CurvedUIShape.CYLINDER || (double)Mathf.Pow(a.x - savedPos.x, 2f) > 1E-05 || (double)Mathf.Pow(a.z - savedPos.z, 2f) > 1E-05))
				{
					savedPos = a;
					curvingRequired = true;
				}
				Vector3 normalized = mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up).normalized;
				if (!savedUp.AlmostEqual(normalized, 0.0001))
				{
					bool flag = normalized.AlmostEqual(Vector3.up.normalized);
					bool flag2 = savedUp.AlmostEqual(Vector3.up.normalized);
					if ((!flag && flag2) || (flag && !flag2))
					{
						tesselationRequired = true;
					}
					savedUp = normalized;
					curvingRequired = true;
				}
			}
			if ((bool)myGraphic && (tesselationRequired || curvingRequired))
			{
				myGraphic.SetVerticesDirty();
			}
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!ShouldModify())
			{
				return;
			}
			CheckTextFontMaterial();
			if (tesselationRequired || !Application.isPlaying)
			{
				if (m_tesselatedVerts == null)
				{
					m_tesselatedVerts = new List<UIVertex>();
				}
				else
				{
					m_tesselatedVerts.Clear();
				}
				vh.GetUIVertexStream(m_tesselatedVerts);
				TesselateGeometry(m_tesselatedVerts);
				savedRectSize = (base.transform as RectTransform).rect.size;
				tesselationRequired = false;
				curvingRequired = true;
			}
			if (curvingRequired)
			{
				CanvasToWorld = myCanvas.transform.localToWorldMatrix;
				CanvasToLocal = myCanvas.transform.worldToLocalMatrix;
				MyToWorld = base.transform.localToWorldMatrix;
				MyToLocal = base.transform.worldToLocalMatrix;
				if (m_curvedVerts == null)
				{
					m_curvedVerts = new List<UIVertex>();
				}
				if (m_curvedVerts.Count == m_tesselatedVerts.Count)
				{
					for (int i = 0; i < m_curvedVerts.Count; i++)
					{
						m_curvedVerts[i] = CurveVertex(m_tesselatedVerts[i], mySettings.Angle, mySettings.GetCyllinderRadiusInCanvasSpace(), (myCanvas.transform as RectTransform).rect.size);
					}
				}
				else
				{
					m_curvedVerts.Clear();
					for (int j = 0; j < m_tesselatedVerts.Count; j++)
					{
						m_curvedVerts.Add(CurveVertex(m_tesselatedVerts[j], mySettings.Angle, mySettings.GetCyllinderRadiusInCanvasSpace(), (myCanvas.transform as RectTransform).rect.size));
					}
				}
				curvingRequired = false;
			}
			vh.Clear();
			if (m_curvedVerts.Count % 4 == 0)
			{
				for (int k = 0; k < m_curvedVerts.Count; k += 4)
				{
					int currentVertCount = vh.currentVertCount;
					for (int l = 0; l < 4; l++)
					{
						vh.AddVert(m_curvedVerts[k + l]);
					}
					vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
					vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
				}
			}
			else
			{
				vh.AddUIVertexTriangleStream(m_curvedVerts);
			}
		}

		public void ModifyTMPMesh(ref List<UIVertex> vertexList)
		{
			if (!ShouldModify())
			{
				return;
			}
			CheckTextFontMaterial();
			tesselationRequired = false;
			curvingRequired = true;
			if (curvingRequired)
			{
				CanvasToWorld = myCanvas.transform.localToWorldMatrix;
				CanvasToLocal = myCanvas.transform.worldToLocalMatrix;
				MyToWorld = base.transform.localToWorldMatrix;
				MyToLocal = base.transform.worldToLocalMatrix;
				for (int i = 0; i < vertexList.Count; i++)
				{
					vertexList[i] = CurveVertex(vertexList[i], mySettings.Angle, mySettings.GetCyllinderRadiusInCanvasSpace(), (myCanvas.transform as RectTransform).rect.size);
				}
				curvingRequired = false;
			}
		}

		private bool ShouldModify()
		{
			if (!IsActive())
			{
				return false;
			}
			if (mySettings == null)
			{
				FindParentSettings();
			}
			if (mySettings == null || !mySettings.enabled || mySettings.Angle == 1)
			{
				return false;
			}
			return true;
		}

		private void CheckTextFontMaterial()
		{
			if ((bool)myText && myText.cachedTextGenerator.verts.Count > 0 && myText.cachedTextGenerator.verts[0].uv0 != savedTextUV0)
			{
				savedTextUV0 = myText.cachedTextGenerator.verts[0].uv0;
				tesselationRequired = true;
			}
		}

		public CurvedUISettings FindParentSettings(bool forceNew = false)
		{
			if (mySettings == null || forceNew)
			{
				mySettings = GetComponentInParent<CurvedUISettings>();
				if (mySettings == null)
				{
					return null;
				}
				myCanvas = mySettings.GetComponent<Canvas>();
			}
			return mySettings;
		}

		private UIVertex CurveVertex(UIVertex input, float cylinder_angle, float radius, Vector2 canvasSize)
		{
			Vector3 position = input.position;
			position = CanvasToLocal.MultiplyPoint3x4(MyToWorld.MultiplyPoint3x4(position));
			if (mySettings.Shape == CurvedUISettings.CurvedUIShape.CYLINDER && mySettings.Angle != 0)
			{
				float f = position.x / canvasSize.x * cylinder_angle * (MathF.PI / 180f);
				radius += position.z;
				position.x = Mathf.Sin(f) * radius;
				position.z += Mathf.Cos(f) * radius - radius;
			}
			else if (mySettings.Shape == CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL && mySettings.Angle != 0)
			{
				float f2 = position.y / canvasSize.y * cylinder_angle * (MathF.PI / 180f);
				radius += position.z;
				position.y = Mathf.Sin(f2) * radius;
				position.z += Mathf.Cos(f2) * radius - radius;
			}
			else if (mySettings.Shape == CurvedUISettings.CurvedUIShape.RING)
			{
				float num = 0f;
				float num2 = position.y.Remap(canvasSize.y * 0.5f * (float)(mySettings.RingFlipVertical ? 1 : (-1)), (0f - canvasSize.y) * 0.5f * (float)(mySettings.RingFlipVertical ? 1 : (-1)), (float)mySettings.RingExternalDiameter * (1f - mySettings.RingFill) * 0.5f, (float)mySettings.RingExternalDiameter * 0.5f);
				float f3 = (position.x / canvasSize.x).Remap(-0.5f, 0.5f, MathF.PI / 2f, cylinder_angle * (MathF.PI / 180f) + MathF.PI / 2f) - num;
				position.x = num2 * Mathf.Cos(f3);
				position.y = num2 * Mathf.Sin(f3);
			}
			else if (mySettings.Shape == CurvedUISettings.CurvedUIShape.SPHERE && mySettings.Angle != 0)
			{
				float num3 = mySettings.VerticalAngle;
				float num4 = 0f - position.z;
				if (mySettings.PreserveAspect)
				{
					num3 = cylinder_angle * (canvasSize.y / canvasSize.x);
				}
				else
				{
					radius = canvasSize.x / 2f;
					if (num3 == 0f)
					{
						return input;
					}
				}
				float num5 = (position.x / canvasSize.x).Remap(-0.5f, 0.5f, (180f - cylinder_angle) / 2f - 90f, 180f - (180f - cylinder_angle) / 2f - 90f);
				num5 *= MathF.PI / 180f;
				float num6 = (position.y / canvasSize.y).Remap(-0.5f, 0.5f, (180f - num3) / 2f, 180f - (180f - num3) / 2f);
				num6 *= MathF.PI / 180f;
				position.z = Mathf.Sin(num6) * Mathf.Cos(num5) * (radius + num4);
				position.y = (0f - (radius + num4)) * Mathf.Cos(num6);
				position.x = Mathf.Sin(num6) * Mathf.Sin(num5) * (radius + num4);
				if (mySettings.PreserveAspect)
				{
					position.z -= radius;
				}
			}
			input.position = MyToLocal.MultiplyPoint3x4(CanvasToWorld.MultiplyPoint3x4(position));
			return input;
		}

		private void TesselateGeometry(List<UIVertex> verts)
		{
			Vector2 tesslationSize = mySettings.GetTesslationSize();
			TransformMisaligned = !savedUp.AlmostEqual(Vector3.up.normalized);
			TrisToQuads(verts);
			if (myText == null && myTMP == null && myTMPSubMesh == null && !DoNotTesselate)
			{
				int count = verts.Count;
				for (int i = 0; i < count; i += 4)
				{
					ModifyQuad(verts, i, tesslationSize);
				}
				verts.RemoveRange(0, count);
			}
		}

		private void ModifyQuad(List<UIVertex> verts, int vertexIndex, Vector2 requiredSize)
		{
			for (int i = 0; i < 4; i++)
			{
				m_quad[i] = verts[vertexIndex + i];
			}
			Vector3 vector = m_quad[2].position - m_quad[1].position;
			Vector3 vector2 = m_quad[1].position - m_quad[0].position;
			if (myGraphic != null && myGraphic is Image && (myGraphic as Image).type == Image.Type.Filled)
			{
				vector = ((vector.x > (m_quad[3].position - m_quad[0].position).x) ? vector : (m_quad[3].position - m_quad[0].position));
				vector2 = ((vector2.y > (m_quad[2].position - m_quad[3].position).y) ? vector2 : (m_quad[2].position - m_quad[3].position));
			}
			int num = 1;
			int num2 = 1;
			if (TransformMisaligned || mySettings.Shape == CurvedUISettings.CurvedUIShape.SPHERE || mySettings.Shape == CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL)
			{
				num2 = Mathf.CeilToInt(vector2.magnitude * (1f / Mathf.Max(0.0001f, requiredSize.y)));
			}
			if (TransformMisaligned || mySettings.Shape != CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL)
			{
				num = Mathf.CeilToInt(vector.magnitude * (1f / Mathf.Max(0.0001f, requiredSize.x)));
			}
			bool flag = false;
			bool flag2 = false;
			float y = 0f;
			for (int j = 0; j < num2 || !flag; j++)
			{
				flag = true;
				float num3 = ((float)j + 1f) / (float)num2;
				float x = 0f;
				for (int k = 0; k < num || !flag2; k++)
				{
					flag2 = true;
					float num4 = ((float)k + 1f) / (float)num;
					verts.Add(TesselateQuad(x, y));
					verts.Add(TesselateQuad(x, num3));
					verts.Add(TesselateQuad(num4, num3));
					verts.Add(TesselateQuad(num4, y));
					x = num4;
				}
				y = num3;
			}
		}

		private void TrisToQuads(List<UIVertex> verts)
		{
			int count = verts.Count;
			m_vertsInQuads.Clear();
			for (int i = 0; i < count; i += 6)
			{
				m_vertsInQuads.Add(verts[i]);
				m_vertsInQuads.Add(verts[i + 1]);
				m_vertsInQuads.Add(verts[i + 2]);
				m_vertsInQuads.Add(verts[i + 4]);
			}
			verts.AddRange(m_vertsInQuads);
			verts.RemoveRange(0, count);
		}

		private UIVertex TesselateQuad(float x, float y)
		{
			m_weights[0] = (1f - x) * (1f - y);
			m_weights[1] = (1f - x) * y;
			m_weights[2] = x * y;
			m_weights[3] = x * (1f - y);
			_uv0 = (_uv1 = Vector2.zero);
			_pos = Vector3.zero;
			for (int i = 0; i < 4; i++)
			{
				_uv0 += m_quad[i].uv0 * m_weights[i];
				_uv1 += m_quad[i].uv1 * m_weights[i];
				_pos += m_quad[i].position * m_weights[i];
			}
			m_ret.position = _pos;
			m_ret.color = m_quad[0].color;
			m_ret.uv0 = _uv0;
			m_ret.uv1 = _uv1;
			m_ret.uv2 = m_quad[0].uv2;
			m_ret.uv3 = m_quad[0].uv3;
			m_ret.normal = m_quad[0].normal;
			m_ret.tangent = m_quad[0].tangent;
			return m_ret;
		}

		public void SetDirty()
		{
			TesselationRequired = true;
		}
	}
}
