using System;
using System.Linq;
using _DAO.DAO;
using _Model;
using _Model.Entities;
using System.Configuration;
using System.Collections.Generic;
using _Rules._WsVecinoVirtual;

namespace _Rules.Rules
{
    public class Rules_TipoAuto : BaseRules<TipoAuto>
    {
        private readonly DAO_TipoAuto dao;

        public Rules_TipoAuto(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_TipoAuto.Instance;
        }

        public Resultado<TipoAuto> GetByKeyValue(Enums.TipoAuto keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }
    }
}