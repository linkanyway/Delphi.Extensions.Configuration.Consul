/*******************************************************************************
* File Name：  ConsulConfigValueType.cs
* Namespace: 
*
* CreateTime: 2018/11/13 16:00
* Author：  linkanyway
* Description： ConsulConfigValueType
* Class Name： ConsulConfigValueType
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018/11/13 16:00 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System;

namespace Delphi.Extensions.Configuration.Consul.Parsers
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ConsulConfigValueType
    {
        /// <summary>
        /// 
        /// </summary>
        Json = 0,

        /// <summary>
        /// 
        /// </summary>
        Yaml = 1
    }
}