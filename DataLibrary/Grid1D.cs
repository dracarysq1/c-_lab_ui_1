using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;
using System.Dynamic;
using System.IO;

namespace DataLibrary
{
    [Serializable]
    struct Grid1D
    {
        public float step { get; set; }

        public int n { get; set; }
        public Grid1D(float step,int n)
        {

            this.step = step;
            this.n = n;
        }
        public override string ToString()
        {
            return "Step: " + step.ToString() + "; Num: " + n.ToString();
        }

        public string ToString(string format)
        {
            return "Step: " + step.ToString(format) + "; Num: " + n.ToString(format);
        }
       

    }

    
}
