using System.Reflection;
using Steamworks;
using UnityEngine;

public class PluginPublished : Plugin
{
	public SteamUGCDetails_t steamUgcDetails;

	public string steamMetadata;

	public override PluginInfo? InfoFromAssembly => null;

	public override Assembly Assembly => null;

	public override string DisplayName => steamUgcDetails.m_rgchTitle;

	public override bool Publishable => false;

	public override PublishedFileId_t? PublishedFileId => steamUgcDetails.m_nPublishedFileId;

	public override string SteamMetadata => steamMetadata;

	public PluginPublished(SteamUGCDetails_t steamUgcDetails, string steamMetadata)
	{
		Debug.Log($"Found published workshop item: {steamUgcDetails.m_nPublishedFileId}, title: {steamUgcDetails.m_rgchTitle}, metadata: {steamMetadata}");
		this.steamUgcDetails = steamUgcDetails;
		this.steamMetadata = steamMetadata;
	}
}
