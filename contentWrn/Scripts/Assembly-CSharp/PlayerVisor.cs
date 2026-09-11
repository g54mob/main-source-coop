using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Portningsbolaget.Platforms;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zorro.Core;
using pworld.Scripts.Extensions;

public class PlayerVisor : MonoBehaviour
{
	public Renderer visorRenderer;

	public int visorMaterialIndex;

	private Material m_material;

	private Player m_player;

	public bool m_playedDeadAnim;

	public Optionable<float> hue;

	public int visorColorIndex = -1;

	public Optionable<Color> visorColor;

	[FormerlySerializedAs("visorTextMesh")]
	public TextMeshPro visorFaceText;

	private static readonly int Emis = Shader.PropertyToID("_Emis");

	private static readonly int Color1 = Shader.PropertyToID("_Color");

	private static readonly int Voice = Shader.PropertyToID("_Voice");

	private float m_startEmission;

	private static readonly string[] bannedWords = new string[9] { "ngr", "nga", "nig", "ger", "jap", "fag", "fgt", "kkk", "ккк" };

	private static readonly List<char> bannedCharacters = new List<char> { '卍', '卐' };

	private float m_faceRotation;

	private string m_faceText = "";

	private const float BLOCKED_ROTATION = 270f;

	private const string BLOCKED_FACE = ":)";

	private bool m_initialized;

	private bool m_restrictedCommunication;

	private string m_bufferedText = "";

	public float FaceSize
	{
		get
		{
			return visorFaceText.transform.localScale.x;
		}
		set
		{
			visorFaceText.transform.localScale = new Vector3(value, value, 1f);
		}
	}

	public float FaceRotation
	{
		get
		{
			return visorFaceText.transform.eulerAngles.z;
		}
		set
		{
			Vector3 localEulerAngles = visorFaceText.transform.localEulerAngles;
			localEulerAngles.z = value;
			visorFaceText.transform.localEulerAngles = localEulerAngles;
		}
	}

	private void Awake()
	{
		m_player = GetComponent<Player>();
	}

	private void OnDestroy()
	{
	}

	private IEnumerator Start()
	{
		yield return null;
		Material[] materials = visorRenderer.materials;
		m_material = materials[visorMaterialIndex];
		m_startEmission = m_material.GetFloat(Emis);
		if (m_player.refs.view.IsMine && !m_player.ai && !LoadFaceFromPlayerPrefs())
		{
			PlayerCustomizer playerCustomizer = UnityEngine.Object.FindObjectOfType<PlayerCustomizer>();
			if (playerCustomizer != null)
			{
				ApplyVisorColor(playerCustomizer.colorsToPickFrom.GetRnd());
			}
		}
		if (!string.IsNullOrEmpty(m_bufferedText))
		{
			if (!m_restrictedCommunication)
			{
				SetVisorText(m_bufferedText, null);
			}
			m_bufferedText = string.Empty;
		}
		m_initialized = true;
	}

	[PunRPC]
	public void RPCA_SetCommunicationRestriction(bool restricted)
	{
		if (!m_player.refs.view.IsMine)
		{
			Debug.Log((restricted ? "Restricting" : "Unrestricting") + " player visor");
			m_restrictedCommunication = restricted;
			if (m_restrictedCommunication)
			{
				HideFace(hidden: true);
			}
		}
	}

	[PunRPC]
	public void RPC_SetBlockedFace(int playerID, bool blocked)
	{
		List<Player> players = PlayerHandler.instance.players;
		for (int i = 0; i < players.Count; i++)
		{
			Player player = players[i];
			if (!(player == m_player) && player.refs.view.Controller.CustomProperties["UserID"].GetHashCode() == playerID)
			{
				player.refs.visor.RPCA_SetCommunicationRestriction(blocked);
				return;
			}
		}
		Debug.LogError($"Failed to block face: Missing player {playerID}");
	}

	private void Update()
	{
		if (m_player == null || m_material == null)
		{
			return;
		}
		if (hue.IsSome && visorColor.IsNone)
		{
			Color.RGBToHSV(m_material.GetColor(Color1), out var _, out var S, out var V);
			visorColor = Optionable<Color>.Some(Color.HSVToRGB(hue.Value, S, V));
			m_material.SetColor(Color1, visorColor.Value);
		}
		if (m_player.data.dead)
		{
			if (!m_playedDeadAnim)
			{
				StartCoroutine(DeadAnim());
				m_playedDeadAnim = true;
			}
			return;
		}
		m_material.SetFloat(Voice, m_player.data.microphoneValue);
		if (m_playedDeadAnim)
		{
			SetEmission(m_startEmission);
			m_playedDeadAnim = false;
			Debug.Log("Resetting emission!");
		}
	}

