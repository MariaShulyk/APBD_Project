namespace ProjectAPBD.ResModels;

public class PaymentResModel
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ContractId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}