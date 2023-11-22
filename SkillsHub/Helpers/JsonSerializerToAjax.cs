using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SkillsHub.Helpers;

public static class JsonSerializerToAjax
{
    public static  Object GetJsonByIQueriable(IQueryable<Object> items)
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
