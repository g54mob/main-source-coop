using DefaultNamespace;
using UnityEngine;

public class CaptchaTester : MonoBehaviour
{
	public CaptchaGame CaptchaGame => GetComponent<CaptchaGame>();

	private void Create()
	{
		CaptchaGame.CreateGame();
	}

	private void JoinGame()
	{
		CaptchaGame.StartGame();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.S))
		{
			Create();
			JoinGame();
		}
		else if (CaptchaGame.gameState == CaptchaGame.RESULT.playing)
		{
			if (ValidInput(Input.inputString))
			{
				CaptchaGame.TryText(Input.inputString[0]);
			}
			CaptchaGame.RunTimer();
		}
		static bool ValidInput(string input)
		{
			if (input.Length == 0)
			{
				return false;
			}
			return Input.inputString[0].ToString() switch
			{
				"W" => false, 
				"w" => false, 
				"A" => false, 
				"a" => false, 
				"S" => false, 
				"s" => false, 
				"D" => false, 
				"d" => false, 
				_ => true, 
			};
		}
	}
}
