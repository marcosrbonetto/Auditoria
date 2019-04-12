using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class TipoAuto: BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Enums.TipoAuto KeyValue { get; set; }
    }
}
