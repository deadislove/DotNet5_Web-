using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext.Entities
{
    [Table("Enterprise_MVC_Core")]
    public class Enterprise_MVC_Core
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must have value.")]
        public string Name { get; set; }

        public int? Age { get; set; }

        [NotMapped]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
