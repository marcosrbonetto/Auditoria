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
    public class Rules_TipoInhabilitacion: BaseRules<TipoInhabilitacion>
    {
        private readonly DAO_TipoInhabilitacion dao;

        public Rules_TipoInhabilitacion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_TipoInhabilitacion.Instance;
        }
    }
}