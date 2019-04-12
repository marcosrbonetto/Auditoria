using System;
using System.Linq;

namespace _Rules._WsVecinoVirtual
{
    [Serializable]
    public class VVResult<Entity>
    {
        public Entity Return { get; set; }

        public string Error { get; set; }

        public bool Ok
        {
            get
            {
                return string.IsNullOrEmpty(Error);
            }
        }
    }
}