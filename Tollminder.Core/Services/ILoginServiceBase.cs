using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface ILoginServiceBase
    {
        void Initialize();
        Task<PersonData> Login();
    }
}
