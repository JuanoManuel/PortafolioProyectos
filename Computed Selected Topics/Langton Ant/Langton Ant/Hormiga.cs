using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langton_Ant
{
    class Hormiga
    {
        public const int MUERTA = 0, WHITE = 1, SOLDADO = 3,REINA = 2,OBRERA = 4;
        private int _orientation;
        public int Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }
        private int _x;
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        private int _y;
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        private int _tipo;
        public int Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
        private int _vida;
        public int Vida
        {
            get { return _vida; }
            set { _vida = value; }
        }
        private string _fertil;
        public string Fertil
        {
            get { return _fertil; }
            set { _fertil = value; }
        }
        private int _color;
        public int Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public Hormiga(int orientation,int x,int y,int tipo)
        {
            Orientation = orientation;
            X = x;
            Y = y;
            Color = WHITE;
            Tipo = tipo;
            switch (Tipo)
            {
                case REINA:
                    Vida = 70;
                    Fertil = "0,50";
                    break;
                case SOLDADO:
                    Fertil = "0,40";
                    Vida = 60;
                    break;
                case OBRERA:
                    Fertil = "-1,-1";
                    Vida = 2000;
                    break;
                default:
                    Fertil = "-1,-1";
                    Vida = 0;
                    break;
            }
        }

        public bool isFertil()
        {
            string[] f = Fertil.Split(',');
            if (Vida >= int.Parse(f[0]) && Vida <= int.Parse(f[1]))
                return true;
            return false;
        }
    }
}
