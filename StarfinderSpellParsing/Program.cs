using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using CsvHelper;

namespace StarfinderSpellParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Program");
            string directory = Directory.GetCurrentDirectory();
            string spellModuleXML = directory + "\\common.xml";
            string spellShortDescriptionTSV = directory + "\\SpellShortDescription.tsv";


            using (StreamReader spellModuleStream = new StreamReader(spellModuleXML))
            using (StreamReader spellShortDescriptionStream = new StreamReader(spellShortDescriptionTSV))
            {
                Console.WriteLine("Files opened");

                CsvReader spellShortDescriptionCSVreader = new CsvReader(spellShortDescriptionStream);
                IEnumerable<SpellShortDescription> records = spellShortDescriptionCSVreader.GetRecords<SpellShortDescription>();

                XmlDocument spellModuleXmlDocument = new XmlDocument();
                spellModuleXmlDocument.Load(spellModuleStream);

                spellShortDescriptionCSVreader.Dispose();
            }

                Console.WriteLine("End Program");
            Console.ReadKey();



        }

  
    }
}
