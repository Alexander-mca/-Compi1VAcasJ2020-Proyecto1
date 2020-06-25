using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            this.tokens.RemoveAt(this.tokens.Count - 1);
        }
        public Token Match(Token.Tipo tk,String descripcion)
        { Token.Tipo tipo = preanalisis.TipoToken;
            if (tk.Equals(tipo))
            {
                Token valor = preanalisis;
                String val = valor.Lexema.ToLower();
                valor.Lexema = val;
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

            String nombreTabla=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
            Match(Token.Tipo.establecer, "Se esperaba la palabra Reservada Establecer");
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            Token tk1=Match(Token.Tipo.id, "Se esperaba un id");
            Match(Token.Tipo.igual, "Se espeaba un =");
            List<Campo> valores = new List<Campo>();
            Token tk=VALOR();
            //se empieza a tomar los valores a establecer
            valores.Add(new Campo(tk, tk1));            
            ACTUALIZA(valores);
            Match(Token.Tipo.parCierra, "Se esperaba un )");
            Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
            List<Object> list = new List<object>();
            list.Add(CONDICION());
            CONDICIONES(list);
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            if (this.tablas.ContainsKey(nombreTabla))
            {
                Tabla tabla = this.tablas[nombreTabla];
                List<String> columnas = tabla.getNameColumnas();
                if (valores.Count > tabla.getColumnas().Count )
                {//los campos no son del mismo tam
                    Console.WriteLine("Los campos y las columnas de la tabla difieren en tamaño");
                    return;
                }
                //aca empieza lo de las condiciones, resultante es la unica tabla que resulta de todas las condiciones
                Tabla resultante = getTablaCondiciones(list, tabla, columnas);
                //se compara resultante con la tabla de la que se desea actualizar datos y si hacen match se elimina
                int tam = tabla.getColumnas()[0].Datos.Count;
                for (int i = 0; i < tam; i++)
                {
                    List<Token> valores1 = tabla.getValorFila(i);
                    int tam2 = resultante.getColumnas()[0].Datos.Count;
                    for (int j = 0; j < tam2; j++)
                    {
                        List<Token> valores2 = resultante.getValorFila(j);
                        bool valor = CompararDatos(valores1, valores2, 0);
                        if (valor)
                        {//actualizar datos por columna
                            foreach(Campo item in valores)
                            {
                                Columna colum = tabla.getColumna(item.campo.Lexema);
                                colum.Datos[i] = item.valor;

                            }
                            break;
                        }
                    }
                }

            }


        }
        private void ACTUALIZA(List<Campo> lista)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                Token campo=Match(Token.Tipo.id, "Se esperaba un id");
                Match(Token.Tipo.igual, "Se esperaba un =");
                Token tk=VALOR();
                lista.Add(new Campo(tk,campo));
                ACTUALIZA(lista);
            }
        }
        private void ELIMINAR()
        {
            Match(Token.Tipo.R_de, "Se esperaba la palabra Reservada De");
            String vl1=Match(Token.Tipo.id, "Se esperaba un id").Lexema;
            String nombre = vl1.ToLower();
            List<Object> list = new List<object>();
            ELIMINAR1(list);
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            if (this.tablas.ContainsKey(nombre))
            {
                Tabla tabla = this.tablas[nombre];
                List<String> columnas = tabla.getNameColumnas();
                
                if (list.Count == 0)
                {
                    foreach (Columna item in tabla.getColumnas())
                    {
                        //se eliminan los datos de todas las columnas
                        item.Datos.Clear();
                    }
                    return;
                }
                //aca empieza lo de las condiciones, resultante es la unica tabla que resulta de todas las condiciones
               Tabla resultante= getTablaCondiciones(list, tabla, columnas);
                //se compara resultante con la tabla de la que se desea eliminar datos y si hacen match se elimina
                int tam = tabla.getColumnas()[0].Datos.Count;
                for (int i = 0; i < tam; i++)
                {
                    List<Token> valores1 = tabla.getValorFila(i);
                    int tam2 = resultante.getColumnas()[0].Datos.Count;
                    for (int j = 0; j <tam2; j++)
                    {
                        List<Token> valores2 = resultante.getValorFila(j);
                        bool valor = CompararDatos(valores1, valores2, 0);
                        if (valor)
                        {//remover dato
                            tabla.RemoverFila(i);
                            tam = tabla.getColumnas()[0].Datos.Count;
                            i--;
                            break;
                        }
                    }
                }


            }
            else
            {
                Console.WriteLine("La tabla " + nombre + " no existe");
            }
        }
        private Tabla getTablaCondiciones(List<Object> list,Tabla tabla, List<String> columnas)
        {
            
            ConvertirATablas(list, tabla, columnas);

            //El List<Object> ya tiene valores tipo Tabla, se procede a verificar si hay mas condiciones
            int i = 0;
            if (list.Count > 1)
            {
                int tam = list.Count;
                while (i < tam - 2)
                {
                    Tabla tabla1 = (Tabla)list[i];
                    Token logico = (Token)list[i + 1];
                    Tabla tabla2 = (Tabla)list[i + 2];
                    //se operan con Y | O y devuelve una tabla con los valores de estos
                    Tabla result = getResultado(tabla1, tabla2, logico);
                    if (result != null)
                    {
                        //se procede a guardar la tabla obtenida en list, se remplaza con el primer valor y se eliminan los otros 2
                        list[i] = result;
                        list.Remove(logico);
                        list.Remove(tabla2);
                        tam = list.Count;
                    }
                    i++;
                }

                //ya solo queda una tabla
                if (list[0] is Tabla)
                {
                    Tabla tb = (Tabla)list[0];
                    return tb;
                }


            }
            else if(list.Count==1)
            {//si solo hay uno se retorna 
                if (list[0] is Tabla)
                {
                    Tabla tb = (Tabla)list[0];
                    return tb;
                }
            }
            return null;
        }
        private Tabla getResultado(Tabla tabla1,Tabla tabla2, Token logico)
        {
            Tabla Resultado=new Tabla(tabla1.getNameColumnas());
            
            if (logico.TipoToken.Equals(Token.Tipo.R_Y))
            {
                //se procede a comparar los valores de cada fila de las tablas
                int tam1 = (tabla1.getColumnas()[0]).Datos.Count;
                for (int i = 0; i < tam1; i++)
                {
                    List<Token> val1 = tabla1.getValorFila(i);
                    int tam2 = (tabla2.getColumnas()[0]).Datos.Count;
                    for (int j = 0; j < tam2; j++)
                    {
                        List<Token> val2 = tabla2.getValorFila(j);
                        bool valor = CompararDatos(val1, val2,0);
                        if (valor)
                        {
                            Resultado.Insertar(val2);
                        }
                       
                    }
                }
                return Resultado;
            }else if (logico.TipoToken.Equals(Token.Tipo.R_O))
            {
                int tam1 = (tabla1.getColumnas()[0]).Datos.Count;
                for (int i = 0; i < tam1; i++)
                {
                    List<Token> val1 = tabla1.getValorFila(i);
                    int tam2 = (tabla2.getColumnas()[0]).Datos.Count;
                    for (int j = 0; j < tam2; j++)
                    {
                        List<Token> val2 = tabla2.getValorFila(j);
                        bool valor = CompararDatos(val1, val2, 0);
                        if (!valor)
                        {
                            tabla1.Insertar(val2);
                        }

                    }
                }
                return tabla1;
            }
            return null;
        }
        private bool CompararDatos(List<Token> list1,List<Token> list2, int j)
        {
            bool valor = false;
            if (j < list1.Count)
            {
                Token tk1 = list1[j];
                Token tk2 = list2[j];
                j++;
                if (tk1.Lexema.Equals(tk2.Lexema))
                {
                    valor = true;
                    bool valor2 = valor && CompararDatos(list1, list2, j);
                    return valor2;
                }

            }
            else
            {
                valor = true;
            }
            
            return valor;
        }
        private void ConvertirATablas(List<Object> list, Tabla tabla, List<String> columnas)
        {
            
                //aquí empiezan las multiples condiciones
                for (int i = 0; i < list.Count; i++)
                {
                    //se procede a validar las condiciones
                    Object obj = list[i];
                    if (!(obj is Token))
                    {
                         List<Object> cond = (List<Object>)obj;
                        //Comp puede tener una List<Object>c contiene la tabla y la columna, un token o un String
                        String operando = (String)cond[1];
                        Object comp1 = cond[0];
                        if (comp1 is List<Object>)
                        {
                            List<Object> valores = (List<Object>)comp1;
                            Columna colum = (Columna)valores[1];
                            Object comp2 = cond[2];
                            if (comp2 is List<Object>)
                            {
                                //si ambos valores a comparar son columnas
                                List<Object> valores2 = (List<Object>)comp2;
                                Columna colum2 = (Columna)valores2[1];
                                Condicion condicion = new Condicion(columnas, tabla, tabla, colum, colum2, operando);
                                Tabla result = condicion.Ejecutar();

                            }
                            else if (comp2 is Token)
                            {
                                Token val2 = (Token)comp2;
                                Condicion condicion = new Condicion(columnas, tabla, tabla, colum, val2, operando);
                                //se evalua
                                Tabla result = condicion.Ejecutar();
                                //se guarda en la lista de condiciones
                                list[i] = result;

                            }
                            else if (comp2 is String)
                            {
                                String nameColum = (String)comp2;
                                Columna colum2 = tabla.getColumna(nameColum);
                                    if (colum2 == null)
                                    {
                                        MessageBox.Show("Exestin algunos errores de ejecucion.", "Error");
                                        continue;
                                    }
                                    Condicion condicion = new Condicion(columnas, tabla, tabla, colum, colum2, operando);
                                    //se evalua
                                    Tabla result = condicion.Ejecutar();
                                    //se guarda en la lista
                                    list[i] = result;
                                
                            }

                        }
                        else if (comp1 is Token)
                        {
                            Token tk = (Token)comp1;
                            Object comp2 = cond[2];
                        if (comp2 is String)
                        {

                            String val2 = (String)comp2;
                            Columna cl2 = tabla.getColumna(val2);
                               if (cl2== null)
                                {
                                    MessageBox.Show("Exestin algunos errores de ejecucion.", "Error");
                                    continue;
                                }
                                Condicion condicion = new Condicion(columnas, tabla, tabla, tk, cl2, operando);
                                //se evalua
                                Tabla result = condicion.Ejecutar();
                                //se guarda en la lista
                                list[i] = result;

                            }
                            else if (comp2 is List<Object>)
                            {
                                //recordar agregar la tabla que aparece en este list
                                List<Object> valores = (List<Object>)comp2;
                                Tabla tab2 = (Tabla)valores[0];
                                Columna cl2 = (Columna)valores[1];
                                Condicion condicion = new Condicion(columnas, tabla, tab2, tk, cl2, operando);
                                //se evalua
                                Tabla result = condicion.Ejecutar();
                                //se guarda en la lista
                                list[i] = result;

                            }
                        }
                        else if (comp1 is String)
                        {
                            //string
                            String val1 = (String)comp1;
                            Columna cl1 = tabla.getColumna(val1);
                            if (cl1 == null)
                            {
                            MessageBox.Show("Exestin algunos errores de ejecucion.", "Error");
                                continue;
                            }
                            
                            Object comp2 = cond[2];
                            if (comp2 is Token)
                            {
                                Token valor = (Token)comp2;
                                Condicion condicion = new Condicion(columnas, tabla, null, cl1, valor, operando);
                                Tabla result = condicion.Ejecutar();
                                //se procede a cambiar la List<Object> por una tabla
                                list[i] = result;
                            }
                            else if (comp2 is List<Object>)
                            {

                            }
                            else if (comp2 is String)
                            {
                                String val2 = (String)comp2;
                                Columna cl2 = tabla.getColumna(val2);
                                Condicion condicion = new Condicion(columnas, tabla, tabla, cl1, cl2, operando);
                                //se evalua
                                Tabla result = condicion.Ejecutar();
                                //se guarda en la lista
                                list[i] = result;
                            }
                        }

                    }
                }
            
        }
        private void ELIMINAR1(List<Object> list)
        {
           
            if (preanalisis.TipoToken.Equals(Token.Tipo.donde))
            {
                Match(Token.Tipo.donde, "Se esperaba la palabra Reservada Donde");
                list.Add(CONDICION());
                CONDICIONES(list);
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
            List<Object> lista = new List<object>();
            Object obj=CONDICION();
            lista.Add(obj);
            CONDICIONES(lista);
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
                String valor = COMP1();
                //se usa para verificar si nombreTabla es una tabla o una columna
                if (valor != null)
                {
                    if (this.tablas.ContainsKey(nombreTabla))
                    {
                        Tabla tabla = tablas[nombreTabla];
                        Columna columna = tabla.getColumna(valor);                        
                        List<Object> item = new List<Object>();
                        item.Add(tabla);
                        item.Add(columna);
                        return item;
                    }
                }
                else
                {
                    
                    return nombreTabla;
                }
              
            }
            else
            {
                return VALOR();
            }
            return null;
        }
        private String COMP1()
        {
           
            if (preanalisis.TipoToken.Equals(Token.Tipo.punto))
            {
                Token tk = Match(Token.Tipo.punto, "Se esperaba un .");
                Token tk2 = Match(Token.Tipo.id, "Se esperaba un id");
               
                return tk2.Lexema;
            }
            return null;
        }
        private void CONDICIONES(List<Object> list)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.R_Y))
            {
                Token tk=Match(Token.Tipo.R_Y, "Se esperaba la palabra reservada Y");
                list.Add(tk);
                list.Add(CONDICION());
                CONDICIONES(list);
            }else if (preanalisis.TipoToken.Equals(Token.Tipo.R_O))
            {
                Token tk=Match(Token.Tipo.R_O, "Se esperaba la palabra reservada O");
                list.Add(tk);
                list.Add(CONDICION());
                CONDICIONES(list);
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
            String nameTable = nombreTabla.ToLower();
            Match(Token.Tipo.parAbre, "Se esperaba un (");
            //se obtiene los nombres de las columnas
            String idColumna =Match(Token.Tipo.id, "Se esperaba un id").Lexema;
            String idCol = idColumna.ToLower();
            List<String> columnas = new List<String>();
            columnas.Add(idCol);
            TIPO();
            CAMPOS(columnas);
            Match(Token.Tipo.parCierra, "Se esperaba un )");
            Match(Token.Tipo.puntoycoma, "Se esperaba un ;");
            if (!this.tablas.ContainsKey(nombreTabla))
            {
                Tabla tabla = new Tabla(columnas, nameTable);
                tablas.Add(nameTable, tabla);
            }
        }
        private void CAMPOS(List<String> entradas)
        {
            if (preanalisis.TipoToken.Equals(Token.Tipo.coma))
            {
                Match(Token.Tipo.coma, "Se esperaba una ,");
                String idColumna= Match(Token.Tipo.id, "Se esperaba un id ").Lexema;
                String idCol = idColumna.ToLower();
                entradas.Add(idCol);
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
