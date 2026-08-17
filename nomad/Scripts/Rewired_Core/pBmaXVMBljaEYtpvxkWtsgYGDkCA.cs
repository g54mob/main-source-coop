using Rewired;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

internal sealed class pBmaXVMBljaEYtpvxkWtsgYGDkCA : IElementIdentifierTool
{
	private GUIText PdoXojVGpEfqxzlnCUYSSEMdsYHv;

	private string bFtCLAbXNHBEgIMttmHbgezDemIPA;

	private int IChgqncBarIukWyuaFsUNcytJjlc = 1;

	public void Initialize(GUIText text)
	{
		PdoXojVGpEfqxzlnCUYSSEMdsYHv = text;
	}

	void IElementIdentifierTool.Initialize(GUIText text)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Initialize
		this.Initialize(text);
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

	void IElementIdentifierTool.Start()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Start
		this.Start();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			IChgqncBarIukWyuaFsUNcytJjlc++;
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
		{
			IChgqncBarIukWyuaFsUNcytJjlc--;
		}
		if (IChgqncBarIukWyuaFsUNcytJjlc <= 0)
		{
			IChgqncBarIukWyuaFsUNcytJjlc = 16;
		}
		else if (IChgqncBarIukWyuaFsUNcytJjlc > 16)
		{
			IChgqncBarIukWyuaFsUNcytJjlc = 1;
		}
		bFtCLAbXNHBEgIMttmHbgezDemIPA = "Unity Joystick Element Identifier:\n\n";
		string[] joystickNames = Input.GetJoystickNames();
		if (joystickNames.Length != 0)
		{
			bFtCLAbXNHBEgIMttmHbgezDemIPA += "Connected joysticks:\n";
		}
		else
		{
			bFtCLAbXNHBEgIMttmHbgezDemIPA += "No joysticks detected.\n";
		}
		for (int i = 0; i < joystickNames.Length; i++)
		{
			bFtCLAbXNHBEgIMttmHbgezDemIPA = bFtCLAbXNHBEgIMttmHbgezDemIPA + "[" + i + "] \"" + joystickNames[i] + "\"";
			if (UnityTools.platform == Platform.Linux && UnityTools.externalTools.LinuxInput_IsJoystickPreconfigured(joystickNames[i]))
			{
				bFtCLAbXNHBEgIMttmHbgezDemIPA += " [UNITY PRE-CONFIGURED]";
			}
			bFtCLAbXNHBEgIMttmHbgezDemIPA += "\n";
		}
		bFtCLAbXNHBEgIMttmHbgezDemIPA += "\n";
		bFtCLAbXNHBEgIMttmHbgezDemIPA = bFtCLAbXNHBEgIMttmHbgezDemIPA + "Current Unity Joystick Id: " + IChgqncBarIukWyuaFsUNcytJjlc + "\n";
		bFtCLAbXNHBEgIMttmHbgezDemIPA += "(Press + or - to change monitored joystick id.)\n\n";
		for (int j = 0; j < 29; j++)
		{
			string text = "Axis " + j;
			float joystickAxisValueByJoystickId = UnityInputHelper.GetJoystickAxisValueByJoystickId(IChgqncBarIukWyuaFsUNcytJjlc, j);
			YREaEOVLidmWABVXyBvRPqUMDCfDA(text, joystickAxisValueByJoystickId);
		}
		for (int k = 0; k < 20; k++)
		{
			string text2 = "Button " + k;
			bool joystickButtonValueByJoystickId = UnityInputHelper.GetJoystickButtonValueByJoystickId(IChgqncBarIukWyuaFsUNcytJjlc, k);
			YREaEOVLidmWABVXyBvRPqUMDCfDA(text2, joystickButtonValueByJoystickId);
		}
		PdoXojVGpEfqxzlnCUYSSEMdsYHv.text = bFtCLAbXNHBEgIMttmHbgezDemIPA;
	}

	void IElementIdentifierTool.Update()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Update
		this.Update();
	}

	public void OnDestroy()
	{
	}

	void IElementIdentifierTool.OnDestroy()
	{
		//ILSpy generated this explicit interface implementation from .override directive in OnDestroy
		this.OnDestroy();
	}

	private void YREaEOVLidmWABVXyBvRPqUMDCfDA(string P_0, object P_1)
	{
		bFtCLAbXNHBEgIMttmHbgezDemIPA = bFtCLAbXNHBEgIMttmHbgezDemIPA + P_0 + " = " + P_1.ToString() + "\n";
	}
}
