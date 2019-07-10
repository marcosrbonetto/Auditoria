using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Linq;
using System.Collections.Generic;

namespace _Rules._WsVecinoVirtual
{
    public class VVResultadoUsuarioValidarRenaper
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Cuil { get; set; }
        public bool SexoMasculino { get; set; }
        public string NumeroTramite { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DomicilioLegal { get; set; }
        public string IdentificadorFotoPersonal { get; set; }

    }
}