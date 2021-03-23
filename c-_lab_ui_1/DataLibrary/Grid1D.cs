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
    struct Grid1D
    {
        public float step { get; set; }

        public int n { get; set; }
        public Grid1D(
            float step = constants.field,
            int n = constants.n
            )
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
        public static Grid1D Parse(string string_data)
        {
            string[] parse_data = string_data.Split(' ');
            Grid1D grid = new Grid1D();
            try
            {
                grid = new Grid1D(
                    (float)Convert.ToDouble(parse_data[1]), Convert.ToInt32(parse_data[2])
                    );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exception next = new Exception("Handled");
                throw next;
            }
            return grid;
        }

    }

    
}
