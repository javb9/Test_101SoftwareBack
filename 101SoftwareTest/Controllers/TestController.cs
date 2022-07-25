using _101SoftwareTest.Entities;
using _101SoftwareTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace _101SoftwareTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        /// <summary>
        /// Se obtienen todas las propiedades
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public List<PropertyDto> GetProperties(string filtroPrincipal, int IdPropiedad, int filtroOrdenar)
        {
            using (var db = new Models._101Software_TestContext())
            {
                List<PropertyDto> properties = new List<PropertyDto>();

                DbCommand cmd;
                DbDataReader rdr;
                string sql;

                #region VALIDACION SI SE ESTA BUSCANDO POR CODIGO O NOMBRE DE LA PROPIEDAD Y SI HAY FILTROS
                if (filtroOrdenar == 0)
                {
                    if (filtroPrincipal != null && filtroPrincipal.Length > 0)
                    {
                        sql = "exec dbo.[Consultas] @OPERACION=8, @FILTRO=" + "'" + filtroPrincipal + "'";
                    }
                    else if (IdPropiedad != 0 && IdPropiedad != null) //AQUI SE VALIDA SI SE ESTA BUSCANDO UNA PROPIEDAD EN ESPECIFICO
                    {
                        sql = "exec dbo.[Consultas] @OPERACION=9, @ID_PROPIEDAD=" + IdPropiedad.ToString();
                    }
                    else //AQUI SE BUSCAN TODAS LAS PROPIEDADES
                    {
                        sql = "exec dbo.[Consultas] @OPERACION=1";
                    }
                }
                else
                {
                    if (filtroPrincipal != null && filtroPrincipal.Length > 0)
                    {
                        switch (filtroOrdenar)
                        {
                            case 1:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO=" + "'" + filtroPrincipal + "'" 
                                    + ",@FILTRO_ORDENAR='1'";
                                break;
                            case 2:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO=" + "'" + filtroPrincipal + "'"
                                    + ",@FILTRO_ORDENAR='2'";
                                break;
                            case 3:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO=" + "'" + filtroPrincipal + "'"
                                    + ",@FILTRO_ORDENAR='3'";
                                break;
                            default:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO=" + "'" + filtroPrincipal + "'"
                                    + ",@FILTRO_ORDENAR='4'";
                                break;
                        }

                    }
                    else
                    {
                        switch (filtroOrdenar)
                        {
                            case 1:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO_ORDENAR='PROPERTIES.PRICE DESC'";
                                break;
                            case 2:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO_ORDENAR='PROPERTIES.PRICE ASC'";
                                break;
                            case 3:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO_ORDENAR='PROPERTIES.YEAR ASC'";
                                break;
                            default:
                                sql = "exec dbo.[Consultas] @OPERACION=10,@FILTRO_ORDENAR='PROPERTIES.YEAR DESC'";
                                break;
                        }
                    }
                }
                #endregion

                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    properties.Add(new PropertyDto
                    {

                        IdPropiedad = rdr.GetInt32(0),
                        NombrePropiedad = rdr.GetString(1),
                        DireccionPropiedad = rdr.GetString(2),
                        PrecioPropiedad = rdr.GetDecimal(3),
                        CodigoPropiedad = rdr.GetString(4),
                        AnnoPropiedad=rdr.GetInt32(5),
                        IdPropietario= rdr.GetInt32(6),
                        NombrePropietario= rdr.GetString(7),
                        DireccionPropietario = rdr.GetString(8),
                        FotoPropietario=rdr.GetString(9)
                    });
                }
                rdr.Close();

                if (properties.Count > 0)
                {
                    foreach (PropertyDto property in properties)
                    {
                        sql = "exec dbo.[Consultas] @OPERACION=2, @ID_PROPIEDAD= " + (property.IdPropiedad).ToString();
                        cmd = db.Database.GetDbConnection().CreateCommand();
                        cmd.CommandText = sql;

                        db.Database.OpenConnection();

                        rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        property.FotosPropiedad = new List<ImagePropertyDto>();
                        while (rdr.Read())
                        {
                            property.FotosPropiedad.Add(new ImagePropertyDto
                            {
                                Id = rdr.GetInt32(0),
                                IdPropiedad = rdr.GetInt32(1),
                                Imagen = rdr.GetString(2),
                                Enabled = rdr.GetBoolean(3)
                            });
                        }
                        rdr.Close();
                    }
                }

                return properties;
            }
            
        }

        /// <summary>
        /// Se guarda una nueva propiedad
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [HttpPost("UploadProperty")]
        public bool UploadProperty(PropertyDto property)
        {
            DbCommand cmd;
            DbDataReader rdr;
            string sql;
            using (var db = new Models._101Software_TestContext())
            {
                #region guardar propiedad

                #region variables para guardar propiedad
                string nombrePropiedad = ",@NOMBRE_PROPIEDAD=" + "'" + property.NombrePropiedad + "'";
                string DireccionPropiedad = ",@DIRECCION_PROPIEDAD=" + "'" + property.DireccionPropiedad + "'";
                string PrecioPropiedad = ",@PRECIO_PROPIEDAD=" + property.PrecioPropiedad.ToString();
                string AnnoPropiedad = ",@AÑO_PROPIEDAD=" + property.AnnoPropiedad.ToString();
                string IdPropietario = ",@ID_PROPIETARIO=" + property.IdPropietario.ToString();
                string CodigoPropiedad = ",@CODIGO_PROPIEDAD=" + "'" + property.CodigoPropiedad +"'";
                #endregion

                #region Se valida que el codigo de la propiedad no se repita 

                sql = "exec dbo.[Consultas] @OPERACION=7" 
                        + CodigoPropiedad;
                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
  
                if (rdr.Read())
                {
                    return false;
                }
                rdr.Close();

                #endregion

                sql = "exec dbo.[Consultas] @OPERACION=4"
                                + IdPropietario
                                + nombrePropiedad
                                + DireccionPropiedad
                                + PrecioPropiedad
                                + AnnoPropiedad
                                + CodigoPropiedad;
                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                rdr.Close();
                #endregion

                sql = "exec dbo.[Consultas] @OPERACION=5";
                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                int IdPropiedad= new int();
                while (rdr.Read())
                {
                    IdPropiedad = rdr.GetInt32(0);
                }
                rdr.Close();

                foreach(var imagen in property.FotosPropiedad)
                {

                    #region variables para guardar imagenes
                    string propiedadId= ",@ID_PROPIEDAD=" + IdPropiedad.ToString();
                    string Files = ",@FILES="+ "'" + imagen.Imagen + "'";
                    string Enabled = ",@ENABLED=1"; 
                    #endregion

                    sql = "exec dbo.[Consultas] @OPERACION=6"
                                + propiedadId
                                + Files
                                + Enabled;
                    cmd = db.Database.GetDbConnection().CreateCommand();
                    cmd.CommandText = sql;

                    db.Database.OpenConnection();
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    rdr.Close();
                }
                return true;
            }
        }

        /// <summary>
        /// Se obtienen todos lo propietarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOwners")]
        public List<Owner> GetOwners()
        {
            DbCommand cmd;
            DbDataReader rdr;

            using (var db = new Models._101Software_TestContext())
            {

                string sql = "exec dbo.[Consultas] @OPERACION=3";

                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Owner> propietarios = new List<Owner>();
                while (rdr.Read())
                {
                    propietarios.Add(new Owner
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1)
                    });
                }
                rdr.Close();

                return propietarios;
            }
        }

        /// <summary>
        /// Se obtiene el ID mas alto para sumarle uno y asignar a imagenes nuevas
        /// </summary>
        /// <returns></returns>
        [HttpPost("saveEditProperty")]
        public bool saveEditProperty(PropertyDto property)
        {
            DbCommand cmd;
            DbDataReader rdr;

            using (var db = new Models._101Software_TestContext())
            {
                string idPropiedad = ",@ID_PROPIEDAD=" + property.IdPropiedad.ToString();
                string direccionPropiedad = ",@DIRECCION_PROPIEDAD=" + "'" + property.DireccionPropiedad + "'";
                string precioPropiedad = ",@PRECIO_PROPIEDAD=" + property.PrecioPropiedad.ToString();

                string sql = "exec dbo.[Consultas] @OPERACION=11" 
                            + idPropiedad
                            + direccionPropiedad
                            + precioPropiedad;

                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                rdr.Close();

                return true;
            }
        }

        /// <summary>
        /// deshabilita imagenes
        /// </summary>
        /// <param name="idImage"></param>
        /// <returns></returns>
        [HttpPost("DisabledImage")]
        public bool DisabledImage(ImagePropertyDto image)
        {
            DbCommand cmd;
            DbDataReader rdr;

            using (var db = new Models._101Software_TestContext())
            {

                string sql = "UPDATE PROPERTY_IMAGE SET ENABLED=0 WHERE ID=" + image.Id.ToString();

                cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = sql;

                db.Database.OpenConnection();

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                rdr.Close();

            }

            return true;
        }


        /// <summary>
        /// Insertar imagenes nuevas a propiedad
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost("UploadPropertyImage")]
        public bool UploadPropertyImage(List<ImagePropertyDto> images)
        {
            DbCommand cmd;
            DbDataReader rdr;
            string sql;
            using (var db = new Models._101Software_TestContext())
            {
                foreach (var imagen in images)
                {

                    #region variables para guardar imagenes
                    string propiedadId = ",@ID_PROPIEDAD=" + imagen.IdPropiedad.ToString();
                    string Files = ",@FILES=" + "'" + imagen.Imagen + "'";
                    string Enabled = ",@ENABLED=1";
                    #endregion

                    sql = "exec dbo.[Consultas] @OPERACION=6"
                                + propiedadId
                                + Files
                                + Enabled;
                    cmd = db.Database.GetDbConnection().CreateCommand();
                    cmd.CommandText = sql;

                    db.Database.OpenConnection();
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    rdr.Close();
                }
            }

            return true;
        }

    }
}
