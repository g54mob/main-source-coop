using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AeLa.EasyFeedback.Utility;
using AeLa.EasyFeedback.Web;
using UnityEngine;
using UnityEngine.Networking;

namespace AeLa.EasyFeedback.APIs
{
	public class Trello
	{
		public const int MaxCharLength = 16384;

		public const string CategoryTag = "(EF)";

		public const string TemplateBoardID = "589d1b02a4856195b7cc31c9";

		public const string AppKey = "9babe077311b8a24fddaebb73de1df6a";

		public const string ApiUri = "https://trello.com/1";

		public bool IsDoneUploading;

		public bool UploadError;

		public string ErrorMessage;

		public Exception UploadException;

		public AddCardResponse LastAddCardResponse;

		public UnityWebRequest LastRequest;

		private readonly string token;

		public static string AuthURL => string.Format("{0}/authorize?expiration=never&scope=read,write,account&response_type=token&name=Easy%20Feedback&key={1}", "https://trello.com/1", "9babe077311b8a24fddaebb73de1df6a");

		public Trello(string token)
		{
			this.token = token;
		}

		public string GetURI(string apiPath)
		{
			string text = "?";
			if (apiPath.Contains("?"))
			{
				text = "&";
			}
			return "https://trello.com/1" + apiPath + text + "key=9babe077311b8a24fddaebb73de1df6a&token=" + token;
		}

		public static bool IsValidToken(string token, bool silent = false)
		{
			WWW wWW = new WWW("https://trello.com/1/members/me?key=9babe077311b8a24fddaebb73de1df6a&token=" + token);
			while (!wWW.isDone)
			{
			}
			return string.IsNullOrEmpty(wWW.error);
		}

		public IEnumerator AddCard(string name, string description, IEnumerable<Label> labels, string list, byte[] fileSource = null)
		{
			IsDoneUploading = false;
			UploadError = false;
			ErrorMessage = string.Empty;
			UploadException = null;
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("key", "9babe077311b8a24fddaebb73de1df6a");
			wWWForm.AddField("token", token);
			wWWForm.AddField("name", name);
			if (description.Length > 16384)
			{
				Debug.LogError($"Card description length is higher than maximum length of {16384}. Truncating...");
				description = description.Remove(16383);
			}
			wWWForm.AddField("desc", description);
			string value = string.Join(",", labels.Select((Label l) => l.id).ToArray());
			wWWForm.AddField("idLabels", value);
			wWWForm.AddField("idList", list);
			if (fileSource != null)
			{
				wWWForm.AddBinaryData("fileSource", fileSource);
			}
			yield return WebInterface.PostCoroutine("https://api.trello.com/1/cards", wWWForm, delegate(WebResponse resp)
			{
				UploadError = resp.IsError;
				if (!resp.IsError)
				{
					LastAddCardResponse = JsonUtility.FromJson<AddCardResponse>(resp.Text);
				}
				else
				{
					ErrorMessage = resp.Text;
				}
			});
		}

		public IEnumerator AddAttachmentAsync(string cardID, byte[] file = null, string url = null, string name = null, string mimeType = null)
		{
			IsDoneUploading = false;
			UploadError = false;
			ErrorMessage = string.Empty;
			UploadException = null;
			WWWForm wWWForm = new WWWForm();
			if (file != null)
			{
				wWWForm.AddBinaryData("file", file, name ?? "file.dat");
			}
			if (url != null)
			{
				wWWForm.AddField("url", url);
			}
			if (name != null)
			{
				wWWForm.AddField("name", name);
			}
			if (mimeType != null)
			{
				wWWForm.AddField("mimeType", mimeType);
			}
			string uRI = GetURI("/cards/" + cardID + "/attachments");
			yield return WebInterface.PostCoroutine(uRI, wWWForm, delegate(WebResponse resp)
			{
				UploadError = resp.IsError;
				if (resp.IsError)
				{
					ErrorMessage = resp.Text;
				}
			});
		}

		public IEnumerator GetLabelsAsync(string boardID, Action<Label[]> onFinished)
		{
			string uRI = GetURI("/boards/" + boardID + "/labels");
			yield return WebInterface.GetCoroutine(uRI, delegate(WebResponse response)
			{
				Label[] labels = JsonUtility.FromJson<LabelCollection>(response.Text.WrapToClass("labels")).labels;
				onFinished(labels);
			});
		}

