using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GELib.Service;
namespace GELib.Activity 
{
    public enum N_M_
    {
        Nothing,
        Boosted,
        Hitted,
        Die,
        Follow,
        Flock,
        Stuck,
        Normal,
        Stop,


        Unhitted,
        Unboosted,

        Pad,
        Kill,
        Fire_Trigger,Fire_Force_Trigger, Fire_Blast_Trigger,

        Too_force,

        Full



    }

    public enum N_STM
    {
        Null,

        Move, Animation, Life, Move_Rot,


        Spawning, Normal, Death,
        Normal_Rot, Boosting, After_Contact, Breaking,

        Flock, Stay, Escape,


        Fire_Force, Fire_Blast,

        Recharging, Firing,
        Too_Force

    }
    
    public static class N_
    {
       

      
    }
    
    public class STM_Batch
    {
        public STM_context context;

        List<STM> state_machines;
        public Dictionary<N_STM, STM> Names;
        
        public STM_Batch()
        {
            Names = new Dictionary<N_STM, STM>();
            state_machines = new List<STM>();
            //contexts = new List<STM_context>();
        }
        public void STM_Add(params STM[] machines)
        {
            foreach (var i in machines)
            {
                state_machines.Add(i);
                Names.Add(i.Name, i);
            }
            
        }
        public void Update()
        {
            for (var i = 0; i < state_machines.Count; i++)
            {
                state_machines[i].Update();
            }
          
           
        }

        public void End_Frame()
        {
            context.End_Frame();
        }

        public void Activate()
        {
            for (var i = 0; i < state_machines.Count; i++)
            {
                state_machines[i].Activate();
            }
        }

        public void DeActivate()
        {
            for (var i = 0; i < state_machines.Count; i++)
            {
                state_machines[i].DeActivate();
            }
        }
    }
}
