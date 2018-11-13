/*******************************************************************************
* File Name：  ConsulJsonParser.cs
* Namespace: 
*
* CreateTime: 2018/11/13 16:48
* Author：  linkanyway
* Description： ConsulJsonParser
* Class Name： ConsulJsonParser
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018/11/13 16:48 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System;
using System.Collections.Generic;

namespace Delphi.Extensions.Configuration.Consul.Parsers
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class ConsulJsonParser : IConsulConfigurationParser
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public IDictionary<string, string> Parse(string key, string value)
        {
            var json = "{\"" + key + "\":" + value + "}";
            var dictionary = json.ToDictionary();

            return dictionary;
        }
    }
}