		public IEnumerator GetListsAsync(string boardID, Action<List[]> onFinished)
		{
			string uRI = GetURI("/boards/" + boardID + "/lists");
			yield return WebInterface.GetCoroutine(uRI, delegate(WebResponse response)
			{
				List[] lists = JsonUtility.FromJson<ListCollection>(response.Text.WrapToClass("lists")).lists;
				onFinished(lists);
			});
		}

		public List[] GetLists(string boardID)
		{
			return JsonUtility.FromJson<ListCollection>(WebInterface.Get(GetURI("/boards/" + boardID + "/lists")).Text.WrapToClass("lists")).lists;
		}

		public Board AddBoard(string name, bool defaultLabels = true, bool defaultLists = true, string desc = null, string idOrganization = null, string idBoardSource = null, string keepFromSource = "all", string powerUps = "all", Prefs? prefs = null)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("key", "9babe077311b8a24fddaebb73de1df6a");
			wWWForm.AddField("token", token);
			wWWForm.AddField("name", name);
			wWWForm.AddField("defaultLabels", defaultLabels.ToString().ToLower());
			wWWForm.AddField("defaultLists", defaultLists.ToString().ToLower());
			if (desc != null)
			{
				wWWForm.AddField("desc", desc);
			}
			if (idOrganization != null)
			{
				wWWForm.AddField("idOrganization", idOrganization);
			}
			if (idBoardSource != null)
			{
				wWWForm.AddField("idBoardSource", idBoardSource);
			}
			wWWForm.AddField("keepFromSource", keepFromSource);
			wWWForm.AddField("powerUps", powerUps);
			if (prefs.HasValue)
			{
				Prefs value = prefs.Value;
				if (value.permissionLevel.HasValue)
				{
					wWWForm.AddField("prefs_permissionLevel", value.permissionLevel.Value.ToString());
				}
				if (value.voting.HasValue)
				{
					wWWForm.AddField("prefs_voting", value.voting.Value.ToString());
				}
				if (value.comments.HasValue)
				{
					wWWForm.AddField("prefs_comments", value.comments.Value.ToString());
				}
				if (value.invitations.HasValue)
				{
					wWWForm.AddField("prefs_invitations", value.invitations.Value.ToString());
				}
				if (value.selfJoin.HasValue)
				{
					wWWForm.AddField("prefs_selfJoin", value.selfJoin.Value.ToString().ToLower());
				}
				if (value.cardCovers.HasValue)
				{
					wWWForm.AddField("prefs_cardCovers", value.cardCovers.Value.ToString().ToLower());
				}
				if (value.background != null)
				{
					wWWForm.AddField("prefs_background", value.background);
				}
				if (value.cardAging.HasValue)
				{
					wWWForm.AddField("prefs_cardAging", value.cardAging.Value.ToString());
				}
			}
			return JsonUtility.FromJson<Board>(WebInterface.Post("https://api.trello.com/1/boards", wWWForm).Text);
		}

		public IEnumerator GetBoardsAsync(Action<Board[]> onFinished)
		{
			string uRI = GetURI("/members/me/boards");
			yield return WebInterface.GetCoroutine(uRI, delegate(WebResponse resp)
			{
				Board[] boards = JsonUtility.FromJson<BoardCollection>(resp.Text.WrapToClass("boards")).boards;
				onFinished(boards);
			});
		}

		public Board[] GetBoards()
		{
			return JsonUtility.FromJson<BoardCollection>(WebInterface.Get(GetURI("/members/me/boards")).Text.WrapToClass("boards")).boards;
		}

		public Label[] GetLabels(string boardID)
		{
			return JsonUtility.FromJson<LabelCollection>(WebInterface.Get(GetURI("/boards/" + boardID + "/labels")).Text.WrapToClass("labels")).labels;
		}

		public bool GetSubscribed(string boardID)
		{
			return JsonUtility.FromJson<Subscribed>(WebInterface.Get(GetURI("/boards/" + boardID + "/subscribed")).Text)._value;
		}

		public void PutSubscribed(string boardID, bool value)
		{
			WebInterface.Put(GetURI("/boards/" + boardID + "?subscribed=" + value.ToString().ToLower()));
		}
	}
}
