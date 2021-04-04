using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;
using System.Dynamic;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DataLibrary
{ 
[Serializable]
    struct DataItem : ISerializable
    {

        public double field { get; set; } // это значение поля
        public Vector2 vec { get; set; } // это координаты точки на сетке
                                         // vec.X - координата по оси Ox
                                         // vec.Y - координата по оси Oy




        public DataItem(double field, Vector2 vec)
        {
            this.field = field;
            this.vec = vec;
        }
        
        public string Tostring(string format)
        {
            return "Vector: " + vec.X.ToString(format) + " " + vec.Y.ToString(format) + " " +
                   "field: " + field.ToString(format);
        }

        public override string ToString()
        {

            return "Vector: " + vec.X.ToString() + " " + vec.Y.ToString() + " " +
                   "field: " + field.ToString();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Vector_X", vec.X);
            info.AddValue("Vector_Y", vec.Y);
            info.AddValue("Field", field);
        }

        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = info.GetSingle("Vector_X");
            float y = info.GetSingle("Vector_Y");
            vec = new Vector2(x, y);
            field = (double)info.GetValue("Field", typeof(double));
        }
       

    }
}
    


