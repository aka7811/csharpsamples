using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{
    public   class ActivityI
    {
        public int Frame { get; set; }
        public string Name { get; set; }
        public int Total_Frames { get; set; }
        //public float life { get; set; }
        public bool Finished { get; set; }
        public ActivityI() { Finished = true; Frame = -1; }
        public virtual void Update()
        {
            //Console.WriteLine("====Updating Activity " + Name);  
           
            this.Update_Essentials();
        }
        public virtual void Activate()
        {
            this.Activate_Essentials();
           // if (activate_bk != null) activate_bk();
        }
        public virtual void DeActivate()
        {
          
            
            this.DeActivate_Essentials();
        }

        
        

        public Action activate_bk { get; set; }
        public Action deactivate_bk { get; set; }
        public Action<ActivityI> update_bk { get; set; }

        public Func<ActivityI, bool> highjack_pred { get; set; }
        public Func<ActivityI, bool> done_ka { get; set; }
    }

    public static class Exten
    {

        public static  void Activate_Essentials(this ActivityI a)
        {
            if (!a.Finished) return;
            a.Frame = 0;
            a.Finished = false;
            //a.life = 0;
            if (a.activate_bk != null) a.activate_bk();

        }
        public static void Update_Essentials(this ActivityI a)
        {
            if (a.update_bk != null) a.update_bk(a);
            //if (a.Finished) throw new Exception();
            a.Frame++;

            //if (a.Total_Frames != 0)  
            //a.life = (float) a.Frame / a.Total_Frames;

        }

        public static void DeActivate_Essentials(this ActivityI a)
        {
            //if (a.Finished) return;
            if (a.deactivate_bk != null) a.deactivate_bk();
            a.Frame = -1;
            a.Finished = true;


        }

        public static void Run_(this ActivityI ac)
        {
            if (!ac.Finished)
                ac.Update();
            else
            { return; }

            if (ac.done_ka(ac))
            {
                //ac.Finished = true;
                ac.DeActivate();
            }
        }


       
    }

     
  
}
