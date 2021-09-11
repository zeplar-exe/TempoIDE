using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media;
using CsvHelper;
using CsvHelper.Configuration;

namespace TempoIDE.Core.Inspections
{
    public static class InspectionSeverityAssociator
    {
        public static InspectionSeverity FromCode(string code)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Comment = '#',
                AllowComments = true,
            };
            using var reader = new StreamReader("data/inspection_severity.csv");
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<SeverityPair>();
            
            return records.FirstOrDefault(r => r.Code == code)?.Severity ?? InspectionSeverity.None;
        }

        public static Brush BrushFromSeverity(InspectionSeverity severity)
        {
            return severity switch
            {
                InspectionSeverity.None => Brushes.Transparent,
                InspectionSeverity.Hint => Brushes.PaleGreen,
                InspectionSeverity.Suggestion => Brushes.DodgerBlue,
                InspectionSeverity.Warning => Brushes.Yellow,
                InspectionSeverity.Error => Brushes.OrangeRed,
                InspectionSeverity.Spelling => Brushes.ForestGreen,
                _ => Brushes.Transparent
            };
        }

        private class SeverityPair
        {
            public string Code { get; set; }
            public InspectionSeverity Severity { get; set; }
        }
    }
}