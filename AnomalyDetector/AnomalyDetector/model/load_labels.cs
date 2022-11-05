using System.IO;
using System.Text.Unicode;
using System.Text;
using System.Diagnostics;

namespace AnomalyDetector.model
{
    public class load_labels
    {
        private List<string> label = new List<string>();

        public load_labels(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        label.Add(line);
                    }
                }
            }
        }

        public string Index(int index)
        {
            if (index < 0 || index >= label.Count)
                return "Index Error";
            return label[index];
        }
    }
}
