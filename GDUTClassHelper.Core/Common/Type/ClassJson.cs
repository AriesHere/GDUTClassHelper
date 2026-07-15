using System;
using System.Collections.Generic;
using System.Text;
using GDUTClassHelper.Core.Common.Helper;

namespace GDUTClassHelper.Core.Common.Type
{
    public class ClassJsonWithHeader
    {
        public int total { get; set; } = 0;
        public List<ClassJson> rows { get; set; } = [];
    }

    public class ClassJson
    {
        public string dgksdm { get; set; } = string.Empty;  // I don't know what's this. It seems te be the index of the class.

        /// <summary><see cref="Class.ClassName"/></summary>
        public string jxbmc { get; set; } = string.Empty;

        /// <summary><see cref="Class.StudentCount"/></summary>
        public string pkrs { get; set; } = string.Empty;

        /// <summary><see cref="Class.Name"/></summary>
        public string kcmc { get; set; } = string.Empty;

        /// <summary><see cref="Class.Teacher"/></summary>
        public string teaxms { get; set; } = string.Empty;

        /// <summary><see cref="Class.DayOfWeek"/></summary>
        public string xq { get; set; } = string.Empty;

        /// <summary><see cref="Class.Periods"/></summary>
        public string jcdm { get; set; } = string.Empty;

        /// <summary><see cref="Class.Location"/></summary>
        public string jxcdmc { get; set; } = string.Empty;

        /// <summary><see cref="Class.Week"/></summary>
        public string zc { get; set; } = string.Empty;

        /// <summary><see cref="Class.ClassSequence"/></summary>
        public string kxh { get; set; } = string.Empty;

        /// <summary><see cref="Class.ClassType"/></summary>
        public string jxhjmc { get; set; } = string.Empty;

        public string flfzmc { get; set; } = string.Empty;  // What's this?

        /// <summary><see cref="Class.Profile"/></summary>
        public string sknrjj { get; set; } = string.Empty;

        /// <summary><see cref="Class.Date"/></summary>
        public DateOnly pkrq { get; set; }

        /// <remarks>Just ignore it</remarks>
        public string rownum_ { get; set; } = string.Empty;

        public static implicit operator Class(ClassJson j)
        {
            var c = new Class()
            {
                ClassName = j.jxbmc,
                StudentCount = int.Parse(j.pkrs),
                Name = j.kcmc,
                Teacher = j.teaxms,
                DayOfWeek = int.Parse(j.xq),
                Periods = StringHelper.SplitByCharacterCount(j.jcdm, 2),
                Location = j.jxcdmc,
                Week = int.Parse(j.zc),
                ClassSequence = int.Parse(j.kxh),
                ClassType = j.jxcdmc,
                Profile = j.sknrjj,
                Date = j.pkrq,
            };
            return c;
        }
    }
}
