using System.CodeDom.Compiler;

namespace Microsoft.VisualBasic
{
	internal sealed class VBCodeGenerator : CodeCompiler
	{
		private static readonly char[] s_periodArray = new char[1] { '.' };

		private static readonly string[][] s_keywords = new string[16][]
		{
			null,
			new string[10] { "as", "do", "if", "in", "is", "me", "of", "on", "or", "to" },
			new string[15]
			{
				"and", "dim", "end", "for", "get", "let", "lib", "mod", "new", "not",
				"rem", "set", "sub", "try", "xor"
			},
			new string[30]
			{
				"ansi", "auto", "byte", "call", "case", "cdbl", "cdec", "char", "cint", "clng",
				"cobj", "csng", "cstr", "date", "each", "else", "enum", "exit", "goto", "like",
				"long", "loop", "next", "step", "stop", "then", "true", "wend", "when", "with"
			},
			new string[28]
			{
				"alias", "byref", "byval", "catch", "cbool", "cbyte", "cchar", "cdate", "class", "const",
				"ctype", "cuint", "culng", "endif", "erase", "error", "event", "false", "gosub", "isnot",
				"redim", "sbyte", "short", "throw", "ulong", "until", "using", "while"
			},
			new string[21]
			{
				"csbyte", "cshort", "double", "elseif", "friend", "global", "module", "mybase", "object", "option",
				"orelse", "public", "resume", "return", "select", "shared", "single", "static", "string", "typeof",
				"ushort"
			},
			new string[19]
			{
				"andalso", "boolean", "cushort", "decimal", "declare", "default", "finally", "gettype", "handles", "imports",
				"integer", "myclass", "nothing", "partial", "private", "shadows", "trycast", "unicode", "variant"
			},
			new string[13]
			{
				"assembly", "continue", "delegate", "function", "inherits", "operator", "optional", "preserve", "property", "readonly",
				"synclock", "uinteger", "widening"
			},
			new string[9] { "addressof", "interface", "namespace", "narrowing", "overloads", "overrides", "protected", "structure", "writeonly" },
			new string[6] { "addhandler", "directcast", "implements", "paramarray", "raiseevent", "withevents" },
			new string[2] { "mustinherit", "overridable" },
			new string[1] { "mustoverride" },
			new string[1] { "removehandler" },
			new string[3] { "class_finalize", "notinheritable", "notoverridable" },
			null,
			new string[1] { "class_initialize" }
		};
	}
}
