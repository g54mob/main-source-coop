using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Microsoft.CSharp
{
	internal sealed class CSharpCodeGenerator : ICodeCompiler, ICodeGenerator
	{
		private static readonly char[] s_periodArray = new char[1] { '.' };

		private readonly IDictionary<string, string> _provOptions;

		private static readonly string[][] s_keywords = new string[10][]
		{
			null,
			new string[5] { "as", "do", "if", "in", "is" },
			new string[6] { "for", "int", "new", "out", "ref", "try" },
			new string[15]
			{
				"base", "bool", "byte", "case", "char", "else", "enum", "goto", "lock", "long",
				"null", "this", "true", "uint", "void"
			},
			new string[14]
			{
				"break", "catch", "class", "const", "event", "false", "fixed", "float", "sbyte", "short",
				"throw", "ulong", "using", "while"
			},
			new string[15]
			{
				"double", "extern", "object", "params", "public", "return", "sealed", "sizeof", "static", "string",
				"struct", "switch", "typeof", "unsafe", "ushort"
			},
			new string[7] { "checked", "decimal", "default", "finally", "foreach", "private", "virtual" },
			new string[10] { "abstract", "continue", "delegate", "explicit", "implicit", "internal", "operator", "override", "readonly", "volatile" },
			new string[7] { "__arglist", "__makeref", "__reftype", "interface", "namespace", "protected", "unchecked" },
			new string[2] { "__refvalue", "stackalloc" }
		};

		private static readonly Regex RelatedSymbolsRegex = new Regex("\n            \\(Location\\ of\\ the\\ symbol\\ related\\ to\\ previous\\ (warning|error)\\)\n\t\t\t", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		private string FileExtension => ".cs";

		internal CSharpCodeGenerator()
		{
		}

		public bool Supports(GeneratorSupport support)
		{
			return (support & (GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.GotoStatements | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.DeclareValueTypes | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareDelegates | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareEvents | GeneratorSupport.AssemblyAttributes | GeneratorSupport.ParameterAttributes | GeneratorSupport.ReferenceParameters | GeneratorSupport.ChainedConstructorArguments | GeneratorSupport.NestedTypes | GeneratorSupport.MultipleInterfaceMembers | GeneratorSupport.PublicStaticMembers | GeneratorSupport.ComplexExpressions | GeneratorSupport.Win32Resources | GeneratorSupport.Resources | GeneratorSupport.PartialTypes | GeneratorSupport.GenericTypeReference | GeneratorSupport.GenericTypeDeclaration | GeneratorSupport.DeclareIndexerProperties)) == support;
		}

		public string CreateEscapedIdentifier(string name)
		{
			return CSharpHelpers.CreateEscapedIdentifier(name);
		}

		CompilerResults ICodeCompiler.CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			try
			{
				return FromSourceBatch(options, sources);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
		}

		private CompilerResults FromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			string[] array = new string[sources.Length];
			for (int i = 0; i < sources.Length; i++)
			{
				string text = options.TempFiles.AddExtension(i + FileExtension);
				using (FileStream stream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					using StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
					streamWriter.Write(sources[i]);
					streamWriter.Flush();
				}
				array[i] = text;
			}
			return FromFileBatch(options, array);
		}

		private CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			CompilerResults results = new CompilerResults(options.TempFiles);
			Process process = new Process();
			if (Path.DirectorySeparatorChar == '\\')
			{
				process.StartInfo.FileName = MonoToolsLocator.Mono;
				process.StartInfo.Arguments = "\"" + MonoToolsLocator.McsCSharpCompiler + "\" ";
			}
			else
			{
				process.StartInfo.FileName = MonoToolsLocator.McsCSharpCompiler;
			}
			process.StartInfo.Arguments += BuildArgs(options, fileNames, _provOptions);
			ManualResetEvent stderr_completed = new ManualResetEvent(initialState: false);
			ManualResetEvent stdout_completed = new ManualResetEvent(initialState: false);
			process.StartInfo.EnvironmentVariables.Remove("MONO_GC_PARAMS");
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				if (args.Data != null)
				{
					results.Output.Add(args.Data);
				}
				else
				{
					stderr_completed.Set();
				}
			};
			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				if (args.Data == null)
				{
					stdout_completed.Set();
				}
			};
			ProcessStartInfo startInfo = process.StartInfo;
			Encoding standardOutputEncoding = (process.StartInfo.StandardErrorEncoding = Encoding.UTF8);
			startInfo.StandardOutputEncoding = standardOutputEncoding;
			try
			{
				process.Start();
			}
			catch (Exception ex)
			{
				if (ex is Win32Exception ex2)
				{
					throw new SystemException($"Error running {process.StartInfo.FileName}: {Win32Exception.GetErrorMessage(ex2.NativeErrorCode)}");
				}
				throw;
			}
			try
			{
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();
				results.NativeCompilerReturnValue = process.ExitCode;
			}
			finally
			{
				stderr_completed.WaitOne(TimeSpan.FromSeconds(30.0));
				stdout_completed.WaitOne(TimeSpan.FromSeconds(30.0));
				process.Close();
			}
			bool flag = true;
			StringEnumerator enumerator = results.Output.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CompilerError compilerError = CreateErrorFromString(enumerator.Current);
					if (compilerError != null)
					{
						results.Errors.Add(compilerError);
						if (!compilerError.IsWarning)
						{
							flag = false;
						}
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
			if (results.Output.Count > 0)
			{
				results.Output.Insert(0, process.StartInfo.FileName + " " + process.StartInfo.Arguments + Environment.NewLine);
			}
			if (flag)
			{
				if (!File.Exists(options.OutputAssembly))
				{
					StringBuilder stringBuilder = new StringBuilder();
					enumerator = results.Output.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							stringBuilder.Append(current + Environment.NewLine);
						}
					}
					finally
					{
						if (enumerator is IDisposable disposable2)
						{
							disposable2.Dispose();
						}
					}
					throw new Exception("Compiler failed to produce the assembly. Output: '" + stringBuilder.ToString() + "'");
				}
				if (options.GenerateInMemory)
				{
					using FileStream fileStream = File.OpenRead(options.OutputAssembly);
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, array.Length);
					results.CompiledAssembly = Assembly.Load(array, null);
					fileStream.Close();
				}
				else
				{
					results.PathToAssembly = options.OutputAssembly;
				}
			}
			else
			{
				results.CompiledAssembly = null;
			}
			return results;
		}

		private static string BuildArgs(CompilerParameters options, string[] fileNames, IDictionary<string, string> providerOptions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (options.GenerateExecutable)
			{
				stringBuilder.Append("/target:exe ");
			}
			else
			{
				stringBuilder.Append("/target:library ");
			}
			string privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			if (privateBinPath != null && privateBinPath.Length > 0)
			{
				stringBuilder.AppendFormat("/lib:\"{0}\" ", privateBinPath);
			}
			if (options.Win32Resource != null)
			{
				stringBuilder.AppendFormat("/win32res:\"{0}\" ", options.Win32Resource);
			}
			if (options.IncludeDebugInformation)
			{
				stringBuilder.Append("/debug+ /optimize- ");
			}
			else
			{
				stringBuilder.Append("/debug- /optimize+ ");
			}
			if (options.TreatWarningsAsErrors)
			{
				stringBuilder.Append("/warnaserror ");
			}
			if (options.WarningLevel >= 0)
			{
				stringBuilder.AppendFormat("/warn:{0} ", options.WarningLevel);
			}
			if (options.OutputAssembly == null || options.OutputAssembly.Length == 0)
			{
				string extension = (options.GenerateExecutable ? "exe" : "dll");
				options.OutputAssembly = GetTempFileNameWithExtension(options.TempFiles, extension, !options.GenerateInMemory);
			}
			stringBuilder.AppendFormat("/out:\"{0}\" ", options.OutputAssembly);
			StringEnumerator enumerator = options.ReferencedAssemblies.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					if (current != null && current.Length != 0)
					{
						stringBuilder.AppendFormat("/r:\"{0}\" ", current);
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
			if (options.CompilerOptions != null)
			{
				stringBuilder.Append(options.CompilerOptions);
				stringBuilder.Append(" ");
			}
			enumerator = options.EmbeddedResources.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current2 = enumerator.Current;
					stringBuilder.AppendFormat("/resource:\"{0}\" ", current2);
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable2)
				{
					disposable2.Dispose();
				}
			}
			enumerator = options.LinkedResources.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current3 = enumerator.Current;
					stringBuilder.AppendFormat("/linkresource:\"{0}\" ", current3);
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable3)
				{
					disposable3.Dispose();
				}
			}
			if (providerOptions != null && providerOptions.Count > 0)
			{
				if (!providerOptions.TryGetValue("CompilerVersion", out var value))
				{
					value = "3.5";
				}
				if (value.Length >= 1 && value[0] == 'v')
				{
					value = value.Substring(1);
				}
				if (!(value == "2.0"))
				{
					if (value == "3.5")
					{
					}
				}
				else
				{
					stringBuilder.Append("/langversion:ISO-2 ");
				}
			}
			stringBuilder.Append("/noconfig ");
			stringBuilder.Append(" -- ");
			foreach (string arg in fileNames)
			{
				stringBuilder.AppendFormat("\"{0}\" ", arg);
			}
			return stringBuilder.ToString();
		}

		private static CompilerError CreateErrorFromString(string error_string)
		{
			if (error_string.StartsWith("BETA"))
			{
				return null;
			}
			if (error_string == null || error_string == "")
			{
				return null;
			}
			CompilerError compilerError = new CompilerError();
			Match match = new Regex("\n\t\t\t^\n\t\t\t(\\s*(?<file>[^\\(]+)                         # filename (optional)\n\t\t\t (\\((?<line>\\d*)(,(?<column>\\d*[\\+]*))?\\))? # line+column (optional)\n\t\t\t :\\s+)?\n\t\t\t(?<level>\\w+)                               # error|warning\n\t\t\t\\s+\n\t\t\t(?<number>[^:]*\\d)                          # CS1234\n\t\t\t:\n\t\t\t\\s*\n\t\t\t(?<message>.*)$", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace).Match(error_string);
			if (!match.Success)
			{
				match = RelatedSymbolsRegex.Match(error_string);
				if (!match.Success)
				{
					compilerError.ErrorText = error_string;
					compilerError.IsWarning = false;
					compilerError.ErrorNumber = "";
					return compilerError;
				}
				return null;
			}
			if (string.Empty != match.Result("${file}"))
			{
				compilerError.FileName = match.Result("${file}");
			}
			if (string.Empty != match.Result("${line}"))
			{
				compilerError.Line = int.Parse(match.Result("${line}"));
			}
			if (string.Empty != match.Result("${column}"))
			{
				compilerError.Column = int.Parse(match.Result("${column}").Trim('+'));
			}
			string text = match.Result("${level}");
			if (text == "warning")
			{
				compilerError.IsWarning = true;
			}
			else if (text != "error")
			{
				return null;
			}
			compilerError.ErrorNumber = match.Result("${number}");
			compilerError.ErrorText = match.Result("${message}");
			return compilerError;
		}

		private static string GetTempFileNameWithExtension(TempFileCollection temp_files, string extension, bool keepFile)
		{
			return temp_files.AddExtension(extension, keepFile);
		}
	}
}
