using UnityEngine;

namespace Rewired
{
	public struct ElementAssignment
	{
		public ElementAssignmentType type;

		public int elementMapId;

		public int elementIdentifierId;

		public AxisRange axisRange;

		public KeyCode keyboardKey;

		public ModifierKeyFlags modifierKeyFlags;

		public int actionId;

		public Pole axisContribution;

		public bool invert;

		public ElementAssignment(ElementAssignmentType P_0, int P_1, AxisRange P_2, KeyCode P_3, ModifierKeyFlags P_4, int P_5, Pole P_6, bool P_7, int P_8)
		{
			type = P_0;
			elementIdentifierId = P_1;
			axisRange = P_2;
			keyboardKey = P_3;
			modifierKeyFlags = P_4;
			actionId = P_5;
			axisContribution = P_6;
			invert = P_7;
			elementMapId = P_8;
		}

		public ElementAssignment(ControllerType P_0, ControllerElementType P_1, int P_2, AxisRange P_3, KeyCode P_4, ModifierKeyFlags P_5, int P_6, Pole P_7, bool P_8, int P_9)
		{
			type = cVDyIiOsEfJNYzVuZSmuEXqylgT.fSLdSDsBaiDADEvvjkPpAYnbSfWSb(P_0, P_1, P_3);
			elementIdentifierId = P_2;
			axisRange = P_3;
			keyboardKey = P_4;
			modifierKeyFlags = P_5;
			actionId = P_6;
			axisContribution = P_7;
			invert = P_8;
			elementMapId = P_9;
		}

		public ElementAssignment(ElementAssignmentType P_0, int P_1, AxisRange P_2, KeyCode P_3, ModifierKeyFlags P_4, int P_5, Pole P_6, bool P_7)
		{
			type = P_0;
			elementIdentifierId = P_1;
			axisRange = P_2;
			keyboardKey = P_3;
			modifierKeyFlags = P_4;
			actionId = P_5;
			axisContribution = P_6;
			invert = P_7;
			elementMapId = -1;
		}

		public ElementAssignment(ControllerType P_0, ControllerElementType P_1, int P_2, AxisRange P_3, KeyCode P_4, ModifierKeyFlags P_5, int P_6, Pole P_7, bool P_8)
		{
			type = cVDyIiOsEfJNYzVuZSmuEXqylgT.fSLdSDsBaiDADEvvjkPpAYnbSfWSb(P_0, P_1, P_3);
			elementIdentifierId = P_2;
			axisRange = P_3;
			keyboardKey = P_4;
			modifierKeyFlags = P_5;
			actionId = P_6;
			axisContribution = P_7;
			invert = P_8;
			elementMapId = -1;
		}

		public ElementAssignment(int P_0, int P_1, bool P_2)
		{
			type = ElementAssignmentType.FullAxis;
			elementIdentifierId = P_0;
			axisRange = AxisRange.Full;
			keyboardKey = KeyCode.None;
			modifierKeyFlags = ModifierKeyFlags.None;
			actionId = P_1;
			axisContribution = Pole.Positive;
			invert = P_2;
			elementMapId = -1;
		}

		public ElementAssignment(int P_0, int P_1, bool P_2, int P_3)
		{
			type = ElementAssignmentType.FullAxis;
			elementIdentifierId = P_0;
			axisRange = AxisRange.Full;
			keyboardKey = KeyCode.None;
			modifierKeyFlags = ModifierKeyFlags.None;
			actionId = P_1;
			axisContribution = Pole.Positive;
			invert = P_2;
			elementMapId = P_3;
		}

		public ElementAssignment(int P_0, AxisRange P_1, int P_2, Pole P_3)
		{
			type = ElementAssignmentType.SplitAxis;
			elementIdentifierId = P_0;
			axisRange = P_1;
			keyboardKey = KeyCode.None;
			modifierKeyFlags = ModifierKeyFlags.None;
			actionId = P_2;
			axisContribution = P_3;
			invert = false;
			elementMapId = -1;
		}

		public ElementAssignment(int P_0, AxisRange P_1, int P_2, Pole P_3, int P_4)
		{
			type = ElementAssignmentType.SplitAxis;
			elementIdentifierId = P_0;
			axisRange = P_1;
			keyboardKey = KeyCode.None;
			modifierKeyFlags = ModifierKeyFlags.None;
			actionId = P_2;
			axisContribution = P_3;
			invert = false;
			elementMapId = P_4;
		}

		public ElementAssignment(int P_0, int P_1, Pole P_2)
		{
			type = ElementAssignmentType.Button;
			elementIdentifierId = P_0;
			axisRange = AxisRange.Positive;
			keyboardKey = KeyCode.None;
			modifierKeyFlags = ModifierKeyFlags.None;
			actionId = P_1;
			axisContribution = P_2;
			invert = false;
			elementMapId = -1;
		}

		public ElementAssignment(int P_0, int P_1, Pole P_2, int P_3)
		{
			type = ElementAssignmentType.Button;
			elementIdentifierId = P_0;
			axisRange = AxisRange.Positive;
			keyboardKey = KeyCode.None;
			modifierKeyFlags = ModifierKeyFlags.None;
			actionId = P_1;
			axisContribution = P_2;
			invert = false;
			elementMapId = P_3;
		}

