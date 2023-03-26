using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Threading;

using System.Data;
using System.Xml.Serialization;

using System.Text.RegularExpressions;

using System.Diagnostics;
using System.ComponentModel;

using System.ComponentModel.Design.Serialization;
using System.Collections;



namespace PWR_PL_Implementator
{
    class PWR_PL_Implementator
    {
        readonly static string exe_sciezka = System.Reflection.Assembly.GetExecutingAssembly().Location;
        readonly static string _PWR_PL_naglowek = "Implementator polonizacji PWR_PL by Revok (2023), kompilacja 202303262241";
        readonly static string wersja_polonizacji = PobierzNumerWersjiPolonizacji();
        
        readonly static bool rejestruj_log = true; // jeśli zmienna jest ustawiona jako "true", wtedy rejestrowane są zdarzenia implementatora do pliku: %APPDATA$\PWR_PL\PWR_PL.log
        static FileStream plikLOG_fs;
        static StreamWriter plikLOG_sw;

        static List<string> listasciezek_wykrytekonflikty = new List<string>();

        private static string PobierzAktualnaDateICzas()
        {
            return DateTime.Now.ToString("[yyyy.MM.dd HH:mm:ss]");
        }

        private static void InicjalizujRejestratorLOG()
        {
            if (rejestruj_log == true)
            {
                if (Directory.Exists(APPDATA("PWR_PL\\")) == false)
                {
                    Directory.CreateDirectory(APPDATA("PWR_PL\\"));
                }

                plikLOG_fs = new FileStream(APPDATA("PWR_PL\\PWR_PL.log"), FileMode.Append, FileAccess.Write);

                try
                {
                    plikLOG_sw = new StreamWriter(plikLOG_fs);
                }
                catch
                {
                    Blad("BŁĄD (#LOG): Wystąpił nieoczekiwany problem z zapisywaniem zdarzeń implementatora. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");
                }

            }
        }

        private static void ZapiszLOG(string tresc)
        {
            if (rejestruj_log == true)
            {
                plikLOG_sw.WriteLine(PobierzAktualnaDateICzas() + " " + tresc);
            }
        }

        private static void ZamknijRejestratorLOG()
        {
            if (rejestruj_log == true)
            {
                plikLOG_sw.Close();
                plikLOG_fs.Close();
            }
        }

        private static void Koniec()
        {
            ZapiszLOG("Zakończono działanie implementatora.");
            ZamknijRejestratorLOG();

            Console.WriteLine("Kliknij dowolny klawisz, aby zamknąć to okno.");
            Console.ReadKey();
        }
        private static void Blad(string tresc)
        {
            ZapiszLOG("Komunikat BŁĘDU: \"" + tresc + "\"");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(tresc);
            Console.ResetColor();

        }
        private static void Sukces(string tresc)
        {
            ZapiszLOG("Komunikat SUKCESU: \"" + tresc + "\"");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(tresc);
            Console.ResetColor();

        }
        private static void Sukces2(string tresc)
        {
            ZapiszLOG("Komunikat SUKCESU: \"" + tresc + "\"");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(tresc);
            Console.ResetColor();
        }
        private static void Informacja(string tresc)
        {
            ZapiszLOG("Komunikat INFORMACYJNY: \"" + tresc + "\"");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(tresc);
            Console.ResetColor();
        }

        private static string PobierzNumerWersjiPolonizacji()
        {
            string numerwersjipolonizacji = "NULL";

            string sciezka_do_pliku_IntroductoryText = "Implementacja\\Wrath_Data\\StreamingAssets\\IntroductoryText.json";

            string plikIntroductoryText_tresc = "";

            if (File.Exists(sciezka_do_pliku_IntroductoryText) == true)
            {
                FileStream plikIntroductoryText_fs = new FileStream(sciezka_do_pliku_IntroductoryText, FileMode.Open, FileAccess.Read);

                try
                {
                    StreamReader plikIntroductoryText_sr = new StreamReader(plikIntroductoryText_fs);

                    plikIntroductoryText_tresc = plikIntroductoryText_sr.ReadToEnd();

                    plikIntroductoryText_sr.Close();

                }
                catch
                {
                    Blad("BŁĄD (#IntroductoryText): Wystąpił nieoczekiwany problem z odczytem przynajmniej jednego pliku gry. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");

                }

                plikIntroductoryText_fs.Close();

                if (plikIntroductoryText_tresc.Contains("Zainstalowana wersja: ") == true && plikIntroductoryText_tresc.Contains("</size>"))
                {
                    string[] filtr1 = plikIntroductoryText_tresc.Split("Zainstalowana wersja: ", StringSplitOptions.None);

                    if (filtr1.Length > 1)
                    {
                        string[] filtr2 = filtr1[1].Split("</size>", StringSplitOptions.None);

                        if (filtr2.Length > 1)
                        {
                            numerwersjipolonizacji = filtr2[0];
                        }
                    }
                    else
                    {
                        numerwersjipolonizacji = "NULL";
                    }
                }
                else
                {
                    numerwersjipolonizacji = "NULL";
                }

            }
            else
            {
                numerwersjipolonizacji = "NULL";
            }

            //Console.WriteLine("[DEBUG] numerwersjipolonizacji==" + numerwersjipolonizacji);

            return numerwersjipolonizacji;

        }

