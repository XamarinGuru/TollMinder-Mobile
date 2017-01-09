using System;
using System.Diagnostics.Contracts;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views.Fragments
{
    public class CalendarDialogFragment : MvxDialogFragment<CalendarViewModel>
    {
        ImageButton backToPayHistory;
        //View view;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.EnsureBindingContextSet(savedInstanceState);
            //Contract.Ensures(Contract.Result<Dialog>() != null);
            View view = this.BindingInflate(Resource.Layout.calendar_fragment, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            return (builder.SetView(view).Create());
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.calendar_fragment, container, false);
            backToPayHistory = view.FindViewById<ImageButton>(Resource.Id.calendar_btn_back_to_payhistory);
            var set = this.CreateBindingSet<CalendarDialogFragment, CalendarViewModel>();
            set.Bind(backToPayHistory).To(vm => vm.BackToPayHistoryCommand);
            set.Apply();
            return view;//base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
        }

        public override void OnCancel(IDialogInterface dialog)
        { 
            base.OnCancel(dialog);
        }
    }
}