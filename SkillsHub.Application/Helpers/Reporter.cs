using Newtonsoft.Json;
using Spire.Xls;
using DataTable = System.Data.DataTable;

namespace SkillsHub.Application.Helpers;
public static class Reporter
{
    public static void ReportToXLS<T>(T[] entities)
    {
        try
        {
            var options = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,

            };
            var jsonEntities = JsonConvert.SerializeObject(entities, options);
            var dt = (DataTable)JsonConvert.DeserializeObject(jsonEntities, typeof(DataTable));

            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];

            worksheet.Name = typeof(T).Name;

            //var rowcount = 1;
            //foreach (DataRow row in dt.Rows)
            //{
            //    for (int i = 1; i <= dt.Columns.Count; i++)
            //    {
            //        worksheet.SetCellValue(rowcount, i, row[i].ToString());
            //    }
            //}

            worksheet.InsertDataTable(dt, true, 1, 1);
            workbook.SaveToFile($"report{typeof(T).Name}.xlsx");
        }
        catch { }

    }
}
