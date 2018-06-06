using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_test1
{
    public class Käsi
    {
        public int _kadenArvo;
        public bool _blackjack;
        public int NykyKorttiArvo { get; set; }
        public List<Kortti> Kadenkortit { get; set; }
        public bool BlackJack
        {
            get { return _blackjack;}
        }
        public Käsi()
        {
            KadenArvo = 0;
            NykyKorttiArvo = 0;
            _blackjack = false;
            
            Kadenkortit = new List<Kortti>();
        }

        //public Käsi(int kadenArvo, int nykyKorttiArvo) // Tarviiko tätä ollenkaan?
        //{
        //    KadenArvo = kadenArvo;
        //    NykyKorttiArvo = nykyKorttiArvo;
        //}

        public void OnkoBlackJack()
        {
            if (KadenArvo == 21 && Kadenkortit.Count == 2)
            {
                _blackjack = true;
            }
            else
            {
                _blackjack = false;
            }
        }

        public static void TarkistaJaMuutaAssatYkkosiksi(Käsi k)
        {
            int assiaYhteensa = 0;
            foreach (var item in k.Kadenkortit)
            {
                if (item.Arvo == 11)
                {
                    assiaYhteensa++;
                }
            }
            if (k.KadenArvo > 21 && assiaYhteensa > 0)
            {
                foreach (var item in k.Kadenkortit)
                {
                    if (item.Arvo == 11)
                    {
                        item.Arvo = 1;
                        break;
                    }
                }
            }
        }

        public int KadenArvo
        {
            get
            {
                int summa = 0;
                foreach (var item in Kadenkortit)
                {
                    summa += item.Arvo;
                }
                return summa;
            }
            set
            {
                _kadenArvo = value;
            }
        }
    }

}
