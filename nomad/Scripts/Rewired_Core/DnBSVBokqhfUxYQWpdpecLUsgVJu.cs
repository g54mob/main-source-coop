using System;
using Rewired;
using Rewired.Platforms.Custom;

internal abstract class DnBSVBokqhfUxYQWpdpecLUsgVJu : IDisposable
{
	protected readonly CustomPlatformUnifiedControllerSource SvHZjtqIiXxvvwIkbIwIaqFZgvtX;

	private readonly HardwareControllerMap_Game YzDSdnQeSyYzoPteoLbbwcnOlFup;

	private bool LwflRkjUlHkCNTcMOpLsXGuJjDFy;

	private bool UYIhbNCxAiGfTxYCIKWkHBDgsrsP;

	public InputSource inputSource => InputSource.Custom;

	public HardwareControllerMap_Game hardwareMap => YzDSdnQeSyYzoPteoLbbwcnOlFup;

	public int axisCount => SvHZjtqIiXxvvwIkbIwIaqFZgvtX.axisCount;

	public int buttonCount => SvHZjtqIiXxvvwIkbIwIaqFZgvtX.buttonCount;

	public Controller.Extension controllerExtension => SvHZjtqIiXxvvwIkbIwIaqFZgvtX.controllerExtension;

	public DnBSVBokqhfUxYQWpdpecLUsgVJu(CustomPlatformUnifiedControllerSource P_0, HardwareControllerMap_Game P_1)
	{
		SvHZjtqIiXxvvwIkbIwIaqFZgvtX = P_0;
		YzDSdnQeSyYzoPteoLbbwcnOlFup = P_1;
	}

	public void Clear()
	{
		SvHZjtqIiXxvvwIkbIwIaqFZgvtX.ciMcgCoDTiPEvKMPuLFgSXnANuBF();
	}

	public void UpdateInputData(ControllerDataUpdater dataUpdater)
	{
		SvHZjtqIiXxvvwIkbIwIaqFZgvtX.PQAOuAbNmOhBwIbRhIASuNkNGPE(dataUpdater);
	}

	public void rXYxKYFdGeHSghPTzTsaFHJPaVXcb()
	{
		SvHZjtqIiXxvvwIkbIwIaqFZgvtX.pOiaNcIMMZahjlhCneHRJBdSieLf();
	}

	protected virtual void DhdbgzhCiWYlcJVpoPGbHbErDrDn(bool P_0)
	{
		if (!UYIhbNCxAiGfTxYCIKWkHBDgsrsP)
		{
			if (P_0 && SvHZjtqIiXxvvwIkbIwIaqFZgvtX != null)
			{
				((IDisposable)SvHZjtqIiXxvvwIkbIwIaqFZgvtX).Dispose();
			}
			UYIhbNCxAiGfTxYCIKWkHBDgsrsP = true;
		}
	}

	void IDisposable.Dispose()
	{
		DhdbgzhCiWYlcJVpoPGbHbErDrDn(true);
		GC.SuppressFinalize(this);
	}
}
