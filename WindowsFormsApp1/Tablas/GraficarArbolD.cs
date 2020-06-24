using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Tablas
{
    class GraficarArbolD
    {
        int cont=0;
        public GraficarArbolD(Nodo raiz)
        {
            StringBuilder contenido = new StringBuilder();
            contenido.Append("digraph D{node[shape=circle fillcolor=green style=filled];\n");
            getNodos(raiz, contenido);
            getRelacion(raiz, contenido);
            contenido.Append("}");

            StreamWriter dot = new StreamWriter("arbol.txt");
            dot.WriteLine(contenido.ToString());           
            dot.Close();
            String ruta=Path.GetFullPath("arbol.txt");
            //String comando = "/C cd " + ruta;
            String cmddot="Dot -Tjpg arbol.txt -o arbol.jpg";
            //Process.Start("CMD.exe", comando);
            Process pros = new Process();
            //pros.StartInfo.WindowStyle= System.Diagnostics.ProcessWindowStyle.Hidden;
            pros.StartInfo.FileName = "CMD.exe";
            pros.StartInfo.Arguments = cmddot;
            pros.Start();
            Process.Start("arbol.jpg");
        }
        private void getNodos(Nodo raiz,StringBuilder nodos)
        {
            nodos.Append("node").Append( cont).Append( "[label=\"").Append(raiz.getValor()).Append("\"];\n");
            raiz.setId(cont);
            cont++;
            foreach(Nodo hijo in raiz.getHijos()){
                getNodos(hijo, nodos);
            }
        }
        private void getRelacion(Nodo raiz,StringBuilder relacion)
        {
            foreach(Nodo hijo in raiz.getHijos())
            {
                relacion.Append("\"node").Append(raiz.getId()).Append("\"->");
                relacion.Append("\"node").Append(hijo.getId()).Append("\";\n");
                getRelacion(hijo, relacion);
            }
        }
    }
}
