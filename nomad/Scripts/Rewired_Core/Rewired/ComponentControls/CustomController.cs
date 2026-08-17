using System;
using System.Collections.Generic;
using Rewired.ComponentControls.Data;
using Rewired.Data;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[AddComponentMenu("Rewired/Component Controls/Custom Controller")]
	public class CustomController : ComponentController
	{
		[Serializable]
		public class CreateCustomControllerSettings
		{
			[Tooltip("If true, a new Custom Controller will be created. Otherwise, an existing Custom Controller will be found using the selector properties.")]
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private bool _createCustomController = true;

			[Tooltip("The source id of the Custom Controller to create. Get this from the Rewired Input Manager.")]
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private int _customControllerSourceId = -1;

			[Tooltip("The Player that will be assigned this Custom Controller when it is created.")]
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private int _assignToPlayerId;

			[Tooltip("If true, the Custom Controller created by this component will be destroyed when this component is destroyed.")]
			[SerializeField]
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

		private struct IoNfIOjKzszleozUYMjooWGaYabpA
		{
			public CustomControllerElementSelector.ElementType MWRvQjwIYRxktGVuSYUACFfTiucR;

			public int YyICxkpnipnEfSUrKsYACXWljZqI;

			public float qGSAzwdiAYhmvqfryQoEnmNQPqpe;

			public IoNfIOjKzszleozUYMjooWGaYabpA(CustomControllerElementSelector.ElementType P_0, int P_1, float P_2)
			{
				MWRvQjwIYRxktGVuSYUACFfTiucR = P_0;
				YyICxkpnipnEfSUrKsYACXWljZqI = P_1;
				qGSAzwdiAYhmvqfryQoEnmNQPqpe = P_2;
			}

			public IoNfIOjKzszleozUYMjooWGaYabpA(CustomControllerElementSelector.ElementType P_0, int P_1, bool P_2)
			{
				MWRvQjwIYRxktGVuSYUACFfTiucR = P_0;
				YyICxkpnipnEfSUrKsYACXWljZqI = P_1;
				qGSAzwdiAYhmvqfryQoEnmNQPqpe = (P_2 ? 1f : 0f);
			}

			public bool sKVcvhlufQNlQHyxfSGVzUygOsfp(CustomControllerElementSelector.ElementType P_0, int P_1)
			{
				if (MWRvQjwIYRxktGVuSYUACFfTiucR == P_0)
				{
					return YyICxkpnipnEfSUrKsYACXWljZqI == P_1;
				}
				return false;
			}

			public void LCopXRNjHbSOLhdrikNfFfQkaCds(float P_0)
			{
				qGSAzwdiAYhmvqfryQoEnmNQPqpe = MathTools.MaxMagnitude(qGSAzwdiAYhmvqfryQoEnmNQPqpe, P_0);
			}

			public void iZNCOeuHTVibhuHOiGuAtETMlsAj(bool P_0)
			{
				if (P_0)
				{
					qGSAzwdiAYhmvqfryQoEnmNQPqpe = 1f;
				}
			}
		}

		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Custom Controller elements, etc.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private InputManager_Base _rewiredInputManager;

		[Tooltip("Contains search parameters to find a particular Custom Controller.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerSelector _customControllerSelector = new CustomControllerSelector();

		[Tooltip("Settings for creating a Custom Controller on start.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CreateCustomControllerSettings _createCustomControllerSettings = new CreateCustomControllerSettings();

		private List<IoNfIOjKzszleozUYMjooWGaYabpA> pyKsqGRpcYhfnjOStSYFqyGXlprmA = new List<IoNfIOjKzszleozUYMjooWGaYabpA>(10);

		[NonSerialized]
		private int fjNrdCpBnWHKXExSovRzWwiorzRjA = -1;

		private Action glMVmaqVAAsjgtqJJwBvomUWwayj;

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
					LmgKRLWTDvOaJygdzlywslXymick();
				}
			}
		}

		public CustomControllerSelector customControllerSelector => _customControllerSelector;

		public CreateCustomControllerSettings createCustomControllerSettings => _createCustomControllerSettings;

		internal event Action InputSourceUpdateEvent
		{
			add
			{
				glMVmaqVAAsjgtqJJwBvomUWwayj = (Action)Delegate.Combine(glMVmaqVAAsjgtqJJwBvomUWwayj, value);
			}
			remove
			{
				glMVmaqVAAsjgtqJJwBvomUWwayj = (Action)Delegate.Remove(glMVmaqVAAsjgtqJJwBvomUWwayj, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal CustomController()
		{
		}

		public Rewired.CustomController GetCustomController()
		{
			return FuCwshemnIDCDgCOVzKttjWoKAOh(false);
		}

		[CustomObfuscation(rename = false)]
		internal override void OnEnable()
		{
			base.OnEnable();
			_ = base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS;
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			base.OnDisable();
			if (base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS)
			{
				pyKsqGRpcYhfnjOStSYFqyGXlprmA.Clear();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS)
			{
				LmgKRLWTDvOaJygdzlywslXymick();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDestroy()
		{
			base.OnDestroy();
			sLuDIJEguWofYGGAWsyqcqfASIHMc();
		}

		internal virtual bool OnInitialize()
		{
			if (!base.nOOsAqwpQqySAZWspjurFNceJHMR())
			{
				return false;
			}
			if (GetUseCustomController())
			{
				if (!xmYdBWZwMbHkjiYdzSZNmcCvjLaPA())
				{
					return false;
				}
				if (FuCwshemnIDCDgCOVzKttjWoKAOh(true) == null)
				{
					SetUseCustomController(value: false);
				}
			}
			return true;
		}

		internal virtual void OnSubscribeEvents()
		{
			base.QTVSlGdkixxffTWWemtJmqnfNogq();
			CqiGYPTZeUpHdwdRKzVRvGmXZFAt();
			if (ReInput.isReady)
			{
				ReInput.InputSourceUpdateEvent += sOpsMILkonIBHflQuSgZtrtiGAYQ;
			}
		}

		internal virtual void OnUnsubscribeEvents()
		{
			base.CqiGYPTZeUpHdwdRKzVRvGmXZFAt();
			ReInput.InputSourceUpdateEvent -= sOpsMILkonIBHflQuSgZtrtiGAYQ;
		}

		public override void ClearControlValues()
		{
			base.ClearControlValues();
			if (base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS)
			{
				pyKsqGRpcYhfnjOStSYFqyGXlprmA.Clear();
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
			if (!base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS || element == null || !GetUseCustomController())
			{
				return;
			}
			Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
			if (customController == null)
			{
				return;
			}
			int elementIndex = element.GetElementIndex(customController);
			if (elementIndex < 0)
			{
				return;
			}
			_ = pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count;
			for (int i = 0; i < pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count; i++)
			{
				IoNfIOjKzszleozUYMjooWGaYabpA value2 = pyKsqGRpcYhfnjOStSYFqyGXlprmA[i];
				if (value2.sKVcvhlufQNlQHyxfSGVzUygOsfp(element.elementType, elementIndex))
				{
					value2.LCopXRNjHbSOLhdrikNfFfQkaCds(value);
					pyKsqGRpcYhfnjOStSYFqyGXlprmA[i] = value2;
					return;
				}
			}
			pyKsqGRpcYhfnjOStSYFqyGXlprmA.Add(new IoNfIOjKzszleozUYMjooWGaYabpA(element.elementType, elementIndex, value));
		}

		internal void SetButtonValue(CustomControllerElementSelector element, bool value)
		{
			if (!base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS || element == null || !GetUseCustomController())
			{
				return;
			}
			Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
			if (customController == null)
			{
				return;
			}
			int elementIndex = element.GetElementIndex(customController);
			if (elementIndex < 0)
			{
				return;
			}
			_ = pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count;
			for (int i = 0; i < pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count; i++)
			{
				IoNfIOjKzszleozUYMjooWGaYabpA value2 = pyKsqGRpcYhfnjOStSYFqyGXlprmA[i];
				if (value2.sKVcvhlufQNlQHyxfSGVzUygOsfp(element.elementType, elementIndex))
				{
					value2.iZNCOeuHTVibhuHOiGuAtETMlsAj(value);
					pyKsqGRpcYhfnjOStSYFqyGXlprmA[i] = value2;
					return;
				}
			}
			pyKsqGRpcYhfnjOStSYFqyGXlprmA.Add(new IoNfIOjKzszleozUYMjooWGaYabpA(element.elementType, elementIndex, value));
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
			if (!base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS || element == null || !GetUseCustomController())
			{
				return;
			}
			Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
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
			_ = pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count;
			for (int num = pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count - 1; num >= 0; num--)
			{
				if (pyKsqGRpcYhfnjOStSYFqyGXlprmA[num].sKVcvhlufQNlQHyxfSGVzUygOsfp(element.elementType, elementIndex))
				{
					pyKsqGRpcYhfnjOStSYFqyGXlprmA.RemoveAt(num);
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
				return customControllerById.ContainsElementIdentifier(element.elementId) ? 1 : 0;
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
				return ArrayTools.Contains(customControllerById.GetElementIdentifierNames(), element.elementName) ? 1 : 0;
			default:
				throw new NotImplementedException();
			}
		}

		internal bool ElementExists(CustomControllerElementSelector element)
		{
			if (!base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS)
			{
				return false;
			}
			if (element == null)
			{
				return false;
			}
			Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
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
			if (!base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS)
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
			Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
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

		private void LmgKRLWTDvOaJygdzlywslXymick()
		{
			if (base.qnvpaSZJXiOHXHHvNKlMjyoMoMcS)
			{
				pyKsqGRpcYhfnjOStSYFqyGXlprmA.Clear();
			}
		}

		private bool xmYdBWZwMbHkjiYdzSZNmcCvjLaPA()
		{
			if (ReInput.isReady)
			{
				return true;
			}
			Logger.LogError("Rewired is not initialized. You must have an enabled Rewired Input Manager in the scene if using a Custom Controller. Custom Controller support will be disabled on this Custom Controller.");
			SetUseCustomController(value: false);
			return false;
		}

		private void mbuukicDDqHRWBhyjnpinhKHYoVJ()
		{
			if (pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count == 0)
			{
				return;
			}
			Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
			if (customController == null)
			{
				pyKsqGRpcYhfnjOStSYFqyGXlprmA.Clear();
				return;
			}
			for (int i = 0; i < pyKsqGRpcYhfnjOStSYFqyGXlprmA.Count; i++)
			{
				IoNfIOjKzszleozUYMjooWGaYabpA ioNfIOjKzszleozUYMjooWGaYabpA = pyKsqGRpcYhfnjOStSYFqyGXlprmA[i];
				switch (ioNfIOjKzszleozUYMjooWGaYabpA.MWRvQjwIYRxktGVuSYUACFfTiucR)
				{
				case CustomControllerElementSelector.ElementType.Axis:
					customController.SetAxisValue(ioNfIOjKzszleozUYMjooWGaYabpA.YyICxkpnipnEfSUrKsYACXWljZqI, ioNfIOjKzszleozUYMjooWGaYabpA.qGSAzwdiAYhmvqfryQoEnmNQPqpe);
					break;
				case CustomControllerElementSelector.ElementType.Button:
					customController.SetButtonValue(ioNfIOjKzszleozUYMjooWGaYabpA.YyICxkpnipnEfSUrKsYACXWljZqI, ioNfIOjKzszleozUYMjooWGaYabpA.qGSAzwdiAYhmvqfryQoEnmNQPqpe != 0f);
					break;
				default:
					throw new NotImplementedException();
				}
			}
			pyKsqGRpcYhfnjOStSYFqyGXlprmA.Clear();
		}

		private Rewired.CustomController FuCwshemnIDCDgCOVzKttjWoKAOh(bool P_0)
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
			if (fjNrdCpBnWHKXExSovRzWwiorzRjA >= 0)
			{
				customController = ReInput.controllers.GetCustomController(fjNrdCpBnWHKXExSovRzWwiorzRjA);
				if (customController == null)
				{
					fjNrdCpBnWHKXExSovRzWwiorzRjA = -1;
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
						fjNrdCpBnWHKXExSovRzWwiorzRjA = customController.id;
						iGInKuOiWQBhbGXTQMakpjILwyBT(customController);
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

		private void iGInKuOiWQBhbGXTQMakpjILwyBT(Rewired.CustomController P_0)
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

		private void sLuDIJEguWofYGGAWsyqcqfASIHMc()
		{
			if (fjNrdCpBnWHKXExSovRzWwiorzRjA >= 0 && _createCustomControllerSettings.destroyCustomController)
			{
				Rewired.CustomController customController = FuCwshemnIDCDgCOVzKttjWoKAOh(false);
				if (customController != null && ReInput.isReady)
				{
					ReInput.controllers.DestroyCustomController(customController);
					fjNrdCpBnWHKXExSovRzWwiorzRjA = -1;
				}
			}
		}

		private void sOpsMILkonIBHflQuSgZtrtiGAYQ()
		{
			if (glMVmaqVAAsjgtqJJwBvomUWwayj != null)
			{
				glMVmaqVAAsjgtqJJwBvomUWwayj();
			}
			mbuukicDDqHRWBhyjnpinhKHYoVJ();
		}
	}
}
