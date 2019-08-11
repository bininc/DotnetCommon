using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Common.Web
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetString(this ISession session, string key, string value)
        {
            if (key != null && value != null)
            {
                session.Set(key, Encoding.UTF8.GetBytes(value));
            }
        }

        public static string GetString(this ISession session,string key)
        {
            byte[] values;
            bool suc = session.TryGetValue(key, out values);
            if (suc)
            {
                if (values == null) return null;
                return Encoding.UTF8.GetString(values);
            }
            else
            {
                return null;
            }
        }
    }
}
