using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CurvedUI
{
	[ExecuteInEditMode]
	public class CurvedUITMP : MonoBehaviour
	{
		private CurvedUIVertexEffect crvdVE;

		private TextMeshProUGUI tmpText;

		private CurvedUISettings mySettings;

		private List<UIVertex> m_UIVerts = new List<UIVertex>();

		private UIVertex m_tempVertex;

		private CurvedUITMPSubmesh m_tempSubMsh;

		private Vector2 savedSize;

		private Vector3 savedUp;

		private Vector3 savedPos;

		private Vector3 savedLocalScale;

		private Vector3 savedGlobalScale;

		private List<CurvedUITMPSubmesh> subMeshes = new List<CurvedUITMPSubmesh>();

		public bool Dirty;

		private bool curvingRequired;

		private bool tesselationRequired;

		private bool quitting;

		private bool isUpdatingMesh;

		private Vector3[] vertices;

		private float currentAspect;

		public bool useDelayedUpdate;

		private Action updateEvent;

		private WaitForSeconds updateDelay = new WaitForSeconds(0.08f);

		private void Start()
		{
			if (mySettings == null)
			{
				mySettings = GetComponentInParent<CurvedUISettings>();
			}
			Camera main = Camera.main;
			currentAspect = ((main != null) ? main.aspect : 0f);
			Invoke("TryUpdateCurvedTextMesh", 0.2f);
		}

		private void OnEnable()
		{
			FindTMP();
			if ((bool)tmpText)
			{
				tmpText.RegisterDirtyMaterialCallback(TesselationRequiredCallback);
				TMPro_EventManager.TEXT_CHANGED_EVENT.Add(TMPTextChangedCallback);
				tmpText.SetText(tmpText.text);
			}
		}

		private void OnDisable()
		{
			if ((bool)tmpText)
			{
				tmpText.UnregisterDirtyMaterialCallback(TesselationRequiredCallback);
				TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(TMPTextChangedCallback);
			}
			updateEvent = null;
		}

		private void OnDestroy()
		{
			quitting = true;
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

		private void CreateUIVertexList(Mesh mesh)
		{
			if (mesh.vertexCount < m_UIVerts.Count)
			{
				m_UIVerts.RemoveRange(mesh.vertexCount, m_UIVerts.Count - mesh.vertexCount);
			}
			vertices = mesh.vertices;
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				if (m_UIVerts.Count <= i)
				{
					m_tempVertex = default(UIVertex);
					GetUIVertexFromMesh(ref m_tempVertex, i);
					m_UIVerts.Add(m_tempVertex);
				}
				else
				{
					m_tempVertex = m_UIVerts[i];
					GetUIVertexFromMesh(ref m_tempVertex, i);
					m_UIVerts[i] = m_tempVertex;
				}
			}
		}

		private void GetUIVertexFromMesh(ref UIVertex vert, int i)
		{
			vert.position = vertices[i];
		}

		private void FillMeshWithUIVertexList(Mesh mesh, List<UIVertex> list)
		{
			if (list.Count >= 65536)
			{
				Debug.LogError("CURVEDUI: Unity UI Mesh can not have more than 65536 vertices. Remove some UI elements or lower quality.");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				vertices[i] = list[i].position;
			}
			mesh.vertices = vertices;
			mesh.RecalculateBounds();
		}

		private void FindTMP()
		{
			if (TryGetComponent<TextMeshProUGUI>(out tmpText))
			{
				crvdVE = base.gameObject.GetComponent<CurvedUIVertexEffect>();
				mySettings = GetComponentInParent<CurvedUISettings>();
				base.transform.hasChanged = false;
				FindSubmeshes();
			}
		}

		private void FindSubmeshes()
		{
			TMP_SubMeshUI[] componentsInChildren = GetComponentsInChildren<TMP_SubMeshUI>();
			foreach (TMP_SubMeshUI tMP_SubMeshUI in componentsInChildren)
			{
				m_tempSubMsh = tMP_SubMeshUI.gameObject.AddComponentIfMissing<CurvedUITMPSubmesh>();
				if (!subMeshes.Contains(m_tempSubMsh))
				{
					subMeshes.Add(m_tempSubMsh);
				}
			}
		}

		private bool ShouldTesselate()
		{
			if (savedSize != (base.transform as RectTransform).rect.size)
			{
				return true;
			}
			if (savedLocalScale != mySettings.transform.localScale)
			{
				return true;
			}
			if (savedGlobalScale != mySettings.transform.lossyScale)
			{
				return true;
			}
			if (!savedUp.AlmostEqual(mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up)))
			{
				return true;
			}
			Vector3 b = mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
			if (!savedPos.AlmostEqual(b) && (mySettings.Shape != CurvedUISettings.CurvedUIShape.CYLINDER || (double)Mathf.Pow(b.x - savedPos.x, 2f) > 1E-05 || (double)Mathf.Pow(b.z - savedPos.z, 2f) > 1E-05))
			{
				return true;
			}
			return false;
		}

		private void TMPTextChangedCallback(object obj)
		{
			if (obj.Equals(tmpText))
			{
				TryUpdateCurvedTextMesh(tesselationRequired: true, curvingRequired: false);
			}
		}

		private void TesselationRequiredCallback()
		{
			TryUpdateCurvedTextMesh(tesselationRequired: true, curvingRequired: true);
		}

		private void TryUpdateCurvedTextMesh(bool tesselationRequired, bool curvingRequired)
		{
			this.tesselationRequired = tesselationRequired;
			this.curvingRequired = curvingRequired;
			TryUpdateCurvedTextMesh();
		}

		private void TryUpdateCurvedTextMesh()
		{
			if (isUpdatingMesh)
			{
				return;
			}
			isUpdatingMesh = true;
			float aspect = Camera.main.aspect;
			bool flag = currentAspect != aspect;
			currentAspect = aspect;
			try
			{
				UpdateCurvedTextMesh();
			}
			finally
			{
				isUpdatingMesh = false;
				if (flag)
				{
					Invoke("TryUpdateCurvedTextMesh", 0.1f);
				}
			}
		}

		private void UpdateCurvedTextMesh()
		{
			if (!tmpText)
			{
				FindTMP();
			}
			if (mySettings == null || !tmpText || quitting)
			{
				return;
			}
			if (ShouldTesselate())
			{
				tesselationRequired = true;
			}
			if (Dirty || tesselationRequired || (curvingRequired && !Application.isPlaying))
			{
				if (mySettings == null)
				{
					base.enabled = false;
					return;
				}
				tmpText.renderMode = TextRenderFlags.Render;
				tmpText.ForceMeshUpdate(ignoreActiveState: true);
				CreateUIVertexList(tmpText.mesh);
				crvdVE.ModifyTMPMesh(ref m_UIVerts);
				FillMeshWithUIVertexList(tmpText.mesh, m_UIVerts);
				tmpText.renderMode = TextRenderFlags.DontRender;
				savedLocalScale = mySettings.transform.localScale;
				savedGlobalScale = mySettings.transform.lossyScale;
				savedSize = (base.transform as RectTransform).rect.size;
				savedUp = mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up);
				savedPos = mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
				tesselationRequired = false;
				curvingRequired = false;
				Dirty = false;
				FindSubmeshes();
				foreach (CurvedUITMPSubmesh subMesh in subMeshes)
				{
					subMesh.UpdateSubmesh(tesselate: true, curve: false);
				}
			}
			if (!string.IsNullOrEmpty(tmpText.text) && tmpText.text.Length > 0)
			{
				tmpText.canvasRenderer.SetMesh(tmpText.mesh);
			}
			else
			{
				tmpText.canvasRenderer.Clear();
			}
		}
	}
}
