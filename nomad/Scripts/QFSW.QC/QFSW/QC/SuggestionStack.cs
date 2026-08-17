using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QFSW.QC.Pooling;
using QFSW.QC.Utilities;

namespace QFSW.QC
{
	public class SuggestionStack
	{
		private readonly QuantumSuggestor _suggestor;

		private readonly List<SuggestionSet> _suggestionSets = new List<SuggestionSet>();

		private readonly Pool<SuggestionSet> _setPool = new Pool<SuggestionSet>();

		private readonly StringBuilder _stringBuilder = new StringBuilder();

		public SuggestionSet TopmostSuggestionSet => _suggestionSets.LastOrDefault();

		public IQcSuggestion TopmostSuggestion => TopmostSuggestionSet?.CurrentSelection;

		public event Action<SuggestionSet> OnSuggestionSetCreated;

		public SuggestionStack()
			: this(new QuantumSuggestor())
		{
		}

		public SuggestionStack(QuantumSuggestor suggestor)
		{
			_suggestor = suggestor;
		}

		public void Clear()
		{
			while (PopSet())
			{
			}
		}

		public void UpdateStack(string prompt, SuggestorOptions options)
		{
			if (string.IsNullOrWhiteSpace(prompt))
			{
				Clear();
				return;
			}
			PropagateContextChanges(prompt);
			PopInvalidLayers();
			BuildInitialLayer(prompt, options);
			BuildNewLayers(options);
		}

		private SuggestionContext? GetInnerSuggestionContext(SuggestionSet set)
		{
			IQcSuggestion currentSelection = set.CurrentSelection;
			SuggestionContext context = set.Context;
			return currentSelection?.GetInnerSuggestionContext(context);
		}

		private void InvalidateLayersFrom(int index)
		{
			PopSets(_suggestionSets.Count - index);
		}

		private void PropagateContextChanges(string prompt)
		{
			if (_suggestionSets.Count == 0)
			{
				return;
			}
			_suggestionSets[0].Context.Prompt = prompt;
			for (int i = 0; i < _suggestionSets.Count - 1; i++)
			{
				SuggestionSet set = _suggestionSets[i];
				SuggestionContext? innerSuggestionContext = GetInnerSuggestionContext(set);
				if (innerSuggestionContext.HasValue)
				{
					_suggestionSets[i + 1].Context = innerSuggestionContext.Value;
				}
				else
				{
					InvalidateLayersFrom(i + 1);
				}
			}
		}

		private void PopInvalidLayers()
		{
			for (int i = 0; i < _suggestionSets.Count; i++)
			{
				SuggestionSet suggestionSet = _suggestionSets[i];
				SuggestionContext context = suggestionSet.Context;
				IQcSuggestion currentSelection = suggestionSet.CurrentSelection;
				if (currentSelection == null || !currentSelection.MatchesPrompt(context.Prompt))
				{
					InvalidateLayersFrom(i);
				}
			}
		}

		private void BuildInitialLayer(string prompt, SuggestorOptions options)
		{
			if (_suggestionSets.Count == 0)
			{
				SuggestionContext context = new SuggestionContext
				{
					Prompt = prompt,
					Depth = 0,
					TargetType = null
				};
				CreateLayer(context, options);
			}
		}

		private void BuildNewLayers(SuggestorOptions options)
		{
			if (TopmostSuggestion != null)
			{
				SuggestionSet topmostSuggestionSet = TopmostSuggestionSet;
				SuggestionContext? innerSuggestionContext = GetInnerSuggestionContext(topmostSuggestionSet);
				if (innerSuggestionContext.HasValue && CreateLayer(innerSuggestionContext.Value, options))
				{
					BuildNewLayers(options);
				}
			}
		}

		private void TryAutoSelectSuggestion(SuggestionSet set, string prompt)
		{
			if (set.CurrentSelection == null)
			{
				IQcSuggestion qcSuggestion = set.Suggestions.FirstOrDefault();
				if (qcSuggestion != null && qcSuggestion.MatchesPrompt(prompt))
				{
					set.SelectionIndex = 0;
				}
			}
		}

		private bool CreateLayer(SuggestionContext context, SuggestorOptions options)
		{
			IEnumerable<IQcSuggestion> suggestions = _suggestor.GetSuggestions(context, options);
			SuggestionSet suggestionSet = PushSet();
			suggestionSet.Context = context;
			suggestionSet.Suggestions.AddRange(suggestions);
			if (suggestionSet.Suggestions.Count == 0)
			{
				PopSet();
				return false;
			}
			this.OnSuggestionSetCreated?.Invoke(suggestionSet);
			TryAutoSelectSuggestion(suggestionSet, context.Prompt);
			return true;
		}

		public string GetCompletion()
		{
			if (_suggestionSets.Count == 0)
			{
				return string.Empty;
			}
			IEnumerable<IQcSuggestion> enumerable = from x in _suggestionSets
				select x.CurrentSelection into x
				where x != null
				select x;
			SuggestionContext context = _suggestionSets[0].Context;
			_stringBuilder.Clear();
			foreach (IQcSuggestion item in enumerable)
			{
				string prompt = context.Prompt;
				SuggestionContext? innerSuggestionContext = item.GetInnerSuggestionContext(context);
				if (innerSuggestionContext.HasValue)
				{
					_stringBuilder.Append(prompt, 0, prompt.Length - innerSuggestionContext.Value.Prompt.Length);
				}
				else
				{
					_stringBuilder.Append(item.GetCompletion(prompt));
				}
			}
			return _stringBuilder.ToString();
		}

		public string GetCompletionTail()
		{
			_stringBuilder.Clear();
			foreach (SuggestionSet item in _suggestionSets.Reversed())
			{
				SuggestionContext context = item.Context;
				_stringBuilder.Append(item.CurrentSelection?.GetCompletionTail(context.Prompt));
			}
			return _stringBuilder.ToString();
		}

		public bool SetSuggestionIndex(int suggestionIndex)
		{
			if (_suggestionSets.Count == 0)
			{
				return false;
			}
			if (suggestionIndex < 0 || suggestionIndex > TopmostSuggestionSet.Suggestions.Count)
			{
				return false;
			}
			TopmostSuggestionSet.SelectionIndex = suggestionIndex;
			TopmostSuggestionSet.Context.Prompt = TopmostSuggestion.PrimarySignature;
			return true;
		}

		private SuggestionSet PushSet()
		{
			SuggestionSet suggestionSet = _setPool.GetObject();
			suggestionSet.SelectionIndex = -1;
			suggestionSet.Suggestions.Clear();
			_suggestionSets.Add(suggestionSet);
			return suggestionSet;
		}

		private bool PopSet()
		{
			if (_suggestionSets.Count > 0)
			{
				int index = _suggestionSets.Count - 1;
				SuggestionSet obj = _suggestionSets[index];
				_suggestionSets.RemoveAt(index);
				_setPool.Release(obj);
				return true;
			}
			return false;
		}

		private bool PopSets(int count)
		{
			bool flag;
			for (flag = true; flag && count-- > 0; flag &= PopSet())
			{
			}
			return flag;
		}
	}
}
