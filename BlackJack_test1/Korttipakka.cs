﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_test1
{
    public class Korttipakka : IEnumerable
    {
        public const int KORTTEJA_YHTEENSA = 52;
        public Random random = new Random(DateTime.Now.Millisecond);
        public List<Kortti> PakkaList { get; set; }
        public static int PakkojenMaara { get; set; }
        
        public Korttipakka(int montaPakkaa) //Tähän voi lisätä parametsiksi, kuinka monta pakkaa haluat tehdä. Sit 2 looppia x - 13.
        {
            PakkaList = new List<Kortti>();
            PakkojenMaara = montaPakkaa;

            for (int j = 0; j < PakkojenMaara; j++) // Monta pakkaa tehdään?
            {
                for (int i = 2; i < 15; i++)
                {
                    switch (i)
                    {
                        case 11:
                            PakkaList.Add(new Kortti("♥ Jätkä", 10));
                            PakkaList.Add(new Kortti("♣ Jätkä", 10));
                            PakkaList.Add(new Kortti("♦ Jätkä", 10));
                            PakkaList.Add(new Kortti("♠ Jätkä", 10));
                            break;
                        case 12:
                            PakkaList.Add(new Kortti("♥ Akka", 10));
                            PakkaList.Add(new Kortti("♣ Akka", 10));
                            PakkaList.Add(new Kortti("♦ Akka", 10));
                            PakkaList.Add(new Kortti("♠ Akka", 10));
                            break;
                        case 13:
                            PakkaList.Add(new Kortti("♥ Kunkku", 10));
                            PakkaList.Add(new Kortti("♣ Kunkku", 10));
                            PakkaList.Add(new Kortti("♦ Kunkku", 10));
                            PakkaList.Add(new Kortti("♠ Kunkku", 10));
                            break;
                        case 14:
                            PakkaList.Add(new Kortti("♥ Ässä", 11));
                            PakkaList.Add(new Kortti("♣ Ässä", 11));
                            PakkaList.Add(new Kortti("♦ Ässä", 11));
                            PakkaList.Add(new Kortti("♠ Ässä", 11));
                            break;
                        default:
                            PakkaList.Add(new Kortti("♥", i));
                            PakkaList.Add(new Kortti("♣", i));
                            PakkaList.Add(new Kortti("♦", i));
                            PakkaList.Add(new Kortti("♠", i));
                            break;
                    }
                }
            }
            
        }

        public Kortti otaKorttiPakasta()
        {
            int randomIndeksi = random.Next(0, PakkaList.Count);
            
            Kortti arvottu = PakkaList[randomIndeksi];
            Kortti kopio = new Kortti(PakkaList[randomIndeksi]);

            PakkaList.RemoveAt(randomIndeksi);
            Console.WriteLine(kopio.ToString());
            return kopio;
        }


        //public void PakanSekoitus(Korttipakka pakka) Ei käytössä nyt, koska luodaan sama pakka uusiksi.
        //{
        //    pakka.PakkaList.Clear();
        //    pakka = new Korttipakka(); 
        //}

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)PakkaList).GetEnumerator();
        }

        public override string ToString()
        {
            return $"Pakassa on {PakkaList.Count} korttia."; 
        }

        public void TulostaKaikkiKortit()
        {
            foreach (var item in PakkaList)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
