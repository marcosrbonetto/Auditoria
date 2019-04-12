using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class TipoInscripcion : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Enums.TipoInscripcion KeyValue { get; set; }
    }
}
