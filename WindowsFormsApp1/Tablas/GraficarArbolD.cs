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
            contenido.Append("digraph G{node[shape=circle fillcolor=green forecolor=black style=filled];");
            getNodos(raiz, contenido);
            getRelacion(raiz, contenido);
            contenido.Append("}");

            StreamWriter dot = new StreamWriter("arbol.dot");
            dot.WriteLine(contenido.ToString());
            dot.Close();
            ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
            startInfo.Arguments = "-Tjpg arbol.dot -o arbol.jpg";
            Process.Start(startInfo);
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
