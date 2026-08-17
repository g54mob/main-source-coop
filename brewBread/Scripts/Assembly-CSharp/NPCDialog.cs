using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs/Dialog", order = 1)]
public class NPCDialog : ScriptableObject
{
	public List<Dialog> Dialogs = new List<Dialog>();
}
