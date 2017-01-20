using Android;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace Tollminder.Droid.Views.Fragments
{
    [Register("tollminder.droid.views.fragments.ButtonContainerFragment")]
    public class ButtonContainerFragment : MvxFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.button_container_fragment, null);
        }
    }
}