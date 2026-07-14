namespace GDUTClassHelper.Core.Common.Type;

public struct Class()
{
    public string Name = string.Empty;
    public string ClassName = string.Empty;   // TODO: List<string>, Multiple classes
    public int StudentCount = 0;
    public string Teacher = string.Empty;
    public int Week;
    public int DayOfWeek;
    public List<int> Periods = [];
    public string Location = string.Empty;
    public DateOnly Date;
    public int ClassSequence;
    public string ClassType = string.Empty;
    public string Profile = string.Empty;

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
