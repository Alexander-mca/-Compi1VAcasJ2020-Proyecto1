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
            contenido.Append("digraph D{node[shape=circle];\n");
            getNodos(raiz, contenido);
            getRelacion(raiz, contenido);
            contenido.Append("}");

            StreamWriter dot = new StreamWriter("arbol.txt");
            dot.WriteLine(contenido.ToString());
            dot.Close();
           
            String comando = "-Tpng arbol.txt -o arbol.png";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments =comando;
            process.StartInfo = startInfo;
            process.Start();
            System.Diagnostics.Process.Start("arbol.png");

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
