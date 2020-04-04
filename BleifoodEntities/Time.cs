using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Bleifood.Entities
{
    public class Time:IComparable<Time>
    {
        public int Hour { get; set; }
        public int Minute { get; set; }


        public Time(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }
        public Time (string time)
        {
            if (time.Contains(":"))
            {
                Hour = int.Parse(time.Split(":")[0]);
                Minute = int.Parse(time.Split(":")[1]);
            }
        }

        public Time()
        {
            Hour = 0;
            Minute = 0;
        }

        public override string ToString()
        {
            return $"{Hour:00}:{Minute:00}";
        }

        public int CompareTo([AllowNull] Time other)
        {
            return (Hour * 60 + Minute).CompareTo(other?.Hour * 60 + other?.Minute);
        }

        public DateTime AsDateTime
        {
            get
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Hour, Minute, 0);
            }
            set
            {
                Hour = value.Hour;
                Minute = value.Minute;
            }
        }

        public void AddMinutes(int value)
        {
            Minute += value;
            while (Minute>=60) {
                Hour++;
                Minute -= 60;
            }

            while (Minute<0)
            {
                Hour--;
                Minute += 60;
            }
        }

        
    }
}
