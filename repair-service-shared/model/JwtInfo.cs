namespace repair.service.shared.model
{
    /// <summary>
    /// Class to maintain the Jwt token and the decoded information from the Http Authorization
    /// </summary>
    public class JwtInfo
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string Token { get; set; }
    }
}
