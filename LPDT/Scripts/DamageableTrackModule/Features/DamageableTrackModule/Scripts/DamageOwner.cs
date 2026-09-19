namespace Features.DamageableTrackModule.Scripts
{
	public readonly struct DamageOwner
	{
		public string DamageOwnerTypeName { get; }

		public string PrefabInstanceName { get; }

		public DamageOwner(string damageOwnerTypeName, string prefabInstanceName)
		{
			DamageOwnerTypeName = damageOwnerTypeName ?? string.Empty;
			PrefabInstanceName = prefabInstanceName ?? string.Empty;
		}
	}
}
