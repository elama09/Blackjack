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
            int summa = int.Parse(Console.ReadLine());
            if (summa < 1)
            {
                Console.WriteLine("Liian pieni pelisumma, oletuksena siirretään 50€");
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
            int summa = int.Parse(Console.ReadLine());

            while (int.TryParse(Console.ReadLine(), out int res))
            {

            }
            
            
            if (summa <= Rahat)
            {
                Rahat -= summa;
                NykyinenPanostus = summa;
            }
            else
            {
                Console.WriteLine("Sinulla ei ole riittävästi rahaa.");
            }
        }

        public void MaksaVoitto()
        {
            Console.WriteLine("Voitit juuri " + NykyinenPanostus + "€");
            Rahat += NykyinenPanostus;
            NykyinenPanostus = 0;
        }

        public void Häviö()
        {
            NykyinenPanostus = 0;
            if (Rahat <= 0)
            {
                Console.WriteLine("Hävisit kaikki rahat!");
                // Todo game over
            }
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

    }
}
