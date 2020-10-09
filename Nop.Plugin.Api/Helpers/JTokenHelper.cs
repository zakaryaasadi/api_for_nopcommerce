namespace Nop.Plugin.Api.Helpers
{
    using Newtonsoft.Json.Linq;
    using Nop.Plugin.Api.JSON.Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public static class JTokenHelper
    {
        public static JToken RemoveEmptyChildrenAndFilterByFields(this JToken token, IList<string> jsonFields, int level = 1)
        {
            if (token.Type == JTokenType.Object)
            {
                var copy = new JObject();

                foreach (var prop in token.Children<JProperty>())
                {
                    var allowedFieldsInternal = jsonFields
                        .Where(i => i.Like(prop.Name + "(%"))
                        .Any();

                    // In the current json structure, the first level of properties is level 3. 
                    // If the level is > 3 ( meaning we are not on the first level of properties ), we should not check if the current field is containing into the list with fields, 
                    // so we need to serialize it always.
                    var allowedFields = jsonFields.Contains(prop.Name.ToLowerInvariant()) || allowedFieldsInternal || level > 3;

                    if (!allowedFields)
                        continue;

                    var child = prop.Value;

                    if (child.HasValues)
                    {
                        // Internal filter for attributes like -- fields=localized_names(localized_name) 
                        if (allowedFieldsInternal)
                        {
                            var propNameAndFields = GetRootAndChildren(prop.Name, jsonFields);
                            if (!string.IsNullOrEmpty(propNameAndFields.Key) && !string.IsNullOrEmpty(propNameAndFields.Value))
                            {
                                child = child.RemoveEmptyChildrenAndFilterByFields(GetPropertiesIntoList(propNameAndFields.Value));
                            }
                        }
                        else
                        {
                            child = child.RemoveEmptyChildrenAndFilterByFields(jsonFields, level + 1);
                        }
                    }


                    

                    // If the level == 3 ( meaning we are on the first level of properties ), we should not take into account if the current field is values,
                    // so we need to serialize it always.
                    var notEmpty = !child.IsEmptyOrDefault() || level == 1 || level == 3;

                    if (notEmpty && allowedFields)
                    {
                        copy.Add(prop.Name, child);
                    }
                }

                return copy;
            }

            if (token.Type == JTokenType.Array)
            {
                var copy = new JArray();

                foreach (var item in token.Children())
                {
                    var child = item;

                    if (child.HasValues)
                    {
                        child = child.RemoveEmptyChildrenAndFilterByFields(jsonFields, level + 1);
                    }

                    if (!child.IsEmptyOrDefault())
                    {
                        copy.Add(child);
                    }
                }

                return copy;
            }

            return token;
        }

        private static bool IsEmptyOrDefault(this JToken token)
        {
            return (token.Type == JTokenType.Array && !token.HasValues) || (token.Type == JTokenType.Object && !token.HasValues);
        }

        private static KeyValuePair<string, string> GetRootAndChildren(string propName, IList<string> jsonFields)
        {
            var jsonFieldInternal = jsonFields.First(i => i.Like(propName + "(%"));

            var indexBracket = jsonFieldInternal.IndexOf("(");
            var subLength = jsonFieldInternal.Length - 1 - (indexBracket + 1);

            var root = jsonFieldInternal.Substring(0, indexBracket);
            var children = jsonFieldInternal.Substring(indexBracket + 1, subLength);

            jsonFields.Remove(jsonFieldInternal);
            return new KeyValuePair<string, string>(root, children);
        }


        private static IList<string> GetPropertiesIntoList(string fields)
        {
            IList<string> properties = Split(fields.ToLowerInvariant())
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            return properties;
        }

        private static IList<string> Split(string fields)
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