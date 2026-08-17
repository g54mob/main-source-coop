using System;
using UnityEngine.Events;

[Serializable]
public class UpdateBindingUIEvent : UnityEvent<RebindAction, string, string, string>
{
}
