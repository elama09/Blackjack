using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_test1
{ // SPLIT!!!! KUVAT!!!! SUPERVOITOT! 

    public class Program
    {

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("*****Tervetuloa pelaamaan BlackJack-pelia!*****\n");
            Console.ForegroundColor = ConsoleColor.White;
            PelinToiminnot.KaynnistaPeli();
        }
    }

    public static class PelinToiminnot // Engine luokka..?
    {
        public static Korttipakka pakka;
        public static Tili tili;
        public static int pelaajanVoitot = 0;
        public static int jakajanVoitot = 0;
        public static int tasapelit = 0;
        public static bool ensimmäinenKierros = true;

        public static Korttipakka TäytyyköLuodaUusiPakka(Korttipakka p)
        {
            if (p == null)
            {
                pakka = new Korttipakka(Montapakkaa());
                return pakka;
            }
            else if (p.PakkaList.Count < (Korttipakka.PakkojenMaara * 52 / 5))
            {
                Console.WriteLine("\nKorttipakka loppumassa.. Sekoitetaan pakka..\n");
                System.Threading.Thread.Sleep(2000);
                //Halutaanko että sekoitetaan samoilla pakkojen määrällä, Vai kysytään uusiksi monta pakkaa laitetaan..?
                //pakka = new Korttipakka(Montapakkaa());
                pakka = new Korttipakka(Korttipakka.PakkojenMaara);
                return pakka;
            }
            return p;
        }
        public static void KaynnistaPeli() // Tai muuta tälläistä
        {
            //Jos ensimmäinen kierros kysytään haluatko ladata tallennetun pelisi
            if (ensimmäinenKierros)
            {
                int? ladatutRahat = Utils_IO.HaluatkoLadataPelitilanteenRahat();
                if (ladatutRahat.HasValue)
                {
                    tili = new Tili(ladatutRahat);
                }
                ensimmäinenKierros = false;
            }
            //Luodaan pelitili
            tili = tili == null ? new Tili() : tili;
            //Luodaan uusi pakka / tai jos liian vähän kortteja
            pakka = TäytyyköLuodaUusiPakka(pakka);
            tili.LaitaPanos();
            //Luodaan pelaajan- ja jakajankäsi
            Käsi pe = new Käsi();
            Käsi ja = new Käsi();

            //Otetaan kaksi korttia molemmille pakasta
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Sinun kortti: ");
            System.Threading.Thread.Sleep(2000);
            pe.Kadenkortit.Add(pakka.otaKorttiPakasta());

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Jakajan kortti: ");
            System.Threading.Thread.Sleep(2000);
            ja.Kadenkortit.Add(pakka.otaKorttiPakasta());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Sinun kortti: ");
            System.Threading.Thread.Sleep(2000);
            pe.Kadenkortit.Add(pakka.otaKorttiPakasta());
            Console.WriteLine();
            System.Threading.Thread.Sleep(2000);
            Console.ForegroundColor = ConsoleColor.White;


            pe.OnkoBlackJack();
            if (pe.BlackJack)
            {
                Console.WriteLine("Sinulla on BlackJack!");
                JakajanVuoro(ja, pe, pakka, tili);
                KumpiVoitti(pe, ja, tili);
                PeliLoppui(pe, tili);
            }

            //Tarkistetaan tarviiko muuttaa Ässiä Ykkösiksi
            Käsi.TarkistaJaMuutaAssatYkkosiksi(pe);
            Käsi.TarkistaJaMuutaAssatYkkosiksi(ja);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sinulla on " + pe.KadenArvo);
            System.Threading.Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Jakajalla on " + ja.KadenArvo);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            HaluatkoTuplata(pe, ja, pakka, tili); //Pelaajan vuoro/Tuplaus

            //Pelaajan vuoro
            PelaajanVuoro(pe, pakka, tili);

            //Jakajan vuoro
            JakajanVuoro(ja, pe, pakka, tili);

            //Kumpi voitti tarkistus
            KumpiVoitti(pe, ja, tili);
            
            PeliLoppui(pe, tili);
        }
        public static void HaluatkoTuplata(Käsi pe, Käsi ja, Korttipakka pakka, Tili tili)
        {
            //Liian vähän rahaa tuplaukseen
            if (tili.Rahat < tili.NykyinenPanostus)
            {
                Console.WriteLine("Sinulla ei tarpeeksi rahaa tuplaukseen..");
            }
            else
            {
                Console.WriteLine("Haluatko tuplata?\nY = Yes\nN = No");

                vääräkomento:
                string tuplausKomento = Console.ReadLine();
                if (tuplausKomento.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine();
                    tili.PanoksenTuplaus();
                    Tuplaus(pe, ja, pakka, tili);
                }
                else if (!tuplausKomento.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                    goto vääräkomento;
                }

            }
            Console.WriteLine();
        }
        public static void PelaajanVuoro(Käsi kasi, Korttipakka pakka, Tili tili)
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
                else if (!toiminto.Equals("h", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Sinun kortti: ");
                System.Threading.Thread.Sleep(2000);
                kasi.Kadenkortit.Add(pakka.otaKorttiPakasta());
                Käsi.TarkistaJaMuutaAssatYkkosiksi(kasi);
                System.Threading.Thread.Sleep(2000);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Sinulla on " + kasi.KadenArvo);

                if (kasi.KadenArvo > 21)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Sinulla yli! Jakaja voitti.");
                    Console.ForegroundColor = ConsoleColor.White;

                    jakajanVoitot++;
                    tili.Häviö();
                    PeliLoppui(kasi, tili);
                }
                else if (kasi.KadenArvo == 21)
                {
                    break;
                }
            }
        }

        public static void JakajanVuoro(Käsi kasi, Käsi pelaajankasi, Korttipakka pakka, Tili tili)
        {
            if (pelaajankasi.BlackJack && (kasi.KadenArvo == 10 || kasi.KadenArvo == 11))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Jakaja ottaa kortin yhden kortin..");
                System.Threading.Thread.Sleep(2000);
                kasi.Kadenkortit.Add(pakka.otaKorttiPakasta());
                Console.ForegroundColor = ConsoleColor.White;
                kasi.OnkoBlackJack();
                KumpiVoitti(pelaajankasi, kasi, tili);
                PeliLoppui(pelaajankasi, tili);
            }
            else if (pelaajankasi.BlackJack && kasi.KadenArvo < 10)
            {
                System.Threading.Thread.Sleep(2000);
                KumpiVoitti(pelaajankasi, kasi, tili);
                PeliLoppui(pelaajankasi, tili);
            }

            if (kasi.KadenArvo >= 17)
            {
                Console.WriteLine("Jakaja jää");
            }
            else
            {
                while (kasi.KadenArvo < 17)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Jakaja ottaa kortin..");
                    System.Threading.Thread.Sleep(2000);
                    kasi.Kadenkortit.Add(pakka.otaKorttiPakasta());
                    System.Threading.Thread.Sleep(2000);

                    kasi.OnkoBlackJack();
                    if (kasi.BlackJack == true && pelaajankasi.BlackJack == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Jakajalla myös BlackJack! Tasapeli.");
                        Console.ForegroundColor = ConsoleColor.White;

                        tasapelit++;
                        tili.Tasapeli();
                        PeliLoppui(pelaajankasi, tili);
                    }
                    else if (kasi.BlackJack == true && pelaajankasi.BlackJack == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Jakajalla on BlackJack! Sinä hävisit.");
                        Console.ForegroundColor = ConsoleColor.White;

                        jakajanVoitot++;
                        tili.Häviö();
                        PeliLoppui(pelaajankasi, tili);
                    }
                    else if (kasi.BlackJack == false && pelaajankasi.BlackJack == true)
                    {
                        pelaajanVoitot++;
                        tili.MaksaVoittoBlackjack();
                        PeliLoppui(pelaajankasi, tili);
                    }

                    Käsi.TarkistaJaMuutaAssatYkkosiksi(kasi);
                    Console.WriteLine("Jakajalla on " + kasi.KadenArvo);
                    Console.ForegroundColor = ConsoleColor.White;

                    if (kasi.KadenArvo > 21)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine();
                        Console.WriteLine("Jakaja yli! Sina Voitit!");
                        Console.ForegroundColor = ConsoleColor.White;

                        pelaajanVoitot++;
                        tili.MaksaVoitto();
                        PeliLoppui(pelaajankasi, tili);
                    }
                }
            }
        }

        public static void KumpiVoitti(Käsi pe, Käsi ja, Tili tili)
        {
            Console.WriteLine();
            System.Threading.Thread.Sleep(2000);
            Console.ForegroundColor = ConsoleColor.Yellow;

            if (pe.BlackJack && ja.BlackJack)
            {
                Console.WriteLine("Molemmilla BlackJack! Tasapeli!");
                tasapelit++;
                tili.Tasapeli();
            }
            else if (pe.KadenArvo > ja.KadenArvo)
            {
                Console.WriteLine("Sinä voitit! Onneksi olkoon!");
                pelaajanVoitot++;
                tili.MaksaVoitto();
            }
            else if (pe.KadenArvo < ja.KadenArvo)
            {
                Console.WriteLine("Sinä hävisit. Jakaja voitti.");
                jakajanVoitot++;
                tili.Häviö();
            }
            else if (pe.KadenArvo == ja.KadenArvo)
            {
                Console.WriteLine("Tasapeli!");
                tasapelit++;
                tili.Tasapeli();
            }
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void PeliLoppui(Käsi pe, Tili tili)
        {
            //Supervoitto
            if (pe.OnkoSuperVoitto())
            {
                tili.SuperVoitto();
            }
            Console.WriteLine();
            Console.WriteLine($"Peli loppui, mitä haluat tehdä seuraavaksi?\nP = Pelaa uudelleen\tL = Lopeta peli\t\tT = Tallenna rahat & Lopeta\tD=Tuhoa edellinen pelitallennus");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Pelimerkkisi: {tili.Rahat}€");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Pelaajan voitot: " + pelaajanVoitot);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\tJakajan voitot: " + jakajanVoitot);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\tTasapelit: " + tasapelit);
            Console.ForegroundColor = ConsoleColor.White;

            //Console.WriteLine("Pelaajan voitot: " + pelaajanVoitot + "\tJakajan voitot: " + jakajanVoitot + "\tTasapelit: " + tasapelit);

            UusiKomento:
            string komento = Console.ReadLine();
            if (komento.Equals("p", StringComparison.OrdinalIgnoreCase))
            {
                KaynnistaPeli();
            }
            else if (komento.Equals("l", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Kiitos pelaamisesta! Tervetuloa uudelleen pelaamaan!\nMoi Moi!");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(0);
            }
            else if (komento.Equals("t", StringComparison.OrdinalIgnoreCase))
            {
                Utils_IO.VieRahatTiedostoon(tili);
                Console.WriteLine("Kiitos pelaamisesta! Tervetuloa uudelleen pelaamaan!\nMoi Moi!");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(0);
            }
            else if (komento.Equals("d", StringComparison.OrdinalIgnoreCase))
            {
                Utils_IO.TuhoaTiedosto();
                Console.WriteLine("Tallennus tuhottu, mitä seuraavaksi?");
                goto UusiKomento;
            }
            else
            {
                Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                goto UusiKomento;
            }
        }

        public static int Montapakkaa()
        {
            Uusiksi:
            Console.WriteLine("Kuinka monella pakalla haluat pelata? (1-10)");
            bool successfullyParsed = int.TryParse(Console.ReadLine(), out int pakkojenMaara);
            if (successfullyParsed == true && pakkojenMaara < 11 && pakkojenMaara > 0)
            {
                return pakkojenMaara;
            }
            else
            {
                Console.WriteLine("Ei onnistu. Syötä haluamasi pakkojen määrä 1-10\n");
                goto Uusiksi;
            }
        }

        public static void Tuplaus(Käsi pelaaja, Käsi jakaja, Korttipakka pakka, Tili tili)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Tuplaus:\nSinun kortti: ");
            System.Threading.Thread.Sleep(2000);
            pelaaja.Kadenkortit.Add(pakka.otaKorttiPakasta());
            System.Threading.Thread.Sleep(2000);

            Käsi.TarkistaJaMuutaAssatYkkosiksi(pelaaja);
            Console.WriteLine("Sinulla on " + pelaaja.KadenArvo + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            if (pelaaja.KadenArvo > 21)
            {
                Console.WriteLine("Sinulla yli! Jakaja voitti.");
                jakajanVoitot++;
                PeliLoppui(pelaaja, tili);
            }
            JakajanVuoro(jakaja, pelaaja, pakka, tili);
            KumpiVoitti(pelaaja, jakaja, tili);
            PeliLoppui(pelaaja, tili);
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
