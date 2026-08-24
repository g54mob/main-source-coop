using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.API
{
	public static class Friends
	{
		public static class Client
		{
			private class ImageRequestCallbackLink
			{
				public UserData owner;

				public Action<Texture2D> callback;
			}

			private static GameConnectedFriendChatMsgEvent eventFriendMessageReceived = new GameConnectedFriendChatMsgEvent();

			private static bool listeningForFriendMessages = false;

			private static PersonaStateChangeEvent eventPersonaStateChange = new PersonaStateChangeEvent();

			private static FriendRichPresenceUpdateEvent eventFriendRichPresenceUpdate = new FriendRichPresenceUpdateEvent();

			private static List<ImageRequestCallbackLink> pendingLinks = new List<ImageRequestCallbackLink>();

			private static Dictionary<int, Texture2D> loadedImages = new Dictionary<int, Texture2D>();

			private static Dictionary<UserData, Texture2D> userAvatarMapping = new Dictionary<UserData, Texture2D>();

			private static CallResult<FriendsEnumerateFollowingList_t> m_FriendsEnumerateFollowingList_t;

			private static CallResult<FriendsGetFollowerCount_t> m_FriendsGetFollowerCount_t;

			private static CallResult<FriendsIsFollowing_t> m_FriendsIsFollowing_t;

			private static CallResult<SetPersonaNameResponse_t> m_SetPersonaNameResponse_t;

			private static Callback<GameConnectedFriendChatMsg_t> m_GameConnectedFriendChatMsg_t;

			private static Callback<AvatarImageLoaded_t> m_AvatarImageLoaded_t;

			private static Callback<PersonaStateChange_t> m_PersonaStateChange_t;

			private static Callback<FriendRichPresenceUpdate_t> m_FriendRichPresenceUpdate_t;

			private static bool loadingFollowed = false;

			public static GameConnectedFriendChatMsgEvent EventGameConnectedFriendChatMsg
			{
				get
				{
					if (m_GameConnectedFriendChatMsg_t == null)
					{
						m_GameConnectedFriendChatMsg_t = Callback<GameConnectedFriendChatMsg_t>.Create(delegate(GameConnectedFriendChatMsg_t result)
						{
							if (SteamFriends.GetFriendMessage(result.m_steamIDUser, result.m_iMessageID, out var pvData, 8193, out var peChatEntryType) > 0)
							{
								eventFriendMessageReceived.Invoke(result.m_steamIDUser, pvData, peChatEntryType);
							}
						});
					}
					return eventFriendMessageReceived;
				}
			}

			public static FriendRichPresenceUpdateEvent EventFriendRichPresenceUpdate
			{
				get
				{
					if (m_FriendRichPresenceUpdate_t == null)
					{
						m_FriendRichPresenceUpdate_t = Callback<FriendRichPresenceUpdate_t>.Create(delegate(FriendRichPresenceUpdate_t r)
						{
							eventFriendRichPresenceUpdate.Invoke(r);
						});
					}
					return eventFriendRichPresenceUpdate;
				}
			}

			public static PersonaStateChangeEvent EventPersonaStateChange
			{
				get
				{
					if (m_PersonaStateChange_t == null)
					{
						m_PersonaStateChange_t = Callback<PersonaStateChange_t>.Create(HandlePersonaStateChange);
					}
					return eventPersonaStateChange;
				}
			}

			public static bool ListenForFriendsMessages
			{
				get
				{
					return listeningForFriendMessages;
				}
				set
				{
					SteamFriends.SetListenForFriendsMessages(value);
					listeningForFriendMessages = value;
				}
			}

			public static string PersonaName => SteamFriends.GetPersonaName();

			public static EPersonaState PersonaState => SteamFriends.GetPersonaState();

			public static uint Restrictions => SteamFriends.GetUserRestrictions();

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				eventFriendMessageReceived = new GameConnectedFriendChatMsgEvent();
				listeningForFriendMessages = false;
				eventPersonaStateChange = new PersonaStateChangeEvent();
				eventFriendRichPresenceUpdate = new FriendRichPresenceUpdateEvent();
				pendingLinks = new List<ImageRequestCallbackLink>();
				if (loadedImages.Count > 0)
				{
					UnloadAvatarImages();
				}
				loadedImages = new Dictionary<int, Texture2D>();
				userAvatarMapping = new Dictionary<UserData, Texture2D>();
				m_FriendsEnumerateFollowingList_t = null;
				m_FriendsGetFollowerCount_t = null;
				m_FriendsIsFollowing_t = null;
				m_SetPersonaNameResponse_t = null;
				m_GameConnectedFriendChatMsg_t = null;
				m_AvatarImageLoaded_t = null;
				m_PersonaStateChange_t = null;
				m_FriendRichPresenceUpdate_t = null;
			}

			public static void ClearRichPresence()
			{
				SteamFriends.ClearRichPresence();
			}

			public static void GetFollowed(Action<CSteamID[]> callback)
			{
				if (callback == null)
				{
					return;
				}
				BackgroundWorker bgWorker = new BackgroundWorker();
				bgWorker.DoWork += delegate(object sender, DoWorkEventArgs e)
				{
					if (!loadingFollowed)
					{
						loadingFollowed = true;
						int read = 0;
						int total = 0;
						bool waiting = true;
						bool hasError = false;
						List<UserData> followedIds = new List<UserData>();
						EnumerateFollowingList(0u, delegate(FriendsEnumerateFollowingList_t r, bool flag)
						{
							if (!flag)
							{
								foreach (CSteamID item in r.m_rgSteamID.Where((CSteamID p) => p != CSteamID.Nil))
								{
									followedIds.Add(item);
								}
								total = r.m_nTotalResultCount;
								read = r.m_nResultsReturned;
							}
							else
							{
								hasError = true;
							}
							waiting = false;
						});
						while (waiting)
						{
							Thread.Sleep(15);
						}
						if (read < total)
						{
							while (read < total && !hasError)
							{
								EnumerateFollowingList((uint)read, delegate(FriendsEnumerateFollowingList_t r, bool flag)
								{
									if (!flag)
									{
										foreach (CSteamID item2 in r.m_rgSteamID.Where((CSteamID p) => p != CSteamID.Nil))
										{
											followedIds.Add(item2);
										}
										total = r.m_nTotalResultCount;
										read += r.m_nResultsReturned;
									}
									else
									{
										hasError = true;
									}
									waiting = false;
								});
								while (waiting)
								{
									Thread.Sleep(15);
								}
							}
						}
						e.Result = followedIds.ToArray();
						loadingFollowed = false;
					}
					else
					{
						while (loadingFollowed)
						{
							Thread.Sleep(250);
						}
					}
				};
				bgWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
				{
					callback?.Invoke(e.Result as CSteamID[]);
					bgWorker.Dispose();
				};
				bgWorker.RunWorkerAsync();
			}

			public static void EnumerateFollowingList(uint index, Action<FriendsEnumerateFollowingList_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_FriendsEnumerateFollowingList_t == null)
					{
						m_FriendsEnumerateFollowingList_t = CallResult<FriendsEnumerateFollowingList_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamFriends.EnumerateFollowingList(index);
					m_FriendsEnumerateFollowingList_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static UserData GetCoplayFriend(int coplayFriendIndex)
			{
				return SteamFriends.GetCoplayFriend(coplayFriendIndex);
			}

			public static int GetCoplayFriendCount()
			{
				return SteamFriends.GetCoplayFriendCount();
			}

			public static UserData[] GetCoplayFriends()
			{
				int coplayFriendCount = SteamFriends.GetCoplayFriendCount();
				if (coplayFriendCount > 0)
				{
					UserData[] array = new UserData[coplayFriendCount];
					for (int i = 0; i < coplayFriendCount; i++)
					{
						array[i] = GetCoplayFriend(i);
					}
					return array;
				}
				return new UserData[0];
			}

			public static void GetFollowerCount(UserData userId, Action<FriendsGetFollowerCount_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_FriendsGetFollowerCount_t == null)
					{
						m_FriendsGetFollowerCount_t = CallResult<FriendsGetFollowerCount_t>.Create();
					}
					SteamAPICall_t followerCount = SteamFriends.GetFollowerCount(userId);
					m_FriendsGetFollowerCount_t.Set(followerCount, callback.Invoke);
				}
			}

			public static UserData GetFriendByIndex(int index, EFriendFlags flags)
			{
				return SteamFriends.GetFriendByIndex(index, flags);
			}

			public static AppId_t GetFriendCoplayGame(UserData userId)
			{
				return SteamFriends.GetFriendCoplayGame(userId);
			}

			public static DateTime GetFriendCoplayTime(UserData userId)
			{
				return new DateTime(1970, 1, 1).AddSeconds(SteamFriends.GetFriendCoplayTime(userId));
			}

			public static int GetFriendCount(EFriendFlags flags)
			{
				return SteamFriends.GetFriendCount(flags);
			}

			public static UserData[] GetFriends(EFriendFlags flags)
			{
				int friendCount = SteamFriends.GetFriendCount(flags);
				if (friendCount > 0)
				{
					UserData[] array = new UserData[friendCount];
					for (int i = 0; i < friendCount; i++)
					{
						array[i] = SteamFriends.GetFriendByIndex(i, flags);
					}
					return array;
				}
				return new UserData[0];
			}

			public static int GetFriendCountFromSource(CSteamID source)
			{
				return SteamFriends.GetFriendCountFromSource(source);
			}

			public static UserData GetFriendFromSourceByIndex(CSteamID source, int index)
			{
				return SteamFriends.GetFriendFromSourceByIndex(source, index);
			}

			public static UserData[] GetFriendsFromSource(CSteamID source)
			{
				int friendCountFromSource = SteamFriends.GetFriendCountFromSource(source);
				if (friendCountFromSource > 0)
				{
					UserData[] array = new UserData[friendCountFromSource];
					for (int i = 0; i < friendCountFromSource; i++)
					{
						array[i] = SteamFriends.GetFriendFromSourceByIndex(source, i);
					}
					return array;
				}
				return new UserData[0];
			}

			public static bool GetFriendGamePlayed(UserData userId, out FriendGameInfo results)
			{
				FriendGameInfo_t pFriendGameInfo;
				bool friendGamePlayed = SteamFriends.GetFriendGamePlayed(userId, out pFriendGameInfo);
				results = pFriendGameInfo;
				return friendGamePlayed;
			}

			public static string GetFriendMessage(UserData userId, int index, out EChatEntryType type)
			{
				SteamFriends.GetFriendMessage(userId, index, out var pvData, 8193, out type);
				return pvData;
			}

			public static string GetFriendPersonaName(UserData userId)
			{
				return SteamFriends.GetFriendPersonaName(userId);
			}

			public static string GetFriendPersonaNameHistory(UserData userId, int index)
			{
				return SteamFriends.GetFriendPersonaNameHistory(userId, index);
			}

			public static string[] GetFriendPersonaNameHistory(UserData userId)
			{
				List<string> list = new List<string>();
				int num = 0;
				string friendPersonaNameHistory = SteamFriends.GetFriendPersonaNameHistory(userId, 0);
				while (!string.IsNullOrEmpty(friendPersonaNameHistory))
				{
					list.Add(friendPersonaNameHistory);
					num++;
					friendPersonaNameHistory = SteamFriends.GetFriendPersonaNameHistory(userId, num);
				}
				return list.ToArray();
			}

			public static EPersonaState GetFriendPersonaState(UserData userId)
			{
				return SteamFriends.GetFriendPersonaState(userId);
			}

			public static string GetFriendRichPresence(UserData userId, string key)
			{
				return SteamFriends.GetFriendRichPresence(userId, key);
			}

			public static string GetFriendRichPresenceKeyByIndex(UserData userId, int index)
			{
				return SteamFriends.GetFriendRichPresenceKeyByIndex(userId, index);
			}

			public static int GetFriendRichPresenceKeyCount(UserData userId)
			{
				return SteamFriends.GetFriendRichPresenceKeyCount(userId);
			}

			public static Dictionary<string, string> GetFriendRichPresence(UserData userId)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				int friendRichPresenceKeyCount = SteamFriends.GetFriendRichPresenceKeyCount(userId);
				if (friendRichPresenceKeyCount > 0)
				{
					for (int i = 0; i < friendRichPresenceKeyCount; i++)
					{
						string friendRichPresenceKeyByIndex = SteamFriends.GetFriendRichPresenceKeyByIndex(userId, i);
						string friendRichPresence = SteamFriends.GetFriendRichPresence(userId, friendRichPresenceKeyByIndex);
						if (!dictionary.ContainsKey(friendRichPresenceKeyByIndex))
						{
							dictionary.Add(friendRichPresenceKeyByIndex, friendRichPresence);
						}
					}
				}
				return dictionary;
			}

			public static int GetFriendsGroupCount()
			{
				return SteamFriends.GetFriendsGroupCount();
			}

			public static FriendsGroupID_t GetFriendsGroupIDByIndex(int index)
			{
				return SteamFriends.GetFriendsGroupIDByIndex(index);
			}

			public static FriendsGroupID_t[] GetFriendsGroups()
			{
				int friendsGroupCount = SteamFriends.GetFriendsGroupCount();
				if (friendsGroupCount > 0)
				{
					FriendsGroupID_t[] array = new FriendsGroupID_t[friendsGroupCount];
					for (int i = 0; i < friendsGroupCount; i++)
					{
						array[i] = SteamFriends.GetFriendsGroupIDByIndex(i);
					}
					return array;
				}
				return new FriendsGroupID_t[0];
			}

			public static CSteamID[] GetFriendsGroupMembersList(FriendsGroupID_t groupId)
			{
				int friendsGroupMembersCount = SteamFriends.GetFriendsGroupMembersCount(groupId);
				if (friendsGroupMembersCount > 0)
				{
					CSteamID[] array = new CSteamID[friendsGroupMembersCount];
					SteamFriends.GetFriendsGroupMembersList(groupId, array, friendsGroupMembersCount);
					return array;
				}
				return new CSteamID[0];
			}

			public static string GetFriendsGroupName(FriendsGroupID_t groupId)
			{
				return SteamFriends.GetFriendsGroupName(groupId);
			}

			public static int GetFriendSteamLevel(UserData userId)
			{
				return SteamFriends.GetFriendSteamLevel(userId);
			}

			public static void GetFriendAvatar(CSteamID userId, Action<Texture2D> callback)
			{
				if (callback == null)
				{
					return;
				}
				if (m_AvatarImageLoaded_t == null)
				{
					m_AvatarImageLoaded_t = Callback<AvatarImageLoaded_t>.Create(HandleAvatarImageLoaded);
				}
				if (m_PersonaStateChange_t == null)
				{
					m_PersonaStateChange_t = Callback<PersonaStateChange_t>.Create(HandlePersonaStateChange);
				}
				if (!SteamFriends.RequestUserInformation(userId, bRequireNameOnly: false))
				{
					int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(userId);
					if (largeFriendAvatar > 0)
					{
						if (loadedImages.ContainsKey(largeFriendAvatar))
						{
							callback(loadedImages[largeFriendAvatar]);
							return;
						}
						if (LoadAvatar(largeFriendAvatar, userId))
						{
							callback(loadedImages[largeFriendAvatar]);
							return;
						}
						Debug.LogWarning("Failed to load the requested avatar");
						callback(null);
					}
					else if (largeFriendAvatar < 0)
					{
						pendingLinks.Add(new ImageRequestCallbackLink
						{
							owner = userId,
							callback = callback
						});
					}
					else
					{
						Debug.LogWarning("No avatar available for this user");
						callback(null);
					}
				}
				else
				{
					pendingLinks.Add(new ImageRequestCallbackLink
					{
						owner = userId,
						callback = callback
					});
				}
			}

			public static void UnloadAvatarImages()
			{
				foreach (KeyValuePair<int, Texture2D> loadedImage in loadedImages)
				{
					if (loadedImage.Value != null)
					{
						UnityEngine.Object.Destroy(loadedImage.Value);
					}
				}
				loadedImages.Clear();
				userAvatarMapping.Clear();
			}

			public static void UnloadAvatarImage(Texture2D image)
			{
				List<int> list = new List<int>();
				foreach (KeyValuePair<int, Texture2D> loadedImage in loadedImages)
				{
					if (loadedImage.Value == image)
					{
						list.Add(loadedImage.Key);
					}
				}
				CSteamID cSteamID = CSteamID.Nil;
				foreach (KeyValuePair<UserData, Texture2D> item in userAvatarMapping)
				{
					if (item.Value == image)
					{
						cSteamID = item.Key;
						break;
					}
				}
				UnityEngine.Object.Destroy(image);
				foreach (int item2 in list)
				{
					loadedImages.Remove(item2);
				}
				userAvatarMapping.Remove(cSteamID);
			}

			public static string GetPlayerNickname(UserData userId)
			{
				return SteamFriends.GetPlayerNickname(userId);
			}

			public static bool HasFriend(UserData userId, EFriendFlags flags)
			{
				return SteamFriends.HasFriend(userId, flags);
			}

			public static bool InviteUserToGame(UserData userId, string connectString)
			{
				return SteamFriends.InviteUserToGame(userId, connectString);
			}

			public static void IsFollowing(CSteamID id, Action<FriendsIsFollowing_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_FriendsIsFollowing_t == null)
					{
						m_FriendsIsFollowing_t = CallResult<FriendsIsFollowing_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamFriends.IsFollowing(id);
					m_FriendsIsFollowing_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static bool IsUserInSource(UserData userId, CSteamID sourceId)
			{
				return SteamFriends.IsUserInSource(userId, sourceId);
			}

			public static bool ReplyToFriendMessage(UserData userId, string message)
			{
				return SteamFriends.ReplyToFriendMessage(userId, message);
			}

			public static void RequestFriendRichPresence(UserData userId)
			{
				SteamFriends.RequestFriendRichPresence(userId);
			}

			public static bool RequestUserInformation(UserData userId, bool nameOnly)
			{
				return SteamFriends.RequestUserInformation(userId, nameOnly);
			}

			public static void SetInGameVoiceSpeaking(bool speaking)
			{
				SteamFriends.SetInGameVoiceSpeaking(SteamUser.GetSteamID(), speaking);
			}

			public static void SetListenForFriendsMessages(bool enabled)
			{
				SteamFriends.SetListenForFriendsMessages(enabled);
			}

			public static void SetPersonaName(string name, Action<SetPersonaNameResponse_t, bool> callback)
			{
				if (callback != null)
				{
					if (m_SetPersonaNameResponse_t == null)
					{
						m_SetPersonaNameResponse_t = CallResult<SetPersonaNameResponse_t>.Create();
					}
					SteamAPICall_t hAPICall = SteamFriends.SetPersonaName(name);
					m_SetPersonaNameResponse_t.Set(hAPICall, callback.Invoke);
				}
			}

			public static void SetPlayedWith(UserData userId)
			{
				SteamFriends.SetPlayedWith(userId);
			}

			public static bool SetRichPresence(string key, string value)
			{
				return SteamFriends.SetRichPresence(key, value);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			public static Texture2D GetLoadedAvatar(CSteamID id)
			{
				if (userAvatarMapping.ContainsKey(id))
				{
					return userAvatarMapping[id];
				}
				return null;
			}

			private static void HandleAvatarImageLoaded(AvatarImageLoaded_t results)
			{
				if (LoadAvatar(results.m_iImage, results.m_steamID))
				{
					Texture2D obj = loadedImages[results.m_iImage];
					foreach (ImageRequestCallbackLink pendingLink in pendingLinks)
					{
						if (pendingLink.owner == results.m_steamID && pendingLink.callback != null)
						{
							pendingLink.callback(obj);
						}
					}
					pendingLinks.RemoveAll((ImageRequestCallbackLink p) => p.owner == results.m_steamID);
				}
				else
				{
					Debug.LogWarning("Steam API responded with an Avatar Loaded [" + results.m_iImage + "] message for user [" + results.m_steamID.m_SteamID + "] however no avatar was found on the local disk.");
				}
			}

			private static void HandlePersonaStateChange(PersonaStateChange_t results)
			{
				if (results.m_nChangeFlags == EPersonaChange.k_EPersonaChangeAvatar)
				{
					CSteamID steamId = new CSteamID(results.m_ulSteamID);
					int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamId);
					if (largeFriendAvatar > 0 && LoadAvatar(largeFriendAvatar, steamId))
					{
						Texture2D obj = loadedImages[largeFriendAvatar];
						foreach (ImageRequestCallbackLink pendingLink in pendingLinks)
						{
							if (pendingLink.owner == steamId && pendingLink.callback != null)
							{
								pendingLink.callback(obj);
							}
						}
						pendingLinks.RemoveAll((ImageRequestCallbackLink p) => p.owner == steamId);
					}
				}
				eventPersonaStateChange.Invoke(results);
			}

			private static bool LoadAvatar(int imageHandle, CSteamID user)
			{
				if (SteamUtils.GetImageSize(imageHandle, out var pnWidth, out var pnHeight))
				{
					Texture2D texture2D = null;
					if (loadedImages.ContainsKey(imageHandle))
					{
						texture2D = loadedImages[imageHandle];
					}
					if (texture2D == null)
					{
						texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false);
					}
					else
					{
						UnityEngine.Object.Destroy(texture2D);
						texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false);
					}
					int num = (int)(pnWidth * pnHeight * 4);
					byte[] array = new byte[num];
					if (SteamUtils.GetImageRGBA(imageHandle, array, num))
					{
						texture2D.LoadRawTextureData(Utilities.FlipImageBufferVertical((int)pnWidth, (int)pnHeight, array));
						texture2D.Apply();
					}
					if (loadedImages.ContainsKey(imageHandle))
					{
						loadedImages[imageHandle] = texture2D;
					}
					else
					{
						loadedImages.Add(imageHandle, texture2D);
					}
					if (userAvatarMapping.ContainsKey(user))
					{
						userAvatarMapping[user] = texture2D;
					}
					else
					{
						userAvatarMapping.Add(user, texture2D);
					}
					return true;
				}
				return false;
			}

			public static bool PersonaChangeHasFlag(EPersonaChange value, EPersonaChange checkflag)
			{
				return (value & checkflag) == checkflag;
			}

			public static bool PersonaChangeHasAllFlags(EPersonaChange value, params EPersonaChange[] checkflags)
			{
				foreach (EPersonaChange ePersonaChange in checkflags)
				{
					if ((value & ePersonaChange) != ePersonaChange)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
