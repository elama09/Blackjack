using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_test1
{ // SPLIT!!!! PANOKSET!!!! RAHAT!!!!
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //Alku käynnistys niin saadaan samalla monella pakalla halutaan pelata
            Console.WriteLine("*****Tervetuloa pelaamaan BlackJack-pelia!*****\n");
            int pakkojenmaara = PelinToiminnot.Montapakkaa();

            PelinToiminnot.KaynnistaPeli(pakkojenmaara);
        }
    }

    public static class PelinToiminnot // Engine luokka..?
    {
        public static int pelaajanVoitot = 0;
        public static int jakajanVoitot = 0;
        public static int tasapelit = 0;
        public static bool OnkoTarpeeksiKorttejaPakassa(Korttipakka p)
        {
            if (p.PakkaList.Count < (Korttipakka.PakkojenMaara * 52 / 5))
            {
                Korttipakka pakka = new Korttipakka(Korttipakka.PakkojenMaara);
                
            }
            return true;
        }
        public static void KaynnistaPeli(int pakkojenMaara) // Tai muuta tälläistä
        {
            //Kysytään monta pakkaa pelaaja haluaa luoda, ja luodaan ne
            //int pakkojenMaara = int.Parse(Console.ReadLine());

            

            Korttipakka pakka = new Korttipakka(pakkojenMaara);

            Käsi pe = new Käsi();
            Käsi ja = new Käsi();

            //Otetaan kaksi korttia molemmille pakasta
            Console.WriteLine();
            Console.Write("Sinun kortti: ");
            System.Threading.Thread.Sleep(2000);
            pe.Kadenkortit.Add(pakka.otaKorttiPakasta());
            Console.Write("Jakajan kortti: ");
            System.Threading.Thread.Sleep(2000);
            ja.Kadenkortit.Add(pakka.otaKorttiPakasta());
            Console.Write("Sinun kortti: ");
            System.Threading.Thread.Sleep(2000);
            pe.Kadenkortit.Add(pakka.otaKorttiPakasta());
            Console.WriteLine();
            System.Threading.Thread.Sleep(2000);

            pe.OnkoBlackJack();
            if (pe.BlackJack)
            {
                Console.WriteLine("Sinulla on BlackJack!");
                JakajanVuoro(ja, pe, pakka);
                KumpiVoitti(pe, ja);
                PeliLoppui();
            }

            //Tarkistetaan tarviiko muuttaa Ässiä Ykkösiksi
            Käsi.TarkistaJaMuutaAssatYkkosiksi(pe);
            Käsi.TarkistaJaMuutaAssatYkkosiksi(ja);
            Console.WriteLine("Sinulla on " + pe.KadenArvo);
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Jakajalla on " + ja.KadenArvo);
            Console.WriteLine();

            HaluatkoTuplata(pe, ja, pakka); //Pelaajan vuoro/Tuplaus

            //Pelaajan vuoro
            PelaajanVuoro(pe, pakka);

            //Jakajan vuoro
            JakajanVuoro(ja, pe, pakka);

            //Kumpi voitti tarkistus
            KumpiVoitti(pe, ja);

            PeliLoppui();
        }
        public static void HaluatkoTuplata(Käsi pe, Käsi ja, Korttipakka pakka)
        {
            Console.WriteLine("Haluatko tuplata?\nY = Yes\nN = No");
            vääräkomento:
            string tuplausKomento = Console.ReadLine();
            if (tuplausKomento.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Tuplaus(pe, ja, pakka);
            }
            else if (!tuplausKomento.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                goto vääräkomento;
            }
            Console.WriteLine();
        }
        public static void PelaajanVuoro(Käsi kasi, Korttipakka pakka)
        {
            //Lisää ettei hyväksy muita komentoja.
            string toiminto = " ";

            while (true)
            {
                Console.WriteLine("Mitä tehdään?\nH = Hit\nS = Stand");
                toiminto = Console.ReadLine();
                Console.WriteLine();

                if (toiminto.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                else if (!toiminto.Equals("h",StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                    continue;
                }

                Console.Write("Sinun kortti: ");
                System.Threading.Thread.Sleep(2000);
                kasi.Kadenkortit.Add(pakka.otaKorttiPakasta());
                Käsi.TarkistaJaMuutaAssatYkkosiksi(kasi);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Sinulla on " + kasi.KadenArvo);

                if (kasi.KadenArvo > 21)
                {
                    Console.WriteLine("Sinulla yli! Jakaja voitti.");
                    jakajanVoitot++;
                    PeliLoppui();// Tähän muutos!!!!?????
                }
            }
        }

        public static void JakajanVuoro(Käsi kasi, Käsi pelaajankasi, Korttipakka pakka)
        {

            if (kasi.KadenArvo >= 17)
            {
                Console.WriteLine("Jakaja jää");
            }
            else
            {
                while (kasi.KadenArvo < 17)
                {
                    Console.WriteLine("Jakaja ottaa kortin..");
                    System.Threading.Thread.Sleep(2000);
                    kasi.Kadenkortit.Add(pakka.otaKorttiPakasta());
                    System.Threading.Thread.Sleep(2000);

                    kasi.OnkoBlackJack();
                    if (kasi.BlackJack == true && pelaajankasi.BlackJack == true)
                    {
                        Console.WriteLine("Jakajalla myös BlackJack! Tasapeli.");
                        tasapelit++;
                        PeliLoppui();
                    }
                    else if (kasi.BlackJack == true && pelaajankasi.BlackJack == false)
                    {
                        Console.WriteLine("Jakajalla on BlackJack! Sinä hävisit.");
                        jakajanVoitot++;
                        PeliLoppui();
                    }
                    else if (kasi.BlackJack == false && pelaajankasi.BlackJack == true)
                    {
                        pelaajanVoitot++;
                        PeliLoppui();
                    }

                    Käsi.TarkistaJaMuutaAssatYkkosiksi(kasi);
                    Console.WriteLine("Jakajalla on " + kasi.KadenArvo);

                    if (kasi.KadenArvo > 21)
                    {
                        Console.WriteLine("Jakaja yli! Sina Voitit!");
                        pelaajanVoitot++;
                        PeliLoppui();
                    }
                }
            }
        }

        public static void KumpiVoitti(Käsi pe, Käsi ja)
        {
            System.Threading.Thread.Sleep(2000);
            if (pe.KadenArvo > ja.KadenArvo)
            {
                Console.WriteLine("Sinä voitit! Onneksi olkoon!");
                pelaajanVoitot++;
            }
            else if (pe.KadenArvo < ja.KadenArvo)
            {
                Console.WriteLine("Sinä hävisit. Jakaja voitti.");
                jakajanVoitot++;
            }
            else if (pe.KadenArvo == ja.KadenArvo)
            {
                Console.WriteLine("Tasapeli!");
                tasapelit++;
            }
        }

        public static void PeliLoppui()
        {
            Console.WriteLine();
            Console.WriteLine("Peli loppui, mitä haluat tehdä seuraavaksi?\nP = Pelaa uudelleen\tL = Lopeta peli");
            Console.WriteLine("Pelaajan voitot: " + pelaajanVoitot + "\tJakajan voitot: " + jakajanVoitot + "\tTasapelit: " + tasapelit);

            UusiKomento:
            string komento = Console.ReadLine();
            if (komento.Equals("p", StringComparison.OrdinalIgnoreCase))
            {
                KaynnistaPeli(Korttipakka.PakkojenMaara);
            }
            else if (komento.Equals("l", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Kiitos pelaamisesta! Tervetuloa uudelleen pelaamaan!\nMoi Moi!");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                goto UusiKomento;
            }
        }

        public static int Montapakkaa()
        {
            Console.WriteLine("Kuinka monella pakalla haluat pelata? ");
            int pakkojenMaara = int.Parse(Console.ReadLine());
            return pakkojenMaara;
        }

        public static void Tuplaus(Käsi pelaaja, Käsi jakaja, Korttipakka pakka)
        {
            Console.Write("Tuplaus:\nSinun kortti: ");
            System.Threading.Thread.Sleep(2000);
            pelaaja.Kadenkortit.Add(pakka.otaKorttiPakasta());
            System.Threading.Thread.Sleep(2000);
            Käsi.TarkistaJaMuutaAssatYkkosiksi(pelaaja);
            Console.WriteLine("Sinulla on " + pelaaja.KadenArvo);
            if (pelaaja.KadenArvo > 21)
            {
                Console.WriteLine("Sinulla yli! Jakaja voitti.");
                jakajanVoitot++;
                PeliLoppui();
            }
            JakajanVuoro(jakaja, pelaaja, pakka);
            KumpiVoitti(pelaaja, jakaja);
            PeliLoppui();
        }
        //SPLITTAUS!!!
        public static void Splittaa(Käsi pe, Käsi ja, Korttipakka pakka)
        {
            Käsi pe2 = new Käsi();
            pe2.Kadenkortit.Add(new Kortti(pe.Kadenkortit[0]));
            pe.Kadenkortit.RemoveAt(0);

            string toiminto = " ";

            // Ensimmäinen käsi
            while (true)
            {
                Console.WriteLine("Ensimmäinen käsi. Mitä tehdään?\nH = Hit\nS = Stand");
                toiminto = Console.ReadLine();
                Console.WriteLine();

                if (toiminto.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                else if (!toiminto.Equals("h", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                    continue;
                }

                Console.Write("Sinun kortti: ");
                System.Threading.Thread.Sleep(2000);
                pe.Kadenkortit.Add(pakka.otaKorttiPakasta());
                Käsi.TarkistaJaMuutaAssatYkkosiksi(pe);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Sinulla on " + pe.KadenArvo);

                if (pe.KadenArvo > 21)
                {
                    Console.WriteLine("Sinulla yli!");
                    jakajanVoitot++; // MITEN VOITOT LASKETAAN KUN SPLITTI!?!?
                    break;
                }
            }
            //Toinen käsi
            while (true)
            {
                Console.WriteLine("Toinen käsi. Mitä tehdään?\nH = Hit\nS = Stand");
                toiminto = Console.ReadLine();
                Console.WriteLine();

                if (toiminto.Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                else if (!toiminto.Equals("h", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                    continue;
                }

                Console.Write("Sinun kortti: ");
                System.Threading.Thread.Sleep(2000);
                pe2.Kadenkortit.Add(pakka.otaKorttiPakasta());
                Käsi.TarkistaJaMuutaAssatYkkosiksi(pe2);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Sinulla on " + pe.KadenArvo);

                if (pe2.KadenArvo > 21)
                {
                    Console.WriteLine("Sinulla yli!");
                    jakajanVoitot++; // MITEN VOITOT LASKETAAN KUN SPLITTI!?!?
                    break;
                }
            }
            //Tähän vielä jatko koodia
            


        }
    }

    public enum Maa // Yritys hyvä, toteutus ei...
    {
        Hertta = 0,
        Ruutu = 1,
        Risti = 2,
        Pata = 3
    }
}
