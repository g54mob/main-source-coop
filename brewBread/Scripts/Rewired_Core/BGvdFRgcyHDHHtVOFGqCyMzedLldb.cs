using Rewired;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

internal sealed class BGvdFRgcyHDHHtVOFGqCyMzedLldb : IElementIdentifierTool
{
	private Rewired.Internal.GUIText IerMiUUvPDBiuZgBuJMKvaqrbiYi;

	private string oPxRsZFHllTNsGPJzfYSqXTwAMvFA;

	private int aLugVPjZaDOafZAHTSUXcgVswsWG = 1;

	public void Initialize(Rewired.Internal.GUIText text)
	{
		IerMiUUvPDBiuZgBuJMKvaqrbiYi = text;
	}

	public void Start()
	{
		string[] joystickNames = Input.GetJoystickNames();
		string text = "Detected " + joystickNames.Length + " attached joysticks";
		if (joystickNames.Length != 0)
		{
			text += ":\n";
		}
		string[] array = joystickNames;
		foreach (string text2 in array)
		{
			text = text + "\"" + text2 + "\"\n";
		}
		Rewired.Logger.Log(text);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			aLugVPjZaDOafZAHTSUXcgVswsWG++;
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
		{
			aLugVPjZaDOafZAHTSUXcgVswsWG--;
		}
		if (aLugVPjZaDOafZAHTSUXcgVswsWG <= 0)
		{
			aLugVPjZaDOafZAHTSUXcgVswsWG = 16;
		}
		else if (aLugVPjZaDOafZAHTSUXcgVswsWG > 16)
		{
			aLugVPjZaDOafZAHTSUXcgVswsWG = 1;
		}
		oPxRsZFHllTNsGPJzfYSqXTwAMvFA = "Unity Joystick Element Identifier:\n\n";
		string[] joystickNames = Input.GetJoystickNames();
		if (joystickNames.Length != 0)
		{
			oPxRsZFHllTNsGPJzfYSqXTwAMvFA += "Connected joysticks:\n";
		}
		else
		{
			oPxRsZFHllTNsGPJzfYSqXTwAMvFA += "No joysticks detected.\n";
		}
		for (int i = 0; i < joystickNames.Length; i++)
		{
			oPxRsZFHllTNsGPJzfYSqXTwAMvFA = oPxRsZFHllTNsGPJzfYSqXTwAMvFA + "[" + i + "] \"" + joystickNames[i] + "\"";
			if (UnityTools.platform == Platform.Linux && UnityTools.externalTools.LinuxInput_IsJoystickPreconfigured(joystickNames[i]))
			{
				oPxRsZFHllTNsGPJzfYSqXTwAMvFA += " [UNITY PRE-CONFIGURED]";
			}
			oPxRsZFHllTNsGPJzfYSqXTwAMvFA += "\n";
		}
		oPxRsZFHllTNsGPJzfYSqXTwAMvFA += "\n";
		oPxRsZFHllTNsGPJzfYSqXTwAMvFA = oPxRsZFHllTNsGPJzfYSqXTwAMvFA + "Current Unity Joystick Id: " + aLugVPjZaDOafZAHTSUXcgVswsWG + "\n";
		oPxRsZFHllTNsGPJzfYSqXTwAMvFA += "(Press + or - to change monitored joystick id.)\n\n";
		for (int j = 0; j < 29; j++)
		{
			string text = "Axis " + j;
			float joystickAxisValueByJoystickId = UnityInputHelper.GetJoystickAxisValueByJoystickId(aLugVPjZaDOafZAHTSUXcgVswsWG, j);
			bCwNtcqVAAwgUPXpdtVSuzPDEvrH(text, joystickAxisValueByJoystickId);
		}
		for (int k = 0; k < 20; k++)
		{
			string text2 = "Button " + k;
			bool joystickButtonValueByJoystickId = UnityInputHelper.GetJoystickButtonValueByJoystickId(aLugVPjZaDOafZAHTSUXcgVswsWG, k);
			bCwNtcqVAAwgUPXpdtVSuzPDEvrH(text2, joystickButtonValueByJoystickId);
		}
		IerMiUUvPDBiuZgBuJMKvaqrbiYi.text = oPxRsZFHllTNsGPJzfYSqXTwAMvFA;
	}

	public void OnDestroy()
	{
	}

	private void bCwNtcqVAAwgUPXpdtVSuzPDEvrH(string P_0, object P_1)
	{
		oPxRsZFHllTNsGPJzfYSqXTwAMvFA = oPxRsZFHllTNsGPJzfYSqXTwAMvFA + P_0 + " = " + P_1.ToString() + "\n";
	}
}
