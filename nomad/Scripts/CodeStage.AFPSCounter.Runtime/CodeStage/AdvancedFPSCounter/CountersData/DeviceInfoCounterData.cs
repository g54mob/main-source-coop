using System;
using System.Text;
using CodeStage.AdvancedFPSCounter.Labels;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	[Serializable]
	public class DeviceInfoCounterData : StaticCounterData
	{
		[Tooltip("Shows operating system & platform info.")]
		[SerializeField]
		private bool platform = true;

		[Tooltip("CPU model and cores (including virtual cores from Intel's Hyper Threading) count.")]
		[SerializeField]
		private bool cpuModel = true;

		[Tooltip("Check to show CPU model on new line.")]
		[SerializeField]
		private bool cpuModelNewLine = true;

		[Tooltip("Shows GPU model name.")]
		[SerializeField]
		private bool gpuModel = true;

		[Tooltip("Check to show GPU model on new line.")]
		[SerializeField]
		private bool gpuModelNewLine = true;

		[Tooltip("Shows graphics API version and type (if possible).")]
		[SerializeField]
		private bool gpuApi = true;

		[Tooltip("Check to show graphics API version on new line.")]
		[SerializeField]
		private bool gpuApiNewLine = true;

		[Tooltip("Shows graphics supported shader model (if possible), approximate pixel fill-rate (if possible) and total Video RAM size (if possible).")]
		[SerializeField]
		private bool gpuSpec = true;

		[Tooltip("Check to show graphics specs on new line.")]
		[SerializeField]
		private bool gpuSpecNewLine = true;

		[Tooltip("Shows total RAM size.")]
		[SerializeField]
		private bool ramSize = true;

		[Tooltip("Check to show RAM size on new line.")]
		[SerializeField]
		private bool ramSizeNewLine = true;

		[Tooltip("Shows screen resolution, size and DPI (if possible).")]
		[SerializeField]
		private bool screenData = true;

		[Tooltip("Check to show screen data on new line.")]
		[SerializeField]
		private bool screenDataNewLine = true;

		[Tooltip("Shows device model. Actual for mobile devices.")]
		[SerializeField]
		private bool deviceModel;

		[Tooltip("Check to show device model on new line.")]
		[SerializeField]
		private bool deviceModelNewLine = true;

		public bool Platform
		{
			get
			{
				return platform;
			}
			set
			{
				if (platform != value && Application.isPlaying)
				{
					platform = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool CpuModel
		{
			get
			{
				return cpuModel;
			}
			set
			{
				if (cpuModel != value && Application.isPlaying)
				{
					cpuModel = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool CpuModelNewLine
		{
			get
			{
				return cpuModelNewLine;
			}
			set
			{
				if (cpuModelNewLine != value && Application.isPlaying)
				{
					cpuModelNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool GpuModel
		{
			get
			{
				return gpuModel;
			}
			set
			{
				if (gpuModel != value && Application.isPlaying)
				{
					gpuModel = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool GpuModelNewLine
		{
			get
			{
				return gpuModelNewLine;
			}
			set
			{
				if (gpuModelNewLine != value && Application.isPlaying)
				{
					gpuModelNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool GpuApi
		{
			get
			{
				return gpuApi;
			}
			set
			{
				if (gpuApi != value && Application.isPlaying)
				{
					gpuApi = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool GpuApiNewLine
		{
			get
			{
				return gpuApiNewLine;
			}
			set
			{
				if (gpuApiNewLine != value && Application.isPlaying)
				{
					gpuApiNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool GpuSpec
		{
			get
			{
				return gpuSpec;
			}
			set
			{
				if (gpuSpec != value && Application.isPlaying)
				{
					gpuSpec = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool GpuSpecNewLine
		{
			get
			{
				return gpuSpecNewLine;
			}
			set
			{
				if (gpuSpecNewLine != value && Application.isPlaying)
				{
					gpuSpecNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool RamSize
		{
			get
			{
				return ramSize;
			}
			set
			{
				if (ramSize != value && Application.isPlaying)
				{
					ramSize = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool RamSizeNewLine
		{
			get
			{
				return ramSizeNewLine;
			}
			set
			{
				if (ramSizeNewLine != value && Application.isPlaying)
				{
					ramSizeNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool ScreenData
		{
			get
			{
				return screenData;
			}
			set
			{
				if (screenData != value && Application.isPlaying)
				{
					screenData = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool ScreenDataNewLine
		{
			get
			{
				return screenDataNewLine;
			}
			set
			{
				if (screenDataNewLine != value && Application.isPlaying)
				{
					screenDataNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool DeviceModel
		{
			get
			{
				return deviceModel;
			}
			set
			{
				if (deviceModel != value && Application.isPlaying)
				{
					deviceModel = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool DeviceModelNewLine
		{
			get
			{
				return deviceModelNewLine;
			}
			set
			{
				if (deviceModelNewLine != value && Application.isPlaying)
				{
					deviceModelNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public string LastValue { get; private set; }

		internal DeviceInfoCounterData()
		{
			color = new Color32(172, 172, 172, 255);
			anchor = LabelAnchor.LowerLeft;
		}

		internal override void UpdateValue(bool force)
		{
			if (!inited && HasData())
			{
				Activate();
			}
			else if (inited && !HasData())
			{
				Deactivate();
			}
			else
			{
				if (!enabled)
				{
					return;
				}
				bool flag = false;
				if (text == null)
				{
					text = new StringBuilder(500);
				}
				else
				{
					text.Length = 0;
				}
				if (platform)
				{
					text.Append("OS: ").Append(SystemInfo.operatingSystem).Append(" [")
						.Append(Application.platform)
						.Append("]");
					flag = true;
				}
				if (cpuModel)
				{
					if (flag)
					{
						text.Append(cpuModelNewLine ? '\n' : ' ');
					}
					text.Append("CPU: ").Append(SystemInfo.processorType).Append(" [")
						.Append(SystemInfo.processorCount)
						.Append(" cores]");
					flag = true;
				}
				if (gpuModel)
				{
					if (flag)
					{
						text.Append(gpuModelNewLine ? '\n' : ' ');
					}
					text.Append("GPU: ").Append(SystemInfo.graphicsDeviceName);
					flag = true;
				}
				if (gpuApi)
				{
					if (flag)
					{
						text.Append(gpuApiNewLine ? '\n' : ' ');
					}
					if (gpuApiNewLine || !gpuModel)
					{
						text.Append("GPU: ");
					}
					text.Append(SystemInfo.graphicsDeviceVersion);
					text.Append(" [").Append(SystemInfo.graphicsDeviceType).Append("]");
					flag = true;
				}
				if (gpuSpec)
				{
					if (flag)
					{
						text.Append(gpuSpecNewLine ? '\n' : ' ');
					}
					if (gpuSpecNewLine || (!gpuModel && !gpuApi))
					{
						text.Append("GPU: SM: ");
					}
					else
					{
						text.Append("SM: ");
					}
					int graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
					if (graphicsShaderLevel >= 10 && graphicsShaderLevel <= 99)
					{
						text.Append(graphicsShaderLevel /= 10).Append('.').Append(graphicsShaderLevel / 10);
					}
					else
					{
						text.Append("N/A");
					}
					text.Append(", VRAM: ");
					int graphicsMemorySize = SystemInfo.graphicsMemorySize;
					if (graphicsMemorySize > 0)
					{
						text.Append(graphicsMemorySize).Append(" MB");
					}
					else
					{
						text.Append("N/A");
					}
					flag = true;
				}
				if (ramSize)
				{
					if (flag)
					{
						text.Append(ramSizeNewLine ? '\n' : ' ');
					}
					int systemMemorySize = SystemInfo.systemMemorySize;
					if (systemMemorySize > 0)
					{
						text.Append("RAM: ").Append(systemMemorySize).Append(" MB");
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				if (screenData)
				{
					if (flag)
					{
						text.Append(screenDataNewLine ? '\n' : ' ');
					}
					Resolution currentResolution = Screen.currentResolution;
					string value = currentResolution.refreshRateRatio.ToString();
					text.Append("SCR: ").Append(currentResolution.width).Append("x")
						.Append(currentResolution.height)
						.Append("@")
						.Append(value)
						.Append("Hz [window size: ")
						.Append(Screen.width)
						.Append("x")
						.Append(Screen.height);
					float dpi = Screen.dpi;
					if (dpi > 0f)
					{
						text.Append(", DPI: ").Append(dpi).Append("]");
					}
					else
					{
						text.Append("]");
					}
					flag = true;
				}
				if (deviceModel)
				{
					if (flag)
					{
						text.Append(deviceModelNewLine ? '\n' : ' ');
					}
					text.Append("Model: ").Append(SystemInfo.deviceModel);
				}
				LastValue = text.ToString();
				if (main.OperationMode == OperationMode.Normal)
				{
					text.Insert(0, colorCached);
					text.Append("</color>");
					ApplyTextStyles();
				}
				else
				{
					text.Length = 0;
				}
				dirty = true;
			}
		}

		protected override bool HasData()
		{
			if (!cpuModel && !gpuModel && !ramSize)
			{
				return screenData;
			}
			return true;
		}

		protected override void CacheCurrentColor()
		{
			colorCached = "<color=#" + AFPSCounter.Color32ToHex(color) + ">";
		}
	}
}
