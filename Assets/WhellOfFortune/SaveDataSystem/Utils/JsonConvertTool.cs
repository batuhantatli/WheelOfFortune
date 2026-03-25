using Newtonsoft.Json;

    public static class JsonConvertTool
    {
        public static T ToJsonObject<T>(this string jsonString)
        {
            return string.IsNullOrEmpty(jsonString) ? default : JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string ToJsonString<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
