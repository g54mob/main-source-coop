using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EvilCore.Inputs
{
	public class InputRemappingDebugger : MonoBehaviour
	{
		[Header("Dependencies")]
		[SerializeField]
		private InputRemappingService remappingService;

		private string _listeningFor = "—";

		private string _lastResult = "";

		[SerializeField]
		private int selectedCategory = 2;

		private List<string> _currentBindings = new List<string>();

		[SerializeField]
		private int _selectedActionIndex = -1;

		private string _selectedCurrentKey = "—";

		private string _selectedActionInfo = "—";

		private List<InputRemappingActionData> _cachedActions = new List<InputRemappingActionData>();

		private bool _subscribed;

		private bool IsListening
		{
			get
			{
				if (remappingService != null)
				{
					return remappingService.IsListening;
				}
				return false;
			}
		}

		private void RefreshBindings()
		{
			if (remappingService == null)
			{
				return;
			}
			_cachedActions = remappingService.GetActionsForCategory(selectedCategory);
			_currentBindings.Clear();
			foreach (InputRemappingActionData cachedAction in _cachedActions)
			{
				string arg = (string.IsNullOrEmpty(cachedAction.CurrentBindingName) ? "(none)" : cachedAction.CurrentBindingName);
				_currentBindings.Add($"{cachedAction.ActionName,-20} [{arg}]");
			}
			_selectedActionIndex = -1;
			_selectedCurrentKey = "—";
			_selectedActionInfo = "—";
		}

		private void StartKBMouseRemap()
		{
			if (!(remappingService == null) && _selectedActionIndex >= 0 && _selectedActionIndex < _cachedActions.Count)
			{
				SubscribeEvents();
				InputRemappingActionData inputRemappingActionData = _cachedActions[_selectedActionIndex];
				_listeningFor = "Waiting for key: " + inputRemappingActionData.ActionName + " (current: " + inputRemappingActionData.CurrentBindingName + ")";
				remappingService.StartKeyboardMouseRemapping(inputRemappingActionData.ActionId, inputRemappingActionData.AxisRange, inputRemappingActionData.ActionElementMapId);
			}
		}

		private void StartJoystickRemap()
		{
			if (!(remappingService == null) && _selectedActionIndex >= 0 && _selectedActionIndex < _cachedActions.Count)
			{
				SubscribeEvents();
				InputRemappingActionData inputRemappingActionData = _cachedActions[_selectedActionIndex];
				_listeningFor = "Waiting for button: " + inputRemappingActionData.ActionName + " (current: " + inputRemappingActionData.CurrentBindingName + ")";
				remappingService.StartJoystickRemapping(inputRemappingActionData.ActionId, inputRemappingActionData.AxisRange, inputRemappingActionData.ActionElementMapId);
			}
		}

		private void Cancel()
		{
			remappingService?.CancelListening();
			_listeningFor = "—";
		}

		private void Save()
		{
			remappingService?.SaveBindings();
		}

		private void Load()
		{
			remappingService?.LoadBindings();
		}

		private void ResetDefaults()
		{
			remappingService?.ResetToDefaults();
		}

		private void OnCategoryChanged()
		{
			if (Application.isPlaying)
			{
				RefreshBindings();
			}
		}

		private IEnumerable<ValueDropdownItem<int>> GetActionOptions()
		{
			for (int i = 0; i < _cachedActions.Count; i++)
			{
				InputRemappingActionData inputRemappingActionData = _cachedActions[i];
				string text = (string.IsNullOrEmpty(inputRemappingActionData.CurrentBindingName) ? "none" : inputRemappingActionData.CurrentBindingName);
				yield return new ValueDropdownItem<int>(inputRemappingActionData.ActionName + " [" + text + "]", i);
			}
		}

		private void OnActionSelected()
		{
			if (_selectedActionIndex < 0 || _selectedActionIndex >= _cachedActions.Count)
			{
				_selectedCurrentKey = "—";
				_selectedActionInfo = "—";
			}
			else
			{
				InputRemappingActionData inputRemappingActionData = _cachedActions[_selectedActionIndex];
				_selectedCurrentKey = (string.IsNullOrEmpty(inputRemappingActionData.CurrentBindingName) ? "(none)" : inputRemappingActionData.CurrentBindingName);
				_selectedActionInfo = $"ID: {inputRemappingActionData.ActionId} | AEM: {inputRemappingActionData.ActionElementMapId} | {inputRemappingActionData.AxisRange}";
			}
		}

		private void SubscribeEvents()
		{
			if (!_subscribed && !(remappingService == null))
			{
				_subscribed = true;
				remappingService.OnInputRemapped += OnRemapped;
				remappingService.OnListeningStopped += OnStopped;
			}
		}

		private void OnDisable()
		{
			if (_subscribed && !(remappingService == null))
			{
				remappingService.OnInputRemapped -= OnRemapped;
				remappingService.OnListeningStopped -= OnStopped;
				_subscribed = false;
			}
		}

		private void OnRemapped(InputRemappingResult result)
		{
			_lastResult = $"{result.ActionName} → {result.NewElementName} ({result.ControllerType})";
			_listeningFor = "—";
			RefreshBindings();
		}

		private void OnStopped()
		{
			if (string.IsNullOrEmpty(_lastResult))
			{
				_lastResult = "Stopped (timeout or cancelled)";
			}
			_listeningFor = "—";
		}

		private static IEnumerable<ValueDropdownItem<int>> GetCategoryOptions()
		{
			return new List<ValueDropdownItem<int>>
			{
				new ValueDropdownItem<int>("Base", 1),
				new ValueDropdownItem<int>("On Foot", 2),
				new ValueDropdownItem<int>("Driving", 3),
				new ValueDropdownItem<int>("Equipping", 4),
				new ValueDropdownItem<int>("Placement", 5),
				new ValueDropdownItem<int>("Sitting", 6)
			};
		}
	}
}
