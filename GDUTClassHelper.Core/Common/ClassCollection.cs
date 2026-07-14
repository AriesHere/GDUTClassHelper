using System;
using System.Collections;
using System.Net;
using GDUTClassHelper.Core.Common.Helper;
using Ical.Net;
using Ical.Net.CalendarComponents;

namespace GDUTClassHelper.Core.Common
{
    public class ClassCollection : ICollection<Class>
    {
        private readonly List<Class> _classes = [];
        private readonly IComparer<Class> _comparer = Comparer<Class>.Create((a1, a2) => a1.Date.CompareTo(a2.Date));

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

        public Class this[int index] => _classes[index];

        public IEnumerator<Class> GetEnumerator() => _classes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

    // TODO
    public static class ClassCollectionExtension
    {
        public static void ExportAsIcalendarJournal(this ClassCollection collection, string path)
        {
            Calendar calendar = new();
            foreach (var item in collection)
            {
                Journal j = new()
                {
                    Name = item.Name,
                    Description = item.Profile,
                    Categories = [item.ClassType]
                };
            }
        }
    }
}
