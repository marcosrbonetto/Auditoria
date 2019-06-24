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
        public bool? ConError { get; set; }
        public bool? Favorito { get; set; }
        public bool? DadosDeBaja { get; set; }
        public int? Antiguedad { get; set; }
        public Enums.TipoAuto? TipoAuto { get; set; }
        public Enums.TipoInscripcion? TipoInscripcion { get; set; }
        public bool? ConSexo { get; set; }

        public Consulta_Usuario()
        {
            DadosDeBaja = false;
        }
    }
}
