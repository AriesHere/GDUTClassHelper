using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using GDUTClassHelper.Core.Common.Type;
namespace GDUTClassHelper.Core.Common;
[JsonSerializable(typeof(Lesson))]
[JsonSerializable(typeof(LessonCollection))]
[JsonSerializable(typeof(LessonJson))]
[JsonSerializable(typeof(LessonJsonWithHeader))]
[JsonSerializable(typeof(List<Lesson>))]
[JsonSerializable(typeof(List<LessonJson>))]
public partial class GlobalJsonContext : JsonSerializerContext
{
    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(
            UnicodeRanges.BasicLatin,
            UnicodeRanges.CjkUnifiedIdeographs,
            UnicodeRanges.CjkSymbolsandPunctuation)
    };
    public static GlobalJsonContext Context { get; } = new GlobalJsonContext(DefaultOptions);
}