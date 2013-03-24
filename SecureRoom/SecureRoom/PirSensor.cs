using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace PirSensorExample
{
    public delegate void PirTriggeredEventHandler(bool triggered, DateTime time);

    public class PirSensor
    {
        private InterruptPort sensor;

        public event PirTriggeredEventHandler SensorTriggered;

        public PirSensor(Cpu.Pin pinNumber)
        {
            sensor =
                new InterruptPort(
                    pinNumber,
                    false,
                    Port.ResistorMode.PullUp,
                    Port.InterruptMode.InterruptEdgeBoth);

            sensor.OnInterrupt +=
                new NativeEventHandler(
                    (data1, data2, time) =>
                    {
                        OnSensorTriggered(data1, data2, time);
                    }
            );

        }

        private void OnSensorTriggered(uint data1, uint data2, DateTime time)
        {
            var evt = SensorTriggered;
            if (evt != null)
                evt.Invoke(data2 == 1, time);
        }
    }
}
