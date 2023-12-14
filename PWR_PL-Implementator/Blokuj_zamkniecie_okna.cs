using System.Runtime.InteropServices;

namespace PWR_PL_Implementator
{
    class Blokuj_zamkniecie_okna
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public static void Czy_blokowac(bool czy_blokowac = true)
        {
            //bool ustawienie = true;
            //if (czy_blokowac == true) { ustawienie = false; }

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), !czy_blokowac), SC_CLOSE, MF_BYCOMMAND);
        }
    }
}
