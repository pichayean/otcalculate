using OTCalculate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTCalculate.Extension
{
    public static class CommonExtension
    {
        public static IEnumerable<ProductivityOT> ToTotalOT(this List<Employee> employees)
        {
            var departments = new List<string>();
            var pot = new List<ProductivityOT>();
            foreach (var item in employees)
            {
                var departmentempcomein = item.WorkHours.GroupBy(p => p.Department.ToUpper().Trim())
                    .Select(e => e.FirstOrDefault().Department.ToUpper().Trim()).ToList();
                departments.AddRange(departmentempcomein);
            }
            var distinctDepartments = departments.GroupBy(d => d)
                   .Select(departm => departm.First())
                   .ToList();
            foreach (var item in distinctDepartments)
            {
                var sumOt = employees.Sum(e => e.GetOT().Where(e => e.Department.Equals(item)).Sum(e => e.OT));
                pot.Add(new ProductivityOT
                {
                    Department = item,
                    OT = sumOt
                });
            }

            return pot;
        }
    }
}
