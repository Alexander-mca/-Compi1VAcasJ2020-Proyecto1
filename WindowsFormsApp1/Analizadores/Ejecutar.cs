using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Tablas;
using static WindowsFormsApp1.Tablas.Tabla;

namespace WindowsFormsApp1.Analizadores
{
    class Ejecutar
    {
        List<Token> tokens = new List<Token>();
        List<Token> errores = new List<Token>();
        Dictionary<String,Tabla> tablas = new Dictionary<String,Tabla>();
        int i;
        Token preanalisis;
        public Ejecutar(List<Token> tokens,List<Token> Errores,Dictionary<String,Tabla> tablas)
        {
            this.tokens = tokens;
            this.errores = Errores;
            this.tablas = tablas;
            this.i = 0;
            this.preanalisis = tokens[i];
            this.tokens.Add(new Token(Token.Tipo.dolar, "$", 0, 0));
            
            INICIO();
        }
        public Token Match(Token.Tipo tk,String descripcion)
        { Token.Tipo tipo = preanalisis.TipoToken;
            if (tk.Equals(tipo))
            {
                Token valor = preanalisis;
                preanalisis = getNextToken();
                return valor;
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

         public void INICIO()
        {
            INSTRUCCIONES();
        }
        private void INSTRUCCIONES()
        {
            if (!preanalisis.TipoToken.Equals(Token.Tipo.dolar))
            {
                INSTRUCCION();
                INSTRUCIONES1();
            }
            else
            {
                return;
            }
        }
        private void INSTRUCIONES1()
        {
            if (!preanalisis.TipoToken.Equals(Token.Tipo.dolar))
            {
                INSTRUCCIONES();
            }
            else
            {
                return;
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
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            Match(Token.Tipo.id, "Se esperaba un id");
            Match(Token.Tipo.igual, "Se espeaba un =");
            VALOR();
            ACTUALIZA();
            Match(Token.Tipo.parCierra, "Se esperaba un )");
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
            List<String> columnas = new List<string>();
            SELECCIONAR1(columnas);
            SELECT();
            Match(Token.Tipo.R_de, "Se esperaba la palabra reservada De");
            Token tk=Match(Token.Tipo.id, "Se esperaba un id");
            Dictionary<String, Tabla> tabs = new Dictionary<string, Tabla>();
            Tabla tb1 = this.tablas[tk.Lexema];
            tabs.Add(tk.Lexema,tb1);
            TABLA(tabs);
            Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
            Object obj=CONDICION();

            CONDICIONES();
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");

        }
        private List<Object> CONDICION()
        {

            Object valor1=COMP();
            String operador=TIPOCOMP();
            Object valor2=COMP();
            List<Object> componentes = new List<object>();
            componentes.Add(valor1);
            componentes.Add(operador);
            componentes.Add(valor2);

            return componentes;
        }
        private String TIPOCOMP()
        {
            String operador = "";
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.mayor:
                    operador= Match(Token.Tipo.mayor, "Se esperaba un >").Lexema;
                    operador+=TC1();
                    break;
                case Token.Tipo.menor:
                   operador= Match(Token.Tipo.menor, "Se esperaba <").Lexema;
                    operador+=TC1();
                    break;
                case Token.Tipo.diferente:
                    operador=Match(Token.Tipo.diferente, "Se esperaba !").Lexema;
                    operador+=Match(Token.Tipo.igual, "Se esperaba un =").Lexema;
                    break;
                case Token.Tipo.igual:
                    operador=Match(Token.Tipo.igual, "Se esperaba un =").Lexema;
                    break;
                default:
                    Errores("Se esperaba un >,<,! o un = y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                    Panic();
                    break;
            }
            return operador;
        }
        private String TC1()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.igual))
            {
                return Match(Token.Tipo.igual, "Se esperaba un =").Lexema;
            }
            return null;
        }
        private Object COMP()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
               String nombreTabla=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
                List<String> valor = COMP1();
                if (valor != null)
                {
                    Tabla tabla = tablas[nombreTabla];
                    Columna columna = tabla.getColumna(valor[1]);
                    List<Object> item = new List<Object>();
                    item.Add(tabla);
                    item.Add(columna);
                    return item;
                }
                else
                {
                    Columna colum = new Columna(nombreTabla);
                    return colum;
                }
              
            }
            else
            {
                return VALOR();
            }
        }
        private List<String> COMP1()
        {
           
            if (preanalisis.TipoToken.Equals(Token.Tipo.punto))
            {
                Token tk = Match(Token.Tipo.punto, "Se esperaba un .");
                Token tk2 = Match(Token.Tipo.id, "Se esperaba un id");
                List<String> valores = new List<string>();
                valores.Add(tk.Lexema);
                valores.Add(tk2.Lexema);
                return valores;
            }
            return null;
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
        private void TABLA(Dictionary<String,Tabla> tabs)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                String nombre=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
                Tabla tab = this.tablas[nombre];
                tabs.Add(nombre, tab);
                TABLA(tabs);
            }
        }
        private void SELECCIONAR1(List<String> columnas)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                String nombre=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
                Match(Token.Tipo.punto, "Se esperaba un punto");
                String val=SELECTO();
                if (val == null) return;
                if (this.tablas.ContainsKey(nombre))
                {
                    Tabla tab = this.tablas[nombre];
                    if (val.Equals("*"))
                    {
                        foreach(Columna item in tab.getColumnas())
                        {
                            columnas.Add(item.Id);
                        }
                    }
                    else
                    {
                        columnas.Add(val);
                    }
                    
                }
                

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
        private String SELECTO()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.id))
            {
                String val=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
                return val;
            }else if (preanalisis.TipoToken.Equals(Token.Tipo.asterisco))
            {
                String val=Match(Token.Tipo.asterisco, "Se esperaba un *").Lexema;
                return val;
            }
            else
            {
                Errores("Se esperaba un id o un * y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                Panic();
            }
            return null;
        }
        private void SELECT()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.como))
            {
                Match(Token.Tipo.como, "Se esperaba la palabra reservada Como");
                Match(Token.Tipo.id, "Se esperaba un id");
                TABLAS();
            }

        }
        private void TABLAS()
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                List<String> columnas = new List<string>();
                SELECCIONAR1(columnas);
                SELECT();
            }
        }
        private void INSERTAR()
        {
            Match(Token.Tipo.en, "Se esperaba la palabra Reservada En");
            //se obtiene el nombre de la tabla
            String nombreTabla=Match(Token.Tipo.id, "Se esperaba un id ").Lexema;
            Match(Token.Tipo.valores, "Se esperaba la palabra Reservada Valores");
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            //se obtienen los valores que se van a insertar
            Token valor=VALOR();
            List<Token> valores = new List<Token>();
            valores.Add(valor);
            VALORES(valores);
            Match(Token.Tipo.parCierra, "Se esperaba un )");
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            //se verifica que la tabla que se esta pidiendo haya sido creada
            if (this.tablas.ContainsKey(nombreTabla))
            {  //se procede a meter los valores
                Tabla tabla = this.tablas[nombreTabla];
                tabla.Insertar(valores);
                this.tablas[nombreTabla] = tabla;
            }
           
        }
        private Token VALOR()
        {
            Token valor =null;
            switch (preanalisis.TipoToken)
            {
                case Token.Tipo.entero:
                    valor=Match(Token.Tipo.entero, "Se esperaba un número tipo entero");
                    break;
                case Token.Tipo.flotante:
                    valor=Match(Token.Tipo.flotante, "Se esperaba un número tipo flotante");
                    break;
                case Token.Tipo.cadena:
                    valor=Match(Token.Tipo.cadena, "Se esperaba un valor tipo cadena");
                    break;
                case Token.Tipo.fecha:
                    valor=Match(Token.Tipo.fecha, "Se esperaba un valor tipo fecha");
                    break;
                default:
                    Errores("Se esperaba un valor tipo entero, flotante, fecha o cadena y se obtuvo " + preanalisis.Lexema, preanalisis.Fila, preanalisis.Columna);
                    Panic();
                    break;
            }
            return valor;
        }
        private void VALORES(List<Token> valores)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                Token valor=VALOR();
                valores.Add(valor);
                VALORES(valores);
            }
        }
         private void CREARTABLA()
        {            
            Match(Token.Tipo.tabla, "Se esperaba la palabra Reservada Tabla");
            //se obtiene el nombre de la tabla que se va a crear
            String nombreTabla=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            //se obtiene los nombres de las columnas
            String idColumna =Match(Token.Tipo.id, "Se esperaba un id").Lexema;
            List<String> columnas = new List<String>();
            columnas.Add(idColumna);
            TIPO();
            CAMPOS(columnas);
            Match(Token.Tipo.parCierra, "Se esperaba un )");
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            if (!this.tablas.ContainsKey(nombreTabla))
            {
                Tabla tabla = new Tabla(columnas, nombreTabla);
                tablas.Add(nombreTabla, tabla);
            }
        }
        private void CAMPOS(List<String> entradas)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                String idColumna= Match(Token.Tipo.id, "Se esperaba un id ").Lexema;
                entradas.Add(idColumna);
                TIPO();
                CAMPOS(entradas);
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
                    Match(Token.Tipo.R_cadena, "Se espera la palabra reservada cadena");
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
