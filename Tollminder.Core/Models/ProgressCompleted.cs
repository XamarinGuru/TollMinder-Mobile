using System;
namespace Tollminder.Core.Models
{
    public class ProgressCompleted
    {
        public ProgressCompleted(double completed)
        {
            Completed = completed / 2;
        }
        public double Completed { get; private set; }
    }
}
