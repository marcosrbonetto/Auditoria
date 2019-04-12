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
    public class Rules_TipoCondicionInscripcion: BaseRules<TipoCondicionInscripcion>
    {
        private readonly DAO_TipoCondicionInscripcion dao;

        public Rules_TipoCondicionInscripcion(UsuarioLogueado data)
            : base(data)
        {
            dao = DAO_TipoCondicionInscripcion.Instance;
        }

        public Resultado<TipoCondicionInscripcion> GetByKeyValue(Enums.TipoCondicionInscripcion keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }
    }
}