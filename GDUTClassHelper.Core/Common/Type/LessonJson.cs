using System.Net;
using GDUTClassHelper.Core.Common.Helper;

namespace GDUTClassHelper.Core.Common.Type
{
    public class LessonJsonWithHeader
    {
        public int total { get; set; } = 0;
        public List<LessonJson> rows { get; set; } = [];
    }

    public class LessonJson
    {
        /// <summary>The code of the classroom, just ignore it</summary>
        public string dgksdm { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.ClassName"/></summary>
        public string jxbmc { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.StudentCount"/></summary>
        public string pkrs { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.Name"/></summary>
        public string kcmc { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.Teacher"/></summary>
        public string teaxms { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.DayOfWeek"/></summary>
        public string xq { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.Sessions"/></summary>
        public string jcdm { get; set; } = string.Empty;

        /// <summary>Another type of <see cref="jcdm"/>. <see cref="Lesson.Sessions"/></summary>
        public string jcdm2 { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.Location"/></summary>
        public string jxcdmc { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.Week"/></summary>
        public string zc { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.LessonSequence"/></summary>
        public string kxh { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.LessonType"/></summary>
        public string jxhjmc { get; set; } = string.Empty;

        public string flfzmc { get; set; } = string.Empty;  // What's this?

        /// <summary><see cref="Lesson.Profile"/></summary>
        public string sknrjj { get; set; } = string.Empty;

        /// <summary><see cref="Lesson.Date"/></summary>
        public DateOnly pkrq { get; set; }

        /// <remarks><see cref="Lesson.Number"/></remarks>
        public string rownum_ { get; set; } = string.Empty;

        public static implicit operator Lesson(LessonJson j)
        {
            var lesson = new Lesson()
            {
                ClassName = WebUtility.HtmlDecode(j.jxbmc),
                StudentCount = int.Parse(j.pkrs),
                Name = WebUtility.HtmlDecode(j.kcmc),
                Teacher = WebUtility.HtmlDecode(j.teaxms),
                DayOfWeek = int.Parse(j.xq),
                Location = WebUtility.HtmlDecode(j.jxcdmc),
                Week = int.Parse(j.zc),
                LessonSequence = int.Parse(j.kxh),
                LessonType = WebUtility.HtmlDecode(j.jxcdmc),
                Profile = WebUtility.HtmlDecode(j.sknrjj),
                Date = j.pkrq,
            };
            if (string.IsNullOrWhiteSpace(j.jcdm))
            {
                var splitted = j.jcdm2.Split(',');
                foreach (var item in splitted)
                {
                    lesson.Sessions.Add(int.Parse(item));
                }
            }
            else
            {
                lesson.Sessions = StringHelper.SplitByCharacterCount(j.jcdm, 2);
            }
            if (j.rownum_ != "")
            {
                lesson.Number = int.Parse(j.rownum_);
            }
            return lesson;
        }
    }
}
