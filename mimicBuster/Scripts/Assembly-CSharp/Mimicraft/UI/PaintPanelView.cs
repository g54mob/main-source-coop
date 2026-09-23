using System.Globalization;
using Mimicraft.VoxelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PaintPanelView : MonoBehaviour
	{
		private const float WheelSize = 140f;

		private static readonly Color ActiveColor = new Color(0.41960785f, 0.6f, 0.56078434f, 1f);

		private static readonly Color InactiveColor = new Color(0.7372549f, 0.8745098f, 72f / 85f, 1f);

		[SerializeField]
		private ColorWheelWidget wheel;

		[SerializeField]
		private Slider alphaSlider;

		[SerializeField]
		private TMP_InputField alphaField;

		[SerializeField]
		private Slider redSlider;

		[SerializeField]
		private TMP_InputField redField;

		[SerializeField]
		private Slider greenSlider;

		[SerializeField]
		private TMP_InputField greenField;

		[SerializeField]
		private Slider blueSlider;

		[SerializeField]
		private TMP_InputField blueField;

		[SerializeField]
		private TMP_InputField hexField;

		[SerializeField]
		private Slider valueSlider;

		[SerializeField]
		private TMP_InputField valueField;

		[SerializeField]
		private Slider hueSlider;

		[SerializeField]
		private TMP_InputField hueField;

		[SerializeField]
		private Slider saturationSlider;

		[SerializeField]
		private TMP_InputField saturationField;

		[Tooltip("Palet swatch'larının içine kurulacağı obje. Sen veriyorsun: kolon sayısı, hücre boyutu ve boşluklar senin bu objeye koyduğun GridLayoutGroup'tan geliyor, kod hiçbirine karışmıyor.\n\nBu objenin ALTI palete ait - her açılışta temizlenip yeniden doldurulur, yani içine elle bir şey koyma. Başlık gibi bir şey istiyorsan bu objenin YANINA koy.\n\nBoş bırakırsan panel eskisi gibi kendi kabını da kendi kurar.")]
		[SerializeField]
		private RectTransform paletteContainer;

		[Tooltip("Palet kaç kolon üretsin. 0 = kabın GridLayoutGroup'undan oku, ki normalde istediğin budur - sayı tek yerde durur.\n\nSadece kabın Constraint'i Fixed Column Count DEĞİLSE doldur: esnek bir grid genişliğe göre akar, yani swatch'lar kurulurken kaç kolon olacağı henüz belli değildir ve okunacak bir sayı yoktur.")]
		[SerializeField]
		[Min(0f)]
		private int paletteColumns;

		[SerializeField]
		private Image[] paletteSwatches;

		[SerializeField]
		private Image previewSwatch;

		[SerializeField]
		private Button brushModeButton;

		[SerializeField]
		private Image brushModeBackground;

		[SerializeField]
		private Image brushModeIcon;

		[SerializeField]
		private Button bucketModeButton;

		[SerializeField]
		private Image bucketModeBackground;

		[SerializeField]
		private Button patternModeButton;

		[SerializeField]
		private Image patternModeBackground;

		[SerializeField]
		private TextMeshProUGUI brushLabel;

		[SerializeField]
		private Slider brushRadiusSlider;

		[SerializeField]
		private TextMeshProUGUI thresholdLabel;

		[SerializeField]
		private Slider thresholdSlider;

		[SerializeField]
		private TMP_InputField brushRadiusField;

		[SerializeField]
		private TMP_InputField thresholdField;

		[SerializeField]
		private RecentColorsView recentColors;

		[SerializeField]
		private PatternPickerView patternPicker;

		[SerializeField]
		private Button eyedropperButton;

		[SerializeField]
		private Button modeSelectorButton;

		[SerializeField]
		private Button colorWheelButton;

		[SerializeField]
		private GameObject modeSelectionPanel;

		[SerializeField]
		private GameObject colorWheelContainer;

		[SerializeField]
		private GameObject colorPanel;

		[SerializeField]
		private GameObject patternPanel;

		[Header("Mode Sprites")]
		public Sprite brushSprite;

		public Sprite bucketSprite;

		public Sprite patternSprite;

		private VoxelEditorController boundController;

		private Color lastSyncedColor;

		private bool suppressCallback;

		private const float ChannelFieldWidth = 40f;

		private const float ChannelLabelWidth = 16f;

		private const float ChannelRowSpacing = 6f;

		private const int DefaultPaletteColumns = 11;

		private const float PaletteSwatchSize = 22f;

		private const float PaletteSpacing = 3f;

		private const int PaletteRowCount = 4;

		private static readonly Vector3[] HueAnchors = new Vector3[11]
		{
			new Vector3(0f, 0.78f, 0.84f),
			new Vector3(21.1f, 0.85f, 0.9f),
			new Vector3(31.7f, 0.82f, 0.94f),
			new Vector3(48.1f, 0.94f, 0.95f),
			new Vector3(67.1f, 0.77f, 0.82f),
			new Vector3(95.5f, 0.75f, 0.75f),
			new Vector3(145.4f, 0.77f, 0.8f),
			new Vector3(180f, 0.86f, 0.74f),
			new Vector3(204.1f, 0.76f, 0.86f),
			new Vector3(246.9f, 0.6f, 0.91f),
			new Vector3(321.7f, 0.78f, 0.84f)
		};

		private const float MutedSaturation = 0.81f;

		private const float MutedValue = 0.71f;

		private static readonly Color32[] EarthAnchors = new Color32[6]
		{
			new Color32(92, 34, 32, byte.MaxValue),
			new Color32(101, 67, 33, byte.MaxValue),
			new Color32(139, 90, 43, byte.MaxValue),
			new Color32(190, 140, 100, byte.MaxValue),
			new Color32(222, 184, 135, byte.MaxValue),
			new Color32(245, 222, 179, byte.MaxValue)
		};

		private static readonly Color32[] StoneAnchors = new Color32[5]
		{
			new Color32(58, 74, 58, byte.MaxValue),
			new Color32(40, 78, 78, byte.MaxValue),
			new Color32(84, 92, 100, byte.MaxValue),
			new Color32(112, 128, 144, byte.MaxValue),
			new Color32(62, 52, 96, byte.MaxValue)
		};

		private bool warnedAboutPaletteColumns;

		public static PaintPanelView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "PaintPanel");
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			ColorWheelWidget colorWheelWidget = ColorWheelWidget.Create(rectTransform, 140f);
			BuildHueSaturationControls(rectTransform, 140f, out var slider, out var slider2);
			Slider slider3 = UIFactory.CreateSlider(rectTransform, "ValueSlider", 0f, 1f, 1f);
			((RectTransform)slider3.transform).sizeDelta = new Vector2(140f, 20f);
			UIFactory.CreateLabel(rectTransform, "AlphaLabel", "Alpha", 12);
			Slider slider4 = UIFactory.CreateSlider(rectTransform, "AlphaSlider", 0f, 1f, 1f);
			((RectTransform)slider4.transform).sizeDelta = new Vector2(140f, 20f);
			BuildRgbHexControls(rectTransform, 140f, out var slider5, out var tMP_InputField, out var slider6, out var tMP_InputField2, out var slider7, out var tMP_InputField3, out var tMP_InputField4);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "ColorPreview");
			rectTransform2.sizeDelta = new Vector2(140f, 32f);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.sprite = PlaceholderIcons.CreateCheckerboard();
			image.type = Image.Type.Simple;
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform2, "Swatch");
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.offsetMin = Vector2.zero;
			rectTransform3.offsetMax = Vector2.zero;
			Image image2 = rectTransform3.gameObject.AddComponent<Image>();
			image2.color = Color.white;
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform, "EyedropperButton", "Eyedropper (Alt+Click)", out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(140f, 28f);
			RectTransform rectTransform4 = UIFactory.CreateRect(rectTransform, "ModeRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform4.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			ContentSizeFitter contentSizeFitter = rectTransform4.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Sprite iconSprite = PlaceholderIcons.CreateSquare(new Color(0.9f, 0.35f, 0.55f, 1f));
			Image iconImage;
			Button button2 = UIFactory.CreateIconButton(rectTransform4, "BrushModeButton", iconSprite, new Vector2(22f, 22f), out iconImage);
			((RectTransform)button2.transform).sizeDelta = new Vector2(48f, 40f);
			button2.gameObject.AddComponent<UITooltipTrigger>().Text = "Brush";
			Sprite iconSprite2 = PlaceholderIcons.CreateSquare(new Color(0.35f, 0.75f, 0.9f, 1f));
			Button button3 = UIFactory.CreateIconButton(rectTransform4, "BucketModeButton", iconSprite2, new Vector2(22f, 22f), out iconImage);
			((RectTransform)button3.transform).sizeDelta = new Vector2(48f, 40f);
			button3.gameObject.AddComponent<UITooltipTrigger>().Text = "Bucket";
			Sprite iconSprite3 = PlaceholderIcons.CreateSquare(new Color(0.6f, 0.5f, 0.3f, 1f));
			Button button4 = UIFactory.CreateIconButton(rectTransform4, "PatternModeButton", iconSprite3, new Vector2(22f, 22f), out iconImage);
			((RectTransform)button4.transform).sizeDelta = new Vector2(48f, 40f);
			button4.gameObject.AddComponent<UITooltipTrigger>().Text = "Pattern";
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "BrushLabel", "Brush Radius", 12);
			Slider slider8 = UIFactory.CreateSlider(rectTransform, "BrushRadiusSlider", 0f, 5f, 0f);
			((RectTransform)slider8.transform).sizeDelta = new Vector2(140f, 20f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, "ThresholdLabel", "Threshold", 12);
			Slider slider9 = UIFactory.CreateSlider(rectTransform, "ThresholdSlider", 0f, 1f, 0f);
			((RectTransform)slider9.transform).sizeDelta = new Vector2(140f, 20f);
			PatternPickerView patternPickerView = PatternPickerView.Create(rectTransform);
			RecentColorsView recentColorsView = RecentColorsView.Create(rectTransform);
			PaintPanelView paintPanelView = rectTransform.gameObject.AddComponent<PaintPanelView>();
			paintPanelView.wheel = colorWheelWidget;
			paintPanelView.valueSlider = slider3;
			paintPanelView.alphaSlider = slider4;
			paintPanelView.redSlider = slider5;
			paintPanelView.redField = tMP_InputField;
			paintPanelView.greenSlider = slider6;
			paintPanelView.greenField = tMP_InputField2;
			paintPanelView.blueSlider = slider7;
			paintPanelView.blueField = tMP_InputField3;
			paintPanelView.hexField = tMP_InputField4;
			paintPanelView.previewSwatch = image2;
			paintPanelView.eyedropperButton = button;
			paintPanelView.brushModeButton = button2;
			paintPanelView.brushModeBackground = button2.GetComponent<Image>();
			paintPanelView.bucketModeButton = button3;
			paintPanelView.bucketModeBackground = button3.GetComponent<Image>();
			paintPanelView.patternModeButton = button4;
			paintPanelView.patternModeBackground = button4.GetComponent<Image>();
			paintPanelView.brushLabel = textMeshProUGUI;
			paintPanelView.brushRadiusSlider = slider8;
			paintPanelView.thresholdLabel = textMeshProUGUI2;
			paintPanelView.thresholdSlider = slider9;
			paintPanelView.patternPicker = patternPickerView;
			paintPanelView.recentColors = recentColorsView;
			paintPanelView.hueSlider = slider;
			paintPanelView.saturationSlider = slider2;
			paintPanelView.paletteContainer = BuildPaletteContainer(rectTransform);
			return paintPanelView;
		}

		private static void BuildRgbHexControls(Transform parent, float totalWidth, out Slider redSlider, out TMP_InputField redField, out Slider greenSlider, out TMP_InputField greenField, out Slider blueSlider, out TMP_InputField blueField, out TMP_InputField hexField)
		{
			redSlider = BuildChannelRow(parent, totalWidth, "R", out redField);
			greenSlider = BuildChannelRow(parent, totalWidth, "G", out greenField);
			blueSlider = BuildChannelRow(parent, totalWidth, "B", out blueField);
			hexField = UIFactory.CreateInputField(parent, "HexField", "#RRGGBB");
			((RectTransform)hexField.transform).sizeDelta = new Vector2(totalWidth, 24f);
			hexField.characterLimit = 7;
		}

		private static Color32[] BuildPaletteColors(int columns)
		{
			columns = Mathf.Max(1, columns);
			Color32[] array = new Color32[columns * 4];
			for (int i = 0; i < columns; i++)
			{
				float num = ((columns == 1) ? 0f : ((float)i / (float)(columns - 1)));
				byte b = (byte)Mathf.RoundToInt(num * 255f);
				array[i] = new Color32(b, b, b, byte.MaxValue);
				Vector3 vector = SampleHueAnchor(num);
				array[columns + i] = FromHsv(vector.x, vector.y, vector.z);
				array[columns * 2 + i] = FromHsv(vector.x, vector.y * 0.81f, vector.z * 0.71f);
			}
			FillEarthRow(array, columns * 3, columns);
			return array;
		}

		private static void FillEarthRow(Color32[] colors, int offset, int columns)
		{
			int num = EarthAnchors.Length + StoneAnchors.Length;
			int value = Mathf.RoundToInt((float)(columns * EarthAnchors.Length) / (float)num);
			value = ((columns > 1) ? Mathf.Clamp(value, 1, columns - 1) : columns);
			for (int i = 0; i < value; i++)
			{
				colors[offset + i] = SampleRamp(EarthAnchors, Fraction(i, value));
			}
			int num2 = columns - value;
			for (int j = 0; j < num2; j++)
			{
				colors[offset + value + j] = SampleRamp(StoneAnchors, Fraction(j, num2));
			}
		}

		private static float Fraction(int index, int count)
		{
			if (count > 1)
			{
				return (float)index / (float)(count - 1);
			}
			return 0f;
		}

		private static Vector3 SampleHueAnchor(float t)
		{
			float num = t * (float)(HueAnchors.Length - 1);
			int num2 = Mathf.Min((int)num, HueAnchors.Length - 2);
			return Vector3.Lerp(HueAnchors[num2], HueAnchors[num2 + 1], num - (float)num2);
		}

		private static Color32 SampleRamp(Color32[] anchors, float t)
		{
			float num = t * (float)(anchors.Length - 1);
			int num2 = Mathf.Min((int)num, anchors.Length - 2);
			float num3 = num - (float)num2;
			Color32 color = anchors[num2];
			Color32 color2 = anchors[num2 + 1];
			return new Color32((byte)Mathf.RoundToInt((float)(int)color.r + (float)(color2.r - color.r) * num3), (byte)Mathf.RoundToInt((float)(int)color.g + (float)(color2.g - color.g) * num3), (byte)Mathf.RoundToInt((float)(int)color.b + (float)(color2.b - color.b) * num3), byte.MaxValue);
		}

		private static Color32 FromHsv(float hueDegrees, float saturation, float value)
		{
			Color color = Color.HSVToRGB(Mathf.Repeat(hueDegrees, 360f) / 360f, Mathf.Clamp01(saturation), Mathf.Clamp01(value));
			return new Color32((byte)Mathf.RoundToInt(color.r * 255f), (byte)Mathf.RoundToInt(color.g * 255f), (byte)Mathf.RoundToInt(color.b * 255f), byte.MaxValue);
		}

		private static Slider BuildChannelRow(Transform parent, float totalWidth, string label, out TMP_InputField field)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, label + "Row");
			rectTransform.sizeDelta = new Vector2(totalWidth, 24f);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 6f;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = true;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			((RectTransform)UIFactory.CreateLabel(rectTransform, label + "Label", label, 12).transform).sizeDelta = new Vector2(16f, 20f);
			float x = totalWidth - 16f - 40f - 12f;
			Slider slider = UIFactory.CreateSlider(rectTransform, label + "Slider", 0f, 255f, 0f);
			slider.wholeNumbers = true;
			((RectTransform)slider.transform).sizeDelta = new Vector2(x, 20f);
			field = UIFactory.CreateInputField(rectTransform, label + "Field", "0");
			((RectTransform)field.transform).sizeDelta = new Vector2(40f, 24f);
			field.contentType = TMP_InputField.ContentType.IntegerNumber;
			field.characterLimit = 3;
			return slider;
		}

		private static void BuildHueSaturationControls(Transform parent, float totalWidth, out Slider hueSlider, out Slider saturationSlider)
		{
			UIFactory.CreateLabel(parent, "HueLabel", "Hue", 12);
			hueSlider = UIFactory.CreateSlider(parent, "HueSlider", 0f, 1f, 0f);
			((RectTransform)hueSlider.transform).sizeDelta = new Vector2(totalWidth, 20f);
			UIFactory.CreateLabel(parent, "SaturationLabel", "Saturation", 12);
			saturationSlider = UIFactory.CreateSlider(parent, "SaturationSlider", 0f, 1f, 1f);
			((RectTransform)saturationSlider.transform).sizeDelta = new Vector2(totalWidth, 20f);
		}

		private static RectTransform BuildPaletteContainer(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "ColorPalette");
			GridLayoutGroup gridLayoutGroup = rectTransform.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(22f, 22f);
			gridLayoutGroup.spacing = new Vector2(3f, 3f);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 11;
			ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			return rectTransform;
		}

		private Image[] FillPalette(RectTransform container)
		{
			for (int num = container.childCount - 1; num >= 0; num--)
			{
				GameObject obj = container.GetChild(num).gameObject;
				obj.SetActive(value: false);
				Object.Destroy(obj);
			}
			Color32[] array = BuildPaletteColors(ResolveColumns(container));
			Image[] array2 = new Image[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Image image = UIFactory.CreatePanel(container, $"PaletteSwatch{i}", array[i]);
				image.gameObject.AddComponent<Button>().targetGraphic = image;
				array2[i] = image;
			}
			return array2;
		}

		private int ResolveColumns(RectTransform container)
		{
			if (paletteColumns > 0)
			{
				return paletteColumns;
			}
			if (container.TryGetComponent<GridLayoutGroup>(out var component) && component.constraint == GridLayoutGroup.Constraint.FixedColumnCount && component.constraintCount > 0)
			{
				return component.constraintCount;
			}
			if (!warnedAboutPaletteColumns)
			{
				warnedAboutPaletteColumns = true;
				Debug.LogWarning("[PaintPanelView] Palet kabında kolon sayısını okuyabileceğim bir GridLayoutGroup yok - Constraint'i Fixed Column Count yap, ya da bu bileşendeki " + $"Palette Columns alanına sayıyı yaz. Şimdilik {11} kolon " + "üretiliyor.", container);
			}
			return 11;
		}

		private void EnsureRgbHexControls()
		{
			if (!(redSlider != null))
			{
				BuildRgbHexControls(base.transform, 140f, out redSlider, out redField, out greenSlider, out greenField, out blueSlider, out blueField, out hexField);
				int num = ((alphaSlider != null) ? (alphaSlider.transform.GetSiblingIndex() + 1) : base.transform.childCount);
				redSlider.transform.parent.SetSiblingIndex(num);
				greenSlider.transform.parent.SetSiblingIndex(num + 1);
				blueSlider.transform.parent.SetSiblingIndex(num + 2);
				hexField.transform.SetSiblingIndex(num + 3);
			}
		}

		private void EnsureHueSaturationControls()
		{
			if (!(hueSlider != null))
			{
				BuildHueSaturationControls(base.transform, 140f, out hueSlider, out saturationSlider);
				int num = ((valueSlider != null) ? valueSlider.transform.GetSiblingIndex() : base.transform.childCount);
				hueSlider.transform.parent.SetSiblingIndex(num);
				hueSlider.transform.SetSiblingIndex(num + 1);
				saturationSlider.transform.parent.SetSiblingIndex(num + 2);
				saturationSlider.transform.SetSiblingIndex(num + 3);
			}
		}

		private void EnsurePalette()
		{
			if (paletteContainer != null)
			{
				paletteSwatches = FillPalette(paletteContainer);
			}
			else if (paletteSwatches == null || paletteSwatches.Length == 0)
			{
				if (colorPanel == null)
				{
					Debug.LogWarning("[PaintPanelView] Palet kurulamıyor - ne Palette Container ne de Color Panel bağlı. Paletin duracağı objeyi Palette Container alanına ver.", this);
					return;
				}
				paletteContainer = BuildPaletteContainer(colorPanel.transform);
				paletteSwatches = FillPalette(paletteContainer);
				int siblingIndex = ((recentColors != null) ? (recentColors.transform.GetSiblingIndex() + 1) : base.transform.childCount);
				paletteContainer.SetSiblingIndex(siblingIndex);
			}
		}

		private void WireHueSaturation()
		{
			if (hueSlider == null || saturationSlider == null)
			{
				return;
			}
			hueSlider.onValueChanged.RemoveAllListeners();
			hueSlider.onValueChanged.AddListener(delegate(float v)
			{
				SetFloatFieldText(hueField, v);
				if (!suppressCallback)
				{
					ApplyHsvFromSliders();
				}
			});
			WireFloatField(hueField, hueSlider);
			saturationSlider.onValueChanged.RemoveAllListeners();
			saturationSlider.onValueChanged.AddListener(delegate(float v)
			{
				SetFloatFieldText(saturationField, v);
				if (!suppressCallback)
				{
					ApplyHsvFromSliders();
				}
			});
			WireFloatField(saturationField, saturationSlider);
		}

		private void ApplyHsvFromSliders()
		{
			float v = ((valueSlider != null) ? valueSlider.value : 1f);
			ApplyRgb(Color.HSVToRGB(hueSlider.value, saturationSlider.value, v));
		}

		private void SyncHueSaturation(Color color)
		{
			if (!(hueSlider == null) && !(saturationSlider == null))
			{
				Color.RGBToHSV(color, out var H, out var S, out var _);
				hueSlider.value = H;
				saturationSlider.value = S;
			}
		}

		private void WirePalette()
		{
			if (paletteSwatches == null)
			{
				return;
			}
			for (int i = 0; i < paletteSwatches.Length; i++)
			{
				Image image = paletteSwatches[i];
				if (image == null)
				{
					continue;
				}
				Button component = image.GetComponent<Button>();
				if (!(component == null))
				{
					Color swatchColor = image.color;
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate
					{
						ApplyRgb(swatchColor);
					});
				}
			}
		}

		private void Awake()
		{
			EnsureRgbHexControls();
			EnsureHueSaturationControls();
			EnsurePalette();
			WirePalette();
			WireHueSaturation();
			wheel.ColorChanged += OnWheelColorChanged;
			wheel.ColorCommitted += OnColorCommitted;
			valueSlider.onValueChanged.RemoveAllListeners();
			valueSlider.onValueChanged.AddListener(delegate(float v)
			{
				SetFloatFieldText(valueField, v);
				if (!suppressCallback)
				{
					wheel.SetValue(v);
				}
			});
			WireFloatField(valueField, valueSlider);
			alphaSlider.onValueChanged.RemoveAllListeners();
			alphaSlider.onValueChanged.AddListener(delegate(float v)
			{
				SetFloatFieldText(alphaField, v);
				OnAlphaChanged(v);
			});
			WireFloatField(alphaField, alphaSlider);
			WireChannelSlider(redSlider);
			WireChannelSlider(greenSlider);
			WireChannelSlider(blueSlider);
			WireChannelField(redField, redSlider);
			WireChannelField(greenField, greenSlider);
			WireChannelField(blueField, blueSlider);
			hexField.onEndEdit.RemoveAllListeners();
			hexField.onEndEdit.AddListener(OnHexEndEdit);
			eyedropperButton.onClick.RemoveAllListeners();
			eyedropperButton.onClick.AddListener(delegate
			{
				boundController?.ArmPaintEyedropper();
			});
			brushModeButton.onClick.RemoveAllListeners();
			brushModeButton.onClick.AddListener(delegate
			{
				if (boundController != null)
				{
					boundController.PaintMode = PaintMode.Brush;
				}
			});
			bucketModeButton.onClick.RemoveAllListeners();
			bucketModeButton.onClick.AddListener(delegate
			{
				if (boundController != null)
				{
					boundController.PaintMode = PaintMode.Bucket;
				}
			});
			patternModeButton.onClick.RemoveAllListeners();
			patternModeButton.onClick.AddListener(delegate
			{
				if (boundController != null)
				{
					boundController.PaintMode = PaintMode.Pattern;
				}
			});
			brushRadiusSlider.onValueChanged.RemoveAllListeners();
			brushRadiusSlider.onValueChanged.AddListener(delegate(float v)
			{
				SetFloatFieldText(brushRadiusField, v);
				if (!suppressCallback && boundController != null)
				{
					boundController.BrushRadius = v;
				}
			});
			WireFloatField(brushRadiusField, brushRadiusSlider);
			thresholdSlider.onValueChanged.RemoveAllListeners();
			thresholdSlider.onValueChanged.AddListener(delegate(float v)
			{
				SetFloatFieldText(thresholdField, v);
				if (!suppressCallback && boundController != null)
				{
					boundController.PaintThreshold = v;
				}
			});
			WireFloatField(thresholdField, thresholdSlider);
			if (modeSelectorButton != null)
			{
				modeSelectorButton.onClick.RemoveAllListeners();
				modeSelectorButton.onClick.AddListener(delegate
				{
					if (modeSelectionPanel != null)
					{
						modeSelectionPanel.SetActive(!modeSelectionPanel.activeSelf);
					}
				});
			}
			if (colorWheelButton != null)
			{
				colorWheelButton.onClick.RemoveAllListeners();
				colorWheelButton.onClick.AddListener(delegate
				{
					if (colorWheelContainer != null)
					{
						colorWheelContainer.SetActive(!colorWheelContainer.activeSelf);
					}
					LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)wheel.transform);
				});
			}
			patternPicker.PatternClicked += OnPatternClicked;
			recentColors.SwatchClicked += OnRecentColorClicked;
			ShowColor(VoxelEditorSettings.PaintColor);
		}

		private void OnEnable()
		{
			ShowColor(VoxelEditorSettings.PaintColor);
		}

		private void ShowColor(Color color)
		{
			if (!(wheel == null) && !(previewSwatch == null))
			{
				previewSwatch.color = color;
				wheel.SetColor(color);
				Color.RGBToHSV(color, out var H, out var S, out var V);
				suppressCallback = true;
				valueSlider.value = V;
				alphaSlider.value = color.a;
				if (hueSlider != null)
				{
					hueSlider.value = H;
				}
				if (saturationSlider != null)
				{
					saturationSlider.value = S;
				}
				SetFloatFieldText(valueField, V);
				SetFloatFieldText(alphaField, color.a);
				SetFloatFieldText(hueField, H);
				SetFloatFieldText(saturationField, S);
				SyncRgbHexDisplay(color);
				suppressCallback = false;
			}
		}

		public void ReportModeOptions()
		{
			EditorModeOptions.SetOffered(PaintMode.Brush, IsShown(brushModeButton));
			EditorModeOptions.SetOffered(PaintMode.Bucket, IsShown(bucketModeButton));
			EditorModeOptions.SetOffered(PaintMode.Pattern, IsShown(patternModeButton));
		}

		private static bool IsShown(Button button)
		{
			if (button != null)
			{
				return button.gameObject.activeSelf;
			}
			return false;
		}

		public void Bind(VoxelEditorController controller)
		{
			boundController = controller;
			if (!(controller == null))
			{
				ReportModeOptions();
				PaintMode paintMode = EditorModeOptions.Resolve(controller.PaintMode);
				if (paintMode != controller.PaintMode)
				{
					controller.PaintMode = paintMode;
				}
				bool flag = controller.PaintMode == PaintMode.Brush;
				bool flag2 = controller.PaintMode == PaintMode.Bucket;
				bool flag3 = controller.PaintMode == PaintMode.Pattern;
				brushModeBackground.color = (flag ? ActiveColor : InactiveColor);
				bucketModeBackground.color = (flag2 ? ActiveColor : InactiveColor);
				patternModeBackground.color = (flag3 ? ActiveColor : InactiveColor);
				switch (controller.PaintMode)
				{
				case PaintMode.Brush:
					brushModeIcon.sprite = brushSprite;
					break;
				case PaintMode.Bucket:
					brushModeIcon.sprite = bucketSprite;
					break;
				case PaintMode.Pattern:
					brushModeIcon.sprite = patternSprite;
					break;
				}
				bool activeSelf = brushLabel.gameObject.activeSelf;
				bool flag4 = flag2 || flag3;
				bool activeSelf2 = thresholdLabel.gameObject.activeSelf;
				brushLabel.gameObject.SetActive(flag);
				brushRadiusSlider.gameObject.SetActive(flag);
				thresholdLabel.gameObject.SetActive(flag4);
				thresholdSlider.gameObject.SetActive(flag4);
				patternPicker.gameObject.SetActive(flag3);
				if (activeSelf != flag || activeSelf2 != flag4)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)brushLabel.transform.parent);
				}
				if (flag3)
				{
					patternPicker.SetSelected(VoxelEditorSettings.PatternTexture);
				}
				Color paintColor = controller.PaintColor;
				previewSwatch.color = paintColor;
				if (colorPanel != null)
				{
					colorPanel.SetActive(!flag3);
				}
				if (patternPanel != null)
				{
					patternPanel.SetActive(flag3);
				}
				if (modeSelectorButton != null && modeSelectionPanel != null)
				{
					modeSelectorButton.image.color = (modeSelectionPanel.activeSelf ? ActiveColor : InactiveColor);
				}
				if (colorWheelButton != null && colorWheelContainer != null)
				{
					colorWheelButton.image.color = (colorWheelContainer.activeSelf ? ActiveColor : InactiveColor);
				}
				if (!(paintColor == lastSyncedColor))
				{
					lastSyncedColor = paintColor;
					ShowColor(paintColor);
					suppressCallback = true;
					brushRadiusSlider.value = controller.BrushRadius;
					thresholdSlider.value = controller.PaintThreshold;
					SetFloatFieldText(brushRadiusField, controller.BrushRadius);
					SetFloatFieldText(thresholdField, controller.PaintThreshold);
					suppressCallback = false;
				}
			}
		}

		private void OnWheelColorChanged(Color color)
		{
			if (!(boundController == null))
			{
				color.a = alphaSlider.value;
				boundController.PaintColor = color;
				lastSyncedColor = color;
				previewSwatch.color = color;
				suppressCallback = true;
				SyncRgbHexDisplay(color);
				suppressCallback = false;
			}
		}

		private void OnAlphaChanged(float a)
		{
			if (!suppressCallback && !(boundController == null))
			{
				Color paintColor = boundController.PaintColor;
				paintColor.a = a;
				boundController.PaintColor = paintColor;
				lastSyncedColor = paintColor;
				previewSwatch.color = paintColor;
			}
		}

		private void OnColorCommitted()
		{
			if (boundController != null)
			{
				recentColors.Push(boundController.PaintColor);
			}
		}

		private void OnPatternClicked(Texture2D pattern)
		{
			VoxelEditorSettings.PatternTexture = pattern;
			patternPicker.SetSelected(pattern);
		}

		private void OnRecentColorClicked(Color color)
		{
			if (!(boundController == null))
			{
				boundController.PaintColor = color;
				lastSyncedColor = color;
				ShowColor(color);
			}
		}

		private void WireChannelSlider(Slider slider)
		{
			slider.onValueChanged.RemoveAllListeners();
			slider.onValueChanged.AddListener(delegate
			{
				if (!suppressCallback)
				{
					ApplyRgb(CurrentRgb());
				}
			});
		}

		private void WireChannelField(TMP_InputField field, Slider slider)
		{
			field.onEndEdit.RemoveAllListeners();
			field.onEndEdit.AddListener(delegate(string text)
			{
				if (!suppressCallback)
				{
					if (int.TryParse(text, out var result))
					{
						slider.value = Mathf.Clamp(result, 0, 255);
					}
					else
					{
						field.SetTextWithoutNotify(Mathf.RoundToInt(slider.value).ToString());
					}
				}
			});
		}

		private void SetFloatFieldText(TMP_InputField field, float value)
		{
			if (field != null)
			{
				field.SetTextWithoutNotify(value.ToString("0.00", CultureInfo.InvariantCulture));
			}
		}

		private void WireFloatField(TMP_InputField field, Slider slider)
		{
			if (field == null)
			{
				return;
			}
			field.contentType = TMP_InputField.ContentType.DecimalNumber;
			SetFloatFieldText(field, slider.value);
			field.onEndEdit.RemoveAllListeners();
			field.onEndEdit.AddListener(delegate(string text)
			{
				if (!suppressCallback)
				{
					if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
					{
						slider.value = Mathf.Clamp(result, slider.minValue, slider.maxValue);
					}
					else
					{
						SetFloatFieldText(field, slider.value);
					}
				}
			});
		}

		private void OnHexEndEdit(string text)
		{
			if (!suppressCallback)
			{
				if (TryParseHex(text, out var color))
				{
					ApplyRgb(color);
				}
				else
				{
					hexField.SetTextWithoutNotify(ColorToHex(CurrentRgb()));
				}
			}
		}

		private Color32 CurrentRgb()
		{
			return new Color32((byte)Mathf.RoundToInt(redSlider.value), (byte)Mathf.RoundToInt(greenSlider.value), (byte)Mathf.RoundToInt(blueSlider.value), byte.MaxValue);
		}

		private void ApplyRgb(Color32 rgb)
		{
			if (!(boundController == null))
			{
				Color color = rgb;
				color.a = alphaSlider.value;
				boundController.PaintColor = color;
				lastSyncedColor = color;
				previewSwatch.color = color;
				wheel.SetColor(color);
				Color.RGBToHSV(color, out var _, out var _, out var V);
				suppressCallback = true;
				valueSlider.value = V;
				SyncHueSaturation(color);
				SyncRgbHexDisplay(rgb);
				suppressCallback = false;
			}
		}

		private void SyncRgbHexDisplay(Color32 rgb)
		{
			redSlider.value = (int)rgb.r;
			greenSlider.value = (int)rgb.g;
			blueSlider.value = (int)rgb.b;
			redField.SetTextWithoutNotify(rgb.r.ToString());
			greenField.SetTextWithoutNotify(rgb.g.ToString());
			blueField.SetTextWithoutNotify(rgb.b.ToString());
			hexField.SetTextWithoutNotify(ColorToHex(rgb));
		}

		private static bool TryParseHex(string text, out Color32 color)
		{
			color = default(Color32);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			string text2 = text.Trim().TrimStart('#');
			if (text2.Length != 6)
			{
				return false;
			}
			if (!byte.TryParse(text2.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
			{
				return false;
			}
			if (!byte.TryParse(text2.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result2))
			{
				return false;
			}
			if (!byte.TryParse(text2.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result3))
			{
				return false;
			}
			color = new Color32(result, result2, result3, byte.MaxValue);
			return true;
		}

		private static string ColorToHex(Color32 c)
		{
			return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
		}
	}
}
