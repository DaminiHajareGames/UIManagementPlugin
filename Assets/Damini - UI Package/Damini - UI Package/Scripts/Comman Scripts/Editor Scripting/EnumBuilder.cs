#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class EnumBuilder
{
    public static void CreateEnumFromList(string enumName, List<string> enumEntries)
    {
        string filePathAndName = "Assets/Damini - UI Package/Damini - UI Package/Scripts/Enums/EnumsFromEnumBuilder/" + enumName + ".cs"; //The folder Scripts/Enums/ is expected to exist
        List<string> copyOfEnumEntries = new List<string>();
       // string enumEntry;

        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.WriteLine("public enum " + enumName);
            streamWriter.WriteLine("{");
            copyOfEnumEntries.Clear();

            for (int i = 0; i < enumEntries.Count; i++)
            {
               // enumEntry = enumEntries[i].Replace(" ", string.Empty);
                copyOfEnumEntries.Add(enumEntries[i].Replace(" ", string.Empty));

                Debug.Log("Enum = " + copyOfEnumEntries[i]);
                streamWriter.WriteLine("\t" + copyOfEnumEntries[i] + ",");
            }
            streamWriter.WriteLine("}");
        }
        AssetDatabase.Refresh();
    }
}
#endif
