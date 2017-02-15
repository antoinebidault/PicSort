using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{

    public class ProgressChangedCustomEventArgs : EventArgs
    { 
        public ProgressChangedCustomEventArgs(int progressPercentage, string label)
        {
            this.Label = label;
            this.ProgressPercentage = progressPercentage;
        }

        public int ProgressPercentage { get; set; }
        public string Label { get; set; }
    }

}
