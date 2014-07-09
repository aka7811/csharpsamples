using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{

    public class Activity_Series : ActivityI
    {
        
        private bool ___reached_end_flag;

        public List<ActivityI> children;
        public int current_index;
        public ActivityI current;

        
         

        //private void Activate_Essentials()
        //{
        //   // Console.WriteLine("====Activating Chain Activity " + Name);
        //    Frame = 0;
        //    Finished = false;

        //}
        //private void Update_Essentials()
        //{
        //    //Console.WriteLine("====Updating Chain Activity " + Name+ " The frame is " + Frame );
        //    Frame++;
        //    life = Frame / Total_Frames;

        //}

        //private void DeActivate_Essentials()
        //{
        //    //Console.WriteLine("====DeActivating Chain Activity " + Name);
        //    Frame = -1;
        //    Finished = true;


        //}


        public override void Update()
        {
            
            //if (update_bk != null) update_bk(this);
            this.Update_Essentials();

            while (true)
            {
                current.Update();
                var done_pred = current.done_ka(current);
                if (done_pred)
                {
                    var next = current_index + 1;
                    if (next < children.Count)
                    {
                        this.current_set(next);
                    }
                    else
                    {
                        //if (loop == -1)
                        //{
                            ___reached_end_flag = true;
                            break;
                        //}
                        //else
                        //{
                        //    this.current_set(loop);
                        //}
                    }
                }
                else { break; }
            }

        }
        public override  void Activate()
        {

            this.Activate_Essentials();
            ___reached_end_flag = false;
            //if (activate_bk != null) activate_bk();
            current_set(0);
        }
        public override  void DeActivate()
        {
            if (this.Finished) return;
            current_set(-1);
            //if (deactivate_bk != null) deactivate_bk();
            this.DeActivate_Essentials();
        }





      
        private void current_set(int index)
        {
            if (index== current_index) return;
            if(current_index!=-1/*&&current_index!=index*/)
            {
                current.DeActivate();
            }
            if (index == -1)
            {
                current = null;
                current_index = -1;
                return;
            }
            current_index = index;
            current = children[index];
            current.Activate();
        }

        public Activity_Series()
        {
            current_index = -1;
            current = null;
            Finished = true;
            Total_Frames = -1;
            children = new List<ActivityI>();
            //loop = -1;
            //na valeis kai ena allo gia diakoph
             this.done_ka = (act) => this.___reached_end_flag == true;
            //else this.done_pred = (act) => this.reached_end == true || supplied_done(act);
        }

       }
}
