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
			FilePathsData = new byte[70]
			{
				0, 0, 0, 1, 0, 0, 0, 62, 92, 65,
				115, 115, 101, 116, 115, 92, 70, 101, 97, 116,
				117, 114, 101, 115, 92, 79, 98, 105, 67, 111,
				108, 108, 105, 100, 101, 114, 115, 67, 104, 117,
				110, 107, 92, 83, 99, 114, 105, 112, 116, 115,
				92, 79, 98, 105, 67, 111, 108, 108, 105, 100,
				101, 114, 67, 104, 117, 110, 107, 46, 99, 115
			},
			TypesData = new byte[56]
			{
				0, 0, 0, 0, 51, 70, 101, 97, 116, 117,
				114, 101, 115, 46, 79, 98, 105, 67, 111, 108,
				108, 105, 100, 101, 114, 115, 67, 104, 117, 110,
				107, 46, 83, 99, 114, 105, 112, 116, 115, 124,
				79, 98, 105, 67, 111, 108, 108, 105, 100, 101,
				114, 67, 104, 117, 110, 107
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
