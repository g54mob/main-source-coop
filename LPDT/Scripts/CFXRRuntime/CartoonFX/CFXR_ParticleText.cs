using System;
using UnityEngine;

namespace CartoonFX
{
	[RequireComponent(typeof(ParticleSystem))]
	public class CFXR_ParticleText : MonoBehaviour
	{
		[Header("Dynamic")]
		[Tooltip("Allow changing the text at runtime with the 'UpdateText' method. If disabled, this script will be excluded from the build.")]
		public bool isDynamic;

		[Header("Text")]
		[SerializeField]
		private string text;

		[SerializeField]
		private float size = 1f;

		[SerializeField]
		private float letterSpacing = 0.44f;

		[Header("Colors")]
		[SerializeField]
		private Color backgroundColor = new Color(0f, 0f, 0f, 1f);

		[SerializeField]
		private Color color1 = new Color(1f, 1f, 1f, 1f);

		[SerializeField]
		private Color color2 = new Color(0f, 0f, 1f, 1f);

		[Header("Delay")]
		[SerializeField]
		private float delay = 0.05f;

		[SerializeField]
		private bool cumulativeDelay;

		[Range(0f, 2f)]
		[SerializeField]
		private float compensateLifetime;

		[Header("Misc")]
		[SerializeField]
		private float lifetimeMultiplier = 1f;

		[Range(-90f, 90f)]
		[SerializeField]
		private float rotation = -5f;

		[SerializeField]
		private float sortingFudgeOffset = 0.1f;

		[SerializeField]
		private CFXR_ParticleTextFontAsset font;

		private float baseLifetime;

		private float baseScaleX;

		private float baseScaleY;

		private float baseScaleZ;

		private Vector3 basePivot;

		private void Awake()
		{
			if (!isDynamic)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				InitializeFirstParticle();
			}
		}

		private void InitializeFirstParticle()
		{
			if (isDynamic && base.transform.childCount == 0)
			{
				throw new Exception("[CFXR_ParticleText] A disabled GameObject with a ParticleSystem component is required as the first child when 'isDyanmic' is enabled, so that its settings can be used as a base for the generated characters.");
			}
			ParticleSystem particleSystem = (isDynamic ? base.transform.GetChild(0).GetComponent<ParticleSystem>() : GetComponent<ParticleSystem>());
			ParticleSystem.MainModule main = particleSystem.main;
			baseLifetime = main.startLifetime.constant;
			baseScaleX = main.startSizeXMultiplier;
			baseScaleY = main.startSizeYMultiplier;
			baseScaleZ = main.startSizeZMultiplier;
			basePivot = particleSystem.GetComponent<ParticleSystemRenderer>().pivot;
			if (isDynamic)
			{
				basePivot.x = 0f;
				particleSystem.gameObject.SetActive(value: false);
				particleSystem.gameObject.name = "MODEL";
			}
		}

