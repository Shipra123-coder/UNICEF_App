using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AI
{
    public interface IOpenAIService
    {
        Task<string> AskAI(string question);
    }
}
