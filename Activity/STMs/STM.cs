using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GELib.Service;

namespace GELib.Activity
{
      
    public class STM:STMI
    {
        //public STM_context context { get; set; }

        public N_STM Name { get; set; }   

         
        public Action<STM> enter_bk { get; set; }
        public Action<STM> exit_bk { get; set; }

       

        STMI current;
        public List<STMI> states;  


        #region self-choice
        public STMI Initial;


        /// maybe not  Func<STM,STMI> cause with a factory function you can pass the first arg with a closure
        
        public Func<STM,STMI> rechoose_bk { get; set; }
        
        
        /// afto na kaleis, dinei initial an den exei cb
      
        public STMI ReChoose()
        {
            if (rechoose_bk == null) return Initial;
            else return rechoose_bk(this);
            // TODO implement an example of a (non random) rechoice strategy
        }
        
        Limited_Stack<STMI> history;
        #endregion


         



        #region indicators
        //Gudeline this means that it is between an enter and exit   //On
        public bool On { get; set; }
         
        //Gudeline this means that the activity at the end (state)  of the hierarchy is not finisshed yet
        public bool HasWork()
        {
            if ((current == null)||(On==false)) return false;
            return current.HasWork();
        }
        #endregion



        #region public-user-interface


        public Dictionary<N_STM, STMI> Names { get; set; }

        /// <summary>
        /// call if you want history enabled
        /// </summary>
        /// <param name="size"></param>
        public void Enable_History(int size)
        {
            history = new Limited_Stack<STMI>(size);

        }

        //guideline safe to use from outside
        public void Switch_State(STMI state, bool reseting = false)
        {

            if ((state == current) && (!reseting)) return;

            if (current != null)
                current.DeActivate();

            // REMINDER null initial forces rechoose
            //guideline null setting will rechoose or go to initial
           // if (state == null) state = ReChoose();
            
            
            current = state;
            if (history != null) history.Add(current);

            if (current != null)
                current.Activate();
        }
 
        #endregion


        #region runner-context interface
        public void Update()
        {
           // Console.WriteLine("Updating Machine " + Name);

        
            Check_For_Transitions();
          //  Console.WriteLine("Current State : " + current.Name);
            
            current.Update();



           // context.Next_Frame();
        }
      
        public void Activate()
        {
            On = true;
            //Console.WriteLine("Entering Machine " + Name);
            if (enter_bk != null) enter_bk(this);

            this.Switch_State(Initial, reseting: true);


        }
        public void DeActivate()
        {
            On = false;

            //guideline activity deactivation here
            Switch_State((STMI)null);
            if (exit_bk != null) exit_bk(this);
           // Console.WriteLine("Exited Machine " + Name);
        }
        public void Check_For_Transitions()
        {



            for (var i = 0; i < current.transitions.Count; i++)
            {
                var transition = current.transitions[i];
                if (transition.can_fire(current))
                {
                    //Console.WriteLine("Transition Activated: " + transition.Name);
                    if (transition.action != null) transition.action();
                    Switch_State(transition.to);
                    return;
                }

            }
        }
        #endregion


        public List<Transition> transitions { get; set; }


        //put initial, context, states, and rechoose strategy
        public STM(N_STM Name 
            
            )
        {
            this.Name = Name;
            
            Names=new Dictionary<N_STM,STMI>();
            states = new List<STMI>();
          

         


            transitions = new List<Transition>();
        }

        public void State_Add(params STMI[] _states)
        {
            foreach (var i in _states)
            {
            states.Add(i);
            Names.Add(i.Name, i);
            }
        }
        
       
    }
   
}
