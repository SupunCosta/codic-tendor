using Newtonsoft.Json;

namespace UPrinceV4.Web.Util;

public static class JsonToStringConverter
{
    public static string getStringFromJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        return json;
    }
}