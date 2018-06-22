using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CarEngine
{
    class EngineStarter
    {
        public enum EngineState
        {
            Off,
            OnBattery,
            Igniting,
            EngineRunning
        }

        public event EventHandler<EngineState> EngineStateChanged;
        public EngineState State;

        public void StartStop()
        {
            if (State == EngineState.EngineRunning)
            {
                StopEngineAsync();
            }
            else
            {
                StartEngineAsync();
            }
        }

        async void StartEngineAsync()
        {
            Hibernate(false);
            while (State != EngineState.EngineRunning)
            {
                switch (State)
                {
                    case EngineState.Off:
                        await Task.Delay(500);
                        State = EngineState.OnBattery;
                        break;
                    case EngineState.OnBattery:
                        await Task.Delay(500);
                        State = EngineState.Igniting;
                        break;
                    case EngineState.Igniting:
                        await Task.Delay(700);
                        State = EngineState.EngineRunning;
                        break;
                    default:
                        break;
                }
                if (EngineStateChanged != null)
                {
                    EngineStateChanged.Invoke(this, State);
                }
            }
        }

        async void StopEngineAsync()
        {
            if (State == EngineState.EngineRunning)
            {
                await Task.Delay(200);
                State = EngineState.OnBattery;
                if (EngineStateChanged != null) EngineStateChanged.Invoke(this, State);
                Hibernate(true);
            }
        }

        //hibernation
        const int WaitTime = 10; //10sec
        int Countdown = WaitTime;
        bool ContinueOff;

        void Hibernate(bool continueOff)
        {
            ContinueOff = continueOff;
            if (continueOff == true)
            {
                CountDownForShutdownAsync();
            }
        }

        async void CountDownForShutdownAsync()
        {
            while (ContinueOff)
            {
                await Task.Delay(1000); //1sec
                Countdown--;
                if (Countdown == 0)
                {
                    Countdown = WaitTime;
                    ContinueOff = false;
                    State = EngineState.Off;
                    if (EngineStateChanged != null)
                    {
                        EngineStateChanged.Invoke(this, State);
                    }
                }
                Debug.Write(Countdown);
            }
            if (!ContinueOff) Countdown = WaitTime;
            return;
        }

        internal void Property(IDictionary<string, string> prop)
        {
            if (prop["start"] == "1")
            {
                StartStop();
            }
        }
    }
}
