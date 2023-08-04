using Newtonsoft.Json;

namespace EventRegistrationSystem.Extensions
{
    public static class SessionExtensions
    {
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            string? value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
