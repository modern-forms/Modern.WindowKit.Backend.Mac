using Mono.Cecil;

namespace ModernComPatcher
{
    public class Program
    {
        public static int Main (string[] args)
        {
            if (args.Length == 0) {
                Console.WriteLine ("Assembly to patch not specified.");
                return 1;
            }

            if (!File.Exists (args[0])) {
                Console.WriteLine ($"Assembly '{args[0]}' not found.");
                return 1;
            }

            // We need a MemoryStream so we can overwrite the input assembly
            using var ms = new MemoryStream ();

            using (var file = File.OpenRead (args[0]))
                file.CopyTo (ms);

            ms.Position = 0;

            var ass = AssemblyDefinition.ReadAssembly (ms);

            ComInteropHelper.PatchAssembly (ass);

            ass.Write (args[0]);

            return 0;
        }
    }
}