using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet5_web.Infra.DBContext.Entities;

namespace DotNet5_web.Core.DTO
{
    public class Enterprise_MVC_CoreDTO
    {
        [Key]
        public int ID { get; set; } = 0;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must have value.")]
        public string Name { get; set; } = string.Empty;

        public int? Age { get; set; } = 0;

        public DateTime Date { get; set; } = DateTime.Now;

        public static explicit operator Enterprise_MVC_CoreDTO(Enterprise_MVC_Core v)
        {
            return new Enterprise_MVC_CoreDTO() { 
                ID = v.ID, 
                Name = v.Name, 
                Age = v.Age,
                Date = v.Date
            };
        }
    }
}
