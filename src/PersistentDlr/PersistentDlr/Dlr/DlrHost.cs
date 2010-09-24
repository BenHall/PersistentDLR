using System;
using System.IO;
using System.Text;
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
            try {
                MemoryStream outputStream = RedirectOutputToStream();

                ScriptSource scriptSource = _scriptEngine.CreateScriptSourceFromString(commandToExecute, SourceCodeKind.AutoDetect);
                var resultOfExecution = scriptSource.Execute(_scriptScope);

                if (!HasOutput(resultOfExecution))
                    return ReadFromStream(outputStream);

                return resultOfExecution;
            }

            catch (Exception ex) {
                var buffer = string.Format("IronRuby failed: {0} - {1}", ex.Message, ex.StackTrace);
                Console.WriteLine(buffer);

                return buffer;
            }
        }

        private bool HasOutput(object execute) {
            return execute != null && !string.IsNullOrEmpty(execute.ToString());
        }

        private MemoryStream RedirectOutputToStream() {
            var memoryStream = new MemoryStream();
            _scriptEngine.Runtime.IO.SetOutput(memoryStream, new StreamWriter(memoryStream));
            _scriptEngine.Runtime.IO.SetErrorOutput(memoryStream, new StreamWriter(memoryStream));
            return memoryStream;
        }

        private static string ReadFromStream(MemoryStream ms)
        {
            int length = (int)ms.Length;
            Byte[] bytes = new Byte[length];

            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, (int)ms.Length);

            return Encoding.GetEncoding("utf-8").GetString(bytes, 0, (int)ms.Length);
        }
    }
}