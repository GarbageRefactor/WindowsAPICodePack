//Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma once
#include "D3D11Resource.h"

namespace  Microsoft { namespace WindowsAPICodePack { namespace DirectX { namespace Direct3D11 {

using namespace System;

    /// <summary>
    /// A 2D texture interface manages texel data, which is structured memory.
    /// <para>(Also see DirectX SDK: ID3D11Texture2D)</para>
    /// </summary>
    public ref class Texture2D :
        public Microsoft::WindowsAPICodePack::DirectX::Direct3D11::D3DResource
    {
    public: 
        /// <summary>
        /// Get the properties of the texture resource.
        /// <para>(Also see DirectX SDK: ID3D11Texture2D::GetDesc)</para>
        /// </summary>
        property Texture2dDescription^ Description
        {
            Texture2dDescription^ get();
        }




    protected:
        Texture2D()
        {
        }
    internal:

        Texture2D(ID3D11Texture2D* pNativeID3D11Texture2D)
        {
            nativeUnknown.Set(pNativeID3D11Texture2D);
        }

        property ID3D11Texture2D* NativeID3D11Texture2D
        {
            ID3D11Texture2D* get()
            {
                return reinterpret_cast<ID3D11Texture2D*>(nativeUnknown.Get());
            }
        }
    };
} } } }
