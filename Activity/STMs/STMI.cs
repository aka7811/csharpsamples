using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{
    public interface STMI
    {
        N_STM Name { get; set; }

        void Activate();
        void DeActivate();
        void Update();
        
        
        bool On { get; set; }
        bool HasWork();
        
        List<Transition> transitions { get; set; }

         
    }
}
