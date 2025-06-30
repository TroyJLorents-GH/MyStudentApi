using MyStudentApi.Models;

namespace MyStudentApi.Helpers
{
    public static class AssignmentUtils
    {
        public static double CalculateCompensation(StudentClassAssignment a)
        {
            int h = a.WeeklyHours;
            if (a.Position == "TA")
            {
                if (h == 5 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 2200;
                if (h == 5 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 2800;
                if (h == 10 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 6636;
                if (h == 10 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 7250;
                if (h == 15 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 8500;
                if (h == 15 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 8950;
                if (h == 20 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 13272;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 14500;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 13272;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 13461.24;
                if (h == 5 && a.EducationLevel == "MS" && a.FultonFellow == "Yes") return 2500;
                if (h == 5 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 3200;
                if (h == 10 && a.EducationLevel == "MS" && a.FultonFellow == "Yes") return 6836;
                if (h == 10 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 7550;
                if (h == 15 && a.EducationLevel == "MS" && a.FultonFellow == "Yes") return 9000;
                if (h == 15 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 9550;
            }

            if (a.Position == "TA (GSA) 1 credit")
            {
                if (h == 10 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 7552.5;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 16825;
            }

            if (a.Position == "Grader")
            {
                if (h == 5 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 2200;
                if (h == 5 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 2800;
                if (h == 10 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 6636;
                if (h == 10 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 7250;
                if (h == 15 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 8500;
                if (h == 15 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 8950;
                if (h == 20 && a.EducationLevel == "MS" && a.FultonFellow == "No") return 13272;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 14500;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "No") return 13272;
                if (h == 20 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 13461.24;
                if (h == 5 && a.EducationLevel == "MS" && a.FultonFellow == "Yes") return 2500;
                if (h == 5 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 3200;
                if (h == 10 && a.EducationLevel == "MS" && a.FultonFellow == "Yes") return 6836;
                if (h == 10 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 7550;
                if (h == 15 && a.EducationLevel == "MS" && a.FultonFellow == "Yes") return 9000;
                if (h == 15 && a.EducationLevel == "PHD" && a.FultonFellow == "Yes") return 9550;
            }

            if (a.Position == "IA")
            {
                var baseFactor = a.ClassSession == "C" ? 2 : 1;
                if (a.EducationLevel == "MS" || a.EducationLevel == "PHD")
                    return baseFactor * 1100 * (h / 5);
            }

            return 0;
        }

        public static string ComputeCostCenterKey(StudentClassAssignment a)
        {
            var match = CostCenterRules.FirstOrDefault(rule =>
                rule.Position == a.Position &&
                rule.Location?.ToUpper() == a.Location?.ToUpper() &&
                rule.Campus?.ToUpper() == a.Campus?.ToUpper() &&
                rule.AcadCareer?.ToUpper() == a.AcadCareer?.ToUpper()
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
            new("TA (GSA) 1 credit", "ASUONLINE", "TEMPE", "UGRD", "CC0136/PG01943"),
            new("TA (GSA) 1 credit", "ASUONLINE", "TEMPE", "GRAD", "CC0136/PG06316"),
            new("TA (GSA) 1 credit", "ASUONLINE", "POLY",  "UGRD", "CC0136/PG02003"),
            new("TA (GSA) 1 credit", "ASUONLINE", "POLY",  "GRAD", "CC0137/PG01943"),
            new("TA (GSA) 1 credit", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("TA (GSA) 1 credit", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("TA (GSA) 1 credit", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),
            new("TA (GSA) 1 credit", "ICOURSE",   "POLY",  "GRAD", "CC0137/PG01943"),

            // === IA ===
            new("IA", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG15818"),
            new("IA", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG15818"),
            new("IA", "POLY",      "POLY",  "UGRD", "CC0136/PG15818"),
            new("IA", "POLY",      "POLY",  "GRAD", "CC0136/PG15818"),
            new("IA", "ASUONLINE", "TEMPE", "UGRD", "CC0136/PG01943"),
            new("IA", "ASUONLINE", "TEMPE", "GRAD", "CC0136/PG01943"),
            new("IA", "ASUONLINE", "POLY",  "UGRD", "CC0136/PG02003"),
            new("IA", "ASUONLINE", "POLY",  "GRAD", "CC0136/PG02003"),
            new("IA", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("IA", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG01943"),
            new("IA", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),
            new("IA", "ICOURSE",   "POLY",  "GRAD", "CC0136/PG02003"),

            // === Grader ===
            new("Grader", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG14700"),
            new("Grader", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG14700"),
            new("Grader", "POLY",      "POLY",  "UGRD", "CC0136/PG14700"),
            new("Grader", "POLY",      "POLY",  "GRAD", "CC0136/PG14700"),
            new("Grader", "ASUONLINE", "TEMPE", "UGRD", "CC0136/PG01943"),
            new("Grader", "ASUONLINE", "TEMPE", "GRAD", "CC0136/PG06316"),
            new("Grader", "ASUONLINE", "POLY",  "UGRD", "CC0136/PG02003"),
            new("Grader", "ASUONLINE", "POLY",  "GRAD", "CC0137/PG01943"),
            new("Grader", "ICOURSE",   "TEMPE", "UGRD", "CC0136/PG01943"),
            new("Grader", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("Grader", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),
            new("Grader", "ICOURSE",   "POLY",  "GRAD", "CC0137/PG01943"),

            // === TA ===
            new("TA", "TEMPE",     "TEMPE", "UGRD", "CC0136/PG02202"),
            new("TA", "TEMPE",     "TEMPE", "GRAD", "CC0136/PG06875"),
            new("TA", "POLY",      "POLY",  "UGRD", "CC0136/PG02202"),
            new("TA", "POLY",      "POLY",  "GRAD", "CC0136/PG06875"),
            new("TA", "ASUONLINE", "TEMPE", "UGRD", "CC0136/PG01943"),
            new("TA", "ASUONLINE", "TEMPE", "GRAD", "CC0136/PG06316"),
            new("TA", "ASUONLINE", "POLY",  "UGRD", "CC0136/PG02003"),
            new("TA", "ASUONLINE", "POLY",  "GRAD", "CC0136/PG02003"),
            new("TA", "ICOURSE",   "TEMPE", "UGRD", "CC0137/PG01943"),
            new("TA", "ICOURSE",   "TEMPE", "GRAD", "CC0136/PG06316"),
            new("TA", "ICOURSE",   "POLY",  "UGRD", "CC0136/PG02003"),
            new("TA", "ICOURSE",   "POLY",  "GRAD", "CC0137/PG01943")
        };
    }
}
