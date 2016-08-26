﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
// Disable warning if a method declaration hides another inherited from a parent COM interface
// To successfully import a COM interface, all inherited methods need to be declared again with 
// the exception of those already declared in "IUnknown"
#pragma warning disable 108

    #region Property System COM Interfaces
    [ComImport,
    Guid(ShellIIDGuid.IPropertyStore),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyStore
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCount([Out] out uint cProps);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAt([In] uint iProp, out PropertyKey pkey);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetValue([In] ref PropertyKey key, out PropVariant pv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetValue([In] ref PropertyKey key, [In] ref PropVariant pv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Commit();
    }

    [ComImport,
    Guid(ShellIIDGuid.IPropertyDescriptionList),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyDescriptionList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCount(out uint pcElem);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
    }

    [ComImport,
    Guid(ShellIIDGuid.IPropertyDescription),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyKey(out PropertyKey pkey);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyType(out VarEnum pvartype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEditInvitation([MarshalAs(UnmanagedType.LPWStr)] out string ppszInvite);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetTypeFlags([In] PropertySystemNativeMethods.PROPDESC_TYPE_FLAGS mask, out PropertySystemNativeMethods.PROPDESC_TYPE_FLAGS ppdtFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetViewFlags(out PropertySystemNativeMethods.PROPDESC_VIEW_FLAGS ppdvFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultColumnWidth(out uint pcxChars);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayType(out PropertyDisplayType pdisplaytype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetColumnState(out uint pcsFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetGroupingRange(out PropertySystemNativeMethods.PROPDESC_GROUPING_RANGE pgr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRelativeDescriptionType(out PropertySystemNativeMethods.PROPDESC_RELATIVEDESCRIPTION_TYPE prdt);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRelativeDescription([In] ref PropVariant propvar1, [In] ref  PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSortDescription(out PropertySystemNativeMethods.PROPDESC_SORTDESCRIPTION psd);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSortDescriptionLabel([In] int fDescending, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDescription);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAggregationType(out PropertyAggregationType paggtype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetConditionType(out PropertySystemNativeMethods.PROPDESC_CONDITION_TYPE pcontype, out PropertySystemNativeMethods.CONDITION_OPERATION popDefault);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEnumTypeList([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CoerceToCanonicalValue([In] ref PropVariant propvar, out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void FormatForDisplay([In] ref PropVariant propvar, [In] ref PropertyDescriptionFormat pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsValueCanonical([In] ref PropVariant propvar);
    }

    [ComImport,
    Guid(ShellIIDGuid.IPropertyDescription2),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyDescription2 : IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyKey(out PropertyKey pkey);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyType(out VarEnum pvartype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEditInvitation([MarshalAs(UnmanagedType.LPWStr)] out string ppszInvite);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetTypeFlags([In] PropertySystemNativeMethods.PROPDESC_TYPE_FLAGS mask, out PropertySystemNativeMethods.PROPDESC_TYPE_FLAGS ppdtFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetViewFlags(out PropertySystemNativeMethods.PROPDESC_VIEW_FLAGS ppdvFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultColumnWidth(out uint pcxChars);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayType(out PropertyDisplayType pdisplaytype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetColumnState(out uint pcsFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetGroupingRange(out PropertySystemNativeMethods.PROPDESC_GROUPING_RANGE pgr);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRelativeDescriptionType(out PropertySystemNativeMethods.PROPDESC_RELATIVEDESCRIPTION_TYPE prdt);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRelativeDescription([In] ref PropVariant propvar1, [In] ref  PropVariant propvar2, 
            [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1, 
            [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSortDescription(out PropertySystemNativeMethods.PROPDESC_SORTDESCRIPTION psd);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSortDescriptionLabel([In] int fDescending, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDescription);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAggregationType(out PropertyAggregationType paggtype);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetConditionType(
            out PropertySystemNativeMethods.PROPDESC_CONDITION_TYPE pcontype, 
            out PropertySystemNativeMethods.CONDITION_OPERATION popDefault);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEnumTypeList([In] ref Guid riid, out IntPtr ppv);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CoerceToCanonicalValue([In] ref PropVariant propvar, out PropVariant ppropvar);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void FormatForDisplay([In] ref PropVariant propvar, [In] ref PropertyDescriptionFormat pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsValueCanonical([In] ref PropVariant propvar);
        
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetImageReferenceForValue(        
            ref PropVariant propvar,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
    }

    [ComImport,
    Guid(ShellIIDGuid.IPropertyEnumType),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyEnumType
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEnumType([Out] out PropEnumType penumtype);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetValue([Out] out PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRangeMinValue([Out] out PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRangeSetValue([Out] out PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayText([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
    }

    [ComImport,
    Guid(ShellIIDGuid.IPropertyEnumType2),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyEnumType2 : IPropertyEnumType
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEnumType([Out] out PropEnumType penumtype);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetValue([Out] out PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRangeMinValue([Out] out PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRangeSetValue([Out] out PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayText([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetImageReference([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
    }


    [ComImport,
    Guid(ShellIIDGuid.IPropertyEnumTypeList),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyEnumTypeList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCount([Out] out uint pctypes);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAt(
        [In] uint itype,
        [In] ref Guid riid,   // riid may be IID_IPropertyEnumType
        [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetConditionAt(
        [In] uint index,
        [In] ref Guid riid,
        out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void FindMatchingIndex(
        [In] ref PropVariant propvarCmp,
        [Out] out uint pnIndex);
    }

    #endregion

#pragma warning restore 108

}
