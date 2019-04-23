using _Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Intranet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_InscripcionListado
    {
        public int Id { get; set; }
        public string Identificador { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Favorito { get; set; }

        //Tipo Auto
        public string TipoAutoNombre { get; set; }
        public int TipoAutoKeyValue { get; set; }

        //Condicion
        public string TipoCondicion { get; set; }
        public int TipoCondicionKeyValue { get; set; }

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

        public ResultadoWS_InscripcionListado(_Model.Entities.Inscripcion entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            Identificador = entity.Identificador;
            FechaInicio = entity.FechaInicio;
            FechaFin = entity.FechaFin;
            Favorito = entity.Favorito;

            if (entity.TipoAuto != null)
            {
                TipoAutoNombre = entity.TipoAuto.Nombre;
                TipoAutoKeyValue = (int)entity.TipoAuto.KeyValue;
            }

            if (entity.TipoCondicionInscripcion != null)
            {
                TipoCondicion = entity.TipoCondicionInscripcion.Nombre;
                TipoCondicionKeyValue = (int)entity.TipoCondicionInscripcion.KeyValue;
            }

            if (entity.TipoInscripcion != null)
            {
                TipoInscripcionNombre = entity.TipoInscripcion.Nombre;
                TipoInscripcionKeyValue = (int)entity.TipoInscripcion.KeyValue;
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

        public static List<ResultadoWS_InscripcionListado> ToList(IList<_Model.Entities.Inscripcion> entities)
        {
            return entities.Select(x => new ResultadoWS_InscripcionListado(x)).ToList();
        }
    }
}