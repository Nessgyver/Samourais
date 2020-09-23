using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samourais.Models
{
    public class SamouraiVM
    {
        public Samourai Samourai{ get; set; }
        public List<Arme> ListeArmes { get; set; }
        public int? IdArme { get; set; }
        public List<ArtMartial> ListeArtMartials { get; set; }
        public List<int> idArtMartials { get; set; }
    }
}