using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Player.Cosmetics
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Cosmetic Set", fileName = "NewCosmeticSet")]
	public class CosmeticSet : ScriptableObject
	{
		[SerializeField]
		private string setName;

		[SerializeField]
		private string feetCosmeticId;

		[SerializeField]
		private string lowerBodyCosmeticId;

		[SerializeField]
		private string upperBodyCosmeticId;

		[SerializeField]
		private string headCosmeticId;

		[SerializeField]
		private List<string> accessoryCosmeticIds = new List<string>();

		public string SetName => setName;

		public string FeetCosmeticId => feetCosmeticId;

		public string LowerBodyCosmeticId => lowerBodyCosmeticId;

		public string UpperBodyCosmeticId => upperBodyCosmeticId;

		public string HeadCosmeticId => headCosmeticId;

		public IReadOnlyList<string> AccessoryCosmeticIds => accessoryCosmeticIds;

		public void EditorSetValues(string newSetName, string feet, string lowerBody, string upperBody, string head, List<string> accessories)
		{
			setName = newSetName;
			feetCosmeticId = feet;
			lowerBodyCosmeticId = lowerBody;
			upperBodyCosmeticId = upperBody;
			headCosmeticId = head ?? "";
			accessoryCosmeticIds = ((accessories != null) ? new List<string>(accessories) : new List<string>());
		}

		private void OnValidate()
		{
			if (string.IsNullOrEmpty(feetCosmeticId))
			{
				Debug.LogWarning("[CosmeticSet] '" + base.name + "': Feet cosmetic ID is required");
			}
			if (string.IsNullOrEmpty(lowerBodyCosmeticId))
			{
				Debug.LogWarning("[CosmeticSet] '" + base.name + "': LowerBody cosmetic ID is required");
			}
			if (string.IsNullOrEmpty(upperBodyCosmeticId))
			{
				Debug.LogWarning("[CosmeticSet] '" + base.name + "': UpperBody cosmetic ID is required");
			}
		}
	}
}
