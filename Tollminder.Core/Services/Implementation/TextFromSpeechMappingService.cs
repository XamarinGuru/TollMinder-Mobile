using System;
using System.Collections.Generic;
using System.Linq;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class TextFromSpeechMappingService : ITextFromSpeechMappingService
	{
		List<string> PositiveList = new List<string>()
		{
			"yea", 
			"yep",
			"yes"
		};

		List<string> NegativeList = new List<string>()
		{
			"nope",
			"no"
		};

		bool Contains(List<string> list, string match)
		{
			foreach (var item in list)
				if (match.Contains(item))
					return true;

			return false;
		}

		public AnswerType DetectAnswer(IList<string> matches)
		{
			if (matches == null)
				return AnswerType.Unknown;

			if (matches.FirstOrDefault(x => Contains(PositiveList, x)) != null)
				return AnswerType.Positive;

			if (matches.FirstOrDefault(x => Contains(NegativeList, x)) != null)
				return AnswerType.Negative;

			return AnswerType.Unknown;
		}
	}
}

