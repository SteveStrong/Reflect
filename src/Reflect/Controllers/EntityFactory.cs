using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

namespace Reflect.Controllers
{
    public class EntityFactory
    {
        public bool MapValueAsDate(JObject obj, string name, Action<DateTime> assignValue)
        {
            JToken token;
            var result = obj.TryGetValue(name, StringComparison.CurrentCultureIgnoreCase, out token);
            if (result)
            {
                var value = DateTime.Parse(token.ToString());
                var date = new DateTime(value.Ticks, DateTimeKind.Local);
                assignValue(date);
            }
            return result;
        }
        public bool MapValueAsString(JObject obj, string name, Action<String> assignValue)
        {
            JToken token;
            var result = obj.TryGetValue(name, StringComparison.CurrentCultureIgnoreCase, out token);
            if (result)
            {
                assignValue(token.ToString());
            }
            return result;
        }
        public T Create<T>(string Id, object value) where T : Grievance
        {
            var split = Id.Split(new[] { "." }, StringSplitOptions.None);
            var source = Activator.CreateInstance<T>();
            Hydrate<T>(source, value);
            SetId<T>(Id, source);

            return source;
        }

        public T SetId<T>(string Id, T obj) where T : IDocument
        {
            var split = Id.Split(new[] { "." }, StringSplitOptions.None);

            obj.Id = Id;
            obj.Name = split[0];
            obj.Version = split[1];

            var body = JObject.Parse(obj.Document);
            body["Id"] = Id;
            obj.Document = body.ToString();
            return obj;
        }

        public void TrackPrevious<TO, TN>(TO oldObj, TN newObj) where TO : IDocument where TN : IDocument
        {
            oldObj.NextVersion = newObj.Id;
            newObj.LastVersion = oldObj.Id;
        }

        public T Hydrate<T>(T source, object value) where T : IDocument
        {
            var obj = JObject.Parse(value?.ToString());
            var info = source.GetType().GetTypeInfo();
            var props = info.GetProperties();

            foreach (var item in props)
            {
                if (item.Name == "Document") continue;
                if (item.PropertyType == typeof(DateTime))
                {
                    MapValueAsDate(obj, item.Name, x => item.SetValue(source, x));
                }
                else
                {
                    MapValueAsString(obj, item.Name, x => item.SetValue(source, x));
                }
            }

            source.Document = value?.ToString();
            return source;
        }

        public T Rehydrate<T>(T source, object value) where T : IDocument
        {
            var obj = JObject.Parse(value?.ToString());
            var info = source.GetType().GetTypeInfo();
            var props = info.GetProperties();

            foreach (var item in props)
            {
                if (item.Name == "Document") continue;
                obj[item.Name] = item.GetValue(source).ToString();
            }

            source.Document = JsonConvert.SerializeObject(obj);
            return source;
        }
    }
}
