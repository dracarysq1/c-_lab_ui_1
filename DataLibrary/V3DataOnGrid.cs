
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Globalization;

namespace DataLibrary
{
    [Serializable]
    [KnownType(typeof(Grid1D))]

    class V3DataOnGrid : V3Data, IEnumerable<DataItem>
    {
        //private static readonly DateTime t;
#pragma warning disable IDE0044 // 添加只读修饰符
       // private static string s;
#pragma warning restore IDE0044 // 添加只读修饰符

        // Два свойства типа Grid1D определяют двумерную сетку.
        // В каждом узле сетки задано одно значение типа double,
        // эти значения хранятся в двумерном массиве cle.

        public Grid1D cyc_x;//{ get; set; }
   
        public Grid1D cyc_y;// { get; set; }
        
        public double[,] cle { get; set; }
        //public Vector2[] value { get; set; }
        public V3DataOnGrid(string y, DateTime t0, Grid1D cyc_x, Grid1D cyc_y) : base(y, t0)
        {
            this.cyc_x = cyc_x;
            this.cyc_y = cyc_y;
            cle = new double[cyc_x.n, cyc_y.n];
            //this.value = new Vector2[cyc_x.n];
            // Память для массива выделена, но все значения равны 0.
        }
        public V3DataOnGrid(string filename) 
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string[] strlines = File.ReadAllLines(filename, Encoding.Default);

            int i = 0;
            info = strlines[i];
            t0 = DateTime.Parse(strlines[i + 1]);


            cyc_x.step = float.Parse(strlines[i + 2]);
            cyc_x.n = Convert.ToInt32(strlines[i + 3]);
            cyc_y.step = float.Parse(strlines[i + 4]);
            cyc_y.n = Convert.ToInt32(strlines[i + 5]);
            cle = new double[cyc_x.n, cyc_y.n];
            int l = 6;
            for (int k = 0; k < cyc_x.n; k++)
            {
                for (int j = 0; j < cyc_y.n; j++)
                {
                    cle[k, j] = Convert.ToDouble(strlines[l]);
                    l++;
                }

            }
        }
       
        public void InitRandom(double minValue, double maxValue)
        {
            
            Random arr = new Random();

            for (int i = 0; i < cyc_x.n; i++)
            {
                for (int j = 0; j < cyc_y.n; j++)
                {
                    cle[i, j] = minValue + arr.NextDouble() * (maxValue - minValue);
                }
            }


        }
        public static explicit operator V3DataCollection(V3DataOnGrid source)
        {
            V3DataCollection v3DataCollection = new V3DataCollection(source.info, source.t0);
            for (int i = 0; i < source.cyc_x.n; i++)
            {
                for (int j = 0; j < source.cyc_y.n; j++)
                {
                    float x_coord = source.cyc_x.step * i;
                    float y_coord = source.cyc_y.step * j;

                    Vector2 vec = new Vector2(x_coord, y_coord);
                    double field = source.cle[i, j];
                    v3DataCollection.list.Add(new DataItem(field, vec));
                }
            }
            return v3DataCollection;
        }
       

        public override Vector2[] Nearest(Vector2 v)
        {
            // На равном расстоянии может находиться несколько узлов сетки,
            // например, если сетка состоит из четырех узлов, то все узлы
            // находятся на равном рассоянии от точки пересечения диагоналей.
            double min, a;
            int count = 0;
            double x, y;


            Vector2 vec = new Vector2();
            min = float.MaxValue; // надо взять самое большое значение            
                                  // i,j - это не координаты узла, а индексы в двумерном массиве, такие 
                                  // cle[i,j] - это значение поля в данном узле
                                  // x координата узла x_coord = cyc_x.step * i;
                                  // y координата узла y_coord = cyc_y.step * j;

            for (int i = 0; i < cyc_x.n; i++)
            {
                for (int j = 0; j < cyc_y.n; j++)
                {
                    float x_coord = cyc_x.step * i;
                    float y_coord = cyc_y.step * j;
                    a = System.Math.Sqrt((x_coord - v.X) * (x_coord - v.X) + (y_coord - v.Y) * (y_coord - v.Y));
                    //Console.WriteLine($"V3DataOnGrid.Nearest: i = {i} j = {j} x_coord = {x_coord} y_coord = {y_coord} a = {a}");
                    if (a < min)
                    {
                        min = a;
                        count++;
                        x = x_coord;
                        y=y_coord;

                    }

                }
            }
            Vector2[] ret = new Vector2[count];
            
            count = 0;
            for (int i = 0; i < cyc_x.n; i++)
            {
                for (int j = 0; j < cyc_y.n; j++)
                {
                    float x_coord = cyc_x.step * i;
                    float y_coord = cyc_y.step * j;
                    a = System.Math.Sqrt((x_coord - v.X) * (x_coord - v.X) + (y_coord - v.Y) * (y_coord - v.Y));
                    if (((a == min) && (vec.X != x_coord)) || ((a == min) && (vec.Y != y_coord)))
                    {
                        ret[count++].X = vec.X;
                        ret[count].Y = vec.Y;
                        
                        
                    }
                }
            }
           
            return ret;
        }


        public override string ToString()
        {

            return $"V3DataOnGrid: \n{base.ToString()} {cyc_x} {cyc_y}"; // base.ToString() вызлв метода из базового класса
        }

        public override string ToLongString()
        {

            string a = "\nV3DataOnGrid: " + ToString();

            for (int i = 0; i < cyc_x.n; i++)
                for (int j = 0; j < cyc_y.n; j++)
                {
                    float x_coord = cyc_x.step * i;
                    float y_coord = cyc_y.step * j;

                    a += $"\nx = {x_coord} y = {y_coord} value = {cle[i, j]}";
                }
            a += "\n";
            return a;
        }
        public string InFo
        {
            get
            {
                string a = "";

                for (int i = 0; i < cyc_x.n; i++)
                    for (int j = 0; j < cyc_y.n; j++)
                    {
                        float x_coord = cyc_x.step * i;
                        float y_coord = cyc_y.step * j;

                        a += $"\nx = {x_coord} y = {y_coord} value = {cle[i, j]}";
                    }
                a += "\n";
                return a;
            }
        }
        public override string ToLongString(string format)
        {
            string str = "\nV3DataOnGrid: " + ToString(); ;

            for (int i = 0; i < cyc_x.n; i++)
                for (int j = 0; j < cyc_y.n; j++)
                {
                    float x_coord = cyc_x.step * i;
                    float y_coord = cyc_y.step * j;
                    str += String.Format(format, cyc_x.step, cyc_x.n, cyc_y.step, cyc_y.n, x_coord, y_coord, cle[i, j]);

                    str += "\n";
                    //a += $"\nx = {x_coord} y = {y_coord} value = {cle[i, j]}";
                }

            return str;
        }
        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {

            V3DataCollection collection_data = (V3DataCollection)this;
            return collection_data.list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            V3DataCollection collection_data = (V3DataCollection)this;
            return collection_data.list.GetEnumerator();
        }
       
             
        
    };
}
