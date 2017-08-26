using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExcuseManager
{
    [Serializable]
    class Excuse
    {
        public string Description { get; set; }
        public string Result { get; set; }
        public DateTime LastUsed { get; set; }
        public string ExcusePath { get; private set; }
        public Excuse()
        {
            ExcusePath = string.Empty;
        }
        public Excuse(string excusePath)
        {
            OpenFile(excusePath);
        }
        public Excuse(Random random, string folder)
        {
            string[] fileNames = Directory.GetFiles(folder, "*.excuse");
            OpenFile(fileNames[random.Next(fileNames.Length)]);
        }
        public void OpenFile(string excusePath)
        {
            this.ExcusePath = excusePath;
            BinaryFormatter formatter = new BinaryFormatter();
            Excuse tempExcuse;
            using (Stream reader = File.OpenRead(ExcusePath))
            {
                tempExcuse = (Excuse)formatter.Deserialize(reader);
            }
            Description = tempExcuse.Description;
            Result = tempExcuse.Result;
            LastUsed = tempExcuse.LastUsed;

        }
        public void Save(string nameFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using(Stream writer = File.OpenWrite(nameFile))
            {
                formatter.Serialize(writer, this);
            }
        }
    }
}
