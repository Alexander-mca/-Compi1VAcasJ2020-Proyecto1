using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Analizadores
{
    class Token
    {
        String lexema;
        TipoError error;
        Tipo tipo;
        int fila, columna;

        public Token(Tipo tipo, String lexema,int fila,int columna)
        {
            this.tipo = tipo;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
        }
        public Token(TipoError error, String descripcion, int fila, int columna)
        {
            this.error = error;
            this.lexema = descripcion;
            this.fila = fila;
            this.columna = columna;
        }

        public string Lexema { get => lexema; set => lexema = value; }
        public int Fila { get => fila; set => fila = value; }
        public int Columna { get => columna; set => columna = value; }
        internal TipoError Error { get => error; set => error = value; }
        internal Tipo TipoToken { get => tipo; set => tipo = value; }

        public enum Tipo
        {
            id=1,entero=2,fecha=3,ComentLinea=4,ComentMult=5,flotante=6,cadena=7,menor=8,mayor=9,igual=10,diferente=11,parAbre=12,parCierra=13, coma=14,puntoycoma=15,punto=16,crear=17,tabla=18,asterisco=19, insertar=20,en=21,valores=22,R_entero=23,R_flotante=24,R_fecha=25,R_cadena=26,
            seleccionar=27, R_de=28, donde=29, como=30, R_Y=31, R_O=32, eliminar=33,actualizar=34, establecer=35,dolar=36
        }
        public enum TipoError
        {
            Lexico=1,Sintactico=2
        }
    }
}
