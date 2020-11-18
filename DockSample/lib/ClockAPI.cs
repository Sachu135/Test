using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockSample.lib
{
    public class ClockAPI
    {
        public string abbreviation { get; set; }
        public string client_ip { get; set; }
        public string datetime { get; set; }
        public string day_of_week { get; set; }
        public string day_of_year { get; set; }
        public string dst { get; set; }
        public string dst_from { get; set; }
        public string dst_offset { get; set; }
        public string dst_until { get; set; }
        public string raw_offset { get; set; }
        public string timezone { get; set; }
        public string unixtime { get; set; }
        public string utc_datetime { get; set; }
        public string utc_offset { get; set; }
        public string week_number { get; set; }
    }

    public class WorldClockAPI
    {
        public string id { get; set; }
        public string currentDateTime { get; set; }
        public string utcOffset { get; set; }
        public string isDayLightSavingsTime { get; set; }
        public string dayOfTheWeek { get; set; }
        public string timeZoneName { get; set; }
        public string currentFileTime { get; set; }
        public string ordinalDate { get; set; }
        public string serviceResponse { get; set; }
    }
}
