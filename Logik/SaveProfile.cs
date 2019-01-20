using System;
using System.IO;
using System.Xml.Serialization;

namespace Logik
{
    public class SaveProfile
    {
        public void Spara<TIn>(TIn objekt, string fil)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += $"/{fil}.xml";
            var serializer = new XmlSerializer(typeof(TIn));
            using (var stream = new StreamWriter(path))
            {
                serializer.Serialize(stream, objekt);
            }
        }
        public void CreateXML(string fileName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += $"/{fileName}.xml";

            var dir = new FileInfo(path);
            if (dir.Exists == false)
            {
                var file = dir.Create();
                file.Close();
            }
        }
    }
}
