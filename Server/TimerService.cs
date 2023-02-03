using Common;
using SecurityManager;
using System.Timers;
using System;
using System.Threading;
using System.ServiceModel;
using System.Security.Permissions;

namespace Server
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class TimerService : IServiceContracts
    {
        private static System.Timers.Timer _timer = new System.Timers.Timer();
        private static int milisecondsLeft = 0;
        [PrincipalPermission(SecurityAction.Demand, Role = "Change")]
        public void CancelTimer()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formatter.ParseName(principal.Identity.Name);

            if (milisecondsLeft > 0)
            {
                _timer.Stop();
                milisecondsLeft = 0;
                try
                {
                    Audit.ActionSuccesful(username, "CancelTimer");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            else
            {
                try
                {
                    Audit.ActionFailed(username, "CancelTimer");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                throw new FaultException("Timer is already at 0.");

            }

        }

        [PrincipalPermission(SecurityAction.Demand, Role = "See")]
        public int ReadTimer()
        {
            return milisecondsLeft;

        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Change")]
        public void SetTimer(string miliseconds)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formatter.ParseName(principal.Identity.Name);
            int input;
            bool correctInput = Int32.TryParse(EncryptionManager.DecryptMessage(miliseconds), out input);
            if (correctInput)
            {
                milisecondsLeft = input;
                try
                {
                    Audit.ActionSuccesful(username, "SetTimerTime");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            }
            else
            {
                try
                {
                    Audit.ActionFailed(username, "SetTimerTime");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
      
                throw new FaultException("Wrong input, please try again.");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "StartStop")]
        public void StartTimer()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formatter.ParseName(principal.Identity.Name);
            if (milisecondsLeft > 0)
            {
                _timer.Enabled = true;
                _timer.Start();
                try
                {
                    Audit.ActionSuccesful(username, "StartTimer");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
               
            }
            else
            {
                try
                {
                    Audit.ActionFailed(username, "StartTimer");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                throw new FaultException("Timer has elapsed, input a time to start it again.");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "StartStop")]
        public void StopTimer()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formatter.ParseName(principal.Identity.Name);
            if (milisecondsLeft > 0)
            {
                _timer.Stop();
                try
                {
                    Audit.ActionSuccesful(username, "StopTimer");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            else
            {
                try
                {
                    Audit.ActionFailed(username, "StopTimer");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                throw new FaultException("Timer has elapsed, cannot be stopped.");
            }
        }


        private static void HandleEvent(object sender, EventArgs e)
        {
            milisecondsLeft -= 100;
            if(milisecondsLeft == 0)
            {
                _timer.Stop();
            }
        }
        
        public static void InitiateTimer()
        {
            _timer.Enabled = false;
            _timer.Elapsed += new ElapsedEventHandler(HandleEvent);
            _timer.Interval = 100;
        }
    }
}
