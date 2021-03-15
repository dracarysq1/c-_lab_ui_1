using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;
using System.Dynamic;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;


namespace DataLibrary
{
    [Serializable]
    class V3DataCollection : V3Data, IEnumerable<DataItem>
    {
        public List<DataItem> list = new List<DataItem>();


        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }




        public V3DataCollection(string z, DateTime t0) : base(z, t0)
        {
            this.list = new List<DataItem>(); // память распределена, но число элементов равно 0.
        }
        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {

            Random arr_double = new Random();

            for (int i = 0; i < nItems; i++)
            {

                float x = ((float)arr_double.NextDouble()) * xmax;
                float y = ((float)arr_double.NextDouble()) * ymax;
                double value = minValue + arr_double.NextDouble() * (maxValue - minValue);
                Vector2 v2 = new Vector2(x, y);
                var cy = new DataItem(value, v2);
                list.Add(cy);
            }
        }

        // В Distance (System.Numerics.Vector2 value1, System.Numerics.Vector2 value2);Vector2 есть метод 
        //  Distance (System.Numerics.Vector2 value1, System.Numerics.Vector2 value2); - расстояние между двумя заданными точками и
        // DistanceSquared(System.Numerics.Vector2 value1, System.Numerics.Vector2 value2); - квадрат евклидова расстояния между двумя заданными точками.
        public override Vector2 Nearest(Vector2 v)
        {
            double min, a;

            Vector2 vec = new Vector2();

            min = float.MaxValue; // надо взять самое большое значение

            for (int i = 0; i < list.Count; i++)
            {
                a = System.Math.Sqrt((list[i].vec.X - v.X) * (list[i].vec.X - v.X) + (list[i].vec.Y - v.Y) * (list[i].vec.Y - v.Y));
                Console.WriteLine($"V3DataCollection.Nearest: i = {i} list[i].vec = {list[i].vec} a = {a}");
                if (a < min)
                {
                    min = a;

                    vec.X = list[i].vec.X;
                    vec.Y = list[i].vec.Y;

                }
            }
            Console.WriteLine($"vec = {vec}");
            for (int i = 0; i < list.Count; i++)
            {
                a = System.Math.Sqrt((list[i].vec.X - v.X) * (list[i].vec.X - v.X) + (list[i].vec.Y - v.Y) * (list[i].vec.Y - v.Y));
                if (((a == min) && (vec.X != list[i].vec.X)) || ((a == min) && (vec.Y != list[i].vec.Y)))
                {
                    vec.X = list[i].vec.X;
                    vec.Y = list[i].vec.Y;
                    Console.WriteLine($"vec = {vec}");
                }
            }
            return vec;
        }
        public override string ToString()
        {

            return $"info={info} t0={t0} ";
        }
        public override string ToLongString()
        {
            string a = "\nV3DataCollection: " + ToString();

            for (int i = 0; i < list.Count; i++)
            {
                a += "\n" + list[i];
            }
            a += "\n";
            return a;
        }
        public override string ToLongString(string format)
        {
            string str = "\nV3DataCollection: " + ToString();

            for (int i = 0; i < list.Count; i++)
            {
                str += String.Format(format, list[i]);
                //str += "\n" + list[i]; 

            }
            str += "\n";
            return str;
        }
    };
}