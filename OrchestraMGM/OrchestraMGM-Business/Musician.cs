using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestraMGM_Business
{
    public class Musician
    {
        public long Id { get; set; }
        public string Voornaam { get; set; }
        public string Naam { get; set; }
        public string Adres { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }
        public string Instrument { get; set; }
    }
}
