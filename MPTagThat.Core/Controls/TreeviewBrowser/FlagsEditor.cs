using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing;

namespace Raccoom.Windows.Forms.Design
{
	/// <summary>
	/// Implements a custom type editor for enum's with FlagAttribute
	/// </summary>
	/// <remarks>
	/// Copyright by Thierry Bouquain, <a href="http://www.codeproject.com/cs/miscctrl/flagseditor.asp?target=FlagsEditor" target="_blank">A flag editor article on codeproject.com</a>
	/// </remarks>
	public class FlagsEditor : UITypeEditor
	{
		/// <summary>
		/// Internal class used for storing custom data in listviewitems
		/// </summary>
		internal class clbItem
		{
			/// <summary>
			/// Creates a new instance of the <c>clbItem</c>
			/// </summary>
			/// <param name="str">The string to display in the <c>ToString</c> method. 
			/// It will contains the name of the flag</param>
			/// <param name="value">The integer value of the flag</param>
			/// <param name="tooltip">The tooltip to display in the <see cref="CheckedListBox"/></param>
			public clbItem(string str, int value, string tooltip)
			{
				this.str = str;
				this.value = value;
				this.tooltip = tooltip;
			}

			private string str;

			private int value;
			/// <summary>
			/// Gets the int value for this item
			/// </summary>
			public int Value
			{
				get{return value;}
			}

			private string tooltip;

			/// <summary>
			/// Gets the tooltip for this item
			/// </summary>
			public string Tooltip
			{
				get{return tooltip;}
			}

			/// <summary>
			/// Gets the name of this item
			/// </summary>
			/// <returns>The name passed in the constructor</returns>
			public override string ToString()
			{
				return str;
			}
		}

		private IWindowsFormsEditorService edSvc = null;
		private CheckedListBox clb;
		private ToolTip tooltipControl;

		/// <summary>
		/// Overrides the method used to provide basic behaviour for selecting editor.
		/// Shows our custom control for editing the value.
		/// </summary>
		/// <param name="context">The context of the editing control</param>
		/// <param name="provider">A valid service provider</param>
		/// <param name="value">The current value of the object to edit</param>
		/// <returns>The new value of the object</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			if (context != null
				&& context.Instance != null
				&& provider != null) 
			{

				edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

				if (edSvc != null) 
				{					
					// Create a CheckedListBox and populate it with all the enum values
					clb = new CheckedListBox();
					clb.BorderStyle = BorderStyle.FixedSingle;
					clb.CheckOnClick = true;
					clb.MouseDown += new MouseEventHandler(this.OnMouseDown);
					clb.MouseMove += new MouseEventHandler(this.OnMouseMoved);

					tooltipControl = new ToolTip();
					tooltipControl.ShowAlways = true;

					foreach(string name in Enum.GetNames(context.PropertyDescriptor.PropertyType))
					{
						// Get the enum value
						object enumVal = Enum.Parse(context.PropertyDescriptor.PropertyType, name);
						// Get the int value 
						int intVal = (int) Convert.ChangeType(enumVal, typeof(int));
						
						// Get the description attribute for this field
						System.Reflection.FieldInfo fi = context.PropertyDescriptor.PropertyType.GetField(name);
						DescriptionAttribute[] attrs = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

						// Store the the description
						string tooltip = attrs.Length > 0 ? attrs[0].Description : string.Empty;

						// Get the int value of the current enum value (the one being edited)
						int intEdited = (int) Convert.ChangeType(value, typeof(int));

						// Creates a clbItem that stores the name, the int value and the tooltip
						clbItem item = new clbItem(enumVal.ToString(), intVal, tooltip);

						// Get the checkstate from the value being edited
						//bool checkedItem = (intEdited & intVal) > 0;
						bool checkedItem = (intEdited & intVal) == intVal;

						// Add the item with the right check state
						clb.Items.Add(item, checkedItem);
					}					

					// Show our CheckedListbox as a DropDownControl. 
					// This methods returns only when the dropdowncontrol is closed
					edSvc.DropDownControl(clb);

					// Get the sum of all checked flags
					int result = 0;
					foreach(clbItem obj in clb.CheckedItems)
					{
						//result += obj.Value;
						result |= obj.Value;
					}
					
					// return the right enum value corresponding to the result
					return Enum.ToObject(context.PropertyDescriptor.PropertyType, result);
				}
			}

			return value;
		}

		/// <summary>
		/// Shows a dropdown icon in the property editor
		/// </summary>
		/// <param name="context">The context of the editing control</param>
		/// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return UITypeEditorEditStyle.DropDown;			
		}

		private bool handleLostfocus = false;

		/// <summary>
		/// When got the focus, handle the lost focus event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseDown(object sender, MouseEventArgs e) 
		{
			if(!handleLostfocus && clb.ClientRectangle.Contains(clb.PointToClient(new Point(e.X, e.Y))))
			{
				clb.LostFocus += new EventHandler(this.ValueChanged);
				handleLostfocus = true;
			}
		}

		/// <summary>
		/// Occurs when the mouse is moved over the checkedlistbox. 
		/// Sets the tooltip of the item under the pointer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseMoved(object sender, MouseEventArgs e) 
		{			
			int index = clb.IndexFromPoint(e.X, e.Y);
			if(index >= 0)
				tooltipControl.SetToolTip(clb, ((clbItem) clb.Items[index]).Tooltip);
		}

		/// <summary>
		/// Close the dropdowncontrol when the user has selected a value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ValueChanged(object sender, EventArgs e) 
		{
			if (edSvc != null) 
			{
				edSvc.CloseDropDown();
			}
		}
	}
}
