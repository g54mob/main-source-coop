using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PTextLerp : MonoBehaviour
	{
		[Serializable]
		public class LerpValues
		{
			public float spring = 2f;

			public float drag = 0.98f;

			public float speed = 60f;

			public float haxx = 0.249f;

			public float min;

			public float max = 255f;

			public float iSpeedMul = 1f;
		}

		[Serializable]
		public class StringLerp
		{
			[SerializeField]
			public List<CharLerp> letters = new List<CharLerp>();

			public string GimmeString()
			{
				string text = null;
				foreach (CharLerp letter in letters)
				{
					text += letter.Current;
				}
				return text;
			}

			public IEnumerator Lerp()
			{
				float start = 1f;
				for (int i = 0; i < letters.Count; i++)
				{
					CharLerp item = letters[i];
					while (!item.Done)
					{
						item.PhysicsLerp();
						item.iSpeed = start;
						start += Time.deltaTime;
						yield return null;
					}
				}
			}
		}

		[Serializable]
		public class CharLerp
		{
			public char goal;

			public float velocity;

			public bool useTimeScale = true;

			public float iSpeed = 1f;

			public bool print;

			private float curInFloat;

			private char current;

			private LerpValues values;

			public bool Done
			{
				get
				{
					if (Mathf.Abs(velocity) < 0.001f)
					{
						return goal == Current;
					}
					return false;
				}
			}

			public char Current
			{
				get
				{
					return (char)(int)ExtMath.PLoop(curInFloat, values.min, values.max);
				}
				set
				{
					current = value;
					curInFloat = (int)current;
				}
			}

			public CharLerp(char g, char cur, LerpValues lerpVals)
			{
				Current = cur;
				goal = g;
				values = lerpVals;
			}

			public void PhysicsLerp()
			{
				if ((double)(Time.realtimeSinceStartup + (float)UnityEngine.Random.Range(0, 10)) % 0.25 < (double)values.haxx)
				{
					return;
				}
				for (int i = 0; (float)i < iSpeed * values.iSpeedMul; i++)
				{
					float num = Mathf.Clamp(useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime, 0f, 0.02f);
					num *= values.speed;
					velocity += ((float)(int)goal - curInFloat) * num * values.spring;
					velocity -= values.drag * velocity * num;
					curInFloat += velocity * num;
					Debug.Log("velocity " + velocity);
					Debug.Log("val " + curInFloat);
					if (Done)
					{
						break;
					}
				}
			}
		}

		public LerpValues lerpVals;

		public StringLerp stringLerp;

		private TMP_InputField tmpInput;

		private TextMeshProUGUI tmpText;

		private void Awake()
		{
			tmpText = GetComponent<TextMeshProUGUI>();
			tmpInput = base.transform.parent.GetComponentInChildren<TMP_InputField>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (stringLerp != null && stringLerp.letters != null && stringLerp.letters.Count >= 1)
			{
				string text = stringLerp.GimmeString();
				string text2 = "";
				string text3 = text;
				for (int i = 0; i < text3.Length; i++)
				{
					char character = text3[i];
					text2 = (tmpText.font.HasCharacter(character) ? (text2 + character) : (text2 + " "));
				}
				if (text2 != null)
				{
					tmpText.text = text2;
				}
			}
		}

		public void ValChanged()
		{
			Lerp(tmpInput.text);
		}

		private void Lerp(string newText)
		{
			stringLerp = new StringLerp();
			if (newText.Length > tmpText.text.Length)
			{
				for (int i = 0; i < newText.Length; i++)
				{
					char g = newText[i];
					char cur = ((i <= tmpText.text.Length - 1) ? tmpText.text[i] : ' ');
					stringLerp.letters.Add(new CharLerp(g, cur, lerpVals));
				}
			}
			else
			{
				for (int j = 0; j < tmpText.text.Length; j++)
				{
					char cur2 = tmpText.text[j];
					char g2 = ((j <= newText.Length - 1) ? newText[j] : ' ');
					stringLerp.letters.Add(new CharLerp(g2, cur2, lerpVals));
				}
			}
			stringLerp.letters[0].print = true;
			StartCoroutine(stringLerp.Lerp());
		}
	}
}
