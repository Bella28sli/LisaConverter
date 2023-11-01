using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaConverter
{
    public class Paric
    {
        public string Parica;
        public int Price;
        public string Style;
        public Paric(string Parica, int Price, string Style) 
        {
            this.Parica = Parica;
            this.Price = Price;
            this.Style = Style;
        }
        public Paric() { }
    }

}
