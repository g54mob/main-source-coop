using Global.SerializableDictionary;
using UnityEngine;

namespace Features.GoogleFormModule.Scripts
{
	[CreateAssetMenu(fileName = "GoogleFormConfiguration", menuName = "Configurations/GoogleForm/GoogleFormConfiguration")]
	public class GoogleFormConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<ExternalLinkType, string> ExternalLinks { get; private set; }

		[field: SerializeField]
		public bool IsAutoFormOnQuitEnabled { get; private set; } = true;
	}
}
