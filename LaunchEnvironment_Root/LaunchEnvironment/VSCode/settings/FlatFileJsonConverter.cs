using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.settings
{
    public class FlatFileJsonConverter: JsonConverter
    {
        public static List<FlatFileJsonConverter> GetAllConverters(Type type)
        {
            throw new NotImplementedException();
        }
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                JObject newO = new JObject();

                foreach(var item in o.Properties())
                {
                    GetPropertyName("", item, newO);
                }

                newO.WriteTo(writer);
            }
        }


        private void GetPropertyName(string currParentPropName,JProperty srcProp, JObject resultProp)
        {
            if(srcProp.Value.Type == JTokenType.Null)
            {
                return;
            }

            if(srcProp.Value.Type !=  JTokenType.Object)
            {
                string tempPropName = string.IsNullOrEmpty(currParentPropName) ? srcProp.Name : string.Format("{0}.{1}", currParentPropName, srcProp.Name);
                resultProp.Add(new JProperty(tempPropName, srcProp.Value));
            }
            else
            {
                JObject o = (JObject) srcProp.Value;
                //discard null values
                if (o != null)
                {
                    string tempPropName = string.IsNullOrEmpty(currParentPropName) ? srcProp.Name : string.Format("{0}.{1}", currParentPropName, srcProp.Name);
                    foreach (var item in o.Properties())
                    {
                        GetPropertyName(tempPropName, item, resultProp);
                    }
                }
            }
        }

    }
}
