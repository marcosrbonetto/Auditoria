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
    public class Rules_TipoInscripcion : BaseRules<TipoInscripcion>
    {
        private readonly DAO_TipoInscripcion dao;

        public Rules_TipoInscripcion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_TipoInscripcion.Instance;
        }

        public Resultado<TipoInscripcion> GetByKeyValue(Enums.TipoInscripcion keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }
    }
}