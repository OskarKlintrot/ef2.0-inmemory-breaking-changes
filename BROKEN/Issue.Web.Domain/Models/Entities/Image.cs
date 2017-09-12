using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Intranet.Web.Domain.Models.Entities
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public String FileName { get; set; }
    }
}
