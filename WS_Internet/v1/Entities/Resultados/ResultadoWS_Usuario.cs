using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Internet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string DomicilioBarrio { get; set; }
        public string DomicilioCalle { get; set; }
        public string DomicilioAltura { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string Observaciones { get; set; }
        public string DomicilioPiso { get; set; }
        public string DomicilioDepto { get; set; }
        public string DomicilioCodigoPostal { get; set; }
        public string Error { get; set; }
    }
}