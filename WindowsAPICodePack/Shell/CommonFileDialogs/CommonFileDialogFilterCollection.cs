//Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack;

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Strongly typed collection for file dialog filters.
    /// </summary>
    public class CommonFileDialogFilterCollection : Collection<CommonFileDialogFilter>
    {
        internal ShellNativeMethods.COMDLG_FILTERSPEC[] GetAllFilterSpecs()
        {
            ShellNativeMethods.COMDLG_FILTERSPEC[] filterSpecs = 
                new ShellNativeMethods.COMDLG_FILTERSPEC[this.Count];

            for (int i = 0; i < this.Count; i++)
                filterSpecs[i] = this[i].GetFilterSpec();

            return filterSpecs;
        }
    }
}
