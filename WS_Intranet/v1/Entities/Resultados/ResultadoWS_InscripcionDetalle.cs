using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_InscripcionDetalle
    {
        public int Id { get; set; }
        public string Identificador { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaTelegrama { get; set; }
        public DateTime? FechaVencimientoLicencia { get; set; }
        public string ArtCompañia { get; set; }
        public DateTime? ArtFechaVencimiento { get; set; }
        public string Caja { get; set; }
        public string Observaciones { get; set; }
        public bool Favorito { get; set; }

        //Tipo Auto
        public string TipoAutoNombre { get; set; }
        public int TipoAutoKeyValue { get; set; }

        //Condicion
        public string TipoCondicionInscripcionNombre { get; set; }
        public int TipoCondicionInscripcionKeyValue { get; set; }


        //Tipo Inscripcion
        public string TipoInscripcionNombre { get; set; }
        public int TipoInscripcionKeyValue { get; set; }


        //Usuario
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioApellidoNombre { get; set; }
        public int? UsuarioDni { get; set; }
        public bool? UsuarioSexoMasculino { get; set; }

        public string Error { get; set; }
        public ResultadoWS_InscripcionDetalle(_Model.Entities.Inscripcion entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            Identificador = entity.Identificador;
            FechaInicio = entity.FechaInicio;
            FechaFin = entity.FechaFin;
            FechaTelegrama = entity.FechaTelegrama;
            FechaVencimientoLicencia = entity.FechaVencimientoLicencia;
            ArtCompañia = entity.ArtCompañia;
            ArtFechaVencimiento = entity.ArtFechaVencimiento;
            Caja = entity.Caja;
            Observaciones = entity.Observaciones;
            Favorito = entity.Favorito;

            if (entity.TipoAuto != null)
            {
                TipoAutoNombre = entity.TipoAuto.Nombre;
                TipoAutoKeyValue = (int)entity.TipoAuto.KeyValue;
            }

            if (entity.TipoInscripcion != null)
            {
                TipoInscripcionNombre = entity.TipoInscripcion.Nombre;
                TipoInscripcionKeyValue = (int)entity.TipoInscripcion.KeyValue;
            }

            if (entity.TipoCondicionInscripcion != null)
            {
                TipoCondicionInscripcionNombre = entity.TipoCondicionInscripcion.Nombre;
                TipoCondicionInscripcionKeyValue = (int)entity.TipoCondicionInscripcion.KeyValue;
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
            Error = entity.Error;
        }

        public static List<ResultadoWS_InscripcionDetalle> ToList(IList<_Model.Entities.Inscripcion> entities)
        {
            return entities.Select(x => new ResultadoWS_InscripcionDetalle(x)).ToList();
        }
    }
}