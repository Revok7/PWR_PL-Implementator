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
        private static void Blad(string tresc)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(tresc);
            Console.ResetColor();

        }
        private static void Sukces(string tresc)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine(tresc);
            Console.ResetColor();

        }
        private static void Sukces2(string tresc)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(tresc);
            Console.ResetColor();
        }
        private static void Informacja(string tresc)
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.WriteLine(tresc);
            Console.ResetColor();
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
                    Blad("BŁĄD: Wystąpił nieoczekiwany problem z odczytem przynajmniej jednego pliku gry. Spróbuj uruchomić instalator spolszczenia z uprawnieniami Administratora.");
                }

                plikVersionInfo_fs.Close();

                return plikVersionInfo_tresc;
            }
            else
            {
                return "NULL NULL NULL NULL NULL";
            }
        }

        static void Main(string[] args)
        {

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
                File.Exists("..\\Wrath_Data\\StreamingAssets\\Localization\\")
                &&
                File.Exists("..\\Wrath_Data\\StreamingAssets\\IntroductoryText.json")
                &&
                File.Exists("Implementacja\\Wrath_Data\\StreamingAssets\\Localization\\")
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

                if (kompatybilnoscspolszczenia_dane == aktualniezainstalowanawersjagry_dane)
                {

                    /*
                    if exist "Implementacja\Wrath_data\sharedassets0.assets"(

                    if not exist "..\Wrath_Data\sharedassets0.assets.ORIG.BAK"(move "..\Wrath_Data\sharedassets0.assets" "..\Wrath_Data\sharedassets0.assets.ORIG.BAK" >> tmp.log)

                    copy "Implementacja\Wrath_Data\sharedassets0.assets" "..\Wrath_Data\sharedassets0.assets" >> tmp.log
	                )
                    */

                    if (File.Exists("Implementacja\\Wrath_data\\sharedassets0.assets") == true)
                    {
                        if (File.Exists("..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry) == false)
                        {
                            File.Move("..\\Wrath_Data\\sharedassets0.assets", "..\\Wrath_Data\\sharedassets0.assets.ORIG.BAK-" + kompatybilny_numerwersjigry);
                        }
                    }

                    /*
                    	if exist "Implementacja\bundle-ui\" (
		                if exist "Implementacja\bundle-ui\xdelta3.exe" (
			                if exist "Implementacja\bundle-ui\pwr_pl-ui.patch" (
				                if exist "..\Bundles\ui.PWR_PL" ( del "..\Bundles\ui.PWR_PL" )
				
				                "Implementacja\bundle-ui\xdelta3.exe" -d -s "..\Bundles\ui" "Implementacja\bundle-ui\pwr_pl-ui.patch" "..\Bundles\ui.PWR_PL"
				
				                if exist "..\Bundles\ui.PWR_PL" (
					                if not exist "..\Bundles\ui.ORIG.BAK" ( move "..\Bundles\ui" "..\Bundles\ui.ORIG.BAK" >> tmp.log )
					                move "..\Bundles\ui.PWR_PL" "..\Bundles\ui" >> tmp.log					
				                )
			                )	
		                )	
	                )
  
                    */

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

                        //"Implementacja\bundle-ui\xdelta3.exe" - d - s "..\Bundles\ui" "Implementacja\bundle-ui\pwr_pl-ui.patch" "..\Bundles\ui.PWR_PL"

                        if (File.Exists("..\\Bundles\\ui.PWR_PL") == true)
                        {
                            if (File.Exists("..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry) == false)
                            {
                                File.Move("..\\Bundles\\ui", "..\\Bundles\\ui.ORIG.BAK-" + kompatybilny_numerwersjigry);
                            }
                        }

                    }

                }
                else
                {
                    Blad("BŁĄD: Nie można zainstalować spolszczenia, ponieważ wystąpiła niezgodność wersji spolszczenia z zainstalowaną wersją gry.");
                    Informacja("Upewnij się, że instalujesz wersję spolszczenia zgodną z aktualnie zainstalowaną wersją gry.");
                }

            }
            else
            {
                Blad("BŁĄD: Weryfikacja plików gry nie powiodła się. Upewnij się, że folder \"PWR_PL\" wraz całą zawartością znajduje się w głównym folderze z zainstalowaną grą Pathfinder Wrath of the Righteous. Jeśli tak jest, a mimo tego wyświetla się ten błąd, wtedy sprawdź spójność plików gry w Steam/GoG/Epic, a nastepnie spróbuj ponownie zainstalować spolszczenie.");
            }


            Console.ReadKey();
        }
    }
}