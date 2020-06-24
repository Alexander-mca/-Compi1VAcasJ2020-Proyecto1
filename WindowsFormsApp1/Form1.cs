using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Analizadores;
using WindowsFormsApp1.Tablas;
using static WindowsFormsApp1.Tablas.Tabla;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        OpenFileDialog abrir;
        SaveFileDialog save;
        List<Token> ListaTokens = new List<Token>();
        List<Token> Errores = new List<Token>();
        Dictionary<String,Tabla> Tablas = new Dictionary<String,Tabla>();
        StringBuilder reporteTokens = new StringBuilder();
        StringBuilder reporteErrores = new StringBuilder();
        Nodo arbolDervicacion;
        public Form1()
        {
            InitializeComponent();
            
        }

        public void AddLineNumbers()
        {
            // create & set Point pt to (0,0)    
            Point pt = new Point(0, 0);
            // get First Index & First Line from richTextBox1    
            int First_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int First_Line = richTextBox1.GetLineFromCharIndex(First_Index);
            // set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively    
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            // get Last Index & Last Line from richTextBox1    
            int Last_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int Last_Line = richTextBox1.GetLineFromCharIndex(Last_Index);
            // set Center alignment to LineNumberTextBox    
            LineNumberTextBox.SelectionAlignment = HorizontalAlignment.Center;
            // set LineNumberTextBox text to null & width to getWidth() function value    
            LineNumberTextBox.Text = "";
            LineNumberTextBox.Width = getWidth();
            // now add each line number to LineNumberTextBox upto last line    
            for (int i = First_Line; i <Last_Line+2; i++)
            {
                LineNumberTextBox.Text += i + 1 + "\n";
            }
        }
        public int getWidth()
        {
            int w = 22;
            // get total lines of richTextBox1    
            int line = richTextBox1.Lines.Length;

            if (line <= 99)
            {
                w = 20 + (int)richTextBox1.Font.Size;
            }
            else if (line <= 999)
            {
                w = 30 + (int)richTextBox1.Font.Size;
            }
            else
            {
                w = 50 + (int)richTextBox1.Font.Size;
            }

            return w;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }
       


        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            AddLineNumbers();
            

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AddLineNumbers();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            LineNumberTextBox.Text = "";
            AddLineNumbers();
            LineNumberTextBox.Invalidate();
        }

        private void richTextBox1_FontChanged(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void LineNumberTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            richTextBox1.Select();
            LineNumberTextBox.DeselectAll();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
           abrir = new OpenFileDialog();

            abrir.Filter = "Archivos de texto (*.DAMC)|*.DAMC";
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                StreamReader arch = new StreamReader(abrir.FileName);
                richTextBox1.Text = arch.ReadToEnd();
                Dock = DockStyle.Fill;
                arch.Close();

            }
            AddLineNumbers();
            //String tex = text1.Text;
            //text1.Clear();
            //Colorear(text1, tex, Color.Black);
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (abrir == null)
            {
                MessageBox.Show("El archivo no existe.", "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    StreamWriter w = new StreamWriter(abrir.FileName);
                    w.WriteLine(richTextBox1.Text);
                    w.Close();
                    MessageBox.Show("Archivo guardado.", "Mensaje de Confirmación.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }catch(ArgumentException ex)
                {
                   Console.Write("Error");
                }
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (abrir != null)
            {
                save = new SaveFileDialog();
                save.FileName = abrir.FileName;

                // filtros
                save.Filter = "Archivos de texto (*.DAMC)|*.DAMC";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    RichTextBox w = new RichTextBox();
                    w =richTextBox1;
                    Dock = DockStyle.Fill;
                    w.SaveFile(save.FileName, RichTextBoxStreamType.UnicodePlainText);
                }


            }
            else
            {
                save = new SaveFileDialog();


                // filtros
                save.Filter = "Archivos de texto (*.DAMC)|*.DAMC";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    RichTextBox w = new RichTextBox();
                    w = richTextBox1;
                    Dock = DockStyle.Fill;
                    w.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Desea guardar el archivo?", "Advertencia", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                if (abrir != null)
                {
                    StreamWriter w = new StreamWriter(abrir.FileName);
                    w.WriteLine(richTextBox1.Text);
                    w.Close();
                    MessageBox.Show("Archivo guardado.", "Mensaje de Confirmación.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    save = new SaveFileDialog();
                    // filtros
                    save.Filter = "Archivos de texto (*.DAMC)|*.DAMC";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        RichTextBox w = new RichTextBox();
                        w = richTextBox1;
                        Dock = DockStyle.Fill;
                        w.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                    }
                }
                this.Dispose();
            }else if (dialogResult == DialogResult.No)
            {
                this.Dispose();
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void ejecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ejecutar();
            
        }
        public void Ejecutar()
        {
            this.ListaTokens.Clear();
            this.Errores.Clear();
            
            Scanner_201700539 scanner = new Scanner_201700539(ListaTokens,richTextBox1);           
            this.ListaTokens = scanner.getListaDeTokens();
            ColorearTexto();
            ReporteTokens(this.reporteTokens);
            List<Token> tokens = this.ListaTokens;
            QuitarComentarios(tokens);
            this.Errores = scanner.getErrores();
            Parser_201700539 parser = new Parser_201700539(tokens,this.Errores);
            this.arbolDervicacion = parser.getArbol();
            if (parser.getErrores().Count != 0)
            {
                MessageBox.Show("Existen Errores");
                return;
            }
            Ejecutar ejecuta = new Ejecutar(tokens, this.Errores, this.Tablas);
            MessageBox.Show("Proceso de Ejecución Terminado");
        }

        private void cargarTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir = new OpenFileDialog();
            String texto="";
            abrir.Filter = "Archivos de texto (*.SQLE)|*.SQLE";
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                StreamReader arch = new StreamReader(abrir.FileName);
                texto = arch.ReadToEnd();
                Dock = DockStyle.Fill;
                arch.Close();

            }
            richTextBox1.Text = texto;
            AddLineNumbers();
        }
        private void QuitarComentarios(List<Token> list)
        {
            List<Token> tokens = new List<Token>();
            tokens = list;
            int tam = tokens.Count;
            for (int i = 0; i < tam; i++)
            {
                Token item = tokens[i];
                if (item.TipoToken.Equals(Token.Tipo.ComentLinea) || item.TipoToken.Equals(Token.Tipo.ComentMult))
                {
                    tokens.Remove(item);
                    tam--;
                }

            }
               
           
        }

        private void verTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Tablas.Count == 0)
            {
                MessageBox.Show("No hay tablas cargadas");
                return;
            }//se procede a mostrar una ventana con la informacion de las tablas
            StringBuilder data = new StringBuilder();
            VerTablas(data);
            StreamWriter streamWriter = new StreamWriter("Tablas.html");
            String archivo = data.ToString();
            streamWriter.WriteLine(archivo);
            streamWriter.Close();
            System.Diagnostics.Process.Start("Tablas.html");
            
        }

        private void VerTablas(StringBuilder buffer)
        {
            buffer.Append("<html><head><title>Tablas</title>").Append("<style>table, th, td {border: 1px solid black;border - collapse: collapse;");
            buffer.Append( "  } \nth, td { padding: 5px;  } \nth { text - align: left; }</style></head><body>");
            foreach(Tabla tabla in this.Tablas.Values)
            {
                buffer.Append("<h2><center>").Append(tabla.getId()).Append("</center></h2><br/><div><center><table>");
                List<Columna> columnas = tabla.getColumnas();
                buffer.Append("<tr>");
                foreach(Columna item in columnas)
                {
                    buffer.Append("<th>").Append(item.Id).Append("</th>");
                }
                buffer.Append("</tr>");
                for (int i = 0; i <columnas[0].Datos.Count; i++)
                {
                    buffer.Append("<tr>");
                    foreach(Columna item in columnas)
                    {
                        buffer.Append("<td>").Append(item.Datos[i].Lexema).Append("</td>");
                    }
                    buffer.Append("</tr>");
                }
                buffer.Append("</center></div></table><br/><br/>");
            }
            buffer.Append("</body></html>");
            

        }

        private void mostrarÁrbolDeDerivaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.arbolDervicacion != null)
            {
                GraficarArbolD arbol = new GraficarArbolD(this.arbolDervicacion);
            }
            else
            {
                MessageBox.Show("No se puede mostrar el arbol de derivacion.\nCiertos componentes no han sido ejecutados");
            }
        }

        private void mostraTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ListaTokens.Count == 0)
            {
                MessageBox.Show("Los archivos no han sido analizados");
                return;
            }
            
            String info = this.reporteTokens.ToString();
            StreamWriter dat = new StreamWriter("201700539_tok.html");
            dat.WriteLine(info);
            dat.Close();
            System.Diagnostics.Process.Start("201700539_tok.html");

        }
        private void ReporteTokens(StringBuilder buffer)
        {
            buffer.Append("<html><head><title>Reporte de Tokens</title>").Append("<style>table, th, td {border: 1px solid black;border - collapse: collapse;");
            buffer.Append("  }\n th, td { padding: 5px;  } \nth { text - align: left; }</style></head><body>");
            buffer.Append("</head><body><h1><center>Reporte de Tokens</center></h1><br/><div><center><table>");
            buffer.Append("<tr><th>Token</th><th>Tipo</th><th>Lexema</th><th>Linea</th><th>Columna</th></tr>");
            for (int i = 0; i < this.ListaTokens.Count; i++)
            {
                Token tk = this.ListaTokens[i];
                buffer.Append("<tr>");
                buffer.Append("<td>").Append((int)tk.TipoToken).Append("</td>");
                buffer.Append("<td>").Append(tk.TipoToken.ToString()).Append("</td>");
                buffer.Append("<td>").Append(tk.Lexema).Append("</td>");
                buffer.Append("<td>").Append(tk.Fila).Append("</td>");
                buffer.Append("<td>").Append(tk.Columna).Append("</td></tr>");

            }
            buffer.Append("</table></center></div></body></html>");
        }
        private void ReporteErrores(StringBuilder buffer)
        {
            buffer.Append("<html><head><title>Reporte de Errores</title>").Append("<style>table, th, td {border: 1px solid black;border - collapse: collapse;");
            buffer.Append("  }\n th, td { padding: 5px;  } \nth { text - align: left; }</style></head><body>");
            buffer.Append("</head><body><h1><center>Reporte de Tokens</center></h1><br/><div><center><table>");

            buffer.Append("<tr><th>Tipo de Error</th><th>Descripcion</th><th>Linea</th><th>Columna</th></tr>");
            for (int i = 0; i < this.Errores.Count; i++)
            {
                Token tk = this.Errores[i];
                buffer.Append("<tr>");
                buffer.Append("<td>").Append(tk.Error.ToString()).Append("</td>");
                buffer.Append("<td>").Append(tk.Lexema).Append("</td>");
                buffer.Append("<td>").Append(tk.Fila).Append("</td>");
                buffer.Append("<td>").Append(tk.Columna).Append("</td></tr>");

            }
            buffer.Append("</table></center></div></body></html>");
        }

        private void mostrarErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Errores.Count == 0)
            {
                MessageBox.Show("No sugieron errores durante el analisis.");
                return;
            }
            StringBuilder data = new StringBuilder();
            ReporteErrores(data);
            String info = data.ToString();
            StreamWriter dt = new StreamWriter("201700539_err.html");
            dt.WriteLine(data.ToString());
            dt.Close();
            System.Diagnostics.Process.Start("201700539_err.html");
        }
        
        private void  ColorearTexto()
        {
            Dictionary<String, Token> datos = ReducirLista(this.ListaTokens);
            
            foreach (Token tk in datos.Values)
            {
                int i =(int)tk.TipoToken;
                switch (i)
                {
                    case 1:
                        HighlightPhrase(richTextBox1, tk.Lexema, Color.Brown);
                        break;
                    case 3:
                        HighlightPhrase(richTextBox1, tk.Lexema, Color.Orange);
                        break;
                    case 2:
                    case 6:
                        HighlightPhrase(richTextBox1, tk.Lexema, Color.Blue);
                        break;
                    case 4:
                    case 5:
                        HighlightPhrase(richTextBox1, tk.Lexema, Color.LightGray);
                        break;
                    case 7:
                        String val = tk.Lexema.Replace("\\", "");
                        HighlightPhrase(richTextBox1, val, Color.Green);
                        break;
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        HighlightPhrase(richTextBox1, tk.Lexema, Color.Red);
                        break;
                    case 18:                        
                    case 20:
                    case 33:
                    case 34:
                        HighlightPhrase(richTextBox1, tk.Lexema, Color.Violet);
                        break;

                }
            }
        }
        private Dictionary<String,Token> ReducirLista(List<Token> tk)
        {
            Dictionary<String, Token> datos = new Dictionary<string, Token>();
            foreach(Token token in tk)
            {
                if (!datos.ContainsKey(token.Lexema))
                {
                    datos.Add(token.Lexema,token);
                }
            }
            return datos;
        }
        static void HighlightPhrase(RichTextBox box, string phrase, Color color)
        {
            int pos = box.SelectionStart;
            string s = box.Text;
            for (int ix = 0; ;)
            {
                int jx = s.IndexOf(phrase, ix, StringComparison.CurrentCultureIgnoreCase);
                if (jx < 0) break;
                box.SelectionStart = jx;
                box.SelectionLength = phrase.Length;
                box.SelectionColor = color;
                ix = jx + 1;
            }
            box.SelectionStart = pos;
            box.SelectionLength = 0;
        }
    
    private void label1_Click(object sender, EventArgs e)
        {

        }

        private void manualTécnicpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Manual Técnico.docx");
        }

        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Manual de Usuario.docx");
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Versión: 1.0\nCreador:Denis Alexander Morales Catalán\nCarné:201700539","Acerca De");
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            AddLineNumbers();
        }
    }
}
