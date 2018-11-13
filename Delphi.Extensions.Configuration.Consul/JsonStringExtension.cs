/*******************************************************************************
* File Name：  JsonStringExtension.cs
* Namespace: 
*
* CreateTime: 2018/11/13 15:47
* Author：  linkanyway
* Description： JsonStringExtension
* Class Name： JsonStringExtension
*
* Ver ChangeDate Author Description
* ───────────────────────────────────
* V0.01 2018/11/13 15:47 linkanyway draft
*
* Copyright (c) 2018 Allen Lian 
* Description: Framework
*
*********************************************************************************/

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Delphi.Extensions.Configuration.Consul
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonStringExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<string, string> ToDictionary(this string json)
        {
            var dictionary = new Dictionary<string, string>();

            //非空校验
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            //验证json字符串格式
            if (json[0] != '{' || json[json.Length - 1] != '}')
            {
                throw new ArgumentException("非法的Json字符串!");
            }

            // 转换为json对象
            var jObj = JObject.Parse(json);
            ParseJsonObject(jObj, dictionary, null);

            return dictionary;
        }

        private static void ParseJsonProperties(JProperty jProperty, IDictionary<string, string> dictionary,
            string parentName)
        {
            var key = parentName != null ? $"{parentName}:{jProperty.Name}" : jProperty.Name;
            switch (jProperty.Value.Type)
            {
                case JTokenType.None:
                    break;
                case JTokenType.Object
                    when jProperty.Value.HasValues:
                    var jObj = (JObject) jProperty.Value;
                    ParseJsonObject(jObj, dictionary, key);
                    break;
                case JTokenType.Array
                    when jProperty.Value.HasValues:
                    var jArr = (JArray) jProperty.Value;
                    ParseJsonArray(jArr, dictionary, key);
                    break;
                case JTokenType.Constructor:
                    break;
                case JTokenType.Property:
                    break;
                case JTokenType.Comment:
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.Date:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    dictionary.Add(key, jProperty.Value.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ParseJsonObject(JObject jObject, IDictionary<string, string> dictionary, string parentName)
        {
            var jProperties = jObject.Properties();
            foreach (var jProperty in jProperties)
            {
                ParseJsonProperties(jProperty, dictionary, parentName);
            }
        }

        private static void ParseJsonArray(JArray jArray, IDictionary<string, string> dictionary, string parentName)
        {
            foreach (var jToken in jArray)
            {
                var jObject = JObject.Parse(jToken.ToString());
                var jProperties = jObject.Properties();
                foreach (var jProperty in jProperties)
                {
                    ParseJsonProperties(jProperty, dictionary, parentName);
                }
            }
        }
    }
}