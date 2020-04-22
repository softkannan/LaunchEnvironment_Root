using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaunchEnvironment.Editors
{
    public class EditorFactory
    {
        private static EditorFactory _inst = new EditorFactory();
        public static EditorFactory Inst  { get => _inst; }

        public EditorDefault GetEditor(string toolName)
        {
            string lookUpName = toolName;
            if (toolName.EndsWith("-32Bit"))
            {
                lookUpName = toolName.Substring(0, toolName.LastIndexOf("-32Bit"));
            }

            var tool = RuntimeInfo.Inst.GetTool(lookUpName);

            if(tool == null)
            {
                ErrorLog.Inst.ShowError("Unable to find Tool : {0}, please verify Tools.xml", lookUpName);
                return null;
            }

            // if custom editor is defined then use defined editor.
            var tempEditorName = tool.Editor;
            if(!string.IsNullOrWhiteSpace(tempEditorName))
            {
                lookUpName = tempEditorName;
            }

            EditorDefault retVal = null;

            Type typeObj = Assembly.GetExecutingAssembly().GetType(string.Format("LaunchEnvironment.Editors.{0}Editor",lookUpName));
            if (typeObj != null)
            {
                retVal = Activator.CreateInstance(typeObj) as EditorDefault;
            }
            else
            {
                retVal = new Editors.EditorDefault();
            }

            retVal.Initialize(tool);

            return retVal;
        }
    }
}
