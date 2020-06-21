using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Tablas;

namespace WindowsFormsApp1.Analizadores
{
    class Parser_201700539
    {
        List<Token> tokens;
        List<Token> errores;
        Nodo arbolSintactico;
        int i;
        Token preanalisis;
        public List<Token> getErrores()
        {
            return this.errores;
        }
        public Nodo getArbol()
        {
            return this.arbolSintactico;
        }
        public Parser_201700539(List<Token> tokens, List<Token> Errores)
        {   //se deben eliminar los comentarios
            this.tokens = tokens;
            this.errores = Errores;
            this.i = 0;
            this.preanalisis = tokens[i];
            this.tokens.Add(new Token(Token.Tipo.dolar, "$", 0, 0));
            Nodo nodo1 = new Nodo("INICIO");
            INICIO(nodo1);
            this.arbolSintactico = nodo1;
        }
        private Token Match(Token.Tipo tk,String descripcion)
        { Token.Tipo tipo = preanalisis.TipoToken;
            if (tk.Equals(tipo))
            {
                Token tok = preanalisis;
                preanalisis = getNextToken();
                return tok;
            }
            else
            {
                Errores(descripcion+" y se obtuvo "+preanalisis.Lexema,preanalisis.Fila,preanalisis.Columna);
                Panic();
            }
            

            return null;
        }
        private void Panic()
        {
            //este metodo se encarga de evitar que el analisis se detenga por un error, lo recupera
            preanalisis = getNextToken();
            Token.Tipo tipo = preanalisis.TipoToken;
            while (!tipo.Equals(Token.Tipo.puntoycoma)){
                tipo = getNextToken().TipoToken;
            }
            //preanalisis = getNextToken();
        }
        private void Errores(String descripcion, int fila,int columna)
        {
            Token error = new Token(Token.TipoError.Sintactico, descripcion, fila, columna);
            errores.Add(error);

        }
        private Token getNextToken()
        {
            if (i != tokens.Count - 1)
            {
                i++;
            }
            return tokens[i];

        }

         private void INICIO(Nodo nodo)
        {
            Nodo nodo1 = new Nodo("INSTRUCCIONES");
            INSTRUCCIONES(nodo1);
            nodo.Agregar(nodo1);
        }
        private void INSTRUCCIONES(Nodo nodo)
        {
            Nodo nodo1 = new Nodo("INSTRUCCION");
            Nodo nodo2 = new Nodo("INSTRUCCIONES");
            INSTRUCCION(nodo1);
            if (!preanalisis.TipoToken.Equals(Token.Tipo.dolar))
            {

                INSTRUCCIONES(nodo2);
            }
            else
            {
                return;
            }
            nodo.Agregar(nodo1);
            nodo.Agregar(nodo2);
            
        }
        //private void INSTRUCIONES1()
        //{
        //    if (!preanalisis.TipoToken.Equals(Token.Tipo.dolar))
        //    {
        //        INSTRUCCIONES();
        //    }

