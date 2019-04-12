using System;
using System.Linq;
using _Model;
using _Rules.Rules;
using _Rules;

namespace WS_Intranet.v0.Rules
{
    public class WSRules_Base
    {
        private UsuarioLogueado data;

        private WSRules_Base()
        {

        }

        public WSRules_Base(UsuarioLogueado data)
            : this()
        {
            this.data = data;
        }

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        protected void setUsuarioLogueado(UsuarioLogueado data)
        {
            this.data = data;
        }
    }
}