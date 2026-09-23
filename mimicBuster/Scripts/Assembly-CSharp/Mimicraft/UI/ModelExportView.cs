using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Mimicraft.Export;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ModelExportView : MonoBehaviour
	{
		private const string PrefsPrefix = "Mimicraft.Export.";

		private static readonly int[] PixelChoices = new int[5] { 1, 2, 4, 8, 16 };

		private static readonly int[] PaddingChoices = new int[4] { 0, 1, 2, 4 };

		private const float MinScale = 0.001f;

		private const float MaxScale = 1000f;

		private const int FileNameLimit = 64;

		private const int ScaleLimit = 9;

		[Tooltip("Açılıp kapanan panel. Boş bırakılırsa bu bileşenin kendi objesi kullanılır.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Hangi modelin dışa aktarıldığını yazar. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI modelNameLabel;

		[Header("Ayarlar")]
		[Tooltip("Dosya adı, uzantısız. Boş bırakılırsa modelin adı kullanılır. Dosya adında geçersiz karakterler yazılamaz; ayarını kod yapıyor.")]
		[SerializeField]
		private TMP_InputField fileNameField;

		[Tooltip("Boyut çarpanı. 1 = oyundaki gerçek boyutu, metre cinsinden. Sadece pozitif ondalık sayı kabul eder, virgül noktaya çevrilir; ayarını kod yapıyor.")]
		[SerializeField]
		private TMP_InputField scaleField;

		[Tooltip("Bir voxel yüzüne düşen piksel: 1, 2, 4, 8, 16. Seçenekleri kod doldurur. Prefab'ın Caption Text, Item Text ve Template alanları dolu olmalı.")]
		[SerializeField]
		private TMP_Dropdown pixelsPerVoxelDropdown;

		[Tooltip("Atlastaki her bloğun etrafındaki kenar payı, piksel: 0, 1, 2, 4. Seçenekleri kod doldurur.")]
		[SerializeField]
		private TMP_Dropdown paddingDropdown;

		[Tooltip("Modelin sıfır noktası: alt orta, orta ya da editördeki orijin. Seçenekleri kod doldurur.")]
		[SerializeField]
		private TMP_Dropdown pivotDropdown;

		[Tooltip("Doku boyutunu ikinin kuvvetine yuvarlar. Çoğu motor bunu ister.")]
		[SerializeField]
		private Toggle powerOfTwoToggle;

		[Tooltip("Birden çok parçalı modeli tek obje olarak yazar. Kapalıyken her parça ayrı obje olur.")]
		[SerializeField]
		private Toggle mergePiecesToggle;

		[Tooltip("Dokuyu FBX'in içine de gömer. PNG her durumda yanına yazılır.")]
		[SerializeField]
		private Toggle embedTextureToggle;

		[Header("Düğmeler")]
		[SerializeField]
		private Button exportButton;

		[Tooltip("Paneli kapatır. İsteğe bağlı - Esc de kapatır.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Çıktı klasörünü açar, dışa aktarmadan. İsteğe bağlı.")]
		[SerializeField]
		private Button openFolderButton;

		[Tooltip("Dışa aktarma bitince çıktı klasörünü kendiliğinden açar.")]
		[SerializeField]
		private bool openFolderWhenDone = true;

		private readonly ModelExportSettings settings = new ModelExportSettings();

		private TemplateListEntry entry;

		private bool wired;

		private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char>(Path.GetInvalidFileNameChars());

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

		public bool IsOpen => Target.activeSelf;

		public void Show(TemplateListEntry entry)
		{
			this.entry = entry;
			Wire();
			LoadSettings();
			ApplyToControls();
			if (modelNameLabel != null)
			{
				modelNameLabel.text = entry.ModelName;
			}
			if (fileNameField != null)
			{
				fileNameField.SetTextWithoutNotify(CleanFileName(entry.ModelName));
			}
			Target.SetActive(value: true);
		}

		public void Hide()
		{
			Target.SetActive(value: false);
		}

		private void Wire()
		{
			if (!wired)
			{
				wired = true;
				if (exportButton != null)
				{
					exportButton.onClick.AddListener(Export);
				}
				else
				{
					Debug.LogWarning("[ModelExportView] Export Button bağlı değil - panel açılır ama dışa aktaramaz.", this);
				}
				if (closeButton != null)
				{
					closeButton.onClick.AddListener(Hide);
				}
				if (openFolderButton != null)
				{
					openFolderButton.onClick.AddListener(ModelFbxExporter.RevealFolder);
				}
				ConfigureFileNameField();
				ConfigureScaleField();
				CheckDropdown(pixelsPerVoxelDropdown, "pixelsPerVoxelDropdown");
				CheckDropdown(paddingDropdown, "paddingDropdown");
				CheckDropdown(pivotDropdown, "pivotDropdown");
				Loc.Changed += FillDropdowns;
			}
		}

		private void OnDestroy()
		{
			Loc.Changed -= FillDropdowns;
		}

		private void Update()
		{
			if (!DialogView.IsShowing && Target.activeSelf && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, Hide);
			}
		}

		private void ConfigureFileNameField()
		{
			if (fileNameField == null)
			{
				return;
			}
			fileNameField.contentType = TMP_InputField.ContentType.Standard;
			fileNameField.lineType = TMP_InputField.LineType.SingleLine;
			fileNameField.characterLimit = 64;
			fileNameField.onValidateInput = ValidateFileNameChar;
			fileNameField.onEndEdit.AddListener(delegate(string text)
			{
				string text2 = CleanFileName(text);
				if (text2 != text)
				{
					fileNameField.SetTextWithoutNotify(text2);
				}
			});
		}

		private static char ValidateFileNameChar(string text, int charIndex, char added)
		{
			if (!InvalidFileNameChars.Contains(added))
			{
				return added;
			}
			return '\0';
		}

		private static string CleanFileName(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			foreach (char c in text)
			{
				if (!InvalidFileNameChars.Contains(c))
				{
					stringBuilder.Append(c);
				}
			}
			string text2 = stringBuilder.ToString();
			if (text2.Length <= 64)
			{
				return text2;
			}
			return text2.Substring(0, 64);
		}

		private void ConfigureScaleField()
		{
			if (scaleField == null)
			{
				return;
			}
			scaleField.contentType = TMP_InputField.ContentType.DecimalNumber;
			scaleField.lineType = TMP_InputField.LineType.SingleLine;
			scaleField.characterLimit = 9;
			scaleField.onValidateInput = ValidateScaleChar;
			scaleField.onValueChanged.AddListener(delegate(string text)
			{
				string text2 = CleanDecimal(text);
				if (text2 != text)
				{
					scaleField.SetTextWithoutNotify(text2);
				}
			});
			scaleField.onEndEdit.AddListener(delegate(string text)
			{
				if (TryParseScale(text, out var value))
				{
					settings.scale = value;
				}
				scaleField.SetTextWithoutNotify(FormatScale(settings.scale));
			});
		}

		private static char ValidateScaleChar(string text, int charIndex, char added)
		{
			if (added >= '0' && added <= '9')
			{
				return added;
			}
			if (added != '.' && added != ',')
			{
				return '\0';
			}
			return '.';
		}

		private static string CleanDecimal(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			bool flag = false;
			foreach (char c in text)
			{
				char c2 = ((c == ',') ? '.' : c);
				if (c2 >= '0' && c2 <= '9')
				{
					stringBuilder.Append(c2);
				}
				else if (c2 == '.' && !flag)
				{
					stringBuilder.Append(c2);
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}

		private static bool TryParseScale(string text, out float value)
		{
			value = 0f;
			string text2 = CleanDecimal(text);
			if (text2.Length == 0 || text2 == "." || !float.TryParse(text2, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || result <= 0f)
			{
				return false;
			}
			value = Mathf.Clamp(result, 0.001f, 1000f);
			return true;
		}

		private static string FormatScale(float value)
		{
			return value.ToString("0.###", CultureInfo.InvariantCulture);
		}

		private void CheckDropdown(TMP_Dropdown dropdown, string field)
		{
			if (!(dropdown == null))
			{
				List<string> list = new List<string>();
				if (dropdown.template == null)
				{
					list.Add("Template");
				}
				if (dropdown.captionText == null)
				{
					list.Add("Caption Text");
				}
				if (dropdown.itemText == null)
				{
					list.Add("Item Text");
				}
				if (list.Count != 0)
				{
					Debug.LogWarning("[ModelExportView] '" + dropdown.name + "' (" + field + ") açılır listesinde " + string.Join(", ", list) + " boş - seçenekler doluyor ama görünmüyor. Prefab kopyasında bu alanlara sağ tıklayıp 'Revert' yap.", dropdown);
				}
			}
		}

		private void FillDropdowns()
		{
			Fill(pixelsPerVoxelDropdown, Labels(PixelChoices), IndexOf(PixelChoices, settings.pixelsPerVoxel));
			Fill(paddingDropdown, Labels(PaddingChoices), IndexOf(PaddingChoices, settings.padding));
			Fill(pivotDropdown, new List<string>
			{
				Loc.Get("Export.Pivot.BottomCenter"),
				Loc.Get("Export.Pivot.Center"),
				Loc.Get("Export.Pivot.Origin")
			}, (int)settings.pivot);
		}

		private void ApplyToControls()
		{
			FillDropdowns();
			if (scaleField != null)
			{
				scaleField.SetTextWithoutNotify(FormatScale(settings.scale));
			}
			if (powerOfTwoToggle != null)
			{
				powerOfTwoToggle.SetIsOnWithoutNotify(settings.powerOfTwo);
			}
			if (mergePiecesToggle != null)
			{
				mergePiecesToggle.SetIsOnWithoutNotify(settings.mergePieces);
			}
			if (embedTextureToggle != null)
			{
				embedTextureToggle.SetIsOnWithoutNotify(settings.embedTexture);
			}
		}

		private void ReadControls()
		{
			if (scaleField != null && TryParseScale(scaleField.text, out var value))
			{
				settings.scale = value;
			}
			if (pixelsPerVoxelDropdown != null)
			{
				settings.pixelsPerVoxel = PixelChoices[Mathf.Clamp(pixelsPerVoxelDropdown.value, 0, PixelChoices.Length - 1)];
			}
			if (paddingDropdown != null)
			{
				settings.padding = PaddingChoices[Mathf.Clamp(paddingDropdown.value, 0, PaddingChoices.Length - 1)];
			}
			if (pivotDropdown != null)
			{
				settings.pivot = (ExportPivot)Mathf.Clamp(pivotDropdown.value, 0, 2);
			}
			if (powerOfTwoToggle != null)
			{
				settings.powerOfTwo = powerOfTwoToggle.isOn;
			}
			if (mergePiecesToggle != null)
			{
				settings.mergePieces = mergePiecesToggle.isOn;
			}
			if (embedTextureToggle != null)
			{
				settings.embedTexture = embedTextureToggle.isOn;
			}
		}

		private void LoadSettings()
		{
			float num = PlayerPrefs.GetFloat("Mimicraft.Export.Scale", settings.scale);
			settings.scale = ((num > 0f) ? Mathf.Clamp(num, 0.001f, 1000f) : 1f);
			settings.pixelsPerVoxel = PlayerPrefs.GetInt("Mimicraft.Export.PixelsPerVoxel", settings.pixelsPerVoxel);
			settings.padding = PlayerPrefs.GetInt("Mimicraft.Export.Padding", settings.padding);
			settings.pivot = (ExportPivot)Mathf.Clamp(PlayerPrefs.GetInt("Mimicraft.Export.Pivot", (int)settings.pivot), 0, 2);
			settings.powerOfTwo = PlayerPrefs.GetInt("Mimicraft.Export.PowerOfTwo", settings.powerOfTwo ? 1 : 0) != 0;
			settings.mergePieces = PlayerPrefs.GetInt("Mimicraft.Export.MergePieces", settings.mergePieces ? 1 : 0) != 0;
			settings.embedTexture = PlayerPrefs.GetInt("Mimicraft.Export.EmbedTexture", settings.embedTexture ? 1 : 0) != 0;
		}

		private void SaveSettings()
		{
			PlayerPrefs.SetFloat("Mimicraft.Export.Scale", settings.scale);
			PlayerPrefs.SetInt("Mimicraft.Export.PixelsPerVoxel", settings.pixelsPerVoxel);
			PlayerPrefs.SetInt("Mimicraft.Export.Padding", settings.padding);
			PlayerPrefs.SetInt("Mimicraft.Export.Pivot", (int)settings.pivot);
			PlayerPrefs.SetInt("Mimicraft.Export.PowerOfTwo", settings.powerOfTwo ? 1 : 0);
			PlayerPrefs.SetInt("Mimicraft.Export.MergePieces", settings.mergePieces ? 1 : 0);
			PlayerPrefs.SetInt("Mimicraft.Export.EmbedTexture", settings.embedTexture ? 1 : 0);
			PlayerPrefs.Save();
		}

		private void Export()
		{
			ReadControls();
			SaveSettings();
			if (scaleField != null)
			{
				scaleField.SetTextWithoutNotify(FormatScale(settings.scale));
			}
			TemplateModel templateModel = (string.IsNullOrEmpty(entry.FilePath) ? null : TemplateStorage.LoadTemplate(entry.FilePath));
			if (templateModel == null)
			{
				Tell(Loc.Get("Export.LoadFailed"));
				return;
			}
			string fileName = ((fileNameField != null) ? CleanFileName(fileNameField.text) : entry.ModelName);
			ModelExportReport report;
			string detail;
			switch (ModelFbxExporter.TryExport(templateModel, fileName, settings, out report, out detail))
			{
			case ModelExportFailure.None:
				Tell(Loc.Format("Export.Done", report.Quads, report.TextureWidth, report.TextureHeight, report.FbxPath));
				if (openFolderWhenDone)
				{
					ModelFbxExporter.RevealFolder();
				}
				break;
			case ModelExportFailure.Empty:
				Tell(Loc.Get("Export.Empty"));
				break;
			case ModelExportFailure.TextureTooLarge:
				Tell(Loc.Get("Export.TooLarge"));
				break;
			default:
				Tell(Loc.Format("Export.Failed", detail));
				break;
			}
		}

		private static void Tell(string message)
		{
			DialogView.Show(new DialogRequest(message, Loc.Get("Common.Ok")), null);
		}

		private static void Fill(TMP_Dropdown dropdown, List<string> labels, int index)
		{
			if (!(dropdown == null))
			{
				dropdown.ClearOptions();
				dropdown.AddOptions(labels);
				dropdown.SetValueWithoutNotify(Mathf.Clamp(index, 0, Mathf.Max(0, labels.Count - 1)));
				dropdown.RefreshShownValue();
			}
		}

		private static List<string> Labels(int[] values)
		{
			List<string> list = new List<string>(values.Length);
			foreach (int num in values)
			{
				list.Add(num.ToString(CultureInfo.InvariantCulture));
			}
			return list;
		}

		private static int IndexOf(int[] values, int wanted)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] == wanted)
				{
					return i;
				}
			}
			return 0;
		}
	}
}
