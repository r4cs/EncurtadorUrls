namespace cp2.Entities;

public class EncurtadorUrl
{
    public Guid Id { get; set; }

    public string UrlLonga { get; set; } = String.Empty;
    
    public string UrlCurta { get; set; } = String.Empty;

    public string Codigo { get; set; } = String.Empty;
    
    public DateTime CriadoEm { get; set; }
}