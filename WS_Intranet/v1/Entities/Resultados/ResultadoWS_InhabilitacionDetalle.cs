using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_InhabilitacionDetalle
    {
        public int Id { get; set; }
        public DateTime? FechaInicio { get; set; }
        public string FechaInicioString { get; set; }
        public DateTime? FechaFin { get; set; }
        public string FechaFinString { get; set; }
        public string TipoInhabilitacionNombre { get; set; }
        public _Model.Enums.TipoInhabilitacion? TipoInhabilitacionKeyValue { get; set; }
        public string DtoRes { get; set; }
        public string Expediente { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesTipoAuto { get; set; }
        public string ObservacionesAutoChapa { get; set; }
        public string Error { get; set; }


        //Usuario
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioApellidoNombre { get; set; }
        public int? UsuarioDni { get; set; }
        public bool? UsuarioSexoMasculino { get; set; }

        public ResultadoWS_InhabilitacionDetalle(_Model.Entities.Inhabilitacion entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            FechaInicio = entity.FechaInicio;
            FechaInicioString = entity.FechaInicioString;
            FechaFin = entity.FechaFin;
            FechaFinString = entity.FechaFinString;
            DtoRes = entity.DtoRes;
            Expediente = entity.Expediente;
            ObservacionesAutoChapa = entity.ObservacionesAutoChapa;
            ObservacionesTipoAuto = entity.ObservacionesTipoAuto;
            Observaciones = entity.Observaciones;
            Error = entity.Error;

            if (entity.TipoInhabilitacion != null)
            {
                TipoInhabilitacionNombre = entity.TipoInhabilitacion.Nombre;
                TipoInhabilitacionKeyValue = entity.TipoInhabilitacion.KeyValue;
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

        public static List<ResultadoWS_InhabilitacionDetalle> ToList(IList<_Model.Entities.Inhabilitacion> entities)
        {
            return entities.Select(x => new ResultadoWS_InhabilitacionDetalle(x)).ToList();
        }

    }
}