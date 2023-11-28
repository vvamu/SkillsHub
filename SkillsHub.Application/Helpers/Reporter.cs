using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Xls;

namespace SkillsHub.Application.Helpers
{
    public static class Reporter
    {
        public static void ReportToXLS<T>(T[] entities)
        {
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];


            DataTable dataTable = new DataTable(typeof(T).Name);

            foreach(var prop in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (var (entity, i) in entities.Select((value, i) => (value, i)))
            {
                var row = dataTable.NewRow();

                foreach (var prop in typeof(T).GetProperties())
                { 
                    var value = prop.GetValue(entity, null);
                    row[i] = value ?? "-";
                }
            }

            worksheet.InsertDataTable(dataTable, true, 1, 1);
            workbook.SaveToFile($"report{typeof(T).Name}.xlsx");
        }
    }
}
