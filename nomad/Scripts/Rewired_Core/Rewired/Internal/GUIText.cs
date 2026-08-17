using System.ComponentModel;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Internal
{
	[AddComponentMenu("")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class GUIText : MonoBehaviour
	{
		private string LeYuRCvpmfnYDJQSMHmfIOzpBdLJ;

		private GUIStyle GMMZPjHxTEgIEjSsdoWSTFORTqvT;

		private TextAnchor tcnJrydisIRhknvIhmcAeGMKslsL;

		private TextAlignment cDVcFpgbUQrSzhGIXOCWpvjdvCFI;

		private float AYQBVmeXPKFuXDVQSZayqiShzQNS;

		private Font csntfbmyxcEFGqQXUXAPLpJOhiLK;

		private int RBzFFAScuUmAMxIOJruzQgSQWlQt = -1;

		private FontStyle RkgKvGxMymskRjdtWPsjHpmHWuAm;

		private Color IZkGfAEsaNxzDSbWFDAYnHofHuSR = Color.white;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Vector2 _pixelOffset;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _useUnityUI;

		private bool AcoEkmBEtMPXfcsjJWhszBMQSdiaB;

		private bool ksRFWyfbKsoYabytLqCyXlTAEGCg;

		private bool ISpjQkBBDmASRcglThceCGJNVRmo;

		private bool XewacZCLUHPKfrFxTEsIpwPUuTIL;

		private bool rwbwESsARNeedrQPAGFRMoMjsmPT;

		private bool xDMyIFYXkuXBZQeiNrvIFydSYAfI;

		private bool stjhyBBAjnPqElTiECIcZSczSHhq;

		private Text JVycMVhvBCkBMSZzYxJHFQLpRmUr;

		private bool ZKEetHFFzPgyvUFumHbNuMzmJnyV;

		private bool bXdEnppTyETkQPmoqTErMahoJJJn;

		public string text
		{
			get
			{
				return LeYuRCvpmfnYDJQSMHmfIOzpBdLJ;
			}
			set
			{
				LeYuRCvpmfnYDJQSMHmfIOzpBdLJ = value;
			}
		}

		public TextAnchor anchor
		{
			get
			{
				return tcnJrydisIRhknvIhmcAeGMKslsL;
			}
			set
			{
				tcnJrydisIRhknvIhmcAeGMKslsL = value;
				AcoEkmBEtMPXfcsjJWhszBMQSdiaB = true;
				if (GMMZPjHxTEgIEjSsdoWSTFORTqvT != null)
				{
					GMMZPjHxTEgIEjSsdoWSTFORTqvT.alignment = value;
				}
			}
		}

		public TextAlignment alignment
		{
			get
			{
				return cDVcFpgbUQrSzhGIXOCWpvjdvCFI;
			}
			set
			{
				cDVcFpgbUQrSzhGIXOCWpvjdvCFI = value;
				ksRFWyfbKsoYabytLqCyXlTAEGCg = true;
			}
		}

		public float lineSpacing
		{
			get
			{
				return AYQBVmeXPKFuXDVQSZayqiShzQNS;
			}
			set
			{
				AYQBVmeXPKFuXDVQSZayqiShzQNS = value;
				ISpjQkBBDmASRcglThceCGJNVRmo = true;
				_ = GMMZPjHxTEgIEjSsdoWSTFORTqvT;
			}
		}

		public Font font
		{
			get
			{
				return csntfbmyxcEFGqQXUXAPLpJOhiLK;
			}
			set
			{
				XewacZCLUHPKfrFxTEsIpwPUuTIL = true;
				csntfbmyxcEFGqQXUXAPLpJOhiLK = value;
				if (GMMZPjHxTEgIEjSsdoWSTFORTqvT != null)
				{
					GMMZPjHxTEgIEjSsdoWSTFORTqvT.font = value;
				}
			}
		}

		public int fontSize
		{
			get
			{
				return RBzFFAScuUmAMxIOJruzQgSQWlQt;
			}
			set
			{
				RBzFFAScuUmAMxIOJruzQgSQWlQt = value;
				rwbwESsARNeedrQPAGFRMoMjsmPT = true;
				if (GMMZPjHxTEgIEjSsdoWSTFORTqvT != null)
				{
					GMMZPjHxTEgIEjSsdoWSTFORTqvT.fontSize = value;
				}
			}
		}

		public FontStyle fontStyle
		{
			get
			{
				return RkgKvGxMymskRjdtWPsjHpmHWuAm;
			}
			set
			{
				RkgKvGxMymskRjdtWPsjHpmHWuAm = value;
				xDMyIFYXkuXBZQeiNrvIFydSYAfI = true;
				if (GMMZPjHxTEgIEjSsdoWSTFORTqvT != null)
				{
					GMMZPjHxTEgIEjSsdoWSTFORTqvT.fontStyle = value;
				}
			}
		}

		public Color color
		{
			get
			{
				return IZkGfAEsaNxzDSbWFDAYnHofHuSR;
			}
			set
			{
				IZkGfAEsaNxzDSbWFDAYnHofHuSR = value;
				stjhyBBAjnPqElTiECIcZSczSHhq = true;
				if (GMMZPjHxTEgIEjSsdoWSTFORTqvT != null)
				{
					GMMZPjHxTEgIEjSsdoWSTFORTqvT.normal.textColor = value;
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
					ZKEetHFFzPgyvUFumHbNuMzmJnyV = value;
					if (value)
					{
						tuAsfHKuVZpBcLzhEFYioOJEWBUy();
					}
					else
					{
						wkMVwnPMPGeCKgohULHQPYEaDpCu();
					}
				}
			}
		}

		[CustomObfuscation(rename = false)]
		private void Awake()
		{
			bXdEnppTyETkQPmoqTErMahoJJJn = true;
		}

		[CustomObfuscation(rename = false)]
		private void Start()
		{
			ZKEetHFFzPgyvUFumHbNuMzmJnyV = _useUnityUI;
			if (_useUnityUI)
			{
				tuAsfHKuVZpBcLzhEFYioOJEWBUy();
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnGUI()
		{
			if (!_useUnityUI)
			{
				if (GMMZPjHxTEgIEjSsdoWSTFORTqvT == null)
				{
					pfoaUFmFMCBYsBGZTVhwTBiNfLsc();
				}
				if (!string.IsNullOrEmpty(LeYuRCvpmfnYDJQSMHmfIOzpBdLJ))
				{
					Vector2 vector = base.transform.localPosition;
					GUI.Label(new Rect(vector.x * (float)Screen.width + _pixelOffset.x, vector.y * (float)Screen.height + _pixelOffset.y, MathTools.Clamp((float)Screen.width - vector.x * (float)Screen.width, 0f, 3.4028235E+38f), MathTools.Clamp((float)Screen.height - vector.y * (float)Screen.height, 0f, 3.4028235E+38f)), LeYuRCvpmfnYDJQSMHmfIOzpBdLJ, GMMZPjHxTEgIEjSsdoWSTFORTqvT);
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
			if (JVycMVhvBCkBMSZzYxJHFQLpRmUr == null)
			{
				Logger.LogError("Text component has been deleted.");
				return;
			}
			RectTransform component = JVycMVhvBCkBMSZzYxJHFQLpRmUr.GetComponent<RectTransform>();
			if (component.anchoredPosition != _pixelOffset)
			{
				component.anchoredPosition = _pixelOffset;
			}
			JVycMVhvBCkBMSZzYxJHFQLpRmUr.text = LeYuRCvpmfnYDJQSMHmfIOzpBdLJ;
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			if (bXdEnppTyETkQPmoqTErMahoJJJn && _useUnityUI != ZKEetHFFzPgyvUFumHbNuMzmJnyV)
			{
				ZKEetHFFzPgyvUFumHbNuMzmJnyV = _useUnityUI;
				if (_useUnityUI)
				{
					tuAsfHKuVZpBcLzhEFYioOJEWBUy();
				}
				else
				{
					wkMVwnPMPGeCKgohULHQPYEaDpCu();
				}
			}
		}

		private void tuAsfHKuVZpBcLzhEFYioOJEWBUy()
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
			JVycMVhvBCkBMSZzYxJHFQLpRmUr = GetComponent<Text>();
			if (!(JVycMVhvBCkBMSZzYxJHFQLpRmUr == null))
			{
				return;
			}
			RectTransform rectTransform = base.gameObject.AddComponent<RectTransform>();
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.localPosition = Vector2.zero;
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = Vector3.zero;
			JVycMVhvBCkBMSZzYxJHFQLpRmUr = base.gameObject.AddComponent<Text>();
			JVycMVhvBCkBMSZzYxJHFQLpRmUr.color = Color.white;
			if (_useUnityUI)
			{
				try
				{
					JVycMVhvBCkBMSZzYxJHFQLpRmUr.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
				catch
				{
					try
					{
						JVycMVhvBCkBMSZzYxJHFQLpRmUr.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
					}
					catch
					{
						Logger.LogError("No default font found for GUIText.");
					}
				}
			}
			JVycMVhvBCkBMSZzYxJHFQLpRmUr.fontSize = 13;
			if (AcoEkmBEtMPXfcsjJWhszBMQSdiaB)
			{
				JVycMVhvBCkBMSZzYxJHFQLpRmUr.alignment = tcnJrydisIRhknvIhmcAeGMKslsL;
			}
			else
			{
				tcnJrydisIRhknvIhmcAeGMKslsL = JVycMVhvBCkBMSZzYxJHFQLpRmUr.alignment;
			}
			if (XewacZCLUHPKfrFxTEsIpwPUuTIL)
			{
				JVycMVhvBCkBMSZzYxJHFQLpRmUr.font = csntfbmyxcEFGqQXUXAPLpJOhiLK;
			}
			else
			{
				csntfbmyxcEFGqQXUXAPLpJOhiLK = JVycMVhvBCkBMSZzYxJHFQLpRmUr.font;
			}
			if (rwbwESsARNeedrQPAGFRMoMjsmPT)
			{
				JVycMVhvBCkBMSZzYxJHFQLpRmUr.fontSize = RBzFFAScuUmAMxIOJruzQgSQWlQt;
			}
			else
			{
				RBzFFAScuUmAMxIOJruzQgSQWlQt = JVycMVhvBCkBMSZzYxJHFQLpRmUr.fontSize;
			}
			if (xDMyIFYXkuXBZQeiNrvIFydSYAfI)
			{
				JVycMVhvBCkBMSZzYxJHFQLpRmUr.fontStyle = RkgKvGxMymskRjdtWPsjHpmHWuAm;
			}
			else
			{
				RkgKvGxMymskRjdtWPsjHpmHWuAm = JVycMVhvBCkBMSZzYxJHFQLpRmUr.fontStyle;
			}
			if (stjhyBBAjnPqElTiECIcZSczSHhq)
			{
				JVycMVhvBCkBMSZzYxJHFQLpRmUr.color = IZkGfAEsaNxzDSbWFDAYnHofHuSR;
			}
			else
			{
				IZkGfAEsaNxzDSbWFDAYnHofHuSR = JVycMVhvBCkBMSZzYxJHFQLpRmUr.color;
			}
		}

		private void wkMVwnPMPGeCKgohULHQPYEaDpCu()
		{
			if (Application.isPlaying)
			{
				if (JVycMVhvBCkBMSZzYxJHFQLpRmUr != null)
				{
					JVycMVhvBCkBMSZzYxJHFQLpRmUr.text = string.Empty;
				}
				JVycMVhvBCkBMSZzYxJHFQLpRmUr = null;
			}
		}

		private void pfoaUFmFMCBYsBGZTVhwTBiNfLsc()
		{
			GMMZPjHxTEgIEjSsdoWSTFORTqvT = new GUIStyle(GUI.skin.label);
			if (AcoEkmBEtMPXfcsjJWhszBMQSdiaB)
			{
				GMMZPjHxTEgIEjSsdoWSTFORTqvT.alignment = tcnJrydisIRhknvIhmcAeGMKslsL;
			}
			else
			{
				tcnJrydisIRhknvIhmcAeGMKslsL = GMMZPjHxTEgIEjSsdoWSTFORTqvT.alignment;
			}
			if (XewacZCLUHPKfrFxTEsIpwPUuTIL)
			{
				GMMZPjHxTEgIEjSsdoWSTFORTqvT.font = csntfbmyxcEFGqQXUXAPLpJOhiLK;
			}
			else
			{
				csntfbmyxcEFGqQXUXAPLpJOhiLK = GMMZPjHxTEgIEjSsdoWSTFORTqvT.font;
			}
			if (rwbwESsARNeedrQPAGFRMoMjsmPT)
			{
				GMMZPjHxTEgIEjSsdoWSTFORTqvT.fontSize = RBzFFAScuUmAMxIOJruzQgSQWlQt;
			}
			else
			{
				RBzFFAScuUmAMxIOJruzQgSQWlQt = GMMZPjHxTEgIEjSsdoWSTFORTqvT.fontSize;
			}
			if (xDMyIFYXkuXBZQeiNrvIFydSYAfI)
			{
				GMMZPjHxTEgIEjSsdoWSTFORTqvT.fontStyle = RkgKvGxMymskRjdtWPsjHpmHWuAm;
			}
			else
			{
				RkgKvGxMymskRjdtWPsjHpmHWuAm = GMMZPjHxTEgIEjSsdoWSTFORTqvT.fontStyle;
			}
			if (stjhyBBAjnPqElTiECIcZSczSHhq)
			{
				GMMZPjHxTEgIEjSsdoWSTFORTqvT.normal.textColor = IZkGfAEsaNxzDSbWFDAYnHofHuSR;
			}
			else
			{
				IZkGfAEsaNxzDSbWFDAYnHofHuSR = GMMZPjHxTEgIEjSsdoWSTFORTqvT.normal.textColor;
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
