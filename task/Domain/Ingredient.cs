namespace RecipesApp.Core.Domain
{
    public struct Quantity
    {
        public double Value { get; set; }
        public MeasurementUnit Unit { get; set; }

        public Quantity(double value, MeasurementUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString()
        {
            return Value.ToString() + " " + Unit.ToString();
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public Quantity Amount { get; set; }

        public Ingredient(string name, Quantity amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
