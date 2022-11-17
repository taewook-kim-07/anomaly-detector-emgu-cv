using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetector.utils
{
    public class ButtonStatus
    {
        public struct buttonResult
        {
            public string label_name;
            public int buttonCnt;
            public bool normal;
        };

        private List<buttonResult> button_result = new List<buttonResult>();

        public ButtonStatus(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        button_result.Add(new buttonResult()
                        {
                            label_name = line,
                            buttonCnt = 0,
                            normal = false
                        });
                    }
                }
            }
        }

        public string name(int index)
        {
            return button_result[index].label_name;
        }

        public void clear()
        {
            Parallel.For(0, button_result.Count, (i) =>
            {
                buttonResult button = button_result[i];
                button.buttonCnt = 0;
                button.normal = false;
                button_result[i] = button;
            });
        }

        public void add(int button_idx, bool normal)
        {
            buttonResult button = button_result[button_idx];
            button.buttonCnt += 1;
            button.normal = normal;
            button_result[button_idx] = button;
        }

        public string isNormal()
        {
            string ret = "";

            foreach(var button in button_result)
            {
                if (!(button.buttonCnt == 1 && button.normal))
                {
                    if (ret.Length != 0)
                        ret = string.Concat(ret, $"|{button.label_name}");
                    else
                        ret = button.label_name;
                }
            }
            return ret;
        }
    }
}
