using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{
    public class Repeat_Until:ActivityI
    {
   

       

        public ActivityI inner;

        public override  void Update()
        {
           // Console.WriteLine("====Updating Conditioned Activity " + Name);
           // if (update_bk != null) update_bk(this);
            this.Update_Essentials();
            inner.Update();
            if(inner.done_ka(inner)/*&&!this.done_pred(this)*/)
            {
                inner.DeActivate();
                
                inner.Activate();
            }
        }
        public override  void Activate()
        {
            this.Activate_Essentials();
           
            inner.Activate();
        }
        public override  void DeActivate()
        {
            if (this.Finished) return;
            inner.DeActivate();
            //if (deactivate_bk != null) deactivate_bk();
            this.DeActivate_Essentials();
            
        }
        
        
        
        public Repeat_Until()
        {
            Finished = true;
            Total_Frames = -1;
           
        }

    }
}
