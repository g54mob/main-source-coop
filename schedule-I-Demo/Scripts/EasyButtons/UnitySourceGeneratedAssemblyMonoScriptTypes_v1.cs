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
			FilePathsData = new byte[65]
			{
				0, 0, 0, 1, 0, 0, 0, 57, 92, 65,
				115, 115, 101, 116, 115, 92, 84, 104, 105, 114,
				100, 80, 97, 114, 116, 121, 92, 69, 97, 115,
				121, 66, 117, 116, 116, 111, 110, 115, 92, 82,
				117, 110, 116, 105, 109, 101, 92, 66, 117, 116,
				116, 111, 110, 65, 116, 116, 114, 105, 98, 117,
				116, 101, 46, 99, 115
			},
			TypesData = new byte[32]
			{
				0, 0, 0, 0, 27, 69, 97, 115, 121, 66,
				117, 116, 116, 111, 110, 115, 124, 66, 117, 116,
				116, 111, 110, 65, 116, 116, 114, 105, 98, 117,
				116, 101
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
