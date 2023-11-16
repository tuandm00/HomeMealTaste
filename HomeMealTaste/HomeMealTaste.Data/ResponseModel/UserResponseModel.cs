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
    }
}
