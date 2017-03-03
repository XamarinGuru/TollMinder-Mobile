using UIKit;
using Xamarin;

namespace Tollminder.Touch
{
    public class Application
    {
        static void Main(string[] args)
        {
            Insights.Initialize("951f79208700699208f428bfed2efe78eb950895");

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}