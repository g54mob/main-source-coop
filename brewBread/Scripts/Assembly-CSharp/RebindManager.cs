using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class RebindManager : MonoBehaviour
{
	[SerializeField]
	private InputActionAsset[] inputActions;

	private void Awake()
	{
		LoadInputs();
	}

	private void OnDestroy()
	{
		SaveInputs();
	}

	public void SaveInputs()
	{
		for (int i = 0; i < inputActions.Length; i++)
		{
			foreach (InputActionMap actionMap in inputActions[i].actionMaps)
			{
				foreach (InputBinding binding in actionMap.bindings)
				{
					if (!string.IsNullOrEmpty(binding.overridePath))
					{
						string key = binding.id.ToString();
						string overridePath = binding.overridePath;
						PlayerPrefs.SetString(key, overridePath);
					}
				}
			}
		}
	}

	public void LoadInputs()
	{
		for (int i = 0; i < inputActions.Length; i++)
		{
			foreach (InputActionMap actionMap in inputActions[i].actionMaps)
			{
				ReadOnlyArray<InputBinding> bindings = actionMap.bindings;
				for (int j = 0; j < bindings.Count; j++)
				{
					string text = PlayerPrefs.GetString(bindings[j].id.ToString(), null);
					if (!string.IsNullOrEmpty(text))
					{
						actionMap.ApplyBindingOverride(j, new InputBinding
						{
							overridePath = text
						});
					}
				}
			}
		}
	}
}
