using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkillsHub.Helpers;

public static class JsonSerializerToAjax
{
    public static Object GetJsonByIQueriable(IQueryable<Object> items)
    {
        string json = "";

        try
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            json = JsonSerializer.Serialize(items, options);
        }
        catch (Exception ex) { }
        return json;
    }
}
