﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
    class ShellFolderItems : IEnumerator<ShellObject>
    {
        #region Private Fields

        private IEnumIDList nativeEnumIdList = null;
        private ShellObject currentItem = null;
        IShellFolder nativeShellFolder;

        #endregion

        #region Private Methods

        private void CreateNativeEnumIdList()
        {
            nativeShellFolder.EnumObjects(
                IntPtr.Zero,
                ShellNativeMethods.SHCONT.SHCONTF_FOLDERS | ShellNativeMethods.SHCONT.SHCONTF_NONFOLDERS,
                out nativeEnumIdList);

            currentItem = null;
        }

        #endregion

        #region Internal Constructor

        internal ShellFolderItems(IShellFolder nativeShellFolder)
        {
            this.nativeShellFolder = nativeShellFolder;

            nativeShellFolder.EnumObjects(
                IntPtr.Zero,
                ShellNativeMethods.SHCONT.SHCONTF_FOLDERS | ShellNativeMethods.SHCONT.SHCONTF_NONFOLDERS,
                out nativeEnumIdList);
        }

        #endregion

        #region IEnumerator<ShellObject> Members

        public ShellObject Current
        {            
            get 
            { 
                return currentItem;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Marshal.FinalReleaseComObject(nativeEnumIdList);            
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get
            {
                return currentItem;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            IntPtr item;
            uint numItemsReturned;
            uint itemsRequested = 1;
            nativeEnumIdList.Next(itemsRequested, out item, out numItemsReturned);


            if (numItemsReturned < itemsRequested)
                return false;

            currentItem = ShellObject.FromIDList(item, nativeShellFolder);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            nativeEnumIdList.Reset();
        }


        #endregion
    }
}
