using UnityEngine;

[CreateAssetMenu(menuName = "PS4/Region Settings", fileName = "PS4 Region Settings")]
public class PS4RegionSettings : ScriptableObject
{
	public string CONTENT_ID_EU = "";

	public string NPTITLE_DAT_EU = "";

	public string NPSECRET_HEX_EU = "";

	public string PARAM_SFX_EU = "";

	public int PARENTAL_LEVEL_EU;

	public string CONTENT_ID_NA = "";

	public string NPTITLE_DAT_NA = "";

	public string NPSECRET_HEX_NA = "";

	public string PARAM_SFX_NA = "";

	public int PARENTAL_LEVEL_NA;
}
