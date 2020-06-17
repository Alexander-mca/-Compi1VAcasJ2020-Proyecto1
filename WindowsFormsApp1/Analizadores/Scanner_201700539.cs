using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Analizadores
{
    class Scanner_201700539
    {
        List<Token> ListaTokens = new List<Token>();
        List<Token> Errores = new List<Token>();
        public List<Token> getErrores()
        {
            return this.Errores;
        }
        public List<Token> getListaDeTokens()
        {
            return this.ListaTokens;
        }
        public Scanner_201700539(List<Token> ListaTokens,String doc)
        {
            int estado = 0;
            String lexema="";
            int fila=1, columna = 1;
            for  (int i=0;i<doc.Length;i++)
            {
                char c = doc[i];
                Token tk;
                switch (estado)
                {
                    case 0:
                        if (Char.IsLetter(c))
                        {
                            estado = 5;
                            lexema += c;
                        }
                        else if (Char.IsDigit(c))
                        {
                            estado = 4;
                            lexema += c;
                        } else if (c == '\n')
                        {
                            fila++;
                            columna = 1;
                        } else if (c == ' ' || c == '\b' || c == '\r' || c == '\t')
                        {

                        }
                        else
                        {
                            switch (c)
                            {
                                case '-':
                                    lexema += c;
                                    estado = 1;
                                    break;
                                case '/':
                                    lexema += c;
                                    estado = 2;
                                    break;
                                case '\'':
                                    lexema += c;
                                    estado = 6;
                                    break;
                                case '"':
                                    estado = 3;
                                    lexema += c;
                                    break;
                                case '(':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.parAbre, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case ')':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.parCierra, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case ',':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.coma, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case ';':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.puntoycoma, lexema, fila, columna);
                                    lexema = "";
                                    break;

                                case '.':                                    
                                        lexema += c;
                                        estado = 0;
                                        tk = new Token(Token.Tipo.punto, lexema, fila, columna);
                                        lexema = "";                                    
                                    break;
                                case '*':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.asterisco, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case '<':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.menor, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case '>':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.mayor, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case '=':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.igual, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                case '!':
                                    lexema += c;
                                    estado = 0;
                                    tk = new Token(Token.Tipo.diferente, lexema, fila, columna);
                                    lexema = "";
                                    break;
                                default:
                                    tk = new Token(Token.TipoError.Lexico, "El caracter " + c + ", no es parte del lenguaje.", fila, columna);
                                    Errores.Add(tk);
                                    estado = 0;
                                    break;
                            }
                        }
                        break;
                    case 1:
                        if (c == '-')
                        {
                            estado = 8;
                            lexema += c;
                        }
                        break;
                    case 2:
                        if (c == '*')
                        {
                            estado = 9;
                            lexema += c;
                        }
                        break;
                    case 3:
                        if (c == '"')
                        {
                            tk = new Token(Token.Tipo.cadena, lexema, fila, columna);
                            ListaTokens.Add(tk);
                            estado = 0;
                            lexema = "";
                            continue;
                        }
                        lexema += c;
                        estado = 3;
                        break;
                    case 4:
                        if (!(Char.IsDigit(c) || c == '.'))
                        {
                            lexema += c;
                            estado = 0;
                            tk = new Token(Token.Tipo.entero, lexema, fila, columna);
                            ListaTokens.Add(tk);
                            lexema = "";
                            i--;
                            continue;
                        }

                        if (c == '.')
                        {
                            lexema += c;
                            estado = 11;
                        } else if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 4;
                        }

                        break;
                    case 5:
                        if (!(Char.IsLetterOrDigit(c) || c == '_'))
                        {
                            lexema += c;
                            estado = 0;
                            tk = Reservadas(lexema,fila,columna);
                            ListaTokens.Add(tk);
                            lexema = "";
                            i--;
                            continue;
                        }
                        lexema += c;
                        break;
                    case 6:
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 12;
                        }

                        break;
                    case 8:
                        if (c != '\n')
                        {
                            lexema += c;
                            estado = 8;
                            continue;
                        }
                        lexema += c;
                         tk = new Token(Token.Tipo.ComentLinea, lexema, fila, columna);
                        ListaTokens.Add(tk);
                        lexema = "";
                        estado = 0;
                        break;
                    case 9:
                        if (c == '*')
                        {
                            estado = 13;
                            lexema += c;
                            continue;
                        }
                        lexema += c;
                        estado = 9;

                        break;
                  
                    case 11:
                        
                        if (!Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 0;
                            tk = new Token(Token.Tipo.flotante, lexema, fila, columna);
                            ListaTokens.Add(tk);
                            lexema = "";
                            i--;
                            continue;

                        }
                        
                        lexema += c;
                        estado = 11;
                        break;
                    case 12:
                        if (Char.IsDigit(c))
                        {
                            estado = 15;
                            lexema += c;
                        }

                        break;
                    case 13:
                        if (c == '/')
                        {
                            estado = 0;
                            lexema += c;
                            tk = new Token(Token.Tipo.ComentMult, lexema, fila, columna);
                            ListaTokens.Add(tk);
                            lexema = "";
                        }
                        break;
                    case 15:
                        if (c == '/')
                        {
                            lexema += c;
                            estado = 16;
                        }
                        break;
                    case 16:
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 17;
                        }
                        break;
                    case 17:
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 18;
                        }
                        break;
                    case 18:
                        if (c == '/')
                        {
                            lexema += c;
                            estado = 19;
                        }
                        break;
                    case 19:
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 20;
                        }
                        break;
                    case 20:
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 20;
                        }
                        if (c == '\'')
                        {
                            lexema += c;
                            tk = new Token(Token.Tipo.fecha, lexema, fila, columna);
                            ListaTokens.Add(tk);
                            lexema = "";
                            estado = 0;
                        }
                        break;

                }
                columna++;
            }
            this.ListaTokens = ListaTokens;
        }

      

        private Token Reservadas(String lexema,int fila,int columna)
        {
            String reservada = lexema.ToLower();
            Token tk;
            switch (reservada)
            {
                case "crear":
                    tk = new Token(Token.Tipo.crear, lexema, fila, columna);
                    break;
                case "tabla":
                    tk = new Token(Token.Tipo.tabla, lexema, fila, columna);
                    break;
                case "insertar":
                    tk = new Token(Token.Tipo.insertar, lexema, fila, columna);
                    break;
                case "en":
                    tk = new Token(Token.Tipo.en, lexema, fila, columna);
                    break;
                case "valores":
                    tk = new Token(Token.Tipo.valores, lexema, fila, columna);
                    break;
                case "entero":
                    tk = new Token(Token.Tipo.R_entero, lexema, fila, columna);
                    break;
                case "flotante":
                    tk = new Token(Token.Tipo.R_flotante, lexema, fila, columna);
                    break;
                case "fecha":
                    tk = new Token(Token.Tipo.R_fecha, lexema, fila, columna);
                    break;
                case "cadena":
                    tk = new Token(Token.Tipo.cadena, lexema, fila, columna);
                    break;
                case "seleccionar":
                    tk = new Token(Token.Tipo.seleccionar, lexema, fila, columna);
                    break;
                case "de":
                    tk = new Token(Token.Tipo.R_de, lexema, fila, columna);
                    break;
                case "donde":
                    tk = new Token(Token.Tipo.donde, lexema, fila, columna);
                    break;
                case "como":
                    tk = new Token(Token.Tipo.como, lexema, fila, columna);
                    break;
                case "y":
                    tk = new Token(Token.Tipo.R_Y, lexema, fila, columna);
                    break;
                case "o":
                    tk = new Token(Token.Tipo.R_O, lexema, fila, columna);
                    break;
                case "eliminar":
                    tk = new Token(Token.Tipo.eliminar, lexema, fila, columna);
                    break;
                case "actualizar":
                    tk = new Token(Token.Tipo.actualizar, lexema, fila, columna);
                    break;
                case "establecer":
                    tk = new Token(Token.Tipo.establecer, lexema, fila, columna);
                    break;
                default:
                    tk = new Token(Token.Tipo.id, lexema, fila, columna);
                    break;
                    
            }

            return tk;
        }
    }
}
