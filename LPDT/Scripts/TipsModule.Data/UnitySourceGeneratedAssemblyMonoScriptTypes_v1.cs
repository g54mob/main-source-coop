using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
[GeneratedCode("Unity.MonoScriptGenerator.MonoScriptInfoGenerator", null)]
internal class UnitySourceGeneratedAssemblyMonoScriptTypes_v1
{
	private struct MonoScriptData
	{
		public byte[] FilePathsData;

		public byte[] TypesData;

		public int TotalTypes;

		public int TotalFiles;

		public bool IsEditorOnly;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static MonoScriptData Get()
	{
		return new MonoScriptData
		{
			FilePathsData = new byte[72]
			{
				0, 0, 0, 1, 0, 0, 0, 64, 92, 65,
				115, 115, 101, 116, 115, 92, 70, 101, 97, 116,
				117, 114, 101, 115, 92, 84, 105, 112, 115, 77,
				111, 100, 117, 108, 101, 92, 83, 99, 114, 105,
				112, 116, 115, 92, 68, 97, 116, 97, 92, 84,
				105, 112, 73, 110, 112, 117, 116, 68, 101, 118,
				105, 99, 101, 70, 105, 108, 116, 101, 114, 46,
				99, 115
			},
			TypesData = new byte[58]
			{
				0, 0, 0, 0, 53, 70, 101, 97, 116, 117,
				114, 101, 115, 46, 84, 105, 112, 115, 77, 111,
				100, 117, 108, 101, 46, 83, 99, 114, 105, 112,
				116, 115, 46, 68, 97, 116, 97, 124, 84, 105,
				112, 73, 110, 112, 117, 116, 68, 101, 118, 105,
				99, 101, 70, 105, 108, 116, 101, 114
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
