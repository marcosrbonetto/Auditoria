using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Model.Entities
{
    public class TipoInhabilitacion : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual bool Permanente { get; set; }
        public virtual bool Invalido { get; set; }
    }
}