		public void UpdateText(string newText = null, float? newSize = null, Color? newColor1 = null, Color? newColor2 = null, Color? newBackgroundColor = null, float? newLifetimeMultiplier = null)
		{
			if (Application.isPlaying && !isDynamic)
			{
				throw new Exception("[CFXR_ParticleText] You cannot update the text at runtime if it's not marked as dynamic.");
			}
			if (newText != null)
			{
				switch (font.letterCase)
				{
				case CFXR_ParticleTextFontAsset.LetterCase.Lower:
					newText = newText.ToLowerInvariant();
					break;
				case CFXR_ParticleTextFontAsset.LetterCase.Upper:
					newText = newText.ToUpperInvariant();
					break;
				}
				string text = newText;
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					if (!char.IsWhiteSpace(c) && font.CharSequence.IndexOf(c) < 0)
					{
						throw new Exception("[CFXR_ParticleText] Invalid character supplied for the dynamic text: '" + c + "'\nThe allowed characters from the selected font are: " + font.CharSequence);
					}
				}
				this.text = newText;
			}
			if (newSize.HasValue)
			{
				size = newSize.Value;
			}
			if (newColor1.HasValue)
			{
				color1 = newColor1.Value;
			}
			if (newColor2.HasValue)
			{
				color2 = newColor2.Value;
			}
			if (newBackgroundColor.HasValue)
			{
				backgroundColor = newBackgroundColor.Value;
			}
			if (newLifetimeMultiplier.HasValue)
			{
				lifetimeMultiplier = newLifetimeMultiplier.Value;
			}
			if (this.text == null || font == null || !font.IsValid())
			{
				return;
			}
			if (base.transform.childCount == 0)
			{
				throw new Exception("[CFXR_ParticleText] A disabled GameObject with a ParticleSystem component is required as the first child when 'isDyanmic' is enabled, so that its settings can be used as a base for the generated characters.");
			}
			float num = 0f;
			int num2 = 0;
			for (int j = 0; j < this.text.Length; j++)
			{
				if (char.IsWhiteSpace(this.text[j]))
				{
					if (j > 0)
					{
						num += letterSpacing * size;
					}
					continue;
				}
				num2++;
				if (j > 0)
				{
					int num3 = font.CharSequence.IndexOf(this.text[j]);
					float num4 = font.CharSprites[num3].rect.width + font.CharKerningOffsets[num3].post + font.CharKerningOffsets[num3].pre;
					num += (num4 * 0.01f + letterSpacing) * size;
				}
			}
			if (num2 > 0)
			{
				int num5 = base.transform.childCount - (isDynamic ? 1 : 0);
				if (num5 < num2)
				{
					GameObject original = (isDynamic ? base.transform.GetChild(0).gameObject : null);
					for (int k = num5; k < num2; k++)
					{
						GameObject gameObject = (isDynamic ? UnityEngine.Object.Instantiate(original, base.transform) : new GameObject());
						if (!isDynamic)
						{
							gameObject.transform.SetParent(base.transform);
							gameObject.AddComponent<ParticleSystem>();
						}
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
					}
				}
				float num6 = num / 2f;
				num = 0f;
				int num7 = ((!isDynamic) ? (-1) : 0);
				if (!isDynamic)
				{
					GetComponent<ParticleSystem>();
				}
				GetComponent<ParticleSystemRenderer>();
				for (int l = 0; l < this.text.Length; l++)
				{
					char c2 = this.text[l];
					if (char.IsWhiteSpace(c2))
					{
						num += letterSpacing * size;
						continue;
					}
					num7++;
					int num8 = font.CharSequence.IndexOf(this.text[l]);
					Sprite sprite = font.CharSprites[num8];
					float num9 = size * sprite.rect.width / 50f;
					num += font.CharKerningOffsets[num8].pre * 0.01f * size;
					float num10 = (num - num6) / num9;
					float num11 = sprite.rect.width + font.CharKerningOffsets[num8].post;
					num += (num11 * 0.01f + letterSpacing) * size;
					GameObject obj = base.transform.GetChild(num7).gameObject;
					obj.name = c2.ToString();
					ParticleSystem component = obj.GetComponent<ParticleSystem>();
					ParticleSystem.MainModule main = component.main;
					main.startSizeXMultiplier = baseScaleX * num9;
					main.startSizeYMultiplier = baseScaleY * num9;
					main.startSizeZMultiplier = baseScaleZ * num9;
					component.textureSheetAnimation.SetSprite(0, sprite);
					main.startRotation = MathF.PI / 180f * rotation;
					main.startColor = backgroundColor;
					ParticleSystem.CustomDataModule customData = component.customData;
					customData.enabled = true;
					customData.SetColor(ParticleSystemCustomData.Custom1, color1);
					customData.SetColor(ParticleSystemCustomData.Custom2, color2);
					if (cumulativeDelay)
					{
						main.startDelay = delay * (float)l;
						main.startLifetime = Mathf.LerpUnclamped(baseLifetime, baseLifetime + delay * (float)(this.text.Length - l), compensateLifetime / lifetimeMultiplier);
					}
					else
					{
						main.startDelay = delay;
					}
					main.startLifetime = main.startLifetime.constant * lifetimeMultiplier;
					ParticleSystemRenderer component2 = component.GetComponent<ParticleSystemRenderer>();
					component2.enabled = true;
					component2.pivot = new Vector3(basePivot.x + num10, basePivot.y, basePivot.z);
					component2.sortingFudge += (float)l * sortingFudgeOffset;
				}
			}
			int m = 1;
			for (int childCount = base.transform.childCount; m < childCount; m++)
			{
				base.transform.GetChild(m).gameObject.SetActive(m <= num2);
			}
		}
	}
}
