using MyStudentApi.Models;

namespace MyStudentApi.Helpers
{
    public static class AssignmentUtils
    {
        public static double CalculateCompensation(StudentClassAssignment a)
        {
            int h = a.WeeklyHours ?? 0;
            string position = (a.Position ?? "").Trim();
            string level = (a.EducationLevel ?? "").Trim().ToUpper();
            string fellow = (a.FultonFellow ?? "").Trim();
            string session = (a.ClassSession ?? "").Trim().ToUpper();

            // --- Grader ---
            if (position == "Grader" && (level == "MS" || level == "PHD") && fellow == "No")
            {
                if (h == 5)
                {
                    if (session == "A" || session == "B") return 781;
                    if (session == "C") return 1562;
                }
                if (h == 10)
                {
                    if (session == "A" || session == "B") return 1562;
                    if (session == "C") return 3124;
                }
                if (h == 15)
                {
                    if (session == "A" || session == "B") return 2343;
                    if (session == "C") return 4686;
                }
                if (h == 20)
                {
                    if (session == "A" || session == "B") return 3124;
                    if (session == "C") return 6248;
                }
            }

            // --- TA (GSA) 1 credit ---
            if (position == "TA (GSA) 1 credit" && level == "PHD" && fellow == "No")
            {
                if (h == 10 && (session == "A" || session == "B" || session == "C")) return 7552.5;
                if (h == 20 && (session == "A" || session == "B" || session == "C")) return 16825;
            }

            // --- TA ---
            if (position == "TA")
            {
                if (h == 20 && level == "PHD" && fellow == "Yes" && (session == "A" || session == "B" || session == "C")) return 13461.15;
                if (h == 10 && level == "MS" && fellow == "No" && (session == "A" || session == "B" || session == "C")) return 6636;
                if (h == 10 && level == "PHD" && fellow == "No" && (session == "A" || session == "B" || session == "C")) return 7250;
                if (h == 20 && level == "MS" && fellow == "No" && (session == "A" || session == "B" || session == "C")) return 13272;
                if (h == 20 && level == "PHD" && fellow == "No" && (session == "A" || session == "B" || session == "C")) return 14500;
            }

            // --- IA ---
            if (position == "IA" && (level == "MS" || level == "PHD") && fellow == "No")
            {
                if (h == 5)
                {
                    if (session == "A" || session == "B") return 1100;
                    if (session == "C") return 2200;
                }
                if (h == 10)
                {
                    if (session == "A" || session == "B") return 2200;
                    if (session == "C") return 4400;
                }
                if (h == 15)
                {
                    if (session == "A" || session == "B") return 2640;
                    if (session == "C") return 6600;
                }
                if (h == 20)
                {
                    if (session == "A" || session == "B") return 4400;
                    if (session == "C") return 8800;
                }
            }

            return 0;
        }

        // Infer AcadCareer from CatalogNum
        public static string InferAcadCareer(StudentClassAssignment a)
        {
            return (a.CatalogNum >= 100 && a.CatalogNum <= 499) ? "UGRD" : "GRAD";
        }

        // Cost Center Logic
        public static string ComputeCostCenterKey(StudentClassAssignment a)
        {
            // AcadCareer is inferred on the fly for matching rules
            string acadCareer = a.AcadCareer;
            if (string.IsNullOrWhiteSpace(acadCareer))
                acadCareer = InferAcadCareer(a);

            var match = CostCenterRules.FirstOrDefault(rule =>
                rule.Position == a.Position &&
                rule.Location?.ToUpper() == a.Location?.ToUpper() &&
                rule.Campus?.ToUpper() == a.Campus?.ToUpper() &&
                rule.AcadCareer?.ToUpper() == acadCareer?.ToUpper()
            );

            return match?.CostCenterKey ?? "UNKNOWN";
        }

        private record CostCenterRule(string Position, string Location, string Campus, string AcadCareer, string CostCenterKey);

        private static readonly List<CostCenterRule> CostCenterRules = new()
        {
            // === TA (GSA) 1 credit ===
            new("TA (GSA) 1 credit", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG02202"),
            new("TA (GSA) 1 credit", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG06875"),
            new("TA (GSA) 1 credit", "POLY",      "POLY",  "UGRD", "CC0136/PG02202"),
            new("TA (GSA) 1 credit", "POLY",      "POLY",  "GRAD", "CC0136/PG06875"),
            new("TA (GSA) 1 credit", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("TA (GSA) 1 credit", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("TA (GSA) 1 credit", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),

            // === TA ===
            new("TA", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG02202"),
            new("TA", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG06875"),
            new("TA", "POLY",      "POLY",  "UGRD", "CC0136/PG02202"),
            new("TA", "POLY",      "POLY",  "GRAD", "CC0136/PG06875"),
            new("TA", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("TA", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("TA", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),

            // === IOR ===
            new("IOR", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG02202"),
            new("IOR", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG06875"),
            new("IOR", "POLY",      "POLY",  "UGRD", "CC0136/PG02202"),
            new("IOR", "POLY",      "POLY",  "GRAD", "CC0136/PG06875"),
            new("IOR", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("IOR", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("IOR", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),

            // === Grader ===
            new("Grader", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG14700"),
            new("Grader", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG14700"),
            new("Grader", "POLY",      "POLY",  "UGRD", "CC0136/PG14700"),
            new("Grader", "POLY",      "POLY",  "GRAD", "CC0136/PG14700"),
            new("Grader", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("Grader", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("Grader", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),

            // === IA ===
            new("IA", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG15818"),
            new("IA", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG15818"),
            new("IA", "POLY",      "POLY",  "UGRD", "CC0136/PG15818"),
            new("IA", "POLY",      "POLY",  "GRAD", "CC0136/PG15818"),
            new("IA", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("IA", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG01943"),
            new("IA", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),
            new("IA", "ICOURSE",   "POLY",  "GRAD", "CC0136/PG02003")
        };
    }
}
