using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;

namespace _DAO.DAO
{
    public class DAO_TipoInhabilitacion : BaseDAO<TipoInhabilitacion>
    {
        private static DAO_TipoInhabilitacion instance;

        public static DAO_TipoInhabilitacion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_TipoInhabilitacion();
                }
                return instance;
            }
        }

        public Resultado<TipoInhabilitacion> GetByKeyValue(Enums.TipoInhabilitacion keyValue)
        {
            var resultado = new Resultado<TipoInhabilitacion>();

            try
            {
                var entity = GetSession().QueryOver<TipoInhabilitacion>().Where(x => x.KeyValue == keyValue && x.FechaBaja == null).SingleOrDefault();
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