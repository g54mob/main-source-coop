using System;
using Rewired.Config;
using Rewired.Utils;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class ButtonLoopSet : UpdateLoopDataSet<ButtonLoopSet.ButtonData>
	{
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public class ButtonData
		{
			public readonly UpdateLoopType updateLoop;

			public readonly bool[] values;

			public readonly bool[] wasTrueThisFrame;

			private bool[] ITnovbzHVUUHyzqSwRWuOqohgXsE;

			private int QIYgwfJXSeBaxHYWuOlHCzYnBQVCb;

			private readonly bool[] vFYJHuCFCwhKzwMdaiSgkuiFHyuNA;

			private readonly bool[] nZMRaOvgiSEmiVSSAROfPsIsadPBA;

			public bool[] effectiveValue
			{
				get
				{
					if (updateLoop == UpdateLoopType.FixedUpdate)
					{
						dJDgotqKtBrajsCvldZOHIYLSLXaA();
					}
					return ITnovbzHVUUHyzqSwRWuOqohgXsE;
				}
			}

			public ButtonData(int P_0, UpdateLoopType P_1)
			{
				updateLoop = P_1;
				values = new bool[P_0];
				nZMRaOvgiSEmiVSSAROfPsIsadPBA = new bool[P_0];
				wasTrueThisFrame = new bool[P_0];
				vFYJHuCFCwhKzwMdaiSgkuiFHyuNA = new bool[P_0];
				ITnovbzHVUUHyzqSwRWuOqohgXsE = new bool[P_0];
				QIYgwfJXSeBaxHYWuOlHCzYnBQVCb = ReInput.timeScalePauseChangedCount;
			}

			public void SetValue(int index, bool value)
			{
				if (updateLoop == UpdateLoopType.FixedUpdate)
				{
					dJDgotqKtBrajsCvldZOHIYLSLXaA();
				}
				values[index] = value;
				if (value)
				{
					wasTrueThisFrame[index] = true;
					if (!nZMRaOvgiSEmiVSSAROfPsIsadPBA[index])
					{
						vFYJHuCFCwhKzwMdaiSgkuiFHyuNA[index] = true;
					}
				}
				ITnovbzHVUUHyzqSwRWuOqohgXsE[index] = value | vFYJHuCFCwhKzwMdaiSgkuiFHyuNA[index];
				nZMRaOvgiSEmiVSSAROfPsIsadPBA[index] = value;
			}

			public void ClearWasTrueThisFrame()
			{
				for (int i = 0; i < values.Length; i++)
				{
					wasTrueThisFrame[i] = false;
					vFYJHuCFCwhKzwMdaiSgkuiFHyuNA[i] = false;
					ITnovbzHVUUHyzqSwRWuOqohgXsE[i] = values[i];
				}
			}

			public void Clear()
			{
				Array.Clear(values, 0, values.Length);
				Array.Clear(nZMRaOvgiSEmiVSSAROfPsIsadPBA, 0, values.Length);
				Array.Clear(wasTrueThisFrame, 0, wasTrueThisFrame.Length);
				Array.Clear(vFYJHuCFCwhKzwMdaiSgkuiFHyuNA, 0, vFYJHuCFCwhKzwMdaiSgkuiFHyuNA.Length);
				Array.Clear(ITnovbzHVUUHyzqSwRWuOqohgXsE, 0, ITnovbzHVUUHyzqSwRWuOqohgXsE.Length);
				QIYgwfJXSeBaxHYWuOlHCzYnBQVCb = ReInput.timeScalePauseChangedCount;
			}

			public void Import(ButtonData source)
			{
				if (source != null)
				{
					int num = MathTools.Min(values.Length, source.values.Length);
					for (int i = 0; i < num; i++)
					{
						values[i] = source.values[i];
						nZMRaOvgiSEmiVSSAROfPsIsadPBA[i] = source.nZMRaOvgiSEmiVSSAROfPsIsadPBA[i];
						wasTrueThisFrame[i] = source.wasTrueThisFrame[i];
						vFYJHuCFCwhKzwMdaiSgkuiFHyuNA[i] = source.vFYJHuCFCwhKzwMdaiSgkuiFHyuNA[i];
						ITnovbzHVUUHyzqSwRWuOqohgXsE[i] = source.ITnovbzHVUUHyzqSwRWuOqohgXsE[i];
						QIYgwfJXSeBaxHYWuOlHCzYnBQVCb = source.QIYgwfJXSeBaxHYWuOlHCzYnBQVCb;
					}
				}
			}

			private void dJDgotqKtBrajsCvldZOHIYLSLXaA()
			{
				if (ReInput.timeScalePauseChangedCount != QIYgwfJXSeBaxHYWuOlHCzYnBQVCb)
				{
					ClearWasTrueThisFrame();
					QIYgwfJXSeBaxHYWuOlHCzYnBQVCb = ReInput.timeScalePauseChangedCount;
				}
			}
		}

		public readonly int buttonCount;

		public ButtonLoopSet(UpdateLoopSetting P_0, int P_1)
			: base(P_0)
		{
			buttonCount = P_1;
			for (int i = 0; i < base.Count; i++)
			{
				base[i] = new ButtonData(P_1, GetUpdateLoopType(i));
			}
		}

		public void SetValue(int index, bool value, double timestamp)
		{
			int count = base.Count;
			for (int i = 0; i < count; i++)
			{
				base[i].SetValue(index, value);
			}
		}

		public void Clear()
		{
			int count = base.Count;
			for (int i = 0; i < count; i++)
			{
				base[i].Clear();
			}
		}

		public void Import(ButtonLoopSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}
			if (set.buttonCount != buttonCount)
			{
				throw new Exception("Cannot import from a set with a different button count.");
			}
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Import(set[i]);
			}
		}
	}
}
