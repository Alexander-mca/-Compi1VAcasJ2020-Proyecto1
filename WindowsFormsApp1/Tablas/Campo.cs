using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Analizadores;

namespace WindowsFormsApp1.Tablas
{
    class Campo
    {
        public Token valor,campo;

        public Campo(Token valor, Token campo)
        {
            this.valor = valor;
            this.campo = campo;
        }
    }
}
