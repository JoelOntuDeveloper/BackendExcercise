using System.Globalization;
using Util.Constants;

namespace Util.General {
    public static class FormatDates {

        public static DateTime ConvertStringToDate(string dateString) {
            DateTime dateValue;
            if (DateTime.TryParseExact(dateString, Formats.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue)) {
                return dateValue;
            } else {
                throw new FormatException($"El formato de la fecha debe ser '{Formats.DATE_FORMAT}'.");
            }
        }

    }
}
