using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Internet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_InhabilitacionListado
    {
        public int Id { get; set; }
        public string Identificador { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string DtoRes { get; set; }
        public string Expediente { get; set; }
        public DateTime? FechaExpediente { get; set; }
        public string Causa { get; set; }
        public string Observaciones { get; set; }

        //Tipo Auto
        public string TipoAutoNombre{ get; set; }
        public int TipoAutoKeyValue { get; set; }

        //Usuario
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioApellidoNombre { get; set; }
        public int? UsuarioDni { get; set; }
        public bool? UsuarioSexoMasculino { get; set; }
    }
}