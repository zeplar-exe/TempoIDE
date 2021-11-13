using System.Diagnostics.CodeAnalysis;

namespace TempoIDE.Core.Helpers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ApplicationErrorCode
    {
        TI_UNHANDLED,
        
        TI_FILE_NOMEM,
        TI_FILE_NO_ACCESS,
        TI_DIR_NO_ACCESS,
        
        TI_INVALID_SKIN,
        TI_INVALID_SKIN_NAME,
        TI_INVALID_PLUGIN,
        
        TI_INVALID_FILE,
        TI_INVALID_DIRECTORY,
    }
}