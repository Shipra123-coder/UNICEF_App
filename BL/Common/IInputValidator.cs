using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public interface IInputValidator
    {
        public result ContainsScriptTags(dynamic modelValue);
        public result ValidateInput(dynamic modelValue);
        public bool ContainsScriptTags(string input);

    }
}
