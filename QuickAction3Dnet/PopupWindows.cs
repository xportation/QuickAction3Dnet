using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Java.Lang;

namespace QuickAction3Dnet
{

	public class PopupWindows
	{
		protected Context context;
		protected PopupWindow window;
		protected View rootView;
		protected Drawable background = null;
		protected IWindowManager windowManager;
		
		public event EventHandler DismissEvent;

		public PopupWindows(Context context) {
			this.context	= context;
			window = new PopupWindow(context);
			windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

			window.DismissEvent+= (sender, e) => {
				onDismissEvent(sender, e);
			};
		}

		protected virtual void onDismissEvent(object sender, EventArgs e)
		{
			EventHandler handler = DismissEvent;
			if (handler != null)
			{
				handler(sender, e);
			}
		}

		protected void preShow() {
			if (rootView == null) 
				throw new IllegalStateException("setContentView was not called with a view to display.");
		
			if (background == null) 
				window.SetBackgroundDrawable(new BitmapDrawable());
			else 
				window.SetBackgroundDrawable(background);

			window.Width = WindowManagerLayoutParams.WrapContent;
			window.Height = WindowManagerLayoutParams.WrapContent;
			window.Touchable = true;
			window.Focusable = true;
			window.OutsideTouchable = true;

			window.ContentView = rootView;
		}

		public void SetBackgroundDrawable(Drawable background) 
		{
			this.background = background;
		}

		public View ContentView
		{
			set {
				rootView = value;			
				window.ContentView = value;
			}
		}

		/// <summary>
		/// Sets the content view.
		/// </summary>
		/// <param name="layoutResID">Layout resource id.</param>
		public void SetContentView(int layoutResID) 
		{
			LayoutInflater inflator = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
			ContentView= inflator.Inflate(layoutResID, null);
		}

		public void Dismiss() 
		{
			window.Dismiss();
		}
	}
}
