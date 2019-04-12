using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Comandos
{
    public class Comando_InhabilitacionActualizar
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public virtual string DtoRes { get; set; }
        public virtual string Expediente { get; set; }
        public virtual string Observaciones { get; set; }
    }
}
