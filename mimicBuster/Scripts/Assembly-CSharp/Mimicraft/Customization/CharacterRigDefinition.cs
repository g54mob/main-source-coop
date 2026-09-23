using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Customization
{
	[CreateAssetMenu(fileName = "NewCharacterRig", menuName = "Mimicraft/Character Rig")]
	public class CharacterRigDefinition : ScriptableObject
	{
		[Tooltip("Ağa giden sabit kimlik. Kaydedilen bir karakter hangi rig'e ait olduğunu bununla söylüyor, böylece ileride silah rig'i geldiğinde ikisi karışmıyor.")]
		[SerializeField]
		private string rigId = "character";

		[Tooltip("Bu rig'i oluşturan parçalar. Sıra menüdeki listeyi belirliyor.")]
		[SerializeField]
		private List<CharacterPartDefinition> parts = new List<CharacterPartDefinition>();

		[Tooltip("Bütün parçalar toplamında en fazla kaç voxel olabileceği. Hem performans hem de sunucunun kabul edeceğinin üst sınırı.")]
		[SerializeField]
		[Min(1f)]
		private int maxTotalVoxels = 20000;

		[Tooltip("Hiç karakter yapmamış bir oyuncunun görüneceği karakter - projeye attığın bir .character dosyası. Boş bırakılırsa özelleştirmemiş oyuncu varsayılan skinned modelde kalır ve Customization'da 'Yeni' dolu bloklarla başlar.")]
		[SerializeField]
		private CharacterAsset defaultCharacter;

		[Tooltip("Hazir karakterler - 'Yeni' denince cikan liste. Default Character ile ayni turden .character dosyalari; fark, birinin hicbir seyi olmayan oyuncunun gorunusu olmasi, bunlarin ise secenek olmasi. Bos birakilabilir - o zaman 'Yeni' dogrudan bos karakteri acar.")]
		[SerializeField]
		private List<CharacterAssetDefinition> presets = new List<CharacterAssetDefinition>();

		private List<CharacterPartDefinition> validated;

		public string RigId => rigId;

		public CharacterAsset DefaultCharacter => defaultCharacter;

		public IReadOnlyList<CharacterAssetDefinition> Presets => presets;

		public int MaxTotalVoxels => maxTotalVoxels;

		public int EffectiveMaxTotalVoxels
		{
			get
			{
				if (maxTotalVoxels <= 0)
				{
					return maxTotalVoxels;
				}
				double num = 0.0;
				double num2 = 0.0;
				foreach (CharacterPartDefinition part in Parts)
				{
					if (!(part == null))
					{
						Vector3Int authoredBoxSize = part.AuthoredBoxSize;
						double num3 = (double)authoredBoxSize.x * (double)authoredBoxSize.y * (double)authoredBoxSize.z;
						int boxSizeMultiplier = part.BoxSizeMultiplier;
						num += num3;
						num2 += num3 * (double)boxSizeMultiplier * (double)boxSizeMultiplier * (double)boxSizeMultiplier;
					}
				}
				if (num <= 0.0)
				{
					return maxTotalVoxels;
				}
				return (int)Math.Min(2147483647.0, Math.Ceiling((double)maxTotalVoxels * (num2 / num)));
			}
		}

		public IReadOnlyList<CharacterPartDefinition> Parts
		{
			get
			{
				if (validated != null)
				{
					return validated;
				}
				List<CharacterPartDefinition> list = new List<CharacterPartDefinition>(parts.Count);
				foreach (CharacterPartDefinition part in parts)
				{
					if (!(part == null))
					{
						if (!part.IsUsable)
						{
							Debug.LogWarning("[CharacterRigDefinition] '" + base.name + "' icindeki '" + part.name + "' kullanilamaz (Part Id bos ya da kutu boyutu sifir) - listeye alinmadi.", this);
						}
						else
						{
							list.Add(part);
						}
					}
				}
				validated = list;
				return validated;
			}
		}

		public CharacterPartDefinition Find(string partId)
		{
			if (string.IsNullOrWhiteSpace(partId))
			{
				return null;
			}
			foreach (CharacterPartDefinition part in Parts)
			{
				if (part.PartId == partId)
				{
					return part;
				}
			}
			return null;
		}

		private void OnValidate()
		{
			validated = null;
		}
	}
}
