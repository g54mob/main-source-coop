using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CurvedUI
{
	[ExecuteInEditMode]
	public class CurvedUITMPInputFieldCaret : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		private TMP_InputField myField;

		private RectTransform myCaret;

		private Color origCaretColor;

		private Color origSelectionColor;

		private bool selected;

		private bool selectingText;

		public Color CaretColor
		{
			get
			{
				return origCaretColor;
			}
			set
			{
				origCaretColor = value;
			}
		}

		public Color SelectionColor
		{
			get
			{
				return origSelectionColor;
			}
			set
			{
				origSelectionColor = value;
			}
		}

		public float CaretBlinkRate
		{
			get
			{
				return myField.caretBlinkRate;
			}
			set
			{
				myField.caretBlinkRate = value;
			}
		}

		private void Awake()
		{
			myField = GetComponent<TMP_InputField>();
			if ((bool)myField)
			{
				CheckAndConvertMask();
			}
		}

		private void Update()
		{
			if (selected)
			{
				UpdateCaret();
			}
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (myCaret == null)
			{
				CreateCaret();
			}
			selected = true;
			myCaret.gameObject.SetActive(value: true);
			StartCoroutine(CaretBlinker());
		}

		public void OnDeselect(BaseEventData eventData)
		{
			selected = false;
			myCaret.gameObject.SetActive(value: false);
		}

		private IEnumerator CaretBlinker()
		{
			while (selected)
			{
				myCaret.gameObject.SetActive(selectingText || !myCaret.gameObject.activeSelf);
				yield return new WaitForSeconds(0.5f / myField.caretBlinkRate);
			}
		}

		private void CreateCaret()
		{
			GameObject gameObject = new GameObject("CurvedUI_TMPCaret");
			gameObject.AddComponent<RectTransform>();
			gameObject.AddComponent<Image>();
			gameObject.AddComponent<CurvedUIVertexEffect>();
			gameObject.transform.SetParent(base.transform.GetChild(0).GetChild(0));
			gameObject.transform.localScale = Vector3.one;
			(gameObject.transform as RectTransform).anchoredPosition3D = Vector3.zero;
			(gameObject.transform as RectTransform).pivot = new Vector2(0f, 1f);
			gameObject.GetComponent<Image>().color = myField.caretColor;
			myCaret = gameObject.transform as RectTransform;
			gameObject.transform.SetAsFirstSibling();
			myField.customCaretColor = true;
			origCaretColor = myField.caretColor;
			myField.caretColor = new Color(0f, 0f, 0f, 0f);
			origSelectionColor = myField.selectionColor;
			myField.selectionColor = new Color(0f, 0f, 0f, 0f);
			gameObject.gameObject.SetActive(value: false);
		}

		private void UpdateCaret()
		{
			if (myCaret == null)
			{
				CreateCaret();
			}
			Vector2 localPositionInText = GetLocalPositionInText(myField.caretPosition);
			if (myField.selectionFocusPosition != myField.selectionAnchorPosition)
			{
				selectingText = true;
				Vector2 vector = new Vector2(GetLocalPositionInText(myField.selectionAnchorPosition).x - GetLocalPositionInText(myField.selectionFocusPosition).x, GetLocalPositionInText(myField.selectionAnchorPosition).y - GetLocalPositionInText(myField.selectionFocusPosition).y);
				localPositionInText = ((vector.x < 0f) ? GetLocalPositionInText(myField.selectionAnchorPosition) : GetLocalPositionInText(myField.selectionFocusPosition));
				vector = new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y) + myField.textComponent.fontSize);
				myCaret.sizeDelta = new Vector2(vector.x, vector.y);
				myCaret.anchoredPosition = localPositionInText;
				myCaret.GetComponent<Image>().color = origSelectionColor;
			}
			else
			{
				selectingText = false;
				myCaret.sizeDelta = new Vector2(myField.caretWidth, myField.textComponent.fontSize);
				myCaret.anchoredPosition = localPositionInText;
				myCaret.GetComponent<Image>().color = origCaretColor;
			}
		}

		private Vector2 GetLocalPositionInText(int charNo)
		{
			if (myField.isFocused)
			{
				TMP_TextInfo textInfo = myField.textComponent.textInfo;
				if (charNo > textInfo.characterCount - 1)
				{
					charNo = textInfo.characterCount - 1;
				}
				TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[charNo];
				return new Vector2(tMP_CharacterInfo.topLeft.x, tMP_CharacterInfo.ascender);
			}
			return Vector2.zero;
		}

		private void CheckAndConvertMask()
		{
			foreach (Transform item in base.transform)
			{
				if (item.GetComponent<RectMask2D>() != null)
				{
					Object.DestroyImmediate(item.GetComponent<RectMask2D>());
					item.AddComponentIfMissing<Image>();
					item.AddComponentIfMissing<Mask>();
				}
			}
		}
	}
}
