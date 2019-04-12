using System;
using System.Linq;
using _Model;
using _Model.Entities;
using NHibernate;
using System.Collections.Generic;

namespace _DAO.DAO
{
    public class DAO_TipoCondicionInscripcion : BaseDAO<TipoCondicionInscripcion>
    {
        private static DAO_TipoCondicionInscripcion instance;

        public static DAO_TipoCondicionInscripcion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAO_TipoCondicionInscripcion();
                }
                return instance;
            }
        }

        public Resultado<TipoCondicionInscripcion> GetByKeyValue(Enums.TipoCondicionInscripcion keyValue)
        {
            var resultado = new Resultado<TipoCondicionInscripcion>();

            try
            {
                var entity = GetSession().QueryOver<TipoCondicionInscripcion>().Where(x => x.KeyValue == keyValue && x.FechaBaja == null).SingleOrDefault();
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