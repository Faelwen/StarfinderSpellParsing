using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace StarfinderSpellParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            string spellModuleFile = Directory.GetCurrentDirectory() + "\\Starfinderspells.mod";

            Stream spellStream = ProcessSpellArchive(spellModuleFile);

            if (spellStream != null)
            {
                XmlDocument spellXMLDocument = new XmlDocument();
                spellXMLDocument.Load(spellStream);
                spellStream.Close();
            }

            
            Console.WriteLine("End Program");
            Console.ReadKey();



        }


        static Stream ProcessSpellArchive(string spellModuleFile)
        {
            bool isCommonFound = false;
            Stream moduleXMLStream = null;

            try
            {
                using (ZipArchive spellArchive = ZipFile.OpenRead(spellModuleFile))
                {
                    Console.WriteLine("Spell module opened");
                    foreach (ZipArchiveEntry entry in spellArchive.Entries)
                    {
                        Console.WriteLine(entry.FullName);
                        if (entry.FullName == "common.xml")
                        {
                            isCommonFound = true;
                            Console.WriteLine("common.xml file found");
                            moduleXMLStream = entry.Open();
                            break;
                        }
                    }
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine("Warning: Spell module {0} not found", spellModuleFile);
                return null;
            }

            if(!isCommonFound)
            {
                Console.WriteLine("Warning: common.xml file not found in spell module");
                return null;
            }

            return moduleXMLStream;
        }
    }
}
