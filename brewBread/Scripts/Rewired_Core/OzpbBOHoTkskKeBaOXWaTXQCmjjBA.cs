using System;
using Rewired;

internal static class OzpbBOHoTkskKeBaOXWaTXQCmjjBA
{
	public static ControllerElementType jabYOprZCpTijtFrYngdXNagQtNF(ElementAssignmentType P_0)
	{
		switch (P_0)
		{
		case ElementAssignmentType.Button:
		case ElementAssignmentType.KeyboardKey:
			return ControllerElementType.Button;
		case ElementAssignmentType.FullAxis:
		case ElementAssignmentType.SplitAxis:
			return ControllerElementType.Axis;
		default:
			throw new NotImplementedException();
		}
	}

	public static ElementAssignmentType MlfBuKtnYXMPsXZFSLJhadIQfWdh(ControllerType P_0, ControllerElementType P_1, AxisRange P_2)
	{
		if (P_0 == ControllerType.Keyboard)
		{
			return ElementAssignmentType.KeyboardKey;
		}
		switch (P_1)
		{
		case ControllerElementType.Axis:
			if (P_2 == AxisRange.Full)
			{
				return ElementAssignmentType.FullAxis;
			}
			return ElementAssignmentType.SplitAxis;
		case ControllerElementType.Button:
			return ElementAssignmentType.Button;
		default:
			throw new NotImplementedException();
		}
	}

	public static AxisRange EOYDATlPnYaJjEbtjqAObSEcZcrvb(Pole P_0)
	{
		return P_0 switch
		{
			Pole.Positive => AxisRange.Positive, 
			Pole.Negative => AxisRange.Negative, 
			_ => throw new NotImplementedException(), 
		};
	}

	public static Type jKoGbyqeUMyjlkewXqNqxnMpXIUc<_0001>() where _0001 : Controller
	{
		return jKoGbyqeUMyjlkewXqNqxnMpXIUc(typeof(_0001));
	}

	public static Type jKoGbyqeUMyjlkewXqNqxnMpXIUc(Type P_0)
	{
		if ((object)P_0 == typeof(Joystick))
		{
			return typeof(JoystickMap);
		}
		if ((object)P_0 == typeof(Keyboard))
		{
			return typeof(KeyboardMap);
		}
		if ((object)P_0 == typeof(Mouse))
		{
			return typeof(MouseMap);
		}
		if ((object)P_0 == typeof(CustomController))
		{
			return typeof(CustomControllerMap);
		}
		if ((object)P_0 == typeof(Controller))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		if ((object)P_0 == typeof(ControllerWithMap))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		if ((object)P_0 == typeof(ControllerWithAxes))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		throw new NotImplementedException();
	}

