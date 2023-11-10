using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsHub.Helpers;

public static class JSONListHelper
{
    public static string GetEventListJSONString(List<Eventh> events)
    {
        var eventlist = new List<EventFullCalendar>();
        foreach (var model in events)
        {
            var myevent = new EventFullCalendar()
            {
                id = 10,
                start = model.StartTime,
                end = model.EndTime,
                resourceId = 10,
                description = model.Description ?? "de",
                title = model.Name ?? "ti",
                className =  model.ClassName
            };
            eventlist.Add(myevent);
        }
        return System.Text.Json.JsonSerializer.Serialize(eventlist);
    }

    /*
    public static string GetResourceListJSONString(List<Models.Location> locations)
    {
        var resourcelist = new List<Resource>();

        foreach (var loc in locations)
        {
            var resource = new Resource()
            {
                id = loc.Id,
                title = loc.Name
            };
            resourcelist.Add(resource);
        }
        return System.Text.Json.JsonSerializer.Serialize(resourcelist);
    }
    */
}

//public class Event
//{
//    public int id { get; set; }
//    public string title { get; set; }
//    public DateTime start { get; set; }
//    public DateTime end { get; set; }
//    public int resourceId { get; set; }
//    public string description { get; set; }
//}

public class Resource
{
    public int id { get; set; }
    public string title { get; set; }

}

public class Eventh
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string ClassName { get; set; }


    //Relational data
    //public virtual Location Location { get; set; }
    public virtual ApplicationUser User { get; set; }

    public Eventh(IFormCollection form, ApplicationUser user)
    {
        User = user;
        Name = form["Eventh.Name"].ToString();
        Description = form["Eventh.Description"].ToString();
        StartTime = DateTime.Parse(form["Eventh.StartTime"].ToString());
        EndTime = DateTime.Parse(form["Eventh.EndTime"].ToString());
    }

    public void UpdateEventh(IFormCollection form, ApplicationUser user)
    {
        User = user;
        Name = form["Eventh.Name"].ToString();
        Description = form["Eventh.Description"].ToString();
        StartTime = DateTime.Parse(form["Eventh.StartTime"].ToString());
        EndTime = DateTime.Parse(form["Eventh.EndTime"].ToString());
    }

    public Eventh()
    {

    }
}

public class EventFullCalendar
{
    public int id { get; set; }
    public string title { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public int resourceId { get; set; }

    public string description { get; set; }

    public string className { get; set; }
}