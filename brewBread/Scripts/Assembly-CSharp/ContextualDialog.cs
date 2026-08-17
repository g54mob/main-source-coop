using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ContextualDialog", menuName = "Dialogs/ContextualDialog", order = 1)]
public class ContextualDialog : ScriptableObject
{
	public LocalizedString[] Texts;
}
