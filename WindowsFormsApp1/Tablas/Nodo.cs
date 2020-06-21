using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Tablas
{
    class Nodo
    {
        String valor;
        List<Nodo> hijos;
        int id;
        public String getValor()
        {
            return this.valor;
        }
        public int getId()
        {
            return this.id;
        }
        public void setId(int id)
        {
            this.id = id;
        }
        public List<Nodo> getHijos()
        {
            return this.hijos;
        }
        public bool TieneHijos()
        {
            if (this.hijos.Count != 0)
            {
                return true;
            }
            return false;
        }
        public Nodo(String valor)
        {
            this.valor = valor;
            this.hijos = new List<Nodo>();
        }

        public void Agregar(Nodo hijo)
        {
            this.hijos.Add(hijo);
        }

       



    }
}
