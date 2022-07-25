using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace _101SoftwareTest.Models
{
    public partial class PropertyImage
    {
        public int Id { get; set; }
        public int IdPropiedad { get; set; }
        public string Files { get; set; }
        public bool Enabled { get; set; }

        public virtual Property IdPropertyNavigation { get; set; }
    }
}
