using System;
using System.Collections.Generic;
using Rewired.ComponentControls.Data;
using Rewired.Data;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[AddComponentMenu("Rewired/Custom Controller")]
	[DisallowMultipleComponent]
	public class CustomController : ComponentController
	{
		[Serializable]
		public class CreateCustomControllerSettings
		{
			[SerializeField]
			[CustomObfuscation(rename = false)]
			[Tooltip("If true, a new Custom Controller will be created. Otherwise, an existing Custom Controller will be found using the selector properties.")]
			private bool _createCustomController = true;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			[Tooltip("The source id of the Custom Controller to create. Get this from the Rewired Input Manager.")]
			private int _customControllerSourceId = -1;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			[Tooltip("The Player that will be assigned this Custom Controller when it is created.")]
			private int _assignToPlayerId;

			[SerializeField]
			[Tooltip("If true, the Custom Controller created by this component will be destroyed when this component is destroyed.")]
			[CustomObfuscation(rename = false)]
			private bool _destroyCustomController = true;

			public bool createCustomController
			{
				get
				{
					return _createCustomController;
				}
				set
				{
					if (_createCustomController != value)
					{
						_createCustomController = value;
					}
				}
			}

			public int customControllerSourceId
			{
				get
				{
					return _customControllerSourceId;
				}
				set
				{
					_customControllerSourceId = value;
				}
			}

			public int assignToPlayerId
			{
				get
				{
					return _assignToPlayerId;
				}
				set
				{
					_assignToPlayerId = value;
				}
			}

			public bool destroyCustomController
			{
				get
				{
					return _destroyCustomController;
				}
				set
				{
					_destroyCustomController = value;
				}
			}
		}

		private struct qznnUAsCoSGpUBlBmvqxnRSYcwac
		{
			public CustomControllerElementSelector.ElementType MdjBbHAfEpOIyysKWtDJwilBqwCjA;

			public int fpRaWzdHfXPowZWkJxwTZPqeXOGK;

			public float yYOUbwIbBcyPAQvjeAEXVsjzLAln;

			public qznnUAsCoSGpUBlBmvqxnRSYcwac(CustomControllerElementSelector.ElementType P_0, int P_1, float P_2)
			{
				MdjBbHAfEpOIyysKWtDJwilBqwCjA = P_0;
				fpRaWzdHfXPowZWkJxwTZPqeXOGK = P_1;
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = P_2;
			}

			public qznnUAsCoSGpUBlBmvqxnRSYcwac(CustomControllerElementSelector.ElementType P_0, int P_1, bool P_2)
			{
				MdjBbHAfEpOIyysKWtDJwilBqwCjA = P_0;
				fpRaWzdHfXPowZWkJxwTZPqeXOGK = P_1;
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = (P_2 ? 1f : 0f);
			}

			public bool AXRaAabAcDFslXRoZntGgwFsvHfcA(CustomControllerElementSelector.ElementType P_0, int P_1)
			{
				if (MdjBbHAfEpOIyysKWtDJwilBqwCjA == P_0)
				{
					return fpRaWzdHfXPowZWkJxwTZPqeXOGK == P_1;
				}
				return false;
			}

			public void oGPVXKkaRuOYcNaLJIZFXXgWIxtx(float P_0)
			{
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = MathTools.MaxMagnitude(yYOUbwIbBcyPAQvjeAEXVsjzLAln, P_0);
			}

			public void oGPVXKkaRuOYcNaLJIZFXXgWIxtx(bool P_0)
			{
				if (P_0)
				{
					yYOUbwIbBcyPAQvjeAEXVsjzLAln = 1f;
				}
			}
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Custom Controller elements, etc.")]
		private InputManager_Base _rewiredInputManager;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Contains search parameters to find a particular Custom Controller.")]
		private CustomControllerSelector _customControllerSelector = new CustomControllerSelector();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Settings for creating a Custom Controller on start.")]
		private CreateCustomControllerSettings _createCustomControllerSettings = new CreateCustomControllerSettings();

		private List<qznnUAsCoSGpUBlBmvqxnRSYcwac> eSbsYaKGZpVsnISkPHYjcxRLpYeD = new List<qznnUAsCoSGpUBlBmvqxnRSYcwac>(10);

		[NonSerialized]
		private int VXJxyJaRzWDAeCgODeEzFozXCjuAb = -1;

		private Action BbIMbQeLvHjivRxkUFEuuHyecJUe;

		public InputManager_Base rewiredInputManager
		{
			get
			{
				return _rewiredInputManager;
			}
			set
			{
				if (!(_rewiredInputManager == value))
				{
					_rewiredInputManager = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public CustomControllerSelector customControllerSelector => _customControllerSelector;

		public CreateCustomControllerSettings createCustomControllerSettings => _createCustomControllerSettings;

		internal event Action InputSourceUpdateEvent
		{
			add
			{
				BbIMbQeLvHjivRxkUFEuuHyecJUe = (Action)Delegate.Combine(BbIMbQeLvHjivRxkUFEuuHyecJUe, value);
			}
			remove
			{
				BbIMbQeLvHjivRxkUFEuuHyecJUe = (Action)Delegate.Remove(BbIMbQeLvHjivRxkUFEuuHyecJUe, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal CustomController()
		{
		}

		public Rewired.CustomController GetCustomController()
		{
			return UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
		}

		[CustomObfuscation(rename = false)]
		internal override void OnEnable()
		{
			base.OnEnable();
			_ = base.jYTNgflwwEgvgbZuuTHYnPAnuRao;
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			base.OnDisable();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				eSbsYaKGZpVsnISkPHYjcxRLpYeD.Clear();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDestroy()
		{
			base.OnDestroy();
			wqpSDNioVwQwQGWdTeRRkteZueuX();
		}

		internal virtual bool OnInitialize()
		{
			if (!base.lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				return false;
			}
			if (GetUseCustomController())
			{
				if (!ajhXJbgnqLHNDTtpxLqCuAgGrnVb())
				{
					return false;
				}
				if (UhFBEoKYbICWKGrfPbNBAMYDdcqzA(true) == null)
				{
					SetUseCustomController(value: false);
				}
			}
			return true;
		}

		internal virtual void OnSubscribeEvents()
		{
			base.sGOXUkdDJMvaXZNxRxZLevjalIqm();
			LYluBUMWPKbwashzGpNPLTeasipd();
			if (ReInput.isReady)
			{
				ReInput.InputSourceUpdateEvent += uZsvbYnQMPckbZUTVOMacdUKDPsr;
			}
		}

		internal virtual void OnUnsubscribeEvents()
		{
			base.LYluBUMWPKbwashzGpNPLTeasipd();
			ReInput.InputSourceUpdateEvent -= uZsvbYnQMPckbZUTVOMacdUKDPsr;
		}

		public override void ClearControlValues()
		{
			base.ClearControlValues();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				eSbsYaKGZpVsnISkPHYjcxRLpYeD.Clear();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual bool GetUseCustomController()
		{
			return true;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void SetUseCustomController(bool value)
		{
		}

		internal void SetAxisValue(CustomControllerElementSelector element, float value)
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao || element == null || !GetUseCustomController())
			{
				return;
			}
			Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
			if (customController == null)
			{
				return;
			}
			int elementIndex = element.GetElementIndex(customController);
			if (elementIndex < 0)
			{
				return;
			}
			_ = eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count;
			for (int i = 0; i < eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count; i++)
			{
				qznnUAsCoSGpUBlBmvqxnRSYcwac value2 = eSbsYaKGZpVsnISkPHYjcxRLpYeD[i];
				if (value2.AXRaAabAcDFslXRoZntGgwFsvHfcA(element.elementType, elementIndex))
				{
					value2.oGPVXKkaRuOYcNaLJIZFXXgWIxtx(value);
					eSbsYaKGZpVsnISkPHYjcxRLpYeD[i] = value2;
					return;
				}
			}
			eSbsYaKGZpVsnISkPHYjcxRLpYeD.Add(new qznnUAsCoSGpUBlBmvqxnRSYcwac(element.elementType, elementIndex, value));
		}

		internal void SetButtonValue(CustomControllerElementSelector element, bool value)
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao || element == null || !GetUseCustomController())
			{
				return;
			}
			Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
			if (customController == null)
			{
				return;
			}
			int elementIndex = element.GetElementIndex(customController);
			if (elementIndex < 0)
			{
				return;
			}
			_ = eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count;
			for (int i = 0; i < eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count; i++)
			{
				qznnUAsCoSGpUBlBmvqxnRSYcwac value2 = eSbsYaKGZpVsnISkPHYjcxRLpYeD[i];
				if (value2.AXRaAabAcDFslXRoZntGgwFsvHfcA(element.elementType, elementIndex))
				{
					value2.oGPVXKkaRuOYcNaLJIZFXXgWIxtx(value);
					eSbsYaKGZpVsnISkPHYjcxRLpYeD[i] = value2;
					return;
				}
			}
			eSbsYaKGZpVsnISkPHYjcxRLpYeD.Add(new qznnUAsCoSGpUBlBmvqxnRSYcwac(element.elementType, elementIndex, value));
		}

		internal void ClearElementValue(CustomControllerElementTargetSet targetSet)
		{
			if (targetSet != null)
			{
				int targetCount = targetSet.targetCount;
				for (int i = 0; i < targetCount; i++)
				{
					ClearElementValue(targetSet[i]);
				}
			}
		}

		internal void ClearElementValue(CustomControllerElementTarget target)
		{
			if (target != null)
			{
				ClearElementValue(target.element);
			}
		}

		internal void ClearElementValue(CustomControllerElementSelector element)
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao || element == null || !GetUseCustomController())
			{
				return;
			}
			Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
			if (customController == null)
			{
				return;
			}
			int elementIndex = element.GetElementIndex(customController);
			if (elementIndex < 0)
			{
				return;
			}
			switch (element.elementType)
			{
			case CustomControllerElementSelector.ElementType.Axis:
				customController.ClearAxisValue(elementIndex);
				break;
			case CustomControllerElementSelector.ElementType.Button:
				customController.ClearButtonValue(elementIndex);
				break;
			default:
				throw new NotImplementedException();
			}
			_ = eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count;
			for (int num = eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count - 1; num >= 0; num--)
			{
				if (eSbsYaKGZpVsnISkPHYjcxRLpYeD[num].AXRaAabAcDFslXRoZntGgwFsvHfcA(element.elementType, elementIndex))
				{
					eSbsYaKGZpVsnISkPHYjcxRLpYeD.RemoveAt(num);
				}
			}
		}

		internal int ElementExists_Editor(CustomControllerElementSelector element)
		{
			if (element == null)
			{
				return -1;
			}
			if (!element.isAssigned)
			{
				return -1;
			}
			if (_rewiredInputManager == null)
			{
				return -1;
			}
			if (!_customControllerSelector.findUsingSourceId)
			{
				return -1;
			}
			CustomController_Editor customControllerById = _rewiredInputManager.userData.GetCustomControllerById(_customControllerSelector.sourceId);
			if (customControllerById == null)
			{
				return -1;
			}
			switch (element.selectorType)
			{
			case CustomControllerElementSelector.SelectorType.Id:
				if (!customControllerById.ContainsElementIdentifier(element.elementId))
				{
					return 0;
				}
				return 1;
			case CustomControllerElementSelector.SelectorType.Index:
				switch (element.elementType)
				{
				case CustomControllerElementSelector.ElementType.Axis:
					if (element.elementIndex < 0 || element.elementIndex >= customControllerById.axisCount)
					{
						return 0;
					}
					return 1;
				case CustomControllerElementSelector.ElementType.Button:
					if (element.elementIndex < 0 || element.elementIndex >= customControllerById.buttonCount)
					{
						return 0;
					}
					return 1;
				default:
					throw new NotImplementedException();
				}
			case CustomControllerElementSelector.SelectorType.Name:
				if (!ArrayTools.Contains(customControllerById.GetElementIdentifierNames(), element.elementName))
				{
					return 0;
				}
				return 1;
			default:
				throw new NotImplementedException();
			}
		}

		internal bool ElementExists(CustomControllerElementSelector element)
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return false;
			}
			if (element == null)
			{
				return false;
			}
			Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
			if (customController == null)
			{
				return false;
			}
			return element.GetElementIndex(customController) >= 0;
		}

		internal bool ValidateElements(CustomControllerElementTargetSet targetSet)
		{
			if (targetSet == null)
			{
				return false;
			}
			bool flag = true;
			int targetCount = targetSet.targetCount;
			for (int i = 0; i < targetCount; i++)
			{
				flag &= ValidateElement(targetSet[i]);
			}
			return flag;
		}

		internal bool ValidateElement(CustomControllerElementTarget target)
		{
			if (target == null)
			{
				return false;
			}
			return ValidateElement(target.element);
		}

		internal bool ValidateElement(CustomControllerElementSelector element)
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return false;
			}
			if (!GetUseCustomController())
			{
				return false;
			}
			if (element == null)
			{
				return false;
			}
			if (!element.isAssigned)
			{
				return false;
			}
			Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
			if (customController == null)
			{
				return false;
			}
			if (!ElementExists(element))
			{
				Logger.LogWarning("No element found for " + element.GetSelectorFormattedString() + " in Custom Controller \"" + customController.name + "\"");
				return false;
			}
			return true;
		}

		private void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				eSbsYaKGZpVsnISkPHYjcxRLpYeD.Clear();
			}
		}

		private bool ajhXJbgnqLHNDTtpxLqCuAgGrnVb()
		{
			if (ReInput.isReady)
			{
				return true;
			}
			Logger.LogError("Rewired is not initialized. You must have an enabled Rewired Input Manager in the scene if using a Custom Controller. Custom Controller support will be disabled on this Custom Controller.");
			SetUseCustomController(value: false);
			return false;
		}

		private void akoAoSlMJbQxrneSGafGKgGyINti()
		{
			if (eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count == 0)
			{
				return;
			}
			Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
			if (customController == null)
			{
				eSbsYaKGZpVsnISkPHYjcxRLpYeD.Clear();
				return;
			}
			for (int i = 0; i < eSbsYaKGZpVsnISkPHYjcxRLpYeD.Count; i++)
			{
				qznnUAsCoSGpUBlBmvqxnRSYcwac qznnUAsCoSGpUBlBmvqxnRSYcwac2 = eSbsYaKGZpVsnISkPHYjcxRLpYeD[i];
				switch (qznnUAsCoSGpUBlBmvqxnRSYcwac2.MdjBbHAfEpOIyysKWtDJwilBqwCjA)
				{
				case CustomControllerElementSelector.ElementType.Axis:
					customController.SetAxisValue(qznnUAsCoSGpUBlBmvqxnRSYcwac2.fpRaWzdHfXPowZWkJxwTZPqeXOGK, qznnUAsCoSGpUBlBmvqxnRSYcwac2.yYOUbwIbBcyPAQvjeAEXVsjzLAln);
					break;
				case CustomControllerElementSelector.ElementType.Button:
					customController.SetButtonValue(qznnUAsCoSGpUBlBmvqxnRSYcwac2.fpRaWzdHfXPowZWkJxwTZPqeXOGK, qznnUAsCoSGpUBlBmvqxnRSYcwac2.yYOUbwIbBcyPAQvjeAEXVsjzLAln != 0f);
					break;
				default:
					throw new NotImplementedException();
				}
			}
			eSbsYaKGZpVsnISkPHYjcxRLpYeD.Clear();
		}

		private Rewired.CustomController UhFBEoKYbICWKGrfPbNBAMYDdcqzA(bool P_0)
		{
			if (!GetUseCustomController())
			{
				return null;
			}
			if (!ReInput.isReady)
			{
				return null;
			}
			Rewired.CustomController customController;
			if (VXJxyJaRzWDAeCgODeEzFozXCjuAb >= 0)
			{
				customController = ReInput.controllers.GetCustomController(VXJxyJaRzWDAeCgODeEzFozXCjuAb);
				if (customController == null)
				{
					VXJxyJaRzWDAeCgODeEzFozXCjuAb = -1;
				}
			}
			else
			{
				customController = null;
			}
			if (customController == null)
			{
				if (_createCustomControllerSettings.createCustomController)
				{
					customController = ReInput.controllers.CreateCustomController(_createCustomControllerSettings.customControllerSourceId);
					if (customController != null)
					{
						VXJxyJaRzWDAeCgODeEzFozXCjuAb = customController.id;
						zOwlIzMeAPOyxMZgBlumMKHsNAAq(customController);
					}
				}
				else
				{
					customController = _customControllerSelector.GetCustomController();
				}
			}
			if (P_0 && customController == null && GetUseCustomController())
			{
				Logger.LogWarning("No Custom Controller was found matching the search parameters.");
			}
			return customController;
		}

		private void zOwlIzMeAPOyxMZgBlumMKHsNAAq(Rewired.CustomController P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			if (_createCustomControllerSettings.assignToPlayerId == -1)
			{
				if (Application.isEditor)
				{
					Logger.LogWarning("The Custom Controller has not been assigned to any Player and will not be used for input until it is assigned. You should set the Player to assign it to in the inspector.");
				}
				return;
			}
			Player player = ReInput.players.GetPlayer(_createCustomControllerSettings.assignToPlayerId);
			if (player == null)
			{
				Logger.LogError("Invalid Player Id " + _createCustomControllerSettings.assignToPlayerId + ". Cannot assign Custom Controller to Player.");
			}
			else
			{
				player.controllers.AddController(P_0, removeFromOtherPlayers: true);
			}
		}

		private void wqpSDNioVwQwQGWdTeRRkteZueuX()
		{
			if (VXJxyJaRzWDAeCgODeEzFozXCjuAb >= 0 && _createCustomControllerSettings.destroyCustomController)
			{
				Rewired.CustomController customController = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(false);
				if (customController != null && ReInput.isReady)
				{
					ReInput.controllers.DestroyCustomController(customController);
					VXJxyJaRzWDAeCgODeEzFozXCjuAb = -1;
				}
			}
		}

		private void uZsvbYnQMPckbZUTVOMacdUKDPsr()
		{
			if (BbIMbQeLvHjivRxkUFEuuHyecJUe != null)
			{
				BbIMbQeLvHjivRxkUFEuuHyecJUe();
			}
			akoAoSlMJbQxrneSGafGKgGyINti();
		}
	}
}
