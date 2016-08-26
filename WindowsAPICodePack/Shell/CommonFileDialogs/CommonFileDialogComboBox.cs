﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Markup;

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Common class for all ComboBox controls on Common File Dialog
    /// </summary>
    [ContentProperty("Items")]
    public class CommonFileDialogComboBox : CommonFileDialogProminentControl, ICommonFileDialogIndexedControls
    {
        private readonly Collection<CommonFileDialogComboBoxItem> items = new Collection<CommonFileDialogComboBoxItem>();
        /// <summary>
        /// Gets the collection of CommonFileDialogBoxBoxItem objects
        /// </summary>
        public Collection<CommonFileDialogComboBoxItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Creates a new instance of this class
        /// </summary>
        public CommonFileDialogComboBox()
        {
        }

        /// <summary>
        /// Creates a new instance of this class with the specified name
        /// </summary>
        /// <param name="name">Text to display for this control</param>
        public CommonFileDialogComboBox(string name): base (name, String.Empty)
        {
        }

        #region ICommonFileDialogIndexedControls Members

        private int selectedIndex = -1;
        /// <summary>
        /// Gets the current index of the selected item
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                // Don't update property if it hasn't changed
                if (selectedIndex == value)
                    return;

                if (HostingDialog == null)
                {
                    selectedIndex = value;
                    return;
                }

                // Only update this property if it has a valid value
                if (value >= 0 && value < items.Count)
                {
                    selectedIndex = value;
                    ApplyPropertyChange("SelectedIndex");
                }
                else
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the CommonFileDialogComboBox.");
                }
            }
        }

        /// <summary>
        /// Raised when the SelectedIndex is changed.
        /// </summary>
        /// 
        /// <remarks>
        /// Initializing the SelectedIndexChanged event with an empty
        /// delegate allows us to skip the 
        /// "if (SelectedIndexChanged != null) { /* fire the event */ }"
        /// test.
        /// </remarks>
        public event EventHandler SelectedIndexChanged = delegate { };

        /// <summary>
        /// Raises the SelectedIndexChanged event if 'this' control is 
        /// enabled.
        /// </summary>
        /// <remarks>Because this method is defined in an interface, we can either
        /// have it as public, or make it private and explicitly implement (like below).
        /// Making it public doesn't really help as its only internal (but can't have this 
        /// internal because of the interface)
        /// </remarks>
        void ICommonFileDialogIndexedControls.RaiseSelectedIndexChangedEvent()
        {
            // Make sure that this control is enabled and has a specified delegate
            if (Enabled)
                SelectedIndexChanged(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Attach the ComboBox control to the dialog object
        /// </summary>
        /// <param name="dialog">The target dialog</param>
        internal override void Attach(IFileDialogCustomize dialog)
        {
            Debug.Assert(dialog != null, "CommonFileDialogComboBox.Attach: dialog parameter can not be null");
            
            // Add the combo box control
            dialog.AddComboBox(this.Id);

            // Add the combo box items
            for (int index = 0; index < items.Count; index++)
                dialog.AddControlItem(this.Id, index, items[index].Text);

            // Set the currently selected item
            if (selectedIndex >= 0 && selectedIndex < items.Count)
            {
                dialog.SetSelectedControlItem(this.Id, this.selectedIndex);
            }
            else if (selectedIndex != -1)
            {
                throw new IndexOutOfRangeException("Index was outside the bounds of the CommonFileDialogComboBox.");
            }

            // Make this control prominent if needed
            if (IsProminent)
                dialog.MakeProminent(this.Id);

            // Sync additional properties
            SyncUnmanagedProperties();
        }

    }

    /// <summary>
    /// Class that represents a ComboBoxItem for the Common File Dialog
    /// </summary>
    public class CommonFileDialogComboBoxItem
    {
        private string text = String.Empty;
        /// <summary>
        /// String that will be displayed for this item
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Creates a new instance of this class
        /// </summary>
        public CommonFileDialogComboBoxItem()
        {
        }

        /// <summary>
        /// Creates a new instance of this class with the specified text
        /// </summary>
        /// <param name="text"></param>
        public CommonFileDialogComboBoxItem(string text)
        {
            this.text = text;
        }
    }
}
