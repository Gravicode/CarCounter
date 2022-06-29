using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter1.Helpers
{
    public class Logger
    {
        public static bool SaveAsCsv(DataTable dt,string FileName)
        {
            try
            {
                var counter = 0;
                StringBuilder sb = new StringBuilder();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (counter > 0) sb.Append(";");
                    sb.Append($"{dc.ColumnName}");
                    counter++;
                }
                sb.AppendLine();
                foreach (DataRow dr in dt.Rows)
                {
                    counter = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (counter > 0) sb.Append(",");
                        sb.Append($"{dr[dc.ColumnName].ToString()}");
                        counter++;
                    }
                    sb.AppendLine();
                }
                File.WriteAllText(FileName, sb.ToString());
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return false;
            }
          

        }
    }
}
