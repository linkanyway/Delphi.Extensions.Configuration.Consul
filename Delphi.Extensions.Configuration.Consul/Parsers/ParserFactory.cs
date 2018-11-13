/*******************************************************************************
* File Name：  ParserFactory.cs
* Namespace: 
*
* CreateTime: 2018/11/13 16:43
* Author：  linkanyway
* Description： ParserFactory
* Class Name： ParserFactory
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018/11/13 16:43 linkanyway draft
*
* Copyright (c) 2018 linkanyway 
* Description: Framework
*
*********************************************************************************/

namespace Delphi.Extensions.Configuration.Consul.Parsers
{
    /// <summary>
    /// 
    /// </summary>
    public class ParserFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IConsulConfigurationParser Build(ConsulConfigValueType type)
        {
            IConsulConfigurationParser parser = null;
            switch (type)
            {
                case
                    ConsulConfigValueType.Json:
                    parser = new ConsulJsonParser();
                    break;
                case ConsulConfigValueType.Yaml:
                    break;
                default:
                    parser = new ConsulJsonParser();
                    break;
            }

            return parser;
        }
    }
}