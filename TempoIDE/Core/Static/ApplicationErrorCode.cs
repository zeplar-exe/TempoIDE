using System.Diagnostics.CodeAnalysis;

namespace TempoIDE.Core.Static
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ApplicationErrorCode
    {
        TI_FILE_NOMEM,
        TI_FILE_NO_ACCESS,
        TI_DIR_NO_ACCESS
    }
}