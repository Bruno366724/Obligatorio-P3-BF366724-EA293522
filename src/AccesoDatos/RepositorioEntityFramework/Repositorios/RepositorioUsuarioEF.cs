using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;
using Dominio.InterfacesRepositorios;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioUsuarioEF : IRepositorioUsuario
    {
        public void Add(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> FindAll()
        {
            throw new NotImplementedException();
        }

        public Usuario FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario FindByNombreUsuario(string nombreUsuario)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Usuario obj)
        {
            throw new NotImplementedException();
        }
    }
}
