namespace Domain.Entities
{
    public class Motorcycle : BaseEntity
    {
        public int Year { get; private set; }
        public string Model { get; private set; } = default!;
        public string Plate { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Motorcycle(int year, string model, string plate)
        {
            if (year < 1980) throw new ArgumentException("Invalid year");
            Year = year; 
            Model = model.Trim(); 
            Plate = plate.Trim().ToUpperInvariant();
        }

        public void UpdatePlate(string plate) => Plate = plate.Trim().ToUpperInvariant();
    }
}
