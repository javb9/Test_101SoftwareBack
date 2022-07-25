using _101SoftwareTest.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace _101SoftwareTest.Entities
{
    public class PropertyDto
    {
        [DataMember(Name = "IdPropiedad")]
        public int IdPropiedad { get; set; }

        [DataMember(Name = "NombrePropiedad")]
        public string NombrePropiedad { get; set; }

        [DataMember(Name = "DireccionPropiedad")]
        public string DireccionPropiedad { get; set; }

        [DataMember(Name = "PrecioPropiedad")]
        public decimal PrecioPropiedad { get; set; }

        [DataMember(Name = "CodigoPropiedad")]
        public string CodigoPropiedad { get; set; }

        [DataMember(Name = "AnnoPropiedad")]
        public int AnnoPropiedad { get; set; }

        [DataMember(Name = "IdPropietario")]
        public int IdPropietario { get; set; }

        [DataMember(Name = "NombrePropietario")]
        public string NombrePropietario { get; set; }

        [DataMember(Name = "DireccionPropietario")]
        public string DireccionPropietario { get; set; }

        [DataMember(Name = "FotoPropietario")]
        public string FotoPropietario { get; set; }

        [DataMember(Name = "identificacionPropietario")]
        public string identificacionPropietario { get; set; }

        [DataMember(Name = "FotosPropiedad")]
        public List<ImagePropertyDto> FotosPropiedad { get; set; }
    }
} 
