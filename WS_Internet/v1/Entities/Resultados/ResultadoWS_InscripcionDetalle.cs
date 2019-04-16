using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Internet.v1.Entities.Resultados
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
    }
}