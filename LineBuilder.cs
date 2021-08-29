using System.Linq;
using System.Text;

namespace OpcodeGenerator {
    public class LineBuilder {
        private int _tabs;
        public string Tabs {
            get {
                if (!_isLineBreak)
                    return "";
                var res = "";
                for (var i = 0; i < _tabs; i++)
                    res += '\t';
                return res;
            }
        }

        private readonly StringBuilder _builder = new StringBuilder();

        public void Push(int count = 1) {
            if (count < 1)
                return;
            _tabs += count;
        }

        public void Pop(int count = 1) {
            if (count < 1)
                return;
            _tabs -= count;
            if (_tabs < 0)
                _tabs = 0;
        }

        private bool _isLineBreak = true;

        public void AppendLine(string line) {
            _builder.AppendLine(Tabs + line);
            _isLineBreak = true;
        }

        public void Append(string text) {
            _builder.Append(Tabs + text);
            _isLineBreak = text.Contains('\n');
        }

        public override string ToString() {
            return _builder.ToString();
        }
    }
}
