namespace Inventario.Application.DTOs
{
    public record LoginRequest(string Username, string Password);
    
    public record AuthResponse(
        Guid Id,
        string Username,
        string Token,
        string Role
    );
}