		public ElementAssignment(KeyCode P_0, ModifierKeyFlags P_1, int P_2, Pole P_3)
		{
			type = ElementAssignmentType.KeyboardKey;
			elementIdentifierId = Keyboard.MMNqiODvdWTogJjoqPDqgGggOGKA(Keyboard.PwxvwLyqqaZuWBnWTwcdeSZvdrjb(P_0));
			axisRange = AxisRange.Positive;
			keyboardKey = P_0;
			modifierKeyFlags = P_1;
			actionId = P_2;
			axisContribution = P_3;
			invert = false;
			elementMapId = -1;
		}

		public ElementAssignment(KeyCode P_0, ModifierKeyFlags P_1, int P_2, Pole P_3, int P_4)
		{
			type = ElementAssignmentType.KeyboardKey;
			elementIdentifierId = Keyboard.MMNqiODvdWTogJjoqPDqgGggOGKA(Keyboard.PwxvwLyqqaZuWBnWTwcdeSZvdrjb(P_0));
			axisRange = AxisRange.Positive;
			keyboardKey = P_0;
			modifierKeyFlags = P_1;
			actionId = P_2;
			axisContribution = P_3;
			invert = false;
			elementMapId = P_4;
		}

		public static ElementAssignment CompleteAssignment(ElementAssignmentType elementAssignmentType, int elementIdentifierId, AxisRange axisRange, KeyCode keyboardKey, ModifierKeyFlags modifierKeyFlags, int actionId, Pole axisContribution, bool invert, int elementMapId)
		{
			return new ElementAssignment(elementAssignmentType, elementIdentifierId, axisRange, keyboardKey, modifierKeyFlags, actionId, axisContribution, invert, elementMapId);
		}

		public static ElementAssignment CompleteAssignment(ControllerType controllerType, ControllerElementType elementType, int elementIdentifierId, AxisRange axisRange, KeyCode keyboardKey, ModifierKeyFlags modifierKeyFlags, int actionId, Pole axisContribution, bool invert, int elementMapId)
		{
			return new ElementAssignment(controllerType, elementType, elementIdentifierId, axisRange, keyboardKey, modifierKeyFlags, actionId, axisContribution, invert, elementMapId);
		}

		public static ElementAssignment CompleteAssignment(ElementAssignmentType elementAssignmentType, int elementIdentifierId, AxisRange axisRange, KeyCode keyboardKey, ModifierKeyFlags modifierKeyFlags, int actionId, Pole axisContribution, bool invert)
		{
			return new ElementAssignment(elementAssignmentType, elementIdentifierId, axisRange, keyboardKey, modifierKeyFlags, actionId, axisContribution, invert);
		}

		public static ElementAssignment CompleteAssignment(ControllerType controllerType, ControllerElementType elementType, int elementIdentifierId, AxisRange axisRange, KeyCode keyboardKey, ModifierKeyFlags modifierKeyFlags, int actionId, Pole axisContribution, bool invert)
		{
			return new ElementAssignment(controllerType, elementType, elementIdentifierId, axisRange, keyboardKey, modifierKeyFlags, actionId, axisContribution, invert);
		}

		public static ElementAssignment FullAxisAssignment(int elementIdentifierId, int actionId, bool invert)
		{
			return new ElementAssignment(elementIdentifierId, actionId, invert);
		}

		public static ElementAssignment FullAxisAssignment(int elementIdentifierId, int actionId, bool invert, int elementMapId)
		{
			return new ElementAssignment(elementIdentifierId, actionId, invert, elementMapId);
		}

		public static ElementAssignment SplitAxisAssignment(int elementIdentifierId, AxisRange axisRange, int actionId, Pole axisContribution)
		{
			return new ElementAssignment(elementIdentifierId, axisRange, actionId, axisContribution);
		}

		public static ElementAssignment SplitAxisAssignment(int elementIdentifierId, AxisRange axisRange, int actionId, Pole axisContribution, int elementMapId)
		{
			return new ElementAssignment(elementIdentifierId, axisRange, actionId, axisContribution, elementMapId);
		}

		public static ElementAssignment ButtonAssignment(int elementIdentifierId, int actionId, Pole axisContribution)
		{
			return new ElementAssignment(elementIdentifierId, actionId, axisContribution);
		}

		public static ElementAssignment ButtonAssignment(int elementIdentifierId, int actionId, Pole axisContribution, int elementMapId)
		{
			return new ElementAssignment(elementIdentifierId, actionId, axisContribution, elementMapId);
		}

		public static ElementAssignment KeyboardKeyAssignment(KeyCode keyboardKey, ModifierKeyFlags modifierKeyFlags, int actionId, Pole axisContribution)
		{
			return new ElementAssignment(keyboardKey, modifierKeyFlags, actionId, axisContribution);
		}

		public static ElementAssignment KeyboardKeyAssignment(KeyCode keyboardKey, ModifierKeyFlags modifierKeyFlags, int actionId, Pole axisContribution, int elementMapId)
		{
			return new ElementAssignment(keyboardKey, modifierKeyFlags, actionId, axisContribution, elementMapId);
		}

		public ElementAssignmentConflictCheck ToElementAssignmentConflictCheck()
		{
			return new ElementAssignmentConflictCheck
			{
				playerId = -1,
				controllerType = ControllerType.Keyboard,
				controllerId = -1,
				controllerMapId = -1,
				controllerMapCategoryId = -1,
				elementAssignmentType = type,
				elementIdentifierId = elementIdentifierId,
				axisRange = axisRange,
				keyboardKey = keyboardKey,
				modifierKeyFlags = modifierKeyFlags,
				actionId = actionId,
				axisContribution = axisContribution,
				invert = invert,
				elementMapId = elementMapId
			};
		}
	}
}
