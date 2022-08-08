using System;
using System.Threading.Tasks;

namespace TanslateBusinessLayer
{
    public interface ITranslateText
    {
        Task<string> Translate(string text, string expected);
    }
}
