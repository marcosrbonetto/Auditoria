using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;

namespace _DAO.DAO
{
    public class DAO_TipoInscripcion : BaseDAO<TipoInscripcion>
    {
        private static DAO_TipoInscripcion instance;

        public static DAO_TipoInscripcion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_TipoInscripcion();
                }
                return instance;
            }
        }



        public Resultado<TipoInscripcion> GetByKeyValue(Enums.TipoInscripcion keyValue)
        {
            var resultado = new Resultado<TipoInscripcion>();

            try
            {
                var entity = GetSession().QueryOver<TipoInscripcion>().Where(x => x.KeyValue == keyValue && x.FechaBaja == null).SingleOrDefault();
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