﻿namespace SkillsHub.Application.Helpers
{
    public static class LessonMath
    {
        public static int Mod(int cur, int next, int mod = 7)
        {
            if (cur > next) return mod - cur + next;
            return next - cur;
        }
    }
}
