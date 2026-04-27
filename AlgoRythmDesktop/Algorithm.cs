using System.Collections.Generic;

namespace AlgoRythmDesktop
{
    public abstract class Algorithm
    {
        public string Name { get; set; }
        public string Complexity { get; set; }
        public string Hint { get; set; }
        public int SearchValue { get; set; } // Добавляем свойство для значения поиска

        public abstract List<TraceStep> GetTrace(int[] array);
    }
}
