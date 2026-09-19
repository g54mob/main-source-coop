using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class Chapter
	{
		[field: SerializeField]
		public ChapterType ChapterType { get; private set; }

		[field: SerializeField]
		public List<LevelType> Levels { get; private set; } = new List<LevelType>();

		[field: SerializeField]
		public bool IsOptional { get; private set; }
	}
}
