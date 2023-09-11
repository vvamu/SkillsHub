using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models;

public class SendingMessage : Mailrequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    public string Data { get; set; }

    public string Date { get; set; } = System.DateTime.Now.ToString();

    [EmailAddress]
    public string Email { get; set; } = "-";


}
