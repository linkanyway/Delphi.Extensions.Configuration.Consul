/*******************************************************************************
* File Name：  IConsulConfigurationParser.cs
* Namespace: 
*
* CreateTime: 2018/11/13 15:52
* Author：  linkanyway
* Description： IConsulConfigurationParser
* Class Name： IConsulConfigurationParser
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018/11/13 15:52 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System.Collections.Generic;

namespace Delphi.Extensions.Configuration.Consul.Parsers
{
    /// <summary>
    /// Parser interface
    /// </summary>
    public interface IConsulConfigurationParser
    {
        /// <summary>
        ///     Parse the <see cref="string" /> into a dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>the dictionary for the configuration.</returns>
        IDictionary<string, string> Parse(string key,string value);
    }
}