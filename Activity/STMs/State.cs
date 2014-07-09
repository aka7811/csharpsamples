using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{
    public class State : STMI
    {

         
        public N_STM Name { get; set; }
       
        public ActivityI activity;
        public List<Transition> transitions { get; set; }
        public bool On { get; set; }
        public Action<State> enter_bk { get; set; }
        public Action<State> exit_bk { get; set; }

        public bool HasWork() { return /*On = true &&*/ activity.Finished == false; }

        public void Update()
        {

            activity.Run_();
            

        }

        public void Activate()
        {
            //Console.WriteLine("Entering State " + Name);
            On = true;
            if (enter_bk != null) enter_bk(this);
            activity.Activate();
        }

        public void DeActivate()
        {
            if (!activity.Finished) activity.DeActivate();
            if (exit_bk != null) 
                exit_bk(this);
            On = false;
           // Console.WriteLine("Exited State " + Name);
        }

        public State( )
        {
             
            this.transitions = new List<Transition>();
            //this.transitions = transitions;
        }


        public static State Null()
        {
            var state = new State();  state.Name= N_STM.Null;  state.activity= AC_L.DoNothing();

            return state;
        }

    }
}
