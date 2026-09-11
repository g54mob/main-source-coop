using JetBrains.Annotations;
using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;

namespace DefaultNamespace
{
	public class CaptchaGame : MonoBehaviour
	{
		public enum RESULT
		{
			notStarted = 0,
			playing = 1,
			completed = 2,
			failed = 3
		}

		public class Captcha
		{
			public string textCaptcha;

			public string buttonCaptcha;

			public int progress;

			public static readonly string textPossibles = "qrtyuiopfghjklzxcvbn1234567890-=!@#$%^&*()_+QRTYUIOPFGHJKLZXCVBN<>?/.,;':[]{}|`~";

			public static readonly string buttonPossibles = "udlrtT0345";

			public int Length => buttonCaptcha.Length;

			public bool Complete => progress >= buttonCaptcha.Length;

			public static string GetTextCaptchaString(int length)
			{
				string text = "";
				for (int i = 0; i < length; i++)
				{
					text += textPossibles[Random.Range(0, textPossibles.Length)];
				}
				return text;
			}

			public static string GetButtonCaptchaString(int length)
			{
				string text = "";
				for (int i = 0; i < length; i++)
				{
					text += buttonPossibles[Random.Range(0, buttonPossibles.Length)];
				}
				return text;
			}

			public Captcha(string textCaptcha, string buttonCaptcha)
			{
				this.textCaptcha = textCaptcha;
				this.buttonCaptcha = buttonCaptcha;
				progress = 0;
			}

			public char GetCurrentTextChar()
			{
				if (progress >= textCaptcha.Length)
				{
					return '\0';
				}
				return textCaptcha[progress];
			}

			public char GetCurrentButtonChar()
			{
				if (progress >= buttonCaptcha.Length)
				{
					return '\0';
				}
				return buttonCaptcha[progress];
			}

			public bool Try(char inputtedChar)
			{
				if (buttonCaptcha[progress] != inputtedChar)
				{
					if (textCaptcha.Length > 0)
					{
						return textCaptcha[progress] == inputtedChar;
					}
					return false;
				}
				return true;
			}

			public void Progress()
			{
				progress = Mathf.Clamp(progress + 1, 0, buttonCaptcha.Length);
			}
		}

		public int captchaLength = 8;

		public float waitBetweenTries = 1f;

		public float timePerTry = 10f;

		public int maxTries = 3;

		public float timeToFail;

		public RESULT gameState;

		public PhotonView view;

		[NotNull]
		public Canvas canvas;

		public Captcha currentCaptcha;

		private int fails;

		[HideInInspector]
		public CapturedCaptchaCanvas capturedCanvas;

		[HideInInspector]
		public CaptchaTerminalCanvas terminalCavnas;

		private Bot_Weeping weeping_g;

		public SFX_Instance successInputSound;

		public SFX_Instance failedInputSound;

		public int Fails
		{
			get
			{
				return fails;
			}
			set
			{
				if (value > fails)
				{
					terminalCavnas.DoFailStuff(weeping_g.playerInCaptchaGame != null && weeping_g.playerInCaptchaGame.refs.view.IsMine);
					capturedCanvas.DoFailStuff(weeping_g.capturedPlayer != null && weeping_g.capturedPlayer.refs.view.IsMine);
				}
				fails = value;
				terminalCavnas.SetTries(fails, maxTries);
				capturedCanvas.SetTries(fails, maxTries);
				if (fails >= maxTries)
				{
					gameState = RESULT.failed;
				}
				else
				{
					NewCaptcha();
				}
			}
		}

		public void TurnOffGame()
		{
			capturedCanvas.root.SetActive(value: false);
			terminalCavnas.root.SetActive(value: false);
			gameState = RESULT.notStarted;
		}

		public void Awake()
		{
			view = GetComponent<PhotonView>();
			weeping_g = GetComponent<Bot_Weeping>();
			capturedCanvas = base.transform.root.GetComponentInChildren<CapturedCaptchaCanvas>();
			terminalCavnas = base.transform.root.GetComponentInChildren<CaptchaTerminalCanvas>();
		}

		public void CreateGame()
		{
			gameState = RESULT.notStarted;
			timeToFail = timePerTry;
			Fails = 0;
			NewCaptcha();
			terminalCavnas.Show();
		}

		private void NewCaptcha()
		{
			if (view.IsMine)
			{
				string textCaptchaString = Captcha.GetTextCaptchaString(captchaLength);
				string buttonCaptchaString = Captcha.GetButtonCaptchaString(captchaLength);
				view.RPC("RPCA_NewCaptcha", RpcTarget.All, textCaptchaString, buttonCaptchaString);
			}
		}

		[PunRPC]
		private void RPCA_NewCaptcha(string textCaptcha, string buttonCaptcha)
		{
			currentCaptcha = new Captcha(textCaptcha, buttonCaptcha);
			terminalCavnas.SetCaptcha(textCaptcha, buttonCaptcha);
			terminalCavnas.SetInput("");
			terminalCavnas.SetButtons("");
			timeToFail = timePerTry;
			terminalCavnas.SetTimer(timeToFail, timePerTry);
			capturedCanvas.SetGameTimer(timeToFail, timePerTry);
		}

		public void StartGame()
		{
			gameState = RESULT.playing;
			capturedCanvas.GameStarted(currentCaptcha.Length);
		}

		public void RunTimer()
		{
			if (gameState == RESULT.playing)
			{
				timeToFail -= Time.deltaTime;
				capturedCanvas.SetGameTimer(timeToFail, timePerTry);
				terminalCavnas.SetTimer(timeToFail, timePerTry);
				if (view.IsMine && timeToFail < 0f)
				{
					view.RPC("RPCA_TimedOut", RpcTarget.All);
				}
			}
		}

		public static bool ValidInput(string input)
		{
			if (input.Length == 0)
			{
				return false;
			}
			if (input != "e" && input != "E" && input != "w" && input != "W" && input != "a" && input != "A" && input != "s" && input != "S" && input != "d")
			{
				return input != "D";
			}
			return false;
		}

		public static bool ValidButton(string input)
		{
			if (input.Length == 0)
			{
				return false;
			}
			return input != "2";
		}

		[PunRPC]
		private void RPCA_TimedOut()
		{
			Fails++;
		}

		public bool TryText(char input)
		{
			if (gameState != RESULT.playing)
			{
				return false;
			}
			terminalCavnas.AddChar(input);
			terminalCavnas.AddButton(currentCaptcha.GetCurrentButtonChar());
			if (currentCaptcha.Try(input))
			{
				OnSuccessfulTry();
				return true;
			}
			OnFailedTry();
			return false;
		}

		public bool TryButton(char input)
		{
			if (gameState != RESULT.playing)
			{
				return false;
			}
			terminalCavnas.AddButton(input);
			terminalCavnas.AddChar(currentCaptcha.GetCurrentTextChar());
			if (currentCaptcha.Try(input))
			{
				OnSuccessfulTry();
				return true;
			}
			OnFailedTry();
			return false;
		}

		private void OnSuccessfulTry()
		{
			if (successInputSound != null)
			{
				successInputSound.Play();
			}
			currentCaptcha.Progress();
			capturedCanvas.SetProgress(currentCaptcha.progress);
			if (currentCaptcha.Complete)
			{
				gameState = RESULT.completed;
				if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager)
				{
					steamRuntimeManager.OnCaptchaCompleted();
				}
			}
		}

		private void OnFailedTry()
		{
			if (failedInputSound != null)
			{
				failedInputSound.Play();
			}
			Fails++;
			NewCaptcha();
		}
	}
}
