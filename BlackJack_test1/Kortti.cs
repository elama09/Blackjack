using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_test1
{
    public class Kortti
    {
        public string Maa { get; set; }
        public int Arvo { get; set; }

        public Kortti(string maa, int arvo)
        {
            Maa = maa;
            Arvo = arvo;
        }

        public Kortti(Kortti kortti)
        {
            Maa = kortti.Maa;
            Arvo = kortti.Arvo;
        }

        public override string ToString()
        {
            return Maa + " " + Arvo;
        }

        public static int operator +(Kortti a, Kortti b)
        {
            return (a.Arvo + b.Arvo);
        }
    }
}
