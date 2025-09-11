namespace Domain.Entities
{
    public class Rental : BaseEntity
    {
        public Guid MotorcycleId { get; private set; }
        public Guid CourierId { get; private set; }
        public int PlanDays { get; private set; } // 7,15,30,45,50
        public decimal DailyRate { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly ExpectedEndDate { get; private set; }
        public DateOnly? EndDate { get; private set; }
        public string Status { get; private set; } = "Created";
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Rental(Guid motorcycleId, Guid courierId, int planDays, decimal dailyRate, DateOnly startDate)
        {
            MotorcycleId = motorcycleId; 
            CourierId = courierId;
            PlanDays = planDays; 
            DailyRate = dailyRate;
            StartDate = startDate.AddDays(1);
            ExpectedEndDate = StartDate.AddDays(planDays);
        }

        public ReturnRentalResponse Close(DateOnly returnDate)
        {
            EndDate = returnDate; 
            Status = "Closed";

            int contracted = PlanDays;
            int effective = Math.Max(0, (returnDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1);

            if (returnDate < ExpectedEndDate)
            {
                int missing = contracted - effective;
                decimal pct = PlanDays switch { 7 => 0.20m, 15 => 0.40m, _ => 0m };
                decimal penalty = missing * DailyRate * pct;

                var result = new ReturnRentalResponse
                {
                    Total = (effective * DailyRate) + penalty,
                    EffectiveDays = effective,
                    ExtraDays = 0,
                    Penalty = penalty
                };

                return result;
            }

            if (returnDate > ExpectedEndDate)
            {
                int extra = (returnDate.ToDateTime(TimeOnly.MinValue) - ExpectedEndDate.ToDateTime(TimeOnly.MinValue)).Days;
                decimal total = (contracted * DailyRate) + (extra * 50m);

                var result = new ReturnRentalResponse
                {
                    Total = total,
                    EffectiveDays = contracted,
                    ExtraDays = extra,
                    Penalty = 0m
                };

                return result;
            }

            var contractedResult = new ReturnRentalResponse
            {
                Total = contracted * DailyRate,
                EffectiveDays = contracted,
                ExtraDays = 0,
                Penalty = 0m
            };

            return contractedResult;
        }

        public class ReturnRentalRequest
        {
            public DateOnly EndDate { get; set; }
        }

        public class ReturnRentalResponse
        {
            public decimal Total { get; set; }
            public int EffectiveDays { get; set; }
            public int ExtraDays { get; set; }
            public decimal Penalty { get; set; }
        }
    }
}
