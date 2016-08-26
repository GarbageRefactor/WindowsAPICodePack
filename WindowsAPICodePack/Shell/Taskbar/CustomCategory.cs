﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.WindowsAPICodePack.Shell.Taskbar
{
    /// <summary>
    /// Represents a custom category on the taskbar's jump list
    /// </summary>
    public class CustomCategory
    {
        private string name;
        /// <summary>
        /// Category name
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (String.Compare(name, value) != 0)
                {
                    name = value;
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        /// <summary>
        /// Collection of jump list items under this category
        /// </summary>
        public JumpListItemCollection<IJumpListItem> JumpListItems { get; set; }

        /// <summary>
        /// Event that is triggered when the jump list collection is modified
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        /// <summary>
        /// Creates a new custom category instance
        /// </summary>
        /// <param name="CategoryName">Category name</param>
        public CustomCategory(string CategoryName)
        {
            Name = CategoryName;

            JumpListItems = new JumpListItemCollection<IJumpListItem>();
            JumpListItems.CollectionChanged += OnJumpListCollectionChanged;
        }

        internal void OnJumpListCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.CollectionChanged(this, args);
        }


        internal void RemoveJumpListItem(string path)
        {
            List<IJumpListItem> itemsToRemove = new List<IJumpListItem>();

            // Check for items to remove
            foreach (IJumpListItem item in JumpListItems)
            {
                if (string.Compare(path, item.Path, true) == 0)
                {
                    itemsToRemove.Add(item);
                }
            }

            // Remove matching items
            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                JumpListItems.Remove(itemsToRemove[i]);
            }
        }
    }
}
