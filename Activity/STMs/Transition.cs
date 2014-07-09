using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{
    public class Transition
    {
        public string Name;
        public STMI from;
        public STMI to;

        // SIDEMARK  Is that of real use? Probably hough I've not found any

        public Action action;
        public Func<STMI, bool> can_fire;
        public Transition(  STMI from, STMI to, Func<STMI, bool> can_fire)
        {
            //this.Name = Name;
            this.from = from;
            this.to = to;
            this.can_fire = can_fire;
            this.from.transitions.Add(this);

        }


        //public static List<Transition> _(params Transition[] transitions)
        //{
        //    var ret = new List<Transition>();
        //    ret.AddRange(transitions);

        //    return ret; 
        //}

        public static List<Transition> Create(  STMI to, Func<STMI, bool> can_fire, params STMI[] starting )
        {
            var ret = new List<Transition>();
            foreach (var u in starting)
            {
                ret.Add(new Transition( u, to, can_fire));
            }
            return ret;
        }


    }

    public static class Extensions
    {

        //just wrapper
        //public static void Par_All(this ActivityI activity, params ActivityI[] activities)
        //{
        //    _A.Parallel(ACTP.All, null, activities);

        //}

        //REMINDER basic way to chain 2 states as with the Meecanim system
        public static Transition EndWith(this STMI from, STMI to)
        {
            var str = to != null ? to.Name+"" : "null";
            var ret= new Transition( from, to, (stmi) => { return !stmi.HasWork(); });
            ret.Name="Ending " + from.Name + " to " + str;
            return ret;

        }

        public static List<Transition> Action_All(this List<Transition> trs, Action action)
        {
            foreach (var tr in trs)
            {
                tr.action = action;
            }
            return trs;
        }
        
    }



    //public struct Message
    //{
    //    public string name;
    //    public int Frame;
    //}

    //idea an den piasei to context me to dictionary se xrono afto ine me hashset
    //public class STM_context
    //{
    //   

    //    public HashSet<string> messages;
    //    public List<Message> messages_list; 
    //    public STM_context()
    //    {
    //        messages_list = new List<Message>();
    //        messages = new HashSet<string>();
    //    }
    //    public void Finished_Update()
    //    {
    //        messages.Clear();
    //    }
    //    public void Start_Update()
    //    {
    //        Console.WriteLine("Messages in the context:");
    //        foreach (var m in messages)
    //            Console.Write(m + " | ");
    //        Console.WriteLine();
    //    }
    //}



   
}
