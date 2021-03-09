using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Langton_Ant
{
    public partial class Form1 : Form
    {
        private const int NORTH = 5, SOUTH = 6, EAST = 7, WEAST = 8, RIGHT = 9, LEFT = 7, DEAD = 0, WHITE = 1, REINA = 2,SOLDADO = 3,OBRERA = 4;
        private const double pReina = 0.05, pSoldado = 0.35, pObrera = 0.60;
        private int cellSize = 20,padding,frames = 0,countReinas=0,countSoldado=0,countObreras=0,countHormigas = 0;
        private int tam = 50;
        private int[,] matrix;
        private Brush bBLACK, bWHITE, bRED,bBLUE,bGREEN;
        private int Status; //nos permite saber si se esta en modo agregar mas hormigas o en modo normal
        private List<Hormiga> hormigas;
        private Graphics g;
        private Random random;
        private Grafica graphDensity,graphEntrophy;

        private void Timer1_Tick(object sender, EventArgs e)
        {
            List<Hormiga> soldados;
            int tamLista = hormigas.Count();
            for(int i = 0;i< tamLista;)
            {
                
                //si la hormiga es reina se checa si hay un candidato a reproducirse
                if(hormigas.ElementAt(i).Tipo == REINA)
                {
                    //la hormiga es candidata si esta dentro de su rango de reproduccion

                    if (hormigas.ElementAt(i).isFertil())
                    {
                        //ya que si se encuentra dentro de su rango de reproduccion toca buscar una hormiga soldado que se encuentre
                        //frente a ella y que este mirando a la hormiga reina
                        //obtenemos a todas la hormigas soldado que esten frente a la hormiga reina
                        int[] poses = new int[] { NORTH, EAST, WEAST};
                        int p = 0;
                        soldados = GetNeighbor(hormigas.ElementAt(i),poses);
                        if (soldados.Count>0)
                        {
                            Debug.WriteLine("Hay soldados o reinas");
                            p = 0;
                            for (int j = 0; j < soldados.Count && p<0; j++,p++)
                            {
                               
                                if (soldados.ElementAt(j).Tipo == SOLDADO)
                                {
                                    Debug.WriteLine("Soldado found");
                                    if (soldados.ElementAt(j).isFertil())
                                    {
                                        Debug.WriteLine("Soldado fertil");
                                        int nX = 0, nY = 0;
                                        //el radio de nacimiento de las nuevas hormigas sera de 6x6 donde el centro es la hormiga reina
                                        //generamos las posiciones X y Y de las nuevas hormigas con un rand
                                        for (int k = 0; k < 2; k++)
                                        {
                                            while (nX != hormigas.ElementAt(i).X && nY != hormigas.ElementAt(i).Y && nX <= tam && nY <= tam && nX >= 0 && nY >= 0)
                                            {
                                                nX = random.Next(hormigas.ElementAt(i).X - 3, hormigas.ElementAt(i).X + 3);
                                                nY = random.Next(hormigas.ElementAt(i).Y - 3, hormigas.ElementAt(i).Y + 3);
                                            }
                                            Hormiga h = CrearHormiga(nX, nY, random.NextDouble());
                                            Debug.WriteLine("Nacio hormiga " + h.Tipo);
                                            tamLista++;
                                            hormigas.Add(h);
                                            AgregarHormigaAMatrix(h);
                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Soldado no fertil");
                                    }
                                }
                                else if (soldados.ElementAt(j).Tipo == REINA)
                                {
                                    Debug.WriteLine("QUEENS FIGHT");
                                    if (!soldados.ElementAt(j).isFertil())
                                    {
                                        hormigas.Remove(hormigas.Find(ant => ant.Equals(hormigas.ElementAt(i))));
                                    }
                                    else if (soldados.ElementAt(j).isFertil())
                                    {
                                        hormigas.Remove(hormigas.ElementAt(i));
                                    }
                                    else
                                    {
                                        if (random.NextDouble() > 0.50)
                                            hormigas.Remove(hormigas.ElementAt(i));
                                        else
                                            hormigas.Remove(hormigas.Find(ant => ant.Equals(hormigas.ElementAt(i))));
                                    }
                                    countHormigas--;
                                    countReinas--;
                                    tamLista--;
                                    Debug.WriteLine("Queen die");
                                }
                            }
                        }
                        else
                            Debug.WriteLine("No hay soldados ni reinas");
                    }

                }
                //siempre que se dibuje una hormiga perdera un punto de vida
                //si la vida de la hormiga llega a 0 la quitamos de la lista;
                if(i<tamLista)
                if (hormigas.ElementAt(i).Vida <= 0)
                {
                    switch (hormigas.ElementAt(i).Tipo)
                    {
                        case REINA:
                            countReinas--;
                            break;
                        case SOLDADO:
                            countSoldado--;
                            break;
                        case OBRERA:
                            countObreras--;
                            break;
                    }
                    hormigas.Remove(hormigas.ElementAt(i));
                    countHormigas--;
                    tamLista--;
                }
                else
                {
                    NextStep(hormigas.ElementAt(i), NextOrientation(hormigas.ElementAt(i), hormigas.ElementAt(i).Color != DEAD ? LEFT : RIGHT));
                    hormigas.ElementAt(i).Vida--;
                    i++;
                }
            }
            labelGenerations.Text = "" + (++frames);
            labelNumHormigas.Text = "" + countHormigas;
            labelReinas.Text = "" + countReinas;
            labelSoldado.Text = "" + countSoldado;
            labelObreras.Text = "" + countObreras;
            if (checkGraficar.Checked)
            {
               graphDensity.Graficar((double)countReinas / (2 * tam), (double)countSoldado / (2 * tam), (double)countObreras / (2 * tam));
               graphEntrophy.Graficar((-countReinas) * Math.Log(countReinas, 2), (-countSoldado) * Math.Log(countSoldado, 2), (-countObreras) * Math.Log(countObreras, 2));
            }
            canvas.Invalidate();
            if (countHormigas <= 0)
            {
                timer.Stop();
                MessageBox.Show("La poblacion a muerto! sobrevivio hasta la generación " + frames);
            }
        }

        private List<Hormiga> GetNeighbor(Hormiga h,int[] posiciones)
        {
            List<Hormiga> ants = new List<Hormiga>();
            //regresamos todas las hormigas que tengan las coordenadas de la hormiga 
            //que este en la posicion "posicion" de h
            //y que sean del mismo tipo "tipo"
            for (int k = 0; k < posiciones.Length; k++)
            {
                switch (h.Orientation)
                {
                    case NORTH:
                        switch (posiciones[k])
                        {
                            case NORTH:

                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y - 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case SOUTH:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y + 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case EAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X + 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case WEAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X - 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                        }
                        break;
                    case SOUTH:
                        switch (posiciones[k])
                        {
                            case NORTH:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y + 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case SOUTH:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y - 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case EAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X - 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case WEAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X + 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                        }
                        break;
                    case EAST:
                        switch (posiciones[k])
                        {
                            case NORTH:

                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X + 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case SOUTH:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X - 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case EAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y + 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case WEAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y - 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                        }
                        break;
                    case WEAST:
                        switch (posiciones[k])
                        {
                            case NORTH:

                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X - 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case SOUTH:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X + 1 == hormigas.ElementAt(i).X && h.Y == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case EAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y - 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                            case WEAST:
                                for (int i = 0; i < hormigas.Count; i++)
                                {
                                    if (h.X == hormigas.ElementAt(i).X && h.Y + 1 == hormigas.ElementAt(i).Y && (hormigas.ElementAt(i).Tipo == SOLDADO || hormigas.ElementAt(i).Tipo == REINA))
                                        ants.Add(hormigas.ElementAt(i));
                                }
                                break;
                        }
                        break;
                }
            }
            return ants;
        }

        private void BtnGeneratePopulation_Click(object sender, EventArgs e)
        {
            int population = int.Parse(txtPopulation.Text);
            int x, y,randX,randY,cordX,cordY;
            padding = (int) (double.Parse(txtPadding.Text) / 100 * (tam*cellSize));
            //se tiene que generar a las hormigas no tan separadas para que sea posible
            //el padding se dedicara a eso, las hormigas se generaran en +- el valor del padding
            randX = random.Next(padding, (tam * cellSize) - padding);
            randY = random.Next(padding, (tam * cellSize) - padding);
            cordX = (int)(randX / cellSize);
            cordY = (int)(randY / cellSize);
            Hormiga h = CrearHormiga(cordX, cordY, pReina);
            hormigas.Add(h);
            AgregarHormigaAMatrix(h);
            for (int i = 0; i < population-1; i++)
            {
                //obtenemos unas coordenanas aleatorias que esten dentro del rango delimitado por el padding
                randX = random.Next(padding, (tam*cellSize) - padding);
                randY = random.Next(padding, (tam*cellSize) - padding);
                //mapeamos las coordenadas a la matriz 
                cordX = (int)(randX / cellSize);
                cordY = (int)(randY / cellSize);
                double randTipo = random.NextDouble();
                h = CrearHormiga(cordX, cordY, 0.08 + (1 - 0.08) * randTipo);
                hormigas.Add(h);
                AgregarHormigaAMatrix(h);
            }
            canvas.Invalidate();
        }
        private void AgregarHormigaAMatrix(Hormiga hormiga)
        {
            switch (hormiga.Tipo)
            {
                case REINA:
                    matrix[hormiga.X, hormiga.Y] = REINA;
                    break;
                case SOLDADO:
                    matrix[hormiga.X, hormiga.Y] = SOLDADO;
                    break;
                case OBRERA:
                    matrix[hormiga.X, hormiga.Y] = OBRERA;
                    break;
            }
        }
        private Hormiga CrearHormiga(int x,int y,double probabilidad) {
            countHormigas++;
            if (probabilidad <= pReina)
            {
                //nace una reina
                countReinas++;
                return new Hormiga(random.Next(5, 8), x, y, REINA);
            }
            else if (probabilidad > pReina && probabilidad <= (pSoldado+pReina))
            {
                countSoldado++;
                //nace un soldado
                return new Hormiga(random.Next(5, 8), x, y, SOLDADO);
            }
            else if (probabilidad > (pSoldado + pReina) && probabilidad <= (pObrera+pSoldado+pReina))
            {
                countObreras++;
                //nace una obrera
                return new Hormiga(random.Next(5, 8), x, y, OBRERA);
            }
            else
                return null;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            switch (Status)
            {
                case 0: //modo normal
                    btnPlay.Text = "Stop";
                    btnPause.Enabled = true;
                    Status = 2; //indica que esta en modo de iteracion del algoritmo
                    timer.Interval = int.Parse(txtInterval.Text);
                    timer.Start();
                    if (checkGraficar.Checked)
                    {
                        graphDensity = new Grafica("Density", "Reinas", "Soldado", "Obreras");
                        graphEntrophy = new Grafica("Entrophy", "Reinas", "Soldado", "Obreras");
                        graphDensity.Show();
                        graphEntrophy.Show();
                    }
                    break;
                case 2: //modo iterando
                case 3: //modo stop
                    countHormigas = 0;
                    countSoldado = 0;
                    countReinas = 0;
                    countObreras = 0;
                    btnPause.Enabled = false;
                    btnPlay.Text = "Play";
                    timer.Stop();
                    GenerarMatriz();
                    canvas.Invalidate();
                    Status = 0;//regresando a modo normal
                    frames = 0;
                    labelGenerations.Text = "" + 0;
                    hormigas = new List<Hormiga>();
                    labelNumHormigas.Text = "" + 0;
                    labelReinas.Text = "" + 0;
                    labelSoldado.Text = "" + 0;
                    labelObreras.Text = "" + 0;
                    if (checkGraficar.Checked)
                    {
                        graphDensity.Dispose();
                        graphEntrophy.Dispose();
                    }
                    break;
            }
            
            
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            switch (Status)
            {
                case 2: //modo iterando
                    btnPause.Text = "Resume";
                    timer.Stop();
                    Status = 3; //modo iteracion pausada
                    break;
                case 3:
                    btnPause.Text = "Pause";
                    timer.Start();
                    Status = 2; //modo iterando
                    break;
            }
        }

        public Form1()
        {
            InitializeComponent();
            canvas.Width = cellSize * tam;
            canvas.Height = cellSize * tam;
            flowPanel.AutoScroll = true;
            flowPanel.Controls.Add(canvas);
            GenerarMatriz();
            hormigas = new List<Hormiga>();
            bBLACK = Brushes.Black;
            bWHITE = Brushes.White;
            bRED = Brushes.Red;
            bBLUE = Brushes.Blue;
            bGREEN = Brushes.Green;
            Status = 0;//indicamos que estamos en status normal
            random = new Random();
        }

        private void BtnCrearHormiga_Click(object sender, EventArgs e)
        {
            switch (Status)
            {
                case 0: //modo normal
                    btnCrearHormiga.Text = "Cancelar";
                    Status = 1; //indicamos que esta en modo agregar hormiga
                    break;
                case 1: //modo agregar hormiga
                    btnCrearHormiga.Text = "Crear hormiga";
                    Status = 0;
                    break;
                case 3: //modo pausado
                    btnCrearHormiga.Text = "Crear hormiga";
                    Status = 4; //indicando modo pausado agregando hormigas
                    break;
            }
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int cordX = (int)(e.X / cellSize);
            int cordY = (int)(e.Y / cellSize);
            switch (Status)
            {
                case 1: //modo agregar hormiga
                case 4: //modo pausado agregar hormigas
                    hormigas.Add(new Hormiga(GetOrientationFromForm(),cordX,cordY,OBRERA));
                    countHormigas++;
                    countObreras++;
                    matrix[cordX, cordY] = 2;
                    canvas.Invalidate();
                    labelNumHormigas.Text =""+ countHormigas;
                    labelObreras.Text = "" + countObreras;
                    break;
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            for (int i = 0; i < tam; i++)
                for (int j = 0; j < tam; j++)
                    switch (matrix[i, j])
                    {
                        case DEAD:
                            g.FillRectangle(bBLACK, i * cellSize, j * cellSize, cellSize, cellSize);
                            break;
                        case 1:
                            g.FillRectangle(bWHITE, i * cellSize, j * cellSize, cellSize, cellSize);
                            break;
                        case REINA:
                            g.FillRectangle(bRED, i * cellSize, j * cellSize, cellSize, cellSize);
                            break;
                        case SOLDADO:
                            g.FillRectangle(bBLUE, i * cellSize, j * cellSize, cellSize, cellSize);
                            break;
                        case OBRERA:
                            g.FillRectangle(bGREEN, i * cellSize, j * cellSize, cellSize, cellSize);
                            break;
                    }  
        }

        private void GenerarMatriz()
        {
            matrix = new int[tam, tam];
            for (int i = 0; i < tam; i++)
                for (int j = 0; j < tam; j++)
                    matrix[i, j] = DEAD;
        }

        private int NextOrientation(Hormiga h,int giro)
        {
            //get new orientation
            switch (h.Orientation)
            {
                case NORTH:
                    if (giro == LEFT)
                        return WEAST;
                    else
                        return EAST;
                case SOUTH:
                    if (giro == LEFT)
                        return EAST;
                    else
                        return WEAST;
                case EAST:
                    if (giro == LEFT)
                        return NORTH;
                    else
                        return SOUTH;
                case WEAST:
                    if (giro == LEFT)
                        return SOUTH;
                    else
                        return NORTH;
                default:
                    return -1;
            }
        }

        private void NextStep(Hormiga h,int orientation)
        {
            try
            {
                UpdateColor(h);
                h.Orientation = orientation;
                switch (orientation)
                {
                    case NORTH:
                        h.Y--;
                        break;
                    case SOUTH:
                        h.Y++;
                        break;
                    case EAST:
                        h.X++;
                        break;
                    case WEAST:
                        h.X--;
                        break;
                }
                h.Color = matrix[h.X, h.Y];
                matrix[h.X, h.Y] = h.Tipo;
            }
            catch(IndexOutOfRangeException ex)
            {
                timer.Stop();
            }
        }

        private int GetOrientationFromForm()
        {
            switch (comboOrientation.SelectedIndex)
            {
                case 0:
                    return NORTH;
                case 1:
                    return SOUTH;
                case 2:
                    return EAST;
                case 3:
                    return WEAST;
                default:
                    MessageBox.Show("Error al obtener la orientacion");
                    return -1;
            }
        }
        /*Remplaza el color que tenia la celda dodne estaba la hormiga por el color que debe tener cuando pasa por esa celda*/
        private void UpdateColor(Hormiga h) {
            if(h.Color == Hormiga.MUERTA)
            {
                h.Color = Hormiga.WHITE;
                matrix[h.X, h.Y] = Hormiga.WHITE;
            }
            else
            {
                h.Color = Hormiga.MUERTA;
                matrix[h.X, h.Y] = Hormiga.MUERTA;
            }
        }

        
    }
}
