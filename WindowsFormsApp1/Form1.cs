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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        OpenFileDialog abrir;
        SaveFileDialog save;
        List<Token> ListaTokens = new List<Token>();
        List<Token> Errores = new List<Token>();
        Dictionary<Tabla,String> Tablas = new Dictionary<Tabla,String>();
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
            for (int i = First_Line; i <= Last_Line + 2; i++)
            {
                LineNumberTextBox.Text += i + 1 + "\n";
            }
        }
        public int getWidth()
        {
            int w = 25;
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
            if (richTextBox1.Text == "")
            {
                AddLineNumbers();
            }

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
            DialogResult dialogResult = MessageBox.Show("Advertencia", "¿Desea guardar el archivo?", MessageBoxButtons.YesNoCancel);
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
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void ejecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String texto = richTextBox1.Text;
            Scanner_201700539 scanner = new Scanner_201700539(ListaTokens, texto);
            //if (scanner.getErrores().Count != 0)
            //{
            //    MessageBox.Show("Existen Errores Léxicos.");
            //}
            this.ListaTokens = scanner.getListaDeTokens();
            this.Errores = scanner.getErrores();
            Parser_201700539 parser = new Parser_201700539(this.ListaTokens, this.Errores);
            if (parser.getErrores().Count != 0)
            {
                MessageBox.Show("Existen Errores");
            }
            

        }

        private void cargarTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir = new OpenFileDialog();
            String texto;
            abrir.Filter = "Archivos de texto (*.SQLE)|*.SQLE";
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                StreamReader arch = new StreamReader(abrir.FileName);
                texto = arch.ReadToEnd();
                Dock = DockStyle.Fill;
                arch.Close();

            }
           
            Scanner_201700539 scanner = new Scanner_201700539(ListaTokens, texto);
            //if (scanner.getErrores().Count != 0)
            //{
            //    MessageBox.Show("Existen Errores Léxicos.");
            //}
            this.ListaTokens = scanner.getListaDeTokens();
            this.Errores = scanner.getErrores();
            Parser_201700539 parser = new Parser_201700539(this.ListaTokens, this.Errores);
            if (parser.getErrores().Count != 0)
            {
                MessageBox.Show("Existen Errores");
            }
        }

        private void verTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Tablas.Count == 0)
            {
                MessageBox.Show("No hay tablas cargadas");
                return;
            }

            //se procede a mostrar una ventana con la informacion de las tablas
        }
    }
}
