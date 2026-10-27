using UnityEditor;
using Unity.CodeEditor;

[InitializeOnLoad]
public static class SetExternalEditor
{
    static SetExternalEditor()
    {
        CodeEditor.Editor.SetCodeEditor("/usr/bin/code");
    }
}   
