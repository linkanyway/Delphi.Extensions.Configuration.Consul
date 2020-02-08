/*******************************************************************************
* File Name：  ConsulConfigurationExtension.cs
* Namespace: 
*
* CreateTime: 2018-11-10 21:13
* Author：  linkanyway
* Description： ConsulConfigurationExtension
* Class Name： ConsulConfigurationExtension
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018-11-10 21:13 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Delphi.Extensions.Configuration.Consul
{
    /// <summary>
    /// Extension for ConfigurationBuilder to add customized consul configuration source
    /// <seealso cref="ConfigurationBuilder"/>
    /// </summary>
    public static class ConsulConfigurationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"><seealso cref="IConfigurationBuilder"/>IConfigurationBuilder</param>
        /// <param name="optionsAction">config action<seealso cref="Action{ConsulConfigurationOptions}"/></param>
        /// <param name="token"></param>
        /// <param name="autoReload"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder,
            Action<ConsulConfigurationOptions> optionsAction, CancellationToken token,bool autoReload
        )
        {
            builder.Add(new ConsulConfigurationSource(optionsAction, token,autoReload));
            return builder;
        }
    }
}