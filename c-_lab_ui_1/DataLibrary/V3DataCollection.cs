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
        
        public List<DataItem> list { get; set; }


        public IEnumerator<DataItem> GetEnumerator()
        {
            return ((IEnumerable<DataItem>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    




        public V3DataCollection(string z, DateTime t) : base(z, t)
        {
            list = new List<DataItem>(); // память распределена, но число элементов равно 0.
            info = z;
            t0 = t;
        }
        /*public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {

            list = new List<DataItem>();
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
        }*/
        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            list = new List<DataItem>();
            Random rnd = new Random();
            for (int i = 0; i < nItems; i++)
            {
                list.Add(new DataItem()
                {
                    vec = new Vector2((float)rnd.NextDouble() * xmax, (float)rnd.NextDouble() * ymax),
                    field = minValue + rnd.NextDouble() * (maxValue - minValue)
            });
            }
        }
        // В Distance (System.Numerics.Vector2 value1, System.Numerics.Vector2 value2);Vector2 есть метод 
        //  Distance (System.Numerics.Vector2 value1, System.Numerics.Vector2 value2); - расстояние между двумя заданными точками и
        // DistanceSquared(System.Numerics.Vector2 value1, System.Numerics.Vector2 value2); - квадрат евклидова расстояния между двумя заданными точками.
        public override Vector2[] Nearest(Vector2 v)
        {
            double min, a = 0;
            int count = 0, mincount = 0;


            min = float.MaxValue; // надо взять самое большое значение

            foreach (DataItem item in list)
            {
                a = Vector2.Distance(item.vec, v);
                count++;
                if (a < min)
                {
                    min = a;
                    mincount = count;
                }
            }
            Vector2[] ret = new Vector2[count];
            foreach (DataItem item in list)
            {

                count = 0;
                a = Vector2.Distance(item.vec, v);
                if (a == min)
                {
                    ret[count++] = item.vec;
                }
            }

            return ret;
        }
            
            

            /*for (int i = 0; i < list.Count; i++)
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
            }*/
            
        
        public override string ToString()
        {

            return $"V3DataCollection:\n info={info} t0={t0}  ";
        }
        public override string ToLongString()
        {
            string a = "";

            foreach (DataItem item in list)
            {
                a += (item.ToString() + "\n");
            }

            return "V3dataCollection: info:" + info + "DateTime: " + t0.ToString() + "\n" + a;
        }
        public override string ToLongString(string format)
        {
            string str = "";

            foreach (DataItem item in list)
            {
                str += (item.Tostring(format) + "\n");
            }
            return "V3dataCollection: info:" + info + "DateTime: " + t0.ToString(format) + "\n" + str;
        }
    };
}