using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace GELib.Activity
{

    // Idea should the fire and move patterns from BUlletML and the like, be given direct interfaces here
    // or should it be left and all specified with lambda's
    public static class _A
    {

        public static Func<ActivityI, bool> W_N_Frames(int n)
        {
            return (a) => {

                var r = a.Frame > n - 1;
                
                return a.Frame > n - 1; 
            }; ;
        }
        public static Func<ActivityI, bool> W_N_Frames(Func<int> frames)
        {
            return (a) =>
            {

                
                
                return a.Frame > frames() - 1;
            }; 
        }
     
         
      
        public static ActivityI A0()
        {
            var ret = new ActivityI();
            ret.done_ka = _A.W_N_Frames(0);
            //ret.done_ka = w;
            //ret.activate_bk = r;
            //ret.update_bk = u;
            //if (r == null) ret.Name = "Waiter";
            return ret;

        }


        public static ActivityI ANS()
        {
            var ret = new ActivityI();
            ret.done_ka = (ac) => { return false; };
            //ret.done_ka = w;
            //ret.activate_bk = r;
            //ret.update_bk = u;
            //if (r == null) ret.Name = "Waiter";
            return ret;

        }

        public static ActivityI R(this ActivityI ac, Action r)
        {
            //var ret = new ActivityI();
            ac.activate_bk = r;
            //ret.done_ka = w;
            //ret.activate_bk = r;
            //ret.update_bk = u;
            //if (r == null) ret.Name = "Waiter";
            return ac;

        }
        public static ActivityI W(this ActivityI ac, Func<ActivityI, bool> w)
        {
            //if (frames != -1) ac.Total_Frames = frames;
             
            ac.done_ka = w;
         
            return ac;

        }

        public static ActivityI U(this ActivityI ac,  Action<ActivityI> u)
        {
            
            ac.update_bk = u;
         
            return ac;

        }

        public static ActivityI F(this ActivityI ac, Action finally_)
        {
          
            ac.deactivate_bk += finally_;
           
            return ac;

        }

       
        public static ActivityI Wait(Func<ActivityI, bool> w)
        {
            return  _A.A0().W( w);
        }

       
        




        public static Repeat_Until Repeat (this ActivityI activity)
        {
            
            var ret = new Repeat_Until();
            ret.inner = activity;
            
           
                ret.done_ka =
                    (ac)  =>  false;
            
            return ret;
        }
        public static Repeat_Until Until(this Repeat_Until activity,Func<int> times)
        {

            int a = -1;
            activity.activate_bk += () =>
               a = times();
            
            activity.inner.deactivate_bk += () => a--;



            activity.done_ka =
          (ac) =>   a <= 0;

            return activity;
        }
        public static Repeat_Until Until(this Repeat_Until activity, Func<bool> breaker)
        {
                    activity.done_ka = (ac)
                    => breaker();

            return activity;
        }



         public static Activity_Series Series( params ActivityI[] activities)
        {
            var ret = new Activity_Series();
            foreach (var ac in activities) ret.children.Add(ac);
           
            return ret;
        }
       
     



        public static Activity_Parallel Parallel(ACTP mode,  params ActivityI[] activities)
        {
            var ret = new Activity_Parallel(mode);
            foreach (var ac in activities) ret.children.Add(ac);

            return ret;
        }


        public static Activity_Switch Switch( Func<Activity_Switch, ActivityI> choice_cb, params ActivityI[] activities)
        {
            var ret = new Activity_Switch();
            foreach (var ac in activities) ret.children.Add(ac);
            ret.choice_cb = choice_cb;
            return ret;
        }





        public static Activity_Series Chain(this ActivityI arg, params ActivityI[] activities)
        {
            var series = arg as Activity_Series;
            //var ret = new Activity_Series();
            if (series != null)
                foreach (var ac in activities) series.children.Add(ac);
            else
            {
                series = new Activity_Series();
                series.children.Add(arg);
                foreach (var ac in activities) series.children.Add(ac);
            }
            return series;
        }

        //defaults to all
        public static Activity_Parallel Chain_Parallel(this ActivityI arg, params ActivityI[] activities)
        {
            //var ret = new Activity_Series();
            var par = arg as Activity_Parallel;
            //var ret = new Activity_Series();
            if (par != null)
                foreach (var ac in activities) par.children.Add(ac);
            else
            {
                par = new Activity_Parallel(ACTP.All);
                par.children.Add(arg);
                foreach (var ac in activities) par.children.Add(ac);
            }
            return par;
        }



        public static State As_State(this ActivityI ac, N_STM name = N_STM.Null)
        {
            var state = new State(); state.Name = name; state.activity = ac;

            return state;
        }





        //public static ActivityI Tween(TC tween)
        //{
            
        //    var ret = new Activity_Tween();

        //    //bool flag;

        //    // TODO_LONG synchronize frames
        //    ret.done_ka = (a) => { return ret.Frame >= tween.Frames; };
        //    ret.activate_bk = () => { 
        //        tween.Plan(); };
        //    ret.update_bk = (ac) =>
        //    {
        //        tween.Update();
        //    };
        //    return ret;

        //}


        
        //////DEPRECATED
        ////// REMINDER loops some activity as with while(true) 
        //////Reminder this ont works with series caus ein waits it messes up their wait condition!!!!!!!!!!!!!
        //////Reminder action -> activity -> series activity
        //////Cleared with actions you supply an update_cb
        //////with activities??????????? it doesn't make sense, shit will just happen in te series
        ////public static Activity_Series Loop_Until(this Activity_Series activity,Func<ActivityI, bool> break_condition)
        ////{ 
        ////  activity.done_pred = break_condition;
        ////    //REMINDER I'v removed this waits, you need to add them yourself
        ////  //Activity waiter = null;
        ////  //if (wait_trailing != 0)
        ////  //{
        ////  //    waiter = _A.R_W(() => { }, wait_trailing);
        ////  //    waiter.Name = "Waiting";
        ////  //    activity.children.Add(waiter);
        ////  //}
            
        ////  //piso activity.loop = 0;
        ////  return activity;
        ////}

      


        //////DEPRECATED!!!!!!!!!!!!!
        //////idea maybe use the this to avoid wrapping stuff in a series
        //////public static Activity_Chain Loop_Until(this ActivityI activity_inner, Func<ActivityI, bool> break_condition, int wait_trailing = 0)
        //////{
        //////    var activity = _A.Series(0, activity_inner);
        //////    activity.done_pred = break_condition;
        //////    Activity waiter = null;
        //////    if (wait_trailing != 0)
        //////    {
        //////        waiter = _A.R_W(() => { }, wait_trailing);
        //////        waiter.Name = "Waiting";
        //////        activity.children.Add(waiter);
        //////    }

        //////    activity.loop = 0;
        //////    return activity;
        //////}
       
        
        ////// iDEA you could actuall add waits at the end of an activity instead of putting them here?? 
        //////Reminder you coukld add the wait back and get one less wrap n laying out
        //////todo mipos anti na antigrafete to esoteriko na ektelite kai apla to do until na exei plithos steps?
        ////public static Activity_Series Repeat(this ActivityI activity,int times/*, int wait=0 */ )
        ////{
        ////    var ret = new Activity_Series();
        ////    //Activity waiter = null;
        ////    //if (wait != 0)
        ////    //{
        ////    //    waiter = _A.R_W(() => { }, wait);
        ////    //    waiter.Name = "Waiting";
        ////    //}
        ////    for (int i = 0; i < times; i++)
        ////    {
        ////        ret.children.Add(activity);
        ////        //if ((waiter != null) && ( i != times - 1))
        ////        //{
        ////        //    ret.children.Add(waiter);
        ////        //}
        ////    }
          
        ////    return ret;
        ////}


        // sthn lista mpenei HIGHJACK CONDITION | ACCTIVITY
        //public static Activity_Pri Priority(params object[] activities)
        //{



        //    // REMINDER?: is the done predicate always null? Answer: you might want the primary path to end when it achieves its goal
        //    var ret = new Activity_Pri();
        //    //foreach (var ac in activities) ret.children.Add(ac);
        //    for (var i = 0; i < activities.Length / 2; i++)
        //    {
        //        var activity = activities[2 * i + 1] as ActivityI;
        //        var highjacker = activities[2 * i] as Func<ActivityI, bool>;
        //        activity.highjack_pred = highjacker;
        //        ret.children.Add(activity);
        //    }
        //    ret.current_set(ret.children.Count - 1);
        //    return ret;
        //}
        //public static Repeat_Until Loop(this ActivityI activity,
        //   int times = Int32.MaxValue,
        //   Func<bool> breaker = null)
        //{
        //    var a = times;
        //    var ret = new Repeat_Until();
        //    ret.inner = activity;
        //    ret.activate_bk += () =>
        //        a = times;
        //    if (breaker == null)
        //        ret.done_ka =
        //            (ac)
        //                =>
        //                a < 0;
        //    else
        //    {
        //        ret.done_ka =
        //        (ac)
        //            =>
        //            breaker() || a < 0;
        //    }
        //    activity.deactivate_bk += () => a--;
        //    return ret;
        //}

      
    }


 



}
