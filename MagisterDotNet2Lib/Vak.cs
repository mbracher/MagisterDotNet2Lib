using System;
using System.Collections.Generic;

namespace MagisterDotNet2Lib
{
    public class Vak
    {
        private List<Cijfer> cijfers;

        public List<Cijfer> Cijfers { get { return cijfers; } }

        private string naam;
        public string Naam { get { return naam; } }

        public Vak(string Naam)
        {
            this.naam = Naam;
            cijfers = new List<Cijfer>();
        }

        public void AddCijfer(decimal waarde, decimal weging)
        {
            cijfers.Add(new Cijfer(waarde, weging));
        }
        public void AddCijfer(Cijfer cijfer)
        {
            cijfers.Add(cijfer);
        }

        public decimal Eindcijfer()
        {
            decimal totaalGewicht = 0;
            decimal totaal = 0;
            foreach (Cijfer c in cijfers)
            {
                totaalGewicht = totaalGewicht + c.Gewicht;
                totaal = totaal + (c.Gewicht * c.Waarde);
            }

            return Math.Round(totaal / totaalGewicht, 1, MidpointRounding.AwayFromZero);

        }

        public decimal WatMoetIkHalen(decimal gewensteEindCijfer, decimal weging)
        {
            decimal totaalGewicht = weging;
            decimal totaal = 0;
            foreach (Cijfer c in cijfers)
            {
                totaalGewicht = totaalGewicht + c.Gewicht;
                totaal = totaal + c.Gewicht * c.Waarde;
            }

            decimal cijfer = (gewensteEindCijfer * totaalGewicht - totaal) / weging;

            if (cijfer < 0) {
                return 0;
            }

            return Math.Round(cijfer, 1, MidpointRounding.AwayFromZero);

        }
       

    }
}
