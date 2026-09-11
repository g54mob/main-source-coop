using UnityEngine;
using Zorro.Core;

[CreateAssetMenu(menuName = "Database/PropEventDatabase", order = 9999, fileName = "PropContentDatabase")]
public class PropContentDatabase : ObjectDatabaseAsset<PropContentDatabase, PropContent>
{
	public static PropContent GetEntryFromID(ushort id)
	{
		foreach (PropContent @object in SingletonAsset<PropContentDatabase>.Instance.Objects)
		{
			if (@object.id == id)
			{
				return @object;
			}
		}
		return null;
	}
}
