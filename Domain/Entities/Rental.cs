namespace Domain.Entities
{
    public class Rental : BaseEntity
    {
        public Guid MotorcycleId { get; private set; }
        public Motorcycle Motorcycle { get; private set; }
        public Guid CourierId { get; private set; }
        public Courier Courier { get; private set; }
        public int PlanDays { get; private set; } // 7,15,30,45,50
        public decimal DailyRate { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly ExpectedEndDate { get; private set; }
        public DateOnly? EndDate { get; private set; }
        public string Status { get; private set; } = "Created";
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        //public Rental(Guid motorcycleId, Guid courierId, int planDays, decimal dailyRate, DateOnly createdOn)
        //{
        //    MotorcycleId = motorcycleId; CourierId = courierId;
        //    PlanDays = planDays; DailyRate = dailyRate;
        //    StartDate = createdOn.AddDays(1);
        //    ExpectedEndDate = StartDate.AddDays(planDays);
        //}

        public (decimal total, int effectiveDays, int extraDays, decimal penalty) Close(DateOnly returnDate)
        {
            EndDate = returnDate; Status = "Closed";

            int contracted = PlanDays;
            int effective = Math.Max(0, (returnDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1);

            if (returnDate < ExpectedEndDate)
            {
                int missing = contracted - effective;
                decimal pct = PlanDays switch { 7 => 0.20m, 15 => 0.40m, _ => 0m };
                decimal penalty = missing * DailyRate * pct;
                return ((effective * DailyRate) + penalty, effective, 0, penalty);
            }

            if (returnDate > ExpectedEndDate)
            {
                int extra = (returnDate.ToDateTime(TimeOnly.MinValue) - ExpectedEndDate.ToDateTime(TimeOnly.MinValue)).Days;
                decimal total = (contracted * DailyRate) + (extra * 50m);
                return (total, contracted, extra, 0m);
            }

            return (contracted * DailyRate, contracted, 0, 0m);
        }
    }
}
