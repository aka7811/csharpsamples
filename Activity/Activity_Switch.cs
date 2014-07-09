using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GELib.Service;

namespace GELib.Activity
{
    public class Activity_Switch : ActivityI
    {
         
        public Func<Activity_Switch, ActivityI> choice_cb { get; set; }

        public ActivityI chosen { get; set; }

   
        Limited_Stack<ActivityI> history { get; set; }
        public void ChoseActive()
        {
            ActivityI choice = this.choice_cb(this) ;
           
            history.Add(choice);
            choice.Activate();
            chosen = choice;
        }
       
        public override void Update()
        {
            //if (update_bk != null) update_bk(this);
            this.Update_Essentials();
          
            chosen.Update();
            //var done = chosen.done_pred(chosen);
            
            //if(done)
            //{
            //    chosen.DeActivate();
                
                
            //     this.reached_end = true; 
                
            //    return;
            //}
            

        }
        public override void Activate()
        {

            this.Activate_Essentials();
           // reached_end = false;
            ChoseActive();
            //if (activate_bk != null) activate_bk();
            
        }
        public override void DeActivate()
        {
            if (this.Finished) return;
            chosen.DeActivate();

            //if (deactivate_bk != null) deactivate_bk();
            this.DeActivate_Essentials();
        }



        public List<ActivityI> children;

        public Activity_Switch(/*, Func<ActivityI, bool> supplied_done = null*/)
        {

            Finished = true;
            Total_Frames = -1;
            children = new List<ActivityI>();
            history = new Limited_Stack<ActivityI>(10);
            //this.loop_on_end = loop_on_end;

            //na valeis kai ena allo gia diakoph
            
                this.done_ka = (obj)=>{return this.chosen.done_ka(this.chosen);};
             
        }



    }
}
