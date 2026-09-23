using System;
using System.Collections.Generic;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class CreditsView : MonoBehaviour
	{
		[Serializable]
		public class OpeningLine
		{
			[Tooltip("UI tablosundaki anahtar. Metinde {0} varsa yerine aşağıdaki isim yazılır.")]
			public string key;

			[Tooltip("{0}'ın yerine geçen isim - çevrilmez. Örn. DevAdam.")]
			public string argument;
		}

		[Serializable]
		public class Section
		{
			[Tooltip("Başlığın UI tablosundaki anahtarı (Credits.Programming gibi). Kurum adı gibi çevrilmeyen bir başlık için boş bırakıp aşağıdaki metni doldur.")]
			public string headingKey;

			[Tooltip("Anahtar boşsa ya da tabloda yoksa görünen başlık. Kurum adları (DevAdam, Tape Corps) burada, çünkü çevrilmezler.")]
			public string headingText;

			[Tooltip("Her satıra bir isim. Çevrilmez.")]
			[TextArea(3, 30)]
			public string names;
		}

		[Header("Panel")]
		[Tooltip("Açılıp kapanan panel. Boşsa bu bileşenin kendi objesi.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Görünen alan - Rect Mask 2D taşıyan obje. Satırlar bunun altından girip üstünden çıkar. Boşsa Content'in parent'ı.")]
		[SerializeField]
		private RectTransform viewport;

		[Tooltip("Satırların eklendiği ve kaydırılan obje. Üzerinde Vertical Layout Group ve Content Size Fitter (Vertical Fit = Preferred Size) olmalı. Anchor ve pivot'unun Y'si açılışta 1'e (üst) çekilir - kaydırma hesabı üstten yapılıyor.")]
		[SerializeField]
		private RectTransform content;

		[SerializeField]
		private Button closeButton;

		[Header("Satır şablonları")]
		[Tooltip("Açılış satırlarının şablonu ('Bir DevAdam Oyunu'). Content'in içinde durabilir; açılışta gizlenir, her satır için kopyalanır.")]
		[SerializeField]
		private TMP_Text openingTemplate;

		[Tooltip("Bölüm başlığının şablonu ('Programlama').")]
		[SerializeField]
		private TMP_Text headingTemplate;

		[Tooltip("İsim satırının şablonu.")]
		[SerializeField]
		private TMP_Text nameTemplate;

		[Header("Akış")]
		[Tooltip("Kayma hızı, saniyede piksel (Canvas birimi).")]
		[SerializeField]
		[Min(1f)]
		private float scrollSpeed = 70f;

		[Tooltip("Açılış satırlarından sonraki boşluk.")]
		[SerializeField]
		[Min(0f)]
		private float spaceAfterOpening = 120f;

		[Tooltip("Bir bölümün son isminden sonraki boşluk.")]
		[SerializeField]
		[Min(0f)]
		private float spaceBetweenSections = 70f;

		[Tooltip("Başlık ile ilk isim arasındaki boşluk.")]
		[SerializeField]
		[Min(0f)]
		private float spaceAfterHeading = 8f;

		[Tooltip("Son satır ekrandan çıktıktan sonra baştan başlamadan önceki boşluk, piksel.")]
		[SerializeField]
		[Min(0f)]
		private float loopGap;

		[Header("İçerik")]
		[SerializeField]
		private List<OpeningLine> opening = new List<OpeningLine>
		{
			new OpeningLine
			{
				key = "Credits.AGameBy",
				argument = "DevAdam"
			},
			new OpeningLine
			{
				key = "Credits.InAssociationWith",
				argument = "Tape Corps"
			},
			new OpeningLine
			{
				key = "Credits.SupportedBy",
				argument = "GameDev.ist"
			}
		};

		[SerializeField]
		private List<Section> sections = new List<Section>
		{
			new Section
			{
				headingKey = "Credits.GameDesign",
				names = "Ahmet Erdoğan Akkulak"
			},
			new Section
			{
				headingKey = "Credits.Programming",
				names = "Ahmet Erdoğan Akkulak"
			},
			new Section
			{
				headingKey = "Credits.LevelDesign",
				names = "Asrın Sıla Şimşek\nNamık Türk"
			},
			new Section
			{
				headingKey = "Credits.UIDesign",
				names = "Ahmet Erdoğan Akkulak\nMir"
			},
			new Section
			{
				headingKey = "Credits.CommunityManager",
				names = "Egemen Akgüner"
			},
			new Section
			{
				headingKey = "Credits.VoiceActing",
				names = "Ahmet Erdoğan Akkulak\nBetül Sürekli"
			},
			new Section
			{
				headingKey = "Credits.Playtesters",
				names = "Tayfun Yılmaz\nEnes Kaya\nAhmet Eren Yıldız\nAhmet Şahin\nFerdiee Pekmetchsi\nUmut Ege Yalçın\nEfkan Alkım Sağır\nSelensu Ayata\nGüven Sarı\nUlaş Yurdakul\nEmir Akıncı"
			},
			new Section
			{
				headingText = "DevAdam",
				names = "Ahmet Erdoğan Akkulak\nNamık Türk\nEgemen Akgüner"
			},
			new Section
			{
				headingText = "Tape Corps",
				names = "Ahmet Erdoğan Akkulak\nArif Can Üçer\nAsrın Sıla Şimşek"
			},
			new Section
			{
				headingText = "GameDev.ist",
				names = "Sinan Akkol\nFurkan Faruk Akıncı\nEran Küçük\nÖzgür Erdönmez\nKerem Doğan Karakoç\nÖzgür Deveci\nAli İhsan Göçmez\nHazal Altuğ\nİsmail Seçkin\nBaran Deniz Uysal\nMehmet Ali Akkın\nBeril Özge Danacı\nSancar Altınocağı\nBerfin Ergin\nMuzaffer Selim Akdoğan\nYağmur Miray Keçe\nCem Bahadır\nCennet Gizmen"
			}
		};

		private readonly List<GameObject> spawned = new List<GameObject>();

		private float scroll;

		private bool builtForCurrentLanguage;

		private bool initialized;

		private GameObject Target
		{
			get
			{
				if (!(panel != null))
				{
					return base.gameObject;
				}
				return panel;
			}
		}

		private RectTransform Viewport
		{
			get
			{
				if (!(viewport != null))
				{
					if (!(content != null))
					{
						return null;
					}
					return content.parent as RectTransform;
				}
				return viewport;
			}
		}

		public bool IsOpen => Target.activeSelf;

		private float StartPosition => 0f - ((Viewport != null) ? Viewport.rect.height : 0f);

		private float EndPosition => ContentHeight + loopGap;

		private float ContentHeight => Mathf.Max(content.rect.height, LayoutUtility.GetPreferredHeight(content));

		private void Awake()
		{
			if (!initialized)
			{
				EnsureInitialized();
				Target.SetActive(value: false);
			}
		}

		private void EnsureInitialized()
		{
			if (!initialized)
			{
				initialized = true;
				if (closeButton != null)
				{
					closeButton.onClick.AddListener(Hide);
				}
				HideTemplate(openingTemplate);
				HideTemplate(headingTemplate);
				HideTemplate(nameTemplate);
				Loc.Changed += OnLanguageChanged;
			}
		}

		private void OnDestroy()
		{
			Loc.Changed -= OnLanguageChanged;
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private static void HideTemplate(TMP_Text template)
		{
			if (template != null)
			{
				template.gameObject.SetActive(value: false);
			}
		}

		public void Show()
		{
			if (content == null)
			{
				Debug.LogWarning("[CreditsView] Content atanmamış - jenerik gösterilemiyor.", this);
				return;
			}
			EnsureInitialized();
			Target.SetActive(value: true);
			GameMenuState.SetMenuOpen(this, open: true);
			if (!builtForCurrentLanguage)
			{
				Build();
			}
			scroll = StartPosition;
			Apply();
		}

		public void Hide()
		{
			Target.SetActive(value: false);
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private void OnLanguageChanged()
		{
			builtForCurrentLanguage = false;
			if (IsOpen)
			{
				Build();
				Apply();
			}
		}

		private void Update()
		{
			if (IsOpen && !(content == null))
			{
				if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
				{
					GameMenuState.RequestEscape(100, Hide);
				}
				scroll += scrollSpeed * Time.unscaledDeltaTime;
				if (scroll > EndPosition)
				{
					scroll = StartPosition;
				}
				Apply();
			}
		}

		private void Apply()
		{
			Vector2 anchoredPosition = content.anchoredPosition;
			anchoredPosition.y = scroll;
			content.anchoredPosition = anchoredPosition;
		}

		private void Build()
		{
			foreach (GameObject item in spawned)
			{
				if (item != null)
				{
					UnityEngine.Object.Destroy(item);
				}
			}
			spawned.Clear();
			content.anchorMin = new Vector2(content.anchorMin.x, 1f);
			content.anchorMax = new Vector2(content.anchorMax.x, 1f);
			content.pivot = new Vector2(content.pivot.x, 1f);
			foreach (OpeningLine item2 in opening)
			{
				if (item2 != null && !string.IsNullOrWhiteSpace(item2.key))
				{
					AddLine(openingTemplate, FormatOpening(item2));
				}
			}
			if (opening.Count > 0)
			{
				AddSpace(spaceAfterOpening);
			}
			for (int i = 0; i < sections.Count; i++)
			{
				Section section = sections[i];
				if (section == null)
				{
					continue;
				}
				string text = Heading(section);
				if (!string.IsNullOrWhiteSpace(text))
				{
					AddLine(headingTemplate, text);
					AddSpace(spaceAfterHeading);
				}
				foreach (string item3 in SplitNames(section.names))
				{
					AddLine(nameTemplate, item3);
				}
				if (i < sections.Count - 1)
				{
					AddSpace(spaceBetweenSections);
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			builtForCurrentLanguage = true;
		}

		private static string FormatOpening(OpeningLine line)
		{
			string text = line.argument ?? "";
			if (!Loc.TryGet(line.key, out var text2))
			{
				return text;
			}
			try
			{
				return string.Format(text2, text);
			}
			catch (FormatException)
			{
				return text2;
			}
		}

		private static string Heading(Section section)
		{
			if (!string.IsNullOrWhiteSpace(section.headingKey) && Loc.TryGet(section.headingKey, out var text))
			{
				return text;
			}
			return section.headingText;
		}

		private static IEnumerable<string> SplitNames(string names)
		{
			if (string.IsNullOrEmpty(names))
			{
				yield break;
			}
			string[] array = names.Split('\n');
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length > 0)
				{
					yield return text;
				}
			}
		}

		private void AddLine(TMP_Text template, string text)
		{
			if (!(template == null) && !string.IsNullOrEmpty(text))
			{
				TMP_Text tMP_Text = UnityEngine.Object.Instantiate(template, content);
				tMP_Text.gameObject.SetActive(value: true);
				tMP_Text.text = text;
				spawned.Add(tMP_Text.gameObject);
			}
		}

		private void AddSpace(float height)
		{
			if (!(height <= 0f))
			{
				GameObject gameObject = new GameObject("Space", typeof(RectTransform), typeof(LayoutElement));
				gameObject.transform.SetParent(content, worldPositionStays: false);
				LayoutElement component = gameObject.GetComponent<LayoutElement>();
				component.minHeight = height;
				component.preferredHeight = height;
				spawned.Add(gameObject);
			}
		}
	}
}
