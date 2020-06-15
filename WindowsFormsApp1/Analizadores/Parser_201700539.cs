using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Analizadores
{
    class Parser_201700539
    {
        List<Token> tokens = new List<Token>();
        List<Token> errores = new List<Token>();
        int i;
        Token preanalisis;
        public Parser_201700539(List<Token> tokens,List<Token> Errores)
        {
            this.tokens = tokens;
            this.errores = Errores;
            this.preanalisis = tokens[i];
            this.tokens.Add(new Token(Token.Tipo.dolar, "$", 0, 0));
            this.i = 0;
            INICIO();
        }
        public Token.Tipo Match(Token.Tipo tk,String descripcion)
        { Token.Tipo tipo = preanalisis.TipoToken;
            if (tk.Equals(tipo))
            {
                preanalisis = getNextToken();               
            }
            else
            {
                Errores(descripcion+" y se obtuvo "+preanalisis.Lexema,preanalisis.Fila,preanalisis.Columna);
                Panic();
            }
            

            return tipo;
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

         public void INICIO()
        {
            INSTRUCCIONES();
        }
        private void INSTRUCCIONES()
        {
            INSTRUCCION();
            INSTRUCIONES1();
        }
        private void INSTRUCIONES1()
        {
            if (!preanalisis.TipoToken.Equals(Token.Tipo.dolar))
            {
                INSTRUCCIONES();
            }
            
        }
        private void INSTRUCCION()
        {
            switch (preanalisis.TipoToken) {
                case Token.Tipo.crear:
                    Match(Token.Tipo.crear, "Se esperaba palabra reservada Crear");
                 CREARTABLA();
                    break;
                case Token.Tipo.insertar:
                    Match(Token.Tipo.insertar, "Se esperaba palabra reservada Insertar");
                    INSERTAR();
                    break;
                case Token.Tipo.seleccionar:
                    Match(Token.Tipo.seleccionar, "Se esperaba la palabra Reservada Seleccionar");
                    SELECCIONAR();
                    break;
                case Token.Tipo.eliminar:
                    Match(Token.Tipo.eliminar, "Se esperaba la palabra Reservada Eliminar");
                    ELIMINAR();
                    break;
                case Token.Tipo.actualizar:
                    Match(Token.Tipo.actualizar, "Se esperaba la palabra Reservada Actualizar");
                    ACTUALIZAR();
                    break;
            }
        }
        private void ACTUALIZAR()
        {
            Match(Token.Tipo.id, "Se esperaba un id");
            Match(Token.Tipo.establecer, "Se esperaba la palabra Reservada Establecer");
            Match(Token.Tipo.id, "Se esperaba un id");
            Match(Token.Tipo.igual, "Se espeaba un =");
            VALOR();
            ACTUALIZA();
            Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
            CONDICION();
            CONDICIONES();
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
        }
        private void ACTUALIZA()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                Match(Token.Tipo.id, "Se esperaba un id");
                Match(Token.Tipo.igual, "Se esperaba un =");
                VALOR();
                ACTUALIZA();
            }
        }
        private void ELIMINAR()
        {
            Match(Token.Tipo.R_de, "Se esperaba la palabra Reservada De");
            Match(Token.Tipo.id, "Se esperaba un id");
            ELIMINAR1();
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");

        }
        private void ELIMINAR1()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.donde))
            {
                Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
                CONDICION();
                CONDICIONES();
            }
            else
            {
                //servira para la ejecucion, cuando venga Eliminar tal Tabla1;
            }
        }
        private void SELECCIONAR()
        {
            SELECCIONAR1();
            SELECT();
            Match(Token.Tipo.R_de, "Se esperaba la palabra reservada De");
            Match(Token.Tipo.id, "Se esperaba un id");
            TABLA();
            Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
            CONDICION();
            CONDICIONES();
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");

        }
        private void CONDICION()
        {
            COMP();
            TIPOCOMP();
            COMP();
        }
        private void TIPOCOMP()
        {
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.mayor:
                    Match(Token.Tipo.mayor, "Se esperaba un >");
                    TC1();
                    break;
                case Token.Tipo.menor:
                    Match(Token.Tipo.menor, "Se esperaba <");
                    TC1();
                    break;
                case Token.Tipo.diferente:
                    Match(Token.Tipo.diferente, "Se esperaba !");
                    Match(Token.Tipo.igual, "Se esperaba un =");
                    break;
                case Token.Tipo.igual:
                    Match(Token.Tipo.igual, "Se esperaba un =");
                    break;
                default:
                    Errores("Se esperaba un >,<,! o un = y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                    Panic();
                    break;
            }
        }
        private void TC1()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.igual))
            {
                Match(Token.Tipo.igual, "Se esperaba un =");
            }
        }
        private void COMP()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                Match(Token.Tipo.id, "Se esperaba un id");
            }
            else
            {
                VALOR();
            }
        }
        private void CONDICIONES()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.R_Y))
            {
                Match(Token.Tipo.R_Y, "Se esperaba la palabra reservada Y");
                CONDICION();
                CONDICIONES();
            }else if (preanalisis.TipoToken.Equals(Token.Tipo.R_O))
            {
                Match(Token.Tipo.R_O, "Se esperaba la palabra reservada O");
                CONDICION();
                CONDICIONES();
            }
        }
        private void TABLA()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                Match(Token.Tipo.id, "Se esperaba un id");
                TABLA();
            }
        }
        private void SELECCIONAR1()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                Match(Token.Tipo.id, "Se esperaba un id");
                Match(Token.Tipo.punto, "Se esperaba un punto");
                Match(Token.Tipo.id, "Se esperaba un id");
            }else if (preanalisis.TipoToken.Equals(Token.Tipo.asterisco))
            {
                Match(Token.Tipo.asterisco, "Se esperaba un *");
            }
            else
            {
                Errores("Se esperaba un id o un * y se obtuvo "+preanalisis.Lexema,preanalisis.Fila,preanalisis.Columna);
                Panic();
            }
        }
        private void SELECT()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.como))
            {
                Match(Token.Tipo.como, "Se esperaba la palabra reservada Como");
                Match(Token.Tipo.id, "Se esperaba un id");
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
        private void INSERTAR()
        {
            Match(Token.Tipo.en, "Se esperaba la palabra Reservada En");
            Match(Token.Tipo.id, "Se esperaba un id ");
            Match(Token.Tipo.valores, "Se esperaba la palabra Reservada Valores");
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            VALOR();
            VALORES();
            Match(Token.Tipo.parCierra, "Se esperaba un )");
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
           
        }
        private void VALOR()
        {
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.entero:
                    Match(Token.Tipo.entero, "Se esperaba un número tipo entero");
                    break;
                case Token.Tipo.flotante:
                    Match(Token.Tipo.flotante, "Se esperaba un número tipo flotante");
                    break;
                case Token.Tipo.cadena:
                    Match(Token.Tipo.cadena, "Se esperaba un valor tipo cadena");
                    break;
                case Token.Tipo.fecha:
                    Match(Token.Tipo.fecha, "Se esperaba un valor tipo fecha");
                    break;
                default:
                    Errores("Se esperaba un valor tipo entero, flotante, fecha o cadena y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                    Panic();
                    break;
            }
        }
        private void VALORES()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                VALOR();
                VALORES();
            }
        }
         private void CREARTABLA()
        {            
            Match(Token.Tipo.tabla, "Se esperaba la palabra Reservada Tabla");
            Match(Token.Tipo.id, "Se esperaba un id");
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            Match(Token.Tipo.id, "Se esperaba un id");
            TIPO();
            CAMPOS();
            Match(Token.Tipo.parCierra, "Se esperaba un )");
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
        }
        private void CAMPOS()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                Match(Token.Tipo.id, "Se esperaba un id ");
                TIPO();
                CAMPOS();
            }
            else
            {

            }
        }
        private void TIPO()
        {
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.R_entero:
                    Match(Token.Tipo.R_entero, "Se espera la palabra reservada entero");
                    break;
                case Token.Tipo.R_flotante:
                    Match(Token.Tipo.R_flotante, "Se espera la palabra reservada flotante");
                    break;
                case Token.Tipo.R_cadena:
                    Match(Token.Tipo.R_entero, "Se espera la palabra reservada cadena");
                    break;
                case Token.Tipo.R_fecha:
                    Match(Token.Tipo.R_fecha, "Se espera la palabra reservada fecha");
                    break;
                default:
                    Errores("Se espera la palabra reservada entero, flotante, fecha o cadena y se obtuvo "+preanalisis.Lexema,preanalisis.Fila,preanalisis.Columna);
                    Panic();
                    break;
            }
        }

    }
   
}
