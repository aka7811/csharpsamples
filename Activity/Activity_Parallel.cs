using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{
    public enum ACTP
    {
        Any,All 
    }
    public class Activity_Parallel:ActivityI
    {

        public List<ActivityI> children;


        //private void Activate_Essentials()
        //{
        //   // Console.WriteLine("====Activating Parallel Activity " + Name);
        //    Frame = 0;
        //    Finished = false;

        //}
        //private void Update_Essentials()
        //{
        //    //Console.WriteLine("====Updating Parallel Activity " + Name + " The frame is " + Frame);
        //    Frame++;
        //    life = Frame / Total_Frames;

        //}

        //private void DeActivate_Essentials()
        //{
        //   // Console.WriteLine("====DeActivating Parallel Activity " + Name);
        //    Frame = -1;
        //    Finished = true;


        //}

        
        public override  void Update()
        {
            
            //if (update_bk != null) update_bk(this);
            this.Update_Essentials();

            for (var i = 0; i < children.Count; i++)
                if (!children[i].Finished)
                {
                    children[i].Update();
                    if(children[i].done_ka(children[i]))children[i].DeActivate();
                }


        }
        public override  void Activate()
        {

            this.Activate_Essentials();
            //reached_end = false;
            //if (activate_bk != null) activate_bk();
            for (var i = 0; i < children.Count; i++) children[i].Activate();
        }
        public override  void DeActivate()
        {
            if(this.Finished) return;

            for (var i = 0; i < children.Count; i++)
                if (!children[i].Finished) children[i].DeActivate();
                
            //if (deactivate_bk != null) deactivate_bk();
            this.DeActivate_Essentials();
        }



        public Activity_Parallel(ACTP mode)
        {
            
            Finished = true;
            Total_Frames = -1;
            children = new List<ActivityI>();
             
            //na valeis kai ena allo gia diakoph
            //if(mode==ACTP.Custom)
             //this.done_pred = supplied_done;
            if(mode==ACTP.All)
                this.done_ka = (act) =>
                    {
                       for(var i=0;i<children.Count;i++)
                       { if (!children[i].Finished) return false; }
                       return true;
                    };
            else if (mode == ACTP.Any)
                this.done_ka = (act) =>
                {
                    for (var i = 0; i < children.Count; i++)
                    { if (children[i].Finished) return true; }
                    return false;
                };
        }

     

    }
}
