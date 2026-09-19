using UnityEngine;

namespace Features.DamageableTrackModule.Scripts
{
	public static class DamageDataSourceExtensions
	{
		public const string PlayerOwnerTypeName = "Player";

		public const string EnvironmentOwnerTypeName = "Environment";

		public const string UnknownOwnerTypeName = "Unknown";

		public static DamageSource ForEnemyAttack(Transform dealerTransform, string damageOwnerTypeName, DamageType damageType)
		{
			string prefabInstanceName = ((dealerTransform != null) ? SanitizeInstanceName(dealerTransform.gameObject.name) : string.Empty);
			return new DamageSource(DamageCauseCategory.Enemy, new DamageOwner(damageOwnerTypeName, prefabInstanceName), damageType);
		}

		public static DamageSource ForPlayerAttack(DamageType damageType)
		{
			return new DamageSource(DamageCauseCategory.Player, new DamageOwner("Player", string.Empty), damageType);
		}

		public static DamageSource ForEnvironment(DamageType damageType)
		{
			return new DamageSource(DamageCauseCategory.Environment, new DamageOwner("Environment", string.Empty), damageType);
		}

		public static DamageSource ForUnknown(DamageType damageType = DamageType.Unknown)
		{
			return new DamageSource(DamageCauseCategory.Unknown, new DamageOwner("Unknown", string.Empty), damageType);
		}

		public static string SanitizeInstanceName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}
			if (name.EndsWith("(Clone)"))
			{
				return name.Substring(0, name.Length - "(Clone)".Length).TrimEnd();
			}
			return name;
		}

		public static DamageRpcSource ToRpc(DamageSource source)
		{
			string text = ((source.Category == DamageCauseCategory.Enemy) ? TruncateForNetworkString(source.Owner.DamageOwnerTypeName, 16) : string.Empty);
			string text2 = ((source.Category == DamageCauseCategory.Enemy) ? TruncateForNetworkString(source.Owner.PrefabInstanceName, 32) : string.Empty);
			return new DamageRpcSource
			{
				Category = (byte)source.Category,
				DamageType = (byte)source.Type,
				OwnerTypeName = text,
				PrefabInstanceName = text2
			};
		}

		public static DamageSource FromRpc(DamageRpcSource rpc)
		{
			DamageCauseCategory category = (DamageCauseCategory)rpc.Category;
			DamageType damageType = (DamageType)rpc.DamageType;
			return category switch
			{
				DamageCauseCategory.Enemy => new DamageSource(DamageCauseCategory.Enemy, new DamageOwner(rpc.OwnerTypeName.ToString(), SanitizeInstanceName(rpc.PrefabInstanceName.ToString())), damageType), 
				DamageCauseCategory.Player => ForPlayerAttack(damageType), 
				DamageCauseCategory.Environment => ForEnvironment(damageType), 
				_ => ForUnknown(damageType), 
			};
		}

		private static string TruncateForNetworkString(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			string text = SanitizeInstanceName(value);
			if (text.Length > maxLength)
			{
				return text.Substring(0, maxLength);
			}
			return text;
		}
	}
}
