using System.Collections.Generic;
using System.Linq;

namespace Alfa
{
    public class ScheduleRating
    {
        public int OhodnoťRozvrh(List<string> rozvrh)
        {
            // Implementace ohodnocení rozvrhu pomocí bodů
            // Čím vyšší počet bodů, tím lepší rozvrh

            int body = 0;

            // Příklad ohodnocení - lze upravit podle potřeby
            body += HodnotíSeDen(rozvrh, "M");
            body += HodnotíSeDen(rozvrh, "PIS");
            // Další kritéria hodnocení

            return body;
        }

        private int HodnotíSeDen(List<string> rozvrh, string předmět)
        {
            // Implementace kritéria hodnocení pro konkrétní předmět
            // ...

            return rozvrh.Count(den => den == předmět);
        }
    }
}