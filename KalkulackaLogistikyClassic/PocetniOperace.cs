namespace KalkulackaLogistikyClassic
{
    internal class PocetniOperace
    {
        // Třída pro výpočet optimální dávky
        public class DavkaOperace
        {
            private double Qopq;
            private double pocetDavek;
            private double periodicitaZadavani;
            private double prislusneNaklady;
            private double nakladyCelkem;

            // Metoda pro vypočet optimální dávka
            public void DavkaVypocet(int velikostPoptavky, float Npz, float Ns, float Nj, float Obdobi)
            {
                Qopq = Math.Ceiling(Math.Sqrt(2 * velikostPoptavky * Npz) / Math.Sqrt(Nj * Ns * Obdobi));
                pocetDavek = Math.Ceiling(velikostPoptavky / Qopq);
                periodicitaZadavani = Math.Ceiling(360 * Obdobi / pocetDavek);
                prislusneNaklady = Qopq / 2 * Nj * Ns * Obdobi;
                nakladyCelkem = velikostPoptavky / Qopq * Npz + prislusneNaklady;
            }
            // Metoda pro výpis výsledku optimální dávky
            public override string ToString()
            {
                return $"Optimální dávka: {Qopq} ks\r\nPočet dávek: {pocetDavek}" +
                    $"\r\nPeriodicita zadávání: {(int)periodicitaZadavani} dnů (360 prac. dnů)\r\nCelkové náklady: {Math.Round(nakladyCelkem, 2)} Kč";
            }
        }


        public class ObjUrovneOperace
        {
            private float SpotrebaINP { get; init; }
            private float ObjednavaciDavkaINP { get; init; }
            private float TydnyRokINP { get; init; }
            private float ZpINP { get; init; }
            private float PokrytiINP { get; init; }
            private float LhutaINP { get; init; }
            private decimal IntervalKontroly { get; init; }
            private float OcekavanaSpotreba { get; init; }
            private float PrumernaZasoba { get; init; }
            private float Davka { get; init; }
            private float Xzp { get; set; }
            private float BQsystem;
            private float sQsystem;
            private bool BQsystemChecked;
            private bool SQsystemChecked;


            public ObjUrovneOperace(float spotrebaINP, float objednavaciDavkaINP, float tydnyRokINP, float zpINP, float pokrytiINP,
                float lhutaINP, decimal intervalKontroly)
            {
                SpotrebaINP = spotrebaINP;
                ObjednavaciDavkaINP = objednavaciDavkaINP;
                TydnyRokINP = tydnyRokINP;
                ZpINP = zpINP;
                PokrytiINP = pokrytiINP;
                LhutaINP = lhutaINP;

                IntervalKontroly = intervalKontroly;
                OcekavanaSpotreba = (int)(SpotrebaINP / TydnyRokINP);
                PrumernaZasoba = TydnyRokINP * 7 / OcekavanaSpotreba;
                Davka = (float)Math.Ceiling(SpotrebaINP / ObjednavaciDavkaINP);
            }
            //  Metoda pro zvolení, jaká početní operace se má provest podle určitých kritérii zvolené ve formuláři
            public void ObjUrovenVolba(bool XzpCheckBoxChecked, bool BQsystemRB, bool sQsystemRB)
            {
                Xzp = OcekavanaSpotreba * PokrytiINP;
                if (BQsystemRB)
                {
                    BQSystemVypocet(XzpCheckBoxChecked);
                }
                else if (sQsystemRB)
                {
                    SQsystemVypocet(XzpCheckBoxChecked);
                }
            }
            //  Metoda pro výpočet objednávací úrovně podle B,Q systému
            private void BQSystemVypocet(bool XzpChecked)
            {
                if (XzpChecked)
                {
                    BQsystem = (float)Math.Ceiling(Xzp + LhutaINP * OcekavanaSpotreba);
                }
                else
                {
                    Xzp = 0;
                    BQsystem = (float)Math.Ceiling(ZpINP + LhutaINP * OcekavanaSpotreba);
                }
                BQsystemChecked = true;
            }
            //  Metoda pro výpočet objednávací úrovně podle s,Q systému
            private void SQsystemVypocet(bool XzpChecked)
            {
                if (XzpChecked)
                {
                    sQsystem = (float)Math.Ceiling(Xzp + OcekavanaSpotreba * (LhutaINP + 0.7 * (int)IntervalKontroly));
                }
                else
                {
                    sQsystem = (float)Math.Ceiling(ZpINP + OcekavanaSpotreba * (LhutaINP + 0.7 * (int)IntervalKontroly));
                }
                SQsystemChecked = true;
            }
            // Metoda pro výpis výsledku podle toho, jaký systém byl zvolen
            public override string ToString()
            {
                if (BQsystemChecked)
                {
                    return $"Budeme objednávat při stavu: {BQsystem} ks\r\nZásoba nám vystačí na: {PrumernaZasoba}" +
                $" tydnů\r\nBudem objednávat v dávkách: {Davka}";
                }
                else if (SQsystemChecked)
                {
                    return $"Budeme objednávat při stavu: {sQsystem} ks\r\nZásoba nám vystačí na: {PrumernaZasoba}" +
                $" tydnů\r\nBudem objednávat v dávkách: {Davka}";
                }
                return "";
            }
        }


        public class PrubeznaDobaOperace
        {
            private int Tpz1 { get; init; }
            private int Tksum { get; init; }
            private int Tkmax { get; init; }
            private int Tmsum { get; init; }
            private int Davka { get; init; }
            private int Varianta { get; init; }
            private int DavkaQd { get; init; }
            private int PocetPracovist { get; init; }
            private int PocetSerizovani { get; init; }
            private int PocetPracovniku;
            private int SoubeznaJednotlive;
            private int SoubeznaDavkach;

            //  Reprezentace třídy pro načtení všech potřebných dat pro další použití
            public PrubeznaDobaOperace(int tpz1, int tksum, int tkmax, int tmsum, int davka, int varianta, int davkaQd, int pocetPracovist, int pocetSerizovani)
            {
                Tpz1 = tpz1;
                Tksum = tksum;
                Tkmax = tkmax;
                Tmsum = tmsum;
                Davka = davka;
                Varianta = varianta;
                DavkaQd = davkaQd;
                PocetPracovist = pocetPracovist;
                PocetSerizovani = pocetSerizovani;
                ZvolenaVarianta();
            }
            //  Veřejná metoda, která zjistí, jaká varianta předávání byla zvolena, zároveň přejde na metodu soukromou kde proběhne výpočet podle zvolené varianty
            private void ZvolenaVarianta()
            {
                switch (Varianta)
                {
                    case 0:         //  0 = prvně zvolená variata a to "souběžné jednotlivé předávání, překryté seřizování"
                        SoubezneJednotlive();
                        PocetPracovniku = PocetPracovist + PocetSerizovani;
                        break;
                    case 1:         // 1 = druhá zvolená varianta a to "předávání v dopravních dávkách po X kusech, překryté seřizování"
                        SoubeznevDavkach();
                        PocetPracovniku = PocetPracovist + PocetSerizovani - DavkaQd;
                        break;
                }
            }
            //  Metoda pro variantu "souběžné jednotlivé předávání, překryté seřizování"
            private void SoubezneJednotlive()
            {
                SoubeznaJednotlive = Tpz1 + Tksum + (Davka - 1) * Tkmax + Tmsum;
            }
            //  Metoda pro variantu "předávání v dopravních dávkách po X kusech, překryté seřizování"
            private void SoubeznevDavkach()
            {
                SoubeznaDavkach = Tpz1 + DavkaQd * Tksum + (Davka - DavkaQd) * Tkmax + Tmsum;
            }
            //  Metoda pro výpis výsledku podle toho, jaká varianta byla zvolená
            public override string ToString()
            {
                return Varianta switch
                {
                    0 => $"T = {SoubeznaJednotlive} min\r\nPočet pracovníků: {PocetPracovniku}",
                    1 => $"T = {SoubeznaDavkach} min\r\nPočet pracovníků: {PocetPracovniku}",
                    _ => "",
                };
            }
        }
    }
}