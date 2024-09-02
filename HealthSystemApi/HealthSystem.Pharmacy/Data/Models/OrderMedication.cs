using HealthSystem.Pharmacy.Data.Models;

public class OrderMedication
{
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int MedicationId { get; set; }
    public Medication? Medication { get; set; }

    public int Quantity { get; set; }
}