        private static string PobierzDaneZVersionInfo(string sciezka_do_pliku_VersionInfo)
        {
            string plikVersionInfo_tresc = "";

            if (File.Exists(sciezka_do_pliku_VersionInfo))
            {
                FileStream plikVersionInfo_fs = new FileStream(sciezka_do_pliku_VersionInfo, FileMode.Open, FileAccess.Read);

                try
                {
                    StreamReader plikVersionInfo_sr = new StreamReader(plikVersionInfo_fs);

                    plikVersionInfo_tresc = plikVersionInfo_sr.ReadToEnd();

                    plikVersionInfo_sr.Close();

                }
                catch
                {
                    Blad("BŁĄD (#Version): Wystąpił nieoczekiwany problem z odczytem przynajmniej jednego pliku gry. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");
                }

                plikVersionInfo_fs.Close();

                return plikVersionInfo_tresc;
            }
            else
            {
                return "NULL NULL NULL NULL NULL";
            }
        }

        private static void SkopiujFolderWrazZZawartoscia(string sciezka_folderu_do_skopiowania, string sciezka_docelowa)
        {
            DirectoryInfo folderzrodlowy_di = new DirectoryInfo(sciezka_folderu_do_skopiowania);
            DirectoryInfo folderdocelowy_di = new DirectoryInfo(sciezka_docelowa);

            if (folderzrodlowy_di.FullName.ToLower() == folderdocelowy_di.FullName.ToLower())
            {
                return;
            }

            if (Directory.Exists(folderdocelowy_di.FullName) == false)
            {
                Directory.CreateDirectory(folderdocelowy_di.FullName);
            }

            foreach (FileInfo fi in folderzrodlowy_di.GetFiles())
            {
                //Console.WriteLine(@"[DEBUG] Kopiowanie {0}\{1}", folderdocelowy_di.FullName, fi.Name);

                fi.CopyTo(Path.Combine(folderdocelowy_di.ToString(), fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in folderzrodlowy_di.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    folderdocelowy_di.CreateSubdirectory(diSourceSubDir.Name);
                //SkopiujFolderWrazZZawartoscia(diSourceSubDir, nextTargetSubDir);
            }
        }

        private static string APPDATA(string sciezka_wewnatrz_APPDATA)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), sciezka_wewnatrz_APPDATA);
        }

