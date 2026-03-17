using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ServiceCategoryRequest
    {
        [Required]
        public string Title { get; set; }
        public string? IconClass { get; set; }
    }
}
