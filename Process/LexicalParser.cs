using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OTCalculate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OTCalculate.Process
{
    public class LexicalParser
    {
        public static List<Employee> ToEmpolyee(IFormFile fileexl)
        {
            List<Employee> els = new List<Employee>();
            using (var stream = new MemoryStream())
            {
                fileexl.CopyTo(stream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var firstSheet = package.Workbook.Worksheets["OT"];
                    char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWX".ToCharArray();
                    int[] rows = Enumerable.Range(1, 100 - 1).ToArray();
                    var item = firstSheet.Cells;
                    for (int i = 1; i < rows.Length; i++)
                    {
                        if (item[$"A{i}"].Text.ToLower().Trim().Equals("name"))
                        {
                            Employee e = new Employee();
                            e.Name = item[$"A{i + 1}"].Text;
                            e.Type = item[$"B{i + 1}"].Text;
                            e.MainDepartment = item[$"C{i + 1}"].Text;
                            e.DepartmentLimit = Convert.ToInt32(item[$"D{i + 1}"].Text);
                            e.WorkHours = new List<WorkDay>();
                            foreach (var col in alpha)
                            {
                                var key = item[$"{col}{i + 1}"].Text.ToString();
                                var val = item[$"{col}{i}"].Text.ToString();
                                if (!String.IsNullOrEmpty(key)
                                    && !String.IsNullOrEmpty(val))
                                {
                                    if (col >= 'E')
                                    {
                                        e.WorkHours.Add(new WorkDay()
                                        {
                                            Department = item[$"{col}{i + 1}"].Text.ToString(),
                                            Hour = Convert.ToInt32(item[$"{col}{i}"].Text)
                                        });
                                    }
                                }
                            }
                            els.Add(e);
                        }
                    }
                }
            }
            return els;

        }
    }
}
