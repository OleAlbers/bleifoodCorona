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

        public Time(DateTime datetime)
        {
            Hour = datetime.Hour;
            Minute = datetime.Minute;
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

        public string AsString
        {
            get {
                return ToString();
            }
            set
            {
                var parts = value.Split(":");
                if (parts.Length != 2) return;
                if (int.TryParse(parts[0], out int hour) && int.TryParse(parts[1], out int minute) && hour>=0 && minute>=0 && hour<=24 && minute<=60)
                {
                    Hour = hour;
                    Minute = minute;
                }
            }
        }

        public DateTime AsDateTime
        {
            get
            {
                if (Hour == 0) return new DateTime(1900,1,1);
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Hour, Minute, 0);
            }
            set
            {
                if (value.Year==1900)
                {
                    Hour = 0;
                    Minute = 0;
                }
                else
                {
                    Hour = value.Hour;
                    Minute = value.Minute;
                }
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
