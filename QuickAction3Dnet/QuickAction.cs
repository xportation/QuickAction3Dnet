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
using Android.Graphics;

namespace QuickAction3Dnet
{
	public class ActionItemClickEventArgs : EventArgs
	{
		public ActionItem ActionItem { get; set; }
	}

	public class QuickAction : PopupWindows 
	{
		private ImageView arrowUp;
		private ImageView arrowDown;
		private LayoutInflater inflater;
		private ViewGroup track;
		private ScrollView scroller;
		
		private List<ActionItem> actionItems = new List<ActionItem>();
		
		private bool didAction;
		
		private int childPos;
		private int insertPos;
	   private int rootWidth=0;
	    
		public enum Orientation { Horizontal= 0, Vertical };
		private Orientation orientation;
	   
		public enum AnimationStyle { GrowFromLeft= 1, GrowFromRight, GrowFromCenter, Reflect, Auto }; 
		private AnimationStyle animationStyle;

		/// <summary>
		/// Constructor for default vertical layout
		/// </summary>
		/// <param name="context">Context.</param>
	   public QuickAction(Context context)
			:	this(context, Orientation.Vertical) 
		{
	   }

		/// <summary>
		/// Constructor allowing orientation override
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="orientation"> Layout orientation, can be vartical or horizontal.</param>
		public QuickAction(Context context, Orientation orientation) 
			:	base(context)
		{
			this.orientation = orientation;	        
			inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

			if (orientation == Orientation.Horizontal) {
				SetRootViewId(Resource.Layout.popup_horizontal);
			} else {
				SetRootViewId(Resource.Layout.popup_vertical);
			}

			animationStyle = AnimationStyle.Auto;
			childPos = 0;
	   }

		protected override void onDismissEvent(object sender, EventArgs e)
		{
			if (!didAction)
				base.onDismissEvent(sender, e);
		}

		public ActionItem GetActionItem(int index) 
		{
			return actionItems[index];
	   }
	    
		public void SetRootViewId(int id) 
		{
			rootView = inflater.Inflate(id, null);
			track = rootView.FindViewById<ViewGroup>(Resource.Id.tracks);

			arrowDown = rootView.FindViewById<ImageView>(Resource.Id.arrow_down);
			arrowUp = rootView.FindViewById<ImageView>(Resource.Id.arrow_up);

			scroller = rootView.FindViewById<ScrollView>(Resource.Id.scroller);

			rootView.LayoutParameters= new Android.Views.ViewGroup.LayoutParams(WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.WrapContent);
			
			ContentView= rootView;
		}

		/// <summary>
		/// Sets the animation style.
		/// </summary>
		/// <param name="animationStyle">The default is set to AnimationStyle.Auto.</param>
		public void SetAnimStyle(AnimationStyle animationStyle) 
		{
			this.animationStyle = animationStyle;
		}
		
		public event EventHandler<ActionItemClickEventArgs> ItemClickEvent;
		
		public void AddActionItem(ActionItem action) 
		{
			actionItems.Add(action);
			
			string title = action.Title;
			Drawable icon = action.Icon;
			
			View container;
			
			if (orientation == Orientation.Horizontal) {
				container = inflater.Inflate(Resource.Layout.action_item_horizontal, null);
	      } else {
				container = inflater.Inflate(Resource.Layout.action_item_vertical, null);
	      }
			
			ImageView img = container.FindViewById<ImageView>(Resource.Id.iv_icon);
			TextView text = container.FindViewById<TextView>(Resource.Id.tv_title);
			
			if (icon != null) {
				img.SetImageDrawable(icon);
			} else {
				img.Visibility = ViewStates.Gone;
			}
			
			if (title != null) {
				text.Text = title;
			} else {
				text.Visibility = ViewStates.Gone;
			}
			
			int pos = childPos;
			container.Click += (sender, e) => {
				EventHandler<ActionItemClickEventArgs> handler = ItemClickEvent;
				if (handler != null)
				{
					ActionItemClickEventArgs eventArgs= new ActionItemClickEventArgs();
					eventArgs.ActionItem = GetActionItem(pos);
					handler(this, eventArgs);
				}

				if (!this.GetActionItem(pos).Sticky) {  
					didAction = true;
					Dismiss();
				}
			};

			container.Focusable = true;
			container.Clickable = true;
				 
			if (orientation == Orientation.Horizontal && childPos != 0) {
				View separator = inflater.Inflate(Resource.Layout.horiz_separator, null);
	         
				RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(
					                                           WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.FillParent);
	         
				separator.LayoutParameters = layoutParams;
				separator.SetPadding(5, 0, 5, 0);
	         
	         track.AddView(separator, insertPos);
	         
	         insertPos++;
	      }
			
			track.AddView(container, insertPos);
			
			childPos++;
			insertPos++;
		}
		
