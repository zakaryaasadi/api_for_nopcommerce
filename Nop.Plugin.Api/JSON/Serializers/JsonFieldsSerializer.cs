namespace Nop.Plugin.Api.JSON.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using Nop.Plugin.Api.DTO;
    using Nop.Plugin.Api.Helpers;

    public class JsonFieldsSerializer : IJsonFieldsSerializer
    {
        public string Serialize(ISerializableObject objectToSerialize, string jsonFields)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentNullException(nameof(objectToSerialize));
            }

            IList<string> fieldsList = null;

            if (!string.IsNullOrEmpty(jsonFields))
            {
                var primaryPropertyName = objectToSerialize.GetPrimaryPropertyName();

                fieldsList = GetPropertiesIntoList(jsonFields);

                // Always add the root manually
                fieldsList.Add(primaryPropertyName);
            }

            var json = Serialize(objectToSerialize, fieldsList);

            return json;
        }

        private string Serialize(object objectToSerialize, IList<string> jsonFields = null)
        {
            var jToken = JToken.FromObject(objectToSerialize);

            if (jsonFields != null)
            {
                jToken = jToken.RemoveEmptyChildrenAndFilterByFields(jsonFields);
            }

            var jTokenResult = jToken.ToString();

            return jTokenResult;
        }

        private IList<string> GetPropertiesIntoList(string fields)
        {
            IList<string> properties = Split(fields.ToLowerInvariant())
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            return properties;
        }

        private IList<string> Split(string fields)
        {
            var arrChars = fields.ToCharArray();

            var stack = new List<string>();
            var exp = "";
            var isOpen = false;
            foreach (var c in arrChars)
            {
                if (char.IsWhiteSpace(c))
                    continue;

                if (c == ',' && !isOpen)
                {
                    if (!string.IsNullOrEmpty(exp))
                        stack.Add(exp);
                    exp = "";
                    continue;
                }
                else if (c == '(')
                    isOpen = true;
                else if (c == ')')
                    isOpen = false;

                exp += c;
            }

            if (!string.IsNullOrEmpty(exp))
                stack.Add(exp);

            return stack;
        }
    }
}