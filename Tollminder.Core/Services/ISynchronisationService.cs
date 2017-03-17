﻿using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface ISynchronisationService
    {
        Task DataSynchronisationAsync();
        Task<bool> AuthorizeTokenSynchronisationAsync();
    }
}
