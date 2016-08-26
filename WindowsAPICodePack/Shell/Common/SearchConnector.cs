﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// A Serch Connector folder in the Shell Namespace
    /// </summary>
    public sealed class SearchConnector : SearchContainer
    {

        #region Private Constructor

        internal SearchConnector()
        {

        }

        internal SearchConnector(IShellItem2 shellItem)
        {
            nativeShellItem = shellItem;
        }

        #endregion
    }
}
