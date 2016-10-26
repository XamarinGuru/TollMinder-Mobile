using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface ITextFromSpeechMappingService
	{
		AnswerType DetectAnswer(IList<string> matches);
	}
}

