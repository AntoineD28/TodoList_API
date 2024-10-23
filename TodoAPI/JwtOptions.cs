namespace TodoAPI
{
    public record class JwtOptions(
        string Issuer,
        string Audience,
        string SecretKey,
        int ExpirationSeconds
    );
}
