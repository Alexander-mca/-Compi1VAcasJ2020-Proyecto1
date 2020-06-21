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
        Object valizq, valder;
        String operando;

        public Condicion(Object valizq, Object valder, string operando)
        {
            this.valizq = valizq;
            this.valder = valder;
            this.operando = operando;
        }

        public bool Ejecutar()
        {   
            if(valizq is Columna && valder is Columna)
            {
                Columna val1 = (Columna)valizq;
                Columna val2 = (Columna)valder;
                for (int i = 0; i <val1.Datos.Count; i++)
                {
                    Token tk1 = val1.Datos[i];
                    for (int j = 0; j < val2.Datos.Count; j++)
                    {
                        Token tk2 = val2.Datos[j];
                        bool validar = ObtenerVal(tk1, tk2);
                        if (validar)
                        {

                        }
                    }
                }

                return true;
            }
            //if(valizq is Columna)
            //{
            //    for (int i = 0; i < length; i++)
            //    {

            //    }
            //}
            //if(valder is Columna)
            //{
            //    for (int i = 0; i < length; i++)
            //    {

            //    }
            //}
            
            return false;
        } 

        public bool ObtenerVal(Token valizq,Token valder)
        {
            
            switch (valizq.TipoToken)
            {
                case Token.Tipo.entero:
                    if (valder.TipoToken.Equals(Token.Tipo.entero))
                    {
                        int valor1 = int.Parse(valizq.Lexema);
                        int valor2 = int.Parse(valder.Lexema);
                        switch (operando)
                        {
                            case ">":
                                if (valor1 > valor2)
                                    return true;
                                break;
                            case "<":
                                if (valor1 < valor2)
                                    return true;
                                break;
                            case ">=":
                                if (valor1 >= valor2)
                                    return true;
                                break;
                            case "<=":
                                if (valor1 <= valor2)
                                    return true;
                                break;
                            case "=":
                                if (valor1 == valor2)
                                    return true;
                                break;
                            case "!=":
                                if (valor1 != valor2)
                                    return true;
                                break;

                        }
                    }
                    break;
                case Token.Tipo.flotante:
                    if (valder.TipoToken.Equals(Token.Tipo.flotante))
                    {
                        double valor1 = double.Parse(valizq.Lexema);
                        double valor2 = double.Parse(valder.Lexema);
                        switch (operando)
                        {
                            case ">":
                                if (valor1 > valor2)
                                    return true;
                                break;
                            case "<":
                                if (valor1 < valor2)
                                    return true;
                                break;
                            case ">=":
                                if (valor1 >= valor2)
                                    return true;
                                break;
                            case "<=":
                                if (valor1 <= valor2)
                                    return true;
                                break;
                            case "=":
                                if (valor1 == valor2)
                                    return true;
                                break;
                            case "!=":
                                if (valor1 != valor2)
                                    return true;
                                break;

                        }

                    }
                    break;
                case Token.Tipo.fecha:
                    if (valder.TipoToken.Equals(Token.Tipo.fecha))
                    {

                        if (operando.Equals("="))
                        {
                            if (valizq.Lexema.Equals(valder.Lexema))
                                return true;
                        }
                        else if (operando.Equals("!="))
                        {
                            if (!valizq.Lexema.Equals(valder.Lexema))
                                return true;
                        }
                    }
                    break;
                case Token.Tipo.cadena:
                    if (valder.TipoToken.Equals(Token.Tipo.cadena))
                    {

                        if (operando.Equals("="))
                        {
                            if (valizq.Lexema.Equals(valder.Lexema))
                                return true;
                        }
                        else if (operando.Equals("!="))
                        {
                            if (!valizq.Lexema.Equals(valder.Lexema))
                                return true;
                        }
                    }

                    break;

            }
            return false;
        }
        

       
    }
}
