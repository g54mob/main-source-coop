using System;
using UnityEngine;

namespace Features.BoosterModule.BoosterModule.Scripts.Entities
{
	public interface IBoosterEntity
	{
		float CurrentBoosterLifeTime { get; set; }

		float BoosterLifeTime { get; }

		Sprite BoosterIcon { get; }

		bool IsLifeTimeBlocked { get; set; }

		event Action<float> OnCurrentLifeTimeChanged;

		void Activate();

		void Deactivate();

		string GetIdentifier();

		bool IsIdentifierEqual(string identifier);
	}
}
