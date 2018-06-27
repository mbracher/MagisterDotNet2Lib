using System;
namespace MagisterDotNet2Lib
{
    public class Cijfer
    {
        public Cijfer(decimal waarde, decimal weging)
        {
            Waarde = waarde;
            Gewicht = weging;
        }

        public decimal Waarde { get; set; }
        public decimal Gewicht { get; set; }
    }
}