	public static Type LhVOjvrBeQHintIDcnRYIHmQmKeN(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => typeof(JoystickMap), 
			ControllerType.Keyboard => typeof(KeyboardMap), 
			ControllerType.Mouse => typeof(MouseMap), 
			ControllerType.Custom => typeof(CustomControllerMap), 
			_ => throw new NotImplementedException(), 
		};
	}

	public static Type GuyVrDmezYhXUNhodBLxzPEFQlDV(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => typeof(Joystick), 
			ControllerType.Keyboard => typeof(Keyboard), 
			ControllerType.Mouse => typeof(Mouse), 
			ControllerType.Custom => typeof(CustomController), 
			_ => throw new NotImplementedException(), 
		};
	}

	public static ControllerType VhaDZGHEgxAjAETVDwHyLGOhLzSgA(Type P_0)
	{
		if ((object)P_0 == null)
		{
			throw new ArgumentNullException("controllerType");
		}
		if ((object)P_0 == typeof(Joystick))
		{
			return ControllerType.Joystick;
		}
		if ((object)P_0 == typeof(Keyboard))
		{
			return ControllerType.Keyboard;
		}
		if ((object)P_0 == typeof(Mouse))
		{
			return ControllerType.Mouse;
		}
		if ((object)P_0 == typeof(CustomController))
		{
			return ControllerType.Custom;
		}
		if ((object)P_0 == typeof(Controller))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		if ((object)P_0 == typeof(ControllerWithMap))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		if ((object)P_0 == typeof(ControllerWithAxes))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		throw new NotImplementedException();
	}

	public static ControllerType VhaDZGHEgxAjAETVDwHyLGOhLzSgA<_0001>()
	{
		return VhaDZGHEgxAjAETVDwHyLGOhLzSgA(typeof(_0001));
	}

	public static ControllerType xnWFdNaGqPTPKeFghcsipBMLpagCA(Type P_0)
	{
		if (!deDOsGjupTEdOubjOAOFyeWFClujA(P_0, out var result))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		return result;
	}

	public static ControllerType xnWFdNaGqPTPKeFghcsipBMLpagCA<_0001>() where _0001 : ControllerMap
	{
		return xnWFdNaGqPTPKeFghcsipBMLpagCA(typeof(_0001));
	}

	public static bool deDOsGjupTEdOubjOAOFyeWFClujA(Type P_0, out ControllerType P_1)
	{
		if ((object)P_0 == null)
		{
			throw new ArgumentNullException("mapType");
		}
		if ((object)P_0 == typeof(JoystickMap))
		{
			P_1 = ControllerType.Joystick;
			return true;
		}
		if ((object)P_0 == typeof(KeyboardMap))
		{
			P_1 = ControllerType.Keyboard;
			return true;
		}
		if ((object)P_0 == typeof(MouseMap))
		{
			P_1 = ControllerType.Mouse;
			return true;
		}
		if ((object)P_0 == typeof(CustomControllerMap))
		{
			P_1 = ControllerType.Custom;
			return true;
		}
		if ((object)P_0 == typeof(ControllerMap))
		{
			P_1 = ControllerType.Keyboard;
			return false;
		}
		if ((object)P_0 == typeof(ControllerMapWithAxes))
		{
			P_1 = ControllerType.Keyboard;
			return false;
		}
		throw new NotImplementedException();
	}

	public static bool deDOsGjupTEdOubjOAOFyeWFClujA<_0001>(out ControllerType P_0) where _0001 : ControllerMap
	{
		return deDOsGjupTEdOubjOAOFyeWFClujA(typeof(_0001), out P_0);
	}

	public static ControllerType bECSBdaBGebtALsMyTrdSOnWXhDU(Type P_0)
	{
		if ((object)P_0 == null)
		{
			throw new ArgumentNullException("controllerMapSaveDataType");
		}
		if ((object)P_0 == typeof(JoystickMapSaveData))
		{
			return ControllerType.Joystick;
		}
		if ((object)P_0 == typeof(KeyboardMapSaveData))
		{
			return ControllerType.Keyboard;
		}
		if ((object)P_0 == typeof(MouseMapSaveData))
		{
			return ControllerType.Mouse;
		}
		if ((object)P_0 == typeof(CustomControllerMapSaveData))
		{
			return ControllerType.Custom;
		}
		if ((object)P_0 == typeof(ControllerMapSaveData))
		{
			throw new Exception(P_0.Name + " is not an allowed type.");
		}
		throw new NotImplementedException();
	}

	public static ControllerType bECSBdaBGebtALsMyTrdSOnWXhDU<_0001>() where _0001 : ControllerMapSaveData
	{
		return bECSBdaBGebtALsMyTrdSOnWXhDU(typeof(_0001));
	}

	public static bool dRTWaRkWIkEMLraxMRHrbdGkCqGCA(ControllerTemplateElementType P_0, ControllerElementType P_1)
	{
		return P_1 switch
		{
			ControllerElementType.Axis => P_0 == ControllerTemplateElementType.Axis, 
			ControllerElementType.Button => P_0 == ControllerTemplateElementType.Button, 
			ControllerElementType.CompoundElement => false, 
			_ => throw new NotImplementedException(), 
		};
	}

	public static ControllerElementType KNuCnQKVqwmTEfKoAFrvIYCKysDe(object P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("type");
		}
		Type type = P_0.GetType();
		if ((object)type == typeof(ControllerElementType))
		{
			return (ControllerElementType)P_0;
		}
		if ((object)type == typeof(ControllerTemplateElementType))
		{
			return KNuCnQKVqwmTEfKoAFrvIYCKysDe((ControllerTemplateElementType)P_0);
		}
		throw new NotImplementedException();
	}

	public static ControllerElementType KNuCnQKVqwmTEfKoAFrvIYCKysDe(ControllerTemplateElementType P_0)
	{
		return P_0 switch
		{
			ControllerTemplateElementType.Axis => ControllerElementType.Axis, 
			ControllerTemplateElementType.Button => ControllerElementType.Button, 
			_ => throw new NotImplementedException(), 
		};
	}

	public static ControllerTemplateElementSourceType oDUYYDBtfTaUYefFeWLVhqWxwMSv(ControllerTemplateElementType P_0, bool P_1)
	{
		switch (P_0)
		{
		case ControllerTemplateElementType.Axis:
			return ControllerTemplateElementSourceType.Axis;
		case ControllerTemplateElementType.Button:
			return ControllerTemplateElementSourceType.Button;
		default:
			if (P_1)
			{
				throw new NotImplementedException();
			}
			return (ControllerTemplateElementSourceType)(-1);
		}
	}

	public static ControllerTemplateElementType yYbzPczOHmktyhazvMykohFtarNh(ControllerElementType P_0, bool P_1)
	{
		switch (P_0)
		{
		case ControllerElementType.Axis:
			return ControllerTemplateElementType.Axis;
		case ControllerElementType.Button:
			return ControllerTemplateElementType.Button;
		default:
			if (P_1)
			{
				throw new NotImplementedException();
			}
			return (ControllerTemplateElementType)(-1);
		}
	}
}
