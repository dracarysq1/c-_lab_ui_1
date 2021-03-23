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


namespace DataLibrary
{
    [Serializable]
    abstract class V3Data 
    {
        
        public string info { get; set; }
        public DateTime t0 { get; set; }
        public V3Data(string info = "info", DateTime t0 = default(DateTime))
        {
            this.info = info;
            this.t0 = t0;

        }

        public string Info
        {
            get
            { return info; }
            set
            { info = value; }
        }

        public DateTime T0
        {
            get { return t0; }
            set
            {
                t0 = value;

            }
        }
        abstract public Vector2[] Nearest(Vector2 v);
        abstract public string ToLongString();
        abstract public string ToLongString(string format);
        public override string ToString()
        {
            return $"info={info} t0={t0.ToShortDateString()}";

        }

        

};
}
