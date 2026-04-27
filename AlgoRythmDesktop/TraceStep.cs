using System;

namespace AlgoRythmDesktop
{
    public enum StepType { Compare, Swap, Found, Shift, Done }

    public class TraceStep
    {
        public int[] ArrayState { get; set; }   // снимок массива
        public int Index1 { get; set; }          // первый индекс
        public int Index2 { get; set; }          // второй индекс
        public StepType Type { get; set; }       // Compare/Swap/Found/Shift
        public string Description { get; set; } // текст для лога

        public TraceStep(int[] arrayState, int index1, int index2, StepType type, string description)
        {
            ArrayState = (int[])arrayState.Clone(); // Клонируем массив, чтобы сохранить его состояние на данном шаге
            Index1 = index1;
            Index2 = index2;
            Type = type;
            Description = description;
        }
    }
}
