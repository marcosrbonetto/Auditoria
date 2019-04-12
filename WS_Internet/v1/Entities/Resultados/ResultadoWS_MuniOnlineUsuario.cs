using System;
using System.Collections.Generic;
using System.Linq;

namespace WS_Internet.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoWS_MuniOnlineUsuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Cuil { get; set; }
        public bool SexoMasculino { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string IdentificadorFotoPersonal { get; set; }


        //Datos acceso
        public string Username { get; set; }

        
        //Datos contacto
        public string Email { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoFijo { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
    }
}