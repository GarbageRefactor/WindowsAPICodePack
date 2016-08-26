//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// A Shell Library in the Shell Namespace
    /// </summary>
    public sealed class ShellLibrary : ShellObject, IShellContainer<ShellFolder>
    {
        #region Private Fields

        private INativeShellLibrary nativeShellLibrary = null;

        private static Guid[] FolderTypesGuids = 
        {
            new Guid(ShellKFIDGuid.GenericLibrary),
            new Guid(ShellKFIDGuid.DocumentsLibrary),
            new Guid(ShellKFIDGuid.MusicLibrary),
            new Guid(ShellKFIDGuid.PicturesLibrary),
            new Guid(ShellKFIDGuid.VideosLibrary)
        };

        #endregion

        #region Private Constructor

        //Construct the ShellLibrary object from a native Shell Library
        private ShellLibrary(INativeShellLibrary nativeShellLibrary)
        {
            ShellHelper.ThrowIfNotWin7();
            this.nativeShellLibrary = nativeShellLibrary;
        }

        #endregion

        #region Public Constructors

        /// <summary>
        /// Creates a shell library in the Libraries Known Folder, 
        /// using the given shell library name.
        /// </summary>
        /// <param name="libraryName">The name of this library</param>
        /// <param name="overwrite">Override an existing library with the same name</param>
        public ShellLibrary(string libraryName, bool overwrite)
        {
            ShellHelper.ThrowIfNotWin7();

            this.Name = libraryName;
            Guid guid = new Guid(ShellKFIDGuid.Libraries);

            ShellNativeMethods.LIBRARYSAVEFLAGS flags = 
                overwrite ? 
                    ShellNativeMethods.LIBRARYSAVEFLAGS.LSF_OVERRIDEEXISTING : 
                    ShellNativeMethods.LIBRARYSAVEFLAGS.LSF_FAILIFTHERE;

            nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
            nativeShellLibrary.SaveInKnownFolder(ref guid, libraryName, flags, out nativeShellItem);
        }

        /// <summary>
        /// Creates a shell library in a given Known Folder, 
        /// using the given shell library name.
        /// </summary>
        /// <param name="libraryName">The name of this library</param>
        /// <param name="knownFolder">The known folder</param>
        /// <param name="overwrite">Override an existing library with the same name</param>
        public ShellLibrary(string libraryName, KnownFolder knownFolder, bool overwrite)
        {
            ShellHelper.ThrowIfNotWin7();

            this.Name = libraryName;
            Guid guid = knownFolder.FolderTypeId;

            ShellNativeMethods.LIBRARYSAVEFLAGS flags =
                overwrite ?
                    ShellNativeMethods.LIBRARYSAVEFLAGS.LSF_OVERRIDEEXISTING :
                    ShellNativeMethods.LIBRARYSAVEFLAGS.LSF_FAILIFTHERE;

            nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
            nativeShellLibrary.SaveInKnownFolder(ref guid, libraryName, flags, out nativeShellItem);
        }

        /// <summary>
        /// Creates a shell library in a given local folder, 
        /// using the given shell library name.
        /// </summary>
        /// <param name="libraryName">The name of this library</param>
        /// <param name="folderPath">The path to the local folder</param>
        /// <param name="overwrite">Override an existing library with the same name</param>
        public ShellLibrary(string libraryName, string folderPath, bool overwrite)
        {
            ShellHelper.ThrowIfNotWin7();

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException("Folder path not found.");
            }

            this.Name = libraryName;

            ShellNativeMethods.LIBRARYSAVEFLAGS flags =
                overwrite ?
                    ShellNativeMethods.LIBRARYSAVEFLAGS.LSF_OVERRIDEEXISTING :
                    ShellNativeMethods.LIBRARYSAVEFLAGS.LSF_FAILIFTHERE;

            Guid guid = new Guid(ShellIIDGuid.IShellItem);

            string libraryPath = System.IO.Path.Combine(folderPath, libraryName + FileExtension);

            IShellItem shellItemIn = null;
            ShellNativeMethods.SHCreateItemFromParsingName(libraryPath, IntPtr.Zero, ref guid, out shellItemIn);

            nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
            nativeShellLibrary.Save(shellItemIn, libraryName, flags, out nativeShellItem);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the library, every library must 
        /// have a name
        /// </summary>
        /// <exception cref="COMException">Will throw if no Icon is set</exception>
        public override string Name
        {
            get
            {
                if (name == null && NativeShellItem != null)
                {
                    name = System.IO.Path.GetFileNameWithoutExtension(ShellHelper.GetParsingName(NativeShellItem));
                }
                return name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IconRefernece IconResourceId
        {
            get
            {
                string iconRef;
                nativeShellLibrary.GetIcon(out iconRef);
                return new IconRefernece(iconRef);
            }

            set
            {
                nativeShellLibrary.SetIcon(value.RefPath);
                nativeShellLibrary.Commit();
            }
        }

        /// <summary>
        /// Get the BitmapSource associated with this Library's Icon
        /// </summary>
        /// <exception cref="COMException">Will throw if no Icon is set</exception>
        public BitmapSource Icon
        {
            get
            {
                return CoreHelpers.GetIcon(this.IconResourceId.RefPath);
            }
    
        }

        /// <summary>
        /// One of predefined Library types
        /// </summary>
        /// <exception cref="COMException">Will throw if no Library Type is set</exception>
        public LibraryFolderType LibraryType 
        {
            get
            {
                Guid folderTypeGuid;
                nativeShellLibrary.GetFolderType(out folderTypeGuid);

                return GetFolderTypefromGuid(folderTypeGuid);
            }

            set
            {
                Guid guid = FolderTypesGuids[(int)value];
                nativeShellLibrary.SetFolderType(ref guid);
                nativeShellLibrary.Commit();
            }
        }

        /// <summary>
        /// The Guid of the Library type
        /// </summary>
        /// <exception cref="COMException">Will throw if no Library Type is set</exception>
        public Guid LibraryTypeId
        {
            get
            {
                Guid folderTypeGuid;
                nativeShellLibrary.GetFolderType(out folderTypeGuid);

                return folderTypeGuid;
            }
        }

        private static LibraryFolderType GetFolderTypefromGuid(Guid folderTypeGuid)
        {
            for (int i = 0; i < FolderTypesGuids.Length; i++)
            {
                if (folderTypeGuid.Equals(FolderTypesGuids[i]))
                    return (LibraryFolderType)i;
            }
            throw new ArgumentOutOfRangeException("Invalid FoldeType Guid");
        }
        
        /// <summary>
        /// By default, this folder is the first location 
        /// added to the library. The default save folder 
        /// is both the default folder where files can 
        /// be saved, and also where the library XML 
        /// file will be saved, if no other path is specified
        /// </summary>
        public string DefaultSaveFolder 
        {
            get
            {
                Guid guid = new Guid(ShellIIDGuid.IShellItem);
                
                IShellItem saveFolderItem;
                
                nativeShellLibrary.GetDefaultSaveFolder(
                    ShellNativeMethods.DEFAULTSAVEFOLDERTYPE.DSFT_DETECT,
                    ref guid,
                    out saveFolderItem);

                return ShellHelper.GetParsingName(saveFolderItem);
            }
            
            set 
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("DefaultSaveFolder");
                }

                if (!Directory.Exists(value))
                    throw new DirectoryNotFoundException("DefaultSaveFolder Path not found.");

                string fullPath = new DirectoryInfo(value).FullName;

                Guid guid = new Guid(ShellIIDGuid.IShellItem);
                IShellItem saveFolderItem;

                ShellNativeMethods.SHCreateItemFromParsingName(fullPath, IntPtr.Zero, ref guid, out saveFolderItem);

                nativeShellLibrary.SetDefaultSaveFolder(
                    ShellNativeMethods.DEFAULTSAVEFOLDERTYPE.DSFT_DETECT,
                    saveFolderItem);

                nativeShellLibrary.Commit();
            }
        }

        /// <summary>
        /// Whether the library will be pinned to the 
        /// Explorer Navigation Pane
        /// </summary>
        public bool IsPinnedToNavigationPane 
        {
            get
            {
                ShellNativeMethods.LIBRARYOPTIONFLAGS flags = 
                    ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_PINNEDTONAVPANE;

                nativeShellLibrary.GetOptions(out flags);
                
                return (
                    (flags & ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_PINNEDTONAVPANE) == 
                    ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_PINNEDTONAVPANE);
            }

            set
            {
                ShellNativeMethods.LIBRARYOPTIONFLAGS flags =
                    ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_DEFAULT;

                if (value)
                    flags |= ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_PINNEDTONAVPANE;
                else
                    flags &= ~ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_PINNEDTONAVPANE;

                nativeShellLibrary.SetOptions(ShellNativeMethods.LIBRARYOPTIONFLAGS.LOF_PINNEDTONAVPANE, flags);
                nativeShellLibrary.Commit();

            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the library management dialog which enables users to mange the library folders and default save location.
        /// </summary>
        /// <param name="windowHandle">The parent window,or IntPtr.Zero for no parent</param>
        /// <param name="title">A title for the library management dialog, or null to use the library name as the title</param>
        /// <param name="instruction">An optional help string to display for the library management dialog</param>
        /// <param name="allowAllLocations">If true, do not show warning dialogs about locations that cannot be indexed</param>
        /// <returns>true if the user cliked O.K, false if the user clicked Cancel</returns>
        public bool ShowManageLibraryUI(IntPtr windowHandle, string title, string instruction, bool allowAllLocations)
        {
            // this method is not safe for MTA consumption and will blow
            // Access Violations if called from an MTA thread so we wrap this
            // call up into a Worker thread that performs all operations in a
            // single threaded apartment

            int hr = 0;

            Thread staWorker = new Thread(() =>
            {
                 hr = ShellNativeMethods.SHShowManageLibraryUI(
                     NativeShellItem, 
                     windowHandle, 
                     title, 
                     instruction, 
                     allowAllLocations ? 
                        ShellNativeMethods.LIBRARYMANAGEDIALOGOPTIONS.LMD_NOUNINDEXABLELOCATIONWARNING : 
                        ShellNativeMethods.LIBRARYMANAGEDIALOGOPTIONS.LMD_DEFAULT);
            });

            staWorker.SetApartmentState(ApartmentState.STA);
            staWorker.Start();
            staWorker.Join();

            if (!CoreErrorHelper.Succeeded(hr))
                Marshal.ThrowExceptionForHR(hr);

            return true; //At this point we're sure the function has succeeded 
        }

        /// <summary>
        /// Shows the library management dialog which enables users to mange the library folders and default save location.
        /// </summary>
        /// <param name="mainWindow">The parent window for a WPF app</param>
        /// <param name="title">A title for the library management dialog, or null to use the library name as the title</param>
        /// <param name="instruction">An optional help string to display for the library management dialog</param>
        /// <param name="allowAllLocations">If true, do not show warning dialogs about locations that cannot be indexed</param>
        /// <returns>true if the user cliked O.K, false if the user clicked Cancel</returns>
        public bool ShowManageLibraryUI(Window mainWindow, string title, string instruction, bool allowAllLocations)
        {
            WindowInteropHelper helper = new WindowInteropHelper(mainWindow);
            return ShowManageLibraryUI(helper.Handle, title, instruction, allowAllLocations);
        }

        /// <summary>
        /// Shows the library management dialog which enables users to mange the library folders and default save location.
        /// </summary>
        /// <param name="form">A windows form (WinForm)</param>
        /// <param name="title">A title for the library management dialog, or null to use the library name as the title</param>
        /// <param name="instruction">An optional help string to display for the library management dialog</param>
        /// <param name="allowAllLocations">If true, do not show warning dialogs about locations that cannot be indexed</param>
        /// <returns>true if the user cliked O.K, false if the user clicked Cancel</returns>
        public bool ShowManageLibraryUI(Form form, string title, string instruction, bool allowAllLocations)
        {
            return ShowManageLibraryUI(form.Handle, title, instruction, allowAllLocations);
        }

        /// <summary>
        /// Close the library, and release its associated file system resources
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        #endregion

        #region Internal Properties

        internal const string FileExtension = ".library-ms";

        internal override IShellItem NativeShellItem
        {
            get
            {
                return NativeShellItem2;
            }

        }

        internal override IShellItem2 NativeShellItem2
        {
            get
            {
                return nativeShellItem;
            }
        }

        #endregion

        #region Static Shell Library methods

        /// <summary>
        /// Creates a copy of a ShellLibrary, 
        /// using a new name and path
        /// </summary>
        /// <param name="library"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="overrideExisting"></param>
        /// <returns></returns>
        public static ShellLibrary Copy(
            ShellLibrary library, 
            string name, string path,
            bool overrideExisting) { throw new NotImplementedException(); }
        
        /// <summary>
        /// Creates a copy of a ShellLibrary, 
        /// using a new name and known folder
        /// </summary>
        /// <param name="library"></param>
        /// <param name="name"></param>
        /// <param name="kf"></param>
        /// <param name="overrideExisting"></param>
        /// <returns></returns>
        public static ShellLibrary Copy(
            ShellLibrary library, 
            string name, 
            KnownFolder kf,
            bool overrideExisting) { throw new NotImplementedException(); }

        /// <summary>
        /// Get a the known folder FOLDERID_Libraries 
        /// </summary>
        public static KnownFolder LibrariesKnownFolder
        {
            get
            {
                return KnownFolderManager.GetKnownFolder(new Guid(ShellKFIDGuid.Libraries));
            }
        }

        /// <summary>
        /// Resolve a renamed/moved folder in a library
        /// </summary>
        /// <param name="path"></param>
        /// <param name="timeOutMillis"></param>
        /// <returns></returns>
        public static ShellFolder ResolveFolder(
            string path, int timeOutMillis)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolve a renamed/moved folder in a library
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="timeOutMillis"></param>
        /// <returns></returns>
        public static ShellFolder ResolveFolder(
            ShellFolder folder, int timeOutMillis)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load the library using a number of options
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="isReadOnly"></param>
        /// <returns>A ShellLibrary Object</returns>
        public static ShellLibrary Load(
            string libraryName, 
            bool isReadOnly) 
        {
            Guid guid = new Guid(ShellKFIDGuid.Libraries);
            string librariesFolderPath = KnownFolderManager.GetPath(guid);

            // Create the shell item path
            string shellItemPath = System.IO.Path.Combine(librariesFolderPath, libraryName + FileExtension);

            ShellFileSystemObject item = ShellFileSystemObject.FromPath(shellItemPath);
            
            IShellItem nativeShellItem = item.NativeShellItem;
            INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
            ShellNativeMethods.STGM flags = 
                isReadOnly ? 
                    ShellNativeMethods.STGM.Read : 
                    ShellNativeMethods.STGM.ReadWrite;
            nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, flags);

            ShellLibrary library = new ShellLibrary(nativeShellLibrary);
            library.nativeShellItem = (IShellItem2)nativeShellItem;
            library.Name = libraryName;

            return library;
        }

        /// <summary>
        /// Load the library using a number of options
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="folderPath"></param>
        /// <param name="isReadOnly"></param>
        /// <returns>A ShellLibrary Object</returns>
        public static ShellLibrary Load(
            string libraryName,
            string folderPath,
            bool isReadOnly)
        {

            // Create the shell item path
            string shellItemPath = System.IO.Path.Combine(folderPath, libraryName + FileExtension);
            ShellFileSystemObject item = ShellFileSystemObject.FromPath(shellItemPath);

            IShellItem nativeShellItem = item.NativeShellItem;
            INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
            ShellNativeMethods.STGM flags =
                isReadOnly ?
                    ShellNativeMethods.STGM.Read :
                    ShellNativeMethods.STGM.ReadWrite;
            nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, flags);

            return new ShellLibrary(nativeShellLibrary);
        }

        /// <summary>
        /// Load the library using a number of options
        /// </summary>
        /// <param name="nativeShellItem"></param>
        /// <returns>A ShellLibrary Object</returns>
        new internal static ShellLibrary FromShellItem(
            IShellItem nativeShellItem)
        {
            return ShellLibrary.FromShellItem(nativeShellItem, false);
        }

        /// <summary>
        /// Load the library using a number of options
        /// </summary>
        /// <param name="nativeShellItem"></param>
        /// <param name="isReadOnly"></param>
        /// <returns>A ShellLibrary Object</returns>
        internal static ShellLibrary FromShellItem(
            IShellItem nativeShellItem,
            bool isReadOnly)
        {
            INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
            
            ShellNativeMethods.STGM flags =
                isReadOnly ?
                    ShellNativeMethods.STGM.Read :
                    ShellNativeMethods.STGM.ReadWrite;

            nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, flags);

            ShellLibrary library = new ShellLibrary(nativeShellLibrary);
            library.nativeShellItem = (IShellItem2)nativeShellItem;

            return library;
        }        

        /// <summary>
        /// Load the library using a number of options
        /// </summary>
        /// <param name="kf"></param>
        /// <param name="isReadOnly"></param>
        /// <returns>A ShellLibrary Object</returns>
        public static ShellLibrary Load(
            KnownFolder kf, 
            bool isReadOnly) 
        {
            INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();

            ShellNativeMethods.STGM flags =
                isReadOnly ?
                    ShellNativeMethods.STGM.Read :
                    ShellNativeMethods.STGM.ReadWrite;

            IShellItem2 nativeShellItem = kf.NativeShellItem2;

            Guid guid = kf.FolderTypeId;

            nativeShellLibrary.LoadLibraryFromKnownFolder(ref guid, flags);

            ShellLibrary library = new ShellLibrary(nativeShellLibrary);
            library.nativeShellItem = (IShellItem2)nativeShellItem;

            return library;
        
        }

        #endregion

        #region Collection Members

        /// <summary>
        /// Add a new ShellFolder or SearchConnector
        /// </summary>
        /// <param name="item"></param>
        public void Add(ShellFolder item)
        {
            nativeShellLibrary.AddFolder(item.NativeShellItem);
            nativeShellLibrary.Commit();
        }

        /// <summary>
        /// Add an existing folder to this library
        /// </summary>
        /// <param name="folderPath"></param>
        public void Add(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException("Folder path not found.");
            }

            Add(ShellFolder.FromFolderPath(folderPath));
        }

        /// <summary>
        /// Clear all items of this Library 
        /// </summary>
        public void Clear()
        {
            List<ShellFolder> list = ItemsList;
            foreach (ShellFolder folder in list)
            {
                nativeShellLibrary.RemoveFolder(folder.NativeShellItem);
            }
            
            nativeShellLibrary.Commit();
        }

        /// <summary>
        /// Remove a folder or search connector
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(ShellFolder item)
        {
            try
            {
                nativeShellLibrary.RemoveFolder(item.NativeShellItem);
                nativeShellLibrary.Commit();
            }
            catch (COMException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove a folder or search connector
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public bool Remove(string folderPath)
        {
                
            ShellFolder item = (ShellFolder)ShellFolder.FromFolderPath(folderPath);
            return Remove(item);
        }        
        
        #endregion

        #region Disposable Pattern

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (nativeShellLibrary != null)
            {
                Marshal.FinalReleaseComObject(nativeShellLibrary);
                nativeShellLibrary = null;
            }

            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Release resources
        /// </summary>
        ~ShellLibrary()
        {
            Dispose(false);
        }

        #endregion

        #region Private Properties

        private List<ShellFolder> ItemsList
        {
            get
            {
                return GetFolders();
            }

        }
        private List<ShellFolder> GetFolders()
        {
            List<ShellFolder> list = new List<ShellFolder>();
            IShellItemArray itemArray;

            Guid shellItemArrayGuid = new Guid(ShellIIDGuid.IShellItemArray);
            try
            {
                nativeShellLibrary.GetFolders(ShellNativeMethods.LIBRARYFOLDERFILTER.LFF_ALLITEMS, ref shellItemArrayGuid, out itemArray);
            }
            catch
            {
                return list;
            }

            uint count;
            itemArray.GetCount(out count);

            //currentList = new List<ShellFolder>((int)count);

            for (uint i = 0; i < count; ++i)
            {
                IShellItem shellItem;
                itemArray.GetItemAt(i, out shellItem);
                list.Add((ShellFolder)ShellFolder.FromShellItem(shellItem));
            }

            if (itemArray != null)
            {
                Marshal.FinalReleaseComObject(itemArray);
                itemArray = null;
            }

            return list;
        }

        #endregion

        #region IEnumerable<ShellFolder> Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ShellFolder> GetEnumerator()
        {
            return ItemsList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ItemsList.GetEnumerator();
        }

        #endregion

        #region ICollection<ShellFolder> Members


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public bool Contains(string fullPath)
        {
            if (String.IsNullOrEmpty(fullPath))
            {
                throw new ArgumentNullException("fullPath");
            }

            List<ShellFolder> list = ItemsList;

            foreach (ShellFolder folder in list)
            {
                if (fullPath.Equals(folder.Path, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ShellFolder item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            List<ShellFolder> list = ItemsList;

            foreach (ShellFolder folder in list)
            {
                if (item.Path.Equals(folder.Path, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        #endregion

    }

}
