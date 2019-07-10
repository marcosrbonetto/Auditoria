using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Linq;
using System.Collections.Generic;

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