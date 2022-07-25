﻿using System;
using System.Collections.Generic;

#nullable disable

namespace _101SoftwareTest.Models
{
    public partial class Property
    {
        public Property()
        {
            PropertyImages = new HashSet<PropertyImage>();
            PropertyTraces = new HashSet<PropertyTrace>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public string CodeInternal { get; set; }
        public int? Year { get; set; }
        public int IdOwner { get; set; }

        public virtual Owner IdOwnerNavigation { get; set; }
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
        public virtual ICollection<PropertyTrace> PropertyTraces { get; set; }
    }
}
