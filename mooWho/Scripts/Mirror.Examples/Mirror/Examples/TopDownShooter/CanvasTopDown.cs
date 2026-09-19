using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.TopDownShooter
{
	public class CanvasTopDown : MonoBehaviour
	{
		public NetworkTopDown networkTopDown;

		public PlayerTopDown playerTopDown;

		public Button buttonSpawnEnemy;

		public Button buttonRespawnPlayer;

		public Text textEnemies;

		public Text textKills;

		public GameObject shotMarker;

		public GameObject deathSplatter;

		public AudioSource soundGameIntro;

		public AudioSource soundGameLoop;

		public AudioSource soundButtonUI;

		private void Start()
		{
			buttonSpawnEnemy.onClick.AddListener(ButtonSpawnEnemy);
			buttonRespawnPlayer.onClick.AddListener(ButtonRespawnPlayer);
			StartCoroutine(BGSound());
		}

		private void ButtonSpawnEnemy()
		{
			PlaySoundButtonUI();
			networkTopDown.SpawnEnemy();
		}

		private void ButtonRespawnPlayer()
		{
			PlaySoundButtonUI();
			playerTopDown.CmdRespawnPlayer();
		}

		public void UpdateEnemyUI(int value)
		{
			textEnemies.text = "Enemies: " + value;
		}

		public void UpdateKillsUI(int value)
		{
			textKills.text = "Kills: " + value;
		}

		public void ResetUI()
		{
			if (NetworkServer.active)
			{
				buttonSpawnEnemy.gameObject.SetActive(value: true);
			}
			else
			{
				buttonSpawnEnemy.gameObject.SetActive(value: false);
			}
			buttonRespawnPlayer.gameObject.SetActive(value: false);
			shotMarker.SetActive(value: false);
			textEnemies.text = "Enemies: 0";
			textKills.text = "Kills: 0";
		}

		private IEnumerator BGSound()
		{
			soundGameIntro.Play();
			yield return new WaitForSeconds(4.1f);
			soundGameLoop.Play();
		}

		public void PlaySoundButtonUI()
		{
			soundButtonUI.Play();
		}
	}
}
