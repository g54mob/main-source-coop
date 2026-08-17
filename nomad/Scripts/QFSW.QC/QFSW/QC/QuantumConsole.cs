using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QFSW.QC.Pooling;
using QFSW.QC.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFSW.QC
{
	[DisallowMultipleComponent]
	public class QuantumConsole : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _containerRect;

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private RectTransform _suggestionPopupRect;

		[SerializeField]
		private RectTransform _jobCounterRect;

		[SerializeField]
		private Image[] _panels;

		[SerializeField]
		private QuantumTheme _theme;

		[SerializeField]
		private QuantumKeyConfig _keyConfig;

		[SerializeField]
		private QuantumLocalization _localization;

		[Command("verbose-errors", "If errors caused by the Quantum Console Processor or commands should be logged in verbose mode.", MonoTargetType.Registry, Platform.AllPlatforms)]
		[SerializeField]
		private bool _verboseErrors;

		[Command("verbose-logging", "The minimum log severity required to use verbose logging.", MonoTargetType.Registry, Platform.AllPlatforms)]
		[SerializeField]
		private LoggingThreshold _verboseLogging;

		[Command("logging-level", "The minimum log severity required to intercept and display the log.", MonoTargetType.Registry, Platform.AllPlatforms)]
		[SerializeField]
		private LoggingThreshold _loggingLevel = LoggingThreshold.Always;

		[SerializeField]
		private LoggingThreshold _openOnLogLevel;

		[SerializeField]
		private bool _interceptDebugLogger = true;

		[SerializeField]
		private bool _interceptWhilstInactive = true;

		[SerializeField]
		private bool _prependTimestamps;

		[SerializeField]
		private SupportedState _supportedState;

		[SerializeField]
		private bool _activateOnStartup = true;

		[SerializeField]
		private bool _initialiseOnStartup;

		[SerializeField]
		private bool _focusOnActivate = true;

		[SerializeField]
		private bool _closeOnSubmit;

		[SerializeField]
		private bool _singletonMode;

		[SerializeField]
		private AutoScrollOptions _autoScroll = AutoScrollOptions.OnInvoke;

		[SerializeField]
		private bool _enableAutocomplete = true;

		[SerializeField]
		private bool _showPopupDisplay = true;

		[SerializeField]
		private SortOrder _suggestionDisplayOrder = SortOrder.Descending;

		[SerializeField]
		private int _maxSuggestionDisplaySize = -1;

		[SerializeField]
		private bool _useFuzzySearch;

		[SerializeField]
		private bool _caseSensitiveSearch = true;

		[SerializeField]
		private bool _collapseSuggestionOverloads = true;

		[SerializeField]
		private bool _showCurrentJobs = true;

		[SerializeField]
		private bool _blockOnAsync;

		[SerializeField]
		private bool _storeCommandHistory = true;

		[SerializeField]
		private bool _storeDuplicateCommands = true;

		[SerializeField]
		private bool _storeAdjacentDuplicateCommands;

		[SerializeField]
		private int _commandHistorySize = -1;

		[SerializeField]
		private int _maxStoredLogs = 1024;

		[SerializeField]
		private int _maxLogSize = 8192;

		[SerializeField]
		private bool _showInitLogs = true;

		[SerializeField]
		private TMP_InputField _consoleInput;

		[SerializeField]
		private TextMeshProUGUI _inputPlaceholderText;

		[SerializeField]
		private TextMeshProUGUI _consoleLogText;

		[SerializeField]
		private TextMeshProUGUI _consoleSuggestionText;

		[SerializeField]
		private TextMeshProUGUI _suggestionPopupText;

		[SerializeField]
		private TextMeshProUGUI _jobCounterText;

		private readonly QuantumSerializer _serializer = new QuantumSerializer();

		private SuggestionStack _suggestionStack;

		private ILogStorage _logStorage;

		private ILogQueue _logQueue;

		private readonly List<string> _previousCommands = new List<string>();

		private readonly List<Task> _currentTasks = new List<Task>();

		private readonly List<IEnumerator<ICommandAction>> _currentActions = new List<IEnumerator<ICommandAction>>();

		private readonly StringBuilderPool _stringBuilderPool = new StringBuilderPool();

		private int _selectedPreviousCommandIndex = -1;

		private string _currentInput;

		private string _previousInput;

		private bool _isGeneratingTable;

		private bool _consoleRequiresFlush;

		private bool _isHandlingUserResponse;

		private ResponseConfig _currentResponseConfig;

		private Action<string> _onSubmitResponseCallback;

		private TextMeshProUGUI[] _textComponents;

		private readonly Type _voidTaskType = typeof(Task<>).MakeGenericType(Type.GetType("System.Threading.Tasks.VoidTaskResult"));

		public static QuantumConsole Instance { get; private set; }

		public QuantumTheme Theme => _theme;

		public QuantumKeyConfig KeyConfig
		{
			get
			{
				return _keyConfig;
			}
			set
			{
				_keyConfig = value;
			}
		}

		public QuantumLocalization Localization
		{
			get
			{
				return _localization;
			}
			set
			{
				_localization = value;
			}
		}

		[Command("max-logs", MonoTargetType.Registry, Platform.AllPlatforms)]
		[CommandDescription("The maximum number of logs that may be stored in the log storage before old logs are removed.")]
		public int MaxStoredLogs
		{
			get
			{
				return _maxStoredLogs;
			}
			set
			{
				_maxStoredLogs = value;
				if (_logStorage != null)
				{
					_logStorage.MaxStoredLogs = value;
				}
				if (_logQueue != null)
				{
					_logQueue.MaxStoredLogs = value;
				}
			}
		}

		private bool IsBlockedByAsync
		{
			get
			{
				if ((_blockOnAsync && _currentTasks.Count > 0) || _currentActions.Count > 0)
				{
					return !_isHandlingUserResponse;
				}
				return false;
			}
		}

		public bool IsActive { get; private set; }

		public bool IsFocused
		{
			get
			{
				if (IsActive && (bool)_consoleInput)
				{
					return _consoleInput.isFocused;
				}
				return false;
			}
		}

		public bool AreActionsExecuting => _currentActions.Count > 0;

		public event Action OnStateChange;

		public event Action<string> OnInvoke;

		public event Action OnClear;

		public event Action<ILog> OnLog;

		public event Action OnActivate;

		public event Action OnDeactivate;

		public event Action<SuggestionSet> OnSuggestionSetGenerated;

		public void ApplyTheme(QuantumTheme theme, bool forceRefresh = false)
		{
			_theme = theme;
			if (!theme)
			{
				return;
			}
			if (_textComponents == null || forceRefresh)
			{
				_textComponents = GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
			}
			TextMeshProUGUI[] textComponents = _textComponents;
			foreach (TextMeshProUGUI textMeshProUGUI in textComponents)
			{
				if ((bool)theme.Font)
				{
					textMeshProUGUI.font = theme.Font;
				}
			}
			Image[] panels = _panels;
			foreach (Image obj in panels)
			{
				obj.material = theme.PanelMaterial;
				obj.color = theme.PanelColor;
			}
		}

		protected virtual void Update()
		{
			if (!IsActive)
			{
				if (_keyConfig.ShowConsoleKey.IsPressed() || _keyConfig.ToggleConsoleVisibilityKey.IsPressed())
				{
					Activate();
				}
				return;
			}
			ProcessAsyncTasks();
			ProcessActions();
			HandleAsyncJobCounter();
			if (_keyConfig.HideConsoleKey.IsPressed() || _keyConfig.ToggleConsoleVisibilityKey.IsPressed())
			{
				Deactivate();
				return;
			}
			if (QuantumConsoleProcessor.TableIsGenerating)
			{
				_consoleInput.interactable = false;
				string text = (_logStorage.GetLogString() + "\n" + GetTableGenerationText()).Trim();
				if (text != _consoleLogText.text)
				{
					if (_showInitLogs)
					{
						this.OnStateChange?.Invoke();
						_consoleLogText.text = text;
					}
					if ((bool)_inputPlaceholderText)
					{
						_inputPlaceholderText.text = _localization.Loading;
					}
				}
				return;
			}
			if (IsBlockedByAsync)
			{
				this.OnStateChange?.Invoke();
				_consoleInput.interactable = false;
				if ((bool)_inputPlaceholderText)
				{
					_inputPlaceholderText.text = _localization.ExecutingAsyncCommand;
				}
			}
			else if (!_consoleInput.interactable)
			{
				this.OnStateChange?.Invoke();
				_consoleInput.interactable = true;
				if ((bool)_inputPlaceholderText)
				{
					_inputPlaceholderText.text = _localization.EnterCommand;
				}
				OverrideConsoleInput(string.Empty);
				if (_isGeneratingTable)
				{
					if (_showInitLogs)
					{
						AppendLog(new Log(GetTableGenerationText()));
						_consoleLogText.text = _logStorage.GetLogString();
					}
					_isGeneratingTable = false;
					ScrollConsoleToLatest();
				}
			}
			_previousInput = _currentInput;
			_currentInput = _consoleInput.text;
			if (_consoleInput.isFocused && _keyConfig.DeleteWordBeforeCursorKey.IsPressed())
			{
				DeleteWordBeforeCursor();
			}
			if (_currentInput != _previousInput)
			{
				OnInputChange();
			}
			else if (!IsBlockedByAsync)
			{
				if (InputHelper.GetKeyDown(_keyConfig.SubmitCommandKey))
				{
					InvokeCommand();
				}
				if (_storeCommandHistory)
				{
					ProcessCommandHistory();
				}
				ProcessAutocomplete();
			}
		}

		private void LateUpdate()
		{
			if (IsActive)
			{
				FlushQueuedLogs();
				FlushToConsoleText();
			}
		}

		private string GetTableGenerationText()
		{
			string text = string.Format(_localization.InitializationProgress, QuantumConsoleProcessor.LoadedCommandCount);
			if (QuantumConsoleProcessor.TableIsGenerating)
			{
				return text + "...";
			}
			string text2 = ((_theme == null) ? _localization.InitializationComplete : _localization.InitializationComplete.ColorText(_theme.SuccessColor));
			return text + "\n" + text2;
		}

		private void ProcessCommandHistory()
		{
			if (InputHelper.GetKeyDown(_keyConfig.NextCommandKey) || InputHelper.GetKeyDown(_keyConfig.PreviousCommandKey))
			{
				if (InputHelper.GetKeyDown(_keyConfig.NextCommandKey))
				{
					_selectedPreviousCommandIndex++;
				}
				else if (_selectedPreviousCommandIndex > 0)
				{
					_selectedPreviousCommandIndex--;
				}
				_selectedPreviousCommandIndex = Mathf.Clamp(_selectedPreviousCommandIndex, -1, _previousCommands.Count - 1);
				if (_selectedPreviousCommandIndex > -1)
				{
					string newInput = _previousCommands[_previousCommands.Count - _selectedPreviousCommandIndex - 1];
					OverrideConsoleInput(newInput);
				}
			}
		}

		private void UpdateSuggestions()
		{
			if (_isHandlingUserResponse)
			{
				ClearSuggestions();
				ClearPopup();
				return;
			}
			SuggestorOptions options = new SuggestorOptions
			{
				CaseSensitive = _caseSensitiveSearch,
				Fuzzy = _useFuzzySearch,
				CollapseOverloads = _collapseSuggestionOverloads
			};
			_suggestionStack.UpdateStack(_currentInput, options);
			UpdateSuggestionText();
			if (_showPopupDisplay)
			{
				UpdatePopupDisplay();
			}
		}

		private void ProcessAutocomplete()
		{
			if (!_enableAutocomplete || (!_keyConfig.SelectNextSuggestionKey.IsPressed() && !_keyConfig.SelectPreviousSuggestionKey.IsPressed()))
			{
				return;
			}
			SuggestionSet topmostSuggestionSet = _suggestionStack.TopmostSuggestionSet;
			if (topmostSuggestionSet != null && topmostSuggestionSet.Suggestions.Count > 0)
			{
				if (_keyConfig.SelectNextSuggestionKey.IsPressed())
				{
					topmostSuggestionSet.SelectionIndex++;
				}
				if (_keyConfig.SelectPreviousSuggestionKey.IsPressed())
				{
					topmostSuggestionSet.SelectionIndex--;
				}
				topmostSuggestionSet.SelectionIndex += topmostSuggestionSet.Suggestions.Count;
				topmostSuggestionSet.SelectionIndex %= topmostSuggestionSet.Suggestions.Count;
				SetSuggestion(topmostSuggestionSet.SelectionIndex);
			}
		}

		private void FormatSuggestion(IQcSuggestion suggestion, bool selected, StringBuilder buffer)
		{
			if (!_theme)
			{
				buffer.Append(suggestion.FullSignature);
				return;
			}
			Color white = Color.white;
			Color suggestionColor = _theme.SuggestionColor;
			if (selected)
			{
				white *= _theme.SelectedSuggestionColor;
				suggestionColor *= _theme.SelectedSuggestionColor;
			}
			buffer.AppendColoredText(suggestion.PrimarySignature, white);
			buffer.AppendColoredText(suggestion.SecondarySignature, suggestionColor);
		}

		private string GetFormattedSuggestions(SuggestionSet suggestionSet)
		{
			StringBuilder stringBuilder = _stringBuilderPool.GetStringBuilder();
			GetFormattedSuggestions(suggestionSet, stringBuilder);
			return _stringBuilderPool.ReleaseAndToString(stringBuilder);
		}

		private void GetFormattedSuggestions(SuggestionSet suggestionSet, StringBuilder buffer)
		{
			int num = suggestionSet.Suggestions.Count;
			if (_maxSuggestionDisplaySize > 0)
			{
				num = Mathf.Min(num, _maxSuggestionDisplaySize + 1);
			}
			for (int i = 0; i < num; i++)
			{
				if (_maxSuggestionDisplaySize > 0 && i >= _maxSuggestionDisplaySize)
				{
					if ((bool)_theme && suggestionSet.SelectionIndex >= _maxSuggestionDisplaySize)
					{
						buffer.AppendColoredText("...", _theme.SelectedSuggestionColor);
					}
					else
					{
						buffer.Append("...");
					}
					continue;
				}
				bool selected = i == suggestionSet.SelectionIndex;
				buffer.Append("<link=");
				buffer.Append(i);
				buffer.Append(">");
				FormatSuggestion(suggestionSet.Suggestions[i], selected, buffer);
				buffer.AppendLine("</link>");
			}
		}

		private void UpdatePopupDisplay()
		{
			SuggestionSet topmostSuggestionSet = _suggestionStack.TopmostSuggestionSet;
			if (topmostSuggestionSet == null || topmostSuggestionSet.Suggestions.Count == 0)
			{
				ClearPopup();
			}
			else if ((bool)_suggestionPopupRect && (bool)_suggestionPopupText)
			{
				string text = GetFormattedSuggestions(topmostSuggestionSet);
				if (_suggestionDisplayOrder == SortOrder.Ascending)
				{
					text = text.ReverseItems('\n');
				}
				_suggestionPopupRect.gameObject.SetActive(value: true);
				_suggestionPopupText.text = text;
			}
		}

		public void SetSuggestion(int suggestionIndex)
		{
			if (!_suggestionStack.SetSuggestionIndex(suggestionIndex))
			{
				throw new ArgumentException($"Cannot set suggestion to index {suggestionIndex}.");
			}
			OverrideConsoleInput(_suggestionStack.GetCompletion());
			UpdateSuggestionText();
		}

		private void UpdateSuggestionText()
		{
			Color color = (_theme ? _theme.SuggestionColor : Color.gray);
			StringBuilder stringBuilder = _stringBuilderPool.GetStringBuilder();
			stringBuilder.AppendColoredText(_currentInput, Color.clear);
			stringBuilder.AppendColoredText(_suggestionStack.GetCompletionTail(), color);
			_consoleSuggestionText.text = _stringBuilderPool.ReleaseAndToString(stringBuilder);
		}

		public void OverrideConsoleInput(string newInput, bool shouldFocus = true)
		{
			_currentInput = newInput;
			_previousInput = newInput;
			_consoleInput.text = newInput;
			if (shouldFocus)
			{
				FocusConsoleInput();
			}
			OnInputChange();
		}

		private void DeleteWordBeforeCursor()
		{
			int num = _consoleInput.caretPosition + (_previousInput.Length - _currentInput.Length);
			int num2 = _previousInput.Length - num;
			string text = _previousInput.WithoutWord(num);
			OverrideConsoleInput(text, shouldFocus: false);
			int stringPosition = text.Length - num2;
			_consoleInput.stringPosition = stringPosition;
		}

		public void FocusConsoleInput()
		{
			_consoleInput.Select();
			_consoleInput.caretPosition = _consoleInput.text.Length;
			_consoleInput.selectionAnchorPosition = _consoleInput.text.Length;
			_consoleInput.MoveTextEnd(shift: false);
			_consoleInput.ActivateInputField();
		}

		private void OnInputChange()
		{
			if (_selectedPreviousCommandIndex >= 0 && _currentInput.Trim() != _previousCommands[_previousCommands.Count - _selectedPreviousCommandIndex - 1])
			{
				ClearHistoricalSuggestions();
			}
			if (_enableAutocomplete)
			{
				UpdateSuggestions();
			}
		}

		private void ClearHistoricalSuggestions()
		{
			_selectedPreviousCommandIndex = -1;
		}

		private void ClearSuggestions()
		{
			_suggestionStack.Clear();
			_consoleSuggestionText.text = string.Empty;
		}

		private void ClearPopup()
		{
			if ((bool)_suggestionPopupRect)
			{
				_suggestionPopupRect.gameObject.SetActive(value: false);
			}
			if ((bool)_suggestionPopupText)
			{
				_suggestionPopupText.text = string.Empty;
			}
		}

		public void InvokeCommand()
		{
			string text = _consoleInput.text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				string command = text.Trim();
				if (_isHandlingUserResponse)
				{
					HandleUserResponse(command);
					return;
				}
				InvokeCommand(command);
				OverrideConsoleInput(string.Empty);
				StoreCommand(command);
			}
		}

		private void HandleUserResponse(string command)
		{
			if (_currentResponseConfig.LogInput)
			{
				LogUserInput(command);
				StoreCommand(command);
			}
			_onSubmitResponseCallback(command);
			_onSubmitResponseCallback = null;
			_consoleInput.interactable = false;
			_isHandlingUserResponse = false;
			this.OnStateChange?.Invoke();
		}

		private void LogUserInput(string input)
		{
			ILog log = GenerateCommandLog(input);
			LogToConsole(log);
		}

		protected ILog GenerateCommandLog(string command)
		{
			string format = ((_theme != null) ? _theme.CommandLogFormat : "> {0}");
			if (command.Contains("<"))
			{
				command = "<noparse>" + command + "</noparse>";
			}
			string text = string.Format(format, command);
			if ((bool)_theme)
			{
				text = text.ColorText(_theme.CommandLogColor);
			}
			return new Log(text);
		}

		public object InvokeCommand(string command)
		{
			object obj = null;
			if (!string.IsNullOrWhiteSpace(command))
			{
				LogUserInput(command);
				string logText = string.Empty;
				try
				{
					obj = QuantumConsoleProcessor.InvokeCommand(command);
					if (!(obj is Task item))
					{
						if (!(obj is IEnumerator<ICommandAction> action))
						{
							if (obj is IEnumerable<ICommandAction> enumerable)
							{
								StartAction(enumerable.GetEnumerator());
							}
							else
							{
								logText = Serialize(obj);
							}
						}
						else
						{
							StartAction(action);
						}
					}
					else
					{
						_currentTasks.Add(item);
					}
				}
				catch (TargetInvocationException ex)
				{
					logText = GetInvocationErrorMessage(ex.InnerException);
				}
				catch (Exception e)
				{
					logText = GetErrorMessage(e);
				}
				LogToConsole(logText);
				this.OnInvoke?.Invoke(command);
				if (_autoScroll == AutoScrollOptions.OnInvoke)
				{
					ScrollConsoleToLatest();
				}
				if (_closeOnSubmit)
				{
					Deactivate();
				}
			}
			else
			{
				OverrideConsoleInput(string.Empty);
			}
			return obj;
		}

		[Command("qc-script-extern", "Executes an external source of QC script file, where each line is a separate QC command.", MonoTargetType.Registry, ~Platform.WebGLPlayer)]
		public async Task InvokeExternalCommandsAsync(string filePath)
		{
			using StreamReader reader = new StreamReader(filePath);
			while (!reader.EndOfStream)
			{
				if (InvokeCommand(await reader.ReadLineAsync()) is Task task)
				{
					await task;
					ProcessAsyncTasks();
				}
			}
		}

		public async Task InvokeCommandsAsync(IEnumerable<string> commands)
		{
			foreach (string command in commands)
			{
				if (InvokeCommand(command) is Task task)
				{
					await task;
					ProcessAsyncTasks();
				}
			}
		}

		private string GetErrorMessage(Exception e)
		{
			return GetErrorMessage(e, _localization.ConsoleError);
		}

		private string GetInvocationErrorMessage(Exception e)
		{
			return GetErrorMessage(e, _localization.CommandError);
		}

		private string GetErrorMessage(Exception e, string label)
		{
			string text = (_verboseErrors ? $"{label} ({e.GetType()}): {e.Message}\n{e.StackTrace}" : (label + ": " + e.Message));
			if (!_theme)
			{
				return text;
			}
			return text.ColorText(_theme.ErrorColor);
		}

		public void LogToConsoleAsync(string logText, LogType logType = LogType.Log)
		{
			if (!string.IsNullOrWhiteSpace(logText))
			{
				Log log = new Log(logText, logType);
				LogToConsoleAsync(log);
			}
		}

		public void LogToConsoleAsync(ILog log)
		{
			this.OnLog?.Invoke(log);
			_logQueue.QueueLog(log);
		}

		private void FlushQueuedLogs()
		{
			bool flag = false;
			bool flag2 = false;
			ILog log;
			while (_logQueue.TryDequeue(out log))
			{
				AppendLog(log);
				LoggingThreshold loggingThreshold = log.Type.ToLoggingThreshold();
				flag |= _autoScroll == AutoScrollOptions.Always;
				flag2 |= loggingThreshold <= _openOnLogLevel;
			}
			if (flag)
			{
				ScrollConsoleToLatest();
			}
			if (flag2)
			{
				Activate(shouldFocus: false);
			}
		}

		private void ProcessAsyncTasks()
		{
			for (int num = _currentTasks.Count - 1; num >= 0; num--)
			{
				if (_currentTasks[num].IsCompleted)
				{
					if (_currentTasks[num].IsFaulted)
					{
						foreach (Exception innerException in _currentTasks[num].Exception.InnerExceptions)
						{
							string invocationErrorMessage = GetInvocationErrorMessage(innerException);
							LogToConsole(invocationErrorMessage);
						}
					}
					else
					{
						Type type = _currentTasks[num].GetType();
						if (type.IsGenericTypeOf(typeof(Task<>)) && !_voidTaskType.IsAssignableFrom(type))
						{
							object value = _currentTasks[num].GetType().GetProperty("Result").GetValue(_currentTasks[num]);
							string logText = _serializer.SerializeFormatted(value, _theme);
							LogToConsole(logText);
						}
					}
					_currentTasks.RemoveAt(num);
				}
			}
		}

		public void BeginResponse(Action<string> onSubmitResponseCallback, ResponseConfig config)
		{
			if (onSubmitResponseCallback == null)
			{
				throw new ArgumentNullException("onSubmitResponseCallback");
			}
			_onSubmitResponseCallback = onSubmitResponseCallback;
			_currentResponseConfig = config;
			_isHandlingUserResponse = true;
			this.OnStateChange?.Invoke();
			_consoleInput.interactable = true;
			if ((bool)_inputPlaceholderText)
			{
				_inputPlaceholderText.text = _currentResponseConfig.InputPrompt;
			}
			FocusConsoleInput();
		}

		public void StartAction(IEnumerator<ICommandAction> action)
		{
			_currentActions.Add(action);
			ProcessActions();
		}

		public void CancelAllActions()
		{
			_currentActions.Clear();
		}

		private void ProcessActions()
		{
			if (_keyConfig.CancelActionsKey.IsPressed())
			{
				CancelAllActions();
				return;
			}
			ActionContext context = new ActionContext
			{
				Console = this
			};
			for (int num = _currentActions.Count - 1; num >= 0; num--)
			{
				IEnumerator<ICommandAction> action = _currentActions[num];
				try
				{
					if (action.Execute(context) != ActionState.Running)
					{
						_currentActions.RemoveAt(num);
					}
				}
				catch (Exception e)
				{
					_currentActions.RemoveAt(num);
					string invocationErrorMessage = GetInvocationErrorMessage(e);
					LogToConsole(invocationErrorMessage);
					break;
				}
			}
		}

		private void HandleAsyncJobCounter()
		{
			if (_showCurrentJobs && (bool)_jobCounterRect && (bool)_jobCounterText)
			{
				if (_currentTasks.Count == 0)
				{
					_jobCounterRect.gameObject.SetActive(value: false);
					return;
				}
				_jobCounterRect.gameObject.SetActive(value: true);
				_jobCounterText.text = string.Format("{0} job{1} in progress", _currentTasks.Count, (_currentTasks.Count == 1) ? "" : "s");
			}
		}

		public string Serialize(object value)
		{
			return _serializer.SerializeFormatted(value, _theme);
		}

		public void LogToConsole(string logText, bool newLine = true)
		{
			if (!string.IsNullOrEmpty(logText))
			{
				LogToConsole(new Log(logText, LogType.Log, newLine));
			}
		}

		public void LogToConsole(ILog log)
		{
			FlushQueuedLogs();
			AppendLog(log);
			this.OnLog?.Invoke(log);
			if (_autoScroll == AutoScrollOptions.Always)
			{
				ScrollConsoleToLatest();
			}
		}

		private void FlushToConsoleText()
		{
			if (_consoleRequiresFlush)
			{
				_consoleRequiresFlush = false;
				_consoleLogText.text = _logStorage.GetLogString();
			}
		}

		private ILog TruncateLog(ILog log)
		{
			if (log.Text.Length <= _maxLogSize || _maxLogSize < 0)
			{
				return log;
			}
			string text = string.Format(_localization.MaxLogSizeExceeded, log.Text.Length, _maxLogSize);
			if ((bool)_theme)
			{
				text = text.ColorText(_theme.ErrorColor);
			}
			return new Log(text, LogType.Error);
		}

		protected void AppendLog(ILog log)
		{
			_logStorage.AddLog(TruncateLog(log));
			RequireFlush();
		}

		protected void RequireFlush()
		{
			_consoleRequiresFlush = true;
		}

		public void RemoveLogTrace()
		{
			_logStorage.RemoveLog();
			RequireFlush();
		}

		private void ScrollConsoleToLatest()
		{
			if ((bool)_scrollRect)
			{
				_scrollRect.verticalNormalizedPosition = 0f;
			}
		}

		private void StoreCommand(string command)
		{
			if (_storeCommandHistory)
			{
				if (!_storeDuplicateCommands)
				{
					_previousCommands.Remove(command);
				}
				if (_storeAdjacentDuplicateCommands || _previousCommands.Count == 0 || _previousCommands[_previousCommands.Count - 1] != command)
				{
					_previousCommands.Add(command);
				}
				if (_commandHistorySize > 0 && _previousCommands.Count > _commandHistorySize)
				{
					_previousCommands.RemoveAt(0);
				}
			}
		}

		[Command("clear", "Clears the Quantum Console", MonoTargetType.Registry, Platform.AllPlatforms)]
		public void ClearConsole()
		{
			_logStorage.Clear();
			_logQueue.Clear();
			_consoleLogText.text = string.Empty;
			_consoleLogText.SetLayoutDirty();
			ClearBuffers();
			this.OnClear?.Invoke();
		}

		public string GetConsoleText()
		{
			return _consoleLogText.text;
		}

		protected virtual void ClearBuffers()
		{
			ClearHistoricalSuggestions();
			ClearSuggestions();
			ClearPopup();
		}

		private void Awake()
		{
			InitializeLogging();
		}

		private void OnEnable()
		{
			QuantumRegistry.RegisterObject(this);
			Application.logMessageReceivedThreaded += DebugIntercept;
			if (IsSupportedState())
			{
				if (_singletonMode)
				{
					if (Instance == null)
					{
						Instance = this;
						UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
					}
					else if (Instance != this)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
				}
				if (_activateOnStartup)
				{
					bool shouldFocus = SystemInfo.deviceType == DeviceType.Desktop;
					Activate(shouldFocus);
					return;
				}
				if (_initialiseOnStartup)
				{
					Initialize();
				}
				Deactivate();
			}
			else
			{
				DisableQC();
			}
		}

		private bool IsSupportedState()
		{
			SupportedState supportedState = SupportedState.Always;
			return _supportedState <= supportedState;
		}

		private void OnDisable()
		{
			QuantumRegistry.DeregisterObject(this);
			Application.logMessageReceivedThreaded -= DebugIntercept;
			Deactivate();
		}

		private void DisableQC()
		{
			Deactivate();
			base.enabled = false;
		}

		private void Initialize()
		{
			if (!QuantumConsoleProcessor.TableGenerated)
			{
				QuantumConsoleProcessor.GenerateCommandTable(deployThread: true);
				_consoleInput.interactable = false;
				_isGeneratingTable = true;
			}
			InitializeSuggestionStack();
			InitializeLogging();
			_consoleLogText.richText = true;
			_consoleSuggestionText.richText = true;
			ApplyTheme(_theme);
			if (!_keyConfig)
			{
				_keyConfig = ScriptableObject.CreateInstance<QuantumKeyConfig>();
			}
			if (!_localization)
			{
				_localization = ScriptableObject.CreateInstance<QuantumLocalization>();
			}
		}

		private void InitializeSuggestionStack()
		{
			if (_suggestionStack == null)
			{
				_suggestionStack = CreateSuggestionStack();
				_suggestionStack.OnSuggestionSetCreated += this.OnSuggestionSetGenerated;
			}
		}

		private void InitializeLogging()
		{
			_logStorage = _logStorage ?? CreateLogStorage();
			_logQueue = _logQueue ?? CreateLogQueue();
		}

		protected virtual ILogStorage CreateLogStorage()
		{
			return new LogStorage(_maxStoredLogs);
		}

		protected virtual ILogQueue CreateLogQueue()
		{
			return new LogQueue(_maxStoredLogs);
		}

		protected virtual SuggestionStack CreateSuggestionStack()
		{
			return new SuggestionStack();
		}

		public void Toggle()
		{
			if (IsActive)
			{
				Deactivate();
			}
			else
			{
				Activate();
			}
		}

		public void Activate()
		{
			Activate(_focusOnActivate);
		}

		public void Activate(bool shouldFocus)
		{
			Initialize();
			IsActive = true;
			_containerRect.gameObject.SetActive(value: true);
			OverrideConsoleInput(string.Empty, shouldFocus);
			if (!EventSystem.current)
			{
				Debug.LogWarning("Quantum Console's UI requires an EventSystem in the scene but there were none present.");
			}
			this.OnActivate?.Invoke();
		}

		public void Deactivate()
		{
			IsActive = false;
			_containerRect.gameObject.SetActive(value: false);
			this.OnDeactivate?.Invoke();
		}

		private void DebugIntercept(string condition, string stackTrace, LogType type)
		{
			if (_interceptDebugLogger && (IsActive || _interceptWhilstInactive) && _loggingLevel >= type.ToLoggingThreshold())
			{
				bool appendStackTrace = _verboseLogging >= type.ToLoggingThreshold();
				ILog log = ConstructDebugLog(condition, stackTrace, type, _prependTimestamps, appendStackTrace);
				LogToConsoleAsync(log);
			}
		}

		protected virtual ILog ConstructDebugLog(string condition, string stackTrace, LogType type, bool prependTimeStamp, bool appendStackTrace)
		{
			if (prependTimeStamp)
			{
				DateTime now = DateTime.Now;
				condition = string.Format(_theme ? _theme.TimestampFormat : "[{0:00}:{1:00}:{2:00}]", now.Hour, now.Minute, now.Second) + " " + condition;
			}
			if (appendStackTrace)
			{
				condition = condition + "\n" + stackTrace;
			}
			if ((bool)_theme)
			{
				switch (type)
				{
				case LogType.Warning:
					condition = condition.ColorText(_theme.WarningColor);
					break;
				case LogType.Error:
				case LogType.Assert:
				case LogType.Exception:
					condition = condition.ColorText(_theme.ErrorColor);
					break;
				}
			}
			return new Log(condition, type);
		}

		protected virtual void OnValidate()
		{
			MaxStoredLogs = _maxStoredLogs;
		}
	}
}
