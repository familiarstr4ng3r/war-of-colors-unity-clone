using System.IO;
using UnityEditor;
using WOC;

public class SaveContextMenu
{
    [MenuItem("Cucumber 🥒/Delete Save", true)]
    private static bool SaveExists()
    {
        return File.Exists(SaveManager.GetSaveDirectory(SaveHandler.FileName));
    }

    [MenuItem("Cucumber 🥒/Delete Save")]
    private static void DeleteSave()
    {
        SaveManager.DeleteSave(SaveHandler.FileName);

        //string a = SaveHandler.FileName;
        //string[] files = new string[] { a, a + ".meta" };

        //for (int i = 0; i < files.Length; i++)
        //{
        //    string f = SaveManager.GetSaveDirectory(files[i]);
        //    File.Delete(f);
        //}
    }
}