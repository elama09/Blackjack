using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_test1
{
    public class Tili
    {
        public int Rahat { get; set; }
        public int NykyinenPanostus { get; set; }

        public Tili()
        {
            Console.Write("Paljonko laitetaan rahaa pelitilille? ");
            int summa = int.TryParse(Console.ReadLine(), out summa) ? summa : 0;
            if (summa < 1)
            {
                Console.WriteLine("Ei onnistu.. Oletuksena siirretään 50€");
                Rahat = 50;
            }
            else
            {
                Rahat = summa;
            }
        }

        public void LaitaPanos()
        {
            // Todo exception handling
            Console.Write("Paljonko haluat panostaa seuraavaan käteen? ");
            while (true)
            {
                int summa = int.TryParse(Console.ReadLine(), out summa) ? summa : 0;

                if (summa <= Rahat && summa > 0)
                {
                    Rahat -= summa;
                    NykyinenPanostus = summa;
                    break;
                }
                else
                {
                    Console.WriteLine("Sinulla ei ole riittävästi rahaa, tai liian pieni panos, tai omituinen luku. Kokeile uusiksi..");
                }
            }
        }

        public void MaksaVoitto()
        {
            Console.WriteLine("Voitit juuri " + NykyinenPanostus * 2 + "€");
            Rahat += NykyinenPanostus * 2;
            NykyinenPanostus = 0;
        }

        public void MaksaVoittoBlackjack()
        {
            Console.WriteLine("Blackjack voitto, voitit juuri " + (int)Math.Round(NykyinenPanostus * 2.5) + "€");

            Rahat += (int)Math.Round(NykyinenPanostus * 2.5);
            NykyinenPanostus = 0;
        }

        public void Häviö()
        {
            NykyinenPanostus = 0;
            if (Rahat <= 0)
            {
                Console.WriteLine("Hävisit kaikki rahat! Moi Moi!");
                // Todo game over
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(0);
            }
        }

        public void Tasapeli()
        {
            Console.WriteLine("Rahat takaisin.");
            Rahat += NykyinenPanostus;
            NykyinenPanostus = 0;
        }

        public void PanoksenTuplaus()
        {
            if (Rahat < NykyinenPanostus)
            {
                Console.WriteLine("Ei riittävästi rahaa tuplaukseen.");
            }
            else
            {
                Rahat -= NykyinenPanostus;
                NykyinenPanostus *= 2;
                Console.WriteLine("Panos tuplattu. Uusi panos " + NykyinenPanostus + "€");
            }
        }

        public void LisääRahaaTilille()
        {
            Console.Write("Kuinka paljon rahaa haluat siirtää pelitilillesi? ");
            while (true)
            {
                int summa = int.TryParse(Console.ReadLine(), out summa) ? summa : 0;
                if (summa > 0)
                {
                    Rahat += summa;
                }
                else
                {
                    Console.WriteLine("Omituinen luku tai negatiivinen luku, kokeile uusiksi..");
                }
            }
        }

        public void SuperVoitto()
        {
            Console.WriteLine("Onneksi olkoon! Sait supervoiton! Panoksesi vielä 50 kertaistetaan!");
            Rahat += NykyinenPanostus * 100;
            NykyinenPanostus = 0;
        }

    }
}
