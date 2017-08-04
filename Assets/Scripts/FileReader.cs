using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class FileReader {

    public string[] listToArray(List<string> list)
    {
        string[] array = new string[list.Count];

        for(int i = 0; i < array.Length; i++)
        {
            array[i] = list[i];
        }

        return array;
    }


    public List<string> readFile(string fileName)
    {
        List<string> content = new List<string>();

        string filepath = Application.dataPath + "/Resources/Files/" + fileName;

        if (File.Exists(filepath))
        {

            try
            {
                string line;

                StreamReader reader = new StreamReader(filepath);

                using (reader)
                {

                    do
                    {
                        line = reader.ReadLine();

                        if (line != null)
                        {
                            content.Add(line);
                        }
                    }
                    while (line != null);

                    // Done reading, close the reader and return true to broadcast success    
                    reader.Close();
                    return content;
                }
            }
            // If anything broke in the try block, we throw an exception with information
            // on what didn't work
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }

        }
        return null;
    }

}
