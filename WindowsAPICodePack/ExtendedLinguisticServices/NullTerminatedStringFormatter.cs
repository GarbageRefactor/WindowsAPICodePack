// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.ExtendedLinguisticServices
{

    /// <summary>
    /// Converts byte arrays containing Unicode null-terminated strings into .NET string objects.
    /// </summary>
    public class NullTerminatedStringFormatter : IMappingFormatter<string>
    {
        /// <summary>
        /// Converts a single MappingDataRange into a string, stripping the trailing null character.
        /// If the string doesn't contain null characters, the empty string is returned.
        /// </summary>
        /// <param name="dataRange">The MappingDataRange to convert</param>
        /// <returns>The resulting string</returns>
        public string Format(MappingDataRange dataRange)
        {
            byte[] data = dataRange.Data;
            if ((data.Length & 1) != 0)
            {
                throw new LinguisticException(LinguisticException.E_INVALIDARG);
            }
            int nullIndex = data.Length;
            for (int i = 0; i < data.Length; i += 2)
            {
                if (data[i] == 0 && data[i + 1] == 0)
                {
                    nullIndex = i;
                    break;
                }
            }
            string resultText = Encoding.Unicode.GetString(data, 0, nullIndex);
            return resultText;
        }

        /// <summary>
        /// Uses string Format(MappingDataRange dataRange) to format all the ranges of the supplied
        /// MappingPropertyBag.
        /// </summary>
        /// <param name="bag">The property bag to convert.</param>
        /// <returns>An array of strings, one per MappingDataRange.</returns>
        public string[] FormatAll(MappingPropertyBag bag)
        {
            MappingDataRange[] dataRanges = bag.ResultRanges;
            string[] results = new string[dataRanges.Length];
            for (int i = 0; i < results.Length; ++i)
            {
                results[i] = Format(dataRanges[i]);
            }
            return results;
        }
    }

}
