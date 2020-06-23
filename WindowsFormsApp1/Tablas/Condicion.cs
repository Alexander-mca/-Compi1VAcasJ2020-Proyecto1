using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Analizadores;
using static WindowsFormsApp1.Tablas.Tabla;

namespace WindowsFormsApp1.Tablas
{
    class Condicion
    {
        Tabla tabla1, tabla2;
        List<String> columnas;
        String operando;
        Object valizq, valder;

        public Condicion(List<String> columnas, Tabla tabla1, Tabla tabla2, Object valizq, Object valder, string operando)
        {
            this.tabla1 = tabla1;
            this.tabla2 = tabla2;
            this.columnas = columnas;
            this.valizq = valizq;
            this.valder = valder;
            this.operando = operando;
        }

        public Tabla Ejecutar()
        {
            Tabla tabla = new Tabla(this.columnas);
            if (!(valizq is Columna))
            {
                //se compara valores de un token con una columna
                Columna aux = (Columna)valder;
                Token valor = (Token)valizq;
                for (int k = 0; k < aux.Datos.Count; k++)
                {
                    Token valcelda = aux.Datos[k];
                    bool val = Validar(valcelda, valor);
                    if (val)
                    {
                        InsertarValores(tabla, this.tabla2, k);
                    }
                }
                return tabla;
            }
            if (!(valder is Columna))
            {
                //se comparan valores de una columna con un token
                Columna aux = (Columna)valizq;
                Token valor = (Token)valder;
                for (int k = 0; k <aux.Datos.Count; k++)
                {
                    Token valcelda = aux.Datos[k];
                    bool val = Validar(valcelda, valor);
                    if (val)
                    {
                        InsertarValores(tabla, this.tabla1, k);
                    }
                }
                return tabla;
            }
            //cuando ambos son columnas
            Columna columna1 = (Columna)valizq;
            Columna columna2 = (Columna)valder;
            //List<Columna> tabla1 = this.tabla1.getColumnas();
            //List<Columna> tabla2 = this.tabla2.getColumnas();
            int i = 0;
            int j = 0;
            foreach (Token tk1 in columna1.Datos)
            {
                foreach (Token tk2 in columna2.Datos)
                {
                    bool valor = Validar(tk1, tk2);
                    if (valor)
                    {

                        InsertarValores(tabla, this.tabla1, i);
                        InsertarValores(tabla, this.tabla2, j);
                    }
                    j++;
                }
                i++;
            }
            return tabla;

        }

        private void InsertarValores(Tabla table,Tabla tabla2,int j)
        {
            foreach (String columna in this.columnas)
            {
                Columna colum = tabla2.getColumna(columna);
                if (colum != null)
                {
                    table.getColumna(columna).Datos.Add(colum.Datos[j]);

                }
            }
        }

        private bool Validar(Token tk,Token tk2)
        {

            switch (tk.TipoToken) 
            {
                case Token.Tipo.entero:
                    int a = int.Parse(tk.Lexema);
                    switch (tk2.TipoToken)
                    {
                        case Token.Tipo.entero:
                            int b = int.Parse(tk2.Lexema);
                            return Operar(a, b);                            
                        case Token.Tipo.flotante:
                            double b2 = double.Parse(tk2.Lexema);
                            int bc = (int)b2;
                           return Operar(a, bc);
                        default:
                            Console.WriteLine("Error Semantico. No se puede operar un valor tipo entero\ncon un valor tipo fecha o cadena.\nTipoToken1:" + tk.TipoToken.ToString() + "/TokenTipo2:" + tk2.TipoToken.ToString());
                            break;
                    }
                    break;
                case Token.Tipo.flotante:
                    double a2 = double.Parse(tk.Lexema);
                    switch (tk2.TipoToken)
                    {
                        case Token.Tipo.entero:
                            int b = int.Parse(tk2.Lexema);
                            double b2 = (double)b;
                            return OperarFlotante(a2, b2);
                        case Token.Tipo.flotante:
                            double b3 = double.Parse(tk2.Lexema);                            
                           return OperarFlotante(a2, b3);
                        default:
                            Console.WriteLine("Error Semantico. No se puede operar un valor tipo flotante\ncon un valor tipo fecha o cadena.\nTipoToken1:"+tk.TipoToken.ToString()+"/TokenTipo2:"+tk2.TipoToken.ToString());
                            break;
                    }
                    break;
                case Token.Tipo.cadena:
                    if (!tk2.TipoToken.Equals(Token.Tipo.cadena))
                    {
                        Console.WriteLine("Error Semantico.Los valores no pueden se comparados.\nTipoToken1:" + tk.TipoToken.ToString() + "/TokenTipo2:" + tk2.TipoToken.ToString());
                        break;
                    }
                    if (operando.Equals("="))
                    {
                        if (tk.Lexema.Equals(tk2.Lexema))
                        {
                            return true;
                        }
                    }else if (operando.Equals("!="))
                    {
                        if (!tk.Lexema.Equals(tk2.Lexema))
                        {
                            return true;
                        }
                    }
                    break;
                case Token.Tipo.fecha:
                    if (!tk2.TipoToken.Equals(Token.Tipo.fecha))
                    {
                        Console.WriteLine("Error Semantico.Los valores no pueden se comparados.\nTipoToken1:" + tk.TipoToken.ToString() + "/TokenTipo2:" + tk2.TipoToken.ToString());
                        break;
                    }
                    if (operando.Equals("="))
                    {
                        if (tk.Lexema.Equals(tk2.Lexema))
                        {
                            return true;
                        }
                    }
                    else if (operando.Equals("!="))
                    {
                        if (!tk.Lexema.Equals(tk2.Lexema))
                        {
                            return true;
                        }
                    }

                    break;
            }
            return false;
        }

        private bool Operar(int a,int b)
        {
            switch (operando)
            {
                case "<":
                    if (a < b) return true;
                    break;
                case ">":
                    if (a > b) return true;
                    break;
                case "=":
                    if (a == b) return true;
                    break;
                case "!=":
                    if (a != b) return true;
                    break;
                case ">=":
                    if (a >= b) return true;
                    break;
                case "<=":
                    if (a <= b) return true;
                    break;
                    
            }
            return false;
        }

        private bool OperarFlotante(double a, double b)
        {
            switch (operando)
            {
                case "<":
                    if (a < b) return true;
                    break;
                case ">":
                    if (a > b) return true;
                    break;
                case "=":
                    if (a == b) return true;
                    break;
                case "!=":
                    if (a != b) return true;
                    break;
                case ">=":
                    if (a >= b) return true;
                    break;
                case "<=":
                    if (a <= b) return true;
                    break;

            }
            return false;
        }





    }
}
