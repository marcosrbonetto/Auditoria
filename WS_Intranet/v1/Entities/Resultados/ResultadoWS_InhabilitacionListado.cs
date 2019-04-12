using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_InhabilitacionListado
    {
        public int Id { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string DtoRes { get; set; }
        public string Expediente { get; set; }
        public string Observaciones { get; set; }

        //Usuario
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioApellidoNombre { get; set; }
        public int? UsuarioDni { get; set; }
        public bool? UsuarioSexoMasculino { get; set; }

        public ResultadoWS_InhabilitacionListado(_Model.Entities.Inhabilitacion entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            FechaInicio = entity.FechaInicio;
            FechaFin = entity.FechaFin;
            DtoRes = entity.DtoRes;
            Expediente = entity.Expediente;
            Observaciones = entity.Observaciones;

            if (entity.Usuario != null)
            {
                UsuarioId = entity.Usuario.Id;
                UsuarioNombre = entity.Usuario.Nombre;
                UsuarioApellido = entity.Usuario.Apellido;
                UsuarioApellidoNombre = entity.Usuario.Apellido + " " + entity.Usuario.Nombre;
                UsuarioDni = entity.Usuario.Dni;
                UsuarioSexoMasculino = entity.Usuario.SexoMasculino;
            }
        }

        public static List<ResultadoWS_InhabilitacionListado> ToList(IList<_Model.Entities.Inhabilitacion> entities)
        {
            return entities.Select(x => new ResultadoWS_InhabilitacionListado(x)).ToList();
        }
    }
}