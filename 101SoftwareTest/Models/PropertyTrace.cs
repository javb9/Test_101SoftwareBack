using System;
using System.Collections.Generic;

#nullable disable

namespace _101SoftwareTest.Models
{
    public partial class PropertyTrace
    {
        public int Id { get; set; }
        public DateTime? DateSale { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public decimal? Tax { get; set; }
        public int IdProperty { get; set; }

        public virtual Property IdPropertyNavigation { get; set; }
    }
}
