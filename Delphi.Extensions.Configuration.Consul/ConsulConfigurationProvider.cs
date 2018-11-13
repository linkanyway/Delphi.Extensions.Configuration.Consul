/*******************************************************************************
* File Name：  ConsulConfigurationProvider.cs
* Namespace: 
*
* CreateTime: 2018-11-10 21:06
* Author：  linkanyway
* Description： ConsulConfigurationProvider
* Class Name： ConsulConfigurationProvider
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018-11-10 21:06 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Delphi.Extensions.Configuration.Consul.Abstraction;
using Delphi.Extensions.Configuration.Consul.Parsers;
using Microsoft.Extensions.Configuration;

namespace Delphi.Extensions.Configuration.Consul
{
    /// <inheritdoc />
    /// <summary>
    /// Consul Configuration Provider
    /// <seealso cref="ConfigurationProvider"/>
    /// </summary>
    public sealed class ConsulConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Consul configuration option
        /// </summary>
        private readonly ConsulConfigurationOptions _options = new ConsulConfigurationOptions();


        /// <summary>
        /// failure count
        /// </summary>
        private int _failureCount;

        /// <summary>
        /// consul url index / determine which url used to connect and get KV currently
        /// </summary>
        private int _consulUrlIndex;


        ///todo: replace the engines by Consul DNS  <seealso cref="http://stackexchange.com/"/>
        /// <summary>
        /// Consul Urls /  used to automatic switch once get Consul request failure
        /// </summary>
        private readonly IReadOnlyList<Uri> _consulUrls;

        /// <summary>
        /// 
        /// </summary>
        private readonly Task _configurationListeningTask;

        /// <summary>
        /// Consul client
        /// <seealso cref="ConsulClient"/>
        /// </summary>
        private readonly ConsulClient _consulClient;


        /// <summary>
        /// KV prefix
        /// </summary>
        private readonly string _prefix;

        private IConsulConfigurationSource _source;

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action">Action for config ConsuleConfigurationOptions</param>
        /// <param name="cancellationToken"></param>
        public ConsulConfigurationProvider(IConsulConfigurationSource source, Action<ConsulConfigurationOptions> action,
            CancellationToken cancellationToken)
        {
            _source = source;

            if (_source.Parser == null) //check if have the parser instance
            {
                throw new ArgumentNullException(nameof(source.Parser));
            }

            CancellationToken = cancellationToken;
            action(_options); //  parameter pass by reference, it will can the orignal object  /  引用类型默认传引用
            _prefix = _options.Prefix;
            if (!string.IsNullOrEmpty(_prefix))
            {
                _prefix = _prefix.Trim();
            }

            _consulUrls = _options.Address;

            //create new consul client
            _consulClient = new ConsulClient(cfg =>
            {
                cfg.Address = _options.Address[_consulUrlIndex];
                cfg.Datacenter = _options.Datacenter;
                cfg.HttpAuth = _options.HttpAuth; //todo: delete this feature later
                cfg.Token = _options.Token;
                cfg.WaitTime = _options.WaitTime;
            });


            //
            if (_consulUrls.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_consulUrls));
            }

          
                _configurationListeningTask = new Task(ListenToConfigurationChanges);
            
        }

        /// <inheritdoc />
        /// <summary>
        ///Read config from remote consul server
        /// </summary>
        public override void Load()
        {
            var result = _consulClient.KV.List(_prefix, CancellationToken).Result;

            //check result
            if (result == null || result.StatusCode != HttpStatusCode.OK || result.Response == null) return;


            foreach (var item in result.Response) //loop all response and convert by each
            {
                if (item.Key.EndsWith("/") || item.Value == null) continue;
                
                try
                {
                    var key = string.IsNullOrEmpty(_prefix)
                        ? item.Key.Replace('/', ':')
                        : item.Key.Substring(_prefix.Length + 1).Replace('/', ':');
                    var value = System.Text.Encoding.UTF8.GetString(item.Value);
                    
                    //todo: check value content type json or yaml???

                    var parse = ParserFactory.Build(ConsulConfigValueType.Json);

                    var dics = parse.Parse(key, value);
                 

                    foreach (var dic in dics)
                    {
                        if (Data.ContainsKey(dic.Key))
                        {
                            Data[dic.Key] = dic.Value;
                        }
                        else
                        {
                            Data.Add(dic);
                        }
                    }
                }
                catch (TaskCanceledException exception)
                {
                    Console.WriteLine(exception);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // throw;
                }
            }


            //start sync task

            if (!_source.ReloadOnChange) return;
            if (_configurationListeningTask.Status == TaskStatus.Created)
                _configurationListeningTask.Start();

        }


        /// <summary>
        /// watch the K/V change on remote Consul server
        /// </summary>
        private async void ListenToConfigurationChanges()
        {
            while (true) //loop
            {
                try
                {
                    if (_failureCount > _consulUrls.Count)
                    {
                        _failureCount = 0;
                        // ReSharper disable once MethodSupportsCancellation
                        await Task.Delay(TimeSpan.FromMinutes(1));
                    }

                    Load();
                    OnReload(); // fire the reload event
                    _failureCount = 0; //clear the failure counter!
                }
                catch (TaskCanceledException) //canceled by user
                {
                    _failureCount = 0;
                }
                catch (Exception ex)
                {
                    _consulUrlIndex = (_consulUrlIndex + 1) % _consulUrls.Count;
                    _failureCount++;
                }
            }
        }
    }
}