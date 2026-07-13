namespace GDUTClassHelper.Core.Common;

public struct Class()
{
    public string Name { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;   // TODO: List<string>, Multiple classes
    public int StudentCount { get; set; } = 0;
    public string Teacher { get; set; } = string.Empty;
    public int Week { get; set; }
    public int DayOfWeek { get; set; }
    public List<int> Periods { get; set; } = [];
    public string Location { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int ClassSequence { get; set; }
    public string ClassType { get; set; } = string.Empty;
    public string Profile { get; set; } = string.Empty;

    public override readonly string ToString()
    {
        return $"""
            Class:
              - Name:{Name}
              - ClassName:{ClassName}
              - StudentCount:{StudentCount}
              - Teacher:{Teacher}
              - Week:{Week}
              - DayOfWeek:{DayOfWeek}
              - Periods:{string.Join(",", Periods)}
              - Location:{Location}
              - Date:{Date}
              - ClassSequence:{ClassSequence}
              - ClassType:{ClassType}
              - Profile:{Profile}
            """;
    }
}
