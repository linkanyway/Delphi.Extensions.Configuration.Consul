/*******************************************************************************
* File Name：  ConsulConfigurationSource.cs
* Namespace: 
*
* CreateTime: 2018-11-10 21:10
* Author：  linkanyway
* Description： ConsulConfigurationSource
* Class Name： ConsulConfigurationSource
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018-11-10 21:10 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System;
using System.Threading;
using Delphi.Extensions.Configuration.Consul.Abstraction;
using Delphi.Extensions.Configuration.Consul.Parsers;
using Microsoft.Extensions.Configuration;

namespace Delphi.Extensions.Configuration.Consul
{
    /// <inheritdoc />
    /// <summary>
    /// Consul configuration source
    /// </summary>
    public class ConsulConfigurationSource : IConsulConfigurationSource
    {
        /// <summary>
        /// config action
        /// </summary>
        private readonly Action<ConsulConfigurationOptions> _action;

        /// <summary>
        /// 
        /// </summary>
        public CancellationToken CancellationToken { get; }


        /// <summary>
        /// 
        /// </summary>
        public IConsulConfigurationParser Parser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReloadOnChange { get; set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action">config action<seealso cref="Action{T}"/></param>
        /// <param name="cancellationToken"></param>
        /// <param name="autoReload"></param>
        public ConsulConfigurationSource(Action<ConsulConfigurationOptions> action, CancellationToken cancellationToken,bool autoReload)
        {
            _action = action;
            CancellationToken = cancellationToken;
            ReloadOnChange = autoReload;

            Parser = new ConsulJsonParser();
        }


        /// <inheritdoc />
        /// <summary>
        /// Build  its will be invoke by <seealso cref="T:Microsoft.Extensions.Configuration.ConfigurationBuilder" />
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(this, _action, CancellationToken);
        }
    }
}