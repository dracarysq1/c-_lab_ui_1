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
    struct DataItem : IComparable<DataItem>, ISerializable
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
        public DataItem(SerializationInfo info, StreamingContext streamingContext)
        {
            float x = info.GetSingle(constants.DataItemElems.VecComponentsNames.X);
            float y = info.GetSingle(constants.DataItemElems.VecComponentsNames.Y);
            vec = new System.Numerics.Vector2(x, y);
            field = info.GetSingle(constants.DataItemElems.FieldName);
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
        int IComparable<DataItem>.CompareTo(DataItem other) /* New */
        {
           
            return this.vec.Length() > other.vec.Length() ? 1 : this.vec.Length() == other.vec.Length() ? 0 : -1;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(constants.DataItemElems.FieldName, field);
            info.AddValue(constants.DataItemElems.VecComponentsNames.X, vec.X);
            info.AddValue(constants.DataItemElems.VecComponentsNames.Y, vec.Y);
            
        }
    }
}
    


