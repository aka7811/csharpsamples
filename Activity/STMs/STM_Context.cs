using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GELib.Activity
{

    //todo_long use timestamps to avoid the full iteration to use minus?
    // how many accesses will there be in the transition lookup? each involves a comparison >=0 (Has, Grab)
    // with the other technique the comparison will be with the frame counter
    

    public class STM_context2
    {
        public int Frame = 0;
        public Dictionary<N_M_, int> messages;

        public STM_context2( )
        {

            messages = new Dictionary<N_M_, int>();
            
        }
        public void Message_Name_Add(params N_M_[] message_names)
        {
            foreach (var i in message_names)
            {
                messages[i] = -1;
            }
        }

        /// <summary>
        /// 0 sto pianei se afto, alla den prepei na exeis trexei to update prin erthei,
        /// gia sigouria dld thes 1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Frames"></param>

        public void Message(N_M_ name, int Frames = 0)
        {
            messages[name] = Frame + Frames;
        }
        public void Next_Frame()
        {
            //for (var i = 0; i < messages.Keys.Count;i++ )
            //{
            //    var key = messages.Keys.ElementAt(i);
            //    //var u = messages.Keys.e;
            //    messages[key]--;
            //}
            Frame++;
            //OTI MESSAGE STEILEIS META APO EDO ME FRAME 0 THA TREXEI ONTOS STO EPOMENO
            //ME FRAME 1 THA TREXEI TA DYO EPOMENA
            //AN STEILEIS ME FRAME 0 PRIN TO UPDATE
            // THA TREXEI EFOSON DEN KANEIS TO UPDATE PRIN TO FIRE KTL
            //OPOTE LOGIKA KANEIS UPDATE STM_CONTEXT STO TELOS TELOS
            //KAI STELNES TA 0 MINIMATA EX ARXHS KAI OXI STO ENDIAMESO? ALLIOS 1?

        }
        public bool Has(N_M_ message)
        {
            return messages[message] >= Frame;
        }

        // CLEARED : messages erased on access / deferred, mallon dld

        public bool Grab(N_M_ message)
        {
            var ret = messages[message] >= Frame;
            messages[message] = Frame - 1; //sidemark na glitoso apo eddo mia afiresh?
            return ret;
        }


        public Func<STMI, bool> tr_on_me(N_M_ message)
        {
            return (stm) => { return this.Has(message); };
        }
    }

    
    public class STM_context
    {
        public struct message_token 
        {
            public int count; public bool keep_next;
        }

        public int Frame = 0;
        public Dictionary<N_M_, message_token> messages;

        public STM_context()
        {

            messages = new Dictionary<N_M_, message_token>();

        }
        public void Message_Name_Add(params N_M_[] message_names)
        {
            foreach (var i in message_names)
            {
                messages[i] = new  message_token();
            }
        }

        /// <summary>
        /// 0 sto pianei se afto, alla den prepei na exeis trexei to update prin erthei,
        /// gia sigouria dld thes 1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Frames"></param>

        public void Message(N_M_ name )
        {
            messages[name] = new message_token() { count = messages[name].count + 1, keep_next = messages[name].keep_next };
        }
        public void Message_And_Next(N_M_ name)
        {
            messages[name] = new message_token() { count = messages[name].count + 1, keep_next = true };

        }
        public void End_Frame()
        {
            for (var i = 0; i < messages.Keys.Count; i++)
            {
                var key = messages.Keys.ElementAt(i);

                messages[key] = new message_token() { count = messages[key].keep_next?1:0, keep_next = false };
            }
            Frame++;
            //OTI MESSAGE STEILEIS META APO EDO ME FRAME 0 THA TREXEI ONTOS STO EPOMENO
            //ME FRAME 1 THA TREXEI TA DYO EPOMENA
            //AN STEILEIS ME FRAME 0 PRIN TO UPDATE
            // THA TREXEI EFOSON DEN KANEIS TO UPDATE PRIN TO FIRE KTL
            //OPOTE LOGIKA KANEIS UPDATE STM_CONTEXT STO TELOS TELOS
            //KAI STELNES TA 0 MINIMATA EX ARXHS KAI OXI STO ENDIAMESO? ALLIOS 1?

        }
        public bool Has(N_M_ message)
        {
            return messages[message].count >0;
        }

       

        public bool Grab(N_M_ message)
        {
            var ret = messages[message].count > 0;
            messages[message] = new message_token() { count =  0, keep_next = false }; 
            return ret;
        }


        public Func<STMI, bool> on(N_M_ message)
        {
            return (stm) => { return this.Has(message); };
        }
    }
    
}
