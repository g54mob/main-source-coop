using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AnimalTickManager : MonoBehaviour
{
	[Header("Time Slicing")]
	public int ticksPerFrame = 20;

	private readonly List<AnimalBotController> _bots = new List<AnimalBotController>();

	private int _cursor;

	public static AnimalTickManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void Register(AnimalBotController bot)
	{
		_bots.Add(bot);
	}

	public void Unregister(AnimalBotController bot)
	{
		_bots.Remove(bot);
	}

	private void Update()
	{
		if (NetworkServer.active && _bots.Count != 0)
		{
			int num = Mathf.Min(ticksPerFrame, _bots.Count);
			for (int i = 0; i < num; i++)
			{
				int index = (_cursor + i) % _bots.Count;
				_bots[index].ManagedTick();
			}
			_cursor = (_cursor + num) % _bots.Count;
		}
	}
}
