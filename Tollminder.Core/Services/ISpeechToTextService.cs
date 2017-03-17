﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface ISpeechToTextService
    {
        Task<bool> AskQuestionAsync(string question);
    }
}

