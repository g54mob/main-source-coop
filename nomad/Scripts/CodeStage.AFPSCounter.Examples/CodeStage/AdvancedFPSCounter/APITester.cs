using CodeStage.AdvancedFPSCounter.Labels;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter
{
	public class APITester : MonoBehaviour
	{
		private int selectedTab;

		private readonly string[] tabs = new string[5] { "Common", "Look & Feel", "FPS Counter", "Memory Counter", "Device info" };

		private FPSLevel currentFPSLevel;

		private void Start()
		{
			AFPSCounter.AddToScene().fpsCounter.OnFPSLevelChange += OnFPSLevelChanged;
		}

		private void OnFPSLevelChanged(FPSLevel newLevel)
		{
			currentFPSLevel = newLevel;
		}

		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect(40f, 150f, Screen.width - 80, Screen.height - 80));
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label)
			{
				richText = true
			};
			GUIStyle style = new GUIStyle(gUIStyle)
			{
				alignment = TextAnchor.UpperCenter
			};
			GUILayout.Label("<b>Public API usage examples</b>", style);
			selectedTab = GUILayout.Toolbar(selectedTab, tabs);
			switch (selectedTab)
			{
			case 0:
				GUILayout.Space(10f);
				DrawCommonTab();
				break;
			case 1:
				GUILayout.Space(10f);
				DrawLookFeelTab();
				break;
			case 2:
				GUILayout.Space(10f);
				DrawFPSCounterTab();
				break;
			case 3:
				GUILayout.Space(10f);
				DrawMemoryCounterTab();
				break;
			case 4:
				GUILayout.Space(10f);
				DrawDeviceInfoTab();
				break;
			default:
				GUILayout.Label("Wrong tab!");
				break;
			}
			GUILayout.Space(5f);
			GUILayout.Label("<b>Raw counters values</b> (read using API)", gUIStyle);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical(GUILayout.ExpandWidth(expand: true), GUILayout.MinWidth(300f));
			GUILayout.Label("<size=11>  FPS: " + AFPSCounter.Instance.fpsCounter.LastValue + "  [" + AFPSCounter.Instance.fpsCounter.LastMillisecondsValue + " MS]  AVG: " + AFPSCounter.Instance.fpsCounter.LastAverageValue + "  [" + AFPSCounter.Instance.fpsCounter.LastAverageMillisecondsValue + " MS]\n  MIN: " + AFPSCounter.Instance.fpsCounter.LastMinimumValue + "  [" + AFPSCounter.Instance.fpsCounter.LastMinMillisecondsValue + " MS]  MAX: " + AFPSCounter.Instance.fpsCounter.LastMaximumValue + "  [" + AFPSCounter.Instance.fpsCounter.LastMaxMillisecondsValue + " MS]\n  RNDR: [" + AFPSCounter.Instance.fpsCounter.LastRenderValue + " MS]\n  Level (direct / callback): " + AFPSCounter.Instance.fpsCounter.CurrentFpsLevel.ToString() + " / " + currentFPSLevel.ToString() + "</size>");
			if (AFPSCounter.Instance.memoryCounter.Precise)
			{
				GUILayout.Label("<size=11>  Memory (Total, Allocated, Mono, Gfx):\n  " + (float)AFPSCounter.Instance.memoryCounter.LastTotalValue / 1048576f + ", " + (float)AFPSCounter.Instance.memoryCounter.LastAllocatedValue / 1048576f + ", " + (float)AFPSCounter.Instance.memoryCounter.LastMonoValue / 1048576f + ", " + (float)AFPSCounter.Instance.memoryCounter.LastGfxValue / 1048576f + "</size>");
			}
			else
			{
				GUILayout.Label("<size=11>  Memory (Total, Allocated, Mono, Gfx):\n  " + AFPSCounter.Instance.memoryCounter.LastTotalValue + ", " + AFPSCounter.Instance.memoryCounter.LastAllocatedValue + ", " + AFPSCounter.Instance.memoryCounter.LastMonoValue + ", " + AFPSCounter.Instance.memoryCounter.LastGfxValue + "</size>");
			}
			GUILayout.EndVertical();
			if (AFPSCounter.Instance.deviceInfoCounter.Enabled)
			{
				GUILayout.Label("<size=11>" + AFPSCounter.Instance.deviceInfoCounter.LastValue + "</size>");
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		private void DrawCommonTab()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Operation Mode:", GUILayout.MaxWidth(100f));
			int operationMode = (int)AFPSCounter.Instance.OperationMode;
			operationMode = GUILayout.Toolbar(operationMode, new string[3]
			{
				OperationMode.Disabled.ToString(),
				OperationMode.Background.ToString(),
				OperationMode.Normal.ToString()
			});
			if (GUI.changed)
			{
				AFPSCounter.Instance.OperationMode = (OperationMode)operationMode;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Hot Key:", GUILayout.MaxWidth(100f));
			int selected = (int)((AFPSCounter.Instance.hotKey == KeyCode.BackQuote) ? ((KeyCode)1) : AFPSCounter.Instance.hotKey);
			selected = GUILayout.Toolbar(selected, new string[2] { "None (disabled)", "BackQuote (`)" });
			AFPSCounter.Instance.hotKey = ((selected == 1) ? KeyCode.BackQuote : KeyCode.None);
			AFPSCounter.Instance.circleGesture = GUILayout.Toggle(AFPSCounter.Instance.circleGesture, "Circle Gesture", GUILayout.ExpandWidth(expand: false));
			GUILayout.EndHorizontal();
			GUI.enabled = selected == 1;
			GUILayout.Label("Hot Key modifiers:");
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.hotKeyCtrl = GUILayout.Toggle(AFPSCounter.Instance.hotKeyCtrl, "Ctrl / Cmd", GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(10f);
			AFPSCounter.Instance.hotKeyAlt = GUILayout.Toggle(AFPSCounter.Instance.hotKeyAlt, "Alt", GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(10f);
			AFPSCounter.Instance.hotKeyShift = GUILayout.Toggle(AFPSCounter.Instance.hotKeyShift, "Shift", GUILayout.ExpandWidth(expand: false));
			GUILayout.EndHorizontal();
			GUI.enabled = true;
			GUILayout.Space(10f);
			GUILayout.Label("KeepAlive enabled: " + AFPSCounter.Instance.KeepAlive);
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.ForceFrameRate = GUILayout.Toggle(AFPSCounter.Instance.ForceFrameRate, "Force FPS", GUILayout.Width(100f));
			AFPSCounter.Instance.ForcedFrameRate = (int)SliderLabel(AFPSCounter.Instance.ForcedFrameRate, -1f, 100f);
			GUILayout.EndHorizontal();
		}

		private void DrawLookFeelTab()
		{
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.PixelPerfect = GUILayout.Toggle(AFPSCounter.Instance.PixelPerfect, "Pixel Perfect", GUILayout.Width(100f));
			AFPSCounter.Instance.AutoScale = GUILayout.Toggle(AFPSCounter.Instance.AutoScale, "Auto scale", GUILayout.Width(100f));
			GUILayout.Label("Scale", GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(5f);
			AFPSCounter.Instance.ScaleFactor = SliderLabel(AFPSCounter.Instance.ScaleFactor, 0.1f, 10f);
			GUILayout.Space(30f);
			GUILayout.Label("Font Size", GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(5f);
			AFPSCounter.Instance.FontSize = (int)SliderLabel(AFPSCounter.Instance.FontSize, 1f, 100f);
			GUILayout.EndHorizontal();
			AFPSCounter.Instance.PaddingOffset = Vector2Slider(AFPSCounter.Instance.PaddingOffset, "Padding");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Line spacing", GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(5f);
			AFPSCounter.Instance.LineSpacing = SliderLabel(AFPSCounter.Instance.LineSpacing, 0f, 10f);
			GUILayout.Space(30f);
			GUILayout.Label("Counters spacing", GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(5f);
			AFPSCounter.Instance.CountersSpacing = (int)SliderLabel(AFPSCounter.Instance.CountersSpacing, 0f, 10f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.Background = GUILayout.Toggle(AFPSCounter.Instance.Background, "Background", GUILayout.Width(100f));
			GUILayout.Space(5f);
			GUI.enabled = AFPSCounter.Instance.Background;
			AFPSCounter.Instance.BackgroundColor = ColorSliders("Color", AFPSCounter.Instance.BackgroundColor);
			GUILayout.Label("Padding", GUILayout.Width(60f));
			AFPSCounter.Instance.BackgroundPadding = (int)SliderLabel(AFPSCounter.Instance.BackgroundPadding, 0f, 50f);
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.Shadow = GUILayout.Toggle(AFPSCounter.Instance.Shadow, "Shadow", GUILayout.Width(100f));
			GUILayout.Space(5f);
			GUI.enabled = AFPSCounter.Instance.Shadow;
			AFPSCounter.Instance.ShadowColor = ColorSliders("Color", AFPSCounter.Instance.ShadowColor);
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.Outline = GUILayout.Toggle(AFPSCounter.Instance.Outline, "Outline", GUILayout.Width(100f));
			GUILayout.Space(5f);
			GUI.enabled = AFPSCounter.Instance.Outline;
			AFPSCounter.Instance.OutlineColor = ColorSliders("Color", AFPSCounter.Instance.OutlineColor);
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			Camera.main.backgroundColor = ColorSliders("Scene background color", Camera.main.backgroundColor);
		}

		private void DrawFPSCounterTab()
		{
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.fpsCounter.Enabled = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.Enabled, "Enabled");
			GUILayout.Label("Style: ", GUILayout.Width(35f));
			AFPSCounter.Instance.fpsCounter.Style = (FontStyle)GUILayout.Toolbar((int)AFPSCounter.Instance.fpsCounter.Style, new string[4] { "Normal", "Bold", "Italic", "Bold&Italic" });
			GUILayout.Label("Extra text: ", GUILayout.Width(70f));
			if (GUILayout.Button("Append", GUILayout.ExpandWidth(expand: false)))
			{
				AFPSCounter.Instance.fpsCounter.ExtraText = "<b>Some</b> <color=#A76ED1>text</color>!";
			}
			if (GUILayout.Button("Remove", GUILayout.ExpandWidth(expand: false)))
			{
				AFPSCounter.Instance.fpsCounter.ExtraText = null;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			AFPSCounter.Instance.fpsCounter.Anchor = (LabelAnchor)GUILayout.Toolbar((int)AFPSCounter.Instance.fpsCounter.Anchor, new string[6] { "UpperLeft", "UpperRight", "LowerLeft", "LowerRight", "UpperCenter", "LowerCenter" });
			GUILayout.BeginHorizontal();
			GUILayout.Label("Update Interval", GUILayout.Width(100f));
			AFPSCounter.Instance.fpsCounter.UpdateInterval = SliderLabel(AFPSCounter.Instance.fpsCounter.UpdateInterval, 0.1f, 10f);
			GUILayout.EndHorizontal();
			AFPSCounter.Instance.fpsCounter.Milliseconds = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.Milliseconds, "Milliseconds");
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.fpsCounter.Average = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.Average, "Average FPS", GUILayout.Width(100f));
			if (AFPSCounter.Instance.fpsCounter.Average)
			{
				GUILayout.Label("Samples", GUILayout.ExpandWidth(expand: false));
				AFPSCounter.Instance.fpsCounter.AverageSamples = (int)SliderLabel(AFPSCounter.Instance.fpsCounter.AverageSamples, 0f, 100f);
				GUILayout.Space(10f);
				AFPSCounter.Instance.fpsCounter.AverageMilliseconds = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.AverageMilliseconds, "Milliseconds");
				AFPSCounter.Instance.fpsCounter.AverageNewLine = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.AverageNewLine, "On new line");
				AFPSCounter.Instance.fpsCounter.resetAverageOnNewScene = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.resetAverageOnNewScene, "Reset On Load", GUILayout.ExpandWidth(expand: false));
				if (GUILayout.Button("Reset now!", GUILayout.ExpandWidth(expand: false)))
				{
					AFPSCounter.Instance.fpsCounter.ResetAverage();
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.fpsCounter.MinMax = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.MinMax, "MinMax FPS", GUILayout.Width(100f));
			if (AFPSCounter.Instance.fpsCounter.MinMax)
			{
				GUILayout.Label("Delay", GUILayout.ExpandWidth(expand: false));
				AFPSCounter.Instance.fpsCounter.minMaxIntervalsToSkip = (int)SliderLabel(AFPSCounter.Instance.fpsCounter.minMaxIntervalsToSkip, 0f, 10f);
				GUILayout.Space(10f);
				AFPSCounter.Instance.fpsCounter.MinMaxMilliseconds = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.MinMaxMilliseconds, "Milliseconds");
				AFPSCounter.Instance.fpsCounter.MinMaxTwoLines = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.MinMaxTwoLines, "On two lines");
				AFPSCounter.Instance.fpsCounter.MinMaxNewLine = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.MinMaxNewLine, "On new line");
				AFPSCounter.Instance.fpsCounter.resetMinMaxOnNewScene = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.resetMinMaxOnNewScene, "Reset On Load", GUILayout.ExpandWidth(expand: false));
				if (GUILayout.Button("Reset now!", GUILayout.ExpandWidth(expand: false)))
				{
					AFPSCounter.Instance.fpsCounter.ResetMinMax();
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.fpsCounter.Render = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.Render, "Render Time", GUILayout.Width(100f));
			if (AFPSCounter.Instance.fpsCounter.Render)
			{
				AFPSCounter.Instance.fpsCounter.RenderNewLine = GUILayout.Toggle(AFPSCounter.Instance.fpsCounter.RenderNewLine, "On new line");
			}
			GUILayout.EndHorizontal();
		}

		private void DrawMemoryCounterTab()
		{
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.memoryCounter.Enabled = GUILayout.Toggle(AFPSCounter.Instance.memoryCounter.Enabled, "Enabled");
			GUILayout.Label("Style: ", GUILayout.Width(35f));
			AFPSCounter.Instance.memoryCounter.Style = (FontStyle)GUILayout.Toolbar((int)AFPSCounter.Instance.memoryCounter.Style, new string[4] { "Normal", "Bold", "Italic", "Bold&Italic" });
			GUILayout.Label("Extra text: ", GUILayout.Width(70f));
			if (GUILayout.Button("Append", GUILayout.ExpandWidth(expand: false)))
			{
				AFPSCounter.Instance.memoryCounter.ExtraText = "<b>Some</b> <color=#A76ED1>text</color>!";
			}
			if (GUILayout.Button("Remove", GUILayout.ExpandWidth(expand: false)))
			{
				AFPSCounter.Instance.memoryCounter.ExtraText = null;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			AFPSCounter.Instance.memoryCounter.Anchor = (LabelAnchor)GUILayout.Toolbar((int)AFPSCounter.Instance.memoryCounter.Anchor, new string[6] { "UpperLeft", "UpperRight", "LowerLeft", "LowerRight", "UpperCenter", "LowerCenter" });
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Update Interval", GUILayout.Width(100f));
			AFPSCounter.Instance.memoryCounter.UpdateInterval = SliderLabel(AFPSCounter.Instance.memoryCounter.UpdateInterval, 0.1f, 10f);
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			AFPSCounter.Instance.memoryCounter.Precise = GUILayout.Toggle(AFPSCounter.Instance.memoryCounter.Precise, "Precise (uses more system resources)");
			AFPSCounter.Instance.memoryCounter.Total = GUILayout.Toggle(AFPSCounter.Instance.memoryCounter.Total, "Total reserved memory size");
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			AFPSCounter.Instance.memoryCounter.Allocated = GUILayout.Toggle(AFPSCounter.Instance.memoryCounter.Allocated, "Allocated memory size");
			AFPSCounter.Instance.memoryCounter.MonoUsage = GUILayout.Toggle(AFPSCounter.Instance.memoryCounter.MonoUsage, "Mono memory usage");
			AFPSCounter.Instance.memoryCounter.Gfx = GUILayout.Toggle(AFPSCounter.Instance.memoryCounter.Gfx, "GfxDriver memory");
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		private void DrawDeviceInfoTab()
		{
			GUILayout.BeginHorizontal();
			AFPSCounter.Instance.deviceInfoCounter.Enabled = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.Enabled, "Enabled");
			GUILayout.Label("Style: ", GUILayout.Width(35f));
			AFPSCounter.Instance.deviceInfoCounter.Style = (FontStyle)GUILayout.Toolbar((int)AFPSCounter.Instance.deviceInfoCounter.Style, new string[4] { "Normal", "Bold", "Italic", "Bold&Italic" });
			GUILayout.Label("Extra text: ", GUILayout.Width(70f));
			if (GUILayout.Button("Append", GUILayout.ExpandWidth(expand: false)))
			{
				AFPSCounter.Instance.deviceInfoCounter.ExtraText = "<b>Some</b> <color=#A76ED1>text</color>!";
			}
			if (GUILayout.Button("Remove", GUILayout.ExpandWidth(expand: false)))
			{
				AFPSCounter.Instance.deviceInfoCounter.ExtraText = null;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			AFPSCounter.Instance.deviceInfoCounter.Anchor = (LabelAnchor)GUILayout.Toolbar((int)AFPSCounter.Instance.deviceInfoCounter.Anchor, new string[6] { "UpperLeft", "UpperRight", "LowerLeft", "LowerRight", "UpperCenter", "LowerCenter" });
			GUILayout.Space(10f);
			using (new GUILayout.HorizontalScope())
			{
				using (new GUILayout.VerticalScope())
				{
					AFPSCounter.Instance.deviceInfoCounter.Platform = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.Platform, "Platform info");
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.CpuModel = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.CpuModel, "CPU info", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.CpuModelNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.CpuModelNewLine, "new line");
					}
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.GpuModel = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.GpuModel, "GPU Model", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.GpuModelNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.GpuModelNewLine, "new line");
					}
				}
				using (new GUILayout.VerticalScope())
				{
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.GpuApi = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.GpuApi, "GPU API", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.GpuApiNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.GpuApiNewLine, "new line");
					}
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.GpuSpec = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.GpuSpec, "GPU Spec", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.GpuSpecNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.GpuSpecNewLine, "new line");
					}
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.RamSize = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.RamSize, "Total RAM size", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.RamSizeNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.RamSizeNewLine, "new line");
					}
				}
				using (new GUILayout.VerticalScope())
				{
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.ScreenData = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.ScreenData, "Display info", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.ScreenDataNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.ScreenDataNewLine, "new line");
					}
					using (new GUILayout.HorizontalScope())
					{
						AFPSCounter.Instance.deviceInfoCounter.DeviceModel = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.DeviceModel, "Device model", GUILayout.Width(150f));
						AFPSCounter.Instance.deviceInfoCounter.DeviceModelNewLine = GUILayout.Toggle(AFPSCounter.Instance.deviceInfoCounter.DeviceModelNewLine, "new line");
					}
				}
			}
		}

		private static float SliderLabel(float sliderValue, float sliderMinValue, float sliderMaxValue)
		{
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Space(8f);
			sliderValue = GUILayout.HorizontalSlider(sliderValue, sliderMinValue, sliderMaxValue);
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			GUILayout.Label($"{sliderValue:F2}", GUILayout.ExpandWidth(expand: false));
			GUILayout.EndHorizontal();
			return sliderValue;
		}

		private Color ColorSliders(string caption, Color color)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(caption, GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(5f);
			GUILayout.Label("R:", GUILayout.Width(20f));
			color.r = SliderLabel(color.r, 0f, 1f);
			GUILayout.Space(5f);
			GUILayout.Label("G:", GUILayout.Width(20f));
			color.g = SliderLabel(color.g, 0f, 1f);
			GUILayout.Space(5f);
			GUILayout.Label("B:", GUILayout.Width(20f));
			color.b = SliderLabel(color.b, 0f, 1f);
			GUILayout.EndHorizontal();
			return color;
		}

		private Vector2 Vector2Slider(Vector2 input, string label)
		{
			Vector2 result = input;
			GUILayout.BeginHorizontal();
			GUILayout.Label(label, GUILayout.ExpandWidth(expand: false));
			GUILayout.Space(5f);
			GUILayout.Label("X: ", GUILayout.Width(20f));
			result.x = (int)SliderLabel(result.x, 0f, 100f);
			GUILayout.Space(30f);
			GUILayout.Label("Y:", GUILayout.Width(20f));
			result.y = (int)SliderLabel(result.y, 0f, 100f);
			GUILayout.EndHorizontal();
			return result;
		}
	}
}
