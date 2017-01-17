using System;
using Android.Views;
using MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Plugins;

namespace Tollminder.Droid.Controls.MvxExpandableList
{
    public class Plugin: IMvxPlugin
    {
        public void Load()
        {
            // The type cache is not initialized yet when the plugins are loaded. So add a callback when is does.
            Mvx.CallbackWhenRegistered<IMvxTypeCache<View>>(FillViewTypes);
            Mvx.CallbackWhenRegistered<IMvxAxmlNameViewTypeResolver>(FillAxmlViewTypeResolver);
        }

        private void FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver viewTypeResolver)
        {
            viewTypeResolver.ViewNamespaceAbbreviations["DeapExt"] = "DeapExtensions.Binding.Droid.Views";
        }

        protected void FillViewTypes(IMvxTypeCache<View> cache)
        {
            cache.AddAssembly(typeof(ListViews.BindableGroupListView).Assembly);
        }
    }
}
