using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen.Collision
{
	[Serializable]
	public class DungeonCollisionSettings
	{
		public bool DisallowOverhangs;

		public float OverlapThreshold = 0.01f;

		public float Padding;

		public readonly List<Bounds> AdditionalCollisionBounds = new List<Bounds>();

		public bool AvoidCollisionsWithOtherDungeons;

		public AdditionalCollisionsPredicate AdditionalCollisionsPredicate;
	}
}
