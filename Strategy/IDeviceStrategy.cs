using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_3.Strategy
{
    public interface IDeviceStrategy
    {
        void DisplaySpecificOptions();
        bool HandleSpecificOption(char choice);
    }
}
