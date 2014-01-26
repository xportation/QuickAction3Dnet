using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using QuickAction3Dnet;

namespace QuickAction3DnetExample
{
	[Activity(Label = "QuickAction3DnetExample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		//action id
		private const int ID_UP = 1;
		private const int ID_DOWN = 2;
		private const int ID_SEARCH = 3;
		private const int ID_INFO = 4;
		private const int ID_ERASE = 5;        
		private const int ID_OK = 6;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			ActionItem nextItem = new ActionItem(ID_DOWN, "Next", Resources.GetDrawable(Resource.Drawable.menu_down_arrow));
			ActionItem prevItem = new ActionItem(ID_UP, "Prev", Resources.GetDrawable(Resource.Drawable.menu_up_arrow));
			ActionItem searchItem = new ActionItem(ID_SEARCH, "Find", Resources.GetDrawable(Resource.Drawable.menu_search));
			ActionItem infoItem = new ActionItem(ID_INFO, "Info", Resources.GetDrawable(Resource.Drawable.menu_info));
			ActionItem eraseItem = new ActionItem(ID_ERASE, "Clear", Resources.GetDrawable(Resource.Drawable.menu_eraser));
			ActionItem okItem = new ActionItem(ID_OK, "OK", Resources.GetDrawable(Resource.Drawable.menu_ok));

			prevItem.Sticky = true;
			nextItem.Sticky = true;

			//create QuickAction. Use QuickAction.VERTICAL or QuickAction.HORIZONTAL param to define layout 
			//orientation
			QuickAction quickAction = new QuickAction(this, QuickAction.Orientation.Horizontal);

			//add action items into QuickAction
			quickAction.AddActionItem(nextItem);
			quickAction.AddActionItem(prevItem);
			quickAction.AddActionItem(searchItem);
			quickAction.AddActionItem(infoItem);
			quickAction.AddActionItem(eraseItem);
			quickAction.AddActionItem(okItem);

			quickAction.ItemClickEvent+= (object sender, ActionItemClickEventArgs e) => {
				ActionItem actionItem = e.ActionItem;

				//here we can filter which action item was clicked with pos or actionId parameter
				if (actionItem.ActionId == ID_SEARCH) {
					Toast.MakeText(this, "Let's do some search action", ToastLength.Short).Show();
				} else if (actionItem.ActionId == ID_INFO) {
					Toast.MakeText(this, "I have no info this time", ToastLength.Short).Show();
				} else {
					Toast.MakeText(this, actionItem.Title + " selected", ToastLength.Short).Show();
				}
			};

			quickAction.DismissEvent+= (sender, e) => Toast.MakeText(this, "Dismissed", ToastLength.Short).Show();

			Button button1 = FindViewById<Button>(Resource.Id.btn1);
			button1.Click+= (sender, e) => quickAction.Show(button1);

			Button button2 = FindViewById<Button>(Resource.Id.btn2);
			button2.Click+= (sender, e) => quickAction.Show(button2);

			Button button3 = FindViewById<Button>(Resource.Id.btn3);
			button3.Click += (sender, e) => {
				quickAction.Show(button3);
				quickAction.SetAnimStyle(QuickAction.AnimationStyle.Reflect);
			};
		}

	}
}