        private static void ZmienJezykWPlikuKonfiguracyjnymGry(string oznaczeniejezyka_przedzmiana, string oznaczeniejezyka_pozmianie) // "deDE": niemiecki/polski, "enGB": angielski
        {

            if (File.Exists(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json")) == true)
            {
                File.Move(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json"), APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json.TMP"));

                FileStream plikkonfiguracjigry_przedzmiana_fs = new FileStream(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json.TMP"), FileMode.Open, FileAccess.Read);

                string plikkonfiguracjigry_przedzmiana_tresc = "";

                try
                {
                    StreamReader plikkonfiguracjigry_przedzmiana_sr = new StreamReader(plikkonfiguracjigry_przedzmiana_fs);

                    plikkonfiguracjigry_przedzmiana_tresc = plikkonfiguracjigry_przedzmiana_sr.ReadToEnd();

                    plikkonfiguracjigry_przedzmiana_sr.Close();

                }
                catch
                {
                    Blad("BŁĄD (#GeneralSettingsTMP(Read)): Wystąpił nieoczekiwany problem z odczytem przynajmniej jednego pliku gry. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");
                }

                plikkonfiguracjigry_przedzmiana_fs.Close();



                FileStream plikkonfiguracjigry_pozmianie_fs = new FileStream(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json"), FileMode.CreateNew, FileAccess.Write);

                try
                {
                    StreamWriter plikkonfiguracjigry_pozmianie_sw = new StreamWriter(plikkonfiguracjigry_pozmianie_fs);

                    plikkonfiguracjigry_pozmianie_sw.Write(plikkonfiguracjigry_przedzmiana_tresc.Replace("\"settings.game.main.locale\": \"" + oznaczeniejezyka_przedzmiana + "\"", "\"settings.game.main.locale\": \"" + oznaczeniejezyka_pozmianie + "\""));

                    plikkonfiguracjigry_pozmianie_sw.Close();

                }
                catch
                {
                    Blad("BŁĄD (#GeneralSettings(Write)): Wystąpił nieoczekiwany problem z odczytem przynajmniej jednego pliku gry. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");
                }

                plikkonfiguracjigry_pozmianie_fs.Close();

                if (File.Exists(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json.TMP")) == true) { File.Delete(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json.TMP")); }

                ZapiszLOG("Zmodyfikowano oznaczenie języka w pliku konfiguracyjnym gry z \"" + oznaczeniejezyka_przedzmiana + "\" na \"" + oznaczeniejezyka_pozmianie + "\".");
            }

        }

        private static IEnumerable<string> WyszukajPlikiKopiiZapasowych(string sciezka_do_folderu, bool zapisz_na_liscie_jako_element_potencjalnie_stwarzajacy_konflikt = true)
        {
            IEnumerable<string> rezultat = null;

            if (Directory.Exists(sciezka_do_folderu))
            {
                Regex kopiezapasowe_regex = new Regex(@"ORIG.BAK");

                rezultat = Directory.GetFiles(sciezka_do_folderu, "*.*").Where(sciezka => kopiezapasowe_regex.IsMatch(sciezka));


                foreach (string plik in rezultat)
                {
                    if (zapisz_na_liscie_jako_element_potencjalnie_stwarzajacy_konflikt == true)
                    {
                        listasciezek_wykrytekonflikty.Add(plik);
                    }

                    //Console.WriteLine("[DEBUG] plik==" + plik);
                }

                if (listasciezek_wykrytekonflikty.Count > 0)
                {
                    string listasciezek_string = "";

                    for (int liwr = 0; liwr < listasciezek_wykrytekonflikty.Count; liwr++)
                    {
                        listasciezek_string = listasciezek_string + "\n" + liwr+1.ToString() + ") " + listasciezek_wykrytekonflikty[liwr];
                    }

                    ZapiszLOG("Znaleziono następujące elementy kopii zapasowych gry:" + listasciezek_string);
                }

            }

            return rezultat;
        }
        private static IEnumerable<string> WyszukajFolderyKopiiZapasowych(string sciezka_do_folderu, bool zapisz_na_liscie_jako_element_potencjalnie_stwarzajacy_konflikt = true)
        {
            IEnumerable<string> rezultat = null;

            if (Directory.Exists(sciezka_do_folderu))
            {
                Regex kopiezapasowe_regex = new Regex(@"ORIG.BAK");

                rezultat = Directory.GetDirectories(sciezka_do_folderu, "*.*").Where(sciezka => kopiezapasowe_regex.IsMatch(sciezka));


                foreach (string folder in rezultat)
                {
                    if (zapisz_na_liscie_jako_element_potencjalnie_stwarzajacy_konflikt == true)
                    {
                        listasciezek_wykrytekonflikty.Add(folder);
                    }

                    //Console.WriteLine("[DEBUG] folder==" + folder);
                }

                if (listasciezek_wykrytekonflikty.Count > 0)
                {
                    string listasciezek_string = "";

                    for (int liwr = 0; liwr < listasciezek_wykrytekonflikty.Count; liwr++)
                    {
                        listasciezek_string = listasciezek_string + "\n" + liwr + 1.ToString() + ") " + listasciezek_wykrytekonflikty[liwr];
                    }

                    ZapiszLOG("Znaleziono następujące elementy kopii zapasowych gry:" + listasciezek_string);
                }

            }

            return rezultat;
        }

        private static void Main(string[] args)
        {
            try
            {

                Console.Title = _PWR_PL_naglowek;

                Process[] proces_implementatora = Process.GetProcessesByName("Zaimplementuj_PWR_PL");

                InicjalizujRejestratorLOG();


                if (proces_implementatora.Length <= 1)
                {
                    string separator_naglowka = "";
                    for (int s1 = 0; s1 < _PWR_PL_naglowek.Length + 4; s1++)
                    {
                        separator_naglowka += "-";
                    }

                    Console.WriteLine(separator_naglowka);
                    Console.WriteLine("| " + _PWR_PL_naglowek + " |");
                    Console.WriteLine(separator_naglowka);

                    Console.WriteLine("Wersja polonizacji PWR_PL: " + wersja_polonizacji);

                    if (args.Length > 0)
                    {
                        if (args[0] == "-test")
                        {
                            Test();
                        }
                        else if (args[0] == "-deimplementuj")
                        {
                            Deimplementuj_PWR_PL();
                        }
                    }
                    else
                    {
                        Zaimplementuj_PWR_PL();
                    }

                }

            }
            catch (Exception ex)
            {
                ZapiszLOG("Implementator napotkał krytyczny wyjątek i został niespodziewanie zamknięty: " + ex.Message);

                Blad("NIEOCZEKIWANY BŁĄD: Implementator spolszczenia napotkał krytyczny wyjątek i musi zostać zamknięty.");

                Koniec();
                
            }
        }

        private static void Test()
        {
            /*
            DirectoryInfo test_di = new DirectoryInfo("..\\Wrath_Data\\StreamingAssets\\Version.info");

            Console.WriteLine("[DEBUG] test_di == " + test_di);

            Console.WriteLine("[DEBUG] Numer wersji polonizacji: " + PobierzNumerWersjiPolonizacji());
            */

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test.");
            Console.ResetColor();

            Koniec();
        }

        private static void Zaimplementuj_PWR_PL()
        {
            ZapiszLOG("Zainicjalizowano implementację polonizacji: " + wersja_polonizacji);

            if
            (
            File.Exists("..\\Wrath_Data\\StreamingAssets\\Version.info")
            &&
            File.Exists("..\\Wrath.exe")
            &&
            File.Exists("..\\Wrath_Data\\sharedassets0.assets")
            &&
            File.Exists("..\\Bundles\\ui")
            &&
            Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization\\")
            &&
            File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")
            &&
            Directory.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\Localization\\")
            &&
            File.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")
            &&
            File.Exists("Konfiguracja\\deDE-default-general_settings.json")
            &&
            File.Exists("Kompatybilnosc\\Version.info")
            )
            {
                string kompatybilnoscspolszczenia_dane = PobierzDaneZVersionInfo("Kompatybilnosc\\Version.info");
                string aktualniezainstalowanawersjagry_dane = PobierzDaneZVersionInfo("..\\Wrath_Data\\StreamingAssets\\Version.info");

                string kompatybilny_numerwersjigry = kompatybilnoscspolszczenia_dane.Split(new char[] { ' ' })[3];
                string numerzainstalowanejwersjigry = aktualniezainstalowanawersjagry_dane.Split(new char[] { ' ' })[3];

                if (kompatybilnoscspolszczenia_dane == aktualniezainstalowanawersjagry_dane)
                {
                    ZapiszLOG("Kompatybilność wersji polonizacji z wersją gry została potwierdzona (" + wersja_polonizacji + " --->" + numerzainstalowanejwersjigry + ").");

                    var kopiezapasowe_sharedassets0assets = WyszukajPlikiKopiiZapasowych("..\\Wrath_Data\\");
                    var kopiezapasowe_Bundlesui = WyszukajPlikiKopiiZapasowych("..\\Bundles\\");
                    var kopiezapasowe_IntroductoryText = WyszukajPlikiKopiiZapasowych("..\\Wrath_Data\\StreamingAssets\\");

                    var kopiezapasowe_Localization = WyszukajFolderyKopiiZapasowych("..\\Wrath_Data\\StreamingAssets\\");

                    /*
                    for (int il1 = 0; il1 < listasciezek_wykrytekonflikty.Count; il1++)
                    {
                        Console.WriteLine("[DEBUG] listasciezek_wykrytekonflikty[" + il1 + "]==" + listasciezek_wykrytekonflikty[il1]);
                    }
                    */


                    if (listasciezek_wykrytekonflikty.Count == 0)
                    {
                        Console.WriteLine("Trwa implementacja spolszczenia...");
                        Console.WriteLine("Nie zamykaj tego okna i poczekaj, aż wyświetlą się kolejne informacje. Może to trochę potrwać...");

                        if (File.Exists("Implementacja\\Wrath_data\\sharedassets0.assets") == true)
                        {
                            if (File.Exists("..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry) == false)
                            {
                                File.Move("..\\Wrath_Data\\sharedassets0.assets", "..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry);
                            }
                        }


                        if
                        (
                            Directory.Exists("Implementacja\\bundle-ui\\") == true
                            &&
                            File.Exists("Implementacja\\bundle-ui\\xdelta3.exe") == true
                            &&
                            File.Exists("Implementacja\\bundle-ui\\pwr_pl-ui.patch") == true
                        )
                        {
                            if (File.Exists("..\\Bundles\\ui.PWR_PL") == true)
                            {
                                File.Delete("..\\Bundles\\ui.PWR_PL");
                            }

                            ProcessStartInfo xdelta3_startInfo = new ProcessStartInfo();
                            xdelta3_startInfo.CreateNoWindow = false;
                            xdelta3_startInfo.UseShellExecute = false;
                            xdelta3_startInfo.FileName = "Implementacja\\bundle-ui\\xdelta3.exe";
                            xdelta3_startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            xdelta3_startInfo.Arguments = "-d -s ..\\Bundles\\ui Implementacja\\bundle-ui\\pwr_pl-ui.patch ..\\Bundles\\ui.PWR_PL";



                            try
                            {
                                using (Process xdelta3_proces = Process.Start(xdelta3_startInfo))
                                {
                                    xdelta3_proces.WaitForExit();
                                }
                            }
                            catch
                            {
                                Blad("BŁĄD: Wystąpił nieoczekiwany problem z dostępem do patchera. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");
                            }

                            if (File.Exists("..\\Bundles\\ui.PWR_PL") == true)
                            {
                                if (File.Exists("..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry) == false)
                                {
                                    File.Move("..\\Bundles\\ui", "..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry);
                                }

                                File.Move("..\\Bundles\\ui.PWR_PL", "..\\Bundles\\ui");
                            }

                        }


                        if (Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization.ORIG.BAK-" + kompatybilny_numerwersjigry + "\\") == false)
                        {
                            SkopiujFolderWrazZZawartoscia("..\\Wrath_Data\\StreamingAssets\\Localization\\", "..\\Wrath_Data\\StreamingAssets\\Localization.ORIG.BAK-" + kompatybilny_numerwersjigry + "\\");
                        }

                        if (File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json.ORIG.BAK-" + kompatybilny_numerwersjigry) == false)
                        {
                            File.Move("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json", "..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json.ORIG.BAK-" + kompatybilny_numerwersjigry);
                        }


                        SkopiujFolderWrazZZawartoscia("Implementacja\\Wrath_Data\\StreamingAssets\\Localization\\", "..\\Wrath_Data\\StreamingAssets\\Localization\\");

                        File.Copy("Implementacja\\Wrath_Data\\StreamingAssets\\IntroductoryText.json", "..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json");



                        if (File.Exists(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json")) == true)
                        {
                            ZmienJezykWPlikuKonfiguracyjnymGry("enGB", "deDE");
                        }
                        else
                        {
                            if (Directory.Exists(APPDATA("..\\LocalLow\\Owlcat Games\\")) == false) { Directory.CreateDirectory(APPDATA("..\\LocalLow\\Owlcat Games\\")); }
                            if (Directory.Exists(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\")) == false) { Directory.CreateDirectory(APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\")); }
                            File.Copy("Konfiguracja\\deDE-default-general_settings.json", APPDATA("..\\LocalLow\\Owlcat Games\\Pathfinder Wrath Of The Righteous\\general_settings.json"));
                        }


                        Sukces("Zaimplementowano polonizację " + wersja_polonizacji + " do gry Pathfinder Wrath of the Righteous v." + kompatybilny_numerwersjigry + ".");
                        Informacja("Jeśli po instalacji spolszczenia gra uruchamia się w języku angielskim to aby aktywować język polski, w opcjach gry przełącz na niemiecki wchodząc w Options/Language/\"Deutsch\"/Accept.");


                    }
                    else
                    {

                        if (File.Exists("..\\Wrath_Data\\sharedassets0.assets")) { File.Delete("..\\Wrath_Data\\sharedassets0.assets"); }
                        if (File.Exists("..\\Bundles\\ui")) { File.Delete("..\\Bundles\\ui"); }
                        if (File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")) { File.Delete("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json"); }

                        if (Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization\\")) { Directory.Delete("..\\Wrath_Data\\StreamingAssets\\Localization\\", true); }


                        for (int il2 = 0; il2 < listasciezek_wykrytekonflikty.Count; il2++)
                        {
                            if (File.Exists(listasciezek_wykrytekonflikty[il2])) { File.Delete(listasciezek_wykrytekonflikty[il2]); }
                            if (Directory.Exists(listasciezek_wykrytekonflikty[il2])) { Directory.Delete(listasciezek_wykrytekonflikty[il2], true); }
                        }


                        Blad("Nie można zainstalować spolszczenia, ponieważ wykryto błędy w integralności plików gry. Istnieją pliki i/lub foldery potencjalnie stwarzające konflikty.");
                        Informacja("Powyższy błąd może wynikać z faktu wielokrotnych prób instalacji spolszczenia niewłaściwą metodą nałożenia jednej instalacji na drugą (zawsze przed instalacją innej wersji polonizacji należy najpierw odinstalować poprzednią).");
                        Informacja("Pliki/foldery stwarzające konflikty zostały teraz automatycznie usunięte przez implementator spolszczenia, natomiast koniecznie sprawdź spójność plików gry w Steam/GoG/Epic przed kolejną próbą uruchomienia gry lub ponowną instalacją polonizacji.");
                    }

                }
                else
                {
                    ZapiszLOG("Instalowana wersja polonizacji NIE JEST kompatybilna z wersją zainstalowanej gry (" + wersja_polonizacji + " --->" + numerzainstalowanejwersjigry + ").");

                    Blad("BŁĄD: Nie można zainstalować spolszczenia, ponieważ wystąpiła niezgodność wersji spolszczenia z zainstalowaną wersją gry.");
                    Informacja("Upewnij się, że instalujesz wersję spolszczenia zgodną z aktualnie zainstalowaną wersją gry.");
                    Console.WriteLine("Wersja spolszczenia, którą próbujesz zainstalować jest przeznaczona dla wersji gry: " + kompatybilny_numerwersjigry);
                    Console.WriteLine("Posiadasz zainstalowaną wersję gry: " + numerzainstalowanejwersjigry);

                }

            }
            else
            {
                ZapiszLOG("Folder PWR_PL został umieszczony przed próbą implementacji w niewłaściwym miejscu lub brakuje istotnych plików gry (ścieżka zainicjowanego implementatora: " + exe_sciezka + ").");
                Blad("BŁĄD: Weryfikacja plików gry nie powiodła się. Upewnij się, że folder \"PWR_PL\" wraz całą zawartością znajduje się w głównym folderze z zainstalowaną grą Pathfinder Wrath of the Righteous. Jeśli tak jest, a mimo tego wyświetla się ten błąd, wtedy sprawdź spójność plików gry w Steam/GoG/Epic, a nastepnie spróbuj ponownie zainstalować spolszczenie.");
            }

            Koniec();
        }

        private static void Deimplementuj_PWR_PL()
        {
            ZapiszLOG("Zainicjalizowano deimplementację polonizacji: " + wersja_polonizacji);

            int ilosc_wykrytychbrakujacychelementowORIGBAKdlaTEJWERSJIGRY = 0;

            if
            (
            File.Exists("..\\Wrath_Data\\StreamingAssets\\Version.info")
            &&
            File.Exists("..\\Wrath.exe")
            &&
            File.Exists("..\\Wrath_Data\\sharedassets0.assets")
            &&
            File.Exists("..\\Bundles\\ui")
            &&
            Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization\\")
            &&
            File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")
            &&
            Directory.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\Localization\\")
            &&
            File.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")
            &&
            File.Exists("Konfiguracja\\deDE-default-general_settings.json")
            &&
            File.Exists("Kompatybilnosc\\Version.info")
            )
            {

                string kompatybilnoscspolszczenia_dane = PobierzDaneZVersionInfo("Kompatybilnosc\\Version.info");
                string aktualniezainstalowanawersjagry_dane = PobierzDaneZVersionInfo("..\\Wrath_Data\\StreamingAssets\\Version.info");

                string kompatybilny_numerwersjigry = kompatybilnoscspolszczenia_dane.Split(new char[] { ' ' })[3];
                string numerzainstalowanejwersjigry = aktualniezainstalowanawersjagry_dane.Split(new char[] { ' ' })[3];

                var kopiezapasowe_sharedassets0assets = WyszukajPlikiKopiiZapasowych("..\\Wrath_Data\\");
                var kopiezapasowe_Bundlesui = WyszukajPlikiKopiiZapasowych("..\\Bundles\\");
                var kopiezapasowe_IntroductoryText = WyszukajPlikiKopiiZapasowych("..\\Wrath_Data\\StreamingAssets\\");

                var kopiezapasowe_Localization = WyszukajFolderyKopiiZapasowych("..\\Wrath_Data\\StreamingAssets\\");

                /*
                for (int il1 = 0; il1 < listasciezek_wykrytekonflikty.Count; il1++)
                {
                    Console.WriteLine("[DEBUG] listasciezek_wykrytekonflikty[" + il1 + "]==" + listasciezek_wykrytekonflikty[il1]);
                }
                */


                if (kompatybilnoscspolszczenia_dane == aktualniezainstalowanawersjagry_dane)
                {
                    ZapiszLOG("Kompatybilność wersji polonizacji z wersją gry została potwierdzona (" + wersja_polonizacji + " --->" + numerzainstalowanejwersjigry + ").");
                    ZapiszLOG("Wykryto użycie zgodnego/prawidłowego implementatora w celu deimplementacji spolszczenia z zainstalowanej gry.");

                    Console.WriteLine("Trwa usuwanie Polskiej Lokalizacji PWR_PL: " + PobierzNumerWersjiPolonizacji() + "");
                    Console.WriteLine("Nie zamykaj tego okna i poczekaj, aż wyświetlą się kolejne informacje. Może to trochę potrwać...");

                    if (File.Exists("Implementacja\\Wrath_Data\\sharedassets0.assets") == true)
                    {
                        if (File.Exists("..\\Wrath_Data\\sharedassets0.assets") == true)
                        {
                            File.Delete("..\\Wrath_Data\\sharedassets0.assets");
                        }

                        if (File.Exists("..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry) == true)
                        {
                            File.Move("..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry, "..\\Wrath_Data\\sharedassets0.assets");
                        }
                        else
                        {
                            ZapiszLOG("Wykryto brak kopii zapasowej oryginalnego pliku gry, który deimplementator chciał automatycznie przywrócić z: " + exe_sciezka.Replace("Zaimplementuj_PWR_PL.exe", "") + "..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry + ".");

                            ilosc_wykrytychbrakujacychelementowORIGBAKdlaTEJWERSJIGRY++;
                        }
                    }


                    if (File.Exists("Implementacja\\bundle-ui\\pwr_pl-ui.patch") == true)
                    {
                        if (File.Exists("..\\Bundles\\ui") == true)
                        {
                            File.Delete("..\\Bundles\\ui");
                        }


                        if (File.Exists("..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry) == true)
                        {
                            File.Move("..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry, "..\\Bundles\\ui");
                        }
                        else
                        {
                            ZapiszLOG("Wykryto brak kopii zapasowej oryginalnego pliku gry, który deimplementator chciał automatycznie przywrócić z: " + exe_sciezka.Replace("Zaimplementuj_PWR_PL.exe", "") + "..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry + ".");

                            ilosc_wykrytychbrakujacychelementowORIGBAKdlaTEJWERSJIGRY++;
                        }
                    }


                    //if (File.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\IntroductoryText.json") == true) /* WARUNEK IF W TYM PRZYPADKU NIE OBOWIĄZUJE, ponieważ plik "IntroductoryText.json" zawsze musi znajdować się w builderze instalatora */
                    {
                        if (File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json") == true)
                        {
                            File.Delete("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json");
                        }


                        if (File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json.ORIG.BAK-" + kompatybilny_numerwersjigry) == true)
                        {
                            File.Move("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json.ORIG.BAK-" + kompatybilny_numerwersjigry, "..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json");
                        }
                        else
                        {
                            ZapiszLOG("Wykryto brak kopii zapasowej oryginalnego pliku gry, który deimplementator chciał automatycznie przywrócić z: " + exe_sciezka.Replace("Zaimplementuj_PWR_PL.exe", "") + "..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json.ORIG.BAK-" + kompatybilny_numerwersjigry + ".");

                            ilosc_wykrytychbrakujacychelementowORIGBAKdlaTEJWERSJIGRY++;
                        }
                    }


                    //if (Directory.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\Localization\\") == true) /* WARUNEK IF W TYM PRZYPADKU NIE OBOWIĄZUJE, ponieważ folder "Localization" zawsze musi znajdować się w builderze instalatora */
                    {
                        if (Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization\\") == true)
                        {
                            Directory.Delete("..\\Wrath_Data\\StreamingAssets\\Localization\\", true);
                        }


                        if (Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization.ORIG.BAK-" + kompatybilny_numerwersjigry) == true)
                        {
                            Directory.Move("..\\Wrath_Data\\StreamingAssets\\Localization.ORIG.BAK-" + kompatybilny_numerwersjigry, "..\\Wrath_Data\\StreamingAssets\\Localization");
                        }
                        else
                        {
                            ZapiszLOG("Wykryto brak kopii zapasowej oryginalnego folderu gry z zawartością, który deimplementator chciał automatycznie przywrócić z: " + exe_sciezka.Replace("Zaimplementuj_PWR_PL.exe", "") + "..\\Wrath_Data\\StreamingAssets\\Localization.ORIG.BAK-" + kompatybilny_numerwersjigry + ".");

                            ilosc_wykrytychbrakujacychelementowORIGBAKdlaTEJWERSJIGRY++;
                        }
                    }

                }
                else
                { /*---*/
                    ZapiszLOG("Ta wersja polonizacji NIE JEST kompatybilna z aktualnie zainstalowaną grą (" + wersja_polonizacji + " --->" + numerzainstalowanejwersjigry + ").");
                    ZapiszLOG("Wykryto użycie NIEzgodnego/NIEprawidłowego implementatora w celu deimplementacji spolszczenia z zainstalowanej gry.");


                    if (File.Exists("..\\Wrath_Data\\sharedassets0.assets")) { File.Delete("..\\Wrath_Data\\sharedassets0.assets"); }
                    if (File.Exists("..\\Bundles\\ui")) { File.Delete("..\\Bundles\\ui"); }
                    if (File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")) { File.Delete("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json"); }

                    if (Directory.Exists("..\\Wrath_Data\\StreamingAssets\\Localization\\")) { Directory.Delete("..\\Wrath_Data\\StreamingAssets\\Localization\\", true); }


                    for (int il2 = 0; il2 < listasciezek_wykrytekonflikty.Count; il2++)
                    {
                        if (File.Exists(listasciezek_wykrytekonflikty[il2])) { File.Delete(listasciezek_wykrytekonflikty[il2]); }
                        if (Directory.Exists(listasciezek_wykrytekonflikty[il2])) { Directory.Delete(listasciezek_wykrytekonflikty[il2], true); }
                    }


                    Blad("Wykryto błędy w integralności plików gry. Istnieją pliki i/lub foldery potencjalnie stwarzające konflikty.");
                    Informacja("Powyższy błąd może wynikać z faktu próby użycia deinstalatora nie z tej wersji spolszczenia, która aktualnie była zainstalowana.");
                    Informacja("Pliki/foldery stwarzające konflikty zostały teraz automatycznie usunięte przez deimplementator spolszczenia, natomiast koniecznie sprawdź spójność plików gry w Steam/GoG/Epic przed kolejną próbą uruchomienia gry lub ponowną instalacją polonizacji.");

                }

                ZmienJezykWPlikuKonfiguracyjnymGry("deDE", "enGB");

                if (ilosc_wykrytychbrakujacychelementowORIGBAKdlaTEJWERSJIGRY == 0)
                {
                    Sukces("Polska Lokalizacja PWR " + PobierzNumerWersjiPolonizacji() + " została pomyślnie odinstalowana z gry Pathfinder Wrath of the Righteous " + kompatybilny_numerwersjigry + ".");
                }
                else
                {
                    Informacja("Polska Lokalizacja PWR " + PobierzNumerWersjiPolonizacji() + " została usunięta z gry Pathfinder Wrath of the Righteous " + kompatybilny_numerwersjigry + ", natomiast deimplementator spolszczenia napotkał przynajmniej jeden krytyczny wyjątek i wyniku tego nie zdołał przywrócić wszystkich plików gry do oryginalnego stanu.");
                    Blad("Przed próbą uruchomienia gry koniecznie sprawdź spójność plików gry w Steam/GoG/Epic.");
                }


                if (File.Exists("unins000.exe") && File.Exists("unins000.dat"))
                {
                    Process[] procesy_nazwa = Process.GetProcessesByName("unins000");

                    if (procesy_nazwa.Length <= 1)
                    {
                        ProcessStartInfo unins000_startInfo = new ProcessStartInfo();
                        unins000_startInfo.CreateNoWindow = false;
                        unins000_startInfo.UseShellExecute = false;
                        unins000_startInfo.FileName = "unins000.exe";
                        unins000_startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        //unins000_startInfo.Arguments = "";



                        try
                        {
                            using (Process unins000_proces = Process.Start(unins000_startInfo))
                            {
                                ZapiszLOG("Automatycznie uruchomiono proces aplikacji odinstalowującej polonizację (unins000.exe).");

                                //unins000_proces.WaitForExit();
                            }
                        }
                        catch
                        {
                            Blad("BŁĄD: Wystąpił nieoczekiwany problem z dostępem do aplikacji odinstalowującej (unins000.exe). Spróbuj uruchomić plik \"unins000.exe\" znajdujący się w folderze \"PWR_PL\" z uprawnieniami Administratora.");
                        }

                    }
                }


            }
            else
            {
                ZapiszLOG("Folder PWR_PL został umieszczony przed próbą implementacji w niewłaściwym miejscu lub brakuje istotnych plików gry (ścieżka zainicjowanego implementatora: " + exe_sciezka + ").");
                Blad("BŁĄD: Weryfikacja plików gry nie powiodła się. Upewnij się, że folder \"PWR_PL\" wraz całą zawartością znajduje się w głównym folderze z zainstalowaną grą Pathfinder Wrath of the Righteous. Jeśli tak jest, a mimo tego wyświetla się ten błąd, wtedy sprawdź spójność plików gry w Steam/GoG/Epic.");
            }


            Koniec();

        }

    }

}