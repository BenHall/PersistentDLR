using System;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace PersistentDlr.Dlr
{
    public class DlrHost {
        
        private readonly ScriptEngine _scriptEngine;
        private readonly ScriptScope _scriptScope;

        public DlrHost() {
            _scriptEngine = IronRuby.Ruby.CreateEngine();
            _scriptScope = _scriptEngine.CreateScope();
        }

        public object Execute(string commandToExecute) {
            try
            {
                ScriptSource s = _scriptEngine.CreateScriptSourceFromString(commandToExecute, SourceCodeKind.AutoDetect);
                return s.Execute(_scriptScope);
            }
            catch (System.Exception ex) {
                var buffer = "IronRuby failed:" + ex.Message;
                Console.WriteLine(buffer);

                return buffer;
            }
        }
    }
}