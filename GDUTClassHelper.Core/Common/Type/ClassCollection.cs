using System;
using System.Collections;
using System.Net;
using GDUTClassHelper.Core.Common.Helper;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace GDUTClassHelper.Core.Common.Type
{
    public class ClassCollection : ICollection<Class>
    {
        #region ICollection

        public int Count => _classes.Count;
        public bool IsReadOnly => false;

        public void Add(Class item)
        {
            int index = _classes.BinarySearch(item, _comparer);
            if (index < 0) index = ~index;
            _classes.Insert(index, item);
        }

        public bool Remove(Class item)
        {
            int index = _classes.BinarySearch(item, _comparer);
            if (index < 0) return false;
            _classes.RemoveAt(index);
            return true;
        }

        public void Update(Class item)
        {
            int oldIndex = _classes.FindIndex(x => EqualityComparer<Class>.Default.Equals(x, item));
            if (oldIndex < 0)
                throw new ArgumentException("Item not found in collection.");
            _classes.RemoveAt(oldIndex);
            Add(item);
        }

        public void Clear() => _classes.Clear();
        public bool Contains(Class item) => _classes.BinarySearch(item, _comparer) >= 0;
        public void CopyTo(Class[] array, int arrayIndex) => _classes.CopyTo(array, arrayIndex);

        public IEnumerator<Class> GetEnumerator() => _classes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private readonly List<Class> _classes = [];
        private readonly IComparer<Class> _comparer = Comparer<Class>.Create((a1, a2) => a1.Date.CompareTo(a2.Date));

        public Class this[int index] => _classes[index];

        public static ClassCollection ReadFromText(string path)
        {
            using TextReader reader = new StreamReader(path);
            var collection = new ClassCollection();
            reader.ReadLine(); // Skip header line
            while (reader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                List<string> s = StringHelper.ExtractQuotedStrings(line);
                Class newClass = new()
                {
                    Name = WebUtility.HtmlDecode(s[0]),
                    ClassName = WebUtility.HtmlDecode(s[1]),
                    StudentCount = int.Parse(s[2]),
                    Teacher = WebUtility.HtmlDecode(s[3]),
                    Week = int.Parse(s[4]),
                    DayOfWeek = int.Parse(s[5]),
                    Periods = StringHelper.SplitByCharacterCount(s[6], 2),
                    Location = WebUtility.HtmlDecode(s[7]),
                    Date = DateOnly.Parse(s[8]),
                    ClassSequence = int.Parse(s[9]),
                    ClassType = WebUtility.HtmlDecode(s[10]),
                    Profile = WebUtility.HtmlDecode(s[11]),
                };
                collection.Add(newClass);
            }
            return collection;
        }
    }

    public static class ClassCollectionExtension
    {
        /// <summary>Get intersection of the two ClassCollection. If <paramref name="c1"/> is larger than <paramref name="c2"/>, this method has better performance</summary>
        public static List<(Class Source, Class Target)> CompareTo(this ClassCollection c1, ClassCollection c2)
        {
            List<(Class Source, Class Target)> result = [];
            int cur = 0;
            int today = 0;
            foreach (var item2 in c2)
            {
                for (; cur < c1.Count; cur++)
                {
                    if (c1[cur].Date == item2.Date)
                    {
                        today = cur;
                        if (c1[cur].Periods.Intersect(item2.Periods).Any())
                        {
                            result.Add(new(c1[cur], item2));
                            break;
                        }
                    }
                    else if (c1[cur].Date > item2.Date)
                    {
                        cur = today;
                        break;
                    }
                }
            }
            return result;
        }

        public static void ExportAsIcalendarJournal(this ClassCollection collection, PeriodCollection pc, string path)
        {
            Calendar calendar = new();
            Alarm alarm = new()
            {
                Summary = "Class",  // TODO: Globalization
                Description = "Class starts in 15 minutes",
                Action = AlarmAction.Display,
                Trigger = new Trigger(new Duration(minutes: 15)),   // TODO: Customizable
            };
            foreach (var item in collection)
            {
                foreach (var period in item.Periods)
                {
                    CalendarEvent e = new()
                    {
                        Summary = item.Name,
                        Description = item.Profile,
                        Organizer = new(item.Teacher),
                        Categories = [item.ClassType],
                        Start = new(new DateTime(item.Date, pc.Periods[period].StartTime)),
                        End = new(new DateTime(item.Date, pc.Periods[period].EndTime)),
                    };
                    e.Alarms.Add(alarm);
                    calendar.AddChild(e);
                }
            }
        }
    }
}
