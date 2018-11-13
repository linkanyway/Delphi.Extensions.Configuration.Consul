/*******************************************************************************
* File Name：  IConsulConfigurationSource.cs
* Namespace: 
*
* CreateTime: 2018-11-10 21:15
* Author：  linkanyway
* Description： IConsulConfigurationSource
* Class Name： IConsulConfigurationSource
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018-11-10 21:15 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System.Threading;
using Delphi.Extensions.Configuration.Consul.Parsers;
using Microsoft.Extensions.Configuration;

namespace Delphi.Extensions.Configuration.Consul.Abstraction
{
    /// <inheritdoc />
    /// <summary>
    /// IConsulConfiguration<seealso cref="T:Microsoft.Extensions.Configuration.IConfigurationSource" />
    /// </summary>
    public interface IConsulConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// the token used to cancel auto-reload function
        /// </summary>
       CancellationToken CancellationToken { get; }

        
        /// <summary>
        /// the parser used to parse the content to Microsoft's configuration format
        /// </summary>
         IConsulConfigurationParser Parser { get; set; }
        
        /// <summary>
        /// if reload by automatically
        /// </summary>
        bool ReloadOnChange { get; set; }
    }
}