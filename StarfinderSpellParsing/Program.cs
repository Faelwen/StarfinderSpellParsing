using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

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


            using (var spellShortDescriptionStream = new StreamReader(spellShortDescriptionTSV))
            using (var spellModuleStream = new StreamReader(spellModuleXML, Encoding.GetEncoding(28591)))
            {
                Console.WriteLine("Files opened");

                var spellShortDescriptionCSVreader = new CsvReader(spellShortDescriptionStream);
                spellShortDescriptionCSVreader.Configuration.Delimiter = "\t";
                spellShortDescriptionCSVreader.Configuration.HasHeaderRecord = false;
                var records = spellShortDescriptionCSVreader.GetRecords<SpellShortDescription>();
                var recordList = new List<SpellShortDescription>();
                foreach(var record in records) { recordList.Add(record); }
                var spellModuleXmlDocument = new XmlDocument();
                spellModuleXmlDocument.Load(spellModuleStream);

                //Get spells from xml file
                var root = spellModuleXmlDocument.DocumentElement;
                var spellrefnode = root.SelectNodes("/root/reference/spells/category/*");

                //Cross ref with the csv file
                foreach(XmlNode spell in spellrefnode)
                {
                    var spellName = spell["name"].InnerText;
                    var shortDescriptionRecord = GetSpellShortDescriptionRecord(spellName, recordList);
                    if (shortDescriptionRecord == null)
                    {
                        Console.WriteLine("{0} not found!", spellName);
                    }
                }

                spellShortDescriptionCSVreader.Dispose();
            }

            Console.WriteLine("End Program");
            Console.ReadKey();
        }

        private static SpellShortDescription GetSpellShortDescriptionRecord(String spellname, List<SpellShortDescription> records)
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