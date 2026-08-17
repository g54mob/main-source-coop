using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootItemDefinition : MonoBehaviour
	{
		[Header("Item Identity")]
		[Tooltip("The category this loot belongs to. Used for editor visualization and filtering.")]
		[SerializeField]
		private LootCategory _category = LootCategory.Misc;

		[Tooltip("Display name shown in editor. If empty, uses GameObject name.")]
		[SerializeField]
		private string _displayName;

		public LootCategory Category => _category;

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(_displayName))
				{
					return _displayName;
				}
				return base.gameObject.name;
			}
		}
	}
}
