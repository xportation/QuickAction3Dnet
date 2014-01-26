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
	public class ActionItem {
		private Drawable icon;
		private Bitmap thumb;
		private string title;
		private int actionId = -1;
		private bool selected;
		private bool sticky;

		/// <summary>
		/// Initializes a new instance of the <see cref="QuickAction3Dnet.ActionItem"/> class.
		/// </summary>
		/// <param name="actionId">Action identifier for case statements.</param>
		/// <param name="title">Title.</param>
		/// <param name="icon">Icon to use.</param>
		public ActionItem(int actionId, String title, Drawable icon) {
			this.title = title;
			this.icon = icon;
			this.actionId = actionId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QuickAction3Dnet.ActionItem"/> class.
		/// </summary>
		public ActionItem() 
			: 	this(-1, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QuickAction3Dnet.ActionItem"/> class.
		/// </summary>
		/// <param name="actionId">Action identifier of the item.</param>
		/// <param name="title">Text to show for the item.</param>
		public ActionItem(int actionId, String title)
			:	this(actionId, title, null)
		{
		}

		/*		*
     * Constructor
     * 
     * @param icon {@link Drawable} action icon
     */
		/// <summary>
		/// Initializes a new instance of the <see cref="QuickAction3Dnet.ActionItem"/> class.
		/// </summary>
		/// <param name="icon">Action Icon.<see cref="Android.Drawable"/></param>
		public ActionItem(Drawable icon)
			:	this(-1, null, icon)
		{
		}

		/*		*
     * Constructor
     * 
     * @param actionId  Action ID of item
     * @param icon      {@link Drawable} action icon
     */
		/// <summary>
		/// Initializes a new instance of the <see cref="QuickAction3Dnet.ActionItem"/> class.
		/// </summary>
		/// <param name="actionId">Action identifier of item.</param>
		/// <param name="icon">Action Icon. <see cref="Android.Drawable"/></param>
		public ActionItem(int actionId, Drawable icon)
			:	this(actionId, null, icon)
		{
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title {
			get {
				return title;
			}
			set {
				title = value;
			}
		}

		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// <value>The icon.</value>
		public Drawable Icon {
			get {
				return icon;
			}
			set {
				icon = value;
			}
		}

		/// <summary>
		/// Gets or sets the action identifier.
		/// </summary>
		/// <value>The action identifier.</value>
		public int ActionId {
			get {
				return actionId;
			}
			set {
				actionId = value;
			}
		}

		/// <summary>
		/// Gets or sets sticky status of button.
		/// </summary>
		/// <value><c>true</c> for sticky, pop up sends event but does not disappear.</value>
		public bool Sticky {
			get {
				return sticky;
			}
			set {
				sticky = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="QuickAction3Dnet.ActionItem"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public bool Selected {
			get {
				return selected;
			}
			set {
				selected = value;
			}
		}

		/// <summary>
		/// Gets or sets the thumb.
		/// </summary>
		/// <value>The thumb.</value>
		public Bitmap Thumb {
			get {
				return thumb;
			}
			set {
				thumb = value;
			}
		}
	}
}

