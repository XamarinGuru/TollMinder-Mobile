using System;
using System.Threading.Tasks;
using Foundation;
using Google.SignIn;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
    public class TouchGPlusLoginService : SignInUIDelegate, IGPlusLoginService
    {
        public TouchGPlusLoginService()
        {
        }

        public Task<PersonData> GetPersonData()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            
        }

        public void ReleaseResources()
        {
            
        }
    }
}
