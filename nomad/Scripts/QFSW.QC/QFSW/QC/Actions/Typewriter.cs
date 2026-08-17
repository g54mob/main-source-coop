using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QFSW.QC.Actions
{
	public class Typewriter : Composite
	{
		public struct Config
		{
			public enum ChunkType
			{
				Character = 0,
				Word = 1,
				Line = 2
			}

			public float PrintInterval;

			public ChunkType Chunks;

			public static readonly Config Default = new Config
			{
				PrintInterval = 0f,
				Chunks = ChunkType.Character
			};
		}

		private static readonly Regex WhiteRegex = new Regex("(?<=[\\s+])", RegexOptions.Compiled);

		private static readonly Regex LineRegex = new Regex("(?<=[\\n+])", RegexOptions.Compiled);

		public Typewriter(string message)
			: this(message, Config.Default)
		{
		}

		public Typewriter(string message, Config config)
			: base(Generate(message, config))
		{
		}

		private static IEnumerator<ICommandAction> Generate(string message, Config config)
		{
			string[] chunks = config.Chunks switch
			{
				Config.ChunkType.Character => message.Select((char c) => c.ToString()).ToArray(), 
				Config.ChunkType.Word => WhiteRegex.Split(message), 
				Config.ChunkType.Line => LineRegex.Split(message), 
				_ => throw new ArgumentException($"Chunk type {config.Chunks} is not supported."), 
			};
			for (int i = 0; i < chunks.Length; i++)
			{
				yield return new WaitRealtime(config.PrintInterval);
				yield return new Value(chunks[i], i == 0);
			}
		}
	}
}
