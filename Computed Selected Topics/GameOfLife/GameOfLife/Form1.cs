using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        int rows, cols, cellArea, totalGenarations = 0;
        int[][] matrix;
        double density;
        int numAlive, canvasStatus, rulesChanged; //for rulesChanged 0 no change, 1 born change, 2 survive change
        Graphics g;
        Pen pBlack;
        Brush bBlack, bWhite, bGreen, bBlue;
        char[] aliveConditions, surviveConditions;
        Random rand;
        bool canEdit;
        Form2 grafica, grafica2;
        Dictionary<int, int[][]> patterns;
        Dictionary<ulong, ulong> transicion;
        Dictionary<ulong, List<ulong>> recurrencias;
        Dictionary<string, Graph> clasificados;

        private void Button3_Click(object sender, EventArgs e)
        {
            FillZeros();
            canvas.Invalidate();
            if (checkGraficar.Checked)
            {
                grafica.Dispose();
                grafica2.Dispose();
            }
            totalGenarations = 0;
            btnPause.Text = "Pause";
            btnPause.Enabled = false;
            btnStart.Enabled = true;
            canEdit = true;
            Timer.Stop();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                canEdit = true;
                btnPause.Text = "Play";
                Timer.Stop();
            }
            else
            {
                canEdit = false;
                btnPause.Text = "Pause";
                Timer.Start();
            }
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int cordX = (int)(e.X / cellArea);
            int cordY = (int)(e.Y / cellArea);
            switch (canvasStatus) {
                case 0:
                    if (canEdit)
                    {
                        if (matrix[cordX][cordY] == 1)
                            matrix[cordX][cordY] = 0;
                        else
                            matrix[cordX][cordY] = 1;
                        btnPause.Enabled = true;
                        btnPause.Text = "Play";
                        canvas.Invalidate();
                    }
                    else
                        MessageBox.Show("Stop the running automata before editing");
                    break;
                case 1:
                    int[][] pattern = GetPattern(comboPattern.SelectedIndex);
                    int[][] p2 = new int[pattern.Length][];
                    //copied to anoter matrix
                    for (int i = 0; i < pattern.Length; i++)
                    {
                        p2[i] = new int[pattern[i].Length];
                        for (int j = 0; j < pattern[i].Length; j++)
                            p2[i][j] = pattern[i][j];
                    }
                    if (pattern != null)
                    {
                        if (comboPattern.SelectedIndex == 0)//si es glider se mostrara dependiendo del cuadrante en el que este
                        {
                            if (cordX <= (rows - 1) / 2)
                                if (cordY <= (cols - 1) / 2)
                                {
                                    RotateMatrix(p2, 3);
                                }
                                else
                                {
                                }
                            else
                            {
                                if (cordY <= (cols - 1) / 2) {
                                    RotateMatrix(p2, 2);
                                }
                                else
                                {
                                    RotateMatrix(p2, 1);
                                }
                            }
                        }

                        //draw the pattern in the canvas
                        for (int i = 0; i < p2.Length; i++)
                            for (int j = 0; j < p2[i].Length; j++)
                                matrix[cordX - 1 + i][cordY - 1 + j] = p2[i][j];
                        canvas.Invalidate();
                    }
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            labelGenerations.Text = "" + totalGenarations++;
            int[][] matrixbuffer = GameOfLife(aliveConditions, surviveConditions,matrix);//creating the new generation in the buffer
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i][j] = matrixbuffer[i][j];
            if (checkGraficar.Checked)
            {
                grafica.Graficar("" + totalGenarations, (double)numAlive / (rows * cols));
                grafica2.Graficar("" + totalGenarations, (-numAlive) * Math.Log(numAlive, 2));
            }
            /*Once the matrixbuffer is copied in the original matrix we repaint the canvas*/
            canvas.Invalidate();
            readRules();
        }

        /*When its called the Invalidate() method of the picturebox this method is executed*/
        /*Draw the actual generation*/
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (matrix != null)
            {
                g = e.Graphics;
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        g.FillRectangle(matrix[i][j] == 1 ? bWhite : bBlack, i * cellArea, j * cellArea, cellArea, cellArea);
                    }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            FillZeros();
            canvas.Invalidate();
        }

        private void TxtAlive_TextChanged(object sender, EventArgs e)
        {
            rulesChanged = 1;
        }

        private void TxtSurvive_TextChanged(object sender, EventArgs e)
        {
            rulesChanged = 2;
        }

        private void ComboPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelPatrones.Visible = true;
            btnCancelarPatrones.Visible = true;
            canvasStatus = 1;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            FillOnes();
            canvas.Invalidate();
        }

        private void BtnCancelarPatrones_Click(object sender, EventArgs e)
        {
            canvasStatus = 0;
            btnCancelarPatrones.Visible = false;
            labelPatrones.Visible = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            cellArea = int.Parse(txtCellSize.Text);
            /*getting the matrix dimensions*/
            cols = int.Parse(numCols.Text);
            rows = int.Parse(numRows.Text);
            density = double.Parse(txtDensity.Text);
            /*giving the picturebox size*/
            canvas.Width = cellArea * cols;
            canvas.Height = cellArea * rows;
            /*definiing some properties of the picturebox*/
            flowPanel.AutoScroll = true;
            flowPanel.Controls.Add(canvas);
            /*generate the matrix with the given density of ones*/
            matrix = new int[rows][];
            //matrix = new int[3][];
            /*get survive & alive conditions*/
            aliveConditions = txtAlive.Text.ToCharArray();
            surviveConditions = txtSurvive.Text.ToCharArray();
            rulesChanged = 0;
            canvasStatus = 0;
            /*Generating Matrix*/
            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    if (rand.NextDouble() > density)
                        matrix[i][j] = 1;
                    else
                        matrix[i][j] = 0;
            }
            btnPause.Enabled = true;
            btnPause.Text = "Pause";
            btnStart.Enabled = false;
            btnPause.Focus();//focusing the pause button in order to use space to pause or resume the ejecution
            if (checkGraficar.Checked)
            {
                grafica = new Form2("Density");
                grafica.Show();
                grafica2 = new Form2("Entrophy");
                grafica2.Show();
            }
            Timer.Interval = int.Parse(txtInterval.Text);
            Timer.Start();

        }

        private void BtnCombinations_Click(object sender, EventArgs e)
        {
            rows = int.Parse(txtCombinaciones.Text);
            cols = int.Parse(txtCombinaciones.Text);
            /*get survive & alive conditions*/
            aliveConditions = txtAlive.Text.ToCharArray();
            surviveConditions = txtSurvive.Text.ToCharArray();
            CreatePosibilities(int.Parse(txtCombinaciones.Text));
        }

        private int[][] GameOfLife(char[] alive, char[] survive,int[][] field)
        {
            numAlive = 0;
            int[][] aux = new int[rows][];
            for (int i = 0; i < rows; i++) {
                aux[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    aux[i][j] = field[i][j];
            }

            int n;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    n = CountNeighbours(i, j,field);
                    if (field[i][j] == 0)
                    {
                        for (int a = 0; a < alive.Length; a++) //busca si n empata con la cantidad para vivir
                            if (n == Char.GetNumericValue(alive[a]))
                            {
                                aux[i][j] = 1;
                                numAlive++;
                                continue;
                            }

                    }
                    else
                    {
                        bool flag = false;
                        for (int a = 0; a < survive.Length && flag == false; a++) //busca si n empata con la cantidad de sobrevivir
                            if (n == Char.GetNumericValue(survive[a]))
                                flag = true;
                        if (flag == false)
                            aux[i][j] = 0;
                        else
                            numAlive++;
                    }
                }
            }

            return aux;
        }

        private int CountNeighbours(int row, int col,int[][] matriz)
        {
            return matriz[(row + 1) % rows][col] +
                matriz[row][(col + 1) % cols] +
                matriz[(row + rows - 1) % rows][col] +
                matriz[row][(col + cols - 1) % cols] +
                matriz[(row + 1) % rows][(col + 1) % cols] +
                matriz[(row + rows - 1) % rows][(col + 1) % cols] +
                matriz[(row + rows - 1) % rows][(col + cols - 1) % cols] +
                matriz[(row + 1) % rows][(col + cols - 1) % cols];
        }

        //fill the matrix with 0s
        private void FillZeros()
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i][j] = 0;
        }
        //fill the matrix with 0s
        private void FillOnes()
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i][j] = 1;
        }

        private void readRules()
        {
            switch (rulesChanged)
            {
                case 0:
                    break;
                case 1:
                    aliveConditions = txtAlive.Text.ToCharArray();
                    rulesChanged = 0;
                    break;
                case 2:
                    surviveConditions = txtSurvive.Text.ToCharArray();
                    rulesChanged = 0;
                    break;
            }
        }

        private void FillDictionary()
        {
            patterns = new Dictionary<int, int[][]>();
            int[][] aux = new int[3][];
            aux[0] = new int[3] { 1, 0, 1 };
            aux[1] = new int[3] { 1, 1, 0 };
            aux[2] = new int[3] { 0, 1, 0 };
            patterns.Add(0, aux);
        }

        private int[][] GetPattern(int index)
        {
            int[][] res;
            if(patterns.TryGetValue(index,out res))
            {
                return res;
            }

            return null;

        }

        private void RotateMatrix(int[][] m,int times)
        {
            int temp;
            for(int t = 0; t < times; t++)
            {
                for(int c = 0; c < m.Length / 2; c++)
                {
                    for(int i = 0; i < (m.Length - c - 1) - c; i++)
                    {
                        temp = m[c][ c + i];
                        m[c][ c + i] = m[(m.Length - c - 1) - i][ c];
                        m[(m.Length - c - 1) - i][ c] = m[m.Length - c - 1][ (m.Length - c - 1) - i];
                        m[m.Length - c - 1][(m.Length - c - 1) - i] = m[c + i][m.Length - c - 1];
                        m[c + i][m.Length - c - 1] = temp;
                    }
                }
            }

        }

        private void CreatePosibilities(int n)
        {
            ulong numCombinaciones = (ulong)Math.Pow(2.0, n * n);
            Debug.WriteLine("Creando todos los caminos de " + n + "x" + n);
            transicion.Clear();
            recurrencias.Clear();
            clasificados.Clear();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text File|*.txt";
                saveFileDialog.Title = "Historial de nodos";
                saveFileDialog.ShowDialog();
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                for (ulong comb = 1; comb < numCombinaciones; comb++)
                {
                    Debug.WriteLine("LLenando a historia");
                    string cadBinaria = Convert.ToString((long)comb, 2);
                    //rellenamos de 0 para complir con el numero de celulas de la matriz
                    while (cadBinaria.Length < (n * n)) cadBinaria = 0 + cadBinaria;
                    int[][] aux = StringToMatrix(cadBinaria);
                    int[][] bufferMatrix = GameOfLife(aliveConditions, surviveConditions, aux);
                    string strBuffer = MatrixToString(bufferMatrix);
                    ulong nextValue = Convert.ToUInt64(strBuffer, 2);
                    writer.WriteLine(comb + " = " + cadBinaria + " --> " + nextValue + " = " +strBuffer);
                    Debug.WriteLine(comb + " = " + cadBinaria + " --> " + nextValue + " = " + strBuffer);
                    transicion.Add(comb, nextValue);
                }

                writer.Dispose();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al exportar el grado" + e.Message);
            }
            
            //despues de que obtenemos todos los posibles caminos, toca ordenarlos y obtener los que se repiten
            //el diccionario recurrencias tiene un key que es el valor entero de la matriz generada
            //y el value es una lista de todos los nodos que llevan a ese valor entero
            foreach (var item in transicion.OrderBy(key => key.Value))
            {
                if (recurrencias.ContainsKey(item.Value))
                {
                    recurrencias[item.Value].Add(item.Key);
                }
                else
                {
                    List<ulong> aux = new List<ulong>();
                    aux.Add(item.Key);
                    recurrencias.Add(item.Value, aux);
                }
            }
            //Debug.WriteLine(ExportarGrafo(recurrencias));
            Debug.WriteLine("Exportando datos no filtrados");
            ExportarGrafoAMatlab(recurrencias,"Guardar no filtrados");
            //ahora que tenemos las recurrencias de todos los nodos se intentara generar el grafo con los caminos filtrados
            //de todas las recurrencias se generaran sus respectivos grafos
            foreach (KeyValuePair<ulong, List<ulong>> item in recurrencias)
            {
                Dictionary<ulong, List<ulong>> aux = new Dictionary<ulong, List<ulong>>();
                aux.Add(item.Key, item.Value);
                //reccoremos todos los elementos de la lista almacenada en el value de item
                for (int i = 0; i < item.Value.Count; i++)
                {
                    ulong auxLista = item.Value[i];
                    if (recurrencias.ContainsKey(auxLista))
                    {
                        if (!aux.ContainsKey(auxLista))
                            aux.Add(auxLista, recurrencias[auxLista]);
                    }
                }

                Graph actualGraph = new Graph(aux);
                string key = actualGraph.getKey();
                if (aux.Count > 0 && !clasificados.ContainsKey(key))
                    clasificados.Add(key, actualGraph);
            }
            //para los filtrados tenemos que juntar todas los diccionarios de cada uno de los graphs en uno solo
            Dictionary<ulong, List<ulong>> auxDic = new Dictionary<ulong, List<ulong>>();
            foreach(KeyValuePair<string,Graph> itemClas in clasificados)
            {
                foreach(KeyValuePair<ulong,List<ulong>> itemGraph in itemClas.Value.getAllNodes())
                {
                    auxDic[itemGraph.Key] = itemGraph.Value;
                }
            }
            ExportarGrafoAMatlab(auxDic,"Guardar filtrados");
            Debug.WriteLine("Termino de generar todos los caminos");
        }

        private void ExportarGrafoAMatlab(Dictionary<ulong, List<ulong>> dic,string titulo)
        {
            Debug.WriteLine("Comenzando exportacion: "+titulo);
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text File|*.csv";
                saveFileDialog.Title = titulo;
                saveFileDialog.ShowDialog();
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                //recorremos todo el diccionario
                foreach (KeyValuePair<ulong, List<ulong>> item in dic)
                {
                    //recorremos la lista del value para crear cada uno de los nodos y conectarlo al nodo de key
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        writer.WriteLine((item.Value[i]+1)+","+(item.Key+1));
                    }
                }
                writer.Dispose();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al exportar el grado" + e.Message);
            }
            Debug.WriteLine("Exportacion con exito");
            MessageBox.Show("Exportacion " + titulo + " realizada con exito");
        }

        private string MatrixToString(int[][] matrix)
        {
            string res = "";
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    res += matrix[i][j] == 1 ? "1" : "0";
            return res;
        }

        private int[][] StringToMatrix(string cad)
        {
            int size = Convert.ToInt32(Math.Sqrt(cad.Length));
            int[][] newMatrix = new int[size][];
            for(int i = 0,index = 0; i < size; i++)
            {
                newMatrix[i] = new int[size];
                for(int j = 0; j < size; j++)
                {
                    newMatrix[i][j] = cad[index++] == '1' ? 1 : 0;
                }
            }

            return newMatrix;
        }

        public Form1()
        {
            InitializeComponent();
            pBlack = new Pen(Color.Black);
            bBlack = Brushes.Black;
            bWhite = Brushes.White;
            bGreen = Brushes.Green;
            bBlue = Brushes.Blue;
            rand = new Random();
            canEdit = false;
            numAlive = 0;
            //generate dictionary
            FillDictionary();
            transicion = new Dictionary<ulong, ulong>();
            clasificados = new Dictionary<string, Graph>();
            recurrencias = new Dictionary<ulong, List<ulong>>();
        }

    }
}
