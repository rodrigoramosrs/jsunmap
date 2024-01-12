using jsunmap.Services;
using System.Text.Json;

namespace jsunmap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrintHeader();
            

            if (args.Count() <= 0)
            {
                Console.WriteLine("[!] No file or url was provided.");
                return;
            }

            Console.WriteLine($"[ INFO ]");
            Console.WriteLine($"OS: {Environment.OSVersion}");
            Console.WriteLine($"Root Path: {GlobalSettings.RootPath}");
            Console.WriteLine($"Output Path: {GlobalSettings.OutputPath}");
            Console.WriteLine($"");
            Console.WriteLine($"");

            foreach (string arg in args)
            {
                JsUnMapService.StartUnMapJS(arg).GetAwaiter().GetResult();
            }

            
            Console.WriteLine("Process finished...");
        }

        private static void PrintHeader()
        {
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.WriteLine("                  [ Uncovering the 'MAP' ]             ");
            Console.WriteLine(@"           ___      _   _      ___  ___             ");
            Console.WriteLine(@"          |_  |    | | | |     |  \/  |             ");
            Console.WriteLine(@"            | | ___| | | |_ __ | .  . | __ _ _ __   ");
            Console.WriteLine(@"            | |/ __| | | | '_ \| |\/| |/ _` | '_ \  ");
            Console.WriteLine(@"        /\__/ /\__ \ |_| | | | | |  | | (_| | |_) | ");
            Console.WriteLine(@"        \____/ |___/\___/|_| |_\_|  |_/\__,_| .__/  ");
            Console.WriteLine(@"                                            | |     ");
            Console.WriteLine(@"                                            |_|     ");
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.WriteLine("by: Rodrigo Ramos ( https://github.com/rodrigoramosrs ) ");
            Console.WriteLine("");
        }
    }
}
