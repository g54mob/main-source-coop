using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFSW.QC.Extras
{
	public class KeyBinderModule : MonoBehaviour
	{
		private readonly struct Binding
		{
			public readonly KeyCode Key;

			public readonly string Command;

			public Binding(KeyCode key, string command)
			{
				Key = key;
				Command = command;
			}
		}

		private readonly List<Binding> _bindings = new List<Binding>();

		private QuantumConsole _consoleInstance;

		private bool _blocked;

		private void BlockInput()
		{
			_blocked = true;
		}

		private void UnblockInput()
		{
			_blocked = false;
		}

		private void BindToConsoleInstance()
		{
			if (!_consoleInstance)
			{
				_consoleInstance = UnityEngine.Object.FindFirstObjectByType<QuantumConsole>();
			}
			if ((bool)_consoleInstance)
			{
				_consoleInstance.OnActivate += BlockInput;
				_consoleInstance.OnDeactivate += UnblockInput;
				_blocked = _consoleInstance.IsActive;
			}
			else
			{
				UnblockInput();
			}
		}

		private void Awake()
		{
			BindToConsoleInstance();
		}

		private void Update()
		{
			if (_blocked)
			{
				return;
			}
			foreach (Binding binding in _bindings)
			{
				if (InputHelper.GetKeyDown(binding.Key))
				{
					try
					{
						QuantumConsoleProcessor.InvokeCommand(binding.Command);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		[Command("bind", MonoTargetType.Singleton, Platform.AllPlatforms)]
		[CommandDescription("Binds a given command to a given key, so that every time the key is pressed, the command is invoked.")]
		private void AddBinding(KeyCode key, string command)
		{
			_bindings.Add(new Binding(key, command));
		}

		[Command("unbind", MonoTargetType.Singleton, Platform.AllPlatforms)]
		[CommandDescription("Removes every binding for the given key")]
		private void RemoveBindings(KeyCode key)
		{
			_bindings.RemoveAll((Binding x) => x.Key == key);
		}

		[Command("unbind-all", MonoTargetType.Singleton, Platform.AllPlatforms)]
		[CommandDescription("Unbinds every existing key binding")]
		private void RemoveAllBindings()
		{
			_bindings.Clear();
		}

		[Command("display-bindings", MonoTargetType.Singleton, Platform.AllPlatforms)]
		[CommandDescription("Displays all existing bindings on the key binder")]
		private IEnumerable<object> DisplayAllBindings()
		{
			foreach (Binding item in _bindings.OrderBy((Binding x) => x.Key))
			{
				yield return new KeyValuePair<KeyCode, string>(item.Key, item.Command);
			}
		}
	}
}
