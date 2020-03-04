using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class EditorFactory
    {
        private static EditorFactory _inst = new EditorFactory();
        public static EditorFactory Inst  { get => _inst; }

        private Dictionary<string, Func<string, EditorDefault>> _listOfBuilders = new Dictionary<string, Func<string, EditorDefault>>();

        public void AddBuilder(string toolName,Func<string,EditorDefault> buildAction)
        {
            _listOfBuilders[toolName] = buildAction;
        }

        public EditorDefault GetEditor(string toolName)
        {
            string lookUpName = toolName;
            if (toolName.EndsWith("-32Bit"))
            {
                lookUpName = toolName.Substring(0, toolName.LastIndexOf("-32Bit"));
            }

            switch (lookUpName)
            {
                case RuntimeInfo.Arduino:
                    return new ArduinoEditor();
                case RuntimeInfo.CodeBlocks:
                    return new Editors.CodeBlocksEditor();
                case RuntimeInfo.CodeLite:
                    return new Editors.CodeLiteEditor();
                case RuntimeInfo.Komodo:
                    return new Editors.KomodoEditor();
                case RuntimeInfo.MobXTerm:
                    return new Editors.MobXtermEditor();
                case RuntimeInfo.Python:
                    return new Editors.PythonEditor();
                case RuntimeInfo.VSCode:
                    return new Editors.VSCodeEditor();
                case RuntimeInfo.VSStudio:
                    return new Editors.VisualStudioEditor();
                case RuntimeInfo.WinDbg:
                    return new Editors.WinDbgEditor(toolName);
                case RuntimeInfo.WingIDE:
                    return new Editors.WingIDEEditor();
                case RuntimeInfo.OpenSSH:
                    return new Editors.OpenSSHEditor();
            }

            Func<string, EditorDefault> buildAction;
            if(_listOfBuilders.TryGetValue(lookUpName,out buildAction))
            {
                return buildAction(toolName);
            }
            else
            {
                return new Editors.EditorDefault(toolName);
            }
        }
    }
}
