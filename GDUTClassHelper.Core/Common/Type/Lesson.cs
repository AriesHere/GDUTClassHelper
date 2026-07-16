namespace GDUTClassHelper.Core.Common.Type;

public struct Lesson()
{
    public string Name = string.Empty;
    public string ClassName = string.Empty;   // TODO: List<string>, Multiple lessons
    public int StudentCount = 0;
    public string Teacher = string.Empty;
    public int Week;
    public int DayOfWeek;
    public List<int> Sessions = [];
    public string Location = string.Empty;
    public DateOnly Date;
    /// <summary>Calculated through <see cref="LessonCollection.FirstDate"/></summary>
    public DateOnly ActualDate;
    public int LessonSequence;
    public string LessonType = string.Empty;
    public string Profile = string.Empty;

    public override readonly string ToString()
    {
        return $"""
            Lesson:
              - Name:{Name}
              - ClassName:{ClassName}
              - StudentCount:{StudentCount}
              - Teacher:{Teacher}
              - Week:{Week}
              - DayOfWeek:{DayOfWeek}
              - Sessions:{string.Join(",", Sessions)}
              - Location:{Location}
              - Date:{Date}
              - ActualDate:{ActualDate}
              - LessonSequence:{LessonSequence}
              - LessonType:{LessonType}
              - Profile:{Profile}
            """;
    }
}
