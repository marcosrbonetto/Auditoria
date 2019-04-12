using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;

namespace _DAO.DAO
{
    public class DAO_TipoAuto : BaseDAO<TipoAuto>
    {
        private static DAO_TipoAuto instance;

        public static DAO_TipoAuto Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_TipoAuto();
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