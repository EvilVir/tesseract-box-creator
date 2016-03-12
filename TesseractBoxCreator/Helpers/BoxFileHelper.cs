using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesseractBoxCreator.Model;

namespace TesseractBoxCreator.Helpers
{
    public class BoxFileHelper
    {
        public static BoxesList LoadFromFile(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                BoxesList output = new BoxesList();

                string line = null;
                while ((line = file.ReadLine()) != null)
                {
                    BoxItem item = DeserializeBoxItem(line);
                    if (item != null)
                    {
                        output.Add(item);
                    }
                }

                return output;
            }
        }

        public static BoxItem DeserializeBoxItem(string entry)
        {
            if (entry != null)
            {
                string[] parts = entry.Trim().Split(' ');
                if (parts.Length == 6)
                {
                    BoxItem output = new BoxItem();

                    output.Letter = parts[0][0];
                    output.X = Int32.Parse(parts[1]);
                    output.Y = Int32.Parse(parts[2]);
                    output.X2 = Int32.Parse(parts[3]);
                    output.Y2 = Int32.Parse(parts[4]);
                    output.Page = Int32.Parse(parts[5]);

                    return output;
                }
            }

            return null;
        }

        public static void SaveToFile(string path, BoxesList list)
        {
            using (StreamWriter file = new StreamWriter(path, false))
            {
                if (list != null)
                {
                    foreach (BoxItem item in list.GetSorted())
                    {
                        file.WriteLine(SerializeBoxItem(item));
                    }
                }
            }
        }

        public static string SerializeBoxItem(BoxItem item)
        {
            return String.Format("{0} {1} {2} {3} {4} {5}", item.Letter, item.X, item.Y, item.X2, item.Y2, item.Page);
        }
    }
}
