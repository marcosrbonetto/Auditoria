using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Internet.v1.Entities.Resultados
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
        public bool Favorito { get; set; }
    }
}