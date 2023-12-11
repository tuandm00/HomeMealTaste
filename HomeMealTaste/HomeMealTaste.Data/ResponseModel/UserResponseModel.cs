namespace HomeMealTaste.Data.ResponseModel
{
    public class UserResponseModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }
        public bool? Status { get; set; }
        public string Token { get; set; }
        public int? CustomerId { get; set; }
        public int? KitchenId { get; set; }
        public WalletDtoResponse? WalletDtoResponse { get; set; }
    }
    public class WalletDtoResponse
    {
        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }
    }

   
}
