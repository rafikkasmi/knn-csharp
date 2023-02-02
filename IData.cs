using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP01_Heart_Diagnostic
{
    internal interface IData
    {
        float[] Features { get; }
        bool Label { get; }
        void PrintInfo();
    }
}
