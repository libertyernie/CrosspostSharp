using System;

namespace InkbunnyLib {
    public class InkbunnyRatings {
        public bool Nudity { get; set; }
        public bool Violence { get; set; }
        public bool SexualThemes { get; set; }
        public bool StrongViolence { get; set; }

        public bool Any => Nudity || Violence || SexualThemes || StrongViolence;

        internal string this[int index] {
            get {
                if (index == 2) return Nudity ? "yes" : "no";
                if (index == 3) return Violence ? "yes" : "no";
                if (index == 4) return SexualThemes ? "yes" : "no";
                if (index == 5) return StrongViolence ? "yes" : "no";
                throw new ArgumentException("Value must be in 2,3,4,5");
            }
        }
    }
}