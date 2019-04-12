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
        public string FechaInicioString { get; set; }
        public DateTime? FechaFin { get; set; }
        public string FechaFinString { get; set; }
        public string TipoInhabilitacionNombre { get; set; }
        public int? TipoInhabilitacionKeyValue { get; set; }

        //Usuario
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioApellidoNombre { get; set; }
        public int? UsuarioDni { get; set; }
        public bool? UsuarioSexoMasculino { get; set; }

        public string Error { get; set; }

        public ResultadoWS_InhabilitacionListado(_Model.Entities.Inhabilitacion entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            FechaInicio = entity.FechaInicio;
            FechaInicioString = entity.FechaInicioString;
            FechaFin = entity.FechaFin;
            FechaFinString = entity.FechaFinString;
            Error = entity.Error;

            if (entity.TipoInhabilitacion != null)
            {
                TipoInhabilitacionNombre = entity.TipoInhabilitacion.Nombre;
                TipoInhabilitacionKeyValue = (int)entity.TipoInhabilitacion.KeyValue;
            }

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