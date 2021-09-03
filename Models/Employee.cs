using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTCalculate.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int DepartmentLimit { get; set; }
        public string MainDepartment { get; set; }
        public List<WorkDay> WorkHours { get; set; }
        public List<ProductivityOT> GetOT()
        {
            var results = WorkHours.GroupBy(p => p.Department.ToUpper().Trim()).Select(e => e.FirstOrDefault().Department.ToUpper().Trim()).ToList();
            var ots = new List<ProductivityOT>();

            foreach (var item in results)
            {
                var totalHr = WorkHours.Where(e => e.Department.Equals(item)).Sum(e => e.Hour);
                if (item.Equals(MainDepartment))
                {
                    ots.Add(new ProductivityOT
                    {
                        Department = item,
                        OT = totalHr - DepartmentLimit
                    });
                }
                else
                {
                    ots.Add(new ProductivityOT
                    {
                        Department = item,
                        OT = totalHr
                    });
                }
            }
            return ots;
        }
    }
    public class WorkDay
    {
        public int Hour { get; set; }
        public string Department { get; set; }
    }
}