		public void Show (View anchor) 
		{
			preShow();
			
			int xPos, yPos, arrowPos;
			didAction = false;
			
			int[] location = new int[2];		
			anchor.GetLocationOnScreen(location);

			Rect anchorRect = new Rect(location[0], location[1], location[0] + anchor.Width, location[1] + anchor.Height);			
			rootView.Measure(WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.WrapContent);
		
			int rootHeight = rootView.MeasuredHeight;
			
			if (rootWidth == 0) {
				rootWidth = rootView.MeasuredWidth;
			}
			
			int screenWidth = windowManager.DefaultDisplay.Width;
			int screenHeight = windowManager.DefaultDisplay.Height;
			
			//automatically get X coord of popup (top left)
			if ((anchorRect.Left + rootWidth) > screenWidth) {
				xPos = anchorRect.Left - (rootWidth-anchor.Width);
				xPos = (xPos < 0) ? 0 : xPos;
				
				arrowPos = anchorRect.CenterX()-xPos;				
			} else {
				if (anchor.Width > rootWidth) {
					xPos = anchorRect.CenterX() - (rootWidth/2);
				} else {
					xPos = anchorRect.Left;
				}				
				arrowPos = anchorRect.CenterX()-xPos;
			}
			
			int dyTop = anchorRect.Top;
			int dyBottom = screenHeight - anchorRect.Bottom;

			bool onTop = (dyTop > dyBottom) ? true : false;

			if (onTop) {
				if (rootHeight > dyTop) {
					yPos = 15;
					ViewGroup.LayoutParams l = scroller.LayoutParameters;
					l.Height = dyTop - anchor.Height;
				} else {
					yPos = anchorRect.Top - rootHeight;
				}
			} else {
				yPos = anchorRect.Bottom;
				
				if (rootHeight > dyBottom) { 
					ViewGroup.LayoutParams l = scroller.LayoutParameters;
					l.Height = dyBottom;
				}
			}
			
			ShowArrow(((onTop) ? Resource.Id.arrow_down : Resource.Id.arrow_up), arrowPos);			
			SetAnimationStyle(screenWidth, anchorRect.CenterX(), onTop);			
			window.ShowAtLocation(anchor, GravityFlags.NoGravity, xPos, yPos);
		}

		/// <summary>
		/// Sets the animation style.
		/// </summary>
		/// <param name="screenWidth">Screen width.</param>
		/// <param name="requestedX">distance from left edge</param>
		/// <param name="onTop">flag to indicate where the popup should be displayed. Set TRUE if displayed on top of anchor view and vice versa</param>
		private void SetAnimationStyle(int screenWidth, int requestedX, bool onTop) {
			int arrowPos = requestedX - arrowUp.MeasuredWidth/2;

			switch (animationStyle) {
			case AnimationStyle.GrowFromLeft:
				window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Left : Resource.Style.Animations_PopDownMenu_Left;
				break;
						
			case AnimationStyle.GrowFromRight:
				window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Right : Resource.Style.Animations_PopDownMenu_Right;
				break;
						
			case AnimationStyle.GrowFromCenter:
				window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Center : Resource.Style.Animations_PopDownMenu_Center;
				break;
				
			case AnimationStyle.Reflect:
				window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Reflect : Resource.Style.Animations_PopDownMenu_Reflect;
				break;
			
			case AnimationStyle.Auto:
				if (arrowPos <= screenWidth/4) {
					window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Left : Resource.Style.Animations_PopDownMenu_Left;
				} else if (arrowPos > screenWidth/4 && arrowPos < 3 * (screenWidth/4)) {
					window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Center : Resource.Style.Animations_PopDownMenu_Center;
				} else {
					window.AnimationStyle = (onTop) ? Resource.Style.Animations_PopUpMenu_Right : Resource.Style.Animations_PopDownMenu_Right;
				}
						
				break;
			}
		}

		private void ShowArrow(int whichArrow, int requestedX) 
		{
			View showArrow = (whichArrow == Resource.Id.arrow_up) ? arrowUp : arrowDown;
			View hideArrow = (whichArrow == Resource.Id.arrow_up) ? arrowDown : arrowUp;

			int arrowWidth = arrowUp.MeasuredWidth;
			showArrow.Visibility = ViewStates.Visible;
			ViewGroup.MarginLayoutParams param = (ViewGroup.MarginLayoutParams)showArrow.LayoutParameters;
			param.LeftMargin = requestedX - arrowWidth / 2;
			hideArrow.Visibility = ViewStates.Invisible;
	   }
	}
}
