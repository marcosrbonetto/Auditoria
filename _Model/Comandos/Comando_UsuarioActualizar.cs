using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Comandos
{
    public class Comando_UsuarioActualizar
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string FechaNacimiento { get; set; }
        public int Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public string DomicilioBarrio { get; set; }
        public string DomicilioCalle { get; set; }
        public string DomicilioAltura { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string Observaciones { get; set; }
        public string DomicilioPiso { get; set; }
        public string DomicilioDepto { get; set; }
        public string DomicilioCodigoPostal { get; set; }

    }
}
