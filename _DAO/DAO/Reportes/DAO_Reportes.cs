using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;

namespace _DAO.DAO
{
    public class DAO_Reportes : BaseDAO<TipoAuto>
    {
        private static DAO_Reportes instance;

        public static DAO_Reportes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_Reportes();
                }
                return instance;
            }
        }



        public Resultado<TipoAuto> GetByKeyValue(Enums.TipoAuto keyValue)
        {
            var resultado = new Resultado<TipoAuto>();

            try
            {
                var entity = GetSession().QueryOver<TipoAuto>().Where(x => x.KeyValue == keyValue && x.FechaBaja == null).SingleOrDefault();
                resultado.Return = entity;
            }
            catch (Exception e)
            {
                resultado.SetError(e);
            }
            return resultado;

        }
    }
}