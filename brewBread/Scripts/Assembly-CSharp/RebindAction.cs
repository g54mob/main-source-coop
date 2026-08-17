using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;

public class RebindAction : MonoBehaviour
{
	[HideInInspector]
	public InputActionReference m_action;

	[HideInInspector]
	public string bindingGUI;

	public Animator animator;

	public string playerInputScheme;

	[HideInInspector]
	public UpdateBindingUIEvent onBindingDisplayUpdate;

	[Header("UI")]
	public TextMeshProUGUI Bindingtext;

	private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

	private void Start()
	{
		UpdateBindingDisplay();
	}

	public void StartRemaping()
	{
		if (!GetRemapingPar(out var action, out var bindingIndex))
		{
			return;
		}
		if (action.bindings[bindingIndex].isComposite)
		{
			int num = bindingIndex + 1;
			if (num < action.bindings.Count && action.bindings[num].isPartOfComposite)
			{
				RemapButtonClicked(action, num, isComposite: true);
			}
		}
		else
		{
			RemapButtonClicked(action, bindingIndex);
		}
	}

	public void ResetToDefault()
	{
		if (!GetRemapingPar(out var action, out var bindingIndex))
		{
			return;
		}
		if (action.bindings[bindingIndex].isComposite)
		{
			for (int i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; i++)
			{
				action.RemoveBindingOverride(i);
			}
		}
		else
		{
			action.RemoveBindingOverride(bindingIndex);
		}
	}

	private bool GetRemapingPar(out InputAction action, out int bindingIndex)
	{
		bindingIndex = -1;
		action = m_action.action;
		if (action == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(bindingGUI))
		{
			return false;
		}
		Guid bindingId = new Guid(bindingGUI);
		bindingIndex = action.bindings.IndexOf((InputBinding x) => x.id == bindingId);
		if (bindingIndex == -1)
		{
			Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
			return false;
		}
		return true;
	}

	public void UpdateBindingDisplay()
	{
		string text = string.Empty;
		string deviceLayoutName = null;
		string controlPath = null;
		InputAction inputAction = m_action?.action;
		if (inputAction != null)
		{
			int num = inputAction.bindings.IndexOf((InputBinding x) => x.id.ToString() == bindingGUI);
			if (num != -1)
			{
				text = inputAction.GetBindingDisplayString(num, out deviceLayoutName, out controlPath);
			}
		}
		MonoBehaviour.print("Control Path: " + controlPath);
		if (Bindingtext != null)
		{
			Bindingtext.text = text;
		}
		onBindingDisplayUpdate?.Invoke(this, text, deviceLayoutName, controlPath);
	}

	private void RemapButtonClicked(InputAction actionToRebind, int bindingIndex, bool isComposite = false)
	{
		rebindingOperation?.Cancel();
		LocalizeStringEvent localizeStringEvent = Bindingtext.transform.GetComponent<LocalizeStringEvent>();
		rebindingOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex).OnCancel(delegate
		{
			UpdateBindingDisplay();
			CleanUp();
			if (localizeStringEvent != null)
			{
				localizeStringEvent.enabled = false;
			}
		}).OnComplete(delegate
		{
			animator.Play("Normal");
			CleanUp();
			if (isComposite)
			{
				int num = bindingIndex + 1;
				if (num < actionToRebind.bindings.Count && actionToRebind.bindings[num].isPartOfComposite)
				{
					RemapButtonClicked(actionToRebind, num, isComposite: true);
				}
				else
				{
					UpdateBindingDisplay();
					if (localizeStringEvent != null)
					{
						localizeStringEvent.enabled = false;
					}
				}
			}
			else
			{
				UpdateBindingDisplay();
				if (localizeStringEvent != null)
				{
					localizeStringEvent.enabled = false;
				}
			}
		});
		animator.Play("Pressed");
		rebindingOperation.OnMatchWaitForAnother(0.1f).Start();
		void CleanUp()
		{
			rebindingOperation?.Dispose();
			rebindingOperation = null;
		}
	}

	public void EnableLocale()
	{
		LocalizeStringEvent component = Bindingtext.transform.GetComponent<LocalizeStringEvent>();
		if (component != null)
		{
			component.enabled = true;
		}
	}
}
