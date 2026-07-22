using System.Text.Json.Serialization;

namespace GDUTClassHelper.Core.Common.Type;

public struct Lesson()
{
    [JsonInclude] public string Name = string.Empty;
    [JsonInclude] public string ClassName = string.Empty;   // TODO: List<string>, Multiple lessons
    [JsonInclude] public int StudentCount = 0;
    [JsonInclude] public string Teacher = string.Empty;
    [JsonInclude] public int Week;
    [JsonInclude] public int DayOfWeek;
    [JsonInclude] public List<int> Sessions = [];
    [JsonInclude] public string Location = string.Empty;
    [JsonInclude] public DateOnly Date;
    /// <summary>Calculated through <see cref="LessonCollection.FirstDate"/></summary>
    [JsonInclude] public DateOnly ActualDate;
    [JsonInclude] public int LessonSequence;
    [JsonInclude] public string LessonType = string.Empty;
    [JsonInclude] public string Profile = string.Empty;

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
