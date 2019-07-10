using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;
using _Model.Resultados;
using NHibernate.Criterion.Lambda;
using NHibernate.Criterion;

namespace _DAO.DAO
{
    public class DAO_MuniOnlineUsuario : BaseDAO<MuniOnlineUsuario>
    {
        private static DAO_MuniOnlineUsuario instance;

        public static DAO_MuniOnlineUsuario Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_MuniOnlineUsuario();
                }
                return instance;
            }
        }   
    }
}