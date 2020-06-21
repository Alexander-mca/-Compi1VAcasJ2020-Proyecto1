using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Analizadores;

namespace WindowsFormsApp1.Tablas
{
    class Tabla
    {
        public class Columna
        {
            String id;         
            List<Token> datos = new List<Token>();
            public Columna(String id)
            {
                this.id = id;
               
            }

            public string Id { get => id; set => id = value; }
            public List<Token> Datos { get => datos; set => datos = value; }
        }
        String id;
        List<Columna> columnas = new List<Columna>();

        public String getId()
        {
            return this.id;
        }
        public List<Columna> getColumnas()
        {
            return this.columnas;
        }
        public Columna getColumna(String id)
        {
            foreach (Columna item in columnas)
            {
                if (item.Id.Equals(id))
                {
                    return item;
                }
            }
            return null;
        }
        public Tabla(List<String> columnas,String id)
        {
            this.id = id;
            foreach (String item in columnas)
            {
               Columna colum = new Columna(item);
                this.columnas.Add(colum);

            }
            
            
        }
        public void Insertar(List<Token> dat)
        {
            if (dat.Count == columnas.Count)
            {
                int i = 0;
                foreach(Columna colum in this.columnas)
                {   Token valor = dat[i];
                    colum.Datos.Add(valor);

                }
            }
        }

        public void Eliminar()
        {

        }
        public void Actualizar()
        {

        }
        public void Seleccionar(List<String> data)
        {
            if (data.Count == 1) { 
}
        }
    }
}
