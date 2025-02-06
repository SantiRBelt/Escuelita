namespace Entregable_Universities.Models
{
    public class JwtSettingsModel
    {
        public bool ValidateIssuerSigninKey { get; set; }
        public string IssuerSigninKey { get; set; }
        public bool ValidateIssuer { get; set; } = true;
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; } = true;
        public string ValidAudience { get; set; }

        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifeTime { get; set; }
    }
}