	public void SaveFaceToPlayerPrefs()
	{
		if (m_player.refs.view.IsMine)
		{
			PlayerPrefs.SetFloat("VisorColor", hue.Value);
			PlayerPrefs.SetString("FaceText", visorFaceText.text);
			PlayerPrefs.SetFloat("FaceRotation", FaceRotation);
			PlayerPrefs.SetFloat("FaceSize", FaceSize);
			PlayerPrefs.SetInt("FaceColorIndex", visorColorIndex);
			PlayerPrefs.Save();
		}
	}

	public bool LoadFaceFromPlayerPrefs()
	{
		if (!m_player.refs.view.IsMine)
		{
			return false;
		}
		float num = PlayerPrefs.GetFloat("VisorColor", hue.Value);
		string faceText = PlayerPrefs.GetString("FaceText", visorFaceText.text);
		float faceRotation = PlayerPrefs.GetFloat("FaceRotation", FaceRotation);
		float faceSize = PlayerPrefs.GetFloat("FaceSize", FaceSize);
		int num2 = PlayerPrefs.GetInt("FaceColorIndex", visorColorIndex);
		if (num2 < 0)
		{
			return false;
		}
		SetAllFaceSettings(num, num2, faceText, faceRotation, faceSize);
		return true;
	}

	public void SetAllFaceSettings(float hue, int colorIndex, string faceText, float faceRotation, float faceSize)
	{
		m_player.refs.view.RPC("RPCA_SetAllFaceSettings", RpcTarget.AllBuffered, hue, colorIndex, faceText, faceRotation, faceSize);
	}

	[PunRPC]
	public void RPCA_SetAllFaceSettings(float hue, int colorIndex, string faceText, float faceRotation, float faceSize)
	{
		SetVisorColor(hue);
		if (m_initialized)
		{
			SetVisorText(faceText, null);
		}
		else
		{
			m_bufferedText = faceText;
		}
		if (m_restrictedCommunication)
		{
			m_faceRotation = faceRotation;
			faceRotation = 270f;
		}
		FaceSize = faceSize;
		FaceRotation = faceRotation;
		visorColorIndex = colorIndex;
	}

	[PunRPC]
	public void RPCA_SetVisorText(string text)
	{
		if (m_initialized)
		{
			SetVisorText(text, null);
		}
		else
		{
			m_bufferedText = text;
		}
	}

	public void SetVisorText(string text, Action<string> callback)
	{
		PlatformManager.Platform.VerifyString(text, delegate(string result)
		{
			result = SafetyCheckVisorText(result);
			if (m_restrictedCommunication)
			{
				m_faceText = result;
				result = ":)";
			}
			Debug.Log("Setting Visor: Text " + result + " Original " + text);
			visorFaceText.text = result;
			callback?.Invoke(result);
		});
	}

	private string SafetyCheckVisorText(string text)
	{
		if (text.Length > 3)
		{
			text = text.Substring(0, 3);
		}
		if (bannedWords.Contains(text.ToLower()))
		{
			return ":]";
		}
		for (int i = 0; i < bannedCharacters.Count; i++)
		{
			char value = bannedCharacters[i];
			if (text.Contains(value))
			{
				return ":]";
			}
		}
		return text;
	}

	private IEnumerator DeadAnim()
	{
		for (int i = 0; i < 6; i++)
		{
			float p = (float)i * 0.2f;
			SetEmission(0.1f);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f) * p);
			SetEmission(m_startEmission);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f) * (1f - p));
		}
		SetEmission(0f);
	}

	private void SetEmission(float value)
	{
		m_material.SetFloat(Emis, value);
	}

	public void ApplyVisorColor(Color color)
	{
		m_player.refs.view.RPC("SetVisorColor", RpcTarget.AllBuffered, GetHueFromColor(color));
	}

	public static float GetHueFromColor(Color c)
	{
		Color.RGBToHSV(c, out var H, out var _, out var _);
		return H;
	}

	private void HideFace(bool hidden)
	{
		if (hidden)
		{
			m_faceRotation = FaceRotation;
			FaceRotation = 270f;
			m_faceText = visorFaceText.text;
			visorFaceText.text = ":)";
		}
		else
		{
			FaceRotation = m_faceRotation;
			visorFaceText.text = m_faceText;
		}
	}

	[PunRPC]
	public void SetVisorColor(float h)
	{
		visorColor = default(Optionable<Color>);
		hue = Optionable<float>.Some(h);
	}
}
