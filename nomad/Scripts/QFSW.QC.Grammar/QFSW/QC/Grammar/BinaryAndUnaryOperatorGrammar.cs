using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC.Grammar
{
	public abstract class BinaryAndUnaryOperatorGrammar : BinaryOperatorGrammar
	{
		private readonly HashSet<char> _operatorChars = new HashSet<char> { '+', '-', '*', '/', '&', '|', '^', '=', '!', ',' };

		private readonly HashSet<char> _ignoreChars = new HashSet<char> { ' ', '\0' };

		protected override int GetOperatorPosition(string value)
		{
			foreach (int item in TextProcessing.GetScopedSplitPoints(value, OperatorToken, TextProcessing.DefaultLeftScopers, TextProcessing.DefaultRightScopers).Reverse())
			{
				if (IsValidBinaryOperator(value, item))
				{
					return item;
				}
			}
			return -1;
		}

		private bool IsValidBinaryOperator(string value, int position)
		{
			while (position > 0)
			{
				char item = value[--position];
				if (_operatorChars.Contains(item))
				{
					return false;
				}
				if (!_ignoreChars.Contains(item))
				{
					return true;
				}
			}
			return false;
		}
	}
}
