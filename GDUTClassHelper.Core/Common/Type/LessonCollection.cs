using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using GDUTClassHelper.Core.Common.Helper;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace GDUTClassHelper.Core.Common.Type
{
    public class LessonCollection : ICollection<Lesson>
    {
        #region ICollection

        public int Count => Lessons.Count;
        public bool IsReadOnly => false;
        
        public void Add(Lesson item)
        {
            if (!IsReadOnly)
            {
                int index = Lessons.BinarySearch(item, _comparer);
                if (index < 0) index = ~index;
                Lessons.Insert(index, item);
            }
        }

        public bool Remove(Lesson item) => throw new NotSupportedException();
        public void Update(Lesson item) => throw new NotSupportedException();
        public void Clear() => Lessons.Clear();
        public bool Contains(Lesson item) => Lessons.BinarySearch(item, _comparer) >= 0;
        public void CopyTo(Lesson[] array, int arrayIndex) => Lessons.CopyTo(array, arrayIndex);

        public IEnumerator<Lesson> GetEnumerator() => Lessons.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private readonly IComparer<Lesson> _comparer = Comparer<Lesson>.Create((a, b) =>
        {
            int cmp = a.Week.CompareTo(b.Week);
            if (cmp != 0) return cmp;
            return a.DayOfWeek.CompareTo(b.DayOfWeek);
        });

        public List<Lesson> Lessons { get; set; } = [];

        public Lesson this[int index] => Lessons[index];

        /// <summary>
        /// If Total > 0, <see cref="GetReadFlag"/> return not null value.
        /// </summary>
        public int Total = 0;
        public Status Status = Status.Indeterminate;
        public DateOnly FirstDate = DateOnly.MinValue;

        #region Read

        public static LessonCollection ReadFromText(string path)
        {
            using TextReader reader = new StreamReader(path);
            LessonCollection collection = [];
            bool flag = true; 
            reader.ReadLine();  // Skip header line
            while (reader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                List<string> s = StringHelper.ExtractQuotedStrings(line);
                Lesson newClass = new()
                {
                    Name = WebUtility.HtmlDecode(s[0]),
                    ClassName = WebUtility.HtmlDecode(s[1]),
                    StudentCount = int.Parse(s[2]),
                    Teacher = WebUtility.HtmlDecode(s[3]),
                    Week = int.Parse(s[4]),
                    DayOfWeek = int.Parse(s[5]),
                    Sessions = StringHelper.SplitByCharacterCount(s[6], 2),
                    Location = WebUtility.HtmlDecode(s[7]),
                    Date = DateOnly.Parse(s[8]),
                    LessonSequence = int.Parse(s[9]),
                    LessonType = WebUtility.HtmlDecode(s[10]),
                    Profile = WebUtility.HtmlDecode(s[11]),
                };
                if (flag)
                {
                    if (newClass.Date != DateOnly.MinValue 
                        && newClass.Week > 0
                        && newClass.DayOfWeek > 0)
                    {
                        DateOnly firstDate = newClass.Date;
                        int day = (newClass.Week - 1) * 7 + (newClass.DayOfWeek - 1);
                        firstDate.AddDays(-day);
                        collection.FirstDate = firstDate;
                        flag = false;
                    }
                }
                collection.Add(newClass);
            }
            return collection;
        }

        public static LessonCollection ReadFromJsonWithHeader(string jsonString)
        {
            LessonCollection lessons = [];
            LessonJsonWithHeader? json;
            json = JsonSerializer.Deserialize(jsonString, GlobalJsonContext.Context.LessonJsonWithHeader);
            if (json is not null)
            {
                foreach (var item in json.rows) lessons.Add(item);
                lessons.Total = json.total;
                lessons.Status = (lessons.Count == json.total) ? Status.Complete : Status.Incomplete;
            }
            return lessons;
        }

        public static LessonCollection ReadFromJson(string jsonString)
        {
            LessonCollection lessons = [];
            List<LessonJson>? json;
            json = JsonSerializer.Deserialize(jsonString, GlobalJsonContext.Context.ListLessonJson);
            if (json is not null)
            {
                foreach (var item in json) lessons.Add(item);
            }
            return lessons;
        }

        #endregion

        public void Save(string path)
        {
            using FileStream f = new(path, FileMode.Create);
            using BinaryWriter w = new(f);
            w.Write((int)this.Status);
            w.Write(this.Total);
            w.Write(this.FirstDate.DayNumber);
            using var stream = new MemoryStream();
            JsonSerializer.Serialize(stream, this.Lessons, GlobalJsonContext.Context.ListLesson);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var lessonJson = reader.ReadToEnd();
            w.Write(lessonJson);
        }

        public static LessonCollection Load(string path)
        {
            LessonCollection l = [];
            using FileStream f = new(path, FileMode.Open);
            using BinaryReader w = new(f);
            l.Status = (Status)w.ReadInt32();
            l.Total = w.ReadInt32();
            l.FirstDate = DateOnly.FromDayNumber(w.ReadInt32());
            l.Lessons = JsonSerializer.Deserialize(w.ReadString(), GlobalJsonContext.Context.ListLesson)!;
            return l;
        }

        public static Status GetStatusFromFile(string path)
        {
            using FileStream f = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, sizeof(int));
            using BinaryReader w = new(f);
            return (Status)w.ReadInt32();
        }

        public BitArray? GetReadFlag()
        {
            BitArray br = new(this.Total, false);
            foreach (var item in this)
            {
                if (item.Number == -1) return null;
                else br[item.Number - 1] = true;
            }
            return br;
        }
    }

    public static class LessonCollectionExtension
    {
        /// <summary>Get intersection of the two LessonCollection. If <paramref name="l1"/> is larger than <paramref name="l2"/>, this method has better performance</summary>
        public static List<(Lesson Source, Lesson Target)> CompareTo(this LessonCollection l1, LessonCollection l2, bool forceCompare = false)
        {
            List<(Lesson Source, Lesson Target)> result = [];
            if (l1.FirstDate != l2.FirstDate && !forceCompare)
            {
                return result;
            }
            int cur = 0;
            int today = 0;
            foreach (var item2 in l2)
            {
                for (; cur < l1.Count; cur++)
                {
                    if (l1[cur].Week == item2.Week)
                    {
                        today = cur;
                        if (l1[cur].Sessions.Intersect(item2.Sessions).Any())
                        {
                            result.Add(new(l1[cur], item2));
                            break;
                        }
                    }
                    else if (l1[cur].Week > item2.Week)
                    {
                        cur = today;
                        break;
                    }
                }
            }
            return result;
        }

        public static void ExportAsIcalendarJournal(this LessonCollection collection, SessionCollection sc, string path)
        {
            Calendar calendar = new();
            Alarm alarm = new()
            {
                Summary = "Lesson",  // TODO: Globalization
                Description = "Lesson starts in 15 minutes",
                Action = AlarmAction.Display,
                Trigger = new Trigger(new Duration(minutes: 15)),   // TODO: Customizable
            };
            foreach (var item in collection)
            {
                foreach (var period in item.Sessions)
                {
                    CalendarEvent e = new()
                    {
                        Summary = item.Name,
                        Description = item.Profile,
                        Organizer = new(item.Teacher),
                        Categories = [item.LessonType],
                        // TODO
                        Start = new(new DateTime(item.Date, sc.Sessions[period].StartTime)),
                        End = new(new DateTime(item.Date, sc.Sessions[period].EndTime)),
                    };
                    e.Alarms.Add(alarm);
                    calendar.AddChild(e);
                }
            }
        }
    }

    public enum Status
    {
        Indeterminate,
        Incomplete,
        Complete,
    }
}
