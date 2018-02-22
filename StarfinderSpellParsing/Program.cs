using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace StarfinderSpellParsing
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Start Program");
            var directory = Directory.GetCurrentDirectory();
            var spellModuleXML = directory + "\\common.xml";
            var spellShortDescriptionTSV = directory + "\\SpellShortDescription.tsv";

            using (var spellModuleStream = new StreamReader(spellModuleXML))
            using (var spellShortDescriptionStream = new StreamReader(spellShortDescriptionTSV))
            {
                Console.WriteLine("Files opened");

                var spellShortDescriptionCSVreader = new CsvReader(spellShortDescriptionStream);
                spellShortDescriptionCSVreader.Configuration.Delimiter = "\t";
                spellShortDescriptionCSVreader.Configuration.HasHeaderRecord = false;
                var records = spellShortDescriptionCSVreader.GetRecords<SpellShortDescription>();

                var spellModuleXmlDocument = new XmlDocument();
                spellModuleXmlDocument.Load(spellModuleStream);

                Console.WriteLine(GetSpellShortDescriptionRecord("Mind Thrust I", records).Name);

                spellShortDescriptionCSVreader.Dispose();
            }

            Console.WriteLine("End Program");
            Console.ReadKey();
        }

        private static SpellShortDescription GetSpellShortDescriptionRecord(String spellname, IEnumerable<SpellShortDescription> records)
        {
            var isFound = false;
            var record = new SpellShortDescription();

            foreach (SpellShortDescription row in records)
            {
                if (row.Name == spellname)
                {
                    isFound = true;
                    record = row;
                    break;
                }
            }

            if (!isFound) { return null; }
            return record;
        }
    }
}