using System;
using MvvmCross.iOS.Views;

namespace MvxPagerTabStrip.Models
{
	public class MvxPagerTab
	{
		public IMvxIosView View { get; set; }
		public string Title { get; set; }
	}
}

