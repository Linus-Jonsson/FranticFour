using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

//Optimiserat? nej. Funkar det? JA!!!
public class LinkInputManager : MonoBehaviour
{
    private const string FILE_NAME = @"\InputManager.asset";
    private const string DIRECTORY_NAME = @"\ProjectSettings";
    private static string filePath;
    private static string inputManagerStartString = "- serializedVersion";
    private const int KEY_NAME_OFFSET = 1;
    private const int JOY_NUM_OFFSET = 15;
    private int inputsLegnth;
    private static string[] inputs;
    private static string[] text;

    private void Awake()
    {
        //Get inputs
        GetInputNames();

        //Get file path
        filePath = Path.GetDirectoryName(Application.dataPath) + DIRECTORY_NAME + FILE_NAME;
    }

    public static void LinkController(int controllerID, int linkID) //Optimisera
    {
        //Null check
        if (string.IsNullOrEmpty(filePath))
            return;

        //Read from file
        text = File.ReadAllLines(filePath);
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].Contains(inputManagerStartString))
            {
                ContainsKeys(text[i + KEY_NAME_OFFSET], text[i + JOY_NUM_OFFSET], linkID, i + JOY_NUM_OFFSET,
                    controllerID);
                i = i + JOY_NUM_OFFSET;
            }
        }

        //Write to file
        File.WriteAllLines(filePath, text); //Kan skapa problem när spelet byggs, undersök!!!
    }

    private static void
        ContainsKeys(string keyText, string joyText, int linkID, int joyTextLine, int controllerID) //Optimisera
    {
        StringBuilder stringBuilder;
        foreach (string key in inputs)
        {
            if (keyText.Contains(key + linkID))
            {
                //Read
                stringBuilder = new StringBuilder(joyText);
                stringBuilder[stringBuilder.Length - 1] =
                    (controllerID + 1).ToString().ToCharArray()[0];
                //Write
                text[joyTextLine] = stringBuilder.ToString();

                Debug.LogWarning(string.Format("{0} changed {1} to {2}", key + linkID, joyText,
                    stringBuilder.ToString())); //Debug
                break;
            }
        }
    }

    private void GetInputNames()
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Static;
        inputsLegnth = typeof(StringManager.Inputs).GetFields(BINDING_FLAGS).Length;

        inputs = new string[inputsLegnth];

        for (var index = 0; index < inputsLegnth; index++)
        {
            FieldInfo field = typeof(StringManager.Inputs).GetFields(BINDING_FLAGS)[index];
            inputs[index] = field.GetValue(field).ToString();
        }
    }
}