using System;
using System.Collections.Generic;
using System.Text;
using Rewired.Internal.Localization;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public sealed class ActionElementMap
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal int _actionCategoryId;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal int _actionId;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal ControllerElementType _elementType;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal int _elementIdentifierId;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal AxisRange _axisRange;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal bool _invert;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal Pole _axisContribution;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal KeyboardKeyCode _keyboardKeyCode;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal ModifierKey _modifierKey1;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal ModifierKey _modifierKey2;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		internal ModifierKey _modifierKey3;

		[NonSerialized]
		internal ControllerMap LPxrnOpGfQRCZcQMNZvgstOaWZoi;

		[NonSerialized]
		internal bool uPyFcaFdRzKajesnqkOUtFvpIRKHA = true;

		[NonSerialized]
		internal int uxemeTqImFAncCLpTkOkfOWaWKUK;

		[NonSerialized]
		internal readonly int nJilCjIhFvMUTsTBcUWuYpormNsu;

		[NonSerialized]
		private uint MerbNRWLFPJrqZGWIaGALXprFQOb;

		[NonSerialized]
		private string xcZrlmieAJsAZzNmAqaOwTkoCqKH;

		[NonSerialized]
		private string kDzHkcbkAQhbWaliMPTQMvDlBdIiA;

		[NonSerialized]
		private ModifierKeyFlags qdDxobyQabJFkCVUGVIkdujrbZddA;

		[NonSerialized]
		private HardwareControllerMap_Game ykljKPhmOIBOELjZHCuDAUUKdbcg;

		[NonSerialized]
		private double tSchxrtFtMvMZinfMqyLGKsoUFUL;

		private static int uidCounter;

		private static StringBuilder s_toStringSB;

		public int actionId
		{
			get
			{
				return _actionId;
			}
			set
			{
				if (value != _actionId)
				{
					_actionId = value;
					mcFJwsoNSHHvPNLkGxwFTlHQondm();
				}
			}
		}

		public ControllerElementType elementType
		{
			get
			{
				return _elementType;
			}
			internal set
			{
				_elementType = controllerElementType;
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public int elementIdentifierId
		{
			get
			{
				return _elementIdentifierId;
			}
			set
			{
				if (_elementIdentifierId == value)
				{
					return;
				}
				_elementIdentifierId = value;
				if (ReInput.isReady && LPxrnOpGfQRCZcQMNZvgstOaWZoi != null)
				{
					Controller controller = ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.QaXmawDzDviOFGGVAiudAlfjSkMM(LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType, LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerId, true);
					if (controller != null)
					{
						Controller.Element elementById = controller.GetElementById(value);
						if (elementById != null && elementById.type != _elementType)
						{
							LPxrnOpGfQRCZcQMNZvgstOaWZoi.ZVdtyfwTyDwVTuIonGIxjptiyrFA(nJilCjIhFvMUTsTBcUWuYpormNsu, elementById.type);
						}
					}
				}
				if (ReInput.isReady)
				{
					chAwkTluIJWbDVjKaUSgcyQoljPg(false);
				}
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public AxisRange axisRange
		{
			get
			{
				return _axisRange;
			}
			set
			{
				if (_axisRange == value)
				{
					return;
				}
				if (_elementType != ControllerElementType.Axis && ReInput.isReady)
				{
					Logger.LogWarning("You cannot change AxisRange of a non-Axis mapping.");
					return;
				}
				_axisRange = value;
				if (ReInput.isReady)
				{
					chAwkTluIJWbDVjKaUSgcyQoljPg(false);
				}
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public bool invert
		{
			get
			{
				return _invert;
			}
			set
			{
				_invert = value;
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public Pole axisContribution
		{
			get
			{
				return _axisContribution;
			}
			set
			{
				if (_axisContribution != value)
				{
					_axisContribution = value;
					if (ReInput.isReady)
					{
						chAwkTluIJWbDVjKaUSgcyQoljPg(false);
					}
					mcFJwsoNSHHvPNLkGxwFTlHQondm();
				}
			}
		}

		public KeyboardKeyCode keyboardKeyCode
		{
			get
			{
				return _keyboardKeyCode;
			}
			set
			{
				if (_keyboardKeyCode == value)
				{
					return;
				}
				if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType != ControllerType.Keyboard)
				{
					Logger.LogWarning("You cannot set the key code on a non-Keyboard mapping.");
					return;
				}
				_keyboardKeyCode = value;
				if (ReInput.isReady)
				{
					chAwkTluIJWbDVjKaUSgcyQoljPg(true);
				}
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public ModifierKey modifierKey1
		{
			get
			{
				return _modifierKey1;
			}
			set
			{
				if (_modifierKey1 == value)
				{
					return;
				}
				if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType != ControllerType.Keyboard)
				{
					Logger.LogWarning("You cannot set a modifier key on a non-Keyboard mapping.");
					return;
				}
				_modifierKey1 = value;
				if (ReInput.isReady)
				{
					atHaTMlRLxKJeURaFtHVAnPqTnqV();
					chAwkTluIJWbDVjKaUSgcyQoljPg(true);
				}
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public ModifierKey modifierKey2
		{
			get
			{
				return _modifierKey2;
			}
			set
			{
				if (_modifierKey2 == value)
				{
					return;
				}
				if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType != ControllerType.Keyboard)
				{
					Logger.LogWarning("You cannot set a modifier key on a non-Keyboard mapping.");
					return;
				}
				_modifierKey2 = value;
				if (ReInput.isReady)
				{
					atHaTMlRLxKJeURaFtHVAnPqTnqV();
					chAwkTluIJWbDVjKaUSgcyQoljPg(true);
				}
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public ModifierKey modifierKey3
		{
			get
			{
				return _modifierKey3;
			}
			set
			{
				if (_modifierKey3 == value)
				{
					return;
				}
				if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType != ControllerType.Keyboard)
				{
					Logger.LogWarning("You cannot set a modifier key on a non-Keyboard mapping.");
					return;
				}
				_modifierKey3 = value;
				if (ReInput.isReady)
				{
					atHaTMlRLxKJeURaFtHVAnPqTnqV();
					chAwkTluIJWbDVjKaUSgcyQoljPg(true);
				}
				mcFJwsoNSHHvPNLkGxwFTlHQondm();
			}
		}

		public AxisType axisType
		{
			get
			{
				if (_elementType != ControllerElementType.Axis)
				{
					return AxisType.None;
				}
				if (_axisRange == AxisRange.Full)
				{
					return AxisType.Normal;
				}
				return AxisType.Split;
			}
		}

		public ModifierKeyFlags modifierKeyFlags => ModifierKeyFlags.None | Keyboard.ModifierKeyToModifierKeyFlags(_modifierKey1) | Keyboard.ModifierKeyToModifierKeyFlags(_modifierKey2) | Keyboard.ModifierKeyToModifierKeyFlags(_modifierKey3);

		public KeyCode keyCode
		{
			get
			{
				return Keyboard.LEsjdrBwtqnvhqHMemfJpPhGlorHb(_keyboardKeyCode);
			}
			set
			{
				keyboardKeyCode = Keyboard.PwxvwLyqqaZuWBnWTwcdeSZvdrjb(value);
			}
		}

		public bool hasModifiers
		{
			get
			{
				if (_keyboardKeyCode == KeyboardKeyCode.None)
				{
					return false;
				}
				if (_modifierKey1 != ModifierKey.None || _modifierKey2 != ModifierKey.None || _modifierKey3 != ModifierKey.None)
				{
					return true;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal double modifiedTime => tSchxrtFtMvMZinfMqyLGKsoUFUL;

		[CustomObfuscation(rename = false)]
		internal bool isModified
		{
			get
			{
				return tSchxrtFtMvMZinfMqyLGKsoUFUL > 0.0;
			}
			set
			{
				if (value)
				{
					tSchxrtFtMvMZinfMqyLGKsoUFUL = ReInput.realTime;
				}
				else
				{
					tSchxrtFtMvMZinfMqyLGKsoUFUL = 0.0;
				}
			}
		}

		public ControllerMap controllerMap => LPxrnOpGfQRCZcQMNZvgstOaWZoi;

		public bool enabled
		{
			get
			{
				return uPyFcaFdRzKajesnqkOUtFvpIRKHA;
			}
			set
			{
				uPyFcaFdRzKajesnqkOUtFvpIRKHA = value;
			}
		}

		public string elementIdentifierName
		{
			get
			{
				if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType == ControllerType.Keyboard)
				{
					return EFmaFGDsNKpQYhJKRFQDcvRFZuNmb();
				}
				HardwareControllerMap_Game hardwareControllerMap_Game = ((LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller != null) ? LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : ykljKPhmOIBOELjZHCuDAUUKdbcg);
				if (hardwareControllerMap_Game == null)
				{
					return string.Empty;
				}
				switch (_elementType)
				{
				case ControllerElementType.Axis:
					if (axisType == AxisType.Split)
					{
						if (_axisRange == AxisRange.Positive)
						{
							return hardwareControllerMap_Game.GetElementIdentifierPositiveName(_elementIdentifierId);
						}
						return hardwareControllerMap_Game.GetElementIdentifierNegativeName(_elementIdentifierId);
					}
					return hardwareControllerMap_Game.GetElementIdentifierName(_elementIdentifierId);
				case ControllerElementType.Button:
					return hardwareControllerMap_Game.GetElementIdentifierName(_elementIdentifierId);
				default:
					throw new NotImplementedException();
				}
			}
		}

		public string actionDescriptiveName
		{
			get
			{
				InputAction inputAction = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.xZfXTcilMeUxJbAzlmlraJwZALAIA(_actionId);
				if (inputAction == null)
				{
					return string.Empty;
				}
				if (inputAction.type == InputActionType.Axis)
				{
					if (_elementType == ControllerElementType.Axis && _axisRange == AxisRange.Full)
					{
						return inputAction.descriptiveName;
					}
					if (_elementType == ControllerElementType.Axis || _elementType == ControllerElementType.Button)
					{
						if (_axisContribution == Pole.Positive)
						{
							return inputAction.positiveDescriptiveName;
						}
						if (_axisContribution == Pole.Negative)
						{
							return inputAction.negativeDescriptiveName;
						}
						throw new NotImplementedException();
					}
					throw new NotImplementedException();
				}
				if (inputAction.type == InputActionType.Button)
				{
					if (_elementType == ControllerElementType.Axis && _axisRange == AxisRange.Full)
					{
						return inputAction.descriptiveName;
					}
					if (_elementType == ControllerElementType.Axis || _elementType == ControllerElementType.Button)
					{
						if (_axisContribution == Pole.Negative)
						{
							return inputAction.negativeDescriptiveName;
						}
						return inputAction.descriptiveName;
					}
					throw new NotImplementedException();
				}
				throw new NotImplementedException();
			}
		}

		public int elementIndex => uxemeTqImFAncCLpTkOkfOWaWKUK;

		public int id => nJilCjIhFvMUTsTBcUWuYpormNsu;

		public object elementIdentifierGlyph
		{
			get
			{
				using TempListPool.TList<object> tList = TempListPool.GetTList<object>();
				int elementIdentifierGlyphs = GetElementIdentifierGlyphs(tList.list);
				if (elementIdentifierGlyphs == 0)
				{
					return null;
				}
				return tList.list[elementIdentifierGlyphs - 1];
			}
		}

		public int elementIdentifierGlyphCount
		{
			get
			{
				using TempListPool.TList<object> tList = TempListPool.GetTList<object>();
				return GetElementIdentifierGlyphs(tList.list);
			}
		}

		private bool iWXinHvSskxJWQXcHIJGqZjdvZLI
		{
			get
			{
				if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null)
				{
					return LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType == ControllerType.Keyboard;
				}
				return false;
			}
		}

		private static int nxygczJCOMIJXAtsDRjpHHyOvypgA
		{
			get
			{
				int result = uidCounter;
				if (uidCounter == 2147483647)
				{
					uidCounter = 0;
					return result;
				}
				uidCounter++;
				return result;
			}
		}

		internal static bool rWwBQUblidRcekuSVdiHTcNfrIlmA(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				return false;
			}
			if (P_0._actionId != -1 && !ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.PkQdWCLFQkmIhaYncbIRDzOLBwYg(P_0._actionId))
			{
				return false;
			}
			return true;
		}

		internal static void afugGbbrqtlBIcFBizanQAeSmqOQB(ActionElementMap P_0, ActionElementMap P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			if (P_1 == null)
			{
				throw new ArgumentNullException("destination");
			}
			P_1._actionId = P_0._actionId;
			P_1._actionCategoryId = P_0._actionCategoryId;
			P_1._elementType = P_0._elementType;
			P_1._elementIdentifierId = P_0._elementIdentifierId;
			P_1._axisRange = P_0._axisRange;
			P_1._invert = P_0._invert;
			P_1._axisContribution = P_0._axisContribution;
			P_1._keyboardKeyCode = P_0._keyboardKeyCode;
			P_1._modifierKey1 = P_0._modifierKey1;
			P_1._modifierKey2 = P_0._modifierKey2;
			P_1._modifierKey3 = P_0._modifierKey3;
			P_1.LPxrnOpGfQRCZcQMNZvgstOaWZoi = P_0.LPxrnOpGfQRCZcQMNZvgstOaWZoi;
			P_1.uxemeTqImFAncCLpTkOkfOWaWKUK = P_0.uxemeTqImFAncCLpTkOkfOWaWKUK;
			P_1.uPyFcaFdRzKajesnqkOUtFvpIRKHA = P_0.uPyFcaFdRzKajesnqkOUtFvpIRKHA;
			P_1.tSchxrtFtMvMZinfMqyLGKsoUFUL = P_0.tSchxrtFtMvMZinfMqyLGKsoUFUL;
		}

		public static bool TryGetCombinedElementIdentifierName(IList<ActionElementMap> actionElementMaps, out string result)
		{
			int count;
			if (actionElementMaps == null || (count = actionElementMaps.Count) == 0)
			{
				result = null;
				return false;
			}
			HardwareControllerMap_Game hardwareControllerMap_Game = null;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = actionElementMaps[i];
				if (actionElementMap == null)
				{
					continue;
				}
				HardwareControllerMap_Game hardwareControllerMap_Game2 = ((actionElementMap.LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && actionElementMap.LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller != null) ? actionElementMap.LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : actionElementMap.ykljKPhmOIBOELjZHCuDAUUKdbcg);
				if (hardwareControllerMap_Game != null)
				{
					if (hardwareControllerMap_Game2 != hardwareControllerMap_Game)
					{
						result = null;
						return false;
					}
				}
				else
				{
					hardwareControllerMap_Game = hardwareControllerMap_Game2;
				}
			}
			if (hardwareControllerMap_Game == null)
			{
				result = null;
				return false;
			}
			for (int j = 0; j < count; j++)
			{
				ActionElementMap actionElementMap = actionElementMaps[j];
				if (actionElementMap != null && hardwareControllerMap_Game.TryGetCompoundElementMemberCombinedLocalizedName(actionElementMaps, out result))
				{
					return true;
				}
			}
			result = null;
			return false;
		}

		public static bool TryGetCombinedElementIdentifierGlyph(IList<ActionElementMap> actionElementMaps, out object result)
		{
			string text;
			return ysjrOGFbpmzQeeNNafVMoRXghTbb(actionElementMaps, true, false, out result, out text);
		}

		public static bool TryGetCombinedElementIdentifierFinalGlyphKey(IList<ActionElementMap> actionElementMaps, out string result)
		{
			object obj;
			return ysjrOGFbpmzQeeNNafVMoRXghTbb(actionElementMaps, false, true, out obj, out result);
		}

		private static bool ysjrOGFbpmzQeeNNafVMoRXghTbb(IList<ActionElementMap> P_0, bool P_1, bool P_2, out object P_3, out string P_4)
		{
			int count;
			if (P_0 == null || (count = P_0.Count) == 0)
			{
				P_3 = null;
				P_4 = null;
				return false;
			}
			HardwareControllerMap_Game hardwareControllerMap_Game = null;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = P_0[i];
				if (actionElementMap == null)
				{
					continue;
				}
				HardwareControllerMap_Game hardwareControllerMap_Game2 = ((actionElementMap.LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && actionElementMap.LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller != null) ? actionElementMap.LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : actionElementMap.ykljKPhmOIBOELjZHCuDAUUKdbcg);
				if (hardwareControllerMap_Game != null)
				{
					if (hardwareControllerMap_Game2 != hardwareControllerMap_Game)
					{
						P_3 = null;
						P_4 = null;
						return false;
					}
				}
				else
				{
					hardwareControllerMap_Game = hardwareControllerMap_Game2;
				}
			}
			if (hardwareControllerMap_Game == null)
			{
				P_3 = null;
				P_4 = null;
				return false;
			}
			for (int j = 0; j < count; j++)
			{
				ActionElementMap actionElementMap = P_0[j];
				if (actionElementMap != null && hardwareControllerMap_Game.TryGetCompoundElementMemberCombinedGlyph(P_0, P_1, P_2, out P_3, out P_4))
				{
					return true;
				}
			}
			P_3 = null;
			P_4 = null;
			return false;
		}

		public ActionElementMap()
		{
			nJilCjIhFvMUTsTBcUWuYpormNsu = nxygczJCOMIJXAtsDRjpHHyOvypgA;
			_actionId = -1;
			_elementIdentifierId = -1;
			uPyFcaFdRzKajesnqkOUtFvpIRKHA = true;
		}

		public ActionElementMap(ActionElementMap P_0)
			: this()
		{
			afugGbbrqtlBIcFBizanQAeSmqOQB(P_0, this);
		}

		public ActionElementMap(int P_0, ControllerElementType P_1, int P_2)
			: this()
		{
			_actionId = P_0;
			_elementType = P_1;
			_elementIdentifierId = P_2;
		}

		public ActionElementMap(int P_0, ControllerElementType P_1, int P_2, Pole P_3, AxisRange P_4)
			: this()
		{
			_actionId = P_0;
			_elementType = P_1;
			_elementIdentifierId = P_2;
			_axisContribution = P_3;
			_axisRange = P_4;
		}

		public ActionElementMap(int P_0, ControllerElementType P_1, int P_2, Pole P_3, AxisRange P_4, bool P_5)
			: this()
		{
			_actionId = P_0;
			_elementType = P_1;
			_elementIdentifierId = P_2;
			_axisContribution = P_3;
			_axisRange = P_4;
			_invert = P_5;
		}

		public ActionElementMap(int P_0, ControllerElementType P_1, Pole P_2, KeyboardKeyCode P_3, ModifierKey P_4, ModifierKey P_5, ModifierKey P_6)
			: this()
		{
			_actionId = P_0;
			_elementType = P_1;
			_axisContribution = P_2;
			_keyboardKeyCode = P_3;
			_modifierKey1 = P_4;
			_modifierKey2 = P_5;
			_modifierKey3 = P_6;
			MhpKcbNLflnpZEtTGCqhnXXwpFxm();
		}

		public bool CheckForAssignmentConflict(ElementAssignment elementAssignment)
		{
			if (!bDbJJHvNrlZmNYXIDGExuVqlntVP(elementAssignment.type))
			{
				return false;
			}
			if (iWXinHvSskxJWQXcHIJGqZjdvZLI || _keyboardKeyCode != KeyboardKeyCode.None)
			{
				KeyCode keyCode = elementAssignment.keyboardKey;
				if (keyCode == KeyCode.None)
				{
					keyCode = ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.TVvLxBfEgOqnloHdRFcagvmpmnZT.GetKeyCodeById(elementAssignment.elementIdentifierId);
				}
				return RoQXNlBQZRqnTNeqBNFUALultMFV(Keyboard.PwxvwLyqqaZuWBnWTwcdeSZvdrjb(keyCode), elementAssignment.modifierKeyFlags);
			}
			return xUrvnUgPeBcuaFYNjxJdUshpvbQdA(elementAssignment.elementIdentifierId, elementAssignment.axisRange);
		}

		public bool CheckForAssignmentConflict(ActionElementMap elementMap)
		{
			if (elementMap == null || elementMap == this)
			{
				return false;
			}
			if (_elementType != elementMap._elementType)
			{
				return false;
			}
			if (iWXinHvSskxJWQXcHIJGqZjdvZLI || _keyboardKeyCode != KeyboardKeyCode.None)
			{
				return RoQXNlBQZRqnTNeqBNFUALultMFV(elementMap._keyboardKeyCode, elementMap.modifierKeyFlags);
			}
			return xUrvnUgPeBcuaFYNjxJdUshpvbQdA(elementMap._elementIdentifierId, elementMap._axisRange);
		}

		public bool ShowInField(AxisRange fieldActionRange)
		{
			if (!ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.PkQdWCLFQkmIhaYncbIRDzOLBwYg(_actionId))
			{
				return false;
			}
			if (fieldActionRange == AxisRange.Full)
			{
				if (_elementType == ControllerElementType.Axis)
				{
					if (axisRange != AxisRange.Full)
					{
						return false;
					}
				}
				else if (_elementType == ControllerElementType.Button && ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.xZfXTcilMeUxJbAzlmlraJwZALAIA(_actionId).type == InputActionType.Axis)
				{
					return false;
				}
			}
			else
			{
				if (_elementType == ControllerElementType.Axis && axisRange == AxisRange.Full)
				{
					return false;
				}
				if (ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.xZfXTcilMeUxJbAzlmlraJwZALAIA(_actionId).type == InputActionType.Axis)
				{
					if (fieldActionRange == AxisRange.Positive && axisContribution != Pole.Positive)
					{
						return false;
					}
					if (fieldActionRange == AxisRange.Negative && axisContribution != Pole.Negative)
					{
						return false;
					}
				}
				else if (axisContribution != axisContribution)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsTarget(ControllerElementTarget elementTarget)
		{
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			bool result = IsTarget(szcVmbDpoJahYmnXXukLaOXfCanz);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return result;
		}

		public bool IsTarget(IControllerElementTarget elementTarget)
		{
			if (elementTarget == null)
			{
				return false;
			}
			if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null)
			{
				Controller controller = elementTarget.controller;
				if (controller == null)
				{
					return false;
				}
				if (controller.id != LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerId || controller.type != LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType)
				{
					return false;
				}
			}
			if (_elementType != elementTarget.elementType)
			{
				return false;
			}
			if (_elementType == ControllerElementType.Axis)
			{
				if (_elementIdentifierId == elementTarget.elementIdentifierId)
				{
					return _axisRange == elementTarget.axisRange;
				}
				return false;
			}
			if (_elementType == ControllerElementType.Button)
			{
				return _elementIdentifierId == elementTarget.elementIdentifierId;
			}
			throw new NotImplementedException();
		}

		public int GetElementIdentifierGlyphs(ICollection<object> results)
		{
			int count = results.Count;
			if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType == ControllerType.Keyboard)
			{
				return DdSTRUbHwKFPgHWDlYLtMTlTavAFb(results);
			}
			HardwareControllerMap_Game hardwareControllerMap_Game = ((LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller != null) ? LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : ykljKPhmOIBOELjZHCuDAUUKdbcg);
			if (hardwareControllerMap_Game == null)
			{
				return 0;
			}
			ControllerElementIdentifier elementIdentifierById = hardwareControllerMap_Game.GetElementIdentifierById(_elementIdentifierId);
			if (elementIdentifierById == null)
			{
				return 0;
			}
			object obj = _elementType switch
			{
				ControllerElementType.Axis => (axisType != AxisType.Split) ? elementIdentifierById.glyph : ((_axisRange != AxisRange.Positive) ? elementIdentifierById.negativeGlyph : elementIdentifierById.positiveGlyph), 
				ControllerElementType.Button => elementIdentifierById.glyph, 
				_ => throw new NotImplementedException(), 
			};
			if (obj != null)
			{
				results.Add(obj);
			}
			return results.Count - count;
		}

		public int GetElementIdentifierGlyphs<T>(ICollection<T> results)
		{
			using TempListPool.TList<object> tList = TempListPool.GetTList<object>();
			List<object> list = tList.list;
			int elementIdentifierGlyphs = GetElementIdentifierGlyphs(list);
			int count = results.Count;
			for (int i = 0; i < elementIdentifierGlyphs; i++)
			{
				if (!(list[i] is T))
				{
					return 0;
				}
			}
			for (int j = 0; j < elementIdentifierGlyphs; j++)
			{
				results.Add((T)list[j]);
			}
			return results.Count - count;
		}

		public int GetElementIdentifierFinalGlyphKeys(ICollection<string> results)
		{
			int count = results.Count;
			if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType == ControllerType.Keyboard)
			{
				return oTDCYBiMUihHmrStlxEEuaItItLhA(results);
			}
			HardwareControllerMap_Game hardwareControllerMap_Game = ((LPxrnOpGfQRCZcQMNZvgstOaWZoi != null && LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller != null) ? LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : ykljKPhmOIBOELjZHCuDAUUKdbcg);
			if (hardwareControllerMap_Game == null)
			{
				return 0;
			}
			ControllerElementIdentifier elementIdentifierById = hardwareControllerMap_Game.GetElementIdentifierById(_elementIdentifierId);
			if (elementIdentifierById == null)
			{
				return 0;
			}
			string text = _elementType switch
			{
				ControllerElementType.Axis => (axisType != AxisType.Split) ? elementIdentifierById.GetFinalGlyphKey(_elementType, AxisRange.Full) : elementIdentifierById.GetFinalGlyphKey(_elementType, _axisRange), 
				ControllerElementType.Button => elementIdentifierById.GetFinalGlyphKey(_elementType, AxisRange.Full), 
				_ => throw new NotImplementedException(), 
			};
			if (text != null)
			{
				results.Add(text);
			}
			return results.Count - count;
		}

		internal void CeDmmMmdwtjVdcVPRFXshDHqgijv(ControllerMap P_0)
		{
			LPxrnOpGfQRCZcQMNZvgstOaWZoi = P_0;
			ControllerType controllerType = P_0.controllerType;
			HardwareControllerMap_Game hardwareControllerMap_Game = ((P_0.controller != null) ? P_0.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : null);
			OxVTGRqkxqRruPItSpCntyNjiAVE(controllerType, hardwareControllerMap_Game, controllerType == ControllerType.Keyboard && _elementIdentifierId <= 0);
		}

		internal void sahAvUfMOSKesGrXhGKAeDGCZnIJc(ControllerMap P_0, HardwareControllerMap_Game P_1)
		{
			LPxrnOpGfQRCZcQMNZvgstOaWZoi = P_0;
			ykljKPhmOIBOELjZHCuDAUUKdbcg = P_1;
			OxVTGRqkxqRruPItSpCntyNjiAVE(P_0.controllerType, P_1, P_0.controllerType == ControllerType.Keyboard && _elementIdentifierId <= 0);
		}

		private void chAwkTluIJWbDVjKaUSgcyQoljPg(bool P_0)
		{
			if (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null)
			{
				OxVTGRqkxqRruPItSpCntyNjiAVE(LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType, (LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller != null) ? LPxrnOpGfQRCZcQMNZvgstOaWZoi.controller.UNRIOyvPojfCPrjRsEYcHBwwkZqS : null, P_0);
			}
		}

		private void OxVTGRqkxqRruPItSpCntyNjiAVE(ControllerType P_0, HardwareControllerMap_Game P_1, bool P_2)
		{
			if (LPxrnOpGfQRCZcQMNZvgstOaWZoi == null)
			{
				return;
			}
			if (P_0 == ControllerType.Keyboard)
			{
				Keyboard keyboard = ReInput.controllers.Keyboard;
				if (P_2)
				{
					uxemeTqImFAncCLpTkOkfOWaWKUK = keyboard.GetButtonIndex(_keyboardKeyCode);
					MhpKcbNLflnpZEtTGCqhnXXwpFxm();
				}
				else
				{
					uxemeTqImFAncCLpTkOkfOWaWKUK = keyboard.GetButtonIndexById(_elementIdentifierId);
					genFEeSZdcsngoTkCiexSNZgCsXIA();
				}
			}
			else if (P_1 != null)
			{
				switch (_elementType)
				{
				case ControllerElementType.Axis:
					uxemeTqImFAncCLpTkOkfOWaWKUK = P_1.GetAxisIndex(_elementIdentifierId);
					break;
				case ControllerElementType.Button:
					uxemeTqImFAncCLpTkOkfOWaWKUK = P_1.GetButtonIndex(_elementIdentifierId);
					break;
				default:
					throw new NotImplementedException();
				}
			}
		}

		private string EFmaFGDsNKpQYhJKRFQDcvRFZuNmb()
		{
			string text = Keyboard.GetKeyName((KeyCode)_keyboardKeyCode);
			if (string.Equals(text, xcZrlmieAJsAZzNmAqaOwTkoCqKH, StringComparison.Ordinal) && qdDxobyQabJFkCVUGVIkdujrbZddA == modifierKeyFlags && (!LocalizationManager.isEnabled || MerbNRWLFPJrqZGWIaGALXprFQOb == LocalizationManager.version))
			{
				return kDzHkcbkAQhbWaliMPTQMvDlBdIiA;
			}
			xcZrlmieAJsAZzNmAqaOwTkoCqKH = text;
			qdDxobyQabJFkCVUGVIkdujrbZddA = modifierKeyFlags;
			if (LocalizationManager.isEnabled)
			{
				MerbNRWLFPJrqZGWIaGALXprFQOb = LocalizationManager.version;
			}
			if (_modifierKey3 != ModifierKey.None)
			{
				text = $"{Keyboard.GetModifierKeyName(_modifierKey3, getShortName: true)} + {text}";
			}
			if (_modifierKey2 != ModifierKey.None)
			{
				text = $"{Keyboard.GetModifierKeyName(_modifierKey2, getShortName: true)} + {text}";
			}
			if (_modifierKey1 != ModifierKey.None)
			{
				text = $"{Keyboard.GetModifierKeyName(_modifierKey1, getShortName: true)} + {text}";
			}
			kDzHkcbkAQhbWaliMPTQMvDlBdIiA = text;
			return kDzHkcbkAQhbWaliMPTQMvDlBdIiA;
		}

		private int DdSTRUbHwKFPgHWDlYLtMTlTavAFb(ICollection<object> P_0)
		{
			object glyph = ReInput.controllers.Keyboard.GetElementIdentifierByKeyCode((KeyCode)_keyboardKeyCode).glyph;
			if (glyph == null)
			{
				return 0;
			}
			int count = P_0.Count;
			using (TempListPool.TList<object> tList = TempListPool.GetTList<object>())
			{
				List<object> list = tList.list;
				if (_modifierKey1 != ModifierKey.None)
				{
					object modifierKeyGlyph = Keyboard.GetModifierKeyGlyph(_modifierKey1);
					if (modifierKeyGlyph == null)
					{
						return 0;
					}
					list.Add(modifierKeyGlyph);
				}
				if (_modifierKey2 != ModifierKey.None)
				{
					object modifierKeyGlyph2 = Keyboard.GetModifierKeyGlyph(_modifierKey2);
					if (modifierKeyGlyph2 == null)
					{
						return 0;
					}
					list.Add(modifierKeyGlyph2);
				}
				if (_modifierKey3 != ModifierKey.None)
				{
					object modifierKeyGlyph3 = Keyboard.GetModifierKeyGlyph(_modifierKey3);
					if (modifierKeyGlyph3 == null)
					{
						return 0;
					}
					list.Add(modifierKeyGlyph3);
				}
				for (int i = 0; i < list.Count; i++)
				{
					P_0.Add(list[i]);
				}
			}
			P_0.Add(glyph);
			return P_0.Count - count;
		}

		private int oTDCYBiMUihHmrStlxEEuaItItLhA(ICollection<string> P_0)
		{
			string finalGlyphKey = ReInput.controllers.Keyboard.GetElementIdentifierByKeyCode((KeyCode)_keyboardKeyCode).GetFinalGlyphKey(AxisRange.Full);
			if (finalGlyphKey == null)
			{
				return 0;
			}
			int count = P_0.Count;
			using (TempListPool.TList<string> tList = TempListPool.GetTList<string>())
			{
				List<string> list = tList.list;
				if (_modifierKey1 != ModifierKey.None)
				{
					string text = Keyboard.PlUzctevrihyWuRRIquNsVgXsNdv(_modifierKey1);
					if (text == null)
					{
						return 0;
					}
					list.Add(text);
				}
				if (_modifierKey2 != ModifierKey.None)
				{
					string text2 = Keyboard.PlUzctevrihyWuRRIquNsVgXsNdv(_modifierKey2);
					if (text2 == null)
					{
						return 0;
					}
					list.Add(text2);
				}
				if (_modifierKey3 != ModifierKey.None)
				{
					string text3 = Keyboard.PlUzctevrihyWuRRIquNsVgXsNdv(_modifierKey3);
					if (text3 == null)
					{
						return 0;
					}
					list.Add(text3);
				}
				for (int i = 0; i < list.Count; i++)
				{
					P_0.Add(list[i]);
				}
			}
			P_0.Add(finalGlyphKey);
			return P_0.Count - count;
		}

		internal void poaYXnSDRTkbjnijNbPqDYgSEAiS()
		{
			_actionCategoryId = -1;
			_actionId = -1;
			_elementType = ControllerElementType.Axis;
			_elementIdentifierId = -1;
			_axisRange = AxisRange.Full;
			_invert = false;
			_axisContribution = Pole.Positive;
			_keyboardKeyCode = KeyboardKeyCode.None;
			_modifierKey1 = ModifierKey.None;
			_modifierKey2 = ModifierKey.None;
			_modifierKey3 = ModifierKey.None;
			LPxrnOpGfQRCZcQMNZvgstOaWZoi = null;
			uPyFcaFdRzKajesnqkOUtFvpIRKHA = true;
			xcZrlmieAJsAZzNmAqaOwTkoCqKH = null;
			kDzHkcbkAQhbWaliMPTQMvDlBdIiA = null;
			MerbNRWLFPJrqZGWIaGALXprFQOb = 0u;
			qdDxobyQabJFkCVUGVIkdujrbZddA = ModifierKeyFlags.None;
			uxemeTqImFAncCLpTkOkfOWaWKUK = -1;
		}

		private bool RoQXNlBQZRqnTNeqBNFUALultMFV(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
		{
			if (_elementType != ControllerElementType.Button)
			{
				return false;
			}
			if (P_0 == KeyboardKeyCode.None)
			{
				return false;
			}
			if (_keyboardKeyCode != P_0)
			{
				return false;
			}
			if (Keyboard.KZGGDTlKdXOOOfDRXfdLUWnhkIff(modifierKeyFlags) != Keyboard.KZGGDTlKdXOOOfDRXfdLUWnhkIff(P_1))
			{
				return false;
			}
			return true;
		}

		private bool xUrvnUgPeBcuaFYNjxJdUshpvbQdA(int P_0, AxisRange P_1)
		{
			if (_elementIdentifierId != P_0)
			{
				return false;
			}
			if (_elementType == ControllerElementType.Button)
			{
				return true;
			}
			if (_elementType == ControllerElementType.Axis)
			{
				if (_axisRange == AxisRange.Full || P_1 == AxisRange.Full)
				{
					return true;
				}
				if (_axisRange == AxisRange.Positive && P_1 == AxisRange.Positive)
				{
					return true;
				}
				if (_axisRange == AxisRange.Negative && P_1 == AxisRange.Negative)
				{
					return true;
				}
				return false;
			}
			throw new NotImplementedException();
		}

		private bool bDbJJHvNrlZmNYXIDGExuVqlntVP(ElementAssignmentType P_0)
		{
			if (_elementType == ControllerElementType.Button)
			{
				if (P_0 == ElementAssignmentType.Button || P_0 == ElementAssignmentType.KeyboardKey)
				{
					return true;
				}
			}
			else
			{
				if (_elementType != ControllerElementType.Axis)
				{
					throw new NotImplementedException();
				}
				if (P_0 == ElementAssignmentType.FullAxis || P_0 == ElementAssignmentType.SplitAxis)
				{
					return true;
				}
			}
			return false;
		}

		private void MhpKcbNLflnpZEtTGCqhnXXwpFxm()
		{
			_elementIdentifierId = Keyboard.MMNqiODvdWTogJjoqPDqgGggOGKA(_keyboardKeyCode);
		}

		private void genFEeSZdcsngoTkCiexSNZgCsXIA()
		{
			if (_elementIdentifierId < 0)
			{
				_keyboardKeyCode = KeyboardKeyCode.None;
			}
			else if (ReInput.isReady)
			{
				_keyboardKeyCode = Keyboard.PwxvwLyqqaZuWBnWTwcdeSZvdrjb(ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.TVvLxBfEgOqnloHdRFcagvmpmnZT.GetKeyCodeById(_elementIdentifierId));
			}
		}

		private void atHaTMlRLxKJeURaFtHVAnPqTnqV()
		{
			if (_modifierKey1 != ModifierKey.None)
			{
				if (_modifierKey1 == _modifierKey2)
				{
					_modifierKey2 = ModifierKey.None;
				}
				if (_modifierKey1 == _modifierKey3)
				{
					_modifierKey3 = ModifierKey.None;
				}
			}
			if (_modifierKey2 != ModifierKey.None && _modifierKey2 == _modifierKey3)
			{
				_modifierKey3 = ModifierKey.None;
			}
			if (_modifierKey3 != ModifierKey.None && _modifierKey2 == ModifierKey.None)
			{
				_modifierKey2 = _modifierKey3;
				_modifierKey3 = ModifierKey.None;
			}
			if (_modifierKey2 != ModifierKey.None && _modifierKey1 == ModifierKey.None)
			{
				_modifierKey1 = _modifierKey2;
				_modifierKey2 = ModifierKey.None;
			}
		}

		internal void mcFJwsoNSHHvPNLkGxwFTlHQondm()
		{
			if (!ControllerMap.mCTnDNAaJjJXoZDRSzjNPwGcXRBm)
			{
				tSchxrtFtMvMZinfMqyLGKsoUFUL = ReInput.realTime;
			}
		}

		internal SerializedObject nXkDBItKfPbLOehFpkJWWFJeYwiu()
		{
			return new SerializedObject(GetType(), SerializedObject.ObjectType.Object)
			{
				{ "actionCategoryId", _actionCategoryId },
				{ "actionId", _actionId },
				{ "elementType", _elementType },
				{ "elementIdentifierId", _elementIdentifierId },
				{ "axisRange", _axisRange },
				{ "invert", _invert },
				{ "axisContribution", _axisContribution },
				{ "keyboardKeyCode", _keyboardKeyCode },
				{ "modifierKey1", _modifierKey1 },
				{ "modifierKey2", _modifierKey2 },
				{ "modifierKey3", _modifierKey3 },
				{ "enabled", uPyFcaFdRzKajesnqkOUtFvpIRKHA }
			};
		}

		internal void nynjKwRhGWhMIfJsmDLrvmxlEBrrA(SerializedObject P_0)
		{
			_actionCategoryId = -1;
			_actionId = -1;
			_elementIdentifierId = -1;
			_axisRange = AxisRange.Full;
			_invert = false;
			_axisContribution = Pole.Positive;
			_keyboardKeyCode = KeyboardKeyCode.None;
			_modifierKey1 = ModifierKey.None;
			_modifierKey2 = ModifierKey.None;
			_modifierKey3 = ModifierKey.None;
			uPyFcaFdRzKajesnqkOUtFvpIRKHA = true;
			P_0.TryGetDeserializedValueByRef("actionCategoryId", ref _actionCategoryId);
			P_0.TryGetDeserializedValueByRef("actionId", ref _actionId);
			P_0.TryGetDeserializedValueByRef("elementType", ref _elementType);
			P_0.TryGetDeserializedValueByRef("elementIdentifierId", ref _elementIdentifierId);
			P_0.TryGetDeserializedValueByRef("axisRange", ref _axisRange);
			P_0.TryGetDeserializedValueByRef("invert", ref _invert);
			P_0.TryGetDeserializedValueByRef("axisContribution", ref _axisContribution);
			P_0.TryGetDeserializedValueByRef("keyboardKeyCode", ref _keyboardKeyCode);
			P_0.TryGetDeserializedValueByRef("modifierKey1", ref _modifierKey1);
			P_0.TryGetDeserializedValueByRef("modifierKey2", ref _modifierKey2);
			P_0.TryGetDeserializedValueByRef("modifierKey3", ref _modifierKey3);
			P_0.TryGetDeserializedValueByRef("enabled", ref uPyFcaFdRzKajesnqkOUtFvpIRKHA);
			mcFJwsoNSHHvPNLkGxwFTlHQondm();
		}

		public override string ToString()
		{
			if (s_toStringSB == null)
			{
				s_toStringSB = new StringBuilder();
			}
			StringTools.WriteVar(s_toStringSB, "Id", nJilCjIhFvMUTsTBcUWuYpormNsu);
			StringTools.WriteVar(s_toStringSB, "Enabled", uPyFcaFdRzKajesnqkOUtFvpIRKHA);
			StringTools.WriteVar(s_toStringSB, "Controller Map Id", (LPxrnOpGfQRCZcQMNZvgstOaWZoi != null) ? LPxrnOpGfQRCZcQMNZvgstOaWZoi.id : (-1));
			StringTools.WriteVar(s_toStringSB, "Action Id", _actionId);
			StringTools.WriteVar(s_toStringSB, "Action Descriptive Name", actionDescriptiveName);
			StringTools.WriteVar(s_toStringSB, "Element Type", _elementType);
			StringTools.WriteVar(s_toStringSB, "Element Identifier Id", _elementIdentifierId);
			StringTools.WriteVar(s_toStringSB, "Element Index", uxemeTqImFAncCLpTkOkfOWaWKUK);
			StringTools.WriteVar(s_toStringSB, "Axis Range", _axisRange);
			StringTools.WriteVar(s_toStringSB, "Invert", _invert);
			StringTools.WriteVar(s_toStringSB, "Axis Contribution", _axisContribution);
			StringTools.WriteVar(s_toStringSB, "Keyboard Key Code", _keyboardKeyCode);
			StringTools.WriteVar(s_toStringSB, "Has Modifiers", hasModifiers);
			StringTools.WriteVar(s_toStringSB, "Modifier Key 1", _modifierKey1);
			StringTools.WriteVar(s_toStringSB, "modifier Key 2", _modifierKey2);
			StringTools.WriteVar(s_toStringSB, "modifier Key 3", _modifierKey3);
			StringTools.WriteVar(s_toStringSB, "modifier Key Flags", modifierKeyFlags);
			string result = s_toStringSB.ToString();
			s_toStringSB.Length = 0;
			return result;
		}
	}
}