        //}
        private void INSTRUCCION(Nodo nodo)
        {
            Nodo nodo1=null;
            Token tk=null;
            switch (preanalisis.TipoToken) {
                case Token.Tipo.crear:
                  tk= Match(Token.Tipo.crear, "Se esperaba palabra reservada Crear");
                    nodo1 = new Nodo("CREARTABLA");
                 CREARTABLA(nodo1);
                    break;
                case Token.Tipo.insertar:
                    tk=Match(Token.Tipo.insertar, "Se esperaba palabra reservada Insertar");
                    nodo1 = new Nodo("INSERTAR");
                    INSERTAR(nodo1);
                    break;
                case Token.Tipo.seleccionar:
                    tk=Match(Token.Tipo.seleccionar, "Se esperaba la palabra Reservada Seleccionar");
                    nodo1 = new Nodo("SELECCIONAR");
                    SELECCIONAR(nodo1);
                    break;
                case Token.Tipo.eliminar:
                    tk=Match(Token.Tipo.eliminar, "Se esperaba la palabra Reservada Eliminar");
                    nodo1 = new Nodo("ELIMINAR");
                    ELIMINAR(nodo1);
                    break;
                case Token.Tipo.actualizar:
                    tk=Match(Token.Tipo.actualizar, "Se esperaba la palabra Reservada Actualizar");
                    nodo1 = new Nodo("ACTUALIZAR");
                    ACTUALIZAR(nodo1);
                    break;
            }
            if (tk != null)
            {
                nodo.Agregar(new Nodo(tk.Lexema));
                nodo.Agregar(nodo1);
            }
        }
        private void ACTUALIZAR(Nodo nodo)
        {
            Token tk1 = Match(Token.Tipo.id, "Se esperaba un id");
            Token tk2 = Match(Token.Tipo.establecer, "Se esperaba la palabra Reservada Establecer");
            Token tk3 = Match(Token.Tipo.id, "Se esperaba un id");
            Token tk4 = Match(Token.Tipo.igual, "Se espeaba un =");
            Nodo ndv = new Nodo("VALOR");
            VALOR(ndv);
            Nodo nda = new Nodo("ACTUALIZA");
            ACTUALIZA(nda);
            Token tk5 = Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
            Nodo ndc = new Nodo("CONDICION");
            CONDICION(ndc);
            Nodo ndcs = new Nodo("CONDICIONES");
            CONDICIONES(ndcs);
            Token tk6 = Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            nodo.Agregar(new Nodo(tk1.Lexema));
            nodo.Agregar(new Nodo(tk2.Lexema));
            nodo.Agregar(new Nodo(tk3.Lexema));
            nodo.Agregar(new Nodo(tk4.Lexema));
            nodo.Agregar(nda);
            nodo.Agregar(new Nodo(tk5.Lexema));
            nodo.Agregar(ndc);
            nodo.Agregar(ndcs);
            nodo.Agregar(new Nodo(tk6.Lexema));
        }
        private void ACTUALIZA(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
               Token tk1= Match(Token.Tipo.coma, "Se esperaba una ,");
               Token tk2=Match(Token.Tipo.id, "Se esperaba un id");
               Token tk3= Match(Token.Tipo.igual, "Se esperaba un =");
                Nodo ndv = new Nodo("VALOR");
                VALOR(ndv);
                Nodo nda = new Nodo("ACTUALIZA");
                ACTUALIZA(nda);
                nd.Agregar(new Nodo(tk1.Lexema));
                nd.Agregar(new Nodo(tk2.Lexema));
                nd.Agregar(new Nodo(tk3.Lexema));
                nd.Agregar(ndv);
                nd.Agregar(nda);
            }
            else
            {
                nd.Agregar(new Nodo("ε"));
            }
        }
        private void ELIMINAR(Nodo nodo)
        {
            Token tk1=Match(Token.Tipo.R_de, "Se esperaba la palabra Reservada De");
            Token tk2=Match(Token.Tipo.id, "Se esperaba un id");
            Nodo nde1 = new Nodo("ELIMINAR1");
            ELIMINAR1(nde1);
            Token tk3=Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            nodo.Agregar(new Nodo(tk1.Lexema));
            nodo.Agregar(new Nodo(tk2.Lexema));
            nodo.Agregar(nde1);
        }
        private void ELIMINAR1(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.donde))
            {
                Token tk1=Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
                Nodo ndc = new Nodo("CONDICION");
                CONDICION(ndc);
                Nodo ndcs = new Nodo("CONDICIONES");
                CONDICIONES(ndcs);
                nd.Agregar(new Nodo(tk1.Lexema));
                nd.Agregar(ndc);
                nd.Agregar(ndcs);
            }
            else
            {
                //servira para la ejecucion, cuando venga Eliminar tal Tabla1;
                nd.Agregar(new Nodo("ε"));
            }
        }
        private void SELECCIONAR(Nodo nodo)
        {
            Nodo nds1 = new Nodo("SELECCIONAR1");
            SELECCIONAR1(nds1);
            Nodo ndst = new Nodo("SELECT");
            SELECT(ndst);
            Token tk1=Match(Token.Tipo.R_de, "Se esperaba la palabra reservada De");
            Token tk2=Match(Token.Tipo.id, "Se esperaba un id");
            Nodo tab = new Nodo("TABLA");
            TABLA(tab);
            Token tk3=Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
            Nodo ndc = new Nodo("CONDICION");
            CONDICION(ndc);
            Nodo ndcs = new Nodo("CONDICIONES");
            CONDICIONES(ndcs);
            Token tk4=Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            nodo.Agregar(nds1);
            nodo.Agregar(ndst);
            nodo.Agregar(new Nodo(tk1.Lexema));
            nodo.Agregar(new Nodo(tk2.Lexema));
            nodo.Agregar(tab);
            nodo.Agregar(new Nodo(tk3.Lexema));
            nodo.Agregar(ndc);
            nodo.Agregar(ndcs);
            nodo.Agregar(new Nodo(tk4.Lexema));
        }
        private void CONDICION(Nodo nd)
        {
            Nodo ndcp = new Nodo("COMP");
            COMP(ndcp);
            Nodo ndtc = new Nodo("TIPOCOMP");
            TIPOCOMP(ndtc);
            Nodo ndcp2 = new Nodo("COMP");
            COMP(ndcp2);
            nd.Agregar(ndcp);
            nd.Agregar(ndtc);
            nd.Agregar(ndcp2);
        }
        private void TIPOCOMP(Nodo nd)
        {
          
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.mayor:
                    Token tk=Match(Token.Tipo.mayor, "Se esperaba un >");
                    Nodo tc1 = new Nodo("TC1");
                    TC1(tc1);
                    nd.Agregar(new Nodo(tk.Lexema));
                    nd.Agregar(tc1);
                    break;
                case Token.Tipo.menor:
                    Token tk1=Match(Token.Tipo.menor, "Se esperaba <");
                    Nodo nd2 = new Nodo("TC1");
                    TC1(nd2);
                    nd.Agregar(new Nodo(tk1.Lexema));
                    nd.Agregar(nd2);
                    break;
                case Token.Tipo.diferente:
                    Token tok1=Match(Token.Tipo.diferente, "Se esperaba !");
                    Token tok2=Match(Token.Tipo.igual, "Se esperaba un =");
                    nd.Agregar(new Nodo(tok1.Lexema));
                    nd.Agregar(new Nodo(tok2.Lexema));
                    break;
                case Token.Tipo.igual:
                    Token tigual=Match(Token.Tipo.igual, "Se esperaba un =");
                    nd.Agregar(new Nodo(tigual.Lexema));
                    break;
                default:
                    Errores("Se esperaba un >,<,! o un = y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                    Panic();
                    break;
            }
        }
        private void TC1(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.igual))
            {
               Token tk= Match(Token.Tipo.igual, "Se esperaba un =");
                nd.Agregar(new Nodo(tk.Lexema));
            }
            else
            {
                nd.Agregar(new Nodo("ε"));
            }
        }
        private void COMP(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                Token tk1=Match(Token.Tipo.id, "Se esperaba un id");
                Token tk2=Match(Token.Tipo.punto, "Se esperaba un .");
                Token tk3=Match(Token.Tipo.id, "Se esperaba un id");
                nd.Agregar(new Nodo(tk1.Lexema));
                nd.Agregar(new Nodo(tk2.Lexema));
                nd.Agregar(new Nodo(tk3.Lexema));
            }
            else
            {
                Nodo ndv = new Nodo("VALOR");
                VALOR(ndv);
            }
        }
        private void CONDICIONES(Nodo nd)
        {
            
            if (preanalisis.TipoToken.Equals(Token.Tipo.R_Y))
            {
                Token tk=Match(Token.Tipo.R_Y, "Se esperaba la palabra reservada Y");
                Nodo ndc=new Nodo("CONDICION");
                CONDICION(ndc);
                Nodo ndcs = new Nodo("CONDICIONES");
                CONDICIONES(ndcs);
                nd.Agregar(new Nodo(tk.Lexema));
                nd.Agregar(ndc);
                nd.Agregar(ndcs);
            }else if (preanalisis.TipoToken.Equals(Token.Tipo.R_O))
            {
               Token tk= Match(Token.Tipo.R_O, "Se esperaba la palabra reservada O");
                Nodo ndc = new Nodo("CONDICION");
                CONDICION(ndc);
                Nodo ndcs = new Nodo("CONDICIONES");
                CONDICIONES(ndcs);
                nd.Agregar(new Nodo(tk.Lexema));
                nd.Agregar(ndc);
                nd.Agregar(ndcs);

            }
            else
            {
                nd.Agregar(new Nodo("ε"));
            }
        }
        private void TABLA(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Token tk1=Match(Token.Tipo.coma, "Se esperaba una ,");
                Token tk2=Match(Token.Tipo.id, "Se esperaba un id");
                Nodo ndt = new Nodo("TABLA");
                TABLA(ndt);
                nd.Agregar(new Nodo(tk1.Lexema));
                nd.Agregar(new Nodo(tk2.Lexema));
                nd.Agregar(ndt);
            }
            else
            {
                nd.Agregar(new Nodo("ε"));
            }
        }
        private void SELECCIONAR1(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                Token tk=Match(Token.Tipo.id, "Se esperaba un id");
                Token tk2=Match(Token.Tipo.punto, "Se esperaba un .");
                Nodo ndso = new Nodo("SELECTO");
                SELECTO(ndso);
                nd.Agregar(new Nodo(tk.Lexema));
                nd.Agregar(new Nodo(tk2.Lexema));
                nd.Agregar(ndso);
            }else if (preanalisis.TipoToken.Equals(Token.Tipo.asterisco))
            {
                Token tk=Match(Token.Tipo.asterisco, "Se esperaba un *");
                nd.Agregar(new Nodo(tk.Lexema));
            }
            else
            {
                Errores("Se esperaba un id o un * y se obtuvo "+preanalisis.Lexema,preanalisis.Fila,preanalisis.Columna);
                Panic();
            }
        }
        private void SELECTO(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                Token tk = Match(Token.Tipo.id, "Se esperaba un id");
                nd.Agregar(new Nodo(tk.Lexema));
            }
            else if(preanalisis.TipoToken.Equals(Token.Tipo.asterisco))
            {
                Token tk = Match(Token.Tipo.asterisco, "Se esperaba un *");
                nd.Agregar(new Nodo(tk.Lexema));
            }
            else
            {
                Errores("Se esperaba un id o un * y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                Panic();
            }
        }
        private void SELECT(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.como))
            {
                Token tk=Match(Token.Tipo.como, "Se esperaba la palabra reservada Como");
                Token tk1=Match(Token.Tipo.id, "Se esperaba un id");
                nd.Agregar(new Nodo(tk.Lexema));
                nd.Agregar(new Nodo(tk1.Lexema));
            }
            else
            {
                nd.Agregar(new Nodo("ε"));
            }

        }
        private void TABLAS()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                Match(Token.Tipo.id, "Se esperaba un id");
                Match(Token.Tipo.punto, "Se esperaba un .");
                Match(Token.Tipo.id, "Se esperaba un id");
                Match(Token.Tipo.como, "Se esperaba la palabra Reservada Como");
                Match(Token.Tipo.id, "Se esperaba un id");
                TABLAS();
            }
        }
        private void INSERTAR(Nodo nodo)
        {
           Token tk1= Match(Token.Tipo.en, "Se esperaba la palabra Reservada En");
            Token tk2=Match(Token.Tipo.id, "Se esperaba un id ");
            Token tk3=Match(Token.Tipo.valores, "Se esperaba la palabra Reservada Valores");
            Token tk4=Match(Token.Tipo.parAbre, "Se esperaba un (");
            Nodo ndv = new Nodo("VALOR");
            VALOR(ndv);
            Nodo ndvs = new Nodo("VALORES");
            VALORES(ndv);
            Token tk5=Match(Token.Tipo.parCierra, "Se esperaba un )");
            Token tk6=Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            nodo.Agregar(new Nodo(tk1.Lexema));
            nodo.Agregar(new Nodo(tk2.Lexema));
            nodo.Agregar(new Nodo(tk3.Lexema));
            nodo.Agregar(new Nodo(tk4.Lexema));
            nodo.Agregar(ndv);
            nodo.Agregar(ndvs);
            nodo.Agregar(new Nodo(tk5.Lexema));
            nodo.Agregar(new Nodo(tk6.Lexema));
            
        }
        private void VALOR(Nodo nd)
        {
            Token tk = null;
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.entero:
                    tk=Match(Token.Tipo.entero, "Se esperaba un número tipo entero");
                    break;
                case Token.Tipo.flotante:
                    tk=Match(Token.Tipo.flotante, "Se esperaba un número tipo flotante");
                    break;
                case Token.Tipo.cadena:
                    tk=Match(Token.Tipo.cadena, "Se esperaba un valor tipo cadena");
                    break;
                case Token.Tipo.fecha:
                    tk=Match(Token.Tipo.fecha, "Se esperaba un valor tipo fecha");
                    break;
                default:
                    Errores("Se esperaba un valor tipo entero, flotante, fecha o cadena y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                    Panic();
                    break;
            }
            if (tk != null)
            {
                nd.Agregar(new Nodo(tk.Lexema));
            }
        }
        private void VALORES(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
               Token tk= Match(Token.Tipo.coma, "Se esperaba una ,");
                Nodo ndv = new Nodo("VALOR");
                VALOR(ndv);
                Nodo ndvs = new Nodo("VALORES");
                VALORES(ndvs);
            }
            else
            {
                nd.Agregar(new Nodo("ε"));

            }
        }
         private void CREARTABLA(Nodo nodo)
        {            
           Token tk1= Match(Token.Tipo.tabla, "Se esperaba la palabra Reservada Tabla");
           Token tk2=Match(Token.Tipo.id, "Se esperaba un id");
           Token tk3=Match(Token.Tipo.parAbre, "Se esperaba un (");
           Token tk4= Match(Token.Tipo.id, "Se esperaba un id");
            Nodo ndt = new Nodo("TIPO");
            TIPO(ndt);
            Nodo ndc = new Nodo("CAMPOS");
            CAMPOS(ndc);
           Token tk5= Match(Token.Tipo.parCierra, "Se esperaba un )");
           Token tk6= Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            nodo.Agregar(new Nodo(tk1.Lexema));
            nodo.Agregar(new Nodo(tk2.Lexema));
            nodo.Agregar(new Nodo(tk3.Lexema));
            nodo.Agregar(new Nodo(tk4.Lexema));
            nodo.Agregar(ndt);
            nodo.Agregar(ndc);            
            nodo.Agregar(new Nodo(tk5.Lexema));
            nodo.Agregar(new Nodo(tk6.Lexema));
        }
        private void CAMPOS(Nodo nd)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Token tk1=Match(Token.Tipo.coma, "Se esperaba una ,");
                Token tk2=Match(Token.Tipo.id, "Se esperaba un id ");
                nd.Agregar(new Nodo(tk1.Lexema));
                nd.Agregar(new Nodo(tk2.Lexema));
                Nodo ndt = new Nodo("TIPO");
                TIPO(ndt);
                Nodo ndc = new Nodo("CAMPOS");
                CAMPOS(ndc);
                nd.Agregar(ndt);
                nd.Agregar(ndc);
                
            }
            else
            {
               nd.Agregar(new Nodo("ε"));
            }
           
        }
        private void TIPO(Nodo nd)
        {
            Token tk = null;
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.R_entero:
                    tk=Match(Token.Tipo.R_entero, "Se espera la palabra reservada entero");
                    break;
                case Token.Tipo.R_flotante:
                    tk=Match(Token.Tipo.R_flotante, "Se espera la palabra reservada flotante");
                    break;
                case Token.Tipo.R_cadena:
                    tk=Match(Token.Tipo.R_entero, "Se espera la palabra reservada cadena");
                    break;
                case Token.Tipo.R_fecha:
                    tk=Match(Token.Tipo.R_fecha, "Se espera la palabra reservada fecha");
                    break;
                default:
                    Errores("Se espera la palabra reservada entero, flotante, fecha o cadena y se obtuvo "+preanalisis.Lexema,preanalisis.Fila,preanalisis.Columna);
                    Panic();
                    break;
            }
            if (tk != null)
            {
                nd.Agregar(new Nodo(tk.Lexema));
            }
        }

    }
   
}
