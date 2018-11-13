/*******************************************************************************
* File Name：  ConsulConfigurationOptions.cs
* Namespace: 
*
* CreateTime: 2018-11-10 21:05
* Author：  linkanyway
* Description： ConsulConfigurationOptions
* Class Name： ConsulConfigurationOptions
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018-11-10 21:05 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Delphi.Extensions.Configuration.Consul
{
    /// <summary>
    /// Consul server configuration
    /// </summary>
    public class ConsulConfigurationOptions
    {
        /// <summary>
        /// Remote consul server address
        /// </summary>
        public IReadOnlyList<Uri> Address { get; set; }


        /// <summary>
        /// certificate
        /// </summary>
        public X509Certificate2 ClientCertificate { get; set; }


        /// <summary>
        /// DataCenter name
        /// </summary>
        public string Datacenter { get; set; }


        /// <summary>
        /// NetworkCrdential for ui
        /// </summary>
        public NetworkCredential HttpAuth { get; set; }


        /// <summary>
        /// Token for ACL
        /// </summary>
        public string Token { get; set; }


        /// <summary>
        /// Timeout setting
        /// </summary>
        public TimeSpan? WaitTime { get; set; }


        /// <summary>
        /// KV prefix
        /// </summary>
        public string Prefix { get; set; }
    }
}