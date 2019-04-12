using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Consultas
{
    public class Consulta_Usuario
    {
        public string Nombre { get; set; }
        public int? Dni { get; set; }
        public bool? SexoMasculino { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public bool? DadosDeBaja { get; set; }

        public Consulta_Usuario()
        {
            DadosDeBaja = false;
        }
    }
}
