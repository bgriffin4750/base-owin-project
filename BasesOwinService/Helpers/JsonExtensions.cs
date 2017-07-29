using Newtonsoft.Json;

namespace BaseOwinService.Helpers
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}