using System.ComponentModel;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Internal
{
	[AddComponentMenu("")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public class GUIText : MonoBehaviour
	{
		private string plDySQghlnGMxJDApVzMzsdtAzDk;

		private GUIStyle GQbLuHNElCwoaTeQMysXyDhZoSVt;

		private TextAnchor iwwsOyhHdRdwAElwHdQCtBwsJbxQA;

		private TextAlignment zyfAckagJEpAnilevWaXKdWxBeev;

		private float flLsuncOouIqWwqUUIcaDmMFGsVL;

		private Font dwrpHavMScdLoBdcAPzyGrooFHsOA;

		private int DeqAJbeCTjbnxOYqvXVlTzoyruHr = -1;

		private FontStyle VvITnZpRrsHUYWGazgMpLpFpYZuG;

		private Color LnqQdgvbMLuLxYcefdFcdwTHADxDA = Color.white;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Vector2 _pixelOffset;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _useUnityUI;

		private bool PVvuPKnEjhSYTkDIZEVsrlZdYSHN;

		private bool qeFiOVEayHOHeOSNPDOmKfsmZimwA;

		private bool cZkFKOBuYjNpDpfRaYXdROLepRgmA;

		private bool WHSDcKmyiONEpyVLUUdysqSpPhMv;

		private bool QeAjYaZOFneuzuMkMOtWfamljODk;

		private bool CGuFAlZUmFuhFRHhDaRYEvVdANnw;

		private bool YcAeScGpcuyYpoSZxdfYHPnjHtJyB;

		private Text iBARsbBgJalatvSQMpKbpjuOApxp;

		private bool qRVVjTWFGEUwbmVDwlfKLlQzqdYM;

		private bool HixilYgnPTbdgcJPGouUELeLijtEb;

		public string text
		{
			get
			{
				return plDySQghlnGMxJDApVzMzsdtAzDk;
			}
			set
			{
				plDySQghlnGMxJDApVzMzsdtAzDk = value;
			}
		}

		public TextAnchor anchor
		{
			get
			{
				return iwwsOyhHdRdwAElwHdQCtBwsJbxQA;
			}
			set
			{
				iwwsOyhHdRdwAElwHdQCtBwsJbxQA = value;
				PVvuPKnEjhSYTkDIZEVsrlZdYSHN = true;
				if (GQbLuHNElCwoaTeQMysXyDhZoSVt != null)
				{
					GQbLuHNElCwoaTeQMysXyDhZoSVt.alignment = value;
				}
			}
		}

		public TextAlignment alignment
		{
			get
			{
				return zyfAckagJEpAnilevWaXKdWxBeev;
			}
			set
			{
				zyfAckagJEpAnilevWaXKdWxBeev = value;
				qeFiOVEayHOHeOSNPDOmKfsmZimwA = true;
			}
		}

		public float lineSpacing
		{
			get
			{
				return flLsuncOouIqWwqUUIcaDmMFGsVL;
			}
			set
			{
				flLsuncOouIqWwqUUIcaDmMFGsVL = value;
				cZkFKOBuYjNpDpfRaYXdROLepRgmA = true;
				_ = GQbLuHNElCwoaTeQMysXyDhZoSVt;
			}
		}

		public Font font
		{
			get
			{
				return dwrpHavMScdLoBdcAPzyGrooFHsOA;
			}
			set
			{
				WHSDcKmyiONEpyVLUUdysqSpPhMv = true;
				dwrpHavMScdLoBdcAPzyGrooFHsOA = value;
				if (GQbLuHNElCwoaTeQMysXyDhZoSVt != null)
				{
					GQbLuHNElCwoaTeQMysXyDhZoSVt.font = value;
				}
			}
		}

		public int fontSize
		{
			get
			{
				return DeqAJbeCTjbnxOYqvXVlTzoyruHr;
			}
			set
			{
				DeqAJbeCTjbnxOYqvXVlTzoyruHr = value;
				QeAjYaZOFneuzuMkMOtWfamljODk = true;
				if (GQbLuHNElCwoaTeQMysXyDhZoSVt != null)
				{
					GQbLuHNElCwoaTeQMysXyDhZoSVt.fontSize = value;
				}
			}
		}

		public FontStyle fontStyle
		{
			get
			{
				return VvITnZpRrsHUYWGazgMpLpFpYZuG;
			}
			set
			{
				VvITnZpRrsHUYWGazgMpLpFpYZuG = value;
				CGuFAlZUmFuhFRHhDaRYEvVdANnw = true;
				if (GQbLuHNElCwoaTeQMysXyDhZoSVt != null)
				{
					GQbLuHNElCwoaTeQMysXyDhZoSVt.fontStyle = value;
				}
			}
		}

		public Color color
		{
			get
			{
				return LnqQdgvbMLuLxYcefdFcdwTHADxDA;
			}
			set
			{
				LnqQdgvbMLuLxYcefdFcdwTHADxDA = value;
				YcAeScGpcuyYpoSZxdfYHPnjHtJyB = true;
				if (GQbLuHNElCwoaTeQMysXyDhZoSVt != null)
				{
					GQbLuHNElCwoaTeQMysXyDhZoSVt.normal.textColor = value;
				}
			}
		}

		public Vector2 pixelOffset
		{
			get
			{
				return _pixelOffset;
			}
			set
			{
				_pixelOffset = value;
			}
		}

		public bool useUnityUI
		{
			get
			{
				return _useUnityUI;
			}
			set
			{
				if (_useUnityUI != value)
				{
					_useUnityUI = value;
					qRVVjTWFGEUwbmVDwlfKLlQzqdYM = value;
					if (value)
					{
						LctZWriHVqioxWrYTdfBcDICtZsI();
					}
					else
					{
						MlLGeJczucuvpdsEUkBsilTOChTj();
					}
				}
			}
		}

		[CustomObfuscation(rename = false)]
		private void Awake()
		{
			HixilYgnPTbdgcJPGouUELeLijtEb = true;
		}

		[CustomObfuscation(rename = false)]
		private void Start()
		{
			qRVVjTWFGEUwbmVDwlfKLlQzqdYM = _useUnityUI;
			if (_useUnityUI)
			{
				LctZWriHVqioxWrYTdfBcDICtZsI();
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnGUI()
		{
			if (!_useUnityUI)
			{
				if (GQbLuHNElCwoaTeQMysXyDhZoSVt == null)
				{
					wGGqHkrAFzjAgmwIdmVhbkyvajyI();
				}
				if (!string.IsNullOrEmpty(plDySQghlnGMxJDApVzMzsdtAzDk))
				{
					Vector2 vector = base.transform.localPosition;
					GUI.Label(new Rect(vector.x * (float)Screen.width + _pixelOffset.x, vector.y * (float)Screen.height + _pixelOffset.y, MathTools.Clamp((float)Screen.width - vector.x * (float)Screen.width, 0f, float.MaxValue), MathTools.Clamp((float)Screen.height - vector.y * (float)Screen.height, 0f, float.MaxValue)), plDySQghlnGMxJDApVzMzsdtAzDk, GQbLuHNElCwoaTeQMysXyDhZoSVt);
				}
			}
		}

		[CustomObfuscation(rename = false)]
		private void Update()
		{
			if (!_useUnityUI)
			{
				return;
			}
			if (iBARsbBgJalatvSQMpKbpjuOApxp == null)
			{
				Logger.LogError("Text component has been deleted.");
				return;
			}
			RectTransform component = iBARsbBgJalatvSQMpKbpjuOApxp.GetComponent<RectTransform>();
			if (component.anchoredPosition != _pixelOffset)
			{
				component.anchoredPosition = _pixelOffset;
			}
			iBARsbBgJalatvSQMpKbpjuOApxp.text = plDySQghlnGMxJDApVzMzsdtAzDk;
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			if (HixilYgnPTbdgcJPGouUELeLijtEb && _useUnityUI != qRVVjTWFGEUwbmVDwlfKLlQzqdYM)
			{
				qRVVjTWFGEUwbmVDwlfKLlQzqdYM = _useUnityUI;
				if (_useUnityUI)
				{
					LctZWriHVqioxWrYTdfBcDICtZsI();
				}
				else
				{
					MlLGeJczucuvpdsEUkBsilTOChTj();
				}
			}
		}

		private void LctZWriHVqioxWrYTdfBcDICtZsI()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (UnityTools.GetComponentInSelfOrParents<Canvas>(base.transform) == null)
			{
				GameObject gameObject;
				if (base.transform.root == base.transform)
				{
					gameObject = new GameObject("Canvas");
					base.transform.SetParent(gameObject.transform, worldPositionStays: true);
				}
				else
				{
					gameObject = base.transform.root.gameObject;
				}
				gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
				if (!(gameObject.GetComponent<CanvasScaler>() != null))
				{
					gameObject.AddComponent<CanvasScaler>();
				}
				else
				{
					gameObject.GetComponent<CanvasScaler>();
				}
			}
			iBARsbBgJalatvSQMpKbpjuOApxp = GetComponent<Text>();
			if (iBARsbBgJalatvSQMpKbpjuOApxp == null)
			{
				RectTransform rectTransform = base.gameObject.AddComponent<RectTransform>();
				rectTransform.anchorMax = new Vector2(1f, 1f);
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.localPosition = Vector2.zero;
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.sizeDelta = Vector3.zero;
				iBARsbBgJalatvSQMpKbpjuOApxp = base.gameObject.AddComponent<Text>();
				iBARsbBgJalatvSQMpKbpjuOApxp.color = Color.white;
				iBARsbBgJalatvSQMpKbpjuOApxp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				iBARsbBgJalatvSQMpKbpjuOApxp.fontSize = 13;
				if (PVvuPKnEjhSYTkDIZEVsrlZdYSHN)
				{
					iBARsbBgJalatvSQMpKbpjuOApxp.alignment = iwwsOyhHdRdwAElwHdQCtBwsJbxQA;
				}
				else
				{
					iwwsOyhHdRdwAElwHdQCtBwsJbxQA = iBARsbBgJalatvSQMpKbpjuOApxp.alignment;
				}
				if (WHSDcKmyiONEpyVLUUdysqSpPhMv)
				{
					iBARsbBgJalatvSQMpKbpjuOApxp.font = dwrpHavMScdLoBdcAPzyGrooFHsOA;
				}
				else
				{
					dwrpHavMScdLoBdcAPzyGrooFHsOA = iBARsbBgJalatvSQMpKbpjuOApxp.font;
				}
				if (QeAjYaZOFneuzuMkMOtWfamljODk)
				{
					iBARsbBgJalatvSQMpKbpjuOApxp.fontSize = DeqAJbeCTjbnxOYqvXVlTzoyruHr;
				}
				else
				{
					DeqAJbeCTjbnxOYqvXVlTzoyruHr = iBARsbBgJalatvSQMpKbpjuOApxp.fontSize;
				}
				if (CGuFAlZUmFuhFRHhDaRYEvVdANnw)
				{
					iBARsbBgJalatvSQMpKbpjuOApxp.fontStyle = VvITnZpRrsHUYWGazgMpLpFpYZuG;
				}
				else
				{
					VvITnZpRrsHUYWGazgMpLpFpYZuG = iBARsbBgJalatvSQMpKbpjuOApxp.fontStyle;
				}
				if (YcAeScGpcuyYpoSZxdfYHPnjHtJyB)
				{
					iBARsbBgJalatvSQMpKbpjuOApxp.color = LnqQdgvbMLuLxYcefdFcdwTHADxDA;
				}
				else
				{
					LnqQdgvbMLuLxYcefdFcdwTHADxDA = iBARsbBgJalatvSQMpKbpjuOApxp.color;
				}
			}
		}

		private void MlLGeJczucuvpdsEUkBsilTOChTj()
		{
			if (Application.isPlaying)
			{
				if (iBARsbBgJalatvSQMpKbpjuOApxp != null)
				{
					iBARsbBgJalatvSQMpKbpjuOApxp.text = string.Empty;
				}
				iBARsbBgJalatvSQMpKbpjuOApxp = null;
			}
		}

		private void wGGqHkrAFzjAgmwIdmVhbkyvajyI()
		{
			GQbLuHNElCwoaTeQMysXyDhZoSVt = new GUIStyle(GUI.skin.label);
			if (PVvuPKnEjhSYTkDIZEVsrlZdYSHN)
			{
				GQbLuHNElCwoaTeQMysXyDhZoSVt.alignment = iwwsOyhHdRdwAElwHdQCtBwsJbxQA;
			}
			else
			{
				iwwsOyhHdRdwAElwHdQCtBwsJbxQA = GQbLuHNElCwoaTeQMysXyDhZoSVt.alignment;
			}
			if (WHSDcKmyiONEpyVLUUdysqSpPhMv)
			{
				GQbLuHNElCwoaTeQMysXyDhZoSVt.font = dwrpHavMScdLoBdcAPzyGrooFHsOA;
			}
			else
			{
				dwrpHavMScdLoBdcAPzyGrooFHsOA = GQbLuHNElCwoaTeQMysXyDhZoSVt.font;
			}
			if (QeAjYaZOFneuzuMkMOtWfamljODk)
			{
				GQbLuHNElCwoaTeQMysXyDhZoSVt.fontSize = DeqAJbeCTjbnxOYqvXVlTzoyruHr;
			}
			else
			{
				DeqAJbeCTjbnxOYqvXVlTzoyruHr = GQbLuHNElCwoaTeQMysXyDhZoSVt.fontSize;
			}
			if (CGuFAlZUmFuhFRHhDaRYEvVdANnw)
			{
				GQbLuHNElCwoaTeQMysXyDhZoSVt.fontStyle = VvITnZpRrsHUYWGazgMpLpFpYZuG;
			}
			else
			{
				VvITnZpRrsHUYWGazgMpLpFpYZuG = GQbLuHNElCwoaTeQMysXyDhZoSVt.fontStyle;
			}
			if (YcAeScGpcuyYpoSZxdfYHPnjHtJyB)
			{
				GQbLuHNElCwoaTeQMysXyDhZoSVt.normal.textColor = LnqQdgvbMLuLxYcefdFcdwTHADxDA;
			}
			else
			{
				LnqQdgvbMLuLxYcefdFcdwTHADxDA = GQbLuHNElCwoaTeQMysXyDhZoSVt.normal.textColor;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static GUIText GetOrAddComponent(GameObject gameObject)
		{
			if (gameObject == null)
			{
				return null;
			}
			GUIText gUIText = gameObject.GetComponent<GUIText>();
			if (gUIText == null)
			{
				gUIText = gameObject.AddComponent<GUIText>();
			}
			return gUIText;
		}

		[CustomObfuscation(rename = false)]
		internal static GUIText CreateLogger(GameObject gameObject)
		{
			if (gameObject == null)
			{
				return null;
			}
			GUIText orAddComponent = GetOrAddComponent(gameObject);
			orAddComponent.anchor = TextAnchor.LowerLeft;
			return orAddComponent;
		}
	}
}
