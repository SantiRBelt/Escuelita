namespace Entregable_Universities.Models
{
    public class UserTokensModel
    {
        public String? Id { get; set; }
        public string? Token { get; set; }
        public string? userName { get; set; }
        public TimeSpan Validity { get; set; }
        public string? RefreshToken { get; set; }
        public string? EmailId { get; set; }
        public Guid GuidId { get; set; }
        public DateTime ExpiredTime { get; set; }

    }
}
