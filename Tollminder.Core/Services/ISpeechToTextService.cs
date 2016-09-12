using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface ISpeechToTextService
	{
		void AskQuestion(string question);
		void CheckResult(IList<string> matches);
	}
}

