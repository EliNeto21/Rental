namespace Domain.Entities
{
    public class Courier : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Cnpj { get; private set; } = default!;
        public DateOnly BirthDate { get; private set; }
        public string CnhNumber { get; private set; } = default!;
        public string CnhType { get; private set; } = default!;
        public string? CnhImageUrl { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public bool CanRentMotorcycle() => CnhType.ToUpper() is "A" or "A+B";

        public void SetCnhImage(string url) => CnhImageUrl = url;

        public Courier(DateOnly birthDate, string name, string cnhNumber, string cnhType, string cnpj)
        {
            Name = name;
            Cnpj = cnpj;
            BirthDate = birthDate;
            CnhNumber = cnhNumber;
            CnhType = cnhType;
        }
    }
}
