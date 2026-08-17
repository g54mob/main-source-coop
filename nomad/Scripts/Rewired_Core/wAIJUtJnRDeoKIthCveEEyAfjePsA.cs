using System;
using Rewired;

internal static class wAIJUtJnRDeoKIthCveEEyAfjePsA
{
	public static cBFxQChnAZFRRQeDStCHagOAAZyI EvNqTyCxDnMyOBrFuMukHLvYIQjU(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Mouse => cBFxQChnAZFRRQeDStCHagOAAZyI.Mouse, 
			ControllerType.Keyboard => cBFxQChnAZFRRQeDStCHagOAAZyI.Keyboard, 
			ControllerType.Joystick => cBFxQChnAZFRRQeDStCHagOAAZyI.Joystick, 
			ControllerType.Custom => cBFxQChnAZFRRQeDStCHagOAAZyI.CustomController, 
			_ => throw new NotImplementedException(), 
		};
	}
}
